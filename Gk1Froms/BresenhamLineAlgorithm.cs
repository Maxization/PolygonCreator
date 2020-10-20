using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gk1Froms
{
    static class BresenhamLineAlgorithm
    {
        public static void DrawLine(Bitmap b, Point A, Point B)
        {
            using(Graphics g = Graphics.FromImage(b))
            {
                int dx = B.X - A.X;
                int dy = B.Y - A.Y;
                int d = 2 * dy - dx;
                int incrE = 2 * dy;
                int incrNE = 2 * (dy - dx);
                int x = A.X;
                int y = A.Y;

                Brush brush = new SolidBrush(Color.Black);
                g.FillRectangle(brush, x, y, 1, 1);

                if(B.X - A.X >= 0 && B.Y - A.Y >= 0 && B.Y - A.Y <= B.X - A.X)
                {
                    while (x < B.X)
                    {
                        if (d < 0)
                        {
                            d += incrE;
                            x++;
                        }
                        else
                        {
                            d += incrNE;
                            x++;
                            y++;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
                else if(B.X - A.X >= 0 && B.Y - A.Y >= 0 && B.Y - A.Y > B.X - A.X)
                {
                    d = 2 * dx - dy;
                    incrE = 2 * dx;
                    incrNE = 2 * (dx - dy);
                    while (y < B.Y)
                    {
                        if (d < 0)
                        {
                            d += incrE;
                            y++;
                        }
                        else
                        {
                            d += incrNE;
                            x++;
                            y++;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
                else if(B.X - A.X < 0 && B.Y - A.Y >= 0 && B.Y - A.Y >= -(B.X - A.X))
                {
                    d = 2 * dx + dy;
                    incrE = 2 * dx;
                    incrNE = 2 * (dx + dy);
                    while (y < B.Y)
                    {
                        if (d > 0)
                        {
                            d += incrE;
                            y++;
                        }
                        else
                        {
                            d += incrNE;
                            x--;
                            y++;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
                else if(B.X - A.X <= 0 && B.Y - A.Y >=0 && B.Y - A.Y < -(B.X-A.X))
                {
                    d = 2 * dy + dx;
                    incrE = 2 * dy;
                    incrNE = 2 * (dx + dy);
                    while (x > B.X)
                    {
                        if (d < 0)
                        {
                            d += incrE;
                            x--;
                        }
                        else
                        {
                            d += incrNE;
                            x--;
                            y++;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
                else if(B.X -A.X < 0 && B.Y - A.Y < 0 && B.Y - A.Y >= B.X -A.X)
                {
                    d = -2 * dy + dx;
                    incrE = -2 * dy;
                    incrNE = 2 * (dx - dy);
                    while (x > B.X)
                    {
                        if (d < 0)
                        {
                            d += incrE;
                            x--;
                        }
                        else
                        {
                            d += incrNE;
                            x--;
                            y--;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
                else if(B.X - A.X <= 0 && B.Y - A.Y <= 0 && B.Y - A.Y < B.X - A.X)
                {
                    d = -2 * dx + dy;
                    incrE = -2 * dx;
                    incrNE = 2 * (dy - dx);
                    while (y > B.Y)
                    {
                        if (d < 0)
                        {
                            d += incrE;
                            y--;
                        }
                        else
                        {
                            d += incrNE;
                            x--;
                            y--;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }    
                else if(B.X - A.X > 0 && B.Y - A.Y <= 0 && B.Y - A.Y < -(B.X - A.X))
                {
                    d = 2 * dx + dy;
                    incrE = 2 * dx;
                    incrNE = 2 * (dy + dx);
                    while (y > B.Y)
                    {
                        if (d < 0)
                        {
                            d += incrE;
                            y--;
                        }
                        else
                        {
                            d += incrNE;
                            x++;
                            y--;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
                else if(B.X - A.X > 0 && B.Y - A.Y <= 0 && B.Y - A.Y >= -(B.X - A.X))
                {
                    d = 2 * dy + dx;
                    incrE = 2 * dy;
                    incrNE = 2 * (dx + dy);
                    while (x < B.X)
                    {
                        if (d > 0)
                        {
                            d += incrE;
                            x++;
                        }
                        else
                        {
                            d += incrNE;
                            x++;
                            y--;
                        }
                        g.FillRectangle(brush, x, y, 1, 1);
                    }
                }
            }
        }
    }
}
