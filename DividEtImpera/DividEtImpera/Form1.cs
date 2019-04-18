using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DividEtImpera
{
    public partial class Form1 : Form
    {
        private List<Point> points;

        private List<Point> DIV;
        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            points = new List<Point>();
            DIV = new List<Point>();
            button1.Enabled = true;
        }
        private int Side(Point p1, Point p2, Point p)
        {
            int val = (p.Y - p1.Y) * (p2.X - p1.X) -
                      (p2.Y - p1.Y) * (p.X - p1.X);

            if (val > 0)
                return 1;
            if (val < 0)
                return -1;
            return 0;
        }

        private int Distance(Point p1, Point p2, Point p)
        {
            return Math.Abs((p.Y - p1.Y) * (p2.X - p1.X) - (p2.Y - p1.Y) * (p.X - p1.X));
        }


        private void DEI()
        {

            if (points.Count <= 3)
            {
                foreach (var p in points)
                {
                    DIV.Add(p);
                }
                return;
            }

            Point pmin = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x < p2.x ? p1 : p2).point;

            Point pmax = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x > p2.x ? p1 : p2).point;

            DIV.Add(pmin);
            DIV.Add(pmax);

            List<Point> left = new List<Point>();
            List<Point> right = new List<Point>();

            for (int i = 0; i < points.Count; i++)
            {
                Point p = points[i];
                if (Side(pmin, pmax, p) == 1)
                    left.Add(p);
                else
                if (Side(pmin, pmax, p) == -1)
                    right.Add(p);
            }
          
            DivE(pmin, pmax, left);
            DivE(pmax, pmin, right);
        }

        private void DivE(Point a, Point b, List<Point> points)
        {
            int pos = DIV.IndexOf(b);

            if (points.Count == 0)
                return;

            if (points.Count == 1)
            {
                Point pp = points[0];
                DIV.Insert(pos, pp);
                return;
            }

            int dist = int.MinValue;
            int point = 0;

            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
                int distance = Distance(a, b, pp);
                if (distance > dist)
                {
                    dist = distance;
                    point = i;
                }
            }

            Point p = points[point];
            DIV.Insert(pos, p);
            List<Point> ap = new List<Point>();
            List<Point> pb = new List<Point>();

            // слева от AP
            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
                /*Graphics graphics = Graphics.FromImage(pictureBox1.Image);
                Random r = new Random();
                int c = r.Next(0, 15);
                Color random = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                Pen p1 = new Pen(random, 1);
                PointF[] pointF = new PointF[3];
                pointF[0].X = a.X;
                pointF[0].Y = a.Y;
                pointF[1].X = b.X;
                pointF[1].Y = b.Y;
                pointF[2].X = points[i].X;
                pointF[2].Y = points[i].Y;
                graphics.DrawPolygon(p1, pointF);*/

                if (Side(a, p, pp) == 1)
                {
                    ap.Add(pp);
                }
            }
            // слева от PB
            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
                if (Side(p, b, pp) == 1)
                {
                    pb.Add(pp);
                }
            }
            DivE(a, p, ap);
            DivE(p, b, pb);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            Random r = new Random();
            int c = r.Next(0, 15);
            Color random = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
           // Pen p1 = new Pen(random, 1);
            Pen pen = new Pen(random);
            points.Add(new Point(50,80));
            graphics.DrawRectangle(pen, 50, 80, 1, 1);
            points.Add(new Point(35,25));
            graphics.DrawRectangle(pen, 35, 25, 1, 1);
            points.Add(new Point(20,400));
            graphics.DrawRectangle(pen, 20, 400, 1, 1);
            points.Add(new Point(50,80));
            graphics.DrawRectangle(pen, 50, 80, 1, 1);
            points.Add(new Point(50, 300));
            graphics.DrawRectangle(pen, 50, 300, 1, 1);
            points.Add(new Point( 210,240));
            graphics.DrawRectangle(pen, 210, 240, 1, 1);
            points.Add(new Point(80,150));
            
            graphics.DrawRectangle(pen, 80, 150, 1, 1);
            DEI();
            random = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
            pen = new Pen(random);
            graphics.DrawPolygon(pen, DIV.ToArray());
            pictureBox1.Invalidate();
           
        }
    }
}
