
using PuzzleGame.Models;
using PuzzleGame.Services;

namespace PuzzleGame.Pages;

public partial class AddPuzzlePage : ContentPage
{
    private readonly PuzzleService _puzzleService;
    private string? _selectedImagePath;

    public AddPuzzlePage(PuzzleService puzzleService)
    {
        InitializeComponent();
        _puzzleService = puzzleService;
    }

    private async void ChoosePicture_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await MediaPicker.Default.PickPhotoAsync();

            if (result == null)
                return;

            string fileName =
                $"{Guid.NewGuid()}{Path.GetExtension(result.FileName)}";

            string localPath = Path.Combine(
                FileSystem.AppDataDirectory,
                fileName);

            using Stream sourceStream = await result.OpenReadAsync();
            using FileStream localStream = File.Create(localPath);

            await sourceStream.CopyToAsync(localStream);

            _selectedImagePath = localPath;
            PreviewImage.Source = ImageSource.FromFile(localPath);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private async void SavePuzzle_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(PuzzleNameEntry.Text))
        {
            await DisplayAlert(
                "Missing name",
                "Please enter a puzzle name.",
                "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(_selectedImagePath))
        {
            await DisplayAlert(
                "Missing picture",
                "Please choose a picture first.",
                "OK");
            return;
        }

        try
        {
            var puzzle = new Puzzle
            {
                Name = PuzzleNameEntry.Text.Trim(),
                ImagePath = _selectedImagePath,
                CreatedAt = DateTime.Now
            };

            await _puzzleService.SavePuzzleAsync(puzzle);

            await DisplayAlert(
                "Success",
                "Puzzle was saved!",
                "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Save error", ex.Message, "OK");
        }
    }
}
