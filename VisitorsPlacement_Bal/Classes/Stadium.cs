using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorsPlacement_Bal.Enums;
using static System.Collections.Specialized.BitVector32;

namespace VisitorsPlacement_Bal.Classes
{
    public class Stadium
    {
        private readonly List<Visitor> visitors;
        public List<VisitorGroup> visitorGroups;
        public List<EventSection> sections;

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
        }

        private void AssignGroupToSection(VisitorGroup group, List<Visitor> children, List<Visitor> adults)
        {
            bool groupAssigned = false;

            foreach (EventSection section in sections)
            {
                if (GroupFitsInTheSection(group, section) && TryAssignGroupToSection(section, children, adults, ref groupAssigned))
                {
                    break;
                }
            }
        }

        private static bool TryAssignGroupToSection(EventSection section, List<Visitor> children, List<Visitor> adults, ref bool groupAssigned)
        {
            if (section.CanAssignGroupWithChildren(children) || EventSection.CanAssignGroupWithOnlyAdults(children, groupAssigned))
            {
                AssignGroupToSeats(children, adults, section);
                groupAssigned = true;
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

        private static void AssignGroupToSeats(List<Visitor> children, List<Visitor> adults, EventSection section)
        {
            int childIndex = 0;
            int adultIndex = 0;

            foreach (Row row in section.Rows)
            {
                List<Seat>? seatsAvailable = row.GetAvailableSeats();
                ProcessRowForSpace(seatsAvailable, children, adults, ref childIndex, ref adultIndex, section.Rows.Count);
                
                if (IsGroupPlaced(childIndex, adultIndex, children, adults))
                {
                    return;
                }
            }
        }

        private static void ProcessRowForSpace(List<Seat>? seatsAvailable, List<Visitor> children, List<Visitor> adults, ref int childIndex, ref int adultIndex, int numRows)
        {
            for (int i = 0; i < seatsAvailable?.Count; i++)
            {
                Seat seat = seatsAvailable[i];

                if (ChildInGroup(seat, children, ref childIndex))
                {         
                    AssignChildToSeat(seat, children, ref childIndex);
                    AssignOneAdultIfPossible(children, adults, ref childIndex, ref adultIndex, numRows, seatsAvailable, i);
                }
                else if (OnlyAdultsInGroup(seat, adults, ref adultIndex))
                {
                    AssignAdultToSeat(seat, adults, ref adultIndex);
                }
                else
                {
                    break;
                }
            }
        }

        private static void AssignOneAdultIfPossible(List<Visitor> children, List<Visitor> adults, ref int childIndex, ref int adultIndex, int numRows, List<Seat>? seatsAvailable, int currentIndex)
        {
            if (childIndex == children.Count && adultIndex < adults.Count && numRows > 1)
            {
                AssignAdultToNextRowIfAvailable(adults, ref adultIndex, seatsAvailable, currentIndex);
            }
        }

        private static bool IsGroupPlaced(int childIndex, int adultIndex, List<Visitor> children, List<Visitor> adults)
        {
            return childIndex >= children.Count && adultIndex >= adults.Count;
        }

        private static bool ChildInGroup(Seat seat, List<Visitor> children, ref int childIndex)
        {
            if (childIndex < children.Count && seat.Availability == SeatStatus.Available)
            {
                return true;
            }

            return false;
        }

        private static bool OnlyAdultsInGroup(Seat seat, List<Visitor> adults, ref int adultIndex)
        {
            if (adultIndex < adults.Count && seat.Availability == SeatStatus.Available)
            {
                return true;
            }

            return false;
        }

        private static void AssignChildToSeat(Seat seat, List<Visitor> children, ref int childIndex)
        {
            seat.AssignVisitor(children[childIndex]);
            childIndex++;
        }

        private static void AssignAdultToSeat(Seat seat, List<Visitor> adults, ref int adultIndex)
        {
            seat.AssignVisitor(adults[adultIndex]);
            adultIndex++;
        }

        private static void AssignAdultToNextRowIfAvailable(List<Visitor> adults, ref int adultIndex, List<Seat>? seatsAvailable, int currentIndex)
        {
            if (adultIndex < adults.Count && currentIndex < seatsAvailable?.Count - 1)
            {
                Seat? adultSeat = seatsAvailable?[currentIndex + 1];
                adultSeat?.AssignVisitor(adults[adultIndex]);
                adultIndex++;
            }
        }
    }
}