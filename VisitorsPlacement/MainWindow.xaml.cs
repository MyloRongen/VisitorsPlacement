using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisitorsPlacement_Bal.Classes;
using VisitorsPlacement_Bal.Enums;

namespace VisitorsPlacement
{

    public static partial class Kernel32
    {
        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool AllocConsole();

        [LibraryImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static partial bool FreeConsole();
    }

    public partial class MainWindow : Window
    {
        private readonly Stadium stadium = new();
        private readonly Random rnd = new();

        public MainWindow()
        {
            InitializeComponent();
            Kernel32.AllocConsole();

            CreateSections();

            List<(string groupName, List<string> visitors)> groupData = GenerateVisitors.AddRandomVisitors(stadium);
            AddVisitorsToGroup(groupData);

            stadium.AssignVisitors();

            DisplayVisitorGroups();
            DisplayVisitorsOnTheSeats();
        }

        private void AddVisitorsToGroup(List<(string groupName, List<string> visitors)> groupData)
        {
            foreach (var (groupName, visitors) in groupData)
            {
                stadium.CreateVisitorGroup(groupName, visitors);
            }
        }

        private void CreateSections()
        {
            int sectionCount = rnd.Next(1, 4);

            for (int i = 0; i < sectionCount; i++)
            {
                int rowCount = rnd.Next(1, 4);
                int columnCount = rnd.Next(3, 11);

                char sectionName = (char)('A' + i);
                stadium.CreateSection(sectionName.ToString(), rowCount, columnCount);

                Console.WriteLine($"Section: {sectionName}, Rows: {rowCount}, Columns: {columnCount}");

                foreach (Row row in stadium.GetSectionsFromStadium()[i].Rows)
                {
                    foreach (Seat seat in row.GetSeats())
                    {
                        Console.Write(seat.SeatCode + " ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private void DisplayVisitorGroups()
        {
            Console.WriteLine("Visitor Groups:\n");

            foreach (VisitorGroup group2 in stadium.GetVisitorGroupsFromStadium())
            {
                Console.WriteLine($"Group Name: {group2.GroupName}");

                foreach (Visitor member in group2.Members)
                {
                    string ageStage = IsAdult(member);

                    Console.WriteLine($"- {member.Name} " + ageStage);
                }

                Console.WriteLine();
            }
        }

        private void DisplayVisitorsOnTheSeats()
        {
            for (int i = 0; i < stadium.GetSectionsFromStadium().Count; i++)
            {
                EventSection section = stadium.GetSectionsFromStadium()[i];
                int maxColumns = stadium.GetSectionsFromStadium().Max(s => s.NumSeats);

                for (int columnIndex = 0; columnIndex < section.NumSeats; columnIndex++)
                {
                    Console.Write($"Section: {section.SectionName}    ");

                    if (columnIndex < section.NumSeats)
                    {
                        if (section.Rows.Count < 3)
                        {
                            int emptyRowCount = 3 - section.Rows.Count;

                            for (int emptyRowIndex = 0; emptyRowIndex < emptyRowCount; emptyRowIndex++)
                            {
                                string emptyRowInfo = new(' ', section.NumSeats);
                                Console.Write(emptyRowInfo.PadRight(15));
                            }
                        }

                        for (int rowIndex = section.Rows.Count - 1; rowIndex >= 0; rowIndex--)
                        {
                            if (columnIndex < section.Rows[rowIndex].GetSeats().Count)
                            {
                                Seat currentSeat = section.Rows[rowIndex].GetSeats()[columnIndex];

                                if (currentSeat.Availability != SeatStatus.Available)
                                {
                                    if (currentSeat.AssignedVisitor != null)
                                    {
                                        Console.ForegroundColor = currentSeat.AssignedVisitor.ColorGroup;
                                        string ageStage = IsAdult(currentSeat.AssignedVisitor);
                                        string visitorInfo = $"{currentSeat.AssignedVisitor.Name} {ageStage}, ";
                                        Console.Write(visitorInfo.PadRight(15));
                                        Console.ResetColor();
                                    }
                                    
                                }
                                else
                                {
                                    string seatInfo = $"{currentSeat.SeatCode}, ";
                                    Console.Write(seatInfo.PadRight(15));
                                }
                            }
                            else
                            {
                                string emptySeatInfo = "          ";
                                Console.Write(emptySeatInfo);
                            }
                        }
                    }
                    else
                    {
                        string emptySectionInfo = "          ";
                        Console.Write(emptySectionInfo);
                    }

                    Console.Write("                  |                                |"); 
                    Console.WriteLine();
                }

                Console.WriteLine();
            }
        }

        private static string IsAdult(Visitor member)
        {
            if (member.Age <= 12)
            {
                return "K";
            }
            else
            {
                return "V";
            }
        }
    }
}
