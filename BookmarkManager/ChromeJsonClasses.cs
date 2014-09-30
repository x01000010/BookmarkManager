using System.Collections.Generic;

namespace BookmarkManager
{
    public class BookmarkBar
    {
        public List<object> children { get; set; }

        public string date_added { get; set; }

        public string date_modified { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string sync_transaction_version { get; set; }

        public string type { get; set; }
    }

    public class Child
    {
        public List<object> children { get; set; }

        public string date_added { get; set; }

        public string date_modified { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string sync_transaction_version { get; set; }

        public string type { get; set; }

        public string url { get; set; }
    }

    public class Child2
    {
        public string date_added { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string sync_transaction_version { get; set; }

        public string type { get; set; }

        public string url { get; set; }
    }

    public class Other
    {
        public List<Child> children { get; set; }

        public string date_added { get; set; }

        public string date_modified { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string sync_transaction_version { get; set; }

        public string type { get; set; }
    }

    public class RootObject
    {
        public string checksum { get; set; }

        public Roots roots { get; set; }

        public int version { get; set; }
    }

    public class Roots
    {
        public BookmarkBar bookmark_bar { get; set; }

        public Other other { get; set; }

        public string sync_transaction_version { get; set; }

        public Synced synced { get; set; }
    }

    public class Synced
    {
        public List<Child2> children { get; set; }

        public string date_added { get; set; }

        public string date_modified { get; set; }

        public string id { get; set; }

        public string name { get; set; }

        public string sync_transaction_version { get; set; }

        public string type { get; set; }
    }
}