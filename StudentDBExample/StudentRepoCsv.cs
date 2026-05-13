
using CsvHelper;
using CsvHelper.Configuration;
using StudentDBExample.DataModel;
using System.Globalization;
using System.IO;

namespace StudentDBExample
{
    //HOW we are going to implament IRepo
    public class StudentRepoCsv : IRepo
    {
        //IREPO implamentation for CSV READ
        public List<Student> GetCsvRecords(string filePath)
        {
            //IF file exist cont. otherwise throw error
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"CSV file not found: {filePath}");

            //Does it have header? Yes it does
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            { 
                HasHeaderRecord = true 
            };

            //Grab reader file path
            using var reader = new StreamReader(filePath);
            //Convert to CSV over reader
            var csv = new CsvReader(reader, config);
            //Auto map to Student data from CSV file BUT set it to list.
            return csv.GetRecords<Student>().ToList();
        }

        //Throw as not implamented becasue this is for DB only
        public List<Student> GetStudents()
        {
            throw new NotImplementedException();
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
