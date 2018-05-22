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

        public bool IsCrossesEdge(GraphEdge edge)
        {
            if (_vertex1.Equals(edge._vertex1) || _vertex1.Equals(edge._vertex2) ||
                _vertex2.Equals(edge._vertex1) || _vertex2.Equals(edge._vertex2))
                return true;

            //Шаг 1. Ввод данных (x1;y1) (x2; y2) (x3;y3) (x4; y4) 
            var p1 = _vertex1.GetCentreOfClientRectangle();
            var p2 = _vertex2.GetCentreOfClientRectangle();
            var p3 = edge._vertex1.GetCentreOfClientRectangle();
            var p4 = edge._vertex2.GetCentreOfClientRectangle();
            //Шаг 2. Если x1 ≥ x2 то  меняем между собой значения x1 и  x2  и y1 и  y2 
            if (p1.X >= p2.X)
            {
                var tmp = p1;
                p1 = p2;
                p2 = tmp;
            }
            //Шаг 3. Если x3 ≥ x4 то  меняем между собой значения x3 и  x4  и y3 и  y4  ; 
            if (p3.X >= p4.X)
            {
                var tmp = p3;
                p3 = p4;
                p4 = tmp;
            }
            /*Шаг 4. Проверяем, равны ли между собой  у2 и у1, 
            если у2 = у1 да, то принимаем  k1 = 0 иначе  
            Определяем угловой коэффициент в уравнении прямой: */
            double k1 = p1.Y == p2.Y ? 0.0 : (p2.Y - p1.Y) / (p2.X - p1.X);
            /*Шаг 5. Проверяем, равны ли между собой  у3 и у4, 
            если у3 = у4 да, то принимаем  k2 = 0 иначе  
            Определяем угловой коэффициент в уравнении прямой: */
            double k2 = p3.Y == p4.Y ? 0.0 : (p4.Y - p3.Y) / (p4.X - p3.X);
            /*Шаг 6.  Проверим отрезки на параллельность. 
            Если k1 = k2 , то прямые параллельны и отрезки пересекаться не могут. Решение задачи прекращаем.*/
            if (k1 == k2) return false;
            /*Шаг 7. Вычисляем значения свободных переменных  
            Определяем свободные члены в уравнении прямой:   */
            double b1 = p1.Y - k1 * p1.X;
            double b2 = p3.Y - k2 * p3.X;
            /*Шаг 8. Решаем систему уравнений:   
            y = k1 x + b1 
            y = k2 x + b2 
            Если прямые имеют точку пересечения, то  
            k1 x + b1 = k2 x + b2 
            Откуда и вычисляем точку пересечения прямых*/
            double x = (b2 - b1) / (k1 - k2);
            double y = k1 * x + b1;
            /*Шаг 9.Учтем, что точка пересечения прямых может лежать вне отрезков, принадлежащих этим прямым. Таким образом, если отрезки пересекаются, то, поскольку x1 ≤ x2; x3 ≤ x4; 
            должны выполняться условия: x1 ≤ x4 и x4 ≤ x2 или x1 ≤ x3 и x3 ≤ x2 
            Если одно из двух условий верно, то отрезки имеют точку пересечения, иначе - отрезки не пересекаются. */
            return (p1.X <= p4.X && p4.X <= p2.X) || (p1.X <= p3.X && p3.X <= p2.X);
        }

        public bool IsContainsPoint(int x, int y)
        {
            GraphVertex leftVertex, rightVertex;

            if (_vertex1.ClientRectangle.X < _vertex2.ClientRectangle.X)
            {
                leftVertex = _vertex1;
                rightVertex = _vertex2;
            }
            else
            {
                leftVertex = _vertex2;
                rightVertex = _vertex1;
            }

            /* we have rectangle like:
             *     [  ]
             *    /  /
             *   /  /
             *  [  ]
             */
            if (leftVertex.ClientRectangle.Y > rightVertex.ClientRectangle.Y)
            {
                if (x < leftVertex.ClientRectangle.Left &&
                   x > rightVertex.ClientRectangle.Right &&
                   y > leftVertex.ClientRectangle.Bottom &&
                   y < rightVertex.ClientRectangle.Top)
                    return false;

                return IsInPoly(x, y, new List<Point>
                {
                    new Point(leftVertex.ClientRectangle.Left, leftVertex.ClientRectangle.Top),
                    new Point(leftVertex.ClientRectangle.Right, leftVertex.ClientRectangle.Bottom),
                    new Point(rightVertex.ClientRectangle.Left, rightVertex.ClientRectangle.Top),
                    new Point(rightVertex.ClientRectangle.Right, rightVertex.ClientRectangle.Bottom)
                });
            }
            /* we have rectangle like:
             * [  ]
             *  \  \
             *   \  \
             *    [  ]
             */
            else
            {
                if (x < rightVertex.ClientRectangle.Left &&
                   x > leftVertex.ClientRectangle.Right &&
                   y > rightVertex.ClientRectangle.Bottom &&
                   y < leftVertex.ClientRectangle.Top)
                    return false;

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

            foreach (var point in points)
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
