using System.Collections.Generic;

namespace Graph.PointsModel
{
    interface ISelectableGraphElement<T> : IEqualityComparer<T>
    {
        bool IsContainsPoint(int x, int y);
    }
}
