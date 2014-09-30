using System;
using System.Collections.Generic;

namespace BookmarkManager
{
    internal class FolderObject
    {
        public class FolderObject
        {
            private List<string> _bookmarks;
            private List<FolderObject> _folders;
            private bool _isRoot;
            private string _name;
            private FolderObject _parent;

            public FolderObject(string name)
            {
                this._name = name;

                _isRoot = false;
                _bookmarks = new List<string>();
                _folders = new List<FolderObject>();
            }

            public bool IsRoot
            {
                get { return _isRoot; }
                set { _isRoot = value; }
            }

            public string Name
            {
                get { return _name; }
            }
            public void Add(BookmarkObject bo){
            
            }

            private void __add(BookmarkObject bo, string currentPath) { }

            public string ToString()
            {
                string output = string.Empty;
                string name = _isRoot ? string.Format("<H1>{0}</H1>", _name) : string.Format("<H3>{0}</H3>", _name);
                output = string.Format("<DT>{0}{1}<DL><p>{1}", name, Environment.NewLine);
                foreach (FolderObject fo in _folders)
                {
                    output += fo.ToString();
                }
                foreach (string s in _bookmarks)
                {
                    output += s;
                }
                output = string.Format("{0}</DL><p>{1}", output, Environment.NewLine);
                return output;
            }
        }
    }
}