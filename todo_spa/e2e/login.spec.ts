import { test, expect } from "@playwright/test";

test("user can log in", async ({ page }) => {
  await page.goto("http://localhost:5173/login");
  await page.fill('input[placeholder="Email"]', "e2e@example.com");
  await page.fill('input[placeholder="Password"]', "_f0Z3$3{bBw*");
  await page.click('button[type="submit"]');

  await expect(page).toHaveURL(/\/tasks/);
  await expect(page.getByText('Logged in as: e2e@example.com')).toBeVisible();
});

test("redirects to /tasks after login", async ({ page }) => {
  await page.goto("http://localhost:5173/login");
  await page.fill('input[placeholder="Email"]', "e2e@example.com");
  await page.fill('input[placeholder="Password"]', "_f0Z3$3{bBw*");
  await page.click('button[type="submit"]');

  await expect(page).toHaveURL("http://localhost:5173/tasks");
});
