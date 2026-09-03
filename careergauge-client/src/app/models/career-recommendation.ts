export interface CareerRecommendation {
  careerProfileId: number;
  careerName: string;
  readinessPercentage: number;
  requiredSkills: number;
  metSkills: number;
  partialSkills: number;
  missingSkills: number;
  skillGapCount: number;
}