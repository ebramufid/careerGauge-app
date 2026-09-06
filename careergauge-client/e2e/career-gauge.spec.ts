import { test, expect } from '@playwright/test';

test('learner can log in and reach the dashboard', async ({ page }) => {
  await page.goto('/login');

  await page.getByLabel(/email/i).fill('demo@careergauge.local');
  await page.getByLabel(/password/i).fill('Demo123!');
await page.getByRole('button', { name: /sign in/i }).click();
  

  await expect(page).toHaveURL(/dashboard/);

  await expect(
    page.getByText(/career recommendations/i)
  ).toBeVisible();
});