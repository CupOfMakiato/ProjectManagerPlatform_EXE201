using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.DTO.SubCategory
{
    public class CreateSubCategoryDTO
    {
        public required string SubName { get; set; }
        public required int Status { get; set; }
    }
}
