using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBook
{
    class FarrierView
    {
        static string activeProfile
        {
            get { return MainWindow.activeProfile; }
        }
        string dirPathName
        {
            get { return MainWindow.dirPathName; }
        }

        public void saveEditFarrierDemos(List<string> dataToSave)
        {
            List<string[]> openedFile = new List<string[]>();
            var openFarrierStream = new StreamReader(File.OpenRead(dirPathName + activeProfile + @"\" + activeProfile + "farrier.csv"));
            
            int count = 0;
            while (!openFarrierStream.EndOfStream)
            {
                var line = openFarrierStream.ReadLine();
                var lines = line.Split(';');
                openedFile.Add(lines);
                count++;
            }
            count = 0;
            foreach (string item in dataToSave)
            {
                openedFile[0][count] = item;
                count++;
            }
            string finalToWrite = "";
            foreach (string[] item in openedFile)
            {
                foreach (string subitem in item)
                {
                    finalToWrite += subitem + ";";
                }
                finalToWrite += Environment.NewLine;
            }
            openFarrierStream.Close();
            File.WriteAllText(dirPathName + activeProfile + @"\" + activeProfile + "farrier.csv", finalToWrite);

        }

        public void addNewFarrierDate(string newDateToAdd)
        {
            File.AppendAllText(dirPathName + activeProfile + @"\" + activeProfile + "farrier.csv", newDateToAdd + ";content" + Environment.NewLine);
        }

    }
}
