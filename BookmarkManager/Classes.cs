using System;

namespace BookmarkManager
{
    public class Bookmark
    {
        private static string type = "URL";
        private string _addDate;
        private string _icon;
        private string _name;
        private string _parent;
        private string _path;
        private string _url;

        public Bookmark()
        {
        }

        public static string Type
        {
            get { return Bookmark.type; }
        }

        public string AddDate
        {
            get { return _addDate; }
            set { _addDate = value; }
        }

        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string ToString()
        {
            string output = string.Format("{0}{1}:{2}{0}{3}", "************", type, _name, Environment.NewLine);
            return output;
        }
    }

    public class Folder
    {
        private static string type = "Folder";
        private string _addDate;
        private string _lastModified;
        private string _name;
        private string _parent;
        private string _path;

        public Folder()
        {
        }

        public static string Type
        {
            get { return Folder.type; }
        }

        public string AddDate
        {
            get { return _addDate; }
            set { _addDate = value; }
        }

        public string LastModified
        {
            get { return _lastModified; }
            set { _lastModified = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string ToString()
        {
            string output = string.Format("{0}{1}:{2}{0}{3}", "************", type, _name, Environment.NewLine);
            return output;
        }
    }
}