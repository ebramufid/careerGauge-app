import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { RecommendationService } from '../../services/recommendation.service';
import { CareerComparison } from '../../models/career-comparison';
import { CareerProfile } from '../../models/career-profile';
import { AuthService } from '../../services/auth.service';

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

  private readonly authService = inject(AuthService);

  readonly comparisons = signal<CareerComparison[]>([]);
  readonly careers = signal<CareerProfile[]>([]);
  readonly isLoading = signal(true);
  readonly errorMessage = signal('');

  private learnerId = 0;

  readonly selectedCareerIds = signal<number[]>([2, 3]);

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe({
      next: (user) => {
        this.learnerId = user.learnerId;
        this.loadCareers();
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

  isSelected(careerId: number): boolean {
    return this.selectedCareerIds().includes(careerId);
  }

  toggleCareer(careerId: number): void {
    const selected = this.selectedCareerIds();

    if (selected.includes(careerId)) {
      this.selectedCareerIds.set(
        selected.filter(id => id !== careerId)
      );

      return;
    }

    if (selected.length >= 3) {
      return;
    }

    this.selectedCareerIds.set([
      ...selected,
      careerId
    ]);
  }

  readonly readinessLabel = (percentage: number): string => {
    if (percentage >= 70) return 'Strong match';
    if (percentage >= 60) return 'Good match';
    if (percentage >= 50) return 'Developing';
    if (percentage >= 40) return 'Needs development';
    return 'Early stage';
  };

  compareSelected(): void {
    if (this.selectedCareerIds().length < 2) {
      this.errorMessage.set(
        'Select at least two careers to compare.'
      );

      return;
    }

    this.loadComparison();
  }

  private loadCareers(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.recommendationService
      .getCareerProfiles()
      .subscribe({
        next: (careers) => {
          this.careers.set(careers);
          this.loadComparison();
        },

        error: (error) => {
          console.error(
            'Failed to load career profiles:',
            error
          );

          this.errorMessage.set(
            'Unable to load available careers.'
          );

          this.isLoading.set(false);
        }
      });
  }

  private loadComparison(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.recommendationService
      .getComparison(
        this.learnerId,
        this.selectedCareerIds()
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