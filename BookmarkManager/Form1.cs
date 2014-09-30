using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace BookmarkManager
{
    public partial class Form1 : Form
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

        private XElement _bookmarks;
        private List<BookmarkObject> _files = new List<BookmarkObject>();

        public Form1()
        {
            InitializeComponent();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private XElement __changeToXML(string s)
        {
            //need to fix incoming text
            //&#58;
            //&gt;
            //&quot;

            XElement root = null;
            s = s.Replace("</A>", "</A></DT>");
            s = s.Replace("<DL><p>", "<DL>");
            s = s.Replace("</DL><p>", "</DL></DT>");
            s = __fixAndReplaceAmp(s);
            s = "<bookmarks>" + s + "</bookmarks>";
            root = XElement.Parse(s);
            return root;
        }

        private Dictionary<string, List<BookmarkObject>> __createDictionary(SortableBindingList<BookmarkObject> sbl)
        {
            Dictionary<string, List<BookmarkObject>> dict = new Dictionary<string, List<BookmarkObject>>();

            foreach (BookmarkObject bo in sbl)
            {
                if (dict.ContainsKey(bo.Path))
                {
                    dict[bo.Path].Add(bo);
                }
                else
                {
                    List<BookmarkObject> l = new List<BookmarkObject>();
                    l.Add(bo);
                    dict.Add(bo.Path, l);
                }
            }
            return dict;
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
            XElement previousPrevious = x.Parent.Parent;
            if (previousPrevious.Name == "bookmarks")
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
            if (previousPrevious.Name == "bookmarks")
            {
                return string.Format(@"{0}\{1}", previousPrevious.Name, currentPath);
            }
            else
            {
                XElement previousPreviousSibling = previousPrevious.Parent.Element(_h3);
                return __getPath(previousPreviousSibling, string.Format(@"{0}\{1}", previousPreviousSibling.Value, currentPath));
            }
        }

        private XElement __nestDLinDT(XElement root)
        {
            XElement newRoot = root;
            foreach (XElement element in newRoot.Elements())
            {
                XElement next = element.NextNode as XElement;
                if (next != null)
                {
                    if (element.Name.ToString() == "DT" && next.Name.ToString() == "DL")
                    {
                        element.NextNode.Remove();
                        element.Add(next);
                    }
                }
            }
            return newRoot;
        }

        private void __processElement(XElement x)
        {
            BookmarkObject bo = new BookmarkObject();
            bo.Name = x.Value;
            bo.Path = __getPath(x, string.Empty);
            bo.Parent = __getParent(x);
            bo.Url = (x.Attribute(_href) == null) ? string.Empty : x.Attribute(_href).Value;
            bo.Icon = (x.Attribute(_icon) == null) ? string.Empty : x.Attribute(_icon).Value;
            bo.AddDate = (x.Attribute(_addDate) == null) ? string.Empty : x.Attribute(_addDate).Value;
            bo.LastModified = (x.Attribute(_lastModified) == null) ? string.Empty : x.Attribute(_lastModified).Value;
            _files.Add(bo);
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
            dataGridView1.DataSource = new SortableBindingList<BookmarkObject>(noDups);
        }

        private string __stripNetscapeHeader(string s)
        {
            string folder = string.Empty;
            int idx = s.IndexOf("<DT>");
            folder = s.Substring(idx);
            idx = folder.LastIndexOf("</DL><p>");
            folder = folder.Substring(0, idx);
            return folder;
        }

        private void __writeToFile(SortableBindingList<BookmarkObject> bookmarks)
        {
            SortableBindingList<BookmarkObject> sbl = dataGridView1.DataSource as SortableBindingList<BookmarkObject>;
        }

        private void __writeToGrid()
        {
            dataGridView1.DataSource = new SortableBindingList<BookmarkObject>(_files);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string file = File.ReadAllText(ofd.FileName);
                file = __stripNetscapeHeader(file);
                _bookmarks = __changeToXML(file);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string file = File.ReadAllText(ofd.FileName);
                file = __stripNetscapeHeader(file);
                _bookmarks = __changeToXML(file);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            XName aName = "A";
            foreach (XElement x in _bookmarks.Descendants(aName))
            {
                __processElement(x);
            }
            __writeToGrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            foreach (BookmarkObject bo in _files)
            {
                if (bo.Name.Contains("&"))
                {
                    richTextBox1.AppendText(string.Format("{0}:{1}{2}", bo.Name, bo.Url, Environment.NewLine));
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            __removeDups();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            __writeToFile(dataGridView1.DataSource as SortableBindingList<BookmarkObject>);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string file = File.ReadAllText(ofd.FileName);
                file = __stripNetscapeHeader(file);
                _bookmarks = __changeToXML(file);
            }
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