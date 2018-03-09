using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class MyRecord
    {
        public int Lp { get; set; }

        //klucz + pointer + 10 pól = 12*4 + 1 = 49 bajtów pamięci
        public int Key { get; set; }
        public bool IsDeleted { get; set; }
        public int OverflowPointer { get; set; }    //jesli Int32.MaxValue, to znaczy ze nei istnieje

        public int First { get; set; }
        public int Second { get; set; }
        public int Third { get; set; }
        public int Fourth { get; set; }
        public int Fifth { get; set; }
        public int Sixth { get; set; }
        public int Seventh { get; set; }
        public int Eighth { get; set; }
        public int Nineth { get; set; }
        public int Tenth { get; set; }
        public int Total { get; set; }

        public MyRecord()
        {
            Key = 0;

            IsDeleted = false;
            OverflowPointer = Int32.MaxValue;

            First = 0; Second = 0; Third = 0; Fourth = 0;
            Fifth = 0; Sixth = 0; Seventh = 0; Eighth = 0;
            Nineth = 0; Tenth = 0;

            countTotal();
        }

        public MyRecord(int key, int[] liczby, int pointer, bool status)
        {
            Key = key;
            IsDeleted = status;
            OverflowPointer = pointer;

            First = liczby[0]; Second = liczby[1]; Third = liczby[2]; Fourth = liczby[3];
            Fifth = liczby[4]; Sixth = liczby[5]; Seventh = liczby[6]; Eighth = liczby[7];
            Nineth = liczby[8]; Tenth = liczby[9];

            countTotal();
        }

        public MyRecord(int key, int[] liczby)
        {
            Key = key;
            IsDeleted = false;
            OverflowPointer = Int32.MaxValue;

            First = liczby[0]; Second = liczby[1]; Third = liczby[2]; Fourth = liczby[3];
            Fifth = liczby[4]; Sixth = liczby[5]; Seventh = liczby[6]; Eighth = liczby[7];
            Nineth = liczby[8]; Tenth = liczby[9];

            countTotal();
        }

        public MyRecord(int key, int pointer, bool status, int a, int b, int c, int d, int e,
                    int f, int g, int h, int i, int j)
        {
            Key = key;
            IsDeleted = status;
            OverflowPointer = pointer;

            First = a; Second = b; Third = c; Fourth = d;
            Fifth = e; Sixth = f; Seventh = g; Eighth = h;
            Nineth = i; Tenth = j;

            countTotal();
        }

        private void countTotal()
        {
            Total = First + Second + Third + Fourth + Fifth +
                Sixth + Seventh + Eighth + Nineth + Tenth;
        }

        public int[] getValues()
        {
            int[] tablica = new int[10];
            tablica[0] = First; tablica[1] = Second; tablica[2] = Third;
            tablica[3] = Fourth; tablica[4] = Fifth; tablica[5] = Sixth;
            tablica[6] = Seventh; tablica[7] = Eighth; tablica[8] = Nineth;
            tablica[9] = Tenth;

            return tablica;
        }

        public int getTotal()
        {
            return Total;
        }

        public int getKey()
        {
            return Key;
        }


        public bool isDeleted()
        {
            return IsDeleted;
        }

        //Ustawian na true. Nie mozna tego odwrocic
        public void setDeleted()
        {
            IsDeleted = true;
        }

        public int getOverflowPointer()
        {
            return OverflowPointer;
        }

        public void setOverflowPointer(int newPointer)
        {
            OverflowPointer = newPointer;
        }

        public void setLp(int i)
        {
            Lp = i;
        }
        
        public void print()
        {
            Console.WriteLine("Key: " + Key + " Status: " + IsDeleted + " Pointer: " + OverflowPointer);
        }
    }
}
