export interface CandidateDto {
  id?: number;
  name: string;
  nickname: string;
  email: string;
  yearsOfExperience: number;
  maxNumSkills?: number;
  skills?: SkillDto[];
}

export interface SkillDto {
  id?: number;
  candidateId?: number;
  name: string;
  gainDate: string; 
}

export interface FileUploadResultDto {
  totalRecords: number;
  loadedRecords: number;
  duplicatesIgnored: number;
  invalidEmailsIgnored: number;
  invalidNicknamesIgnored: number;
  errors?: string[];
}