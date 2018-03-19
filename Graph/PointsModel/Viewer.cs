using System.Drawing;
using System.Windows.Forms;

namespace Graph.PointsModel
{
    public class Viewer
    {
        public Viewer(Control control, GRPointRepository repository)
        {
            _graphicControl = control;
            _fixedHeight = _graphicControl.Height;
            _fixedWidth = _graphicControl.Width;

            _graphics = _graphicControl.CreateGraphics();

            _repository = repository;
            _repository.DrawPoint += _repository_DrawPoint;
        }

        public bool SaveProportions
        {
            get { return _saveProportions; }
            set { _saveProportions = value; }
        }


        public void DrawPoint(int x, int y)
        {
            if (_graphics != null)
            {
                _graphics.FillEllipse(Brushes.Red, new Rectangle(x - 4, y - 4, 9, 9));
            }
        }

        public void Invalidate()
        {
            _graphics = _graphicControl.CreateGraphics();
            _graphics.Clear(Color.LightGray);
            foreach (var point in _repository.Points)
            {
                if (_saveProportions)
                {
                    DrawPoint(
                        (int)((double)point.X / _fixedWidth * _graphicControl.Width),
                        (int)((double)point.Y / _fixedHeight * _graphicControl.Height));
                }
                else
                {
                    DrawPoint(point.X, point.Y);
                }
            }
        }

        private void _repository_DrawPoint(int x, int y)
        {
            DrawPoint(x, y);
        }

        private Control _graphicControl;
        
        private Graphics _graphics;

        private GRPointRepository _repository;

        private bool _saveProportions;
        private int _fixedHeight;
        private int _fixedWidth;
    }
}
