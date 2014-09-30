using System;

namespace BookmarkManager
{
    public class BookmarkObject
    {
        private string _addDate;
        private string _icon;
        private string _lastModified;
        private int _level;
        private string _name;
        private string _parent;
        private string _path;
        private string _type;
        private string _url;

        public BookmarkObject()
        {
            _type = "Folder";
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

        public string LastModified
        {
            get { return _lastModified; }
            set { _lastModified = value; }
        }

        public int Level
        {
            get { return _level; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public string Path
        {
            get { return _path; }
            set
            {
                _path = value;
                _level = _path.Length - _path.Replace(@"\", "").Length;
            }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; _type = "URL"; }
        }

        public string ToString()
        {
            string output = string.Empty;
            string addDate = (string.IsNullOrEmpty(_addDate)) ? string.Empty : string.Format("ADD_DATE=\"{0}\" ", _addDate);
            string icon = (string.IsNullOrEmpty(_icon)) ? string.Empty : string.Format("ICON=\"{0}\" ", _icon);
            string lastModified = (string.IsNullOrEmpty(_lastModified)) ? string.Empty : string.Format("LAST_MODIFIED=\"{0}\" ", _lastModified);
            output = string.Format("<DT><A HREF=\"{0}\" {1}{2}{3}>{4}</A>{5}", _url, addDate, icon, lastModified, _name, Environment.NewLine);
            return output;
        }
    }
}