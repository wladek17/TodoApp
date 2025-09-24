using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;
using TodoApi.Tests.E2E;

namespace TodoApi.Tests.IntegrationTests
{
    public class TasksControllerIntegrationTestsMySql : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly ApplicationDbContext _dbContext;

        public TasksControllerIntegrationTestsMySql(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        private TasksController CreateTasksControllerWithUser(string userId)
        {
            if (!_dbContext.Users.Any(u => u.Id == userId))
            {
                var user = new User
                {
                    Id = userId,
                    UserName = $"testuser_{userId}"
                };

                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }

            var controller = new TasksController(_dbContext);

            var userClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = userClaimsPrincipal }
            };

            return controller;
        }

        [Fact]
        public async Task GetTask_get_task_successfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            var task = new TodoTask
            {
                Title = "Test Integration Task",
                IsCompleted = false
            };

            // Act: create
            var createResult = await controller.CreateTask(task) as CreatedAtActionResult;

            // Assert: creation result
            createResult.Should().NotBeNull();
            var createdTask = createResult.Value as TodoTask;
            createdTask.Should().NotBeNull();
            createdTask.Title.Should().Be("Test Integration Task");
            createdTask.UserId.Should().Be(userId);

            // Act: get
            var getResult = await controller.GetTask(createdTask.Id) as OkObjectResult;

            // Assert: retrieved task
            getResult.Should().NotBeNull();
            var retrievedTask = getResult.Value as TodoTask;
            retrievedTask.Should().NotBeNull();
            retrievedTask.Id.Should().Be(createdTask.Id);
            retrievedTask.Title.Should().Be("Test Integration Task");
        }

        [Fact]
        public async Task UpdateTask_update_task_successfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            var task = new TodoTask
            {
                Title = "Original Title",
                IsCompleted = false
            };

            var createResult = await controller.CreateTask(task) as CreatedAtActionResult;
            var createdTask = createResult!.Value as TodoTask;

            var updatedTask = new TodoTask
            {
                Title = "Updated Title",
                IsCompleted = true
            };

            // Act
            var updateResult = await controller.UpdateTask(createdTask!.Id, updatedTask);

            // Assert
            updateResult.Should().BeOfType<NoContentResult>();

            var getResult = await controller.GetTask(createdTask.Id) as OkObjectResult;
            var retrievedTask = getResult!.Value as TodoTask;
            retrievedTask!.Title.Should().Be("Updated Title");
            retrievedTask.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTask_delete_task_successfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            var task = new TodoTask
            {
                Title = "Task to Delete",
                IsCompleted = false
            };

            var createResult = await controller.CreateTask(task) as CreatedAtActionResult;
            var createdTask = createResult!.Value as TodoTask;

            // Act
            var deleteResult = await controller.DeleteTask(createdTask!.Id);

            // Assert
            deleteResult.Should().BeOfType<NoContentResult>();

            var getResult = await controller.GetTask(createdTask.Id);
            getResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateTask_when_task_does_not_exist_returns_not_found()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            var updatedTask = new TodoTask
            {
                Title = "Updated Task",
                IsCompleted = true
            };

            // Act
            var result = await controller.UpdateTask(1, updatedTask);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateTask_when_task_exists_updates_task_and_returns_no_content()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            var task = new TodoTask
            {
                Title = "Original Title",
                IsCompleted = false,
                UserId = userId
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            var createdTaskId = task.Id;

            var updatedTask = new TodoTask
            {
                Title = "Updated Title",
                IsCompleted = true
            };

            // Act
            var result = await controller.UpdateTask(createdTaskId, updatedTask);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var taskInDb = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == createdTaskId && t.UserId == userId);

            taskInDb.Should().NotBeNull();
            taskInDb.Title.Should().Be("Updated Title");
            taskInDb.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTask_when_task_does_not_exist_returns_not_found()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteTask_when_task_exists_deletes_task_and_returns_no_content()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var controller = CreateTasksControllerWithUser(userId);

            var task = new TodoTask
            {
                Title = "Task to delete",
                IsCompleted = false,
                UserId = userId
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            var createdTaskId = task.Id;

            // Act
            var result = await controller.DeleteTask(createdTaskId);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var taskInDb = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == createdTaskId && t.UserId == userId);
            taskInDb.Should().BeNull();
        }
    }
}
