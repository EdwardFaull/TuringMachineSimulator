using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TuringMachine
{
    public partial class FiniteStateMachine : Form
    {
        string code = "";
        public string initialState { get; } = "";
        Graph graph;
        const int spacing = 200;

        public FiniteStateMachine(string code, string initialState, string name)
        {
            InitializeComponent();
            Text = "Graphical Representation - " + name;
            this.code = code;
            this.initialState = initialState;
            VerticalScroll.Visible = true;
        }

        //Generate diagram on loading
        private void Generate(object sender, EventArgs e)
        {
            try
            {
                graph = new Graph(code);

                if (graph.statesHalting.Contains(initialState))
                {
                    MessageBox.Show("Error: Initial state cannot be a halting state.");
                    Close();
                }

                //Load nodes
                Spawn(initialState, "");

                //Add edges
                EdgeCanvas edgeCanvas = new EdgeCanvas(graph);
                Controls.Add(edgeCanvas);

                //Set scroll 'cheat' object
                int highest = 0;
                int lowest = 0;
                foreach (Control control in Controls)
                {
                    if (control is Node)
                    {
                        if (control.Location.Y > highest)
                        {
                            highest = control.Location.Y;
                        }
                        if (control.Location.Y < lowest)
                        {
                            lowest = control.Location.Y;
                        }
                    }
                }
                vScrollBar1.Location = new Point(-50, (highest - lowest) / 2);
                vScrollBar1.Size = new Size(2, highest - lowest);
            }
            catch (Exception)
            {
                MessageBox.Show("Error generating FSM.");
                Close();
            }
        }

        //Depth-first loading of nodes
        private void Spawn(string state, string parent)
        {
            if (!Controls.ContainsKey(state))
            {
                //Initial state case
                if (parent == "")
                {
                    Controls.Add(new Node(state));
                    Controls[state].Location = new Point(50, Height / 2 - 64);
                }
                //Spawn relative to parent node
                else
                {
                    Controls.Add(new Node(state));
                    Point parentLocation = Controls[parent].Location;

                    List<string> parentNeighbours = new List<string>(graph.neighbours[parent]);

                    parentNeighbours.Remove(parent);

                    if (parentNeighbours.Count > 1)
                    {
                        int maxHeight = (parentNeighbours.Count - 1) / 2;
                        int IndexInList = parentNeighbours.FindIndex(i => i == state);

                        int X = parentLocation.X + spacing;
                        int Y;
                        if (parentNeighbours.Count % 2 == 0)
                        {
                            Y = parentLocation.Y + spacing * IndexInList - spacing / 2 - spacing * maxHeight;
                        }
                        else
                        {
                            Y = parentLocation.Y + spacing * IndexInList - spacing * maxHeight;
                        }

                        Controls[state].Location = new Point(X, Y);
                    }
                    else
                    {
                        int X = parentLocation.X + spacing;
                        int Y = parentLocation.Y;

                        Controls[state].Location = new Point(X, Y);
                    }
                }
            }
            //If state is halting
            if (graph.statesHalting.Contains(Controls[Controls.Count - 1].Name))
            {
                ((Node)Controls[Controls.Count - 1]).SetHalting();
            }

            //Spawn each neighbour node + their children
            if (graph.neighbours.ContainsKey(state))
            {
                foreach (string neighbour in graph.neighbours[state])
                {
                    if (!Controls.ContainsKey(neighbour))
                    {
                        Spawn(neighbour, state);
                    }
                }
            }
        }

        //Refresh edge background
        private void RefreshEdges(object sender, EventArgs e)
        {
            foreach(Control control in Controls)
            {
                if(control is EdgeCanvas)
                {
                    control.Refresh();
                }
            }
        }
    }
}