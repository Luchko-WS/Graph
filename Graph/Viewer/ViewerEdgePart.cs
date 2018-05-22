using Graph.Model.Elements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;

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

        private void SelectedEdges_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var edge = (GraphEdge)item;
                        DrawSelectedEdge(edge);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var edge = (GraphEdge)item;
                        DrawSimpleEdge(edge);
                    }
                    break;
            }
        }

        private void _repository_OnAddEdge(GraphEdge edge)
        {
            DrawSimpleEdge(edge);
        }

        private void _repository_OnRemoveEdges(ICollection<GraphEdge> edgeCollection)
        {
            Invalidate();
        }
    }
}
