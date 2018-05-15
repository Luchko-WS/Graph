namespace Graph.PointsModel
{
    public class GraphEdge : ISelectableGraphElement<GraphEdge>
    {
        private readonly GraphVertex _vertex1;
        private readonly GraphVertex _vertex2;
        private bool _hasDirection;

        public GraphEdge(GraphVertex vertex1, GraphVertex vertex2) : this(vertex1, vertex2, false) { }

        public GraphEdge(GraphVertex vertex1, GraphVertex vertex2, bool hasDirection)
        {
            _vertex1 = vertex1;
            _vertex2 = vertex2;
            _hasDirection = hasDirection;
        }

        public GraphVertex Vertex1
        {
            get { return _vertex1; }
        }

        public GraphVertex Vertex2
        {
            get { return _vertex2; }
        }

        public bool Equals(GraphEdge x, GraphEdge y)
        {
            return ((x._vertex1.ClientRectangle.X == y._vertex1.ClientRectangle.X &&
                   x._vertex1.ClientRectangle.Y == y._vertex1.ClientRectangle.Y &&
                   x._vertex2.ClientRectangle.X == y._vertex2.ClientRectangle.X &&
                   x._vertex2.ClientRectangle.X == y._vertex2.ClientRectangle.X) ||
                   (x._vertex1.ClientRectangle.X == y._vertex2.ClientRectangle.X &&
                   x._vertex1.ClientRectangle.Y == y._vertex2.ClientRectangle.Y &&
                   x._vertex2.ClientRectangle.X == y._vertex1.ClientRectangle.X &&
                   x._vertex2.ClientRectangle.X == y._vertex1.ClientRectangle.X));
        }

        public int GetHashCode(GraphEdge obj)
        {
            return obj._vertex1.ClientRectangle.X ^ obj._vertex1.ClientRectangle.Y ^
                   obj._vertex2.ClientRectangle.X ^ obj._vertex2.ClientRectangle.Y;
        }

        public bool IsContainsPoint(int x, int y)
        {
            var topY = ((double)((x - _vertex1.ClientRectangle.Left) * (_vertex2.ClientRectangle.Top - _vertex1.ClientRectangle.Top)) 
                / (_vertex2.ClientRectangle.Left - _vertex1.ClientRectangle.Left)) + _vertex1.ClientRectangle.Top;
            var bottonY = ((double)((x - _vertex1.ClientRectangle.Right) * (_vertex2.ClientRectangle.Bottom - _vertex1.ClientRectangle.Bottom))
                / (_vertex2.ClientRectangle.Right - _vertex1.ClientRectangle.Right)) + _vertex1.ClientRectangle.Bottom;

            int leftX, rightX;
            if (_vertex1.ClientRectangle.X < _vertex2.ClientRectangle.X)
            {
                leftX = _vertex1.ClientRectangle.Right;
                rightX = _vertex2.ClientRectangle.Left;
            }
            else
            {
                leftX = _vertex2.ClientRectangle.Right;
                rightX = _vertex1.ClientRectangle.Left;
            }

            return (y >= topY && y <= bottonY) && (x > leftX && x < rightX);
        }
    }
}
