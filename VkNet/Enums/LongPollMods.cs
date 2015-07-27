using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VkNet.Enums
{
    [Flags]
    public enum LongPollMods
    {
        None = 0,
        IncludeAttachemnts = 2,
        IncludeExtendedEvents = 8,
        ReturnPts = 32,
        IncludeOnlineEventAdditionalInfo = 64
    }
}
