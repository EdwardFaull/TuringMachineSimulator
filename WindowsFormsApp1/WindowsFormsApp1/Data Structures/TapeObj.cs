using System;
using System.Windows.Forms;
using System.Drawing;

namespace TuringMachine
{
    public class TapePanel : Panel
    {
        public TapePanel(Control parent)
        {
            //Move read/write head
            Button pointRight = new Button();
            Button pointLeft = new Button();

            //Delete tape
            Button close = new Button();

            //Tape and read/write head
            TextBox point = new TextBox();
            TextBox Tape = new TextBox();
            

            //Initial state drop-down box
            ComboBox iState = new ComboBox();
            Label iStateLabel = new Label();

            //Labels
            Label currentState = new Label();
            Label steps = new Label();

            #region SET VALUES
            Parent = parent;
            Anchor = AnchorStyles.Bottom;
            Controls.Add(pointRight);
            Controls.Add(pointLeft);
            Controls.Add(close);
            Controls.Add(point);
            Controls.Add(Tape);
            Controls.Add(iStateLabel);
            Controls.Add(iState);
            Controls.Add(currentState);
            Controls.Add(steps);
            Name = "tapePanel" + 1.ToString();
            Size = new Size(1100, 70);
            Location = new Point(0, 27);
            TabStop = false;

            Tape.BorderStyle = BorderStyle.None;
            Tape.BackColor = Color.White;
            Tape.Anchor = AnchorStyles.None;
            Tape.Font = new Font("Consolas", 15F);
            Tape.Location = new Point(0, 3);
            Tape.Name = "tape";
            Tape.Size = new Size(1100, 24);
            Tape.TabStop = false;
            Tape.TextAlign = HorizontalAlignment.Center;
            Tape.TextChanged += new EventHandler(((Simulator)Parent).UI_UpdateTape);

            point.BorderStyle = BorderStyle.None;
            point.BackColor = Color.White;
            point.Anchor = AnchorStyles.None;
            point.Font = new Font("Consolas", 15F);
            point.ForeColor = SystemColors.WindowText;
            point.Location = new Point(0, 25);
            point.Margin = new Padding(3, 0, 3, 0);
            point.Name = "pointer";
            point.ReadOnly = true;
            point.Size = new Size(1100, 24);
            point.TabStop = false;
            point.Text = "^";
            point.TextAlign = HorizontalAlignment.Center;

            pointRight.AutoSize = true;
            pointRight.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pointRight.Location = new Point(553, 45);
            pointRight.Name = "pointerRight";
            pointRight.Size = new Size(40, 23);
            pointRight.TabStop = false;
            pointRight.Text = ">>>>";
            pointRight.UseVisualStyleBackColor = true;
            pointRight.Click += new EventHandler(((Simulator)Parent).UI_PointerAdd);

            pointLeft.AutoSize = true;
            pointLeft.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            pointLeft.Location = new Point(507, 45);
            pointLeft.Name = "pointerLeft";
            pointLeft.Size = new Size(40, 23);
            pointLeft.TabStop = false;
            pointLeft.Text = "<<<<";
            pointLeft.UseVisualStyleBackColor = true;
            pointLeft.Click += new EventHandler(((Simulator)Parent).UI_PointerSubtract);

            close.AutoSize = true;
            close.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            close.Location = new Point(831, 44);
            close.Name = "Close";
            close.Size = new Size(71, 23);
            close.TabStop = false;
            close.Text = "Close Tape";
            close.UseVisualStyleBackColor = true;
            close.Visible = true;
            close.Click += new EventHandler(((Simulator)Parent).CloseTape);

            iState.Font = new Font("Microsoft Sans Serif", 10F);
            iState.FormattingEnabled = true;
            iState.Location = new Point(723, 44);
            iState.Name = "initialState";
            iState.Size = new Size(97, 24);
            iState.TabStop = false;
            iState.SelectedIndexChanged += new EventHandler(((Simulator)Parent).UI_UpdateInitialState);

            iStateLabel.AutoSize = true;
            iStateLabel.Font = new Font("Microsoft Sans Serif", 9F);
            iStateLabel.Location = new Point(647, 47);
            iStateLabel.Margin = new Padding(3);
            iStateLabel.Name = "initialStateLabel";
            iStateLabel.Size = new Size(70, 15);
            iStateLabel.TabStop = false;
            iStateLabel.Text = "Initial State:";

            currentState.AutoSize = false;
            currentState.Name = "currentState";
            currentState.Text = "CURRENTSTATE";
            currentState.Font = new Font("Consolas", 12F);
            currentState.Location = new Point(240, 35);
            currentState.BorderStyle = BorderStyle.FixedSingle;
            currentState.Size = new Size(150, 35);
            currentState.TextAlign = ContentAlignment.MiddleCenter;

            steps.AutoSize = false;
            steps.Name = "steps";
            steps.Text = "STEPS";
            steps.Font = new Font("Consolas", 12F);
            steps.Location = new Point(146, 35);
            steps.BorderStyle = BorderStyle.FixedSingle;
            steps.Size = new Size(89, 35);
            steps.TextAlign = ContentAlignment.MiddleCenter;

            currentState.BringToFront();
            steps.BringToFront();
            iState.BringToFront();
            iStateLabel.BringToFront();
            close.BringToFront();

            #endregion
        }

        public ComboBox GetInitialState()
        {
            return (ComboBox)Controls["initialState"];
        }

        public TextBox GetTape()
        {
            return (TextBox)Controls["tape"];
        }

        public TextBox GetPointer()
        {
            return (TextBox)Controls["pointer"];
        }

        public Label GetCurrentStateLabel()
        {
            return (Label)Controls["currentState"];
        }

        public Label GetInitialStateLabel()
        {
            return (Label)Controls["initialStateLabel"];
        }

        public Label GetStepLabel()
        {
            return (Label)Controls["steps"];
        }

        public Button GetCloseButton()
        {
            return (Button)Controls["close"];
        }
    }
}
