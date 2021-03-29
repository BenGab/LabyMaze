using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LabyMaze
{
    class LabyLogic
    {
        LabyModel model;
        private readonly string fname;

        public LabyLogic(LabyModel model, string fname)
        {
            this.model = model;
            this.fname = fname;
            InitModel(string.Format(fname, model.Level));
        }

        private void InitModel(string fname)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fname);
            StreamReader sr = new StreamReader(stream);
            string[] lines = sr.ReadToEnd().Replace("\r", "").Split('\n');

            int width = int.Parse(lines[0]);
            int height = int.Parse(lines[1]);
            model.Walls = new bool[width, height];
            model.TileSize = Math.Min(model.GameWidth / width, model.GameHeight / height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //line[2]
                    char current = lines[y + 2][x];
                    model.Walls[x, y] = (current == 'e');
                    if (current == 'S') model.Player = new Point(x, y);
                    if (current == 'F') model.Exit = new Point(x, y);
                }
            }
        }

        public void LoadNextLevel()
        {
            if(model.Level <= 4)
            {
                InitModel(string.Format(fname, model.Level));
            }
        }

        public bool Move(int dx, int dy) // [-1..1] double???
        {
            int newx = (int)(model.Player.X + dx);
            int newy = (int)(model.Player.Y + dy);
            if (newx >= 0 && newy >= 0 &&
                newx < model.Walls.GetLength(0) &&
                newy < model.Walls.GetLength(1) &&
                !model.Walls[newx, newy])
            {
                // model.Player.X = newx; 
                // can't do this => not a variable => ChangeX, ChangeY => Pong
                model.Player = new Point(newx, newy);
            }
            return model.Player.Equals(model.Exit);
        }

        public Point GetTilePos(Point mousePos) // Pixel position => Tile position
        {
            return new Point((int)(mousePos.X / model.TileSize),
                            (int)(mousePos.Y / model.TileSize));
        }
    }
}
