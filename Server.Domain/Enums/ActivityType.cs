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
        ColumnArchived,
        ColumnUnarchived,
        ColumnMoved,
        ColumnDeleted,

        // Card-related activities
        CardCreated,
        CardUpdated,
        CardArchived,
        CardUnarchived,
        CardMoved,   
        CardDeleted,
        
    }

}
