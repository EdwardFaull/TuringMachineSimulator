using System;
using System.Windows.Forms;
using System.Drawing;

namespace TuringMachine
{
    public class TabObj : TabPage
    {
        public TabObj(Control parent)
        {
            Parent = parent.Controls["InputWindow"];

            RichTextBox tb = new RichTextBox
            {
                Font = new Font("Consolas", 13F),
                Location = new Point(0, 0),
                Multiline = true,
                Name = "codeBox",
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Size = new Size(505, 301),
                TabIndex = 0,
                BorderStyle = BorderStyle.FixedSingle
            };
            tb.SelectionChanged += new EventHandler(((Simulator)parent).SetLineNumberLabel);
            tb.KeyDown += new KeyEventHandler(((Simulator)parent).Tab_Paste);

            Controls.Add(tb);
            Location = new Point(4, 22);
            Name = "tabPage" + Parent.Controls.Count;
            Padding = new Padding(3);
            Size = new Size(443, 299);
            Text = "machine" + Parent.Controls.Count;
            UseVisualStyleBackColor = true;
        }

        public RichTextBox GetText()
        {
            return (RichTextBox)Controls["codeBox"];
        }
    }
}
