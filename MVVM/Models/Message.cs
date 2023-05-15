using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Led_Screen.MVVM.Models
{

    [Table("Messages_dev")]
    public class Message
    {
        [Key]
        public Guid Id { get; protected set; }
        [Required]
        public string Content { get; protected set; }
        [Required]
        public string Tag { get; protected set; }

        public Message(Guid id, string content, string tag)
        {
            this.Id = id;
            this.Content = content;
            this.Tag = tag;
        }
    }
}
