using System.Collections.Generic;
using System.Drawing;

namespace Graph.PointsModel
{
    public class GraphVertex : IEqualityComparer<GraphVertex>
    {
        private Rectangle _clientRectangle;
        private int _diameter = 8;
        private HashSet<GraphVertex> _relativeVertexes;

        public GraphVertex(int x, int y)
        {
            _clientRectangle = new Rectangle(x, y, _diameter, _diameter);
        }

        public Rectangle ClientRectangle
        {
            get { return _clientRectangle; }
        }

        public HashSet<GraphVertex> RelativeVertexes
        {
            get { return _relativeVertexes; }
        }

        public void ChangePointLocation(int x, int y)
        {
            if (x < 0 || y < 0) return;
            _clientRectangle.Location = new Point(x, y);
        }

        public bool IsContainsPoint(int x, int y)
        {
            return _clientRectangle.Contains(x, y);
        }

        public bool Equals(GraphVertex x, GraphVertex y)
        {
            return x._clientRectangle.X.Equals(y._clientRectangle.X) &&
                x._clientRectangle.Y.Equals(y._clientRectangle.Y);
        }

        public int GetHashCode(GraphVertex obj)
        {
            return obj._clientRectangle.X ^ obj._clientRectangle.Y;
        }

        public Point GetCentreOfClientRectangle()
        {
            return new Point(_clientRectangle.X + _diameter / 2, _clientRectangle.Y + _diameter / 2);
        }
    }
}
