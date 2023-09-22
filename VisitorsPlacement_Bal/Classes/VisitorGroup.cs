using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorsPlacement_Bal.Classes
{
    public class VisitorGroup
    {
        public string GroupName { get; }
        public List<Visitor> Members { get; }

        public VisitorGroup(string groupName, List<Visitor> members)
        {
            GroupName = groupName;
            Members = members;
        }
    }
}
