using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graph.PointsModel
{
    public delegate void VertexArg(GraphVertex vertex);
    public delegate void EdgeArg(GraphEdge edge);
    public delegate void VertexPairArg(GraphVertex x, GraphVertex y);

    public class GraphElementsRepository
    {
        private HashSet<GraphVertex> _vertexesSet = new HashSet<GraphVertex>();
        private ObservableCollection<GraphVertex> _selectedVertexes = new ObservableCollection<GraphVertex>();
        private GraphVertex _connectingVertex;

        private HashSet<GraphEdge> _edgesSet = new HashSet<GraphEdge>();
        private ObservableCollection<GraphEdge> _selectedEdges = new ObservableCollection<GraphEdge>();

        public event VertexArg OnCreateVertex;
        public event VertexArg OnRemoveVertex;
        public event VertexArg OnSettingSourceVertex;
        public event VertexArg OnRemovingSourceVertex;
        public event VertexPairArg OnCreateEdge;
        public event EdgeArg OnRemoveEdge;

        public IEnumerable<GraphVertex> Vertexes
        {
            get { return _vertexesSet; }
        }

        public ObservableCollection<GraphVertex> SelectedVertexes
        {
            get { return _selectedVertexes; }
        }

        public GraphVertex СonnectingVertex
        {
            get { return _connectingVertex; }
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

            var vertex = _vertexesSet.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
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
            if (!_vertexesSet.Contains(newVertex))
            {
                _vertexesSet.Add(newVertex);
                var handler = OnCreateVertex;
                if (handler != null)
                {
                    handler.Invoke(newVertex);
                }
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
            var vertex = _vertexesSet.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
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
            var edge = _edgesSet.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
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

            foreach (var vertex in _selectedVertexes)
            {
                foreach (var relVertex in vertex.RelativeVertexes)
                {
                    relVertex.RelativeVertexes.Remove(vertex);
                }
                _vertexesSet.Remove(vertex);

                var handler = OnRemoveVertex;
                if (handler != null)
                {
                    handler.Invoke(vertex);
                }
            }

            _selectedVertexes.Clear();
        }

        public void CreateEdge(int x, int y)
        {
            if (_connectingVertex != null)
            {
                var vertex = _vertexesSet.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
                if(vertex != null)
                {
                    _connectingVertex.RelativeVertexes.Add(vertex);
                    vertex.RelativeVertexes.Add(_connectingVertex);
                    _edgesSet.Add(new GraphEdge(_connectingVertex, vertex));

                    var handler = OnCreateEdge;
                    if (handler != null)
                    {
                        handler.Invoke(_connectingVertex, vertex);
                    }
                }
            }
        }

        public void RemoveSelectedEdges()
        {
            foreach (var edge in _selectedEdges)
            {
                _edgesSet.Remove(edge);

                var handler = OnRemoveEdge;
                if (handler != null)
                {
                    handler.Invoke(edge);
                }
            }
            _selectedEdges.Clear();
        }

        public void RemoveSelectedItems()
        {
            RemoveSelectedEdges();
            RemoveSelectedVertexes();
        }
    }
}
