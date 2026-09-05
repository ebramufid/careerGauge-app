import { AuthService } from '../../services/auth.service';
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

  private readonly authService = inject(AuthService);

  readonly recommendations = signal<CareerRecommendation[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');
  readonly readinessLabel = (percentage: number) => {
  if (percentage >= 70) return 'Strong match';
  if (percentage >= 60) return 'Good match';
  if (percentage >= 50) return 'Developing';
  if (percentage >= 40) return 'Needs development';
  return 'Early stage';
};

  readonly topReadiness = () => {
  const recommendations = this.recommendations();

  if (recommendations.length === 0) {
    return 0;
  }

  return recommendations[0].readinessPercentage;
};

readonly averageReadiness = () => {
  const recommendations = this.recommendations();

  if (recommendations.length === 0) {
    return 0;
  }

  const total = recommendations.reduce(
    (sum, career) => sum + career.readinessPercentage,
    0
  );

  return Math.round((total / recommendations.length) * 100) / 100;
};

readonly topCareerName = () => {
  const recommendations = this.recommendations();

  if (recommendations.length === 0) {
    return '—';
  }

  return recommendations[0].careerName;
};




ngOnInit(): void {
  this.authService.getCurrentUser().subscribe({
    next: (user) => {
      this.loadRecommendations(user.learnerId);
    },
    error: (error) => {
      console.error('Failed to get current user:', error);

      this.errorMessage.set(
        'Unable to identify the current learner.'
      );

      this.isLoading.set(false);
    }
  });
}

  private loadRecommendations(learnerId: number): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

this.recommendationService
  .getRecommendations(learnerId)
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