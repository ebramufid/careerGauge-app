# CareerGauge

CareerGauge is a career-readiness platform that helps learners understand how their current skills align with different career paths.

The platform evaluates a learner's skills against career requirements, calculates career readiness, identifies skill gaps, recommends learning priorities, and provides skill assessments that can update the learner's skill level based on evidence.

---

## Problem

Learners often know which career they are interested in but do not have a clear way to answer questions such as:

- How ready am I for this career?
- Which skills do I already have?
- Which skills am I missing?
- Which skills should I improve first?
- How can I validate my current skill level?

CareerGauge addresses these questions by connecting a learner's skill profile with career requirements and turning the comparison into actionable guidance.

---

## Solution

CareerGauge provides a learner-focused workflow:


Learner Skill Profile
        ↓
Career Requirements
        ↓
Readiness Calculation
        ↓
Career Recommendations
        ↓
Skill Gaps
        ↓
Learning Priorities
        ↓
Skill Assessment
        ↓
Evidence-based Skill Level
        ↓
Updated Career Readiness

Career Readiness Calculation

CareerGauge compares a learner's current skill level with the required level for each career.

A skill is classified as:

Met

The learner's current level meets or exceeds the required level.

Partial

The learner has some proficiency but has not yet reached the required level.

Missing

The learner's current level is zero.

The readiness calculation is:

Readiness =
(met + partial × 0.5) / required × 100


Career Recommendations

Career recommendations are based on the learner's current skill profile and the requirements defined for each career.

Each recommendation includes:

Career name
Readiness percentage
Required skill count
Met skill count
Partial skill count
Missing skill count
Skill gap count
Strengths
Skill gaps
Learning priorities

This allows the system to provide both a recommendation and an explanation for that recommendation.

Authentication

CareerGauge uses:

ASP.NET Identity
JWT Bearer authentication
HttpOnly authentication cookies

The access token is stored in an HttpOnly cookie rather than browser local storage.

This avoids exposing the authentication token directly to JavaScript running in the browser.

The Angular application sends authenticated requests using credentials, while the backend extracts the authenticated learner identity from the JWT claims.

Current Scope

CareerGauge intentionally focuses on a practical career-readiness workflow.

The current implementation includes a C# assessment as the initial evidence-based skill assessment.

The platform is designed so additional assessments and skills can be added later.