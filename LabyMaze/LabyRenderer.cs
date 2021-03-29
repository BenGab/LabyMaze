using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LabyMaze
{
    class LabyRenderer
    {
        LabyModel model;
        Drawing oldBackground;
        Drawing oldWalls;
        Drawing oldExit;
        Drawing oldPlayer;
        Point oldPlayerPosition;
        Dictionary<string, Brush> myBrushes = new Dictionary<string, Brush>();

        public LabyRenderer(LabyModel model)
        {
            this.model = model;
        }

        public void Reset()
        {
            oldBackground = null;
            oldWalls = null;
            oldExit = null;
            oldPlayer = null;
            oldPlayerPosition = new Point(-1, -1);
            myBrushes.Clear();
        }

        Brush GetBrush(string fname, bool isTiled)
        {
            if(!myBrushes.ContainsKey(fname))
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = Assembly.GetExecutingAssembly().GetManifestResourceStream(fname);
                bmp.EndInit();
                ImageBrush ib = new ImageBrush(bmp);
                if(isTiled)
                {
                    ib.TileMode = TileMode.Tile;
                    ib.Viewport = new Rect(0, 0, model.TileSize, model.TileSize);
                    ib.ViewportUnits = BrushMappingMode.Absolute;
                }

                myBrushes.Add(fname, ib);
            }
            return myBrushes[fname];
        }

        Brush PlayerBrush { get { return GetBrush("LabyMaze.Images.player.bmp", false); } }
        Brush ExitBrush { get { return GetBrush("LabyMaze.Images.exit.bmp", false); } }
        Brush WallBrush { get { return GetBrush("LabyMaze.Images.wall.bmp", true); } }

        public Drawing BuildDrawing()
        {
            DrawingGroup dg = new DrawingGroup();
            dg.Children.Add(GetBackground());
            dg.Children.Add(GetWalls());
            dg.Children.Add(GetExit());
            dg.Children.Add(GetPlayer());
            return dg;
        }

        private Drawing GetBackground()
        {
            if (oldBackground == null)
            {
                Geometry backgroundGeometry = new RectangleGeometry(new Rect(0, 0, model.GameWidth, model.GameHeight));
                oldBackground = new GeometryDrawing(Brushes.Black, null, backgroundGeometry);
            }
            return oldBackground;
        }

        private Drawing GetExit()
        {
            if (oldExit == null)
            {
                Geometry g = new RectangleGeometry(new Rect(
                    model.Exit.X * model.TileSize, model.Exit.Y * model.TileSize,
                    model.TileSize, model.TileSize));
                oldExit = new GeometryDrawing(ExitBrush, null, g);
            }
            return oldExit;
        }

        private Drawing GetPlayer()
        {
            if (oldPlayer == null || oldPlayerPosition != model.Player)
            {
                Geometry g = new RectangleGeometry(new Rect(model.Player.X * model.TileSize, model.Player.Y * model.TileSize, model.TileSize, model.TileSize));
                oldPlayer = new GeometryDrawing(PlayerBrush, null, g);
                oldPlayerPosition = model.Player;
            }
            return oldPlayer;
        }

        private Drawing GetWalls()
        {
            if (oldWalls == null)
            {
                GeometryGroup g = new GeometryGroup();
                for (int x = 0; x < model.Walls.GetLength(0); x++)
                {
                    for (int y = 0; y < model.Walls.GetLength(1); y++)
                    {
                        if (model.Walls[x, y])
                        {
                            Geometry box = new RectangleGeometry(new Rect(x * model.TileSize, y * model.TileSize, model.TileSize, model.TileSize));
                            g.Children.Add(box);
                        }
                    }
                }
                oldWalls = new GeometryDrawing(WallBrush, null, g);
            }
            return oldWalls;
        }
    }
}
