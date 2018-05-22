using Graph.Model.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graph.Common.GraphElementsRepositoryEventArgs
{
    public class GraphVertexEventArgs : EventArgs
    {
        public GraphVertex Vertex { get; set; }
    }

    public class GraphEdgeEventArgs : EventArgs
    {
        public GraphEdge Edge { get; set; }
    }

    public class GraphVertexCollectionEventArgs : EventArgs
    {
        public ICollection<GraphVertex> Vertexes { get; set; }
    }

    public class GraphEdgeCollectionEventArgs : EventArgs
    {
        public ICollection<GraphEdge> Edges { get; set; }
    }

    public class GraphVertexPairEventArgs : EventArgs
    {
        public GraphVertex X { get; set; }
        public GraphVertex Y { get; set; }
    }
}
