using System;
using System.Windows.Forms;

namespace BookmarkManager
{
    public partial class TextBoxPop : Form
    {
        public TextBoxPop()
        {
            InitializeComponent();
        }

        public void AddText(string text)
        {
            string output = string.Format("{0}: {1}{2}", DateTime.Now, text, Environment.NewLine);
            rtb_Text.AppendText(output);
            rtb_Text.SelectionStart = rtb_Text.Text.Length;
            rtb_Text.ScrollToCaret();
            //rtb_Text.Refresh();
            
        }

        private void tsmi_Clear_Click(object sender, EventArgs e)
        {
            rtb_Text.Clear();
            rtb_Text.Refresh();
        }

        private void tsmi_Hide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}