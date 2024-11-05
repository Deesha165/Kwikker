using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DomainModels
{
    public class Trend
    {
        public int Id { get; set; }
        public string hashtag { get; set; } = null!;
        public int Occurrences { get; set; }  // Number of times the hashtag appeared
        public DateTime LastOccurred { get; set; } // Last time the hashtag was used
        public double DecayScore { get; set; }  // Calculated decay score based on time and ocuurence

        public ICollection<TweetTrend> TweetTrends { get; set; } = new List<TweetTrend>();
    }
}
