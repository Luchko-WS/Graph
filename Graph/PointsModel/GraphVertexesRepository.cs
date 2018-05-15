using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graph.PointsModel
{
    public delegate void VertexArg(GraphVertex vertex);
    public delegate void VertexPairArg(GraphVertex x, GraphVertex y);

    public class GraphVertexesRepository
    {
        private HashSet<GraphVertex> _vertexesSet = new HashSet<GraphVertex>();
        private ObservableCollection<GraphVertex> _selectedVertexes = new ObservableCollection<GraphVertex>();
        private GraphVertex _connectingVertex;

        private HashSet<GraphEdge> _edgesSet = new HashSet<GraphEdge>();
        private ObservableCollection<GraphEdge> _selectedEdges = new ObservableCollection<GraphEdge>();

        public event VertexArg OnDrawVertex;
        public event VertexArg OnRemoveVertex;
        public event VertexArg OnSettingSourceVertex;
        public event VertexArg OnRemovingSourceVertex;
        public event VertexPairArg OnVertexesConnected;

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
                var handler = OnDrawVertex;
                if (handler != null)
                {
                    handler.Invoke(newVertex);
                }
            }
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
            _selectedVertexes.Clear();
        }

        public void RemoveSelectedVertexes()
        {
            if(_selectedVertexes.Contains(_connectingVertex))
            {
                _connectingVertex = null;
            }

            foreach (var item in _selectedVertexes)
            {
                foreach (var relVertex in item.RelativeVertexes)
                {
                    relVertex.RelativeVertexes.Remove(item);
                }
                _vertexesSet.Remove(item);

                var handler = OnRemoveVertex;
                if (handler != null)
                {
                    handler.Invoke(item);
                }
            }

            _selectedVertexes.Clear();
        }

        public void ConnectVertexWith(int x, int y)
        {
            if (_connectingVertex != null)
            {
                var vertex = _vertexesSet.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
                if(vertex != null)
                {
                    _connectingVertex.RelativeVertexes.Add(vertex);
                    vertex.RelativeVertexes.Add(_connectingVertex);
                    var handler = OnVertexesConnected;
                    if (handler != null)
                    {
                        handler.Invoke(_connectingVertex, vertex);
                    }
                }
            }
        }
    }
}
