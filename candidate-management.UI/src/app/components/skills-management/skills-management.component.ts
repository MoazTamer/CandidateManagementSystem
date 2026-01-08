import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CandidateService } from '../../services/candidate.service';
import { CandidateDto, SkillDto } from '../../modles/models';

@Component({
  selector: 'app-skills-management',
  standalone: true,
  imports: [CommonModule, FormsModule, DatePipe],
  templateUrl: './skills-management.component.html',
  styleUrls: ['./skills-management.component.css']
})
export class SkillsManagementComponent implements OnInit {
  candidate: CandidateDto | null = null;
  candidateId: number = 0;
  
  newSkill: SkillDto = {
    name: '',
    gainDate: ''
  };
  
  editingSkill: SkillDto | null = null;
  isLoading = true;
  errorMessage = '';
  isAddingSkill = false;

  constructor(
    private candidateService: CandidateService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.candidateId = +id;
      this.loadCandidate();
    }
  }

  loadCandidate(): void {
    this.isLoading = true;
    this.candidateService.getCandidate(this.candidateId).subscribe({
      next: (data) => {
        this.candidate = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Error loading candidate';
        this.isLoading = false;
      }
    });
  }

  addSkill(): void {
    if (!this.newSkill.name || !this.newSkill.gainDate) {
      this.errorMessage = 'Please fill all skill fields';
      return;
    }

    // Check if skill name contains numbers
    if (/\d/.test(this.newSkill.name)) {
      this.errorMessage = 'Skill name should not contain numbers';
      return;
    }

    // Check max skills limit
    if (this.candidate?.maxNumSkills && 
        this.candidate.skills && 
        this.candidate.skills.length >= this.candidate.maxNumSkills) {
      this.errorMessage = `Cannot add more than ${this.candidate.maxNumSkills} skills`;
      return;
    }

    this.isAddingSkill = true;
    this.errorMessage = '';

    this.candidateService.addSkill(this.candidateId, this.newSkill).subscribe({
      next: () => {
        this.newSkill = { name: '', gainDate: '' };
        this.loadCandidate();
        this.isAddingSkill = false;
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Error adding skill';
        this.isAddingSkill = false;
      }
    });
  }

  startEditSkill(skill: SkillDto): void {
    this.editingSkill = { ...skill };
  }

  cancelEdit(): void {
    this.editingSkill = null;
  }

  updateSkill(): void {
    if (!this.editingSkill || !this.editingSkill.id) return;

    // Check if skill name contains numbers
    if (/\d/.test(this.editingSkill.name)) {
      this.errorMessage = 'Skill name should not contain numbers';
      return;
    }

    this.candidateService.updateSkill(this.editingSkill.id, this.editingSkill).subscribe({
      next: () => {
        this.editingSkill = null;
        this.loadCandidate();
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Error updating skill';
      }
    });
  }

  deleteSkill(skillId: number): void {
    if (confirm('Are you sure you want to delete this skill?')) {
      this.candidateService.deleteSkill(skillId).subscribe({
        next: () => {
          this.loadCandidate();
        },
        error: (error) => {
          this.errorMessage = 'Error deleting skill';
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/candidates']);
  }

  getMinDate(): string {
    if (!this.candidate) return '';
    const today = new Date();
    const minDate = new Date(today);
    minDate.setFullYear(today.getFullYear() - this.candidate.yearsOfExperience);
    return minDate.toISOString().split('T')[0];
  }

  getMaxDate(): string {
    return new Date().toISOString().split('T')[0];
  }

  hasSkills(): boolean {
    return this.candidate?.skills !== undefined && this.candidate?.skills !== null && this.candidate.skills.length > 0;
  }

  getSkills(): SkillDto[] {
    return this.candidate?.skills || [];
  }
}