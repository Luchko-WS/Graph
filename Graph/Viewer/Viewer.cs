using Graph.Model;
using System.Drawing;
using System.Windows.Forms;

namespace Graph.ViewModel
{
    public partial class Viewer
    {
        private readonly Form _graphicForm;
        private Graphics _graphics;
        private readonly GraphElementsRepository _repository;

        //brushes
        //background
        private static Color _backgroundLayoutColor = Color.White;
        private static Color _backgroundOutsideColor = Color.DarkGray;
        private SolidBrush _backgroundBrush = new SolidBrush(_backgroundLayoutColor);
        //vertex
        private SolidBrush _simpleVertexBrush = new SolidBrush(Color.Black);
        private SolidBrush _selectedVertexBrush = new SolidBrush(Color.Red);
        private SolidBrush _connectingVertexroundBrush = new SolidBrush(Color.Green);
        //edge
        private SolidBrush _simpleEdgeBrush = new SolidBrush(Color.Black);
        private SolidBrush _selectedEdgeBrush = new SolidBrush(Color.Blue);

        private bool _saveProportions;
        private int _fixedHeight;
        private int _fixedWidth;

        public Viewer(Form form, GraphElementsRepository repository)
        {
            _graphicForm = form;
            _graphicForm.BackColor = _backgroundOutsideColor;
            _fixedHeight = _graphicForm.Height;
            _fixedWidth = _graphicForm.Width;

            _repository = repository;

            _graphicForm.Shown += _graphicForm_Shown;
            _graphicForm.ResizeEnd += _graphicForm_ResizeEnd;

            _repository.OnAddVertex += _repository_OnAddVertex;
            _repository.OnRemoveVertexes += _repository_OnRemoveVertexes;
            _repository.OnAddEdge += _repository_OnAddEdge;
            _repository.OnRemoveEdges += _repository_OnRemoveEdges;

            _repository.OnVertexesSelected += _repository_OnVertexesSelected;
            _repository.OnClearSelectedVertexes += _repository_OnClearSelectedVertexes;
            _repository.OnEdgesSelected += _repository_OnEdgesSelected;
            _repository.OnClearSelectedEdges += _repository_OnClearSelectedEdges;

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

        private void _graphicForm_ResizeEnd(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        //need in optimization
        public void Invalidate()
        {
            _graphics = _graphicForm.CreateGraphics();
            _graphics.Clear(_backgroundLayoutColor);

            //update points
            if (_saveProportions)
            {
                foreach (var vertex in _repository.Vertexes)
                {
                    var newX = (int)((double)vertex.ClientRectangle.Location.X / _fixedWidth * _graphicForm.Width);
                    var newY = (int)((double)vertex.ClientRectangle.Location.Y / _fixedHeight * _graphicForm.Height);
                    vertex.ChangeLocation(newX, newY);
                }

                _fixedWidth = _graphicForm.Width;
                _fixedHeight = _graphicForm.Height;
            }

            //draw edges
            foreach (var edge in _repository.Edges)
            {
                if (_repository.SelectedEdges.Contains(edge))
                {
                    DrawSelectedEdge(edge);
                }
                else
                {
                    DrawSimpleEdge(edge);
                }
            }

            //draw vertexes
            //draw vertex again (look in DrawEdge method)
            foreach (var vertex in _repository.Vertexes)
            {
                if (vertex != _repository.СonnectingVertex)
                {
                    DrawSelectedVertex(vertex);
                }

                if (_repository.SelectedVertexes.Contains(vertex))
                {
                    DrawConnectingVertex(_repository.СonnectingVertex);
                }
                else
                {
                    DrawSimpleVertex(vertex);
                }
            }
        }
    }
}
