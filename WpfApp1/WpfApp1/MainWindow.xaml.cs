using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StructureLogic logic;

        public MainWindow()
        {
            InitializeComponent();

            logic = new StructureLogic();
        }

        private void resetFileButton_Click(object sender, RoutedEventArgs e)
        {
            File.Delete("organisation.bin");
            File.Delete("organisation2.bin");
            File.Delete("index.bin");

            logic.Reset();

        }

        private void addRecordButton_Click(object sender, RoutedEventArgs e)
        {

            Random r = new Random();
            int loa = 0;
            int sav = 0;

            int i = r.Next(1, 100);
            int[] values = newTable(i);
            MyRecord c = new MyRecord(i, values);
            logic.addRecord(c, ref sav, ref loa);


        }

        private void showFileButton_Click(object sender, RoutedEventArgs e)
        {
            DataWindow dw = new DataWindow("organisation.bin", 'o');
            dw.Show();
            DataWindow index = new DataWindow("index.bin", 'i');
            index.Show();
        }

        private void reorganizeButton_Click(object sender, RoutedEventArgs e)
        {
            logic.Reorganize();
        }

        private void searchRecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int j = 0;
                int value = Int32.Parse(keyTextBox.Text);
                NewRecordWindow newRecord = new NewRecordWindow(logic, ref j, ref j);
            
                newRecord.editRecord(value, 'S');
            }catch(Exception ex) { }
        }

        private void deleteRecordButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                logic.deleteRecord(Int32.Parse(deleteKeyTextBox.Text));
            }catch(Exception ex) { }
        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int j = 0;
                int value = Int32.Parse(editRecordTextBox.Text);
                NewRecordWindow newRecord = new NewRecordWindow(logic, ref j, ref j);
                newRecord.editRecord(value, 'E');
            }catch(Exception ex) { }
        }

        private void addManuallyRecordButton_Click(object sender, RoutedEventArgs e)
        {
            logic.addRecordManually();
        }

        private int[] newTable(int i)
        {
            int[] tab = { i, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            return tab;
        }

        private void zapiszIndexButton_Click(object sender, RoutedEventArgs e)
        {
            logic.saveIndex();
        }
    }
}
