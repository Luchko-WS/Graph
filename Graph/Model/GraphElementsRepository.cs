using Graph.Model.Elements;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Graph.Model
{
    public delegate void VertexArg(GraphVertex vertex);
    public delegate void VertexCollectionArg(ICollection<GraphVertex> vertexCollection);
    public delegate void EdgeArg(GraphEdge edge);
    public delegate void EdgeCollectionArg(ICollection<GraphEdge> edgeCollection);
    public delegate void VertexPairArg(GraphVertex x, GraphVertex y);

    public class GraphElementsRepository
    {
        private List<GraphVertex> _vertexes = new List<GraphVertex>();
        private List<GraphVertex> _selectedVertexes = new List<GraphVertex>();
        private GraphVertex _connectingVertex;

        private List<GraphEdge> _edges = new List<GraphEdge>();
        private List<GraphEdge> _selectedEdges = new List<GraphEdge>();

        public event VertexArg OnSettingSourceVertex;
        public event VertexArg OnRemovingSourceVertex;
        public event VertexCollectionArg OnVertexesSelected;
        public event VertexCollectionArg OnClearSelectedVertexes;
        public event EdgeCollectionArg OnEdgesSelected;
        public event EdgeCollectionArg OnClearSelectedEdges;
        public event VertexArg OnAddVertex;
        public event VertexCollectionArg OnRemoveVertexes;
        public event EdgeArg OnAddEdge;
        public event EdgeCollectionArg OnRemoveEdges;

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
            if(_connectingVertex != null)
            {
                var rmHandler = OnRemovingSourceVertex;
                if (rmHandler != null)
                {
                    rmHandler.Invoke(_connectingVertex);
                }
            }

            var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
            if (vertex != null)
            {
                _connectingVertex = vertex;

                var stHandler = OnSettingSourceVertex;
                if (stHandler != null)
                {
                    stHandler.Invoke(_connectingVertex);
                }
            }
        }

        public void CreateVertex(int x, int y)
        {
            var newVertex = new GraphVertex(x, y);
            if (!_vertexes.Contains(newVertex))
            {
                _vertexes.Add(newVertex);
                var addHandler = OnAddVertex;
                if (addHandler != null)
                {
                    addHandler.Invoke(newVertex);
                }
            }
        }

        public bool SelectElement(int x, int y, bool multipleSelection)
        {
            if(!multipleSelection)
            {
                ClearSelecting();
            }

            if(SelectVertex(x, y, multipleSelection))
            {
                return true;
            }
            if(SelectEdge(x, y, multipleSelection))
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

                        var handler = OnClearSelectedVertexes;
                        if (handler != null)
                        {
                            handler.Invoke(new List<GraphVertex>() { oldVertex });
                        }

                        return true;
                    }
                }
                _selectedVertexes.Add(vertex);
                var handle = OnVertexesSelected;
                if (handle != null)
                {
                    handle.Invoke(new List<GraphVertex>() { vertex });
                }

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

                        var handler = OnClearSelectedEdges;
                        if (handler != null)
                        {
                            handler.Invoke(new List<GraphEdge>() { oldEdge });
                        }

                        return true;
                    }
                }
                
                _selectedEdges.Add(edge);
                var handle = OnEdgesSelected;
                if (handle != null)
                {
                    handle.Invoke(new List<GraphEdge>() { edge });
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClearSelecting()
        {
            if (_selectedVertexes.Any())
            {
                var clsVertexesHandler = OnClearSelectedVertexes;
                if (clsVertexesHandler != null)
                {
                    clsVertexesHandler.Invoke(_selectedVertexes);
                }
                _selectedVertexes.Clear();
            }

            if (_selectedEdges.Any())
            {
                var clsEdgesHandler = OnClearSelectedEdges;
                if (clsEdgesHandler != null)
                {
                    clsEdgesHandler.Invoke(_selectedEdges);
                }
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
                    edgesToRemove.AddRange(_edges.Where(e => e.Vertex1.Equals(vertexToRemove) || e.Vertex2.Equals(vertexToRemove)).ToList());   
                }

                if (edgesToRemove.Any())
                {
                    edgesToRemove = edgesToRemove.Distinct().ToList();
                    for (var i = 0; i < edgesToRemove.Count; i++)
                    {
                        _edges.Remove(edgesToRemove[i]);
                    }

                    var rmEdgesHandler = OnRemoveEdges;
                    if (rmEdgesHandler != null)
                    {
                        rmEdgesHandler.Invoke(edgesToRemove);
                    }
                }

                foreach(var vertex in _selectedVertexes)
                {
                    _vertexes.Remove(vertex);
                }

                var rmHandler = OnRemoveVertexes;
                if (rmHandler != null)
                {
                    rmHandler.Invoke(_selectedVertexes);
                }

                _selectedVertexes.Clear();
            }
        }

        public void CreateEdge(int x, int y)
        {
            if (_connectingVertex != null)
            {
                var vertex = _vertexes.Where(v => v.IsContainsPoint(x, y)).FirstOrDefault();
                if(vertex != null)
                {
                    _connectingVertex.RelativeVertexes.Add(vertex);
                    vertex.RelativeVertexes.Add(_connectingVertex);

                    var newEdge = new GraphEdge(_connectingVertex, vertex);
                    if (!_edges.Contains(newEdge))
                    {
                        _edges.Add(newEdge);
                        var addHandler = OnAddEdge;
                        if (addHandler != null)
                        {
                            addHandler.Invoke(newEdge);
                        }
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

                var rmHandler = OnRemoveEdges;
                if (rmHandler != null)
                {
                    rmHandler.Invoke(_selectedEdges);
                }
                _selectedEdges.Clear();
            }
        }

        public void RemoveSelectedItems()
        {
            RemoveSelectedVertexes();
            RemoveSelectedEdges();          
        }
    }
}
