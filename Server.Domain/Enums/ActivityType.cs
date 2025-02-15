using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Enums
{
    public enum ActivityType
    {
        // Board-level activities
        BoardCreated,
        BoardRenamed,
        BoardArchived,
        BoardUnarchived,
        BoardDeleted,

        // Column-related activities
        ColumnCreated,
        ColumnRenamed,
        ColumnDeleted,

        // Card-related activities
        CardCreated,
        CardUpdated,
        CardMoved,   
        CardDeleted,
        CardArchived,
        CardUnarchived
    }

}
