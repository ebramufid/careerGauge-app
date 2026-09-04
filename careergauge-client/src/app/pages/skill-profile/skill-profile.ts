import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { LearnerSkillService } from '../../services/learner-skill.service';
import { LearnerSkill } from '../../models/learner-skill';

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

  readonly skills = signal<LearnerSkill[]>([]);
  readonly isLoading = signal(true);
  readonly isSaving = signal(false);
  readonly savedMessage = signal('');
  readonly errorMessage = signal('');

  // Temporary demo learner.
  // We'll replace this with the authenticated learner later.
  private readonly learnerId = 1;

  ngOnInit(): void {
    this.loadSkills();
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

  setLevel(skillId: number, level: number): void {
    this.skills.update(skills =>
      skills.map(skill =>
        skill.skillId === skillId
          ? { ...skill, currentLevel: level }
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

  getLevelLabel(level: number): string {
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