using System.Globalization;

namespace DateFormatter
{

    // keeping public so that I can test
    public class Program
    {
        static void Main(string[] args)
        {
            // accept date value from user.
            // have a separate method which accepts date, in which seggregate date, month and year
            // have an array of days in month to check, if by adding 1 to the received date would cause an overflow.
            // if yes, then increment the month as well. else, dont
            // also check if incrementing the month is not greater than 12, if it is greater, then increment the year as well.
            // return the value.

            Console.WriteLine("Date formatter program.");
            Console.WriteLine("Enter date in dd/mm/yyyy format.");

            DateTime dateTime;

            try
            {
                dateTime = DateTime.Parse(Console.ReadLine());

            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Invalid date format received {ex.Message}.");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error occurred {ex.Message}.");
                throw;
            }

            Console.WriteLine("Enter number of days to be added to the given date.");
            string daysToAdd = Console.ReadLine();

            if (!int.TryParse(daysToAdd, out int addDays))
            {
                throw new InvalidCastException($"Invalid number of days entered. {daysToAdd}");
            }

            string updatedDate = AddDays(dateTime, addDays);
            Console.WriteLine($"Updated date = {updatedDate}");
        }

        // keeping public so that I can test    
        public static string AddDays(DateTime date, int addDays)
        {
            int[] daysInMonths = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            // splitting received date into day, month and year

            int day = date.Day;
            int month = date.Month;
            int year = date.Year;

            // identify if the received year is leap year and update days in 1st index position of array to 29
            if(IsLeapYear(year))
            {
                daysInMonths[1] = 29;
            }

            day = day + addDays;

            // checking if the days crossed the maximum days in month
            // 28th in march + 5 days = 33, but maximum days in march are 31. so 33-31 = 2 of april
            if (day > daysInMonths[month - 1])
            {
                day = day - daysInMonths[month - 1];
                month = month++;

                if (month > 12)
                {
                    month = 1;
                    year = year++;
                }

                return new DateTime(year, month, day).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                return new DateTime(year, month, day).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

        }

        // keeping public so that I can test
        public static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
        }
    }
}


