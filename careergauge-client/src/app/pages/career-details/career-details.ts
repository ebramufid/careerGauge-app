import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { RecommendationService } from '../../services/recommendation.service';
import { ReadinessResult } from '../../models/readiness-result';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-career-details',
  imports: [CommonModule, RouterLink],
  templateUrl: './career-details.html',
  styleUrl: './career-details.scss'
})
export class CareerDetails implements OnInit {
  private readonly recommendationService = inject(
    RecommendationService
  );

  private readonly route = inject(ActivatedRoute);
  private readonly authService = inject(AuthService);

  readonly result = signal<ReadinessResult | null>(null);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');
  readonly getLevelLabel = (level: number) => {
  switch (level) {
    case 0:
      return 'Missing';
    case 1:
      return 'Beginner';
    case 2:
      return 'Intermediate';
    case 3:
      return 'Advanced';
    default:
      return 'Unknown';
  }
};



  ngOnInit(): void {
  const careerProfileId = Number(
    this.route.snapshot.paramMap.get('careerProfileId')
  );

  this.authService.getCurrentUser().subscribe({
    next: (user) => {
      this.loadDetails(user.learnerId, careerProfileId);
    },

    error: (error) => {
      console.error(
        'Failed to get current user:',
        error
      );

      this.errorMessage.set(
        'Unable to identify the current learner.'
      );

      this.isLoading.set(false);
    }
  });
}

  

  private loadDetails(
  learnerId: number,
  careerProfileId: number
): void {
    this.recommendationService
      .getRecommendationDetails(
        learnerId,
        careerProfileId
      )
      .subscribe({
        next: (result) => {

          this.result.set(result);
          this.isLoading.set(false);
        },

        error: (error) => {
          console.error(
            'Failed to load career details:',
            error
          );

          this.errorMessage.set(
            'Unable to load career details.'
          );

          this.isLoading.set(false);
        }
      });
  }
}