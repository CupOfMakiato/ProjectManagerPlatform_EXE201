using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string FileType { get; set; }
        public string FilePublicId { get; set; }
        public bool IsCover { get; set; } = false;

        public Guid CardId { get; set; }

        [ForeignKey("CardId")]
        public Card Card { get; set; }
    }

}
