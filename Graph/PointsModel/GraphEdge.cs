using System;

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

        public bool Equals(GraphEdge x, GraphEdge y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(GraphEdge obj)
        {
            throw new NotImplementedException();
        }

        public bool IsContainsPoint(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}
