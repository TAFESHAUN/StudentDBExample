using Microsoft.Data.SqlClient;
using StudentDBExample.DataModel;
using System.Data;

namespace StudentDBExample
{
    //HOW we are going to implament IRepo
    public class StudentRepoDB : IRepo
    {
        private readonly string _conn;
        public StudentRepoDB(string connString) => _conn = connString;

        //ONLY for csv so just ignore
        public List<Student> GetCsvRecords(string filePath)
        {
            throw new NotImplementedException();
        }

        //Throw as not implamented becasue this is for DB only
        public List<Student> GetStudents()
        {
            var studentResults = new List<Student>();

            using var conn = new SqlConnection(_conn);
            //using var command = new SqlCommand("SELECT * FROM Student", conn);
            // #TODO: Replace with stored procedure call dbo.usp_Student_SelectAll
            using var command = new SqlCommand("dbo.usp_Student_SelectAll", conn);
            conn.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read()) {
                studentResults.Add(
                    new Student
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Age = reader.GetByte(2),
                        YearGroup = reader.GetString(3),
                        SportsTeam = reader.GetString(4)
                    }
                    );
            }
            return studentResults;
        }

        //CREATE
        public void AddStudent(List<Student> studentsToAdd)
        {
            using var conn = new SqlConnection(_conn);
            conn.Open();
            using var tx = conn.BeginTransaction(); // Start to ensure insert
            foreach (var s in studentsToAdd)
            {
                //using var cmd = new SqlCommand(
                //    "INSERT INTO Students (Name,Age,YearGroup,SportsTeam) " +
                //    "VALUES (@Name,@Age,@YearGroup,@SportsTeam); SELECT SCOPE_IDENTITY();",
                //    conn); 

                using var cmd = new SqlCommand(
                    "dbo.usp_Student_Add",
                    conn, tx);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 100) { Value = s.Name ?? (object)DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@Age", SqlDbType.TinyInt) { Value = s.Age });
                cmd.Parameters.Add(new SqlParameter("@YearGroup", SqlDbType.NVarChar, 20) { Value = s.YearGroup ?? (object)DBNull.Value });
                cmd.Parameters.Add(new SqlParameter("@SportsTeam", SqlDbType.NVarChar, 50) { Value = s.SportsTeam ?? (object)DBNull.Value });

                var newIdStudent = new SqlParameter("@NewID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(newIdStudent);

                cmd.ExecuteNonQuery();

                s.Id = Convert.ToInt32(newIdStudent.Value);
            }
            tx.Commit();

        }

        //UPDATE
        public void UpdateStudent(Student updateThisStudent)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_Student_Update", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = updateThisStudent.Id;
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = updateThisStudent.Name;
            cmd.Parameters.Add("@Age", SqlDbType.TinyInt).Value = updateThisStudent.Age;
            cmd.Parameters.Add("@YearGroup", SqlDbType.NVarChar, 20).Value = updateThisStudent.YearGroup;
            cmd.Parameters.Add("@SportsTeam", SqlDbType.NVarChar, 50).Value = updateThisStudent.SportsTeam;
            conn.Open(); cmd.ExecuteNonQuery();


        }

        //DELETE
        public void DeleteStudent(int studentId)
        {
            using var conn = new SqlConnection(_conn);
            using var cmd = new SqlCommand("dbo.usp_Student_Delete", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = studentId;
            conn.Open(); cmd.ExecuteNonQuery();

        }

    }
}
