
using PuzzleGame.Data;

namespace PuzzleGame.Pages;

public partial class ResultsPage : ContentPage
{
    private readonly PuzzleDatabase _database = new();

    public ResultsPage()
    {
        InitializeComponent();
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var puzzles = await _database.GetPuzzlesAsync();
        var results = await _database.GetGameResultsAsync();

        ResultsList.ItemsSource = results
            .Select(result => new
            {
                PuzzleName = puzzles.FirstOrDefault(
                    puzzle => puzzle.Id == result.PuzzleId)?.Name
                    ?? "Deleted puzzle",

                result.Difficulty,
                result.Score,
                result.TimeUsed,
                Status = result.Completed ? "PUZZLE SOLVED" : "PUZZLE FAILED",
                StatusColor = result.Completed ? "#218838" : "#DC3545",
                result.PlayedAt
            })
            .OrderByDescending(result => result.PlayedAt)
            .ToList();
    }

}
