using Graph.Model;
using GraphViewAdapters.WinForms;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApplication
{
    public partial class DrawForm : Form
    {
        GraphModel _graphModel;
        WinFormsViewAdapter _viewerAdapter;
        bool _drawingKeyIsPressed = false;
        bool _connectingKeyIsPressed = false;
        bool _choosingConnectingSourceKeyIsPressed = false;
        bool _ctrlKeyIsPressed = false;
        bool _moveSelectedVertexesKeyIsPressed = false;
        bool _mergeSelectedVertexesIntoVertexKeyIsPressed = false;
        Point _savedMouseLocation;

        public DrawForm()
        {
            InitializeComponent();
            _graphModel = new GraphModel();
            _viewerAdapter = new WinFormsViewAdapter(this, _graphModel)
            {
                SaveProportions = true
            };

            this.KeyDown += DrawForm_KeyDown;
            this.KeyUp += DrawForm_KeyUp;
            this.MouseDown += DrawForm_MouseDown;
            this.MouseUp += DrawForm_MouseUp;
        }

        private void DrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    if (_ctrlKeyIsPressed)
                    {
                        _graphModel.SelectAll();
                    }
                    _drawingKeyIsPressed = true;
                    break;
                case Keys.R:
                    _connectingKeyIsPressed = true;
                    break;
                case Keys.S:
                    _choosingConnectingSourceKeyIsPressed = true;
                    break;
                case Keys.Delete:
                    _graphModel.RemoveSelectedItems();
                    break;
                case Keys.ControlKey:
                    _ctrlKeyIsPressed = true;
                    break;
                case Keys.M:
                    _moveSelectedVertexesKeyIsPressed = true;
                    break;
                case Keys.U:
                    _mergeSelectedVertexesIntoVertexKeyIsPressed = true;
                    break;
            }
        }

        private void DrawForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    _drawingKeyIsPressed = false;
                    break;
                case Keys.R:
                    _connectingKeyIsPressed = false;
                    break;
                case Keys.S:
                    _choosingConnectingSourceKeyIsPressed = false;
                    break;
                case Keys.ControlKey:
                    _ctrlKeyIsPressed = false;
                    break;
                case Keys.M:
                    _moveSelectedVertexesKeyIsPressed = false;
                    break;
                case Keys.U:
                    _mergeSelectedVertexesIntoVertexKeyIsPressed = false;
                    break;
            }
        }

        private void DrawForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_drawingKeyIsPressed)
                {
                    _graphModel.CreateVertex(e.X, e.Y);
                }
                else if (_connectingKeyIsPressed)
                {
                    _graphModel.CreateEdge(e.X, e.Y);
                }
                else if (_choosingConnectingSourceKeyIsPressed)
                {
                    _graphModel.SetConnectingVertex(e.X, e.Y);
                }
                else if (_moveSelectedVertexesKeyIsPressed)
                {
                    _savedMouseLocation = e.Location;
                }
                else if (_mergeSelectedVertexesIntoVertexKeyIsPressed)
                {
                    _graphModel.MergeSelectedVertexesIntoNewVertex(e.X, e.Y);
                }
                else
                {
                    if (_ctrlKeyIsPressed)
                    {
                        _graphModel.SelectElement(e.X, e.Y, multipleSelection: true);
                    }
                    else
                    {
                        _graphModel.SelectElement(e.X, e.Y, multipleSelection: false);
                    }
                }
            }
        }

        private void DrawForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_moveSelectedVertexesKeyIsPressed)
                {
                    int deltaX = e.Location.X - _savedMouseLocation.X;
                    int deltaY = e.Location.Y - _savedMouseLocation.Y;
                    _graphModel.MoveSelectedVertexes(deltaX, deltaY);
                }
            }
        }
    }
}
