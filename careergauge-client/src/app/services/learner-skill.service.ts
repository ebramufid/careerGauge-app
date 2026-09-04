import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { LearnerSkill } from '../models/learner-skill';

interface UpdateLearnerSkill {
  skillId: number;
  currentLevel: number;
}

@Injectable({
  providedIn: 'root'
})
export class LearnerSkillService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5087/api/learnerskills';

  getLearnerSkills(
    learnerId: number
  ): Observable<LearnerSkill[]> {
    return this.http.get<LearnerSkill[]>(
      `${this.apiUrl}/${learnerId}`
    );
  }

  updateLearnerSkills(
    learnerId: number,
    updates: UpdateLearnerSkill[]
  ): Observable<LearnerSkill[]> {
    return this.http.put<LearnerSkill[]>(
      `${this.apiUrl}/${learnerId}`,
      updates
    );
  }
}