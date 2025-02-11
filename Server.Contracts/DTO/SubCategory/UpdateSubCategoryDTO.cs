using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.SubCategory
{
    public class UpdateSubCategoryDTO
    {
        public string? SubName { get; set; }
        public required int Status { get; set; }
    }
}
