using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorsPlacement_Bal.Enums;
/*using static System.Collections.Specialized.BitVector32;*/

namespace VisitorsPlacement_Bal.Classes
{
    public class Stadium
    {
        private readonly List<Visitor> visitors;
        private readonly List<VisitorGroup> visitorGroups;
        private readonly List<EventSection> sections;
        private GroupAssignmentCounts groupAssignmentCounts = new();

        public Stadium()
        {
            visitors = new List<Visitor>();
            visitorGroups = new List<VisitorGroup>();
            sections = new List<EventSection>();
        }

        public void RegisterVisitor(string name, DateTime birthDate, ConsoleColor colorGroup)
        {
            Visitor visitor = new(name, birthDate, colorGroup);
            visitors.Add(visitor);
        }

        public void CreateVisitorGroup(string groupName, List<string> memberNames)
        {
            List<Visitor> groupMembers = new();
            foreach (string memberName in memberNames)
            {
                Visitor? member = visitors.Find(v => v.Name == memberName);
                if (member != null)
                {
                    groupMembers.Add(member);
                }
            }

            VisitorGroup group = new(groupName, groupMembers);
            visitorGroups.Add(group);
        }

        public void CreateSection(string sectionName, int numRows, int numSeatsPerRow)
        {
            EventSection section = new(sectionName, numSeatsPerRow, numRows);
            sections.Add(section);
        }

        public void AssignVisitors()
        {
            foreach (VisitorGroup group in visitorGroups)
            {
                List<Visitor> children = group.Members.Where(member => member.Age <= 12).ToList();
                List<Visitor> adults = group.Members.Where(member => member.Age > 12).ToList();

                AssignGroupToSection(group, children, adults);
            }

            RemoveSectionsWithNoVisitors();
        }

        private void AssignGroupToSection(VisitorGroup group, List<Visitor> children, List<Visitor> adults)
        {
            bool groupAssigned = false;

            foreach (EventSection section in sections)
            {
                if (GroupFitsInTheSection(group, section) && TryAssignGroupToSection(section, children, adults, groupAssigned))
                {
                    groupAssigned = true;
                    break;
                }
            }
        }

        private bool TryAssignGroupToSection(EventSection section, List<Visitor> children, List<Visitor> adults, bool groupAssigned)
        {
            if (section.CanAssignGroupWithChildren(children) || EventSection.CanAssignGroupWithOnlyAdults(children, groupAssigned))
            {
                AssignGroupToSeats(children, adults, section);
                return true;
            }

            return false;
        }

        private static bool GroupFitsInTheSection(VisitorGroup group, EventSection currentSection)
        {
            int availableSeatsCount = currentSection.CalculateAvailableSeats();

            if (group.Members.Count <= availableSeatsCount)
            {
                return true;
            }

            return false;
        }

        private void AssignGroupToSeats(List<Visitor> children, List<Visitor> adults, EventSection section)
        {
            groupAssignmentCounts = new GroupAssignmentCounts();

            foreach (Row row in section.Rows)
            {
                List<Seat>? seatsAvailable = row.GetAvailableSeats();
                ProcessRowForSpace(seatsAvailable, children, adults, section.Rows.Count);
                
                if (IsGroupPlaced(children, adults))
                {
                    return;
                }
            }
        }

        private void ProcessRowForSpace(List<Seat>? seatsAvailable, List<Visitor> children, List<Visitor> adults, int numRows)
        {
            for (int i = 0; i < seatsAvailable?.Count; i++)
            {
                Seat seat = seatsAvailable[i];

                if (ChildInGroup(seat, children))
                {         
                    AssignChildToSeat(seat, children);
                    AssignOneAdultIfPossible(children, adults, numRows, seatsAvailable, i);
                }
                else if (OnlyAdultsInGroup(seat, adults))
                {
                    AssignAdultToSeat(seat, adults);
                }
                else
                {
                    break;
                }
            }
        }

        private void AssignOneAdultIfPossible(List<Visitor> children, List<Visitor> adults, int numRows, List<Seat>? seatsAvailable, int currentIndex)
        {
            if (groupAssignmentCounts.ChildIndex == children.Count && groupAssignmentCounts.AdultIndex < adults.Count && numRows > 1)
            {
                AssignAdultToNextRowIfAvailable(adults, seatsAvailable, currentIndex);
            }
        }

        private bool IsGroupPlaced(List<Visitor> children, List<Visitor> adults)
        {
            return groupAssignmentCounts.ChildIndex >= children.Count && groupAssignmentCounts.AdultIndex >= adults.Count;
        }

        private bool ChildInGroup(Seat seat, List<Visitor> children)
        {
            if (groupAssignmentCounts.ChildIndex < children.Count && seat.Availability == SeatStatus.Available)
            {
                return true;
            }

            return false;
        }

        private bool OnlyAdultsInGroup(Seat seat, List<Visitor> adults)
        {
            if (groupAssignmentCounts.AdultIndex < adults.Count && seat.Availability == SeatStatus.Available)
            {
                return true;
            }

            return false;
        }

        private void AssignChildToSeat(Seat seat, List<Visitor> children)
        {
            seat.AssignVisitor(children[groupAssignmentCounts.ChildIndex]);
            groupAssignmentCounts.ChildIndex++;
        }

        private void AssignAdultToSeat(Seat seat, List<Visitor> adults)
        {
            seat.AssignVisitor(adults[groupAssignmentCounts.AdultIndex]);
            groupAssignmentCounts.AdultIndex++;
        }

        private void AssignAdultToNextRowIfAvailable(List<Visitor> adults, List<Seat>? seatsAvailable, int currentIndex)
        {
            if (groupAssignmentCounts.AdultIndex < adults.Count && currentIndex < seatsAvailable?.Count - 1)
            {
                Seat? adultSeat = seatsAvailable?[currentIndex + 1];
                adultSeat?.AssignVisitor(adults[groupAssignmentCounts.AdultIndex]);
                groupAssignmentCounts.AdultIndex++;
            }
        }

        public int GetSectionCount()
        {
            return sections.Count;
        }

        public List<EventSection> GetSectionsFromStadium()
        {
            return sections;
        }

        public List<VisitorGroup> GetVisitorGroupsFromStadium()
        {
            return visitorGroups;
        }

        private void RemoveSectionsWithNoVisitors()
        {
            List<EventSection> sectionsToRemove = new();

            foreach (EventSection section in sections)
            {
                bool hasVisitor = section.Rows.Any(row => row.Seats.Any(seat => seat.AssignedVisitor != null));

                if (!hasVisitor)
                {
                    sectionsToRemove.Add(section);
                }
            }

            foreach (EventSection sectionToRemove in sectionsToRemove)
            {
                sections.Remove(sectionToRemove);
            }
        }
    }
}