using StudentDBExample.DataModel;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentDBExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StudentRepoDB _repo;

        StudentRepoCsv _repoCSV;

        private int? _selectedId = null;

        public MainWindow()
        {
            InitializeComponent();
          
            _repo = new StudentRepoDB("Data Source=DESKTOP-FFSLR8G\\SQLEXPRESS01;Initial Catalog=StudentDB2025;Integrated Security=True;Trust Server Certificate=True");
            _repoCSV = new StudentRepoCsv();

            studentDg.ItemsSource = _repo.GetStudents();

            //studentDg.ItemsSource = _repoCSV.GetCsvRecords("student.csv");

            studentDg.SelectionChanged += OnSelectionChanged;

        }

        //Track slection on DG
        private void OnSelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (studentDg.SelectedItem is Student selectedStudent)
            {
                _selectedId = selectedStudent.Id;
                StudentName.Text = selectedStudent.Name;
                StudentAge.Text = selectedStudent.Age.ToString();
                StudentYearGroup.Text = selectedStudent.YearGroup;
                StudentSportsTeam.Text = selectedStudent.SportsTeam;
                MessageBox.Show($"The student selected is {selectedStudent.Name}");
            }
        }

        //Refresh DG TODO Fix refresh btn delegator 
        private void Refresh_Grid()
        {
            studentDg.ItemsSource = null;
            studentDg.ItemsSource = _repo.GetStudents();
        }


        //SP EVENTS HERE
        //ADD
        private void Add_Student(object sender, RoutedEventArgs e)
        {
            //lets see if we have some form input
            if (string.IsNullOrWhiteSpace(StudentName.Text))
            {
                MessageBox.Show("Please enter a name"); return;
            }
            //MORE validation here

            //Student Object FILLED from the UI < validation worked out ok
            var studentToAdd = new Student { 
                Name = StudentName.Text,
                Age = int.TryParse(StudentAge.Text, out int age) ? age : 0,
                YearGroup = StudentYearGroup.Text,
                SportsTeam = StudentSportsTeam.Text,
            };

            //SP CALL
            try
            {
                _repo.AddStudent(new List<Student> { studentToAdd });
                //TODO  Refresh
                //TODO: Clear

                MessageBox.Show($"You added the student: {studentToAdd.Name}");
                Refresh_Grid();
            }
            catch (Exception ex) { 
                MessageBox.Show($"Error adding student: {ex.Message}");
            }

        }

        //UPDATE
        private void Update_Student(object sender, RoutedEventArgs e)
        {
            if (_selectedId == null)
            { 
                MessageBox.Show("Select a student first."); 
                return; }
            if (!int.TryParse(StudentAge.Text, out var age))
            { 
                MessageBox.Show("Age must be a valid number."); 
                return; 
            }
            var s = new Student
            {
                Id = _selectedId.Value,
                Name = StudentName.Text,
                Age = age,
                YearGroup = StudentYearGroup.Text,
                SportsTeam = StudentSportsTeam.Text
            };
            try {
                _repo.UpdateStudent(s); Refresh_Grid(); ClearFields(); 
            }
            catch (Exception ex) { 
                MessageBox.Show($"Error: {ex.Message}"); 
            }

        }

        //DELETE
        private void Delete_Student(object sender, RoutedEventArgs e)
        {
            if (_selectedId == null)
            { MessageBox.Show("Select a student first."); return; }
            if (MessageBox.Show("Delete this student?", "Confirm",
                MessageBoxButton.YesNo) != MessageBoxResult.Yes) return;
            try
            {
                _repo.DeleteStudent(_selectedId.Value);
                Refresh_Grid(); 
                ClearFields();
            }
            catch (Exception ex) { MessageBox.Show($"Error: {ex.Message}"); }

        }


        //Clear func
        private void ClearFields()
        {
            _selectedId = null;
            //Multiline Sett empty
            StudentName.Text = StudentAge.Text = StudentYearGroup.Text = StudentSportsTeam.Text = "";
            studentDg.SelectedItem = null;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Grid();
        }
    }
}
//CODE GRAVEYARD
/*
 *             //_repo = new StudentRepoCsv();
            //Set DG source to CSV
            //studentDg.ItemsSource = _repo.GetCsvRecords("student.csv");

            //Set Datagrid to DATABASE over CSV*/