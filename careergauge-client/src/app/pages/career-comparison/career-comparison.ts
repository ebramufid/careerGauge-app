import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { RecommendationService } from '../../services/recommendation.service';
import { CareerComparison } from '../../models/career-comparison';

@Component({
  selector: 'app-career-comparison',
  imports: [CommonModule, RouterLink],
  templateUrl: './career-comparison.html',
  styleUrl: './career-comparison.scss'
})
export class CareerComparisonPage implements OnInit {
  private readonly recommendationService = inject(
    RecommendationService
  );

  readonly comparisons = signal<CareerComparison[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');

  // Temporary demo learner.
  // We'll replace this with the authenticated learner later.
  private readonly learnerId = 1;

  // Temporary selection.
  // We'll make this selectable from the UI shortly.
  private readonly careerProfileIds = [2, 3];

  ngOnInit(): void {
    this.loadComparison();
  }

  private loadComparison(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.recommendationService
      .getComparison(
        this.learnerId,
        this.careerProfileIds
      )
      .subscribe({
        next: (comparisons) => {
          this.comparisons.set(comparisons);
          this.isLoading.set(false);
        },

        error: (error) => {
          console.error(
            'Failed to load career comparison:',
            error
          );

          this.errorMessage.set(
            'Unable to load career comparison.'
          );

          this.isLoading.set(false);
        }
      });
  }
}