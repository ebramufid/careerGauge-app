import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import {
  AssessmentQuestion,
  AssessmentResult,
  SubmitAssessmentRequest
} from '../models/assessment.model';

@Injectable({
  providedIn: 'root'
})
export class AssessmentService {
  private readonly apiUrl =
    'http://localhost:5087/api/assessment';

  constructor(private http: HttpClient) {}

  getQuestions(skillId: number): Observable<AssessmentQuestion[]> {
    return this.http.get<AssessmentQuestion[]>(
      `${this.apiUrl}/${skillId}`
    );
  }

  submitAssessment(
    request: SubmitAssessmentRequest
  ): Observable<AssessmentResult> {
    return this.http.post<AssessmentResult>(
      `${this.apiUrl}/submit`,
      request
    );
  }

  getLatestResult(
    skillId: number
  ): Observable<AssessmentResult> {
    return this.http.get<AssessmentResult>(
      `${this.apiUrl}/history/${skillId}`
    );
  }
}