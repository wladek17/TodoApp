import { test, expect } from "@playwright/test";

test.beforeEach(async ({ page }) => {
  await page.goto("/login");
  await page.fill('input[placeholder="Email"]', "e2e@example.com");
  await page.fill('input[placeholder="Password"]', "_f0Z3$3{bBw*");
  await page.click('button[type="submit"]');
  await expect(page).toHaveURL(/\/tasks/);
});

test("user email is shown after login", async ({ page }) => {
  await expect(page.getByText("Logged in as: e2e@example.com")).toBeVisible();
});

test("user can log out", async ({ page }) => {
  await page.click('button:has-text("Logout")');
  await expect(page).toHaveURL(/\/login/);
});

test("user can add a new task", async ({ page }) => {
  const taskText = `Test Task ${Date.now()}`;
  await page.fill('input[placeholder="New task..."]', taskText);
  await page.click('button:has-text("Add")');
  await expect(page.getByText(taskText)).toBeVisible();
});

test("user can delete a task", async ({ page }) => {
  const taskText = `Test Task ${Date.now()}`;
  await page.fill('input[placeholder="New task..."]', taskText);
  await page.click('button:has-text("Add")');
  const task = page.getByText(taskText);
  await expect(task).toBeVisible();

  const taskItem = page.locator("li", { hasText: taskText });
  const deleteBtn = taskItem.locator('button[title="Delete"]');
  await deleteBtn.click();
  await expect(taskItem).toBeHidden();
});

test("user can toggle task as completed", async ({ page }) => {
  const taskText = `Test Task ${Date.now()}`;
  await page.fill('input[placeholder="New task..."]', taskText);
  await page.click('button:has-text("Add")');

  const taskItem = page.locator("li", { hasText: taskText });
  const checkbox = taskItem.locator('input[type="checkbox"]');

  await checkbox.check();
  await expect(checkbox).toBeChecked();

  await checkbox.uncheck();
  await expect(checkbox).not.toBeChecked();
});
