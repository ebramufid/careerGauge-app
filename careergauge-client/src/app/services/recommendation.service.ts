import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { CareerRecommendation } from '../models/career-recommendation';
import { ReadinessResult } from '../models/readiness-result';
import { CareerComparison } from '../models/career-comparison';
import { CareerProfile } from '../models/career-profile';


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

  getComparison(
  learnerId: number,
  careerProfileIds: number[]
): Observable<CareerComparison[]> {
  const params = careerProfileIds
    .map(id => `careerProfileIds=${id}`)
    .join('&');

return this.http.get<CareerComparison[]>(
  `${this.apiUrl}/${learnerId}/compare?${params}`
  
);
}

getCareerProfiles(): Observable<CareerProfile[]> {
return this.http.get<CareerProfile[]>(
  `${this.apiUrl}/careers`
  
);
}

}