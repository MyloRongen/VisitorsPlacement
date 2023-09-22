using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorsPlacement_Bal.Enums;

namespace VisitorsPlacement_Bal.Classes
{
    public class Seat
    {
        public string SeatCode { get; set; }

        public SeatStatus Availability { get; private set; }

        public Visitor? AssignedVisitor { get; private set; }

        public int Row { get; }

        public Seat(string seatCode, int row)
        {
            Row = row;
            SeatCode = seatCode;
            Availability = SeatStatus.Available;
        }

        public void AssignVisitor(Visitor visitor)
        {
            AssignedVisitor = visitor;
            Availability = SeatStatus.Assigned;
        }
    }
}
