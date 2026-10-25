
using PuzzleGame.Data;
using PuzzleGame.Models;

namespace PuzzleGame.Services;

public class PuzzleService
{
    private readonly PuzzleDatabase _database;

    public PuzzleService(PuzzleDatabase database)
    {
        _database = database;
    }

    public async Task<List<Puzzle>> GetPuzzlesAsync()
    {
        return await _database.GetPuzzlesAsync();
    }

    public async Task SavePuzzleAsync(Puzzle puzzle)
    {
        await _database.SavePuzzleAsync(puzzle);
    }

    public async Task DeletePuzzleAsync(Puzzle puzzle)
    {
        await _database.DeletePuzzleAsync(puzzle);
    }
}
