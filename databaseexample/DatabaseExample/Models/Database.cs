using SQLite;
using System;

namespace DatabaseExample.Models
{
    // Social Feed Links
    public class SocialFeeds
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Uri { get; set; } // Uri to bit.ly or similar
    }

    // Individual website details (taken from social feed)
    public class Links
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Url { get; set; } // Final link to the content
        public string OrigUrl { get; set; } // Link to the original content
        public string Icon { get; set; } // Default to default image
        public string Name { get; set; } // Name of the website
        public int Offline { get; set; } // Website offline last time checked
        public int Enabled { get; set; } // Option to enable/disable certain links
        public override string ToString()
        {
            return this.Name;
        }
    }

    // Individual article details
    public class Articles
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Icon { get; set; } // Default to default image
        [Unique]
        public string Uri { get; set; } // Uri to full article
        //public string FeedId { get; set; } // Article Date
        public string Title { get; set; } // Article Title
        public DateTime Date { get; set; } // Article Date
        public string Content { get; set; } // Content of custom blog
        public int Viewed { get; set; } // Status or read or unread
        public string Website { get; set; } // Assign to the authors website name
        public string Domain { get; set; } // Assign to the authors website url
        public override string ToString()
        {
            return this.Title;
        }
    }

    // Temp object to store URL contents
    public class UrlDetails
    {
        public string Title { get; set; }
        public string FinalUrl { get; set; }
        public string Contents { get; set; }
        public string IconUrl { get; set; }
        public string Icon { get; set; }
        public string OriginalUrl { get; set; }
        public string Domain { get; set; }
    }
}