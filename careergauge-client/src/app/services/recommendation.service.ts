import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CareerRecommendation } from '../models/career-recommendation';

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
}