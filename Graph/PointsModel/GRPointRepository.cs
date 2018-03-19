using System.Collections.Generic;
using System.Drawing;

namespace Graph.PointsModel
{
    public delegate void PointOp(int x, int y);

    public class GRPointRepository
    {     
        public event PointOp DrawPoint;

        public IEnumerable<GRPoint> Points
        {
            get
            {
                return _pointsList;
            }
        }

        public void CreateGRPoint(int x, int y)
        {
            var newPoint = new GRPoint(x, y);
            if (!_pointsList.Contains(newPoint))
            {
                _pointsList.Add(newPoint);
                var handler = DrawPoint;
                if (handler != null)
                {
                    DrawPoint.Invoke(x, y);
                }
            }
        }

        private HashSet<GRPoint> _pointsList = new HashSet<GRPoint>(new GPointEqualityComparer());
    }
}
