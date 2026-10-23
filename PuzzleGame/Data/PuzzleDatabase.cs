using SQLite;
using PuzzleGame.Models;

namespace PuzzleGame.Data;

public class PuzzleDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public PuzzleDatabase()
    {
        string databasePath = Path.Combine(
            FileSystem.AppDataDirectory,
            "puzzlegame.db3");

        _database = new SQLiteAsyncConnection(databasePath);
    }

    public async Task InitAsync()
    {
        await _database.CreateTableAsync<Puzzle>();
        await _database.CreateTableAsync<GameResult>();
    }

    //pusle andmete salvestamine ja lugemine andmebaasist
    public async Task<List<Puzzle>> GetPuzzlesAsync()
    {
        await InitAsync();

        return await _database.Table<Puzzle>().ToListAsync();
    }

    public async Task<int> SavePuzzleAsync(Puzzle puzzle)
    {
        await InitAsync();

        return await _database.InsertAsync(puzzle);
    }

    //mängu tulemuste salvestamine ja lugemine andmebaasist
    public async Task<int> SaveGameResultAsync(GameResult result)
    {
        await InitAsync();

        return await _database.InsertAsync(result);
    }

    public async Task<List<GameResult>> GetGameResultsAsync()
    {
        await InitAsync();

        return await _database
            .Table<GameResult>()
            .ToListAsync();
    }
}