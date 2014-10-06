using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BookmarkManager
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            tb_AddDate.Enabled = false;
            tb_AddDate.Text = DateTime.Now.ToString();
        }
        
        
    }
}


