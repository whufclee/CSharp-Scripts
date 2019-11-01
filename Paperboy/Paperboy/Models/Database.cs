using System;
using SQLite;

namespace Paperboy.News
{
    public class SocialFeeds
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Uri { get; set; } // Uri to bit.ly or similar
    }

    public class Links
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Url { get; set; } // Link to the content
        public string Icon { get; set; } // Default to default image
        public string Name { get; set; } // Link to the content
        public string Type { get; set; } // RSS/Regex/YouTube/Telegram/Blog
        public int Enabled { get; set; } // Option to enable/disable certain links
    }

    public class Articles
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; } // Article Title
        public string Date { get; set; } // Article Date
        [Unique]
        public string Uri { get; set; } // Uri to full article
        public string Content { get; set; } // Content of custom blog
    }
}