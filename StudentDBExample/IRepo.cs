using StudentDBExample.DataModel;

namespace StudentDBExample
{
    public interface IRepo
    {
        //READ CSV
        List<Student> GetCsvRecords(string filePath);

        //READ locl SQL Server DB
        List<Student> GetStudents();

        //CREATE
        void AddStudent(List<Student> studentsToAdd);

        //UPDATE
        void UpdateStudent(Student updateThisStudent);

        //DELETE
        void DeleteStudent(int studentId);
    }
}
