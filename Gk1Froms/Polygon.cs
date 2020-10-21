using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gk1Froms
{
    enum RelationType
    {
        Vertical,
        Horizontal, //pozioma
        Angle,
    }

    abstract class Relation
    {
        //Naprawia relacje i zwraca zmieniony wierzcholek
        public PictureBox test;
        public abstract Vertex[] Fix(Vertex v, ref int dx, ref int dy);
        public abstract bool ContainVertex(Vertex v);
    }
    class HorizontalRelation : Relation
    {
        public Edge Edge { get; set; }
        public HorizontalRelation(Edge e)
        {
            Edge = e;
        }

        public override Vertex[] Fix(Vertex v, ref int dx, ref int dy)
        {
            Vertex k = v == Edge.A ? Edge.B : Edge.A;
            k.Y = v.Y;

            return new Vertex[] { k };
        }

        public override bool ContainVertex(Vertex v)
        {
            return Edge.A == v || Edge.B == v;
        }
    }

    class VerticalRelation : Relation
    {
        public Edge Edge { get; set; }

        public VerticalRelation(Edge e)
        {
            Edge = e;
        }
        public override bool ContainVertex(Vertex v)
        {
            return Edge.A == v || Edge.B == v;
        }

        public override Vertex[] Fix(Vertex v, ref int dx, ref int dy)
        {
            Vertex k = v == Edge.A ? Edge.B : Edge.A;
            k.X = v.X;
            return new Vertex[] { k };
        }
    }

    class AngleRelation: Relation
    {
        double A1, A2;
        Edge edge1, edge2;
        public AngleRelation(Edge e,Edge e2)
        {
            edge1 = e;
            edge2 = e2;

            Vertex w = edge1.A == edge2.A || edge1.A == edge2.B ? edge1.A : edge1.B;
            Vertex v1 = edge1.A == w ? edge1.B : edge1.A;
            Vertex v2 = edge2.A == w ? edge2.B : edge2.A;

            A1 = (double)(w.Y - v1.Y) / (double)(w.X - v1.X);
            A2 = (double)(w.Y - v2.Y) / (double)(w.X - v2.X);

        }

        public override Vertex[] Fix(Vertex v, ref int dx, ref int dy)
        {
            Vertex w = edge1.A == edge2.A || edge1.A == edge2.B ? edge1.A : edge1.B;
            Vertex v1 = edge1.A == w ? edge1.B : edge1.A;
            Vertex v2 = edge2.A == w ? edge2.B : edge2.A;

            if( w != v)
            {
                //v1 ------ w ==== A1
                //v2 ------ w ==== A2
                double B1 = v1.Y - A1 * v1.X;
                double B2 = v2.Y - A2 * v2.X;
                if (v == v1)
                {
                    double newX = (B2 + dy - B1 - A2 * dx) / (A1 - A2);
                    double newY = A1 * newX + B1;
                    w.X = (int)(newX);
                    w.Y = (int)(newY);

                    //using (Graphics g = Graphics.FromImage(test.Image))
                    //{
                    //    Pen pen = new Pen(Color.Red);
                    //    g.DrawLine(pen, new Point(w.X - 50, (int)(A2 * (w.X - 50) + B2)), new Point(w.X + 50, (int)(A2 * (w.X + 50) + B2)));
                    //    g.DrawLine(pen, new Point(w.X - 50, (int)(A1 * (w.X - 50) + B1)), new Point(w.X + 50, (int)(A1 * (w.X + 50) + B1)));
                    //    //g.DrawLine(pen, oldLoc, w);
                    //    //g.DrawLine(pen, w, v2);
                    //    test.Refresh();
                    //}  
                }
                else
                {
                    double newX = (B1 + dy - B2 - A1 * dx) / (A2 - A1);
                    double newY = A1 * newX + B1;
                    w.X = (int)(newX);
                    w.Y = (int)(newY);
                }       
            }
            else
            {
                v1.X += dx;
                v1.Y += dy;

                v2.X += dx;
                v2.Y += dy;

                return new Vertex[] { v1, v2 };
            }
            return new Vertex[] { w };
        }

        public override bool ContainVertex(Vertex v)
        {
            return edge1.A == v || edge1.B == v || edge2.A == v || edge2.B == v;
        }
    }
    class Vertex
    {
        const int VERTEX_SIZE = 10;
        public int X { get; set; }
        public int Y { get; set; }

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vertex(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public static int GetSize() => VERTEX_SIZE;

        public void ChangeLocation(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public int SquareDistanceFromPoint(Point p)
        {
            return (X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y);
        }
        public bool isPointClose(Point p)
        {
            double d = SquareDistanceFromPoint(p);
            return d < 100;
        }

        public void Draw(Bitmap b)
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                Brush brush = new SolidBrush(Color.DimGray);

                g.FillEllipse(brush, X - VERTEX_SIZE / 2, Y - VERTEX_SIZE / 2, VERTEX_SIZE, VERTEX_SIZE);

                brush.Dispose();
            }
        }

        public static implicit operator Point(Vertex v) => new Point(v.X,v.Y);
    }

    class Edge
    {
        public Vertex A { get; set; }
        public Vertex B { get; set; }

        public Edge(Vertex a, Vertex b)
        {
            A = a;
            B = b;
        }

        public bool GetVertex(Point w, out Vertex vertex)
        {
            vertex = null;
            if(A.isPointClose(w))
            {
                vertex = A;
                return true;
            }
            if(B.isPointClose(w))
            {
                vertex = B;
                return true;
            }
            return false;
        }

        public void ChangeLocation(int dx, int dy)
        {
            A.ChangeLocation(dx, dy);
            B.ChangeLocation(dx, dy);
        }

        public bool ContainAndGet(Vertex v,out Vertex s)
        {
            s = null;
            if (v == A)
            {
                s = B;
                return true;
            }
            if(v == B)
            {
                s = A;
                return true;
            }
            return false;
        }

        public void Draw(Bitmap b)
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                Pen pen = new Pen(Color.Black, 2);
                BresenhamLineAlgorithm.DrawLine(b, A, B);
                A.Draw(b);
                B.Draw(b);

                pen.Dispose();
            }
        }
    }

    class Polygon
    {
        //How far detect click on edge
        const int EDGE_DISTANCE = 5;
        List<Edge> edges;

        public PictureBox test;

        List<Relation> relations;

        public Polygon(params Vertex[] vertices)
        {
            edges = new List<Edge>();
            relations = new List<Relation>();

            for(int i=1;i<vertices.Length;i++)
            {
                edges.Add(new Edge(vertices[i - 1], vertices[i]));
            }
            edges.Add(new Edge(vertices[vertices.Length - 1], vertices[0]));
        }

        public double GetAngle(Vertex v)
        {
            List<Edge> ed = new List<Edge>(); 
            foreach(Edge e in edges)
            {
                if(e.A == v || e.B == v)
                {
                    ed.Add(e);
                }
            }

            Vertex v1 = ed[0].A == v ? ed[0].B : ed[0].A;
            Vertex v2 = ed[1].A == v ? ed[1].B : ed[1].A;

            Point w1 = new Point(v1.X - v.X, v1.Y - v.Y);
            Point w2 = new Point(v2.X - v.X, v2.Y - v.Y);

            double cosinus = (w1.X * w2.X + w1.Y * w2.Y) / (Math.Sqrt(w1.X * w1.X + w1.Y * w1.Y) * Math.Sqrt(w2.X * w2.X + w2.Y * w2.Y));
            return Math.Acos(cosinus) * (180f / Math.PI);
        }
        public Polygon(params Edge[] edges)
        {
            this.edges = new List<Edge>(edges);
            relations = new List<Relation>();
        }

        public void AddEdge(Edge e)
        {
            edges.Add(e);
        }
        public void ChangeLocation(int dx, int dy)
        {
            HashSet<Vertex> vertices = new HashSet<Vertex>();
            foreach(Edge e in edges)
            {
                vertices.Add(e.A);
                vertices.Add(e.B);
            }

            foreach(Vertex v in vertices)
            {
                v.ChangeLocation(dx, dy);
            }
        }

        public void MoveEdge(Edge e, int dx, int dy)
        {
            if (!edges.Contains(e)) return;
            e.A.ChangeLocation(dx, dy);
            e.B.ChangeLocation(dx, dy);
            FixRelation(e.A, relations, dx, dy);
            FixRelation(e.B, relations, dx, dy);
        }

        public void MoveVertex(Vertex v, int dx, int dy)
        {
            if (!ContainsVertex(v)) return;
            v.ChangeLocation(dx, dy);
            FixRelation(v, relations, dx, dy);
        }

        public void FixRelation(Vertex v, List<Relation> relations, int dx, int dy)
        {
            if (relations.Count == 0) return;
            List<Relation> vertexRel = GetVertexRelations(v, relations);

            foreach(Relation r in vertexRel)
            {
                r.test = this.test;
                Vertex[] changedV = r.Fix(v, ref dx, ref dy);
                List<Relation> changedVrel = new List<Relation>(vertexRel);
                changedVrel.Remove(r);
                foreach(Vertex k in changedV)
                {
                    FixRelation(k, changedVrel, dx, dy);
                }
                
            }
        }

        public bool CanAddRelation(Edge e, RelationType r)
        {
            List<Relation> list = GetEdgeRelations(e, relations);
            foreach(Relation rel in list)
            {
                if (rel.ContainVertex(e.A) && rel.ContainVertex(e.B)) return false;
                if(r == RelationType.Horizontal && rel is HorizontalRelation)
                {
                     return false;
                }
                else if ( r == RelationType.Vertical && rel is VerticalRelation)
                {
                    return false;
                }
            }

            return true;
        }

        public List<Relation> GetEdgeRelations(Edge e, List<Relation> rels)
        {
            List<Relation> res = new List<Relation>();
            foreach(Relation r in rels)
            {
                if (r.ContainVertex(e.A) || r.ContainVertex(e.B))
                    res.Add(r);
            }
            return res;
        }

        public List<Relation> GetVertexRelations(Vertex v, List<Relation> rels)
        {
            List<Relation> res = new List<Relation>();
            foreach (Relation r in rels)
            {
                if (r.ContainVertex(v))
                    res.Add(r);
            }
            return res;
        }

        public void AddRelation(Edge e, RelationType r)
        {
            if(!CanAddRelation(e,r))
            {
                MessageBox.Show("Cannot add relation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Relation rel = null;
            switch(r)
            {
                case RelationType.Horizontal:
                    e.B.Y = e.A.Y;
                    rel = new HorizontalRelation(e);
                    break;
                case RelationType.Vertical:
                    e.B.X = e.A.X;
                    rel = new VerticalRelation(e);
                    break;
            }
            relations.Add(rel);
        }

        public void AddRelation(Vertex v, RelationType r, double angle)
        {
            List<Edge> e2 = new List<Edge>();
            foreach (Edge e in edges)
            {
                if (e.A == v || e.B == v) e2.Add(e);
            }
            if (!CanAddRelation(e2[0], r) || !CanAddRelation(e2[1], r)) return;

            Vertex v2 = e2[1].A == v ? e2[1].B : e2[1].A;
            Point w2 = new Point(v2.X - v.X, v2.Y - v.Y);

            double prevAngle = GetAngle(v);

            double dAngle =  angle - prevAngle;

            double radians = (Math.PI / 180f) * dAngle;

            Point newW = new Point((int)(w2.X * Math.Cos(radians) - w2.Y * Math.Sin(radians)), (int)(w2.X * Math.Sin(radians) + w2.Y * Math.Cos(radians)));
            v2.X = newW.X + v.X;
            v2.Y = newW.Y + v.Y;

            AngleRelation angleR = new AngleRelation(e2[0], e2[1]);
            relations.Add(angleR);
        }

        public bool ContainsVertex(Vertex v)
        {
            foreach(Edge e in edges)
            {
                if(e.A == v || e.B == v)
                {
                    return true;
                }
            }
            return false;
        }

        public int Count() => edges.Count;

        public double ClosestDistanceToEdges(Point p)
        {
            double res = double.MaxValue;
            foreach(Edge e in edges)
            {
                double dsc = DistanceFromPointToEdge(p, e);
                int minX = Math.Min(e.A.X, e.B.X);
                int maxX = Math.Max(e.A.X, e.B.X);

                minX -= EDGE_DISTANCE;
                maxX += EDGE_DISTANCE;

                if(p.X > minX && p.X < maxX)
                {
                    if (dsc < res)
                    {
                        res = dsc;
                    }
                }

            }

            return res;
        }

        public double DistanceFromPointToEdge(Point p, Edge e)
        {
            int A = e.A.Y - e.B.Y;
            int B = e.B.X - e.A.X;
            int C = e.B.Y * e.A.X - e.A.Y * e.B.X;
            double k = Math.Sqrt(A * A + B * B);
            return Math.Abs(A * p.X + B * p.Y + C) / k;
        }

        public double GetEdge(Point w, out Edge edge, out Polygon polygon)
        {
            edge = null;
            polygon = this;
            double res = double.MaxValue;
            foreach(Edge e in edges)
            {

                int minX = Math.Min(e.A.X, e.B.X);
                int maxX = Math.Max(e.A.X, e.B.X);

                int minY = Math.Min(e.A.Y, e.B.Y);
                int maxY = Math.Max(e.A.Y, e.B.Y);

                maxY += EDGE_DISTANCE;
                maxX += EDGE_DISTANCE;
                minX -= EDGE_DISTANCE;
                minY -= EDGE_DISTANCE;

                double d = DistanceFromPointToEdge(w, e);
                if (d < 10 && w.X < maxX && w.X > minX && w.Y < maxY && w.Y > minY)
                {
                    edge = e;
                    res = d;
                }
            }
            return res;
        }

        public bool GetVertex(Point w,out Vertex vertex, out Polygon polygon)
        {
            vertex = null;
            polygon = null;
            foreach(Edge e in edges)
            {
                if(e.GetVertex(w, out vertex))
                {
                    polygon = this;
                    return true;
                }
            }
            return false;
        }

        public void SplitEdge(Edge e)
        {
            int Sx = (e.A.X + e.B.X) / 2;
            int Sy = (e.A.Y + e.B.Y) / 2;
            Vertex s = new Vertex(Sx, Sy);
            edges.Add(new Edge(e.A, s));
            edges.Add(new Edge(s, e.B));
            DeleteRelation(e);
            edges.Remove(e);
        }

        public void DeleteVertex(Vertex v)
        {
            if(Count() == 3)
            {
                edges.Clear();
                relations.Clear();
                return;
            }

            Vertex v2 = null, ev1 = null, ev2 = null;
            Edge e1 = null, e2 = null;
            for(int i=0;i<edges.Count;i++)
            {
                if(edges[i].ContainAndGet(v, out v2))
                {
                    if(ev1 != null)
                    {
                        ev2 = v2;
                        e2 = edges[i];
                        break;
                    }
                    else
                    {
                        ev1 = v2;
                        e1 = edges[i];
                    }
                }
            }
            DeleteRelation(e1);
            DeleteRelation(e2);
            edges.Remove(e1);
            edges.Remove(e2);
            edges.Add(new Edge(ev1, ev2));
        }

        public void DeleteRelation(Edge e)
        {
            Relation res = relations.Find((Relation r) => r.ContainVertex(e.A) && r.ContainVertex(e.B));
            if(res!=null)
            {
                relations.Remove(res);
            }    
        }

        public void Draw(Bitmap b)
        {
            foreach(Edge e in edges)
            {
                e.Draw(b);
            }
        }
    }
}
