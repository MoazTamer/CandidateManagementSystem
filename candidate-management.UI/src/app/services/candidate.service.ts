import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CandidateDto, FileUploadResultDto, SkillDto } from '../modles/models';

@Injectable({
  providedIn: 'root'
})
export class CandidateService {
  private apiUrl = 'https://localhost:7215/api/Candidates';

  constructor(private http: HttpClient) { }

  getCandidates(): Observable<CandidateDto[]> {
    return this.http.get<CandidateDto[]>(this.apiUrl);
  }

  getCandidate(id: number): Observable<CandidateDto> {
    return this.http.get<CandidateDto>(`${this.apiUrl}/${id}`);
  }

  createCandidate(candidate: CandidateDto): Observable<CandidateDto> {
    return this.http.post<CandidateDto>(this.apiUrl, candidate);
  }

  updateCandidate(id: number, candidate: CandidateDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, candidate);
  }

  deleteCandidate(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  uploadFile(file: File): Observable<FileUploadResultDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<FileUploadResultDto>(`${this.apiUrl}/upload`, formData);
  }

  addSkill(candidateId: number, skill: SkillDto): Observable<SkillDto> {
    return this.http.post<SkillDto>(`${this.apiUrl}/${candidateId}/skills`, skill);
  }

  updateSkill(skillId: number, skill: SkillDto): Observable<any> {
    return this.http.put(`${this.apiUrl}/skills/${skillId}`, skill);
  }

  deleteSkill(skillId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/skills/${skillId}`);
  }
}