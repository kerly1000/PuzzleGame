
using PuzzleGame.Models;


namespace PuzzleGame.Pages;

public partial class PuzzleSetupPage : ContentPage
{
    private readonly Puzzle _puzzle;

    public PuzzleSetupPage(Puzzle puzzle)
    {
        InitializeComponent();

        _puzzle = puzzle;

        PuzzleNameLabel.Text = puzzle.Name;
        SelectedPuzzleImage.Source = ImageSource.FromFile(puzzle.ImagePath);

        DifficultyPicker.SelectedIndex = 0;
    }


    private async void StartPuzzle_Clicked(object sender, EventArgs e)
    {
        if (DifficultyPicker.SelectedIndex < 0)
        {
            await DisplayAlert(
                "Choose difficulty",
                "Please select a difficulty level.",
                "OK");

            return;
        }

        string difficulty = DifficultyPicker.SelectedIndex switch
        {
            0 => "Easy",
            1 => "Medium",
            2 => "Hard",
            _ => "Easy"
        };

        await Navigation.PushAsync(
            new PuzzleGamePage(_puzzle, difficulty));
    }

}
