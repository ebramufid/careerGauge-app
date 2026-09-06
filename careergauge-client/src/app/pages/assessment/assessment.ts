import {
  Component,
  OnInit,
  inject,
  signal
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { AssessmentService } from '../../services/assessment.service';

import {
  AssessmentQuestion,
  AssessmentResult,
  SubmitAssessmentRequest
} from '../../models/assessment.model';

@Component({
  selector: 'app-assessment',
  imports: [CommonModule],
  templateUrl: './assessment.html',
  styleUrl: './assessment.scss'
})
export class Assessment implements OnInit {
  private readonly assessmentService =
    inject(AssessmentService);

  private readonly route = inject(ActivatedRoute);

  private readonly router = inject(Router);

  readonly questions = signal<AssessmentQuestion[]>([]);

  readonly currentQuestionIndex = signal(0);

  readonly selectedAnswers = signal<Record<number, string>>({});

  readonly isLoading = signal(true);

  readonly isSubmitting = signal(false);

  readonly errorMessage = signal('');

  readonly result = signal<AssessmentResult | null>(null);

  skillId = 0;

  ngOnInit(): void {
    const skillId = Number(
      this.route.snapshot.paramMap.get('skillId')
    );

    if (!skillId || skillId <= 0) {
      this.errorMessage.set(
        'Invalid assessment skill.'
      );

      this.isLoading.set(false);

      return;
    }

    this.skillId = skillId;

    this.loadQuestions();
  }

  private loadQuestions(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.assessmentService
      .getQuestions(this.skillId)
      .subscribe({
        next: (questions) => {
          this.questions.set(questions);
          this.currentQuestionIndex.set(0);
          this.isLoading.set(false);
        },

        error: (error) => {
          console.error(
            'Failed to load assessment questions:',
            error
          );

          this.errorMessage.set(
            'Unable to load the assessment questions.'
          );

          this.isLoading.set(false);
        }
      });
  }

  readonly currentQuestion = () => {
    const questions = this.questions();

    return questions[
      this.currentQuestionIndex()
    ];
  };

  readonly progressPercentage = () => {
    const total = this.questions().length;

    if (total === 0) {
      return 0;
    }

    return Math.round(
      ((this.currentQuestionIndex() + 1) / total) * 100
    );
  };

  readonly isLastQuestion = () => {
    return (
      this.currentQuestionIndex() ===
      this.questions().length - 1
    );
  };

  readonly hasSelectedAnswer = () => {
    const question = this.currentQuestion();

    if (!question) {
      return false;
    }

    return !!this.selectedAnswers()[question.id];
  };

  selectAnswer(
    questionId: number,
    answer: string
  ): void {
    this.selectedAnswers.update(
      (answers) => ({
        ...answers,
        [questionId]: answer
      })
    );
  }

  isSelected(
    questionId: number,
    answer: string
  ): boolean {
    return (
      this.selectedAnswers()[questionId] === answer
    );
  }

  nextQuestion(): void {
    if (!this.hasSelectedAnswer()) {
      return;
    }

    if (!this.isLastQuestion()) {
      this.currentQuestionIndex.update(
        (index) => index + 1
      );
    }
  }

  previousQuestion(): void {
    if (this.currentQuestionIndex() > 0) {
      this.currentQuestionIndex.update(
        (index) => index - 1
      );
    }
  }

  submitAssessment(): void {
    if (
      this.isSubmitting() ||
      !this.hasAnsweredAllQuestions()
    ) {
      return;
    }

    const request: SubmitAssessmentRequest = {
      skillId: this.skillId,
      answers: this.questions().map(
        (question) => ({
          questionId: question.id,
          answer:
            this.selectedAnswers()[question.id]
        })
      )
    };

    this.isSubmitting.set(true);
    this.errorMessage.set('');

    this.assessmentService
      .submitAssessment(request)
      .subscribe({
        next: (result) => {
          this.result.set(result);
          this.isSubmitting.set(false);
        },

        error: (error) => {
          console.error(
            'Failed to submit assessment:',
            error
          );

          this.errorMessage.set(
            error?.error?.message ??
            'Unable to submit the assessment.'
          );

          this.isSubmitting.set(false);
        }
      });
  }

  readonly hasAnsweredAllQuestions = () => {
    const questions = this.questions();
    const answers = this.selectedAnswers();

    if (questions.length === 0) {
      return false;
    }

    return questions.every(
      (question) => !!answers[question.id]
    );
  };

  retakeAssessment(): void {
    this.result.set(null);
    this.selectedAnswers.set({});
    this.currentQuestionIndex.set(0);
    this.errorMessage.set('');
  }

  goToSkills(): void {
    this.router.navigate(['/skills']);
  }

  goToDashboard(): void {
    this.router.navigate(['/dashboard']);
  }
}