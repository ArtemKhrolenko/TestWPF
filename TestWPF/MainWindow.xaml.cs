using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TestContext db;

        private const int minDigit = 1;
        private const int maxDigit = 10;

        public MainWindow()
        {
            InitializeComponent();

            db = new TestContext();

            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            db.Dispose();
        }

        /// <summary>
        /// Fills Table 1 by Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FillTableOneButton_Click(object sender, RoutedEventArgs e)
        {
            //Fill Table 1     
            //Get the sorted list of Data from Table 1 
            WaitLabel.Visibility = Visibility.Visible;
            List<SourceDigit> sourseList = await Task.Run(GetDataForTableOne);

            //delete all records from Table1               
            if (db.SourseDigits.Count() != 0)
            {
                foreach (var entity in db.SourseDigits)
                    db.SourseDigits.Remove(entity);
                await db.SaveChangesAsync();
            }

            //Fill Table 1 with new Data
            db.SourseDigits.AddRange(sourseList);
            await db.SaveChangesAsync();

            //Show message
            MessageBoxResult result = MessageBox.Show("The Data was added to Table_1",
                                          "Confirmation",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
            //Update DataGrid
            tableOneGrid.ItemsSource = db.SourseDigits.Local.ToBindingList();
            
            //Set GUI
            fillTableTwoButton.IsEnabled = true;
            WaitLabel.Visibility = Visibility.Hidden;

        }

        private async Task<List<SourceDigit>> GetDataForTableOne()
        {
            await db.SourseDigits.LoadAsync();
            Random rnd = new Random();
            int[] values = new int[maxDigit + 1 - minDigit];
            int value;
            var sourcetList = new List<SourceDigit>();

            for (int i = 0; i < values.Length; i++)
            {
                while (true)
                {
                    value = rnd.Next(minDigit, maxDigit + 1);
                    if (!values.Any(v => v == value))
                    {
                        values[i] = value;
                        sourcetList.Add(new SourceDigit() { Value = value });
                        break;
                    }
                    else
                        continue;
                }
            }

            return sourcetList;
        }


        /// <summary>
        /// Takes Data from Table 1, sorts it and fill Table 2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void FillTableTwoButton_Click(object sender, RoutedEventArgs e)
        {
            //Get the sorted list of Data from Table 1
            List<DestDigit> destList = await Task.Run(GetDataForTableTwo);

            //Delete items from Table 2 if any exists
            if (db.DestDigits.Count() != 0)
            {
                foreach (var entity in db.DestDigits)
                    db.DestDigits.Remove(entity);
                db.SaveChanges();
            }

            //Fill Table 2 with new Data
            db.DestDigits.AddRange(destList);
            await db.SaveChangesAsync();

            //Show message
            MessageBoxResult result = MessageBox.Show("The Data was added to Table_2",
                                          "Confirmation",
                                          MessageBoxButton.OK,
                                          MessageBoxImage.Information);
            //Update DataGrid
            tableTwoGrid.ItemsSource = db.DestDigits.Local.ToBindingList();
        }


        private async Task<List<DestDigit>> GetDataForTableTwo()
        {
            var destList = new List<DestDigit>();

            //Get data from Table 1
            await db.SourseDigits.LoadAsync();

            //Sort items
            var sortedList = from d in db.SourseDigits
                             orderby d.Value ascending
                             select d;

            foreach (var item in sortedList)
            {
                destList.Add(new DestDigit { Value = item.Value });
            }

            return destList;
        }
        
    }
}
