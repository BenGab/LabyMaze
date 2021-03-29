using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LabyMaze
{
    class LabyControl : FrameworkElement
    {
        LabyModel model;
        LabyLogic logic;
        LabyRenderer renderer;
        Stopwatch stw;

        public LabyControl()
        {
            this.Loaded += LabyControl_Loaded;
        }

        private void LabyControl_Loaded(object sender, RoutedEventArgs e)
        {
            stw = new Stopwatch();
            model = new LabyModel(ActualWidth, ActualHeight);
            logic = new LabyLogic(model, "LabyMaze.Levels.L0{0}.lvl");
            renderer = new LabyRenderer(model);

            Window win = Window.GetWindow(this);
            if (win != null)
            {
                win.KeyDown += Win_KeyDown;
            }
            InvalidateVisual();
            stw.Start();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (renderer != null) drawingContext.DrawDrawing(renderer.BuildDrawing());
        }

        private void Win_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            bool finished = false;
            switch (e.Key)
            {
                case Key.W: finished = logic.Move(0, -1); break;
                case Key.S: finished = logic.Move(0, 1); break;
                case Key.A: finished = logic.Move(-1, 0); break;
                case Key.D: finished = logic.Move(1, 0); break;
            }
            InvalidateVisual();
            if (finished)
            {
                stw.Stop();
                MessageBox.Show("YAY! " + stw.Elapsed.ToString(@"hh\:mm\:ss\.fff"));
                //++model.Level;
                //logic.LoadNextLevel();
                InvalidateVisual();
            }
        }
    }
}
