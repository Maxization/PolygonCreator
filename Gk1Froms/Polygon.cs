using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gk1Froms
{
    enum RelationType
    {
        Vertical, //pozioma
        Horizontal,
    }
    class Relation
    {
        Edge edge1;
        RelationType type;
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

                //Implement DrawLine
                g.DrawLine(pen, A, B);
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

        public Polygon(params Vertex[] vertices)
        {
            edges = new List<Edge>();
            
            for(int i=1;i<vertices.Length;i++)
            {
                edges.Add(new Edge(vertices[i - 1], vertices[i]));
            }
            edges.Add(new Edge(vertices[vertices.Length - 1], vertices[0]));
        }

        public Polygon(params Edge[] edges)
        {
            this.edges = new List<Edge>(edges); 
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

        public void MoveEdge(Edge e)
        {
            if (!edges.Contains(e)) return;
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
            edges.Remove(e);
        }

        public void DeleteVertex(Vertex v)
        {
            if(Count() == 3)
            {
                edges.Clear();
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
            edges.Remove(e1);
            edges.Remove(e2);
            edges.Add(new Edge(ev1, ev2));
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
