using Graph.Common.GraphElementsRepositoryEventArgs;
using Graph.Model.Elements;
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

        private void _repository_OnSettingSourceVertex(object sender, GraphVertexEventArgs e)
        {
            DrawConnectingVertex(e.Vertex);
        }

        private void _repository_OnRemovingSourceVertex(object sender, GraphVertexEventArgs e)
        {
            DrawSimpleVertex(e.Vertex);
        }

        private void _repository_OnAddVertex(object sender, GraphVertexEventArgs e)
        {
            DrawSimpleVertex(e.Vertex);
        }

        private void _repository_OnRemoveVertexes(object sender, GraphVertexCollectionEventArgs e)
        {
            Invalidate();
        }

        private void _repository_OnVertexesSelected(object sender, GraphVertexCollectionEventArgs e)
        {
            foreach (var vertex in e.Vertexes)
            {
                DrawSelectedVertex(vertex);
            }
        }

        private void _repository_OnClearSelectedVertexes(object sender, GraphVertexCollectionEventArgs e)
        {
            foreach (var vertex in e.Vertexes)
            {
                if (vertex != _repository.СonnectingVertex)
                {
                    DrawSimpleVertex(vertex);
                }
                else
                {
                    DrawConnectingVertex(vertex);
                }
            }
        }

        //CHANGE THIS!
        private void _repository_OnVertexesLocationChanged(object sender, GraphVertexCollectionEventArgs e)
        {
            Invalidate();
        }

        private void _repository_OnMergeVertexes(object sender, MergeGraphVertexesEventArgs e)
        {
            Invalidate();
        }
    }
}
