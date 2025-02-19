using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server.Domain.Entities
{
    public class Activity : BaseEntity
    {
        //Test first 
        public Guid UserId { get; set; }
        public Guid? CardId { get; set; }
        public Guid? BoardId { get; set; }
        public Guid? ColumnId { get; set; }
        public ActivityType? Type { get; set; }
        public string Data { get; set; }
        //public string Status { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }
        [ForeignKey("BoardId")]
        public Board Board { get; set; }
        [ForeignKey("ColumnId")]
        public Column Column { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public void SetData(Dictionary<string, object> details)
        {
            Data = JsonSerializer.Serialize(details);
        }

        public Dictionary<string, object> GetData()
        {
            return string.IsNullOrEmpty(Data) ? new Dictionary<string, object>() :
                JsonSerializer.Deserialize<Dictionary<string, object>>(Data);
        }
    }
}
