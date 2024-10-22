using DateFormatter;
using Xunit;

namespace DateFormatterTests
{

    // used a separate project under same solution so that all unit tests are isolated from actual business logic
    public class DateOperationsTests
    {

        // IsLeapYear
        [Theory]
        [InlineData(1900, false)]
        [InlineData(1998, false)]
        [InlineData(1999, false)]
        [InlineData(2000, true)]
        [InlineData(2004, true)]
        [InlineData(2100, false)]
        public void IsLeapYear_ShouldReturnAppropriateBoolean(int year, bool isLeapYear)
        {
            var returnResult = DateOperations.IsLeapYear(year);
            Assert.Equal(isLeapYear, returnResult);
        }

        // happy path
        [Theory]
        [InlineData("28/02/2020", 1, "29/02/2020")]
        [InlineData("31/01/2021", 1, "01/02/2021")]
        [InlineData("31/12/2021", 1, "01/01/2022")]
        [InlineData("28/02/2024", 1, "29/02/2024")] // leapyear case for feb 
        [InlineData("28/02/2021", 1, "01/03/2021")] // non leapyear case for feb 
        [InlineData("15/03/2021", 30, "14/04/2021")] // change in month
        [InlineData("31/10/2021", 90, "29/01/2022")] // change in month and year
        public void AddDays_ShouldReturnExpectedDate(string inputDate, int daysToAdd, string expectedDate)
        {
            DateTime convertedDate = DateTime.Parse(inputDate);
            string result = DateOperations.AddDays(convertedDate, daysToAdd);
            Assert.Equal(expectedDate, result);
        }

        // validation scenarios
        [Theory]
        [InlineData("288/02/2020", "1")] // invalid day
        [InlineData("31/21/2021", "1")] // invalid month
        [InlineData("31/12/202198", "1")] // invalid year
        [InlineData("-12/02/2024", "1")] // negative day
        [InlineData("28//02/2021", "1")]  // double '/'
        [InlineData("abcd", "30")] // string
        public void InitialValidation_InvalidDateFormat_ShouldThrowFormatException(string inputDate, string daysToAdd)
        {
            Assert.Throws<FormatException>(() => DateOperations.InitialValidationAndAddDays(inputDate, daysToAdd));
        }

        // validation scenarios
        [Theory]
        [InlineData(null, "30")] // string
        public void InitialValidation_InvalidDateFormat_ShouldThrowException(string inputDate, string daysToAdd)
        {
            var exception = Assert.Throws<Exception>(() => DateOperations.InitialValidationAndAddDays(inputDate, daysToAdd));
            Assert.Contains("Unexpected error occurred", exception.Message);
        }

        [Theory]
        [InlineData("28/02/2020", "abc")]
        [InlineData("31/01/2021", "2/3")]
        [InlineData("31/12/2021", "0.75")]
        public void InitialValidation_InvalidNumberOfDays_ShouldThrowInvalidCastException(string inputDate, string daysToAdd)
        {
            Assert.Throws<InvalidCastException>(() => DateOperations.InitialValidationAndAddDays(inputDate, daysToAdd));
        }

        [Theory]
        [InlineData("28/02/2020", "-40")]
        public void InitialValidation_NegativeDays_ShouldThrowException(string inputDate, string daysToAdd)
        {
            var exception = Assert.Throws<Exception>(() => DateOperations.InitialValidationAndAddDays(inputDate, daysToAdd));
            Assert.Contains("Not supporting subtraction of days", exception.Message);
        }


        // end to end happy flow
        [Theory]
        [InlineData("28/02/2020", "1", "29/02/2020")]
        [InlineData("31/01/2021", "1", "01/02/2021")]
        [InlineData("31/12/2021", "1", "01/01/2022")]
        [InlineData("28/02/2024", "1", "29/02/2024")] // leapyear case for feb 
        [InlineData("28/02/2021", "1", "01/03/2021")] // non leapyear case for feb 
        [InlineData("15/03/2021", "30", "14/04/2021")] // change in month
        [InlineData("31/10/2021", "90", "29/01/2022")] // change in month and year
        public void InitialValidation_ShouldReturnExpectedDate(string inputDate, string daysToAdd, string expectedDate)
        {
            string result = DateOperations.InitialValidationAndAddDays(inputDate, daysToAdd);
            Assert.Equal(expectedDate, result);
        }

    }
}
