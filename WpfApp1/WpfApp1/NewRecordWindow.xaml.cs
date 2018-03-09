using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for NewRecordWindow.xaml
    /// </summary>
    public partial class NewRecordWindow : Window
    {
        static int record_size = 10;
        FileManager myFileManager;
        MyRecord c;
        StructureLogic logic;
        char action;

        int loadsCounter, savesCounter;

        string FileName = "organisation.bin";

        public NewRecordWindow(StructureLogic org, ref int sc, ref int lc)
        {
            InitializeComponent();
            myFileManager = new FileManager();
            logic = org;
            action = 'N';
            savesCounter = sc;
            loadsCounter = lc;
        }

        public void editRecord(int key, char action)
        {
            MyRecord[] ciag = new MyRecord[1];
            this.action = action;
            label.Content = "";
            int position = logic.findElementsPosition(key, ref savesCounter, ref loadsCounter);
            ciag = myFileManager.load(FileName, position, 1);
            loadsCounter++;

            if (ciag[0].getKey() != 0)
                Show();

            //logic.loadsCounter++;
            c = ciag[0];
            textBoxKey.Text = c.Key.ToString();
            textBoxFirst.Text = c.First.ToString(); textBoxSecond.Text = c.Second.ToString();
            textBoxThird.Text = c.Third.ToString(); textBoxFourth.Text = c.Fourth.ToString();
            textBoxFifth.Text = c.Fifth.ToString(); textBoxSixth.Text = c.Sixth.ToString();
            textBoxSeventh.Text = c.Seventh.ToString(); textBoxEighth.Text = c.Eighth.ToString();
            textBoxNineth.Text = c.Nineth.ToString(); textBoxTenth.Text = c.Tenth.ToString();
            if (action == 'E')
                buttonAdd.Content = "Zapisz";
            else if (action == 'S')
            {
                textBoxFirst.IsReadOnly = true; textBoxSecond.IsReadOnly = true; textBoxThird.IsReadOnly = true;
                textBoxFourth.IsReadOnly = true; textBoxFifth.IsReadOnly = true; textBoxSixth.IsReadOnly = true;
                textBoxSeventh.IsReadOnly = true; textBoxEighth.IsReadOnly = true; textBoxNineth.IsReadOnly = true;
                textBoxTenth.IsReadOnly = true;
                textBoxFileName.IsReadOnly = true;
                buttonAdd.Visibility = Visibility.Hidden;
                buttonReset.Visibility = Visibility.Hidden;
            }
            textBoxKey.IsReadOnly = true;
        }


        private void addNewRecord()
        {
            int[] values = new int[record_size];
            labelStatus.Content = "";
            try
            {
                int key = Int32.Parse(textBoxKey.Text);

                values[0] = Int32.Parse(textBoxFirst.Text); values[1] = Int32.Parse(textBoxSecond.Text);
                values[2] = Int32.Parse(textBoxThird.Text); values[3] = Int32.Parse(textBoxFourth.Text);
                values[4] = Int32.Parse(textBoxFifth.Text); values[5] = Int32.Parse(textBoxSixth.Text);
                values[6] = Int32.Parse(textBoxSeventh.Text); values[7] = Int32.Parse(textBoxEighth.Text);
                values[8] = Int32.Parse(textBoxNineth.Text); values[9] = Int32.Parse(textBoxTenth.Text);

                if (action == 'N')
                {
                    savesCounter = 0;
                    loadsCounter = 0;
                    if (key > 0)
                    {
                        c = new MyRecord(key, values);
                        logic.addRecord(c, ref savesCounter, ref loadsCounter);
                    }
                }
                else if(action == 'E')
                {
                    c = new MyRecord(key, values, c.getOverflowPointer(), c.isDeleted());
                    logic.Edit(c, FileName);
                }


            }
            catch (Exception ex)
            {
                labelStatus.Content = "Błąd!";
            }
        }

        private void reset()
        {
            labelStatus.Content = "";
            textBoxFirst.Text = "0"; textBoxSecond.Text = "0"; textBoxThird.Text = "0";
            textBoxFourth.Text = "0"; textBoxFifth.Text = "0"; textBoxSixth.Text = "0";
            textBoxSeventh.Text = "0"; textBoxEighth.Text = "0"; textBoxNineth.Text = "0";
            textBoxTenth.Text = "0"; textBoxFileName.Text = "organisation.bin";

            labelStatus.Content = "Zresetowano!";
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            addNewRecord();

        }

        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }
    }
}
