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
    class Vertex
    {
        const int vertexSize = 10;
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vertex(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ChangeLocation(Point e)
        {
            X -= X - e.X;
            Y -= Y - e.Y;
        }

        public bool isPointClose(Point p)
        {
            double d = (X - p.X) * (X - p.X) + (Y - p.Y) * (Y - p.Y);
            return d < 100;
        }

        public void Draw(Bitmap b)
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                Brush brush = new SolidBrush(Color.DimGray);

                g.FillEllipse(brush, X - vertexSize / 2, Y - vertexSize / 2, vertexSize, vertexSize);

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

        public int Count() => edges.Count;

        public bool GetEdge(Point w, out Edge edge, out Polygon polygon)
        {
            edge = null;
            polygon = this;
            int sum = int.MaxValue;
            foreach(Edge e in edges)
            {
                int A = e.A.Y - e.B.Y;
                int B = e.B.X - e.A.X;
                int C = e.B.Y * e.A.X - e.A.Y * e.B.X;
                double k = Math.Sqrt(A * A + B * B);
                double d = Math.Abs(A * w.X + B * w.Y + C) / k;

                int d1 = (e.A.X - w.X) * (e.A.X - w.X) + (e.A.Y - w.Y) * (e.A.Y - w.Y);
                int d2 = (e.B.X - w.X) * (e.B.X - w.X) + (e.B.Y - w.Y) * (e.B.Y - w.Y);

                int minX, maxX;
                if(e.A.X > e.B.X)
                {
                    maxX = e.A.X;
                    minX = e.B.X;
                }
                else
                {
                    maxX = e.B.X;
                    minX = e.A.X;
                }
                int minY, maxY;
                if (e.A.Y > e.B.Y)
                {
                    maxY = e.A.Y;
                    minY = e.B.Y;
                }
                else
                {
                    maxY = e.B.Y;
                    minY = e.A.Y;
                }

                maxY += 5;
                maxX += 5;
                minX -= 5;
                minY -= 5;

                if (d < 10 && w.X < maxX && w.X > minX && w.Y < maxY && w.Y > minY)
                {
                    
                    if(sum > d1+d2)
                    {
                        sum = d1 + d2;
                        edge = e;
                    }
                }
            }
            return edge != null;
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
