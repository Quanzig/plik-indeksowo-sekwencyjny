using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class FileManager
    {
        public const int rozmiar_ciagu = 12;
        const int minusInf = Int32.MinValue;
        BinaryWriter bw;
        BinaryReader br;


        public void saveRecord(string filename, int position, MyRecord c)
        {
            bw = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write));
            int startingPosition = position * (sizeof(int) * rozmiar_ciagu + sizeof(bool));
            bw.BaseStream.Position = startingPosition;

            int[] values = c.getValues();

            bw.Write(c.getKey());

            bool status = c.isDeleted();
            int pointer = c.getOverflowPointer();
            bw.Write(status);
            bw.Write(pointer);
            int counter = 0;
            for (int i = startingPosition + (2* sizeof(int) + 1);
                        i < startingPosition + (sizeof(int) * rozmiar_ciagu + 1);
                        i += sizeof(int))
            {
                bw.Write(values[counter]);
                counter++;
            }
            bw.Close();
        }

        public void savePage(string fileName, MyRecord[] ciag, int position)
        {
            bw = new BinaryWriter(File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write));

            int startingPosition = position * (sizeof(int) * rozmiar_ciagu + sizeof(bool));
            bw.BaseStream.Position = startingPosition;
            foreach (MyRecord c in ciag)
            {
                try
                {
                    int[] values = new int[rozmiar_ciagu];
                    values = c.getValues();
                    int key = c.getKey();
                    bool status = c.isDeleted();
                    int pointer = c.getOverflowPointer();
                    bw.Write(key);
                    bw.Write(status);
                    bw.Write(pointer);
                    bw.Write(values[0]); bw.Write(values[1]); bw.Write(values[2]);
                    bw.Write(values[3]); bw.Write(values[4]); bw.Write(values[5]);
                    bw.Write(values[6]); bw.Write(values[7]); bw.Write(values[8]);
                    bw.Write(values[9]);
                }
                catch (System.NullReferenceException e)
                {
                    break;
                }
                catch (Exception ex) { }
            }
            bw.Close();
        }

        //action: G - stwórz nowy plik, S - dodaj do już istniejącego, jeśli nie istnieje, to stwórz
        public void save(string fileName, MyRecord[] ciag, char action)
        {
            if (action == 'G')
                bw = new BinaryWriter(File.Open(fileName, FileMode.Create, FileAccess.Write));
            else if (action == 'S')
                bw = new BinaryWriter(File.Open(fileName, FileMode.Append, FileAccess.Write));

            foreach (MyRecord c in ciag)
            {
                if (c != null)
                {
                    try
                    {
                        int[] values = new int[rozmiar_ciagu];
                        values = c.getValues();
                        int key = c.getKey();
                        bw.Write(key);
                        bool status = c.isDeleted();
                        int pointer = c.getOverflowPointer();
                        bw.Write(status);
                        bw.Write(pointer);
                        bw.Write(values[0]); bw.Write(values[1]); bw.Write(values[2]);
                        bw.Write(values[3]); bw.Write(values[4]); bw.Write(values[5]);
                        bw.Write(values[6]); bw.Write(values[7]); bw.Write(values[8]);
                        bw.Write(values[9]);
                    }
                    catch (System.NullReferenceException e)
                    {
                        break;
                    }
                    catch (Exception ex) { }
                }
                else break;
            }
            bw.Close();
        }

        //position - pozycja, licząc od zerowego elementu, zwieksza sie co 1
        //pozycję liczę W REKORDACH
        public MyRecord[] load(string fileName, int position, int buffer_size)
        {
            MyRecord[] buffer = new MyRecord[buffer_size];
            int[] values = new int[rozmiar_ciagu];
            int counter = 0;
            int record_counter = 0;


            if (File.Exists(fileName))
            {
                try
                {
                    br = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read));
                    int startingPosition = position * (sizeof(int) * rozmiar_ciagu + sizeof(bool));
                    br.BaseStream.Position = startingPosition;

                    for (int j = 0; j < buffer_size; j++)
                    {
                        int key = br.ReadInt32();
                        bool status = br.ReadBoolean();
                        int pointer = br.ReadInt32();
                        counter = 0;
                        for (int i = startingPosition + (2 * sizeof(int) + 1);
                            i < startingPosition + (sizeof(int) * rozmiar_ciagu + 1);
                            i += sizeof(int))
                        {
                            values[counter] = br.ReadInt32();
                            counter++;
                        }
                        buffer[record_counter] = new MyRecord(key, values, pointer, status);
                        record_counter++;

                        //zwiększenie pozycji startowej wczytywania rekordu
                        startingPosition += sizeof(int) * rozmiar_ciagu;
                    }
                    br.Close();

                    return buffer;
                }
                catch (IOException ex)
                {
                    //jeśli nastąpi end of file, nadaj wartość minusInfinity sumie ciągu 
                    buffer[record_counter] = new MyRecord(0, Int32.MaxValue, true, minusInf, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    br.Close();
                    return buffer;
                }

                catch (Exception ex)
                {
                    buffer[record_counter] = new MyRecord(0, Int32.MaxValue, true, minusInf, 0, 0, 0, 0, 0, 0, 0, 0, 0);
                    br.Close();
                    return buffer;
                }
            }
            buffer = new MyRecord[1];
            buffer[record_counter] = new MyRecord(0, Int32.MaxValue, true, minusInf, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            return buffer;
        }

        public Index loadIndex(int buffer_size)
        {
            Index index = new Index();
            int value = 0;
            int page = 1;
            int position = 0;
            if (File.Exists("index.bin"))
            {
                while (page != -1)
                {
                    br = new BinaryReader(File.Open("index.bin", FileMode.Open, FileAccess.Read));
                    br.BaseStream.Position = position;
                    for (int i = 0; i < buffer_size; i++)
                    {
                        try
                        {
                            page = br.ReadInt32() - 1;
                            value = br.ReadInt32();
                        }
                        catch (EndOfStreamException ex) { page = -1; }
                        if (page == -1) break;
                        index.increasePagesAmount();
                        index.addIndex(value, page);

                    }
                    position += buffer_size;
                    br.Close();
                }
            }


            return index;
        }

        public void saveIndex(Index index)
        {
            int PageSize = 5;
            int counter = 0;
            int pagesAmount = index.getPagesAmount();
            while (counter != pagesAmount)
            {
                bw = new BinaryWriter(File.Open("index.bin", FileMode.Create, FileAccess.Write));
                for (int i=counter; i < counter + PageSize; i++)
                {
                    if (i == pagesAmount)
                        break;
                    bw.Write(i + 1);
                    bw.Write(index.getKey(i));
                    counter++;
                }
                bw.Close();
            }
        }
    }
}
