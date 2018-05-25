using System;
using System.Collections.Generic;

namespace Graph.Model.Elements
{
    interface ISelectableGraphElement<T> : IEquatable<T>
    {
        bool IsContainsPoint(int x, int y);
    }
}
