using System.Collections.Generic;
using System.Drawing;

namespace Graph.Model.Elements
{
    public class GraphVertex : ISelectableGraphElement<GraphVertex>
    {
        private Rectangle _clientRectangle;
        private static readonly int _diameter = 12;
        private HashSet<GraphVertex> _relativeVertexes;

        public GraphVertex(int x, int y)
        {
            _clientRectangle = new Rectangle(x, y, _diameter, _diameter);
            _relativeVertexes = new HashSet<GraphVertex>();
        }

        public Rectangle ClientRectangle
        {
            get { return _clientRectangle; }
        }

        public static int Diameter
        {
            get { return _diameter; }
        }

        public HashSet<GraphVertex> RelativeVertexes
        {
            get { return _relativeVertexes; }
        }

        public void ChangeLocation(int x, int y)
        {
            if (x < 0 || y < 0) return;
            _clientRectangle.Location = new Point(x, y);
        }

        public bool IsContainsPoint(int x, int y)
        {
            return _clientRectangle.Contains(x, y);
        }

        public bool Equals(GraphVertex other)
        {
            return _clientRectangle.X.Equals(other._clientRectangle.X) &&
                _clientRectangle.Y.Equals(other._clientRectangle.Y);
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
