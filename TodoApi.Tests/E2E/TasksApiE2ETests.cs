using System.Net.Http.Headers;
using TodoApi.Models.Auth;
using FluentAssertions;
using TodoApi.Models;
using System.Net.Http.Json;

namespace TodoApi.Tests.E2E
{
    public class TasksApiE2ETests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public TasksApiE2ETests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }
        
        [Fact]
        public async Task Should_register_login_create_update_delete_task_success()
        {
            // Arrange: Register a new user
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", new RegisterRequest
            {
                FullName = "Test User",
                Email = "test@example.com",
                Password = "Password123!"
            });
            registerResponse.EnsureSuccessStatusCode();

            // Arrange: Login and retrieve token
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", new LoginRequest
            {
                Email = "test@example.com",
                Password = "Password123!"
            });
            loginResponse.EnsureSuccessStatusCode();

            var loginContent = await loginResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            var token = loginContent!["token"];

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Act: Create a new task
            var createResponse = await _client.PostAsJsonAsync("/api/tasks", new
            {
                title = "Integration Test Task",
                isCompleted = false,
            });
            createResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);

            var createdTask = await createResponse.Content.ReadFromJsonAsync<TodoTask>();
            createdTask.Should().NotBeNull();
            createdTask.Title.Should().Be("Integration Test Task");

            // Act: Update the created task
            var updateResponse = await _client.PutAsJsonAsync($"/api/tasks/{createdTask.Id}", new
            {
                title = "Updated Test Task",
                isCompleted = true
            });
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            // Assert: Retrieve and verify the updated task
            var getAfterUpdateResponse = await _client.GetAsync($"/api/tasks/{createdTask.Id}");
            getAfterUpdateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var updatedTask = await getAfterUpdateResponse.Content.ReadFromJsonAsync<TodoTask>();
            updatedTask.Should().NotBeNull();
            updatedTask.Title.Should().Be("Updated Test Task");
            updatedTask.IsCompleted.Should().BeTrue();

            // Act: Delete the task
            var deleteResponse = await _client.DeleteAsync($"/api/tasks/{createdTask.Id}");
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);

            // Assert: Ensure the task no longer exists
            var getAfterDeleteResponse = await _client.GetAsync($"/api/tasks/{createdTask.Id}");
            getAfterDeleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
