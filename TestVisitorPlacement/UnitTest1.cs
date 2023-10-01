using VisitorsPlacement_Bal.Classes;

namespace TestVisitorPlacement
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void Scenario1()
        {
            // Arrange
            Stadium stadium = new();

            stadium.CreateSection("A", 1, 6);
            stadium.CreateSection("B", 2, 6);
            stadium.CreateSection("C", 3, 6);

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

            // Group 4 
            stadium.RegisterVisitor("Viggo", new(2002, 5, 30), ConsoleColor.Yellow);
            stadium.RegisterVisitor("Quin", new(2002, 5, 30), ConsoleColor.Yellow);

            stadium.CreateVisitorGroup("Group 1", new List<string> { "John", "Emma", "Jordy", "Jens", "Bas" });
            stadium.CreateVisitorGroup("Group 2", new List<string> { "Gijs", "Sam", "Koen" });
            stadium.CreateVisitorGroup("Group 3", new List<string> { "Abd", "Jelle", "Thomas" });
            stadium.CreateVisitorGroup("Group 4", new List<string> { "Viggo", "Quin" });

            // Act
            int sectionCount = stadium.GetSectionCount();

            // Assert
            Assert.That(sectionCount, Is.EqualTo(3));
        }
    }
}