﻿using Server.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Contracts.Abstractions.RequestAndResponse.Card
{
    public class AddNewCardRequest
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ColumnId { get; set; }
        public string Title { get; set; }
        //public AssignedCompletion AssignedCompletion { get; set; }
    }
}
