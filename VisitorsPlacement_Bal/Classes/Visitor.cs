using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisitorsPlacement_Bal.Classes
{
    public class Visitor
    {
        public string Name { get; }
        public DateTime BirthDate { get; }
        public int Age => CalculateAge();

        public ConsoleColor ColorGroup;

        public Visitor(string name, DateTime birthDate, ConsoleColor colorGroup)
        {
            Name = name;
            BirthDate = birthDate;
            ColorGroup = colorGroup;
        }

        private int CalculateAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - BirthDate.Year;
            if (today < BirthDate.AddYears(age))
            {
                age--;
            }
            return age;
        }
    }
}
