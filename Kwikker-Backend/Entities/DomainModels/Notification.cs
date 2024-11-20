
using Entities.Models;


namespace Entities.DomainModels
{
    public class Notification
    {
        public int Id { get; set; }  

        public int ReceiverId { get; set; }  
        public string? Type { get; set; }  
     
        public int SenderId { get; set; } 
    
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public bool IsRead { get; set; }  

     
        public  User? Receiver { get; set; }  
    
        public  User? Sender { get; set; }

    }
}
