using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickHull11
{

    public partial class Form1 : Form
    {

        private List<Point> points;

        private List<Point> hull;

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            graphics.Clear(Color.White);
            points = new List<Point>();
            hull = new List<Point>();
            button1.Enabled = true;

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            
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


        private void QuickHull()
        {

            if (points.Count <= 3)
            {
                foreach(var p in points)
                {
                    hull.Add(p);
                }
                return;
            }
                
            Point pmin = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x < p2.x ? p1 : p2).point;

            Point pmax = points
                .Select(p => new { point = p, x = p.X })
                .Aggregate((p1, p2) => p1.x > p2.x ? p1 : p2).point;

            hull.Add(pmin);
            hull.Add(pmax);

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
            CreateHull(pmin, pmax, left);
            CreateHull(pmax, pmin, right);
        }

        private void CreateHull(Point a, Point b, List<Point> points)
        {
            int pos = hull.IndexOf(b);

            if (points.Count == 0)
                return;

            if (points.Count == 1)
            {
                Point pp = points[0];
                hull.Insert(pos, pp);
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
            hull.Insert(pos, p);
            List<Point> ap = new List<Point>();
            List<Point> pb = new List<Point>();

            // слева от AP
            for (int i = 0; i < points.Count; i++)
            {
                Point pp = points[i];
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
            CreateHull(a, p, ap);
            CreateHull(p, b, pb);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            Pen pen = new Pen(Color.Red);
            points.Add(new Point(20,80));
            graphics.DrawRectangle(pen, 20,80, 1, 1);
            points.Add(new Point(10, 90));
            graphics.DrawRectangle(pen, 10, 90, 1, 1);
            points.Add(new Point(50, 60));
            graphics.DrawRectangle(pen, 50, 60, 1, 1);
            points.Add(new Point(80, 100));
            graphics.DrawRectangle(pen, 80,100, 1, 1);
            points.Add(new Point(10, 30));
            graphics.DrawRectangle(pen, 10, 30, 1, 1);
            points.Add(new Point(30, 40));
            graphics.DrawRectangle(pen, 30, 40, 1, 1);
            points.Add(new Point(50, 70));
            graphics.DrawRectangle(pen, 50, 70, 1, 1);
           // hull.Clear();
            /*if(points.Count!=0)
            {
                pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics graphics1 = Graphics.FromImage(pictureBox1.Image);
                graphics1.Clear(Color.White);
                Pen pen1 = new Pen(Color.Red);
                foreach (var p in points)
                {
                    graphics1.DrawRectangle(pen1, p.X, p.Y, 1, 1);
                    pictureBox1.Invalidate();
                }

            }*/
            QuickHull();
            graphics.DrawPolygon(pen, hull.ToArray());
            pictureBox1.Invalidate();
            button2.Enabled = true;

        }
        private void button2_Click(object sender, EventArgs e)
        {
            Graphics graphics = Graphics.FromImage(pictureBox1.Image);
            Pen pen = new Pen(Color.Red);
            graphics.Clear(pictureBox1.BackColor);
            graphics.DrawPolygon(pen, hull.ToArray());
            pictureBox1.Invalidate();
            button2.Enabled = true;
        }
    }
}
