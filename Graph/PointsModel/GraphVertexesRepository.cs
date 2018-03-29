using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graph.PointsModel
{
    public delegate void VertexArg(GraphVertex vertex);

    public class GraphVertexesRepository
    {
        private HashSet<GraphVertex> _vertexesList = new HashSet<GraphVertex>();
        private ObservableCollection<GraphVertex> _selectedVertexes = new ObservableCollection<GraphVertex>();

        public event VertexArg OnDrawVertex;
        public event VertexArg OnRemoveVertex;

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
            var vertexes = _vertexesList.Where(v => v.IsContainsPoint(x, y)).ToList();
            switch (vertexes.Count)
            {
                case 0:
                    return false;
                default:
                    var newVertex = vertexes.First();
                    var oldVertex = _selectedVertexes.FirstOrDefault(v => v.Equals(newVertex));
                    if (oldVertex != null)
                    {
                        _selectedVertexes.Remove(oldVertex);
                    }
                    else
                    {
                        _selectedVertexes.Add(newVertex);
                    }
                    return true;
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
    }
}
