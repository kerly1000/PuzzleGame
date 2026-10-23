using PuzzleGame;
using PuzzleGame.Data;
using PuzzleGame.Models;

private readonly PuzzleDatabase _database;

public MainPage(PuzzleDatabase database)
{
    

    _database = database;
    TestDatabaseAsync();
}

private async void TestDatabaseAsync()
{
    try
    {
        await _database.InitAsync();

        var puzzle = new Puzzle
        {
            Name = "Forest",
            ImagePath = "forest.jpg",
            CreatedAt = DateTime.Now
        };

        await _database.SavePuzzleAsync(puzzle);

        var puzzles = await _database.GetPuzzlesAsync();

        foreach (var item in puzzles)
        {
            System.Diagnostics.Debug.WriteLine(
                $"ID: {item.Id}, Name: {item.Name}, Image: {item.ImagePath}");
        }
    }
    catch (Exception ex)
    {
        System.Diagnostics.Debug.WriteLine(
            $"Database error: {ex.Message}");
    }
}