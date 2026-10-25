
using PuzzleGame.Models;
using PuzzleGame.Services;
using PuzzleGame.Pages;

namespace PuzzleGame;

public partial class MainPage : ContentPage
{
    private readonly PuzzleService _puzzleService;
    
    public MainPage(PuzzleService puzzleService)
    {
        InitializeComponent();
        _puzzleService = puzzleService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadPuzzlesAsync();
    }


    private async Task LoadPuzzlesAsync()
    {
        var puzzles = await _puzzleService.GetPuzzlesAsync();

        foreach (var puzzle in puzzles)
        {
            System.Diagnostics.Debug.WriteLine(
                $"Loaded puzzle: {puzzle.Name}, ID: {puzzle.Id}");
        }

        PuzzleList.ItemsSource = puzzles;
    }


    private async void AddPuzzle_Clicked(object sender, EventArgs e)
    {
        var page = new AddPuzzlePage(_puzzleService);
        await Navigation.PushAsync(page);
    }

    private async void ViewResults_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ResultsPage());
    }

    private async void Puzzle_Selected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Puzzle selectedPuzzle)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Navigation.PushAsync(new PuzzleSetupPage(selectedPuzzle));
    }



    private async void DeletePuzzle_Clicked(object sender, EventArgs e)
    {
        if (sender is not Button button ||
            button.BindingContext is not Puzzle puzzle)
        {
            return;
        }

        bool confirm = await DisplayAlert(
            "Delete puzzle",
            $"Are you sure you want to delete '{puzzle.Name}'?",
            "Delete",
            "Cancel");

        if (!confirm)
            return;

        try
        {
            // Kustutame pusle andmebaasist.
            await _puzzleService.DeletePuzzleAsync(puzzle);


            // Kustutame rakenduse loodud pusletükid.
            string tilesFolder = Path.Combine(
                FileSystem.AppDataDirectory,
                "PuzzleTiles",
                $"Puzzle_{puzzle.Id}");

            if (Directory.Exists(tilesFolder))
            {
                Directory.Delete(tilesFolder, true);
            }

            // Kustutame rakenduse koopia, mitte telefoni algset fotot.
            if (File.Exists(puzzle.ImagePath))
            {
                File.Delete(puzzle.ImagePath);
            }

            // Värskendame nimekirja.
            await LoadPuzzlesAsync();

            await DisplayAlert(
                "Success",
                "Puzzle deleted successfully.",
                "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert(
                "Delete error",
                ex.Message,
                "OK");
        }
    }


}
