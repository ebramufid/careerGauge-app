import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',

  use: {
    baseURL: 'http://localhost:4201',
    browserName: 'chromium',
    headless: true,
    screenshot: 'only-on-failure',
    trace: 'retain-on-failure',
  },

  reporter: 'list',
});