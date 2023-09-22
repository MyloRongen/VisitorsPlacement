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

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Stadium stadium = new();
        private readonly Random rnd = new();

        public MainWindow()
        {
            InitializeComponent();
            Kernel32.AllocConsole();

            CreateSections();

            GenerateVisitors.AddRandomVistors(stadium);

            stadium.AssignVisitors();

            DisplayVisitorGroups();

            DisplayVisitorsOnTheSeats();
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

                foreach (Row row in stadium.sections[i].Rows)
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

        private void CreateDummyVisitors()
        {
            // Group 1
            stadium.RegisterVisitor("John", new(2015, 5, 30), ConsoleColor.Red);
            stadium.RegisterVisitor("Emma", new(2015, 5, 30), ConsoleColor.Red);
            stadium.RegisterVisitor("Jordy", new(2002, 5, 30), ConsoleColor.Red);
            stadium.RegisterVisitor("Jens", new(2002, 5, 30), ConsoleColor.Red);
            stadium.RegisterVisitor("Bas", new(2002, 5, 30), ConsoleColor.Red);

            // Group 2
            stadium.RegisterVisitor("Gijs", new(2015, 5, 30), ConsoleColor.Green);
            stadium.RegisterVisitor("Sam", new(2015, 5, 30), ConsoleColor.Green);
            stadium.RegisterVisitor("Koen", new(2002, 5, 30), ConsoleColor.Green);

            // Group 3
            stadium.RegisterVisitor("Abd", new(2015, 5, 30), ConsoleColor.Blue);
            stadium.RegisterVisitor("Jelle", new(2015, 5, 30), ConsoleColor.Blue);
            stadium.RegisterVisitor("Thomas", new(2002, 5, 30), ConsoleColor.Blue);

            //Group 4 
            stadium.RegisterVisitor("Viggo", new(2002, 5, 30), ConsoleColor.Yellow);
            stadium.RegisterVisitor("Quin", new(2002, 5, 30), ConsoleColor.Yellow);
        }

        private void CreateVisitorGroups()
        {
            stadium.CreateVisitorGroup("Group 1", new List<string> { "John", "Emma", "Jordy", "Jens", "Bas" });
            stadium.CreateVisitorGroup("Group 2", new List<string> { "Gijs", "Sam", "Koen" });
            stadium.CreateVisitorGroup("Group 3", new List<string> { "Abd", "Jelle", "Thomas" });
            stadium.CreateVisitorGroup("Group 4", new List<string> { "Viggo", "Quin" });
        }

        private void DisplayVisitorGroups()
        {
            Console.WriteLine("Visitor Groups:\n");

            foreach (VisitorGroup group2 in stadium.visitorGroups)
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
            for (int i = 0; i < stadium.sections.Count; i++)
            {
                EventSection section = stadium.sections[i];
                int maxColumns = stadium.sections.Max(s => s.NumSeats);

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
