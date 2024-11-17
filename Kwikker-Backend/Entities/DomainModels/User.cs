using Entities.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Entities.Models
{
    public class User:IdentityUser<int>
    {
   
     
        public string? ProfilePicture { get; set; } // Could be a URL or a file path
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Tweet> Tweets { get; set; }=new List<Tweet>();   
        public ICollection<Follow> Followers { get; set; } = new HashSet<Follow>();    
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();

        public ICollection<Notification> notifications { get; set; } = new List<Notification>();

    }
}
