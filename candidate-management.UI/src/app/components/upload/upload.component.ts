import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CandidateService } from '../../services/candidate.service';
import { FileUploadResultDto } from '../../modles/models';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {
  selectedFile: File | null = null;
  uploadResult: FileUploadResultDto | null = null;
  isUploading = false;
  errorMessage = '';

  constructor(private candidateService: CandidateService) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      // Check if it's a text file
      if (file.type === 'text/plain' || file.name.endsWith('.txt')) {
        this.selectedFile = file;
        this.errorMessage = '';
      } else {
        this.errorMessage = 'Please select a valid .txt file';
        this.selectedFile = null;
      }
    }
  }

  uploadFile(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please select a file first';
      return;
    }

    this.isUploading = true;
    this.errorMessage = '';
    this.uploadResult = null;

    this.candidateService.uploadFile(this.selectedFile).subscribe({
      next: (result) => {
        this.uploadResult = result;
        this.isUploading = false;
        this.selectedFile = null;
        // Reset file input
        const fileInput = document.getElementById('fileInput') as HTMLInputElement;
        if (fileInput) fileInput.value = '';
      },
      error: (error) => {
        this.errorMessage = error.error?.message || 'Error uploading file';
        this.isUploading = false;
      }
    });
  }
}