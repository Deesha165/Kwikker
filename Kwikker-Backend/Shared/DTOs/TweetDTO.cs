using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record TweetDTO
(
    int id,
    string content, // Default to an empty string
    int userId,
    DateTime createdAt, // Default to an empty string
    string userName,
    int likesNumber,
    int retweetsNumber,
    int bookmarksNumber,
    string email,
      string profilePicture = "",
       string mediaUrl = ""
);
}
