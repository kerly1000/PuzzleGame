
using PuzzleGame.Data;
using PuzzleGame.Models;

namespace PuzzleGame.Services;

public class DefaultPuzzleService
{
    private readonly PuzzleDatabase _database;

    public DefaultPuzzleService(PuzzleDatabase database)
    {
        _database = database;
    }

    public async Task AddDefaultPuzzlesAsync()
    {
        var existingPuzzles = await _database.GetPuzzlesAsync();

        var defaultPuzzles = new[]
        {
            new { Name = "Old house", FileName = "starter_cat.jpg" },
            new { Name = "White dogs", FileName = "starter_dog.jpg" },
            new { Name = "Sunny mountains", FileName = "starter_forest.jpg" }
        };

        foreach (var item in defaultPuzzles)
        {
            bool alreadyExists = existingPuzzles.Any(
                puzzle => puzzle.Name == item.Name);

            if (alreadyExists)
                continue;

            string sourcePath = Path.Combine(
                AppContext.BaseDirectory,
                "Resources",
                "Images",
                item.FileName);

            // MAUI pakendatud piltide lugemiseks kasutame
            // FileSystem.OpenAppPackageFileAsync meetodit.
            string destinationPath = Path.Combine(
                FileSystem.AppDataDirectory,
                item.FileName);

            if (!File.Exists(destinationPath))
            {
                using var source = await FileSystem.OpenAppPackageFileAsync(
                    item.FileName);

                using var destination = File.Create(destinationPath);

                await source.CopyToAsync(destination);
            }

            await _database.SavePuzzleAsync(new Puzzle
            {
                Name = item.Name,
                ImagePath = destinationPath,
                CreatedAt = DateTime.Now
            });
        }
    }
}
