using Graph.Common.GraphElementsRepositoryEventArgs;
using Graph.Model.Elements;
using System.Drawing;

namespace Graph.ViewModel
{
    public partial class Viewer
    {
        private void DrawEdge(GraphEdge edge, Brush brush)
        {
            DrawEdge(edge.Vertex1, edge.Vertex2, brush);
        }

        private void DrawEdge(GraphVertex x, GraphVertex y, Brush brush)
        {
            if (_graphics != null)
            {
                Point A = x.GetCentreOfClientRectangle();
                Point B = y.GetCentreOfClientRectangle();
                _graphics.DrawLine(new Pen(brush, (int)(GraphVertex.Diameter * GraphEdge.WidthCoef)), A, B);

                DrawVertexByState(x);
                DrawVertexByState(y);
            }
        }

        private void DrawSimpleEdge(GraphEdge edge)
        {
            DrawEdge(edge, _simpleEdgeBrush);
        }

        private void DrawSimpleEdge(GraphVertex x, GraphVertex y)
        {
            DrawEdge(x, y, _simpleEdgeBrush);
        }

        private void ClearEdge(GraphEdge edge)
        {
            DrawEdge(edge, _backgroundBrush);
        }

        private void ClearEdge(GraphVertex x, GraphVertex y)
        {
            DrawEdge(x, y, _backgroundBrush);
        }

        private void DrawSelectedEdge(GraphEdge edge)
        {
            DrawEdge(edge, _selectedEdgeBrush);
        }

        private void DrawSelectedEdge(GraphVertex x, GraphVertex y)
        {
            DrawEdge(x, y, _selectedEdgeBrush);
        }

        private void _repository_OnAddEdge(object sender, GraphEdgeEventArgs e)
        {
            DrawSimpleEdge(e.Edge);
        }

        private void _repository_OnRemoveEdges(object sender, GraphEdgeCollectionEventArgs e)
        {
            Invalidate();
        }

        private void _repository_OnEdgesSelected(object sender, GraphEdgeCollectionEventArgs e)
        {
            foreach (var edge in e.Edges)
            {
                DrawSelectedEdge(edge);
            }
        }

        private void _repository_OnClearSelectedEdges(object sender, GraphEdgeCollectionEventArgs e)
        {
            foreach (var edge in e.Edges)
            {
                DrawSimpleEdge(edge);
            }
        }
    }
}
