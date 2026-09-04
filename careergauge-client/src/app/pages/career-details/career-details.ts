import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { RecommendationService } from '../../services/recommendation.service';
import { ReadinessResult } from '../../models/readiness-result';

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

  readonly result = signal<ReadinessResult | null>(null);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');

  // Temporary demo learner.
  private readonly learnerId = 1;

  ngOnInit(): void {
    const careerProfileId = Number(
      this.route.snapshot.paramMap.get('careerProfileId')
    );

    this.loadDetails(careerProfileId);
  }

  private loadDetails(careerProfileId: number): void {
    this.recommendationService
      .getRecommendationDetails(
        this.learnerId,
        careerProfileId
      )
      .subscribe({
        next: (result) => {
          console.log('CAREER DETAILS RECEIVED:', result);

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