import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CandidateService } from '../../services/candidate.service';
import { CandidateDto } from '../../modles/models';

@Component({
  selector: 'app-candidate-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './candidate-form.component.html',
  styleUrls: ['./candidate-form.component.css']
})
export class CandidateFormComponent implements OnInit {
  candidate: CandidateDto = {
    name: '',
    nickname: '',
    email: '',
    yearsOfExperience: 0,
    maxNumSkills: undefined
  };
  
  isEditMode = false;
  isLoading = false;
  errorMessage = '';
  candidateId: number | null = null;

  constructor(
    private candidateService: CandidateService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id && id !== 'new') {
      this.isEditMode = true;
      this.candidateId = +id;
      this.loadCandidate(this.candidateId);
    }
  }

  loadCandidate(id: number): void {
    this.isLoading = true;
    this.candidateService.getCandidate(id).subscribe({
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

  saveCandidate(): void {
    this.errorMessage = '';
    
    // Basic validation
    if (!this.candidate.name || !this.candidate.nickname || !this.candidate.email) {
      this.errorMessage = 'Please fill all required fields';
      return;
    }

    this.isLoading = true;

    if (this.isEditMode && this.candidateId) {
      this.candidateService.updateCandidate(this.candidateId, this.candidate).subscribe({
        next: () => {
          this.router.navigate(['/candidates']);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Error updating candidate';
          this.isLoading = false;
        }
      });
    } else {
      this.candidateService.createCandidate(this.candidate).subscribe({
        next: () => {
          this.router.navigate(['/candidates']);
        },
        error: (error) => {
          this.errorMessage = error.error?.message || 'Error creating candidate';
          this.isLoading = false;
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/candidates']);
  }
}