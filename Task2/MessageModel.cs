using System.ComponentModel.DataAnnotations;

namespace Task2
{
    public class MessageModel
    {
        [Required]
        [EmailAddress]
        public string Receiver { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
