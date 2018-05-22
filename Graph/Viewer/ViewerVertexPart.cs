using Graph.Model.Elements;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;

namespace Graph.ViewModel
{
    public partial class Viewer
    {
        private void DrawVertex(GraphVertex vertex, Brush brush)
        {
            if (_graphics != null)
            {
                _graphics.FillEllipse(brush, vertex.ClientRectangle);
            }
        }

        private void DrawSimpleVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _simpleVertexBrush);
        }

        private void ClearVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _backgroundBrush);
        }

        private void DrawConnectingVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _connectingVertexroundBrush);
        }

        private void DrawSelectedVertex(GraphVertex vertex)
        {
            DrawVertex(vertex, _selectedVertexBrush);
        }

        private void DrawVertexByState(GraphVertex vertex)
        {
            if (_repository.SelectedVertexes.Contains(vertex))
            {
                DrawSelectedVertex(vertex);
            }
            else if (_repository.СonnectingVertex == vertex)
            {
                DrawConnectingVertex(vertex);
            }
            else
            {
                DrawSimpleVertex(vertex);
            }
        }

        private void _repository_OnSettingSourceVertex(GraphVertex vertex)
        {
            DrawConnectingVertex(vertex);
        }

        private void _repository_OnRemovingSourceVertex(GraphVertex vertex)
        {
            DrawSimpleVertex(vertex);
        }

        private void SelectedVertexes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var vertex = (GraphVertex)item;
                        DrawSelectedVertex(vertex);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var vertex = (GraphVertex)item;
                        if (vertex != _repository.СonnectingVertex)
                        {
                            DrawSimpleVertex(vertex);
                        }
                        else
                        {
                            DrawConnectingVertex(vertex);
                        }
                    }
                    break;
            }
        }

        private void _repository_OnAddVertex(GraphVertex vertex)
        {
            DrawSimpleVertex(vertex);
        }

        private void _repository_OnRemoveVertexes(ICollection<GraphVertex> vertexCollection)
        {
            Invalidate();
        }
    }
}
