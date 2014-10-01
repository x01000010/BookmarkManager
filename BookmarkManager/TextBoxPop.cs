using System;
using System.Windows.Forms;

namespace BookmarkManager
{
    public partial class TextBoxPop : Form
    {
        public TextBoxPop(string text)
        {
            InitializeComponent();
            rtb_Text.AppendText(text);
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}