using Faker;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorsPlacement_Bal.Classes
{
    public class GenerateVisitors
    {
        public static void AddRandomVistors(Stadium stadium)
        {
            Random random = new();
            List<string> groupColors = new() { "Red", "Green", "Blue", "Yellow", "DarkGreen", "Magenta" };

            for (int i = 1; i <= 6; i++)
            {
                List<string> visitors = new();
                for (int j = 0; j < 5; j++)
                {
                    string visitorName = Name.First();
                    visitors.Add(visitorName);

                    int year = random.Next(2002, 2016);
                    int month = random.Next(1, 13);
                    int day = random.Next(1, 29);
                    DateTime dateOfBirth = new(year, month, day);

                    ConsoleColor color = (ConsoleColor)System.Enum.Parse(typeof(ConsoleColor), groupColors[i - 1]);
                    stadium.RegisterVisitor(visitorName, dateOfBirth, color);
                }

                string groupName = "Group " + i;
                stadium.CreateVisitorGroup(groupName, visitors);
            }
        }
    }
}
