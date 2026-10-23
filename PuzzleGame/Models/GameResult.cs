using System;
using System.Collections.Generic;
using System.Text;

namespace PuzzleGame.Models
{
    public class GameResult
    {
        public int Id { get; set; }

        public int PuzzleId { get; set; }

        public string Difficulty { get; set; } = string.Empty;

        public int Score { get; set; }

        public int TimeUsed { get; set; }

        public bool Completed { get; set; }

        public DateTime PlayedAt { get; set; }
    }
}
