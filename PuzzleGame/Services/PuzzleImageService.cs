
using SkiaSharp;

namespace PuzzleGame.Services;

public class PuzzleImageService
{
    public List<string> CreateTiles(
        string imagePath,
        int puzzleId,
        int gridSize)
    {
        var tilePaths = new List<string>();

        using SKCodec codec = SKCodec.Create(imagePath);

        using SKBitmap decoded = new SKBitmap(
            codec.Info.Width,
            codec.Info.Height);

        SKCodecResult result = codec.GetPixels(
            decoded.Info,
            decoded.GetPixels());

        if (result != SKCodecResult.Success &&
            result != SKCodecResult.IncompleteInput)
        {
            throw new InvalidOperationException(
                "The selected image could not be opened.");
        }

        // Parandame kõigepealt foto pööramise.
        using SKBitmap oriented = CorrectOrientation(
            decoded,
            codec.EncodedOrigin);

        // Kärbime foto keskelt ruudukujuliseks.
        using SKBitmap source = CropToSquare(oriented);

        int tileSize = source.Width / gridSize;

        string folderPath = Path.Combine(
            FileSystem.AppDataDirectory,
            "PuzzleTiles",
            $"Puzzle_{puzzleId}");

        Directory.CreateDirectory(folderPath);

        for (int row = 0; row < gridSize; row++)
        {
            for (int column = 0; column < gridSize; column++)
            {
                int left = column * tileSize;
                int top = row * tileSize;

                using SKBitmap tile = new SKBitmap(
                    tileSize,
                    tileSize);

                using (SKCanvas canvas = new SKCanvas(tile))
                {
                    canvas.DrawBitmap(
                        source,
                        new SKRect(
                            left,
                            top,
                            left + tileSize,
                            top + tileSize),
                        new SKRect(
                            0,
                            0,
                            tileSize,
                            tileSize));
                }

                string tilePath = Path.Combine(
                    folderPath,
                    $"tile_{gridSize}_{row}_{column}.png");

                using SKImage image = SKImage.FromBitmap(tile);

                using SKData data = image.Encode(
                    SKEncodedImageFormat.Png,
                    100);

                File.WriteAllBytes(tilePath, data.ToArray());

                tilePaths.Add(tilePath);
            }
        }

        return tilePaths;
    }

    private static SKBitmap CropToSquare(SKBitmap original)
    {
        int squareSize = Math.Min(
            original.Width,
            original.Height);

        // Leiame ruudu keskkoha järgi.
        int left = (original.Width - squareSize) / 2;
        int top = (original.Height - squareSize) / 2;

        var square = new SKBitmap(
            squareSize,
            squareSize);

        using (SKCanvas canvas = new SKCanvas(square))
        {
            canvas.DrawBitmap(
                original,
                new SKRect(
                    left,
                    top,
                    left + squareSize,
                    top + squareSize),
                new SKRect(
                    0,
                    0,
                    squareSize,
                    squareSize));
        }

        return square;
    }

    private static SKBitmap CorrectOrientation(
        SKBitmap original,
        SKEncodedOrigin origin)
    {
        if (origin != SKEncodedOrigin.RightTop &&
            origin != SKEncodedOrigin.LeftBottom)
        {
            return original.Copy();
        }

        using SKBitmap rotated = new SKBitmap(
            original.Height,
            original.Width);

        using (SKCanvas canvas = new SKCanvas(rotated))
        {
            if (origin == SKEncodedOrigin.RightTop)
            {
                canvas.Translate(rotated.Width, 0);
                canvas.RotateDegrees(90);
            }
            else
            {
                canvas.Translate(0, rotated.Height);
                canvas.RotateDegrees(-90);
            }

            canvas.DrawBitmap(original, 0, 0);
            canvas.Flush();
        }

        return rotated.Copy();
    }
}

