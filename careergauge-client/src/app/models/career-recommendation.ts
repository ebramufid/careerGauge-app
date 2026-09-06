import { LearningPriority } from './learning-priority';
export interface CareerRecommendation {
  careerProfileId: number;
  careerName: string;
  readinessPercentage: number;
  requiredSkills: number;
  metSkills: number;
  partialSkills: number;
  missingSkills: number;
  skillGapCount: number;

  strengths: string[];
  skillGaps: SkillGap[];
  learningPriorities: LearningPriority[];
  
}

export interface SkillGap {
  skillId: number;
  skillName: string;
  currentLevel: number;
  requiredLevel: number;
  gap: number;
  status: string;
}