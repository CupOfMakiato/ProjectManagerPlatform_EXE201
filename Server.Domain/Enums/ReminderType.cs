using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Domain.Enums
{
    public enum ReminderType
    {
        None = 1,
        AtTimeOfDueDate = 2,
        FiveMinutes = 3,  
        TenMinutes = 4,   
        OneHour = 5,      
        OneDay = 6,       
        TwoDays = 7,      
    }
}
