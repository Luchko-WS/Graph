using System.Collections.Generic;
using System.Drawing;

namespace Graph.Model.Elements
{
    public class GraphEdge : ISelectableGraphElement<GraphEdge>
    {
        private readonly GraphVertex _vertex1;
        private readonly GraphVertex _vertex2;
        private bool _hasDirection;
        private static readonly double _widthCoef = 0.6;

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

        public static double WidthCoef
        {
            get { return _widthCoef; }
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
            GraphVertex leftVertex, rightVertex;

            if(_vertex1.ClientRectangle.X < _vertex2.ClientRectangle.X)
            {
                leftVertex = _vertex1;
                rightVertex = _vertex2;
            }
            else
            {
                leftVertex = _vertex2;
                rightVertex = _vertex1;
            }

            if(leftVertex.ClientRectangle.Y > rightVertex.ClientRectangle.Y)
            {
                return IsInPoly(x, y, new List<Point>
                {
                    new Point(leftVertex.ClientRectangle.Left, leftVertex.ClientRectangle.Top),
                    new Point(leftVertex.ClientRectangle.Right, leftVertex.ClientRectangle.Bottom),
                    new Point(rightVertex.ClientRectangle.Left, rightVertex.ClientRectangle.Top),
                    new Point(rightVertex.ClientRectangle.Right, rightVertex.ClientRectangle.Bottom)
                });
            }
            else
            {
                return IsInPoly(x, y, new List<Point>
                {
                    new Point(leftVertex.ClientRectangle.Right, leftVertex.ClientRectangle.Top),
                    new Point(leftVertex.ClientRectangle.Left, leftVertex.ClientRectangle.Bottom),
                    new Point(rightVertex.ClientRectangle.Right, rightVertex.ClientRectangle.Top),
                    new Point(rightVertex.ClientRectangle.Left, rightVertex.ClientRectangle.Bottom)
                });
            }
        }

        private bool IsInPoly(int x, int y, ICollection<Point> points)
        {
            var npol = points.Count;
            List<int> xp = new List<int>();
            List<int> yp = new List<int>();

            foreach(var point in points)
            {
                xp.Add(point.X);
                yp.Add(point.Y);
            }

            var j = npol - 1;
            bool c = false;
            for (var i = 0; i < npol; i++)
            {
                if ((((yp[i] <= y) && (y < yp[j])) || ((yp[j] <= y) && (y < yp[i]))) &&
                    (x > (xp[j] - xp[i]) * (y - yp[i]) / (yp[j] - yp[i]) + xp[i]))
                {
                    c = !c;
                }
                j = i;
            }
            return c;
        }
    }
}
