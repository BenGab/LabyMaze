using System.Windows;

namespace LabyMaze
{
    class LabyModel
    {
        public bool[,] Walls { get; set; } // Tile[X, Y] true = wall
        public Point Player { get; set; } // Tile coordinates
        public Point Exit { get; set; } // Tile coordinates

        public double GameWidth { get; private set; } // Pixel size
        public double GameHeight { get; private set; } // Pixel size
        public double TileSize { get; set; } // Pixel size

        public int Level { get; set; }

        public LabyModel(double w, double h)
        {
            Level = 0;
            this.GameWidth = w; this.GameHeight = h;
        }
    }
}
