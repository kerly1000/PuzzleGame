
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
        PuzzleList.ItemsSource = puzzles;
    }

    private async void AddPuzzle_Clicked(object sender, EventArgs e)
    {
        var page = new AddPuzzlePage(_puzzleService);
        await Navigation.PushAsync(page);
    }

    private async void Puzzle_Selected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Puzzle selectedPuzzle)
            return;

        ((CollectionView)sender).SelectedItem = null;

        await Navigation.PushAsync(new PuzzleSetupPage(selectedPuzzle));
    }
}
