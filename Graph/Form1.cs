using System.Windows.Forms;
using Graph.PointsModel;

namespace Graph
{
    public partial class DrawForm : Form
    {
        GraphVertexesRepository _repository;
        Viewer _pointViewer;
        bool _isDrawingKeyIsPressed = false;

        public DrawForm()
        {
            InitializeComponent();
            _repository = new GraphVertexesRepository();
            _pointViewer = new Viewer(this, _repository);
            _pointViewer.SaveProportions = true;
            this.MouseClick += DrawForm_MouseClick;
            this.SizeChanged += DrawForm_SizeChanged;
            this.KeyDown += DrawForm_KeyDown;
            this.KeyUp += DrawForm_KeyUp;

        }

        private void DrawForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D) _isDrawingKeyIsPressed = true;
        }

        private void DrawForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D) _isDrawingKeyIsPressed = false;
        }

        private void DrawForm_SizeChanged(object sender, System.EventArgs e)
        {
            _pointViewer.Invalidate();
        }

        private void DrawForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (_isDrawingKeyIsPressed)
                {
                    _repository.CreateVertex(e.X, e.Y);
                }
                else
                {
                    _repository.SelectVertex(e.X, e.Y);
                }
            }
        }
    }
}
