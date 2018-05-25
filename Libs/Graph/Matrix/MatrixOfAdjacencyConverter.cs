using Graph.Model;
using Graph.Model.Elements;
using System;
using System.Collections.Generic;

namespace Graph.Matrix
{
    public class MatrixOfAdjacency
    {
        private Dictionary<GraphVertex, int> _indexesMap;
        private int[,] _matrix;

        public MatrixOfAdjacency(GraphElementsRepository graph)
        {
            _indexesMap = new Dictionary<GraphVertex, int>();
            int index = 0;
            foreach (var vertex in graph.Vertexes)
            {
                _indexesMap.Add(vertex, index++);
            }

            _matrix = new int[graph.Vertexes.Count, graph.Vertexes.Count];
            try
            {
                foreach (var edge in graph.Edges)
                {
                    int firstVertexIndex = _indexesMap[edge.Vertex1];
                    int secondVertexIndex = _indexesMap[edge.Vertex2];

                    if (!edge.HasDirection)
                    {
                        _matrix[secondVertexIndex, firstVertexIndex]++;
                    }
                    _matrix[firstVertexIndex, secondVertexIndex]++;
                }
            }
            catch
            {
                throw new ArgumentException("Can't find one of vertexes for edge in Vertexes collection");
            }
        }

        public int[,] Matrix
        {
            get { return _matrix; }
        }
    }
}
