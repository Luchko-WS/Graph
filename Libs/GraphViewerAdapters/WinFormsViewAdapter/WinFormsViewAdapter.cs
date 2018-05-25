using Graph.Model;
using System.Drawing;
using System.Windows.Forms;

namespace GraphViewAdapters.WinForms
{
    public partial class WinFormsViewAdapter
    {
        private readonly Form _graphicForm;
        private Graphics _graphics;
        private readonly GraphModel _graphModel;

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

        public WinFormsViewAdapter(Form form, GraphModel graphModel)
        {
            _graphicForm = form;
            _graphicForm.BackColor = _backgroundOutsideColor;
            _fixedHeight = _graphicForm.Height;
            _fixedWidth = _graphicForm.Width;

            _graphModel = graphModel;

            _graphicForm.Shown += _graphicForm_Shown;
            _graphicForm.ResizeEnd += _graphicForm_ResizeEnd;

            _graphModel.OnAddVertex += _repository_OnAddVertex;
            _graphModel.OnRemoveVertexes += _repository_OnRemoveVertexes;
            _graphModel.OnAddEdge += _repository_OnAddEdge;
            _graphModel.OnRemoveEdges += _repository_OnRemoveEdges;

            _graphModel.OnVertexesSelected += _repository_OnVertexesSelected;
            _graphModel.OnClearSelectedVertexes += _repository_OnClearSelectedVertexes;
            _graphModel.OnEdgesSelected += _repository_OnEdgesSelected;
            _graphModel.OnClearSelectedEdges += _repository_OnClearSelectedEdges;

            _graphModel.OnSettingSourceVertex += _repository_OnSettingSourceVertex;
            _graphModel.OnRemovingSourceVertex += _repository_OnRemovingSourceVertex;

            _graphModel.OnVertexesLocationChanged += _repository_OnVertexesLocationChanged;
            _graphModel.OnMergeVertexes += _repository_OnMergeVertexes;
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
            //update points
            if (_saveProportions)
            {
                foreach (var vertex in _graphModel.Vertexes)
                {
                    var newX = (int)((double)vertex.ClientRectangle.Location.X / _fixedWidth * _graphicForm.Width);
                    var newY = (int)((double)vertex.ClientRectangle.Location.Y / _fixedHeight * _graphicForm.Height);
                    vertex.ChangeLocation(newX, newY);
                }

                _fixedWidth = _graphicForm.Width;
                _fixedHeight = _graphicForm.Height;
            }
            Invalidate();
        }

        //need in optimization
        public void Invalidate()
        {
            _graphics = _graphicForm.CreateGraphics();
            _graphics.Clear(_backgroundLayoutColor);

            //draw edges
            foreach (var edge in _graphModel.Edges)
            {
                if (_graphModel.SelectedEdges.Contains(edge))
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
            foreach (var vertex in _graphModel.Vertexes)
            {
                if (_graphModel.SelectedVertexes.Contains(vertex))
                {
                    DrawSelectedVertex(vertex);
                }
                else
                {
                    if (vertex == _graphModel.СonnectingVertex)
                    {
                        DrawConnectingVertex(_graphModel.СonnectingVertex);
                    }
                    else
                    {
                        DrawSimpleVertex(vertex);
                    }
                }
            }
        }
    }
}
