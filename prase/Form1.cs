using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace prase
{
    public partial class Form1 : Form
    {
        Thread t;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // initialisation
            pictureBox1.Image = new System.Drawing.Bitmap(pictureBox1.Width, pictureBox1.Height);

            // put the process into separate thread, so it doesn't lag the window
            t = new Thread(draw);
            t.Start();
        }

        private void draw()
        {
            var bmp = pictureBox1.Image;
            using (var g = Graphics.FromImage(bmp))
            {
                // body
                animateLine(g, new Point(400, 100), new Point(400, 300));
                animateLine(g, new Point(400, 300), new Point(100, 300));
                animateLine(g, new Point(100, 300), new Point(100, 100));
                animateLine(g, new Point(100, 100), new Point(400, 100));

                // head
                animateLine(g, new Point(400, 100), new Point(490, 200));
                animateLine(g, new Point(490, 200), new Point(400, 300));

                // legs
                animateLine(g, new Point(400, 300), new Point(350, 450));
                animateLine(g, new Point(400, 300), new Point(450, 450));

                animateLine(g, new Point(100, 300), new Point(50, 450));
                animateLine(g, new Point(100, 300), new Point(150, 450));

                // eye // TODO animate
                g.DrawEllipse(new Pen(Brushes.Black, 3), 415, 165, 25, 25);

                // spiral // TODO animate
                const double spins = 1.7;
                int pointsAmount = (int) spins * 2 * (100 + 270);
                PointF[] points = new PointF[pointsAmount];
                float fAngle, fScale;

                for (int i = 0; i < pointsAmount; i++)
                {
                    fAngle = (float)(i * 2 * Math.PI / (pointsAmount / spins));
                    fScale = 1 - (float)i / pointsAmount;

                    points[i].X = (float)(100 / 2 * (1 + fScale * Math.Cos(fAngle)));
                    points[i].Y = (float)(270 / 2 * (1 + fScale * Math.Sin(fAngle)));
                }
                g.DrawLines(new Pen(Brushes.Black, 3), points);
            }

            pictureBox1.Invalidate();
        }

        private void animateLine(Graphics g, Point p1, Point p2) // line animation calculations
        {
            int x1 = p1.X;
            int y1 = p1.Y;
            int x2 = p2.X;
            int y2 = p2.Y;
            double distance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));

            Vector2 a = new Vector2(x1, y1);
            Vector2 b = new Vector2(x2, y2);

            for(float i = 0; i <= 1f; i += 0.01f)
            {
                Vector2 c = (a - b) * i + a;

                g.FillRectangle(Brushes.Black, new RectangleF(c.X, c.Y, 3, 3));
                pictureBox1.Invalidate();

                Thread.Sleep((int) distance / 100 * 10);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            t.Abort();
        }
    }

    struct Vector2 // helper struct for lines calculation
    {
        public readonly float X;
        public readonly float Y;

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(b.X - a.X, b.Y - a.Y);
        }
        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }
        public static Vector2 operator *(Vector2 a, float d)
        {
            return new Vector2(a.X * d, a.Y * d);
        }
    }
}
