using Graph.Common;
using Graph.Common.GraphElementsRepositoryEventArgs;
using Graph.Model.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph.Model
{
    public class GraphElementsRepository
    {
        private List<GraphVertex> _vertexes = new List<GraphVertex>();
        private List<GraphVertex> _selectedVertexes = new List<GraphVertex>();
        private GraphVertex _connectingVertex;

        private List<GraphEdge> _edges = new List<GraphEdge>();
        private List<GraphEdge> _selectedEdges = new List<GraphEdge>();

        public event EventHandler<GraphVertexEventArgs> OnSettingSourceVertex;
        public event EventHandler<GraphVertexEventArgs> OnRemovingSourceVertex;
        public event EventHandler<GraphVertexCollectionEventArgs> OnVertexesSelected;
        public event EventHandler<GraphVertexCollectionEventArgs> OnClearSelectedVertexes;
        public event EventHandler<GraphEdgeCollectionEventArgs> OnEdgesSelected;
        public event EventHandler<GraphEdgeCollectionEventArgs> OnClearSelectedEdges;
        public event EventHandler<GraphVertexEventArgs> OnAddVertex;
        public event EventHandler<GraphVertexCollectionEventArgs> OnRemoveVertexes;
        public event EventHandler<GraphVertexCollectionEventArgs> OnVertexesLocationChanged;
        public event EventHandler<GraphEdgeEventArgs> OnAddEdge;
        public event EventHandler<GraphEdgeCollectionEventArgs> OnRemoveEdges;
        public event EventHandler<MergeGraphVertexesEventArgs> OnMergeVertexes;

        public List<GraphVertex> Vertexes
        {
            get { return _vertexes; }
        }

        public List<GraphVertex> SelectedVertexes
        {
            get { return _selectedVertexes; }
        }

        public GraphVertex СonnectingVertex
        {
            get { return _connectingVertex; }
        }

        public List<GraphEdge> Edges
        {
            get { return _edges; }
        }

        public List<GraphEdge> SelectedEdges
        {
            get { return _selectedEdges; }
        }

        public void SetConnectingVertex(int x, int y)
        {
            if (_connectingVertex != null)
            {
                EventHelper.Invoke(OnRemovingSourceVertex, this,
                    new GraphVertexEventArgs() { Vertex = _connectingVertex });
            }

            var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (vertex != null)
            {
                _connectingVertex = vertex;
                EventHelper.Invoke(OnSettingSourceVertex, this,
                    new GraphVertexEventArgs() { Vertex = _connectingVertex });
            }
            else
            {
                _connectingVertex = null;
            }
        }

        public void CreateVertex(int x, int y)
        {
            var newVertex = new GraphVertex(x, y);
            if (!_vertexes.Contains(newVertex))
            {
                _vertexes.Add(newVertex);
                EventHelper.Invoke(OnAddVertex, this,
                    new GraphVertexEventArgs() { Vertex = newVertex });
            }
        }

        public bool SelectElement(int x, int y, bool multipleSelection)
        {
            if (!multipleSelection)
            {
                ClearSelecting();
            }

            if (SelectVertex(x, y, multipleSelection))
            {
                return true;
            }

            if (SelectEdge(x, y, multipleSelection))
            {
                return true;
            }

            return false;
        }

        public bool SelectVertex(int x, int y, bool multipleSelection)
        {
            var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (vertex != null)
            {
                if (multipleSelection)
                {
                    var oldVertex = _selectedVertexes.FirstOrDefault(v => v.Equals(vertex));
                    if (oldVertex != null)
                    {
                        _selectedVertexes.Remove(oldVertex);
                        EventHelper.Invoke(OnClearSelectedVertexes, this,
                            new GraphVertexCollectionEventArgs() { Vertexes = new List<GraphVertex>() { oldVertex } });
                        return true;
                    }
                }

                _selectedVertexes.Add(vertex);
                EventHelper.Invoke(OnVertexesSelected, this,
                    new GraphVertexCollectionEventArgs() { Vertexes = new List<GraphVertex>() { vertex } });
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SelectEdge(int x, int y, bool multipleSelection)
        {
            var edge = _edges.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (edge != null)
            {
                if (multipleSelection)
                {
                    var oldEdge = _selectedEdges.FirstOrDefault(v => v.Equals(edge));
                    if (oldEdge != null)
                    {
                        _selectedEdges.Remove(oldEdge);
                        EventHelper.Invoke(OnClearSelectedEdges, this,
                            new GraphEdgeCollectionEventArgs() { Edges = new List<GraphEdge>() { oldEdge } });
                        return true;
                    }
                }

                _selectedEdges.Add(edge);
                EventHelper.Invoke(OnEdgesSelected, this,
                    new GraphEdgeCollectionEventArgs() { Edges = new List<GraphEdge>() { edge } });
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SelectAll()
        {
            if (_vertexes.Any())
            {
                _selectedVertexes = new List<GraphVertex>(_vertexes);
                EventHelper.Invoke(OnVertexesSelected, this,
                        new GraphVertexCollectionEventArgs() { Vertexes = _selectedVertexes });
            }
            if (_edges.Any())
            {
                _selectedEdges = new List<GraphEdge>(_edges);
                EventHelper.Invoke(OnEdgesSelected, this,
                        new GraphEdgeCollectionEventArgs() { Edges = _edges });
            }
        }

        public void ClearSelecting()
        {
            if (_selectedVertexes.Any())
            {
                EventHelper.Invoke(OnClearSelectedVertexes, this,
                    new GraphVertexCollectionEventArgs() { Vertexes = _selectedVertexes });
                _selectedVertexes.Clear();
            }

            if (_selectedEdges.Any())
            {
                EventHelper.Invoke(OnClearSelectedEdges, this,
                    new GraphEdgeCollectionEventArgs() { Edges = _selectedEdges });
                _selectedEdges.Clear();
            }
        }

        public void RemoveSelectedVertexes()
        {
            if (_selectedVertexes.Any())
            {
                if (_selectedVertexes.Contains(_connectingVertex))
                {
                    _connectingVertex = null;
                }

                List<GraphEdge> edgesToRemove = new List<GraphEdge>();

                foreach (var vertexToRemove in _selectedVertexes)
                {
                    foreach (var relVertex in vertexToRemove.RelativeVertexes)
                    {
                        relVertex.RelativeVertexes.Remove(vertexToRemove);
                    }
                    edgesToRemove.AddRange(_edges
                        .Where(e => e.Vertex1.Equals(vertexToRemove) || e.Vertex2.Equals(vertexToRemove))
                        .ToList());
                }

                if (edgesToRemove.Any())
                {
                    edgesToRemove = edgesToRemove.Distinct().ToList();
                    for (var i = 0; i < edgesToRemove.Count; i++)
                    {
                        _edges.Remove(edgesToRemove[i]);
                    }
                    EventHelper.Invoke(OnRemoveEdges, this,
                        new GraphEdgeCollectionEventArgs() { Edges = edgesToRemove });
                }

                foreach (var vertex in _selectedVertexes)
                {
                    _vertexes.Remove(vertex);
                }
                EventHelper.Invoke(OnRemoveVertexes, this,
                    new GraphVertexCollectionEventArgs() { Vertexes = _selectedVertexes });
                _selectedVertexes.Clear();
            }
        }

        public void CreateEdge(int x, int y)
        {
            if (_connectingVertex != null)
            {
                var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
                if (vertex != null)
                {
                    _connectingVertex.RelativeVertexes.Add(vertex);
                    vertex.RelativeVertexes.Add(_connectingVertex);

                    var newEdge = new GraphEdge(_connectingVertex, vertex);
                    if (!_edges.Contains(newEdge))
                    {
                        _edges.Add(newEdge);
                        EventHelper.Invoke(OnAddEdge, this,
                            new GraphEdgeEventArgs() { Edge = newEdge });
                    }
                }
            }
        }

        public void RemoveSelectedEdges()
        {
            if (_selectedEdges.Any())
            {
                foreach (var edge in _selectedEdges)
                {
                    _edges.Remove(edge);
                }

                EventHelper.Invoke(OnRemoveEdges, this,
                    new GraphEdgeCollectionEventArgs() { Edges = _selectedEdges });
                _selectedEdges.Clear();
            }
        }

        public void RemoveSelectedItems()
        {
            RemoveSelectedVertexes();
            RemoveSelectedEdges();
        }

        public void MoveSelectedVertexes(int deltaX, int deltaY)
        {
            if (_selectedVertexes.Any())
            {
                foreach (var vertex in _selectedVertexes)
                {
                    int x = vertex.ClientRectangle.X + deltaX >= 0 ? vertex.ClientRectangle.X + deltaX : 0;
                    int y = vertex.ClientRectangle.Y + deltaY >= 0 ? vertex.ClientRectangle.Y + deltaY : 0;
                    vertex.ChangeLocation(x, y);
                }
                EventHelper.Invoke(OnVertexesLocationChanged, this,
                    new GraphVertexCollectionEventArgs() { Vertexes = _selectedVertexes });
            }
        }

        public void MergeSelectedVertexesIntoNewVertex(int x, int y)
        {
            _selectedEdges.Clear();
            var sourceVertexes = new List<GraphVertex>(_selectedVertexes);

            GraphVertex newVertex = new GraphVertex(x, y);

            foreach (var vertex in sourceVertexes)
            {
                foreach (var relVertex in vertex.RelativeVertexes)
                {
                    newVertex.RelativeVertexes.Add(relVertex);
                }
            }

            foreach (var vertexToRemove in sourceVertexes)
            {
                _vertexes.Remove(vertexToRemove);
            }

            var edgesToRemove = _edges
                        .Where(e => sourceVertexes
                            .Any(v => v.Equals(e.Vertex1)) && sourceVertexes.Any(v => v.Equals(e.Vertex2)))
                        .ToList();
            foreach(var edgeToRemove in edgesToRemove)
            {
                _edges.Remove(edgeToRemove);
            }

            var relEdges = _edges
                        .Where(e => sourceVertexes
                            .Any(v => v.Equals(e.Vertex1)) || sourceVertexes.Any(v => v.Equals(e.Vertex2)))
                        .ToList();

            foreach(var edgeToRemove in relEdges)
            {
                _edges.Remove(edgeToRemove);
            }

            //create new edges
            foreach(var relEdge in relEdges)
            {
                if (sourceVertexes.Any(v => v.Equals(relEdge.Vertex1)))
                {
                    relEdge.Vertex1 = newVertex;
                }
                else
                {
                    relEdge.Vertex2 = newVertex;
                }
            }
            relEdges = relEdges.Distinct().ToList();
            _edges.AddRange(relEdges);

            _vertexes.Add(newVertex);
            _selectedVertexes.Clear();
            _selectedVertexes.Add(newVertex);

            EventHelper.Invoke(OnMergeVertexes, this, 
                new MergeGraphVertexesEventArgs() { NewVertex = newVertex, Source = sourceVertexes });
        }
    }
}
