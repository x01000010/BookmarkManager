using System;
using System.Collections.Generic;

namespace BookmarkManager
{
    internal class FolderObject
    {
        private List<string> _bookmarks;
        private List<FolderObject> _folders;
        private bool _isRoot;
        private string _name;
        private FolderObject _parent;

        public FolderObject(string name, FolderObject fo)
        {
            this._name = name;

            _isRoot = false;
            _bookmarks = new List<string>();
            _folders = new List<FolderObject>();
            _parent = fo;
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

        internal FolderObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public void Add(BookmarkObject bo)
        {
            if (bo.Parent == this._name)
            {
                _bookmarks.Add(bo.ToString());
            }
            else
            {
                string nextPath = string.Empty;
                string nextFolder = string.Empty;
                string tmpPath = string.Empty;
                tmpPath = __getNextPath(bo.Path);
                nextFolder = __getNextFolder(tmpPath);
                nextPath = __getNextPath(tmpPath);
                FolderObject fo = __getFolder(nextFolder);
                fo.Parent = this;
                fo.__add(bo, nextPath);
            }
        }

        public override string ToString()
        {
            string output = string.Empty;
            string name = _isRoot ? string.Format("<H1>{0}</H1>", _name) : string.Format("<DT><H3>{0}</H3>", _name);
            output = string.Format("{0}{1}<DL><p>{1}", name, Environment.NewLine);
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

        private void __add(BookmarkObject bo, string currentPath)
        {
            if (string.IsNullOrEmpty(currentPath))
            {
                _bookmarks.Add(bo.ToString());
            }
            else
            {
                string nextPath = string.Empty;
                string nextFolder = string.Empty;
                nextFolder = __getNextFolder(currentPath);
                nextPath = __getNextPath(currentPath);
                FolderObject fo = __getFolder(nextFolder);
                fo.Parent = this;
                fo.__add(bo, nextPath);
            }
        }

        private FolderObject __getFolder(string folderName)
        {
            foreach (FolderObject fo in _folders)
            {
                if (fo.Name == folderName)
                {
                    return fo;
                }
            }
            FolderObject newFO = new FolderObject(folderName, this); _folders.Add(newFO);
            return newFO;
        }

        private string __getNextFolder(string str)
        {
            string nextFolder = string.Empty;
            nextFolder = str.Substring(0, str.IndexOf("\\"));
            return nextFolder;
        }

        private string __getNextPath(string str)
        {
            string nextPath = string.Empty;
            if (!(str.IndexOf("\\") == str.LastIndexOf("\\")))
            {
                nextPath = str.Substring(str.IndexOf("\\") + 1);
            }
            return nextPath;
        }
    }
}