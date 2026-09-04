import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { RecommendationService } from '../../services/recommendation.service';
import { CareerRecommendation } from '../../models/career-recommendation';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  private readonly recommendationService = inject(
    RecommendationService
  );

  readonly recommendations = signal<CareerRecommendation[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');

  // Temporary demo learner.
  // We'll replace this with the authenticated learner later.
  private readonly learnerId = 1;

  ngOnInit(): void {
    this.loadRecommendations();
  }

  private loadRecommendations(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.recommendationService
      .getRecommendations(this.learnerId)
      .subscribe({
        next: (recommendations) => {
          

          this.recommendations.set(recommendations);
          this.isLoading.set(false);
        },

        error: (error) => {
          console.error(
            'Failed to load career recommendations:',
            error
          );

          this.errorMessage.set(
            'Unable to load career recommendations.'
          );

          this.isLoading.set(false);
        }
      });
  }
}