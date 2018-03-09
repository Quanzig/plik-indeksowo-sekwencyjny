using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Index
    {
        public int[] Keys;
        public int[] PageNo;
        private int PagesAmount;

        public Index()
        {
            PagesAmount = 1;

            CreateArrays();
        }

        //TECHNICZNE
        /*********************************************/

        //tworzy tablice na podstawie PagesAmount
        private void CreateArrays()
        {
            Keys = new int[PagesAmount];
            PageNo = new int[PagesAmount];
            for (int i = 0; i < PagesAmount; i++)
            {
                Keys[i] = 0;
                PageNo[i] = i + 1;
            }
        }
        public void increasePagesAmount()
        {
            int[] tempPage = PageNo;
            int[] tempKeys = Keys;
            PagesAmount++;
            CreateArrays();
            for (int i = 0; i < PagesAmount - 1; i++)
            {
                Keys[i] = tempKeys[i];
                PageNo[i] = tempPage[i];
            }
        }
        /*********************************************/

        
        //zwraca numer strony, w ktorej powinien byc zapisany rekord (strony liczone sa od 1)
        public int whatPage(int key)
        {
            for (int i = 1; i < PagesAmount; i++)
            {
                if (key < Keys[i]) return i - 1;
            }
            return PagesAmount - 1;
        }


        //GETTERY, SETTERY
        /*********************************************/
        public void addIndex(int index, int page)
        {
            Keys[page] = index;
        }

        public int getPagesAmount()
        {
            return PagesAmount;
        }

        public int getKey(int pageNumber)
        {
            return Keys[pageNumber];
        }

    }
}