using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace Graph.PointsModel
{
    public delegate void VertexArg(Rectangle rectangle);

    public class GraphVertexesRepository
    {     
        public event VertexArg DrawPoint;

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
                var handler = DrawPoint;
                if (handler != null)
                {
                    DrawPoint.Invoke(newVertex.ClientRectangle);
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

        private HashSet<GraphVertex> _vertexesList = new HashSet<GraphVertex>();
        private ObservableCollection<GraphVertex> _selectedVertexes = new ObservableCollection<GraphVertex>();
    }
}
