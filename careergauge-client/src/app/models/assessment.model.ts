export interface AssessmentQuestion {
  id: number;
  skillId: number;
  questionText: string;
  optionA: string;
  optionB: string;
  optionC: string;
  optionD: string;
  difficulty: number;
}

export interface AssessmentAnswer {
  questionId: number;
  answer: string;
}

export interface SubmitAssessmentRequest {
  skillId: number;
  answers: AssessmentAnswer[];
}

export interface AssessmentResult {
  skillId: number;
  score: number;
  totalQuestions: number;
  percentage: number;
  resultLevel: number;
  levelName: string;
}