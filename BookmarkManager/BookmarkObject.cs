using System;
using System.Drawing;
using System.IO;

namespace BookmarkManager
{
    internal class BookmarkObject
    {
        private string _addDate;
        private Image _icon;
        private string _iconString;
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

        public Image Icon
        {
            set { _icon = value; }
            get { return _icon; }
        }

        public string IconString
        {
            get { return _iconString; }
            set { _icon = __setIcon(value); _iconString = value; }
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

        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            return origin.AddSeconds(timestamp).ToLocalTime();
        }

        public static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                // Convert byte[] to Image
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public string ImageToBase64(Image image,
  System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public override string ToString()
        {
            string output = string.Empty;
            string addDate = (string.IsNullOrEmpty(_addDate)) ? string.Empty : string.Format("ADD_DATE=\"{0}\" ", _addDate);
            string iconString = (string.IsNullOrEmpty(_iconString)) ? string.Empty : string.Format("ICON=\"{0}\" ", _iconString);
            string lastModified = (string.IsNullOrEmpty(_lastModified)) ? string.Empty : string.Format("LAST_MODIFIED=\"{0}\" ", _lastModified);
            output = string.Format("<DT><A HREF=\"{0}\" {1}{2}{3}>{4}</A>{5}", _url, addDate, iconString, lastModified, _name, Environment.NewLine);
            return output;
        }

        private Image __setIcon(string value)
        {
            Image icon = null;
            if (!string.IsNullOrEmpty(value))
            {
                string imageString = value.Substring(value.IndexOf(",") + 1);

                icon = Base64ToImage(imageString);
            }
            return icon;
        }
    }
}