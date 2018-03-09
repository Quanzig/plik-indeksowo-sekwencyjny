using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class MyPage
    {
        private int PageSize = 5;       //ile rekordow miesci sie w stronie
        private int UsedSize;           //ile rekordow siedzi juz w stronie


        public MyPage()
        {
            UsedSize = 0;
        }

        public MyPage(int size)
        {
            PageSize = size;
            UsedSize = 0;
        }


        
        public void incrementUsedSize()
        {
            UsedSize++;
        }

        public void decrementUsedSize()
        {
            UsedSize--;
        }


        public int getUsedSize()
        {
            return UsedSize;
        }
        public int getPageSize()
        {
            return PageSize;
        }

        public bool isFull()
        {
            if (UsedSize == PageSize)
                return true;
            else return false;
        }
    }
}
