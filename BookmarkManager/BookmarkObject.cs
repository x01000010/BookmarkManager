using System;

namespace BookmarkManager
{
    internal class BookmarkObject
    {
        private string _addDate;
        private string _icon;
        private string _lastModified;
        private string _name;
        private string _parent;
        private string _path;
        private string _url;

        public BookmarkObject()
        {
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
            set { _path = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public override string ToString()
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