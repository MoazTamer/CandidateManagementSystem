import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CandidateService } from '../../services/candidate.service';
import { CandidateDto } from '../../modles/models';

@Component({
  selector: 'app-candidates-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './candidates-list.component.html',
  styleUrls: ['./candidates-list.component.css']
})
export class CandidatesListComponent implements OnInit {
  candidates: CandidateDto[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private candidateService: CandidateService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCandidates();
  }

  loadCandidates(): void {
    this.isLoading = true;
    this.candidateService.getCandidates().subscribe({
      next: (data) => {
        this.candidates = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage = 'Error loading candidates';
        this.isLoading = false;
      }
    });
  }

  editCandidate(id: number): void {
    this.router.navigate(['/candidate', id]);
  }

  deleteCandidate(id: number): void {
    if (confirm('Are you sure you want to delete this candidate?')) {
      this.candidateService.deleteCandidate(id).subscribe({
        next: () => {
          this.loadCandidates();
        },
        error: (error) => {
          alert('Error deleting candidate');
        }
      });
    }
  }

  manageSkills(id: number): void {
    this.router.navigate(['/candidate', id, 'skills']);
  }

  addNewCandidate(): void {
    this.router.navigate(['/candidate', 'new']);
  }
}