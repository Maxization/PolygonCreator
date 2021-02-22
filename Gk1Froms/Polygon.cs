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
    public enum RelationType
    {
        Vertical,
        Horizontal,
        Angle,
        NoRelation,
    }
    [Serializable]
    public abstract class Relation
    {
        //Naprawia relacje i zwraca zmieniony wierzcholek
        public abstract Vertex[] Fix(Vertex v, ref int dx, ref int dy);
        public abstract bool ContainVertex(Vertex v);

        public abstract void Clear();
    }
    
    [Serializable]
    public class HorizontalRelation : Relation
    {
        public Edge Edge { get; set; }
        public HorizontalRelation(Edge e)
        {
            Edge = e;
        }

        public override Vertex[] Fix(Vertex v, ref int dx, ref int dy)
        {
            Vertex k = v == Edge.A ? Edge.B : Edge.A;
            if (k.Y == v.Y) return new Vertex[] { };
            k.Y = v.Y;

            return new Vertex[] { k };
        }

        public override bool ContainVertex(Vertex v)
        {
            return Edge.A == v || Edge.B == v;
        }

        public override void Clear()
        {
            Edge.RelationTypeE = RelationType.NoRelation;
        }

    }

    [Serializable]
    public class VerticalRelation : Relation
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
            if (k.X == v.X) return new Vertex[] { };
            k.X = v.X;
            return new Vertex[] { k };
        }

        public override void Clear()
        {
            Edge.RelationTypeE = RelationType.NoRelation;
        }
    }

    [Serializable]
    public class AngleRelation: Relation
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

        public Vertex[] MoveEdge(Edge e, int dx, int dy)
        {
            Vertex w = edge1.A == edge2.A || edge1.A == edge2.B ? edge1.A : edge1.B;
            Vertex v1 = edge1.A == w ? edge1.B : edge1.A;
            Vertex v2 = edge2.A == w ? edge2.B : edge2.A;

            // w --- v1 === A1
            if((e.A == w || e.B == w) && (e.A == v1 || e.B == v1))
            {
                w.ChangeLocation(dx, dy);
                v1.ChangeLocation(dx, dy);

                if(double.IsInfinity(A2))
                {
                    v2.X = w.X;
                }
                else
                {
                    double B2 = w.Y - A2 * w.X;
                    v2.Y = (int)(A2 * v2.X + B2);
                }
            }
            else
            {
                w.ChangeLocation(dx, dy);
                v2.ChangeLocation(dx, dy);
                if(double.IsInfinity(A1))
                {
                    v1.X = w.X; 
                }
                else
                {
                    double B1 = w.Y - A1 * w.X;
                    v1.Y = (int)(A1 * v1.X + B1);
                }     
            }
            return new Vertex[] { v1, v2 };

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
                double newX = w.X, newY = w.Y;
                if (v == v1)
                {
                    if (double.IsInfinity(A1))
                    {
                        newX = v1.X;
                        newY = A2 * newX + B2;
                    }
                    else if(double.IsInfinity(A2))
                    {
                        newY = A1 * w.X + B1;
                    }
                    else
                    {
                        newX = (B2 - B1) / (A1 - A2);
                        newY = A1 * newX + B1;
                    }
                    
                    w.X = (int)(newX);
                    w.Y = (int)(newY);
                }
                else
                {
                    if(double.IsInfinity(A2))
                    {
                        newX = v2.Y;
                        newY = A1 * newX + B1;
                    }
                    else if (double.IsInfinity(A1))
                    {
                        newY = A2 * w.X + B2;
                    }
                    else
                    {
                        newX = (B1 - B2) / (A2 - A1);
                        newY = A1 * newX + B1;
                    }
                    
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

        public override void Clear()
        {
            edge1.RelationTypeE = RelationType.NoRelation;
            edge2.RelationTypeE = RelationType.NoRelation;
        }
    }

    [Serializable]
    public class Vertex
    {
        const int VERTEX_SIZE = 10;
        public int X { get; set; }
        public int Y { get; set; }

        public Vertex() { }
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

    [Serializable]
    public class Edge
    {
        public Vertex A { get; set; }
        public Vertex B { get; set; }

        public RelationType RelationTypeE { get; set; }
        public Vertex W { get; set; }

        public Edge() { }
        public Edge(Vertex a, Vertex b)
        {
            RelationTypeE = RelationType.NoRelation;
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

                Point s = new Point((A.X + B.X) / 2, (A.Y + B.Y) / 2);
                switch (RelationTypeE)
                {
                    case RelationType.Horizontal:
                        s.Y -= 10;
                        BresenhamLineAlgorithm.DrawLine(b, new Point(s.X - 10, s.Y), new Point(s.X + 10, s.Y));
                        break;
                    case RelationType.Vertical:
                        s.X -= 10;
                        BresenhamLineAlgorithm.DrawLine(b, new Point(s.X, s.Y - 10), new Point(s.X, s.Y + 10));
                        break;
                    case RelationType.Angle:
                        g.DrawEllipse(pen, W.X - 10, W.Y - 10, 20, 20);
                        break;
                }

                pen.Dispose();
            }
        }
    }

    [Serializable]
    public class Polygon : Figure
    {
        //How far detect click on edge
        const int EDGE_DISTANCE = 5;
        List<Edge> edges;

        List<Relation> relations;

        public Polygon() { }
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

        public Polygon Copy()
        {
            List<Edge> edgesCpy = new List<Edge>();
            foreach(Edge e in edges)
            {
                Vertex vA = new Vertex(e.A.X, e.A.Y);
                Vertex vB = new Vertex(e.B.X, e.B.Y);
                Edge newEdge = new Edge(vA, vB);
                edgesCpy.Add(newEdge);
            }
            Polygon polyCpy = new Polygon(edgesCpy.ToArray());
            return polyCpy;
        }
        public void MoveEdge(Edge e, int dx, int dy)
        {
            if (!edges.Contains(e)) return;
            List<Relation> rel = GetEdgeRelations(e, relations);
            if(rel.Count != 0 && rel[0] is AngleRelation)
            {
                AngleRelation angleRel = (AngleRelation)rel[0];
                Vertex[] changedV = angleRel.MoveEdge(e, dx, dy);
                foreach(Vertex v in changedV)
                {
                    FixRelation(v, dx, dy, new List<Relation>() { rel[0] });
                }
                return;
            }
            e.A.ChangeLocation(dx, dy);
            e.B.ChangeLocation(dx, dy);
            FixRelation(e.A, dx, dy, new List<Relation>());
            FixRelation(e.B, dx, dy, new List<Relation>());
        }

        public void MoveVertex(Vertex v, int dx, int dy)
        {
            if (!ContainsVertex(v)) return;
            v.ChangeLocation(dx, dy);
            FixRelation(v, dx, dy, new List<Relation>());
        }

        public void FixRelation(Vertex v, int dx, int dy, List<Relation> deleted)
        {
            List<Relation> vertexRel = GetVertexRelations(v, relations);
            List<Relation> process = new List<Relation>();
            foreach(Relation rel in vertexRel)
            {
                if(!deleted.Contains(rel))
                {
                    process.Add(rel);
                }
            }
            if (process.Count == 0) return;          

            foreach(Relation r in process)
            {
                Vertex[] changedV = r.Fix(v, ref dx, ref dy);
                foreach(Vertex k in changedV)
                {
                    deleted.Add(r);
                    FixRelation(k, dx, dy, deleted);
                }
                
            }
        }

        public bool CanAddRelation(Edge e, RelationType r)
        {
            List<Relation> list = GetVertexRelations(e.A, relations);
            list.AddRange(GetVertexRelations(e.B, relations));
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
                if (r.ContainVertex(e.A) && r.ContainVertex(e.B))
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
            int oldLoc;
            switch (r)
            {
                case RelationType.Horizontal:
                    oldLoc = e.B.Y;
                    e.B.Y = e.A.Y;
                    FixRelation(e.B, 0, e.A.Y - oldLoc, new List<Relation>());
                    rel = new HorizontalRelation(e);
                    break;
                case RelationType.Vertical:
                    oldLoc = e.B.X; 
                    e.B.X = e.A.X;
                    FixRelation(e.B, e.A.X - oldLoc, 0, new List<Relation>());
                    rel = new VerticalRelation(e);
                    break;
            }
            e.RelationTypeE = r;
            relations.Add(rel);
        }

        public void AddRelation(Vertex v, RelationType r, double angle)
        {

            List<Edge> e2 = new List<Edge>();
            foreach (Edge e in edges)
            {
                if (e.A == v || e.B == v) e2.Add(e);
            }
            if (!CanAddRelation(e2[0], r) || !CanAddRelation(e2[1], r))
            {
                MessageBox.Show("Cannot add relation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Vertex v2 = e2[1].A == v ? e2[1].B : e2[1].A;
            Point w2 = new Point(v2.X - v.X, v2.Y - v.Y);

            double prevAngle = GetAngle(v);

            double dAngle =  -(angle - prevAngle);

            double radians = (Math.PI / 180f) * dAngle;

            Point newW = new Point((int)(w2.X * Math.Cos(radians) - w2.Y * Math.Sin(radians)), (int)(w2.X * Math.Sin(radians) + w2.Y * Math.Cos(radians)));
            int dx = (newW.X + v.X) - v2.X;
            int dy = (newW.Y + v.Y) - v2.Y;

            int oldv2X = v2.X;
            int oldV2Y = v2.Y;
            v2.X = newW.X + v.X;
            v2.Y = newW.Y + v.Y;

            double newAngle = GetAngle(v);
            if (newAngle > angle + 5 || newAngle < angle - 5)
            {
                v2.X = oldv2X;
                v2.Y = oldV2Y;
                dAngle = -dAngle;
                radians = (Math.PI / 180f) * dAngle;
                newW = new Point((int)(w2.X * Math.Cos(radians) - w2.Y * Math.Sin(radians)), (int)(w2.X * Math.Sin(radians) + w2.Y * Math.Cos(radians)));
                dx = (newW.X + v.X) - v2.X;
                dy = (newW.Y + v.Y) - v2.Y;
                v2.X = newW.X + v.X;
                v2.Y = newW.Y + v.Y;
            }

            AngleRelation angleR = new AngleRelation(e2[0], e2[1]);
            FixRelation(v2, dx, dy, new List<Relation>());
            e2[0].RelationTypeE = RelationType.Angle;
            e2[1].RelationTypeE = RelationType.Angle;
            e2[0].W = v;
            e2[1].W = v;
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
                res.Clear();
                relations.Remove(res);
            }    
        }

        public void DeleteRelation(Vertex v)
        {
            List<Relation> res = GetVertexRelations(v, relations);
            if(res.Count == 1 && res[0] is AngleRelation)
            {
                res[0].Clear();
                relations.Remove(res[0]);
            }
        }

        public override void Draw(Bitmap b)
        {
            foreach(Edge e in edges)
            {
                e.Draw(b);
            }
        }
    }

    class myElipse : Figure
    {
        Point S,R;

        public myElipse(Point S, Point R)
        {
            this.S = S;
            this.R = R;
        }

        public void MoveR(int dx, int dy)
        {
            R.X += dx;
            R.Y += dy;
        }

        public myElipse Copy()
        {
            return new myElipse(S, R);
        }
        int distance(Point A, Point B)
        {
            return (int)Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
        }

        public bool GetElipse(Point p, out myElipse eli)
        {
            eli = null;
            if(isClose(p))
            {
                eli = this;
                return true;
            }
            return false;
        }

        public void Move(int dx, int dy)
        {
            S.X += dx;
            S.Y += dy;

            R.X += dx;
            R.Y += dy;
        }

        private bool isClose(Point p)
        {
            Point s = new Point(S.X + distance(S, R)/2, S.Y + distance(S, R)/2);
            if ((p.X - s.X) * (p.X - s.X) + (p.Y - s.Y) * (p.Y - s.Y) <= (distance(s, R) + 5) * (distance(s, R) + 5))
                return true;
            return false;
        }

        public override void Draw(Bitmap b)
        {
            using (Graphics g = Graphics.FromImage(b))
            {
                Pen pen = new Pen(Color.Black, 2);
                int r = distance(S, R);
                g.DrawEllipse(pen, S.X, S.Y, r, r);
                pen.Dispose();
            }
        }
    }

    [Serializable]
    public abstract class Figure
    {
        public abstract void Draw(Bitmap b);

    }
}
