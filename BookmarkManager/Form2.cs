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
    public partial class frm_AddEdit : Form
    {
        public frm_AddEdit()
        {
            InitializeComponent();
            tb_AddDate.Enabled = false;
            tb_AddDate.Text = DateTime.Now.ToString();
            this.Text = "Add";
        }

        public frm_AddEdit(Image icon, string path, string title, string url, string addDate)
        {
            InitializeComponent();
            this.Text = "Edit";
            tb_AddDate.Text = addDate;
            tb_AddDate.Enabled = false;
            pb_Icon.Image = icon;
            tb_Path.Text = path;
            tb_Title.Text = title;
            tb_URL.Text = url;
        }

    }
}