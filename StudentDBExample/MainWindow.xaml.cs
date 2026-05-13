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

        public MainWindow()
        {
            InitializeComponent();
          
            _repo = new StudentRepoDB("Data Source=DESKTOP-FFSLR8G\\SQLEXPRESS01;Initial Catalog=StudentDB2025;Integrated Security=True;Trust Server Certificate=True");

            studentDg.ItemsSource = _repo.GetStudents();

        }
    }
}
//CODE GRAVEYARD
/*
 *             //_repo = new StudentRepoCsv();
            //Set DG source to CSV
            //studentDg.ItemsSource = _repo.GetCsvRecords("student.csv");

            //Set Datagrid to DATABASE over CSV*/