using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using StudentDBExample.DataModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

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
            using var command = new SqlCommand("SELECT * FROM Student", conn);
            // #TODO: Replace with stored procedure call dbo.usp_Student_SelectAll
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
            throw new NotImplementedException();
        }

        //UPDATE
        public void UpdateStudent(Student updateThisStudent)
        {
            throw new NotImplementedException();
        }

        //DELETE
        public void DeleteStudent(int studentId)
        {
            throw new NotImplementedException(); //Set as Delete for CSV but for now just ignore
        }

    }
}
