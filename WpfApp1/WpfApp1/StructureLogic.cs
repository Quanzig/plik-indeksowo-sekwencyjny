using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class StructureLogic
    {
        string FileName = "organisation.bin";
        string FileName2 = "organisation2.bin";

        private const int REORGANIZE_VALUE = 10;

        Index index;
        MyPage[] MainArea;
        FileManager myFileManager;

        int OverflowPointer, OverflowSize;

        int PageSize;
        int PagesInMainAreaCount;

        private const double alfa = 0.6;

        public StructureLogic()
        {
            PagesInMainAreaCount = 1;
            index = new Index();
            MainArea = new MyPage[PagesInMainAreaCount];
            for (int i = 0; i < index.getPagesAmount(); i++)
            {
                MainArea[i] = new MyPage();
            }
            myFileManager = new FileManager();
            PageSize = MainArea[0].getPageSize();

            OverflowPointer = PageSize * PagesInMainAreaCount;
            OverflowSize = 0;
        }

        public void addRecord(MyRecord c, ref int sav, ref int load)
        {
            int whatPage = index.whatPage(c.getKey());

            if (MainArea[0].getUsedSize() == 0)
                addRecord(c, whatPage, ref sav, ref load);
            else
            {
                if (findElementsPosition(c.getKey(), ref sav, ref load) == Int32.MaxValue)
                    addRecord(c, whatPage, ref sav, ref load);
                else
                    Console.WriteLine("Podany element: " + c.getKey() + " juz istnieje!");
            }
            Results addResult = new Results("addRecord", load, sav);
            addResult.Show();
        }

        private void addRecord(MyRecord c, int whatPage, ref int saves, ref int loads)
        {
            int positionInFile = 0;
            int PageSize = MainArea[0].getPageSize();   //wszystkie strony maja taka sama wartosc
            positionInFile = (whatPage) * PageSize;

            //pierwszy rekord na nowej stronie
            if(index.getKey(whatPage) == 0)
            {
                MyRecord[] page = new MyRecord[PageSize];
                for(int i = 0; i < PageSize; i++)
                {
                    page[i] = new MyRecord();
                }
                page[0] = c;
                myFileManager.savePage(FileName, page, positionInFile);
                saves++;

                index.addIndex(c.getKey(), whatPage);
                MainArea[whatPage].incrementUsedSize();
                saveIndex();
            }
            else
            {
                int PageUsedSize = MainArea[whatPage].getUsedSize();

                MyRecord[] page = myFileManager.load(FileName, positionInFile, PageSize);
                loads++;

                //niepelna strona
                if (!MainArea[whatPage].isFull())
                {
                    int positionInPage = PageUsedSize;
                    for(int i = 0; i < PageUsedSize; i++)
                    {
                        if(c.getKey() < page[i].getKey())
                        {
                            positionInPage = i;
                            break;
                        }
                    }

                    //zapis strony
                    MyRecord[] newPage = new MyRecord[PageSize];
                    for(int i = 0; i < positionInPage; i++)
                    {
                        newPage[i] = page[i];
                    }
                    newPage[positionInPage] = c;
                    for(int i = positionInPage + 1; i < PageSize; i++)
                    {
                        newPage[i] = page[i-1];
                    }

                    myFileManager.savePage(FileName, newPage, positionInFile);
                    saves++;
                    MainArea[whatPage].incrementUsedSize();

                    if (c.getKey() < index.getKey(whatPage))
                    {
                        index.addIndex(c.getKey(), whatPage);
                        saveIndex();
                    }
                }

                //pelna strona
                else
                {
                    //jesli rekord jest mniejszy niz najmniejszy na stronie
                    if(c.getKey() < page[0].getKey())
                    {
                        //na indexie
                        if (c.getKey() < index.getKey(whatPage))
                        {
                            index.addIndex(c.getKey(), whatPage);
                            saveIndex();
                        }

                        //na stronie
                        MyRecord temp = page[0];
                        page[0] = c;
                        c = temp;
                        page[0].setOverflowPointer(OverflowPointer + OverflowSize);

                        myFileManager.saveRecord(FileName, positionInFile, page[0]);
                        saves++;
                        //na overflow
                        myFileManager.saveRecord(FileName, OverflowPointer + OverflowSize, c);
                        saves++;
                        OverflowSize++;
                    }
                    else
                    {
                        //znajdywanie odpowiedniego miejsca
                        int positionInPage = PageUsedSize - 1;
                        for(int i=1; i < PageUsedSize; i++)
                        {
                            if (c.getKey() < page[i].getKey())
                            {
                                positionInPage = i - 1;
                                break;
                            }
                        }

                        //jesli odpowiedni rekord nie ma overflow
                        if (page[positionInPage].getOverflowPointer() == Int32.MaxValue)
                        {
                            page[positionInPage].setOverflowPointer(OverflowPointer + OverflowSize);
                            myFileManager.saveRecord(FileName, positionInFile + positionInPage, page[positionInPage]);
                            myFileManager.saveRecord(FileName, OverflowPointer + OverflowSize, c);
                            OverflowSize++;
                            saves += 2;
                        }
                        //jesli ma
                        else
                        {
                            int positionOfPreviousElement = page[positionInPage].getOverflowPointer();

                            MyRecord[] previousElement = myFileManager.load(FileName, positionOfPreviousElement, 1);
                            loads++;

                            if (c.getKey() < previousElement[0].getKey())
                            {
                                c.setOverflowPointer(page[positionInPage].getOverflowPointer());
                                page[positionInPage].setOverflowPointer(OverflowPointer + OverflowSize);
                                myFileManager.saveRecord(FileName, positionInFile + positionInPage, page[positionInPage]);
                                myFileManager.saveRecord(FileName, OverflowPointer + OverflowSize, c);
                                OverflowSize++;
                                saves += 2;
                            }
                            else
                            {
                                bool status = true;
                                if (previousElement[0].getOverflowPointer() != Int32.MaxValue)
                                {
                                    MyRecord[] nextElement = myFileManager.load(FileName, previousElement[0].getOverflowPointer(), 1);
                                    loads++;

                                    //sprwdzaj do ostatniego istniejacego w lancuchu elementu
                                    while (nextElement[0].getTotal() != Int32.MinValue)
                                    {
                                        if (c.getKey() < nextElement[0].getKey())
                                        {
                                            c.setOverflowPointer(previousElement[0].getOverflowPointer());
                                            previousElement[0].setOverflowPointer(OverflowPointer + OverflowSize);

                                            myFileManager.saveRecord(FileName, positionOfPreviousElement, previousElement[0]);
                                            myFileManager.saveRecord(FileName, OverflowPointer + OverflowSize, c);
                                            OverflowSize++;
                                            saves += 2;

                                            status = false;
                                            break;
                                        }
                                        positionOfPreviousElement = previousElement[0].getOverflowPointer();
                                        previousElement = nextElement;
                                        nextElement = myFileManager.load(FileName, previousElement[0].getOverflowPointer(), 1);
                                        loads++;
                                    }
                                    
                                }
                                //ostatni element w lancuchu
                                if (status)
                                {
                                    previousElement[0].setOverflowPointer(OverflowPointer + OverflowSize);

                                    myFileManager.saveRecord(FileName, positionOfPreviousElement, previousElement[0]);
                                    myFileManager.saveRecord(FileName, OverflowPointer + OverflowSize, c);
                                    OverflowSize++;
                                    saves += 2;
                                }
                            }
                        }
                    }
                    if (OverflowSize > REORGANIZE_VALUE) Reorganize();

                }
            }
        }

        public void saveIndex()
        {
            myFileManager.saveIndex(index);
        }

        public void Reorganize()
        {
            int reorgSaves = 0;
            int reorgLoads = 0;
            int positionInOrganisedFile = 0;
            int position = 0;

            for (int i = 0; i < PagesInMainAreaCount; i++) {
                MyRecord[] page = myFileManager.load(FileName, positionInOrganisedFile, PageSize);
                reorgLoads++;

                MyRecord[] newPage = new MyRecord[PageSize];
                int recordsInNewPageIndex = 0;

                for(int j = 0; j < PageSize; j++)
                {
                    if (page[j].getKey() == 0)
                        break;

                    position = page[j].getOverflowPointer();

                    if (!page[j].isDeleted())
                    {
                        newPage[recordsInNewPageIndex] = page[j];
                        newPage[recordsInNewPageIndex].setOverflowPointer(Int32.MaxValue);
                        recordsInNewPageIndex++;

                        checkIfReadyToSave(ref newPage, PageSize, ref recordsInNewPageIndex, ref reorgSaves);
                    }
                    
                    if (position != Int32.MaxValue)
                    {
                        do
                        {
                            MyRecord[] overflowRecord = myFileManager.load(FileName, position, 1);
                            reorgLoads++;

                            position = overflowRecord[0].getOverflowPointer();

                            if (!overflowRecord[0].isDeleted())
                            {
                                newPage[recordsInNewPageIndex] = overflowRecord[0];

                                newPage[recordsInNewPageIndex].setOverflowPointer(Int32.MaxValue);
                                recordsInNewPageIndex++;

                                checkIfReadyToSave(ref newPage, PageSize, ref recordsInNewPageIndex, ref reorgSaves);
                            }
                        } while (position != Int32.MaxValue);
                    }
                }
                if (recordsInNewPageIndex != 0)
                {
                    myFileManager.save(FileName2, newPage, 'S');
                    recordsInNewPageIndex = 0;
                    reorgSaves++;
                }
                positionInOrganisedFile += PageSize;
            }
            File.Delete(FileName);
            AddDataFromFile(ref reorgSaves, ref reorgLoads);
            File.Delete(FileName2);

            OverflowPointer = PagesInMainAreaCount * PageSize;
            OverflowSize = 0;

            Results reorganizeResult = new Results("reorganizeRecord", reorgLoads, reorgSaves);
            reorganizeResult.Show();
        }

        private void checkIfReadyToSave(ref MyRecord[] c, int pageSize, ref int recordsCounter, ref int saves)
        {
            if (c[pageSize - 1] != null)
            {
                myFileManager.save(FileName2, c, 'S');
                saves++;
                c = new MyRecord[PageSize];
                recordsCounter = 0;
            }
        }

        private void AddDataFromFile(ref int saves, ref int loads)
        {
            index = new Index();
            MainArea = new MyPage[1];
            MainArea[0] = new MyPage();

            int positionInFile = 0;
            int positionInDataBuffer = 0;
            PagesInMainAreaCount = 1;
            MyRecord[] buffer;

            do
            {
                buffer = myFileManager.load(FileName2, positionInFile, PageSize);
                loads++;
                positionInDataBuffer = 0;
                foreach (MyRecord c in buffer)
                {
                    if (c.getKey() == 0)
                    {
                        positionInDataBuffer++;
                        break;
                    }

                    int whatPage = index.whatPage(c.getKey());
                    if (MainArea[whatPage].getUsedSize() >= PageSize * alfa)
                    {
                        index.increasePagesAmount();
                        increaseMainAreaPages();
                        whatPage++;
                    }
                    addRecord(c, whatPage, ref saves, ref loads);
                    positionInDataBuffer++;
                }
                positionInFile += PageSize;
            } while (buffer[positionInDataBuffer - 1].getKey() != 0);
        }

        public void addRecordManually()
        {
            int savesCounter = 0;
            int loadsCounter = 0;
            MyRecord c = new MyRecord();
            NewRecordWindow nrw = new NewRecordWindow(this, ref savesCounter, ref loadsCounter);
            nrw.Show();
        }

        public void Edit(MyRecord c, string filename)
        {
            int savesCounter = 0;
            int loadsCounter = 0;
            int position = findElementsPosition(c.getKey(), ref savesCounter, ref loadsCounter);
            myFileManager.saveRecord(filename, position, c);
            savesCounter++;

            Results editResult = new Results("editRecord", loadsCounter, savesCounter);
            editResult.Show();
        }

        public void deleteRecord(int Key)
        {
            int savesCounter = 0;
            int loadsCounter = 0;
            int position = findElementsPosition(Key, ref savesCounter, ref loadsCounter);
            if (position != Int32.MaxValue)
            {
                MyRecord[] c = myFileManager.load(FileName, position, 1);
                loadsCounter++;
                c[0].setDeleted();
                myFileManager.saveRecord(FileName, position, c[0]);
                savesCounter++;
            }
            Results deleteResult = new Results("deleteRecord", loadsCounter, savesCounter);
            deleteResult.Show();
        }

        private void increaseMainAreaPages()
        {
            PagesInMainAreaCount++;
            MyPage[] temp = MainArea;
            MainArea = new MyPage[PagesInMainAreaCount];
            for(int i = 0; i < PagesInMainAreaCount - 1; i++)
            {
                MainArea[i] = temp[i];
            }
            MainArea[PagesInMainAreaCount - 1] = new MyPage();
        }

        public int findElementsPosition(int Key, ref int sc, ref int lc)
        {
            int whatPage = index.whatPage(Key);
            int position = whatPage * PageSize;
            int positionInPage = MainArea[whatPage].getUsedSize() - 1;

            MyRecord[] page = myFileManager.load(FileName, position, PageSize);
            lc++;

            //sprawdzanie w podstawowej liscie
            for (int i = 0; i < positionInPage; i++)
            {
                if (Key == page[i].getKey())
                {
                    return position + i;
                }
            }

            //jesli nie ma, to gdzie ew. powinno byc
            for (int i = 1; i < positionInPage; i++)
            {
                if (Key < page[i].getKey())
                {
                    positionInPage = i - 1;
                    break;
                }
            }

            if (MainArea[0].getUsedSize() == 0)
                return Int32.MaxValue;
            if (page[positionInPage].getKey() == Key)
                return position + positionInPage;
            else
            {
                if (MainArea[whatPage].getUsedSize() == PageSize && page[positionInPage].getOverflowPointer()!=Int32.MaxValue)
                {
                    MyRecord[] c = myFileManager.load(FileName, page[positionInPage].getOverflowPointer(), 1);
                    lc++;
                    positionInPage = page[positionInPage].getOverflowPointer();

                    while (c[0].getTotal() != Int32.MinValue)
                    {
                        if (c[0].getKey() == Key)
                        {
                            return position + positionInPage;
                        }
                        positionInPage = c[0].getOverflowPointer();
                        c = myFileManager.load(FileName, position + positionInPage, 1);
                        lc++;
                    }
                }
                //else
                return Int32.MaxValue;
            }
        }


        public void Reset()
        {
            //reset
            index = new Index();
            PagesInMainAreaCount = 1;
            MainArea = new MyPage[1];
            MainArea[0] = new MyPage();

            OverflowPointer = PageSize * PagesInMainAreaCount;
            OverflowSize = 0;
        }
    }
}
