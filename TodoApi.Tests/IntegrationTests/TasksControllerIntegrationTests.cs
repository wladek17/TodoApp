using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Controllers;
using TodoApi.Data;
using TodoApi.Models;
using FluentAssertions;

namespace TodoApi.Tests.IntegrationTests
{
    public class TasksControllerIntegrationTests
    {
        private TasksController CreateTasksControllerWithUser(ApplicationDbContext context, string userId)
        {
            var controller = new TasksController(context);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        private ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task GetTask_get_task_successfully()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

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
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

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
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

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
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

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
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

            context.Tasks.Add(new TodoTask
            {
                Id = 1,
                Title = "Original Title",
                IsCompleted = false,
                UserId = userId
            });
            await context.SaveChangesAsync();

            var updatedTask = new TodoTask
            {
                Title = "Updated Title",
                IsCompleted = true
            };

            // Act
            var result = await controller.UpdateTask(1, updatedTask);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var taskInDb = await context.Tasks.FirstOrDefaultAsync(t => t.Id == 1 && t.UserId == userId);

            taskInDb.Should().NotBeNull();
            taskInDb.Title.Should().Be("Updated Title");
            taskInDb.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTask_when_task_does_not_exist_returns_not_found()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

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
            var context = CreateInMemoryDbContext();
            var controller = CreateTasksControllerWithUser(context, userId);

            context.Tasks.Add(new TodoTask
            {
                Id = 1,
                Title = "Task to delete",
                IsCompleted = false,
                UserId = userId
            });
            await context.SaveChangesAsync();

            // Act
            var result = await controller.DeleteTask(1);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var taskInDb = await context.Tasks.FirstOrDefaultAsync(t => t.Id == 1 && t.UserId == userId);
            taskInDb.Should().BeNull();
        }
    }
}
