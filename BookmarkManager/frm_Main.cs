using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BookmarkManager
{
    public partial class frm_Main : Form
    {
        private static XName _addDate = "ADD_DATE";
        private static string _endOfFile = "/DL><p>";
        private static XName _h3 = "H3";
        private static XName _href = "HREF";
        private static XName _icon = "ICON";
        private static XName _lastModified = "LAST_MODIFIED";

        private static string _startOfFile = @"<!DOCTYPE NETSCAPE-Bookmark-file-1>
<!-- This is an automatically generated file.
     It will be read and overwritten.
     DO NOT EDIT! -->
<META HTTP-EQUIV=""Content-Type"" CONTENT=""text/html; charset=UTF-8"">
<TITLE>Bookmarks</TITLE>
<H1>Bookmarks</H1>
<DL><p>";
        private TextBoxPop log;
        private List<BookmarkObject> _files = new List<BookmarkObject>();

        public frm_Main()
        {
            InitializeComponent();
            log = new TextBoxPop();
#if DEBUG
            log.Show();
#endif
            dataGridView1.DataSource = __createDataTable();
        }

        private XElement __changeToXML(string s)
        {
            //need to fix incoming text
            //&#58;
            //&#39;
            //&gt;
            //&quot;

            XElement root = null;
            s = s.Replace("</A>", "</A></DT>");
            s = s.Replace("<DL><p>", "<DL>");
            s = s.Replace("</DL><p>", "</DL></DT>");
            s = __fixAndReplaceAmp(s);
            s = "<Bookmarks>" + s + "</Bookmarks>";
            root = XElement.Parse(s);
            return root;
        }

        private string __fixAndReplaceAmp(string str)
        {
            string s = str;
            while (s.Contains("&amp;"))
            {
                s = s.Replace("&amp;", "&");
            }
            s = s.Replace("&", "&amp;");
            return s;
        }



        private string __getPath(XElement x, string currentPath)
        {
            string s = string.Empty;
            XElement previous = x.Parent;
            XElement previousPrevious = previous.Parent;
            if (previousPrevious.Name == "Bookmarks")
            {
                return string.Format(@"{0}\{1}", previousPrevious.Name, currentPath);
            }
            else
            {
                XElement previousPreviousSibling = previousPrevious.Parent.Element(_h3);
                return __getPath(previousPreviousSibling, string.Format(@"{0}\{1}", previousPreviousSibling.Value, currentPath));
            }
        }


        private DataTable __processBookmarks(XElement bookmarks, DataTable dt)
        {


            XName aName = "A";

            IEnumerable<XElement> z = bookmarks.Descendants(aName);
            int count = 0;
            foreach (XElement x in bookmarks.Descendants(aName))
            {
                //log.AddText(x.ToString());
                dt.Rows.Add(__processBookmark(x));
                count++;
            }
            log.AddText(count.ToString());
            return dt;
        }

        private object[] __processBookmark(XElement x)
        {

            //shown: icon | title | path | good/bad | url | addDate
            //hidden: lastmodified

            string iconString = (x.Attribute(_icon) == null) ? string.Empty : x.Attribute(_icon).Value;
            Image icon = BookmarkObject.Base64ToImage(iconString);

            string title = x.Value;
            log.AddText(string.Format("Processing title:{0}", title));
            string path = __getPath(x, string.Empty);

            string url = (x.Attribute(_href) == null) ? string.Empty : x.Attribute(_href).Value;

            string addDate = (x.Attribute(_addDate) == null) ? string.Empty : x.Attribute(_addDate).Value;
            string lastModified = (x.Attribute(_lastModified) == null) ? string.Empty : x.Attribute(_lastModified).Value;



            return new object[] { icon, title, path, string.Empty, url, addDate, lastModified };

        }
        private void __removeDups()
        {
            //DataTable dt = dataGridView1.DataSource as DataTable;
            //DataTable noDups = __createDataTable();

            //List<string> keys = new List<string>();
            //DataRowCollection drc = dt.Rows;
            //foreach (DataRow row in drc)
            //{
            //    string key = row[dt.Columns.IndexOf("URL")] as string;
            //    if (!keys.Contains(key))
            //    {
            //        keys.Add(key);
            //        //dt.Rows.Remove(row);
            //        //noDups.Rows.Add(row);
            //    }
            //    else { dt.Rows.Remove(row); }
            //}

            List<string> keys = new List<string>();
            for (int currentRow = 0; currentRow < dataGridView1.Rows.Count; currentRow++)
            {
                DataGridViewRow rowToCompare = dataGridView1.Rows[currentRow];
                string key = rowToCompare.Cells["URL"].Value as string;
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                }
                else {
                    dataGridView1.Rows.Remove(rowToCompare);
                    log.AddText(string.Format("Removing {0} at indext {1}", key, currentRow));
                    currentRow--;
                }
            }  // __writeToGrid(noDups);
        }

        private string __stripNetscapeHeader(string s)
        {
            string noHeader = string.Empty;
            int idx = s.IndexOf("<DT>");
            noHeader = s.Substring(idx);
            idx = noHeader.LastIndexOf("</DL><p>");
            noHeader = noHeader.Substring(0, idx);
            return noHeader;
        }

        private void __writeToFile(SortableBindingList<BookmarkObject> sbl)
        {
            string output = _startOfFile;
            FolderObject root = new FolderObject("Bookmarks", null);
            root.IsRoot = true;

            foreach (BookmarkObject bo in sbl)
            {
                root.Add(bo);
            }

            output += root.ToString();
            output += _endOfFile;
            //TextBoxPop tbp = new TextBoxPop(output);
            //tbp.Show();
        }

        private void __writeToFile(List<BookmarkObject> lbo)
        {
            __writeToFile(new SortableBindingList<BookmarkObject>(lbo));
        }

        private void __writeToGrid(DataTable dt)
        {
            tssl_Status.Text = "Writing to Grid";
            statusStrip1.Refresh();
            dataGridView1.DataSource = dt;
            tssl_Status.Text = "OK";
            tssl_Count.Text = dataGridView1.Rows.Count.ToString();
            statusStrip1.Refresh();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_RemoveDups_Click(object sender, EventArgs e)
        {
            __removeDups();
        }

        private void btn_ToFile_Click(object sender, EventArgs e)
        {
            __writeToFile(dataGridView1.DataSource as SortableBindingList<BookmarkObject>);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __openFile();
        }
        private void __openFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                DataTable dt = dataGridView1.DataSource as DataTable;
                foreach (string fileName in ofd.FileNames)
                {
                    try
                    {
                        log.AddText(string.Format("Loading {0}", fileName));
                        tssl_Status.Text = string.Format("Loading {0}", fileName);
                        statusStrip1.Refresh();
                        string file = File.ReadAllText(fileName);
                        file = __stripNetscapeHeader(file);
                        XElement bookmarks = __changeToXML(file);
                        //__readBookmarks(bookmarks);
                        dt = __processBookmarks(bookmarks, dt);
                        tssl_Status.Text = "OK";
                        statusStrip1.Refresh();
                    }
                    catch (Exception ex)
                    {
                        log.AddText(ex.Message + fileName);
                    }
                }
                __writeToGrid(dt);

            }
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __openFile();
        }

        private DataTable __createDataTable()
        {


            //shown: icon | title | path | good/bad | url | addDate
            //hidden: lastmodified
            //don't need: parent | iconstring

            DataTable dt = new DataTable();
            dt.Columns.Add("Icon", typeof(Image));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Path", typeof(string));
            dt.Columns.Add("Checked", typeof(string));
            dt.Columns.Add("URL", typeof(string));
            dt.Columns.Add("Add Date", typeof(string));
            dt.Columns.Add("Mod Date", typeof(string));
            return dt;

        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (log.Visible == true)
            {
                log.Hide();
            }
            else
            {
                log.Show();
            }
            this.Focus();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
            tssl_Count.Text = dataGridView1.Rows.Count.ToString();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            int idx = dataGridView1.SelectedRows[0].Index;
            Image i = dataGridView1.Rows[idx].Cells[0].Value as Image;
            string title = dataGridView1.Rows[idx].Cells[1].Value.ToString();
            string path = dataGridView1.Rows[idx].Cells[2].Value.ToString();
            string url = dataGridView1.Rows[idx].Cells[4].Value.ToString();
            string addDate = dataGridView1.Rows[idx].Cells[5].Value.ToString();

            frm_AddEdit edit = new frm_AddEdit(i, path, title, url, addDate);
            edit.Show();
        }
    }
}

//This code removes selected items of dataGridView1:

// private void btnDelete_Click(object sender, EventArgs e)
// {
//     foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
//     {
//         dataGridView1.Rows.RemoveAt(item.Index);
//     }
// }