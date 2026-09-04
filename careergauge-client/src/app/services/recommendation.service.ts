import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CareerRecommendation } from '../models/career-recommendation';
import { ReadinessResult } from '../models/readiness-result';

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5087/api/recommendations';

  getRecommendations(
    learnerId: number
  ): Observable<CareerRecommendation[]> {
    return this.http.get<CareerRecommendation[]>(
      `${this.apiUrl}/${learnerId}`
    );
  }

  getRecommendationDetails(
    learnerId: number,
    careerProfileId: number
  ): Observable<ReadinessResult> {
    return this.http.get<ReadinessResult>(
      `${this.apiUrl}/${learnerId}/career/${careerProfileId}`
    );
  }
}