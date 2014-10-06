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

        private List<BookmarkObject> _files = new List<BookmarkObject>();

        public frm_Main()
        {
            InitializeComponent();
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

        private string __getParent(XElement x)
        {
            
            //maybe use path to get this instead
            XElement previousPrevious = x.Parent.Parent;
            if (previousPrevious.Name == "Bookmarks")
            {
                return previousPrevious.Name.ToString();
            }
            else
            {
                return (previousPrevious.Parent.Element(_h3).Value);
            }
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

        private void __processElement(XElement x)
        {
            BookmarkObject bo = new BookmarkObject();
            bo.Name = x.Value;
            bo.Path = __getPath(x, string.Empty);
            bo.Parent = __getParent(x);
            bo.Url = (x.Attribute(_href) == null) ? string.Empty : x.Attribute(_href).Value;
            XAttribute iconString = x.Attribute(_icon);
            bo.IconString = (x.Attribute(_icon) == null) ? string.Empty : x.Attribute(_icon).Value;

            

            bo.AddDate = (x.Attribute(_addDate) == null) ? string.Empty : x.Attribute(_addDate).Value;
            bo.LastModified = (x.Attribute(_lastModified) == null) ? string.Empty : x.Attribute(_lastModified).Value;
            _files.Add(bo);
        }

        private void __readBookmarks(XElement bookmarks)
        {
            XName aName = "A";
            foreach (XElement x in bookmarks.Descendants(aName))
            {
                __processElement(x);
            }
        }

        private void __removeDups()
        {
            List<BookmarkObject> noDups = new List<BookmarkObject>();
            SortableBindingList<BookmarkObject> sbl = dataGridView1.DataSource as SortableBindingList<BookmarkObject>;
            List<string> keys = new List<string>();
            foreach (BookmarkObject bo in sbl)
            {
                string key = bo.Url;
                if (!keys.Contains(key))
                {
                    keys.Add(key);
                    noDups.Add(bo);
                }
            }
            _files = noDups;
            __writeToGrid();
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
            TextBoxPop tbp = new TextBoxPop(output);
            tbp.Show();
        }

        private void __writeToFile(List<BookmarkObject> lbo)
        {
            __writeToFile(new SortableBindingList<BookmarkObject>(lbo));
        }

        private void __writeToGrid()
        {
            tssl_Status.Text = "Writing to Grid";
            statusStrip1.Refresh();
            dataGridView1.DataSource = new SortableBindingList<BookmarkObject>(_files);
            tssl_Status.Text="OK";
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
                foreach (string fileName in ofd.FileNames)
                {
                    try
                    {
                        tssl_Status.Text = string.Format("Loading {0}", fileName);
                        statusStrip1.Refresh();
                        string file = File.ReadAllText(fileName);
                        file = __stripNetscapeHeader(file);
                        XElement bookmarks = __changeToXML(file);
                        __readBookmarks(bookmarks);
                        tssl_Status.Text = "OK";
                        statusStrip1.Refresh();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(DateTime.Now + ex.Message + fileName);
                    }
                }                
                __writeToGrid();                
                
            }
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __openFile();
        }

        private void __dataTable(XElement x) { 
        
         string _addDate;
         Image _icon;
         string _iconString;
         string _lastModified;
         string _name;
         string _parent;
         string _path;
         string _url;
        //shown: icon | title | path | good/bad | url | addDate
            //hidden: lastmodified
            //don't need: parent | iconstring

         DataTable dt = new DataTable();
         dt.Columns.Add("Icon", typeof(Image));
         dt.Columns.Add("Title", typeof(string));

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