using GroupProject421_521.Controllers;
using GroupProject421_521.Data;
using GroupProject421_521.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GroupProject421_521.Tests
{
    /// <summary>
    /// Runs the controllers against a real SQLite database held in memory,
    /// so each test starts with a clean, isolated schema.
    /// </summary>
    public sealed class TestDatabase : IDisposable
    {
        private readonly SqliteConnection _connection;

        public TestDatabase()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            using var context = CreateContext();
            context.Database.EnsureCreated();
        }

        public ApplicationDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;
            return new ApplicationDbContext(options);
        }

        public void Dispose() => _connection.Dispose();
    }

    public class WinesControllerTests : IDisposable
    {
        private readonly TestDatabase _db = new();

        public void Dispose() => _db.Dispose();

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            using var context = _db.CreateContext();
            var controller = new WinesController(context);

            var result = await controller.Details(null);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenWineDoesNotExist()
        {
            using var context = _db.CreateContext();
            var controller = new WinesController(context);

            var result = await controller.Details(999);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsWine_WhenItExists()
        {
            using (var seed = _db.CreateContext())
            {
                seed.Wine.Add(new Wine { Name = "Test Malbec", Type = "Red", Price = 18.99m });
                await seed.SaveChangesAsync();
            }

            using var context = _db.CreateContext();
            var controller = new WinesController(context);
            var id = context.Wine.Single().WineID;

            var result = await controller.Details(id);

            var view = Assert.IsType<ViewResult>(result);
            var wine = Assert.IsType<Wine>(view.Model);
            Assert.Equal("Test Malbec", wine.Name);
        }

        [Fact]
        public async Task Create_SavesWine_AndRedirectsToIndex()
        {
            using var context = _db.CreateContext();
            var controller = new WinesController(context);

            var result = await controller.Create(new Wine { Name = "Test Riesling", Type = "White", Price = 14m });

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal(nameof(WinesController.Index), redirect.ActionName);
            Assert.Equal("Test Riesling", context.Wine.Single().Name);
        }

        [Fact]
        public async Task Create_ReturnsView_WhenModelIsInvalid()
        {
            using var context = _db.CreateContext();
            var controller = new WinesController(context);
            controller.ModelState.AddModelError("Name", "Required");

            var result = await controller.Create(new Wine { Name = "" });

            Assert.IsType<ViewResult>(result);
            Assert.Empty(context.Wine);
        }

        [Fact]
        public async Task DeleteConfirmed_RemovesWine()
        {
            using (var seed = _db.CreateContext())
            {
                seed.Wine.Add(new Wine { Name = "To Delete", Price = 10m });
                await seed.SaveChangesAsync();
            }

            using var context = _db.CreateContext();
            var controller = new WinesController(context);
            var id = context.Wine.Single().WineID;

            await controller.DeleteConfirmed(id);

            Assert.Empty(context.Wine);
        }
    }

    public class UserPromptsControllerTests : IDisposable
    {
        private readonly TestDatabase _db = new();

        public void Dispose() => _db.Dispose();

        private static UserPrompt NewPrompt(string text, DateTime createdAt, bool deleted = false) => new()
        {
            UserId = "test-user",
            PromptText = text,
            Preferences = "",
            WineSuggestion = "Pinot Noir",
            CreatedAt = createdAt,
            IsDeleted = deleted
        };

        [Fact]
        public async Task Index_HidesSoftDeletedPrompts_AndShowsNewestFirst()
        {
            using (var seed = _db.CreateContext())
            {
                seed.UserPrompt.AddRange(
                    NewPrompt("older", new DateTime(2024, 11, 1)),
                    NewPrompt("newer", new DateTime(2024, 11, 2)),
                    NewPrompt("deleted", new DateTime(2024, 11, 3), deleted: true));
                await seed.SaveChangesAsync();
            }

            using var context = _db.CreateContext();
            var controller = new UserPromptsController(context);

            var result = await controller.Index();

            var view = Assert.IsType<ViewResult>(result);
            var prompts = Assert.IsAssignableFrom<IEnumerable<UserPrompt>>(view.Model).ToList();
            Assert.Equal(new[] { "newer", "older" }, prompts.Select(p => p.PromptText));
        }

        [Fact]
        public async Task DeleteConfirmed_SoftDeletes_InsteadOfRemovingRow()
        {
            using (var seed = _db.CreateContext())
            {
                seed.UserPrompt.Add(NewPrompt("keep my history", DateTime.UtcNow));
                await seed.SaveChangesAsync();
            }

            using var context = _db.CreateContext();
            var controller = new UserPromptsController(context);
            var id = context.UserPrompt.Single().Id;

            await controller.DeleteConfirmed(id);

            var prompt = Assert.Single(context.UserPrompt);
            Assert.True(prompt.IsDeleted);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_ForSoftDeletedPrompt()
        {
            using (var seed = _db.CreateContext())
            {
                seed.UserPrompt.Add(NewPrompt("gone", DateTime.UtcNow, deleted: true));
                await seed.SaveChangesAsync();
            }

            using var context = _db.CreateContext();
            var controller = new UserPromptsController(context);
            var id = context.UserPrompt.Single().Id;

            var result = await controller.Details(id);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
