
using PuzzleGame.Models;
using PuzzleGame.Services;
using PuzzleGame.Data;

namespace PuzzleGame.Pages;

public partial class PuzzleGamePage : ContentPage
{
    private readonly Puzzle _puzzle;
    private readonly string _difficulty;
    private readonly PuzzleImageService _imageService = new();
    private readonly PuzzleDatabase _database = new();

    private int _gridSize;
    private int _moves;

    private int _timeLeft;
    private IDispatcherTimer? _timer;
    private bool _gameOver;

    private readonly List<int> _tileValues = new();
    private readonly List<ImageButton> _tileButtons = new();
    private List<string> _tilePaths = new();

    private int _selectedIndex = -1;

    public PuzzleGamePage(Puzzle puzzle, string difficulty)
    {
        InitializeComponent();

        _puzzle = puzzle;
        _difficulty = difficulty;

        _gridSize = difficulty switch
        {
            "Easy" => 3,
            "Medium" => 4,
            "Hard" => 5,
            _ => 3
        };

        GameTitleLabel.Text = puzzle.Name;
        ReferenceImage.Source = ImageSource.FromFile(puzzle.ImagePath);

        DifficultyLabel.Text =
            $"Difficulty: {_difficulty} ({_gridSize} x {_gridSize})";

        _timeLeft = 90;
        
        TimerLabel.Text = $"Time: {_timeLeft}";

        CreateBoard();
        StartTimer();
    }

    private void CreateBoard()
    {
        BoardGrid.Children.Clear();
        BoardGrid.RowDefinitions.Clear();
        BoardGrid.ColumnDefinitions.Clear();

        _tileValues.Clear();
        _tileButtons.Clear();

        _selectedIndex = -1;
        _moves = 0;

        BoardGrid.WidthRequest = 320;
        BoardGrid.HeightRequest = 320;

        int totalTiles = _gridSize * _gridSize;

        // Loome mängulaua read ja veerud.
        for (int i = 0; i < _gridSize; i++)
        {
            BoardGrid.RowDefinitions.Add(
                new RowDefinition(GridLength.Star));

            BoardGrid.ColumnDefinitions.Add(
                new ColumnDefinition(GridLength.Star));
        }

        // Lõikame algse pildi eraldi pilditükkideks.
        _tilePaths = _imageService.CreateTiles(
            _puzzle.ImagePath,
            _puzzle.Id,
            _gridSize);

        // Iga number tähistab algse pildi üht tükki.
        for (int i = 1; i <= totalTiles; i++)
        {
            _tileValues.Add(i);
        }

        // Segame tükid juhuslikku järjekorda.
        Random random = new Random();

        do
        {
            for (int i = _tileValues.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                (_tileValues[i], _tileValues[j]) =
                    (_tileValues[j], _tileValues[i]);
            }
        }
        while (IsSolved());

        // Kuvame pilditükid mängulaual.
        for (int i = 0; i < totalTiles; i++)
        {
            int index = i;

            ImageButton tileButton = new ImageButton
            {
                Source = ImageSource.FromFile(
                    _tilePaths[_tileValues[i] - 1]),

                Aspect = Aspect.AspectFill,
                Padding = 0,
                Margin = 0,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                BackgroundColor = Colors.Transparent,
                BorderColor = Colors.Transparent,
                BorderWidth = 0
            };

            tileButton.Clicked += Tile_Clicked;

            Grid.SetRow(tileButton, i / _gridSize);
            Grid.SetColumn(tileButton, i % _gridSize);

            BoardGrid.Children.Add(tileButton);
            _tileButtons.Add(tileButton);
        }

        MovesLabel.Text = "Moves: 0";
    }

    private void StartTimer()
    {
        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        if (_gameOver)
            return;

        _timeLeft--;
        TimerLabel.Text = $"Time: {_timeLeft}";

        if (_timeLeft <= 0)
        {
            _gameOver = true;
            _timer?.Stop();

            await SaveGameResultAsync(false);

            await DisplayAlert(
                "Time's up!",
                $"You ran out of time.\nMoves: {_moves}",
                "OK");

            await Navigation.PopAsync();
        }
    }

    private async void Tile_Clicked(object? sender, EventArgs e)
    {
        if (_gameOver || sender is not ImageButton clickedButton)
            return;

        int currentIndex = _tileButtons.IndexOf(clickedButton);

        if (currentIndex == -1)
            return;

        // Esimese klõpsuga valime tüki.
        if (_selectedIndex == -1)
        {
            _selectedIndex = currentIndex;

            clickedButton.BorderColor = Colors.Red;
            clickedButton.BorderWidth = 5;

            return;
        }

        // Teise klõpsuga samal tükil tühistame valiku.
        if (_selectedIndex == currentIndex)
        {
            clickedButton.BorderColor = Colors.Transparent;
            clickedButton.BorderWidth = 2;

            _selectedIndex = -1;

            return;
        }

        // Vahetame kahe valitud tüki asukohad.
        int firstValue = _tileValues[_selectedIndex];
        int secondValue = _tileValues[currentIndex];

        _tileValues[_selectedIndex] = secondValue;
        _tileValues[currentIndex] = firstValue;

        // Uuendame ka mängulaual kuvatavaid pilte.
        _tileButtons[_selectedIndex].Source =
            ImageSource.FromFile(_tilePaths[secondValue - 1]);

        _tileButtons[currentIndex].Source =
            ImageSource.FromFile(_tilePaths[firstValue - 1]);

        _tileButtons[_selectedIndex].BorderColor = Colors.Transparent;
        _tileButtons[_selectedIndex].BorderWidth = 2;

        _tileButtons[currentIndex].BorderColor = Colors.Transparent;
        _tileButtons[currentIndex].BorderWidth = 2;

        _selectedIndex = -1;

        _moves++;
        MovesLabel.Text = $"Moves: {_moves}";

        // Kontrollime, kas kõik tükid on õiges järjekorras.
        if (IsSolved())
        {
            _gameOver = true;
            _timer?.Stop();

            await SaveGameResultAsync(true);

            await DisplayAlert(
                "Congratulations!",
                $"You solved the puzzle in {_moves} moves!",
                "OK");
        }
    }

    private async Task SaveGameResultAsync(bool completed)
    {
        var result = new GameResult
        {
            PuzzleId = _puzzle.Id,
            Difficulty = _difficulty,
            Score = _moves,
            TimeUsed = 90 - _timeLeft,
            Completed = completed,
            PlayedAt = DateTime.Now
        };

        await _database.SaveGameResultAsync(result);
    }


    private bool IsSolved()
    {
        for (int i = 0; i < _tileValues.Count; i++)
        {
            if (_tileValues[i] != i + 1)
                return false;
        }

        return true;
    }

    private async void Back_Clicked(object sender, EventArgs e)
    {
        _gameOver = true;
        _timer?.Stop();

        await Navigation.PopAsync();
    }
}
