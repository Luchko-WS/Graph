using System.Windows.Forms;
using Graph.PointsModel;

namespace Graph
{
    public partial class DrawForm : Form
    {
        GRPointRepository _repository;
        Viewer _pointViewer;

        public DrawForm()
        {
            InitializeComponent();
            _repository = new GRPointRepository();
            _pointViewer = new Viewer(this, _repository);
            _pointViewer.SaveProportions = true;
            this.MouseClick += DrawForm_MouseClick;
            this.SizeChanged += DrawForm_SizeChanged;

        }

        private void DrawForm_SizeChanged(object sender, System.EventArgs e)
        {
            _pointViewer.Invalidate();
        }

        private void DrawForm_MouseClick(object sender, MouseEventArgs e)
        {
            _repository.CreateGRPoint(e.X, e.Y);
        }
    }
}
