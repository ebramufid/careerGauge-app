import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { LearnerSkillService } from '../../services/learner-skill.service';
import { LearnerSkill } from '../../models/learner-skill';
import { AuthService } from '../../services/auth.service';
import {
  AssessmentResult
} from '../../models/assessment.model';
import { AssessmentService } from '../../services/assessment.service';

@Component({
  selector: 'app-skill-profile',
  imports: [CommonModule, RouterLink],
  templateUrl: './skill-profile.html',
  styleUrl: './skill-profile.scss'
})
export class SkillProfile implements OnInit {
  private readonly learnerSkillService = inject(
    LearnerSkillService
  );

  private readonly authService = inject(AuthService);

  private readonly assessmentService = inject(
    AssessmentService
  );

  readonly skills = signal<LearnerSkill[]>([]);
  readonly isLoading = signal(true);
  readonly isSaving = signal(false);
  readonly savedMessage = signal('');
  readonly errorMessage = signal('');

  readonly assessmentResults =
    signal<Record<number, AssessmentResult>>({});

  private learnerId = 0;

  readonly skillSummary = () => {
    const skills = this.skills();

    return {
      advanced: skills.filter(
        s => s.currentLevel === 3
      ).length,

      intermediate: skills.filter(
        s => s.currentLevel === 2
      ).length,

      beginner: skills.filter(
        s => s.currentLevel === 1
      ).length,

      missing: skills.filter(
        s => s.currentLevel === 0
      ).length
    };
  };

  ngOnInit(): void {
    this.authService.getCurrentUser().subscribe({
      next: (user) => {
        this.learnerId = user.learnerId;
        this.loadSkills();
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

  private loadSkills(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.learnerSkillService
      .getLearnerSkills(this.learnerId)
      .subscribe({
        next: (skills) => {
          this.skills.set(skills);
          this.isLoading.set(false);

          this.loadAssessmentResults(skills);
        },

        error: (error) => {
          console.error(
            'Failed to load learner skills:',
            error
          );

          this.errorMessage.set(
            'Unable to load your skills.'
          );

          this.isLoading.set(false);
        }
      });
  }

  private loadAssessmentResults(
    skills: LearnerSkill[]
  ): void {
    const cSharpSkill = skills.find(
      skill =>
        skill.skillName.trim().toLowerCase() === 'c#'
    );

    if (!cSharpSkill) {
      return;
    }

    this.assessmentService
      .getLatestResult(cSharpSkill.skillId)
      .subscribe({
        next: (result) => {
          this.assessmentResults.update(results => ({
            ...results,
            [cSharpSkill.skillId]: result
          }));
        },

        error: (error) => {
          if (error.status !== 404) {
            console.error(
              'Failed to load assessment result:',
              error
            );
          }
        }
      });
  }

  setLevel(
    skillId: number,
    level: number
  ): void {
    this.skills.update(skills =>
      skills.map(skill =>
        skill.skillId === skillId
          ? {
              ...skill,
              currentLevel: level
            }
          : skill
      )
    );

    this.savedMessage.set('');
    this.errorMessage.set('');
  }

  saveSkills(): void {
    this.isSaving.set(true);
    this.savedMessage.set('');
    this.errorMessage.set('');

    const updates = this.skills().map(skill => ({
      skillId: skill.skillId,
      currentLevel: skill.currentLevel
    }));

    this.learnerSkillService
      .updateLearnerSkills(
        this.learnerId,
        updates
      )
      .subscribe({
        next: (skills) => {
          this.skills.set(skills);
          this.isSaving.set(false);

          this.savedMessage.set(
            'Your skills have been saved successfully.'
          );
        },

        error: (error) => {
          console.error(
            'Failed to save learner skills:',
            error
          );

          this.isSaving.set(false);

          this.errorMessage.set(
            'Unable to save your skills. Please try again.'
          );
        }
      });
  }

  hasAssessment(
    skillName: string
  ): boolean {
    return skillName
      .trim()
      .toLowerCase() === 'c#';
  }

  getAssessmentResult(
    skillId: number
  ): AssessmentResult | undefined {
    return this.assessmentResults()[skillId];
  }

  getLevelLabel(
    level: number
  ): string {
    switch (level) {
      case 1:
        return 'Beginner';

      case 2:
        return 'Intermediate';

      case 3:
        return 'Advanced';

      default:
        return 'Missing';
    }
  }
}