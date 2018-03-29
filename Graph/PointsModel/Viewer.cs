using System.Drawing;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace Graph.PointsModel
{
    public class Viewer
    {
        private Control _graphicControl;
        private Graphics _graphics;
        private GraphVertexesRepository _repository;
        private Color _backgroundColor = Color.LightGray;

        private bool _saveProportions;
        private bool _cacheDrawing;
        private int _fixedHeight;
        private int _fixedWidth;

        public Viewer(Control control, GraphVertexesRepository repository)
        {
            _graphicControl = control;
            _fixedHeight = _graphicControl.Height;
            _fixedWidth = _graphicControl.Width;

            _graphics = _graphicControl.CreateGraphics();

            _repository = repository;
            _repository.OnDrawPoint += _repository_DrawPoint;
            _repository.SelectedVertexes.CollectionChanged += SelectedPoints_CollectionChanged;
            _repository.OnRemovePoint += _repository_OnRemovePoint;
        }

        public bool CacheDrawing
        {
            get { return _cacheDrawing; }
            set { _cacheDrawing = value; }
        }

        public bool SaveProportions
        {
            get { return _saveProportions; }
            set { _saveProportions = value; }
        }

        public void DrawVertex(Rectangle rectangle, Brush brush)
        {
            if (_graphics != null)
            {
                _graphics.FillEllipse(brush, rectangle);
            }
        }

        public void Invalidate()
        {
            if (!_cacheDrawing)
            {
                _graphics = _graphicControl.CreateGraphics();
                _graphics.Clear(_backgroundColor);
                foreach (var vertex in _repository.Vertexes)
                {
                    if (_saveProportions)
                    {
                        vertex.ChangePointLocation((int)((double)vertex.ClientRectangle.X / _fixedWidth * _graphicControl.Width),
                            (int)((double)vertex.ClientRectangle.Y / _fixedHeight * _graphicControl.Height));
                    }
                    if (_repository.SelectedVertexes.Contains(vertex))
                    {
                        DrawVertex(vertex.ClientRectangle, Brushes.Red);
                    }
                    else
                    {
                        DrawVertex(vertex.ClientRectangle, Brushes.Black);
                    }
                }
            }
        }

        private void _repository_DrawPoint(GraphVertex vertex)
        {
            DrawVertex(vertex.ClientRectangle, Brushes.Black);
        }

        private void _repository_OnRemovePoint(GraphVertex vertex)
        {
            DrawVertex(vertex.ClientRectangle, new SolidBrush(_backgroundColor));
        }

        private void SelectedPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var item in e.NewItems)
                    {
                        var vertex = (GraphVertex)item;
                        DrawVertex(vertex.ClientRectangle, Brushes.Red);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var vertex = (GraphVertex)item;
                        DrawVertex(vertex.ClientRectangle, Brushes.Black);
                    }
                    break;
            }
        }
    }
}
