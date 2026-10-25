using SQLite;
using PuzzleGame.Models;

namespace PuzzleGame.Data;

public class PuzzleDatabase
{
    private readonly SQLiteAsyncConnection _database;
    private const int DatabaseVersion = 2;

    public PuzzleDatabase()
    {
        string databasePath = Path.Combine(
            FileSystem.AppDataDirectory,
            "puzzlegame.db3");

        _database = new SQLiteAsyncConnection(databasePath);
    }


    public async Task InitAsync()
    {
        var version = await _database.ExecuteScalarAsync<int>(
            "PRAGMA user_version");

        if (version < DatabaseVersion)
        {
            await _database.DropTableAsync<Puzzle>();
            await _database.CreateTableAsync<Puzzle>();

            await _database.CreateTableAsync<GameResult>();

            await _database.ExecuteAsync(
                $"PRAGMA user_version = {DatabaseVersion}");
        }
        else
        {
            await _database.CreateTableAsync<Puzzle>();
            await _database.CreateTableAsync<GameResult>();
        }
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

        int result = await _database.InsertAsync(puzzle);

        System.Diagnostics.Debug.WriteLine(
            $"Saved puzzle: {puzzle.Name}, Database result: {result}, Puzzle ID: {puzzle.Id}");

        return result;
    }


    public async Task<int> DeletePuzzleAsync(Puzzle puzzle)
    {
        await InitAsync();

        int deletedRows = await _database.DeleteAsync(puzzle);

        System.Diagnostics.Debug.WriteLine(
            $"Deleted rows: {deletedRows}, Puzzle ID: {puzzle.Id}");

        return deletedRows;
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