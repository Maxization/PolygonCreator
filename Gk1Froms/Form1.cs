using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gk1Froms
{
    enum Operations
    {
        PolygonAdd,
        VertexMove,
        VertexAdd,
        EdgeMove,
        PolygonMove,
        VertexDelete,
    }
    public partial class Form1 : Form
    {
        Bitmap drawArea;
        List<Polygon> polygons;
        bool drag;
        Vertex vertexP;
        Operations operation = Operations.VertexMove;

        public Form1()
        {
            InitializeComponent();
            polygons = new List<Polygon>();
            drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = drawArea;
            drag = false;
            operation = Operations.PolygonAdd;
        }

        void UpdateArea()
        {
            drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            foreach (Polygon p in polygons)
            {
                p.Draw(drawArea);
            }
            pictureBox1.Image = drawArea;
            pictureBox1.Refresh();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Polygon poly = null;
            Edge edge = null;
            Vertex ver = null;
            if (e.Button == MouseButtons.Left)
            {
                switch (operation)
                {
                    case Operations.PolygonAdd:
                        Vertex p1 = new Vertex(e.X, e.Y);
                        Vertex p2 = new Vertex(e.X, e.Y + 100);
                        Vertex p3 = new Vertex(e.X + 100, e.Y + 100);
                        Vertex p4 = new Vertex(e.X + 100, e.Y);
                        poly = new Polygon(p1, p2, p3, p4);
                        polygons.Add(poly);
                        poly.Draw(drawArea);
                        break;
                    case Operations.VertexMove:
                        if (FindVertex(e.Location, out vertexP, out _))
                        {
                            drag = true;
                        }
                        break;
                    case Operations.VertexDelete:
                        if(FindVertex(e.Location, out ver, out poly))
                        {
                            poly.DeleteVertex(ver);
                            if(poly.Count() == 0)
                            {
                                polygons.Remove(poly);
                            }
                            UpdateArea();
                        }
                        break;
                    case Operations.VertexAdd:
                        if(FindEdge(e.Location, out edge, out poly))
                        {
                            poly.SplitEdge(edge);
                            UpdateArea();
                        }
                        break;
                }
            }
            pictureBox1.Refresh();
        }

        private bool FindEdge(Point w, out Edge e, out Polygon polygon)
        {
            e = null;
            polygon = null;
            foreach(Polygon p in polygons)
            {
                if(p.GetEdge(w, out e, out polygon))
                {
                    return true;
                }
            }
            return false;
        }

        private bool FindVertex(Point w, out Vertex vertex, out Polygon polygon)
        {
            vertex = null;
            polygon = null;
            foreach (Polygon p in polygons)
            {
                if (p.GetVertex(w, out vertex, out polygon))
                {
                    return true;
                }
            }
            return false;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            drag = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (drag)
            {
                vertexP.ChangeLocation(e.Location);
                UpdateArea();
            }
        }

        private void PolygonAdd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if(button.Checked)
            {
                operation = Operations.PolygonAdd;
            }
        }

        private void PolygonMove_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                operation = Operations.PolygonMove;
            }
        }

        private void VertexMove_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                operation = Operations.VertexMove;
            }
        }

        private void VertexDelete_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                operation = Operations.VertexDelete;
            }
        }

        private void VertexAdd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button.Checked)
            {
                operation = Operations.VertexAdd;
            }
        }
    }
}
