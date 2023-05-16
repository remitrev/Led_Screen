using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Windows.Devices.PointOfService;

namespace Led_Screen.MVVM.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public string Tag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUsedDate { get; set; }

        public Message(Guid id, string content, string tag, DateTime createdDate, DateTime lastUsedDate)
        {
            this.Id = id;
            this.Content = content;
            this.Tag = tag;
            CreatedDate = createdDate;
            LastUsedDate = lastUsedDate;
        }

        public void UpdateLastUpdate(DateTime newLastUpdateDate) {
            LastUsedDate = newLastUpdateDate;
        }
    }
}
