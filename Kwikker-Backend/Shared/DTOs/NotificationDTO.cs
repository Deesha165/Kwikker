using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs
{
    public record NotificationDTO( int id,string type,DateTime createdAt, 
        int senderId,string senderUserName,string senderEmail,string senderProfilePicture
        );
}
