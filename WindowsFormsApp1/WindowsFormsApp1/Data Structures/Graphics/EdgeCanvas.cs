using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace TuringMachine
{
    public partial class EdgeCanvas : Panel
    {
        Graph graph;

        public EdgeCanvas()
        {
            InitializeComponent();
        }

        public EdgeCanvas(Graph graph)
        {
            this.graph = graph;
            InitializeComponent();
        }

        //Create graphics object and define pen, load draw method
        private void PaintEvent(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen p = new Pen(Color.Black, 8)
            {
                EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor
            };
            FiniteStateMachine parent = (FiniteStateMachine)((EdgeCanvas)sender).Parent;

            int x = parent.HorizontalScroll.Value;
            int y = parent.VerticalScroll.Value;

            //Create edges and labels
            DrawEdge(g, p, parent, x, y);
        }

        private void DrawEdge(Graphics g, Pen p, FiniteStateMachine parent, int xOffset, int yOffset)
        {
            //Offset initial point by scroll amount
            Point initial = parent.Controls[parent.initialState].Location;
            initial.Offset(xOffset, yOffset);

            initial = new Point(initial.X - 15, initial.Y + 64);

            g.DrawLine(p, new Point(0, initial.Y), initial);

            foreach (KeyValuePair<string, Dictionary<string, string>> transitionFunction in graph.graph)
            {
                string state = transitionFunction.Key;
                Dictionary<string, string> info = transitionFunction.Value;
                foreach (KeyValuePair<string, string> link in info)
                {
                    string finalState = link.Key;
                    //Offset points by how much form is scrolled by
                    Point sender = parent.Controls[state].Location;
                    sender.Offset(xOffset, yOffset);
                    Point destination = parent.Controls[finalState].Location;
                    destination.Offset(xOffset, yOffset);
                    Point spawnPoint;

                    int dY = destination.Y - sender.Y;
                    int dX = destination.X - sender.X;

                    sender = new Point(sender.X + 64, sender.Y + 64);

                    //Make sure arrow points to centre of node
                    if (dX != 0)
                    {
                        int xDisplacement = 64;
                        double yDisplacement = xDisplacement * ((double)dY / (double)dX);

                        destination = new Point(destination.X - 10 * (dX / Math.Abs(dX)), destination.Y + 64 - Convert.ToInt32(yDisplacement));
                    }
                    else
                    {
                        destination = new Point(destination.X - 10, destination.Y + 64);
                    }

                    //Find which way to offset points
                    int sign = -1;
                    if (sender.Y < initial.Y)
                    {
                        sign = 1;
                    }

                    //Straight Line
                    if (dX > 0)
                    {
                        if (destination.Y < initial.Y)
                        {
                            sign = 1;
                        }

                        spawnPoint = new Point((sender.X + destination.X) / 2, (sender.Y + destination.Y) / 2);

                        spawnPoint = new Point(spawnPoint.X + 32, spawnPoint.Y);

                        if(dY != 0)
                        {
                            spawnPoint = new Point(spawnPoint.X + 32, spawnPoint.Y);
                        }
                        else
                        {
                            spawnPoint = new Point(spawnPoint.X, spawnPoint.Y - 32 * sign);
                        }

                        g.DrawLine(p, sender, destination);
                    }
                    //Looping Line
                    else if(dX == 0 && dY == 0)
                    {
                        //Points for bezier curve
                        Point p0 = new Point(sender.X + 16, sender.Y - 70 * sign);
                        Point p1 = new Point(sender.X + 16, sender.Y - 150 * sign);
                        Point p2 = new Point(destination.X + 48, p1.Y);
                        Point p3 = new Point(destination.X + 48, destination.Y - 70 * sign);

                        spawnPoint = new Point((p1.X + p2.X) / 2, (p2.Y));

                        g.DrawBezier(p, p0, p1, p2, p3);
                    }
                    //Arcing Line
                    else
                    {
                        //Points for bezier curve
                        Point p1 = new Point(sender.X, sender.Y - 300 * sign);
                        Point p2 = new Point(destination.X + 64, p1.Y - 25 * sign);
                        Point p3 = new Point(destination.X + 64, destination.Y - 100 * sign);

                        spawnPoint = new Point((p1.X + p2.X) / 2, (p2.Y + 48 * sign));

                        g.DrawBezier(p, sender, p1, p2, p3);
                    }

                    //Add Label on first drawing
                    //Don't if re-drawing
                    if (!parent.Controls.ContainsKey(state + finalState + graph.graph[state][finalState]))
                    {
                        parent.Controls.Add(new TransitionTag(state, finalState, link.Value, spawnPoint));
                        Control label = parent.Controls[parent.Controls.Count - 1];
                        label.BringToFront();
                        label.Location = new Point(label.Location.X - label.Width / 2, label.Location.Y - label.Height / 2);
                    }
                }
            }
        }
    }
}
