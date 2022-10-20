using App1;

namespace TestProject2
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<KeyValuePair<Week, Day>> result = TimeTable.EnumerateTwoWeeks(new DateTime(2022, 10, 20)).ToList();
            List<KeyValuePair<Week, Day>> validResults = new List<KeyValuePair<Week, Day>>()
            {
                new(Week.Bottom, Day.Thursday),
                new(Week.Bottom, Day.Friday),
                new(Week.Bottom, Day.Saturday),

                new(Week.Top, Day.Monday),
                new(Week.Top, Day.Tuesday),
                new(Week.Top, Day.Wednesday),
                new(Week.Top, Day.Thursday),
                new(Week.Top, Day.Friday),
                new(Week.Top, Day.Saturday),

                new(Week.Bottom, Day.Monday),
                new(Week.Bottom, Day.Tuesday),
                new(Week.Bottom, Day.Wednesday),
            };

            CollectionAssert.AreEqual(result, validResults);
        }
    }
}