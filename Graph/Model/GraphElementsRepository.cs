using Graph.Model.Elements;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graph.Model
{
    public delegate void VertexArg(GraphVertex vertex);
    public delegate void EdgeArg(GraphEdge edge);
    public delegate void VertexPairArg(GraphVertex x, GraphVertex y);

    public class GraphElementsRepository
    {
        private ObservableCollection<GraphVertex> _vertexes = new ObservableCollection<GraphVertex>();
        private ObservableCollection<GraphVertex> _selectedVertexes = new ObservableCollection<GraphVertex>();
        private GraphVertex _connectingVertex;

        private ObservableCollection<GraphEdge> _edges = new ObservableCollection<GraphEdge>();
        private ObservableCollection<GraphEdge> _selectedEdges = new ObservableCollection<GraphEdge>();

        public event VertexArg OnSettingSourceVertex;
        public event VertexArg OnRemovingSourceVertex;

        public ObservableCollection<GraphVertex> Vertexes
        {
            get { return _vertexes; }
        }

        public ObservableCollection<GraphVertex> SelectedVertexes
        {
            get { return _selectedVertexes; }
        }

        public GraphVertex СonnectingVertex
        {
            get { return _connectingVertex; }
        }

        public ObservableCollection<GraphEdge> Edges
        {
            get { return _edges; }
        }

        public ObservableCollection<GraphEdge> SelectedEdges
        {
            get { return _selectedEdges; }
        }

        public void SetConnectingVertex(int x, int y)
        {
            if(_connectingVertex != null)
            {
                var rmHandler = OnRemovingSourceVertex;
                if (rmHandler != null)
                {
                    rmHandler.Invoke(_connectingVertex);
                }
            }

            var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (vertex != null)
            {
                _connectingVertex = vertex;

                var stHandler = OnSettingSourceVertex;
                if (stHandler != null)
                {
                    stHandler.Invoke(_connectingVertex);
                }
            }
        }

        public void CreateVertex(int x, int y)
        {
            var newVertex = new GraphVertex(x, y);
            if (!_vertexes.Contains(newVertex))
            {
                _vertexes.Add(newVertex);
            }
        }

        public bool SelectElement(int x, int y)
        {
            if(SelectVertex(x, y))
            {
                return true;
            }
            if(SelectEdge(x, y))
            {
                return true;
            }
            return false;
        }

        public bool SelectVertex(int x, int y)
        {
            var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (vertex != null)
            {
                var oldVertex = _selectedVertexes.FirstOrDefault(v => v.Equals(vertex));
                if (oldVertex != null)
                {
                    _selectedVertexes.Remove(oldVertex);
                }
                else
                {
                    _selectedVertexes.Add(vertex);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SelectEdge(int x, int y)
        {
            var edge = _edges.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (edge != null)
            {
                var oldEdge = _selectedEdges.FirstOrDefault(v => v.Equals(edge));
                if (oldEdge != null)
                {
                    _selectedEdges.Remove(oldEdge);
                }
                else
                {
                    _selectedEdges.Add(edge);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearSelecting()
        {
            var count = _selectedVertexes.Count;
            for (var i = 0; i < count; i++)
            {
                _selectedVertexes.RemoveAt(0);
            }
            count = _selectedEdges.Count;
            for (var i = 0; i < count; i++)
            {
                _selectedEdges.RemoveAt(0);
            }
        }

        public void RemoveSelectedVertexes()
        {
            if(_selectedVertexes.Contains(_connectingVertex))
            {
                _connectingVertex = null;
            }

            foreach (var vertexToRemove in _selectedVertexes)
            {
                foreach (var relVertex in vertexToRemove.RelativeVertexes)
                {
                    relVertex.RelativeVertexes.Remove(vertexToRemove);
                }

                var edgesToRemove = _edges.Where(e => e.Vertex1.Equals(vertexToRemove) || e.Vertex2.Equals(vertexToRemove)).ToList();
                for(var i = 0; i < edgesToRemove.Count; i++)
                {
                    _edges.Remove(edgesToRemove[i]);
                }

                _vertexes.Remove(vertexToRemove);
            }
            _selectedVertexes.Clear();
        }

        public void CreateEdge(int x, int y)
        {
            if (_connectingVertex != null)
            {
                var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
                if(vertex != null)
                {
                    _connectingVertex.RelativeVertexes.Add(vertex);
                    vertex.RelativeVertexes.Add(_connectingVertex);

                    var newEdge = new GraphEdge(_connectingVertex, vertex);
                    if (!_edges.Contains(newEdge))
                    {
                        _edges.Add(new GraphEdge(_connectingVertex, vertex));
                    }
                }
            }
        }

        public void RemoveSelectedEdges()
        {
            foreach (var edge in _selectedEdges)
            {
                _edges.Remove(edge);
            }
            _selectedEdges.Clear();
        }

        public void RemoveSelectedItems()
        {
            RemoveSelectedVertexes();
            RemoveSelectedEdges();          
        }
    }
}
