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
    /// Interaction logic for DataWindow.xaml
    /// </summary>
    public partial class DataWindow : Window
    {
        FileManager myFileManager = new FileManager();
        bool isNotEndOfFile = false;
        const int BUFFER_SIZE = 16;     //w ilości rekordów
        const int minusInfinity = Int32.MinValue;
        MyRecord[] bufor;

        int position_in_file = 0;
        int position_in_buffer = 0;

        public DataWindow(string fileName, char action)
        {
            InitializeComponent();
            initializeColumns(action);

            bufor = new MyRecord[BUFFER_SIZE];

            while (!isNotEndOfFile)
            {
                position_in_buffer = 0;
                if (action == 'o')
                {
                    bufor = myFileManager.load(fileName, position_in_file, BUFFER_SIZE);
                    foreach (MyRecord c in bufor)
                    {
                        if (bufor[position_in_buffer].getTotal() == minusInfinity)
                        {
                            isNotEndOfFile = true;
                            continue;
                        }
                        else if (bufor[position_in_buffer].getTotal() == 0)
                        {
                            c.setLp(position_in_file);
                            dataGrid.Items.Add(c);
                            position_in_buffer++;
                            position_in_file++;
                        }
                        else
                        {
                            c.setLp(position_in_file);
                            dataGrid.Items.Add(c);
                            position_in_file++;
                            position_in_buffer++;
                        }
                    }
                }
                else
                {
                    Index index = myFileManager.loadIndex(BUFFER_SIZE);
                    
                    
                    //dataGrid.Items.Add(index);

                    for(int i = 0; i < index.getPagesAmount(); i++)
                    {
                        Values val = new Values();
                        if (index.getKey(i) != 0)
                        {
                            val.PageNo = i + 1;
                            val.Key = index.getKey(i);
                            dataGrid.Items.Add(val);
                        }
                    }
                    isNotEndOfFile = true;
                }
            }
        }

        private void initializeColumns(char action)
        {
            if (action == 'o') {
                DataGridTextColumn lpColumn = new DataGridTextColumn();
                lpColumn.Header = "Lp";
                lpColumn.Binding = new Binding("Lp");
                dataGrid.Columns.Add(lpColumn);

                DataGridTextColumn idColumn = new DataGridTextColumn();
                idColumn.Header = "Key";
                idColumn.Binding = new Binding("Key");
                dataGrid.Columns.Add(idColumn);

                DataGridTextColumn statusColumn = new DataGridTextColumn();
                statusColumn.Header = "IsDeleted";
                statusColumn.Binding = new Binding("IsDeleted");
                dataGrid.Columns.Add(statusColumn);

                DataGridTextColumn pointerColumn = new DataGridTextColumn();
                pointerColumn.Header = "OverflowPointer";
                pointerColumn.Binding = new Binding("OverflowPointer");
                dataGrid.Columns.Add(pointerColumn);

                DataGridTextColumn totalColumn = new DataGridTextColumn();
                totalColumn.Header = "Suma";
                totalColumn.Binding = new Binding("Total");
                dataGrid.Columns.Add(totalColumn);

                DataGridTextColumn firstColumn = new DataGridTextColumn();
                firstColumn.Header = "First";
                firstColumn.Binding = new Binding("First");
                dataGrid.Columns.Add(firstColumn);

                DataGridTextColumn secondColumn = new DataGridTextColumn();
                secondColumn.Header = "Second";
                secondColumn.Binding = new Binding("Second");
                dataGrid.Columns.Add(secondColumn);

                DataGridTextColumn thirdColumn = new DataGridTextColumn();
                thirdColumn.Header = "Third";
                thirdColumn.Binding = new Binding("Third");
                dataGrid.Columns.Add(thirdColumn);

                DataGridTextColumn fourthColumn = new DataGridTextColumn();
                fourthColumn.Header = "Fourth";
                fourthColumn.Binding = new Binding("Fourth");
                dataGrid.Columns.Add(fourthColumn);

                DataGridTextColumn fifthColumn = new DataGridTextColumn();
                fifthColumn.Header = "Fifth";
                fifthColumn.Binding = new Binding("Fifth");
                dataGrid.Columns.Add(fifthColumn);

                DataGridTextColumn sixthColumn = new DataGridTextColumn();
                sixthColumn.Header = "Sixth";
                sixthColumn.Binding = new Binding("Sixth");
                dataGrid.Columns.Add(sixthColumn);

                DataGridTextColumn seventhColumn = new DataGridTextColumn();
                seventhColumn.Header = "Seventh";
                seventhColumn.Binding = new Binding("Seventh");
                dataGrid.Columns.Add(seventhColumn);

                DataGridTextColumn eighthColumn = new DataGridTextColumn();
                eighthColumn.Header = "Eighth";
                eighthColumn.Binding = new Binding("Eighth");
                dataGrid.Columns.Add(eighthColumn);

                DataGridTextColumn ninethColumn = new DataGridTextColumn();
                ninethColumn.Header = "Nineth";
                ninethColumn.Binding = new Binding("Nineth");
                dataGrid.Columns.Add(ninethColumn);

                DataGridTextColumn tenthColumn = new DataGridTextColumn();
                tenthColumn.Header = "Tenth";
                tenthColumn.Binding = new Binding("Tenth");
                dataGrid.Columns.Add(tenthColumn);
            }
            else
            {
                DataGridTextColumn pageColumn = new DataGridTextColumn();
                pageColumn.Header = "PageNo";
                pageColumn.Binding = new Binding("PageNo");
                dataGrid.Columns.Add(pageColumn);

                DataGridTextColumn keysColumn = new DataGridTextColumn();
                keysColumn.Header = "Key";
                keysColumn.Binding = new Binding("Key");
                dataGrid.Columns.Add(keysColumn);
            }
        }

        private class Values
        {
            public int Key { get; set; }
            public int PageNo { get; set; }
        }
    }
}
