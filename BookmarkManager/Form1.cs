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
        private static XName _lastModified = "LAST_MODIFIED";
        private static string _endOfFile = "/DL><p>";
        private static XName _href = "HREF";
        private static XName _icon = "ICON";
        private static string _startOfFile = @"<!DOCTYPE NETSCAPE-Bookmark-file-1>
<!-- This is an automatically generated file.
     It will be read and overwritten.
     DO NOT EDIT! -->
<META HTTP-EQUIV=""Content-Type"" CONTENT=""text/html; charset=UTF-8"">
<TITLE>Bookmarks</TITLE>
<H1>Bookmarks</H1>
<DL><p>";

        private XElement _bookmarks;

        private List<Bookmark> _files = new List<Bookmark>();

        private List<Folder> _folders = new List<Folder>();

        public Form1()
        {
            InitializeComponent();
        }

        private XElement __changeToXML(string s)
        {
            XElement root = null;
            s = s.Replace("</H3>", "</H3></DT>");
            s = s.Replace("</A>", "</A></DT>");
            s = s.Replace("<p>", string.Empty);
            s = s.Replace("&", "&amp;");
            s = "<root>" + s + "</root>";
            root = XElement.Parse(s);
            return root;
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

        private void __proccessFile(XElement x, string name)
        {
            Bookmark b = new Bookmark();
            b.Name = x.Value;
            b.Path = name;
            b.Url = x.Attribute(_href).Value;
            b.Icon = x.Attribute(_icon).Value;
            b.AddDate = x.Attribute(_addDate).Value;
            _files.Add(b);
        }

        private void __proccessFolder(XElement folder, XElement sub, string name)
        {
            Folder f = new Folder();
            f.Name = folder.Value;
            f.Path = name;
            f.AddDate = folder.Attribute(_addDate).Value;
            f.LastModified = folder.Attribute(_lastModified).Value;
            _folders.Add(f);
            __processXML(sub, name + "," + folder.Value);

        }

        private void __processXML(XElement x, string name)
        {
            x = __nestDLinDT(x);
            foreach (XElement element in x.Elements())
            {
                if (element.Name.ToString() == "A")
                {
                    __proccessFile(element, name);
                }
                else if (element.Name.ToString() == "H3" && ((XElement)element.NextNode).Name.ToString() == "DL")
                {
                    __proccessFolder(element, element.NextNode as XElement, name);
                }
            }
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
            int count = 0;
            XElement x = __nestDLinDT(_bookmarks);
            XElement y = null;
            string name = "bookmarks";
            foreach (XElement element in x.Elements())
            {
                __processXML(element, name);
                count++;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (Bookmark b in _files)
            {
                richTextBox1.AppendText(b.ToString());
            }
            foreach (Folder f in _folders)
            {
                richTextBox1.AppendText(f.ToString());
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}