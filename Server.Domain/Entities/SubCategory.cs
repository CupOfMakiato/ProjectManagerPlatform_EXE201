﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Entities
{
    public class SubCategory : BaseEntity
    {
        public string SubCategoryName { get; set; }
        //public int Status { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
