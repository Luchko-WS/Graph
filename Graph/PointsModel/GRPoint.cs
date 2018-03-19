using System.Collections.Generic;

namespace Graph.PointsModel
{
    public class GRPoint
    {
        public GRPoint(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get { return _x; }
        }
        
        public int Y
        {
            get { return _y; }
        }

        private int _x;
        private int _y;
    }

    class GPointEqualityComparer : IEqualityComparer<GRPoint>
    {
        public bool Equals(GRPoint x, GRPoint y)
        {
            return x.X == y.X && x.Y == y.Y;
        }

        public int GetHashCode(GRPoint obj)
        {
            return obj.X ^ obj.Y;
        }
    }
}
