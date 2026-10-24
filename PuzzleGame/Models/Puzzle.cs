using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PuzzleGame.Models
{
    public class Puzzle
    {
        [PrimaryKey, AutoIncrement]
        public int Id{ get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
