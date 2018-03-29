using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graph.PointsModel
{
    public delegate void VertexArg(GraphVertex vertex);
    public delegate void VertexPairArg(GraphVertex x, GraphVertex y);

    public class GraphVertexesRepository
    {
        private HashSet<GraphVertex> _vertexesList = new HashSet<GraphVertex>();
        private ObservableCollection<GraphVertex> _selectedVertexes = new ObservableCollection<GraphVertex>();

        public event VertexArg OnDrawVertex;
        public event VertexArg OnRemoveVertex;
        public event VertexPairArg OnVertexesConnected;

        public IEnumerable<GraphVertex> Vertexes
        {
            get
            {
                return _vertexesList;
            }
        }

        public ObservableCollection<GraphVertex> SelectedVertexes
        {
            get
            {
                return _selectedVertexes;
            }
        }

        public void CreateVertex(int x, int y)
        {
            var newVertex = new GraphVertex(x, y);
            if (!_vertexesList.Contains(newVertex))
            {
                _vertexesList.Add(newVertex);
                var handler = OnDrawVertex;
                if (handler != null)
                {
                    OnDrawVertex.Invoke(newVertex);
                }
            }
        }

        public bool SelectVertex(int x, int y)
        {
            var vertex = _vertexesList.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
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

        public void ClearSelecting()
        {
            _selectedVertexes.Clear();
        }

        public void RemoveSelectedVertexes()
        {
            foreach (var item in _selectedVertexes)
            {
                _vertexesList.Remove(item);
                var handler = OnRemoveVertex;
                if (handler != null)
                {
                    OnRemoveVertex.Invoke(item);
                }
            }
            _selectedVertexes.Clear();
        }

        public void ConnectVertexWith(int x, int y)
        {
            var sourceVertex = _selectedVertexes.FirstOrDefault();
            if (sourceVertex != null)
            {
                var vertex = _vertexesList.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
                if(vertex != null)
                {
                    sourceVertex.RelativeVertexes.Add(vertex);
                    vertex.RelativeVertexes.Add(sourceVertex);
                    var handler = OnVertexesConnected;
                    if (handler != null)
                    {
                        OnVertexesConnected.Invoke(sourceVertex, vertex);
                    }
                }
            }
        }
    }
}
