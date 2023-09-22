using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisitorsPlacement_Bal.Enums;

namespace VisitorsPlacement_Bal.Classes
{
    public class Row
    {
        private readonly List<Seat> seats;
        public IReadOnlyCollection<Seat> Seats => seats;

        public Row(string sectionName, int numSeats, int numRows)
        {
            seats = new List<Seat>(numSeats);
            PopulateSeats(numSeats, numRows, sectionName);
        }

        private void PopulateSeats(int numSeats, int numRows, string sectionName)
        {
            for (int i = 0; i < numSeats; i++)
            {
                string seatCode = $"{sectionName}-{numRows + 1}-{i + 1}";
                seats.Add(new Seat(seatCode, numRows + 1));
            }
        }

        public int CalculateAvailableSeats()
        {
            return Seats.Count(seat => seat.Availability == SeatStatus.Available);
        }

        public List<Seat>? GetAvailableSeats()
        {
            List<Seat> availableSeats = Seats
                .Where(seat => seat.Availability == SeatStatus.Available)
                .ToList();

            return availableSeats.ToList();
        }

        public int GetSeatCount()
        {
            return Seats.Count;
        }

        public List<Seat> GetSeats()
        {
            return seats;
        }
    }
}
