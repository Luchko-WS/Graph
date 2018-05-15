using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;

namespace Graph.PointsModel
{
    public class Viewer
    {
        private readonly Form _graphicForm;
        private Graphics _graphics;
        private readonly GraphVertexesRepository _repository;

        //brushes
        private readonly static Color _backgroundLayoutColor = Color.White;
        private readonly static Color _backgroundOutsideColor = Color.DarkGray;
        private readonly SolidBrush _backgroundBrush = new SolidBrush(_backgroundLayoutColor);
        private readonly SolidBrush _simpleVertexBrush = new SolidBrush(Color.Black);
        private readonly SolidBrush _selectedVertexBrush = new SolidBrush(Color.Red);
        private readonly SolidBrush _connectingVertexroundBrush = new SolidBrush(Color.Green);

        private bool _saveProportions;
        private int _fixedHeight;
        private int _fixedWidth;

        public Viewer(Form form, GraphVertexesRepository repository)
        {
            _graphicForm = form;
            _graphicForm.BackColor = _backgroundOutsideColor;
            _fixedHeight = _graphicForm.Height;
            _fixedWidth = _graphicForm.Width;

            _repository = repository;

            _graphicForm.Shown += _graphicForm_Shown; ;
            _graphicForm.ResizeEnd += _graphicControl_ResizeEnd;

            _repository.OnDrawVertex += _repository_DrawVertex;
            _repository.SelectedVertexes.CollectionChanged += SelectedPoints_CollectionChanged;
            _repository.OnRemoveVertex += _repository_OnRemoveVertex;
            _repository.OnVertexesConnected += _repository_OnVertexesConnected;
            _repository.OnSettingSourceVertex += _repository_OnSettingSourceVertex;
            _repository.OnRemovingSourceVertex += _repository_OnRemovingSourceVertex;
        }

        private void _graphicForm_Shown(object sender, System.EventArgs e)
        {
            _graphics = _graphicForm.CreateGraphics();
            _graphics.Clear(_backgroundLayoutColor);
        }

        public bool SaveProportions
        {
            get { return _saveProportions; }
            set { _saveProportions = value; }
        }

        private void DrawVertex(GraphVertex vertex, Brush brush)
        {
            if (_graphics != null)
            {
                _graphics.FillEllipse(brush, vertex.ClientRectangle);
            }
        }

        private void DrawSimpleVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _simpleVertexBrush);
        }

        private void ClearVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _backgroundBrush);
        }

        private void DrawConnectingVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _connectingVertexroundBrush);
        }

        private void DrawSelectedVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _selectedVertexBrush);
        }

        private void DrawEdge(GraphVertex x, GraphVertex y, Brush brush)
        {
            if (_graphics != null)
            {
                Point A = x.GetCentreOfClientRectangle();
                Point B = y.GetCentreOfClientRectangle();
                _graphics.DrawLine(new Pen(brush, 4), A, B);
            }
        }

        //need in optimization
        public void Invalidate()
        {
            _graphics = _graphicForm.CreateGraphics();
            _graphics.Clear(_backgroundLayoutColor);

            //begin
            foreach (var vertex in _repository.Vertexes)
            {
                if (_saveProportions)
                {
                    var newX = (int)((double)vertex.ClientRectangle.Location.X / _fixedWidth * _graphicForm.Width);
                    var newY = (int)((double)vertex.ClientRectangle.Location.Y / _fixedHeight * _graphicForm.Height);
                    vertex.ChangeLocation(newX, newY);
                }
            }

            foreach (var vertex in _repository.Vertexes)
            {
                foreach (var relVertex in vertex.RelativeVertexes)
                {
                    DrawEdge(vertex, relVertex, Brushes.Black);
                }

                if (_repository.SelectedVertexes.Contains(vertex))
                {
                    DrawSelectedVertex(vertex);
                }
                else
                {
                    DrawSimpleVertex(vertex);
                }
            }

            if (_repository.СonnectingVertex != null)
            {
                DrawConnectingVertex(_repository.СonnectingVertex);
            }
            //end

            _fixedWidth = _graphicForm.Width;
            _fixedHeight = _graphicForm.Height;
        }

        private void _graphicControl_ResizeEnd(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void _repository_DrawVertex(GraphVertex vertex)
        {
            DrawSimpleVertex(vertex);
        }

        private void _repository_OnRemoveVertex(GraphVertex vertex)
        {
            ClearVertex(vertex);
            foreach (var relVertex in vertex.RelativeVertexes)
            {
                DrawEdge(vertex, relVertex, new SolidBrush(_backgroundLayoutColor));
            }
        }

        private void _repository_OnVertexesConnected(GraphVertex x, GraphVertex y)
        {
            DrawEdge(x, y, Brushes.Black);
        }

        private void _repository_OnSettingSourceVertex(GraphVertex vertex)
        {
            DrawConnectingVertex(vertex);
        }

        private void _repository_OnRemovingSourceVertex(GraphVertex vertex)
        {
            DrawSimpleVertex(vertex);
        }

        private void SelectedPoints_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach(var item in e.NewItems)
                    {
                        var vertex = (GraphVertex)item;
                        DrawSelectedVertex(vertex);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var vertex = (GraphVertex)item;
                        DrawSimpleVertex(vertex);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Invalidate();
                    break;
            }
        }
    }
}
