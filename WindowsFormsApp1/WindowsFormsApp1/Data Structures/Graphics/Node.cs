using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace TuringMachine
{
    public class Node : PictureBox
    {
        public Node(string state)
        {
            Width = 128;
            Height = 128;
            SizeMode = PictureBoxSizeMode.StretchImage;
            Anchor = AnchorStyles.Left;
            BackColor = Color.White;
            SendToBack();

            Label stateLabel = new Label()
            {
                Text = state,
                Font = new Font("Consolas", 15.0f),
                TextAlign = ContentAlignment.MiddleCenter,
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                AutoSize = true
            };
            Name = state;
            Controls.Add(stateLabel);

            stateLabel.Location = new Point((Width - stateLabel.Width) / 2, (Height - stateLabel.Height) / 2);

            string path = Path.GetDirectoryName(Application.StartupPath) + "\\Debug\\state_circle.png";
            
            Image = new Bitmap(path);
        }

        public void SetHalting()
        {
            string path = Path.GetDirectoryName(Application.StartupPath) + "\\Debug\\state_circle_halting.png";

            Image = new Bitmap(path);
        }
    }
}
