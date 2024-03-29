﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

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
        PolygonDelete,
        ElipseAdd,
        ElipseMove,
        ElipseSize,
        ElipseDelete,
    }
    public partial class Form1 : Form
    {
        Bitmap drawArea;
        List<Polygon> polygons;
        List<myElipse> elipses;
        bool dragVertex, dragPolygon, dragEdge, startCreating, dragElipse, creatingElipse;
        Point pointFrom;
        myElipse elipseP;
        Vertex vertexP, newVertex, startVertex;
        Polygon polygonP, newPolygon;
        Edge edgeP, newEdge;
        Operations operation = Operations.VertexMove;
        Polygon oldPolygon;
        myElipse oldElipse;
        Point S,R;
        
        

        public Form1()
        {
            InitializeComponent();
            polygons = new List<Polygon>();
            elipses = new List<myElipse>();
            drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = drawArea;
            dragVertex = false;
            dragPolygon = false;
            dragEdge = false;
            startCreating = false;
            dragElipse = false;
            creatingElipse = false;
            operation = Operations.PolygonAdd;
            try
            {
                Deserialize("./../../polygon");
            }
            catch(Exception)
            {
                MessageBox.Show("Cannot read file with polygon", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateArea();
            pictureBox1.Refresh();
        }
        private void Serialize(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write);

            formatter.Serialize(stream, polygons[0]);
            stream.Close();
        }

        private void Deserialize(string filename)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filename, FileMode.Open);

            Polygon p = (Polygon)formatter.Deserialize(stream);
            polygons.Add(p);
            stream.Close();
        }
        void UpdateArea()
        {
            drawArea = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            List<Figure> figureList = new List<Figure>(polygons);
            figureList.AddRange(elipses);

            if(oldPolygon != null)
            {
                oldPolygon.Draw(drawArea);
            }

            if(oldElipse != null)
            {
                oldElipse.Draw(drawArea);
            }

            foreach (Figure f in figureList)
            {
                f.Draw(drawArea);
            }
            pictureBox1.Image = drawArea;
            pictureBox1.Refresh();
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            Polygon poly = null;
            Edge edge = null;
            Vertex ver = null;
            myElipse delEli = null;
            //Serialize("polygon");
            if (e.Button == MouseButtons.Left)
            {
                switch (operation)
                {
                    case Operations.PolygonAdd:
                        if(!startCreating)
                        {
                            startCreating = true;
                            newVertex = new Vertex(e.Location);
                            startVertex = new Vertex(e.Location);
                            newEdge = new Edge(startVertex, newVertex);
                            newPolygon = new Polygon(newEdge);
                            polygons.Add(newPolygon);
                        }
                        else
                        {
                            if (Math.Abs(e.Location.X - startVertex.X) < Vertex.GetSize() + 20 
                                && Math.Abs(e.Location.Y - startVertex.Y) < Vertex.GetSize() + 20 && newPolygon.Count() > 2)
                            {
                                newEdge.B = startVertex;
                                startCreating = false;
                            }
                            else
                            {
                                Vertex v = new Vertex(e.Location);
                                newEdge = new Edge(newVertex, v);
                                newPolygon.AddEdge(newEdge);
                                newVertex = v;
                            }
                        }
                        UpdateArea();
                        break;
                    case Operations.VertexMove:
                        if (FindVertex(e.Location, out vertexP, out polygonP))
                        {
                            oldPolygon = polygonP.Copy();
                            dragVertex = true;
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
                    case Operations.PolygonMove:
                        if(FindPolygon(e.Location, out polygonP))
                        {
                            oldPolygon = polygonP.Copy();
                            pointFrom = e.Location;
                            dragPolygon = true;
                        }
                        break;
                    case Operations.EdgeMove:
                        if(FindEdge(e.Location, out edgeP, out polygonP))
                        {
                            oldPolygon = polygonP.Copy();
                            pointFrom = e.Location;
                            dragEdge = true;
                        }
                        break;
                    case Operations.PolygonDelete:
                        if(FindPolygon(e.Location, out poly))
                        {
                            polygons.Remove(poly);
                            UpdateArea();
                        }
                        break;
                    case Operations.ElipseAdd:
                        S = new Point(e.Location.X, e.Location.Y);
                        R = new Point(e.Location.X, e.Location.Y);
                        elipseP = new myElipse(S, R);
                        elipses.Add(elipseP);
                        creatingElipse = true;
                        break;
                    case Operations.ElipseMove:
                        if(FindElipse(e.Location, out elipseP))
                        {
                            oldElipse = elipseP.Copy();
                            pointFrom = e.Location;
                            dragElipse = true;
                        }
                        break;
                    case Operations.ElipseSize:
                        if(FindElipse(e.Location, out elipseP))
                        {
                            oldElipse = elipseP.Copy();
                            creatingElipse = true;
                        }
                        break;
                    case Operations.ElipseDelete:
                        if(FindElipse(e.Location,out delEli))
                        {
                            elipses.Remove(delEli);
                            UpdateArea();
                        }
                        break;
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                if (dragEdge || dragPolygon || dragVertex || startCreating) return;
                if(FindVertex(e.Location, out vertexP, out polygonP))
                {
                    ContextMenu cm = CreateMenuV();
                    cm.Show(pictureBox1, e.Location);
                }
                else if (FindEdge(e.Location, out edgeP, out polygonP))
                {
                    ContextMenu cm = CreateMenu();
                    cm.Show(pictureBox1, e.Location);
                }
            }
            pictureBox1.Refresh();
        }

        ContextMenu CreateMenuV()
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Add Angle", new EventHandler(Angle_Click));
            cm.MenuItems.Add("Delete Angle", new EventHandler(Delete_ClickVertex));
            return cm;
        }

        ContextMenu CreateMenu()
        {
            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Add("Vertical", new EventHandler(VerticalR_Click));
            cm.MenuItems.Add("Horizontal", new EventHandler(HorizontalR_Click));
            cm.MenuItems.Add("Delete relation", new EventHandler(Delete_Click));
            return cm;
        }

        private void Angle_Click(object sender, EventArgs e)
        {
            double angle = 0;
            Form2 form2 = new Form2(polygonP.GetAngle(vertexP));

            if (form2.ShowDialog(this) == DialogResult.OK)
            {
                angle = double.Parse(form2.GetText());
            }
            else
            {
                return;
            }

            form2.Dispose();
            polygonP.AddRelation(vertexP, RelationType.Angle, angle);
            UpdateArea();
            pictureBox1.Refresh();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            polygonP.DeleteRelation(edgeP);
            UpdateArea();
            pictureBox1.Refresh();
        }

        private void Delete_ClickVertex(object sender, EventArgs e)
        {
            polygonP.DeleteRelation(vertexP);
            UpdateArea();
            pictureBox1.Refresh();
        }

        private void HorizontalR_Click(object sender, EventArgs e)
        {
            polygonP.AddRelation(edgeP, RelationType.Horizontal);
            UpdateArea();
            pictureBox1.Refresh();
        }
        private void VerticalR_Click(object sender, EventArgs e)
        {
            polygonP.AddRelation(edgeP, RelationType.Vertical);
            UpdateArea();
            pictureBox1.Refresh();
        }

        private bool FindPolygon(Point w, out Polygon polygon)
        {
            polygon = null;
            double dsc = double.MaxValue;
            foreach(Polygon p in polygons)
            {
                double res = p.ClosestDistanceToEdges(w);
                if (res < dsc)
                {
                    dsc = res;
                    polygon = p;
                }
            }

            return dsc != double.MaxValue;
        }

        private bool FindEdge(Point w, out Edge e, out Polygon polygon)
        {
            e = null;
            polygon = null;
            double best = double.MaxValue;
            double res;
            Edge bestE = null;
            Polygon bestP = null;
            foreach(Polygon p in polygons)
            {
                res = p.GetEdge(w, out e, out polygon);
                if(best>res)
                {
                    best = res;
                    bestE = e;
                    bestP = polygon;
                }
            }
            e = bestE;
            polygon = bestP;
            return best != double.MaxValue;
        }

        private bool FindElipse(Point w, out myElipse elipse)
        {
            elipse = null;
            foreach (myElipse eli in elipses)
            {
                if (eli.GetElipse(w,out elipse))
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
            dragVertex = false;
            dragPolygon = false;
            dragEdge = false;
            dragElipse = false;
            creatingElipse = false;
            oldPolygon = null;
            oldElipse = null;
            UpdateArea();
            pictureBox1.Refresh();
        }

        private void elipseMove_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.ElipseMove);
        }

        private void sizeElipse_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.ElipseSize);
        }

        private void deleteElipse_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.ElipseDelete);
        }

        private void addElipse_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.ElipseAdd);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            int dx = e.Location.X - pointFrom.X;
            int dy = e.Location.Y - pointFrom.Y;
            pointFrom = e.Location;

            if(startCreating)
            {
                newVertex.ChangeLocation(dx, dy);
                UpdateArea();
            }


            if (dragVertex)
            {
                polygonP.MoveVertex(vertexP, dx, dy);
                UpdateArea();
            }
            if(dragPolygon)
            {
                polygonP.ChangeLocation(dx, dy);
                UpdateArea();
            }
            if(dragEdge)
            {
                polygonP.MoveEdge(edgeP, dx, dy);
                UpdateArea();
            }
            if(creatingElipse)
            {
                elipseP.MoveR(dx, dy);
                UpdateArea();
            }
            if(dragElipse)
            {
                elipseP.Move(dx, dy);
                UpdateArea();
            }
        }

        private void RadioButtonFunc(RadioButton button, Operations opt)
        {
            if (startCreating)
            {
                PolygonAdd.Checked = true;
                return;
            }
            if (button.Checked)
            {
                operation = opt;
            }
        }

        private void PolygonAdd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.PolygonAdd);
        }

        private void PolygonMove_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.PolygonMove);
        }

        private void VertexMove_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.VertexMove);
        }

        private void VertexDelete_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.VertexDelete);
        }

        private void polygonDelete_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.PolygonDelete);
        }

        private void edgeMove_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.EdgeMove);
        }

        private void VertexAdd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonFunc((RadioButton)sender, Operations.VertexAdd);
        }
    }
}
