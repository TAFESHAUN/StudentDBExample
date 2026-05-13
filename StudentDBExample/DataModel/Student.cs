namespace StudentDBExample.DataModel
{
    //CLASS approach to represent a student in the database
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Age { get; set; }
        public string YearGroup { get; set; } = string.Empty;
        public string SportsTeam { get; set; } = "";
    }
}
