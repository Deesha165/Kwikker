using Entities.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Tweet
    {
        public int ID { get; set; } // Primary Key
        public int UserID { get; set; } // Foreign Key
        public string ?Content { get; set; } 
        public string? MediaURL { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        //for counting
        public int LikesNumber { get; set; }
        public int RetweetsNumber { get; set; }
        public int BookmarksNumber {  get; set; } 

        // Navigation Properties
        public User User { get; set; } = null!; //no tweet without user
         
      

    }
}
