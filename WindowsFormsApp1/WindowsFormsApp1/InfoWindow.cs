using System;
using System.Windows.Forms;

namespace TuringMachine
{
    public partial class InfoWindow : Form
    {
        public InfoWindow()
        {
            InitializeComponent();
        }

        private void Close(object sender, EventArgs e)
        {
            Close();
        }
    }
}
