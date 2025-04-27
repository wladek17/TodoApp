using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TodoApi.Controllers;
using TodoApi.Data;
using FluentAssertions;
using TodoApi.Models;
using Moq;
using MockQueryable.Moq;

namespace TodoApi.Tests.UnitTests
{
    public class TasksControllerUnitTests
    {
        private static TasksController CreateTasksControllerWithUser(Mock<ApplicationDbContext> context, string userId)
        {
            var controller = new TasksController(context.Object);

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

        [Fact]
        public async Task GetTask_when_task_does_not_exist_returns_not_found_result()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var tasks = new List<TodoTask>().AsQueryable().BuildMockDbSet();
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Tasks).Returns(tasks.Object);

            var controller = CreateTasksControllerWithUser(mockContext, userId);

            // Act
            var result = await controller.GetTask(1);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetTask_when_task_exists_returns_ok()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var tasks = new List<TodoTask>
            {
                new TodoTask
                {
                    Id = 1,
                    Title = "Test Task",
                    IsCompleted = false,
                    UserId = userId
                }
            }.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Tasks).Returns(tasks.Object);

            var controller = CreateTasksControllerWithUser(mockContext, userId);

            // Act
            var result = await controller.GetTask(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeOfType<TodoTask>();

            var task = okResult.Value as TodoTask;
            task.Should().NotBeNull();
            task.Id.Should().Be(1);
            task.Title.Should().Be("Test Task");
            task.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public async Task GetTasks_should_return_tasks_when_tasks_exists()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var tasks = new List<TodoTask>
            {
                new TodoTask
                {
                    Id = 1,
                    Title = "Test Task",
                    IsCompleted = false,
                    UserId = userId
                }
            }.AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Tasks).Returns(tasks.Object);

            var controller = CreateTasksControllerWithUser(mockContext, userId);

            // Act
            var result = await controller.GetTasks() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var taskList = result!.Value as List<TodoTask>;
            taskList.Should().NotBeNull();
            taskList!.Count.Should().Be(1);
            taskList[0].Title.Should().Be("Test Task");
        }

        [Fact]
        public async Task GetTasks_should_return_empty_list_when_no_tasks()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var tasks = new List<TodoTask>().AsQueryable().BuildMockDbSet();

            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());
            mockContext.Setup(c => c.Tasks).Returns(tasks.Object);

            var controller = CreateTasksControllerWithUser(mockContext, userId);

            // Act
            var result = await controller.GetTasks() as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            var taskList = result!.Value as List<TodoTask>;
            taskList.Should().NotBeNull();
            taskList.Should().BeEmpty();
        }
    }
}
