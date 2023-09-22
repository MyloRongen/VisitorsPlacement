using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorsPlacement_Bal.Classes
{
    public class EventSection
    {
        public string SectionName { get; private set; }
        public int NumSeats { get; private set; }

        public List<Row> Rows { get; private set; }

        public EventSection(string sectionName, int numSeats, int numRows)
        {
            SectionName = sectionName;
            Rows = new List<Row>(numRows);
            NumSeats = numSeats;

            InitializeRows(numRows);
        }

        private void InitializeRows(int numRows)
        {
            for (int i = 0; i < numRows; i++)
            {
                AddRow(numRows);
            }
        }

        public void AddRow(int numRows)
        {
            Rows.Add(new Row(SectionName, NumSeats, numRows));
        }

        public int CalculateAvailableSeats()
        {
            return Rows.Sum(row => row.CalculateAvailableSeats());
        }

        public bool ChildrenFitInTheFirstRow(List<Visitor> children)
        {
            int requiredSeats = children.Count + 1;
            Row? firstRow = Rows.FirstOrDefault();

            if (firstRow != null && firstRow.GetSeatCount() >= requiredSeats)
            {
                int availableSeats = firstRow.CalculateAvailableSeats();
                return availableSeats >= requiredSeats;
            }

            return false;
        }

        public bool CanAssignGroupWithChildren(List<Visitor> children)
        {
            return children.Count != 0 && ChildrenFitInTheFirstRow(children);
        }

        public static bool CanAssignGroupWithOnlyAdults(List<Visitor> children, bool groupAssigned)
        {
            return children.Count == 0 && !groupAssigned;
        }
    }
}
