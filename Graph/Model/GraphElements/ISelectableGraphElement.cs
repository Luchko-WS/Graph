using System.Collections.Generic;

namespace Graph.Model.Elements
{
    interface ISelectableGraphElement<T> : IEqualityComparer<T>
    {
        bool IsContainsPoint(int x, int y);
    }
}
