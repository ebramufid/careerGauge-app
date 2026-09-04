import { LearningPriority } from './learning-priority';

export interface SkillGap {
  skillId: number;
  skillName: string;
  currentLevel: number;
  requiredLevel: number;
  gap: number;
  status: string;
}

export interface ReadinessResult {
  careerProfileId: number;
  careerName: string;
  readinessPercentage: number;
  requiredSkills: number;
  metSkills: number;
  partialSkills: number;
  missingSkills: number;
  skillGaps: SkillGap[];
  learningPriorities: LearningPriority[];
}