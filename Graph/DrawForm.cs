using Graph.Model;
using Graph.ViewModel;
using System.Windows.Forms;

namespace Graph
{
    public partial class DrawForm : Form
    {
        GraphElementsRepository _repository;
        Viewer _pointViewer;
        bool _isDrawingKeyIsPressed = false;
        bool _isConnectingKeyIsPressed = false;
        bool _isChoosingConnectingSourceKeyIsPressed = false;
        bool _isCtrlKeyIsPressed = false;

        public DrawForm()
        {
            InitializeComponent();
            _repository = new GraphElementsRepository();
            _pointViewer = new Viewer(this, _repository)
            {
                SaveProportions = true
            };

            this.MouseClick += DrawForm_MouseClick;
            this.KeyDown += DrawForm_KeyDown;
            this.KeyUp += DrawForm_KeyUp;
        }

        private void DrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    if(_isCtrlKeyIsPressed)
                    {
                        _repository.SelectAll();
                    }
                    _isDrawingKeyIsPressed = true;
                    break;
                case Keys.C:
                    _repository.ClearSelecting();
                    break;
                case Keys.R:
                    _isConnectingKeyIsPressed = true;
                    break;
                case Keys.S:
                    _isChoosingConnectingSourceKeyIsPressed = true;
                    break;
                case Keys.Delete:
                    _repository.RemoveSelectedItems();
                    break;
                case Keys.ControlKey:
                    _isCtrlKeyIsPressed = true;
                    break;
            }
        }

        private void DrawForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    _isDrawingKeyIsPressed = false;
                    break;
                case Keys.R:
                    _isConnectingKeyIsPressed = false;
                    break;
                case Keys.S:
                    _isChoosingConnectingSourceKeyIsPressed = false;
                    break;
                case Keys.ControlKey:
                    _isCtrlKeyIsPressed = false;
                    break;
            }
        }

        private void DrawForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isDrawingKeyIsPressed)
                {
                    _repository.CreateVertex(e.X, e.Y);
                }
                else if(_isConnectingKeyIsPressed)
                {
                    _repository.CreateEdge(e.X, e.Y);
                }
                else if(_isChoosingConnectingSourceKeyIsPressed)
                {
                    _repository.SetConnectingVertex(e.X, e.Y);
                }
                else
                {
                    if (_isCtrlKeyIsPressed)
                    {
                        _repository.SelectElement(e.X, e.Y, multipleSelection: true);
                    }
                    else
                    {
                        _repository.SelectElement(e.X, e.Y, multipleSelection: false);
                    }
                }
            }
        }
    }
}
