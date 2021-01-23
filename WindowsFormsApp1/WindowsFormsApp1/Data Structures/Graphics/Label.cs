using System.Windows.Forms;
using System.Drawing;

namespace TuringMachine
{
    class TransitionTag : Label
    {
        public TransitionTag(string state, string finalstate, string info, Point location)
        {
            Anchor = AnchorStyles.Left;
            AutoSize = true;
            BackColor = Color.White;
            Font = new Font("Consolas", 11, FontStyle.Bold);
            Name = state + finalstate + info;
            Text = info;
            Location = location;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // TransitionTag
            // 
            this.BackColor = System.Drawing.Color.White;
            this.ResumeLayout(false);

        }
    }
}
