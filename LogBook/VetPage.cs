using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBook
{
    class VetPage
    {
        static string activeProfile
        {
            get { return MainWindow.activeProfile;}
        }
        string dirPathName
        {
            get { return MainWindow.dirPathName; }
        }

        public void editSaveVetDemo(System.Windows.Controls.Button buttonState, List<System.Windows.Controls.TextBox> vetDemosEdits, List<System.Windows.Controls.Label> vetDemosDisplayEdits)
        {
            if (buttonState.Content.ToString() == "Edit")
            {
                buttonState.Content = "Save";
                foreach (System.Windows.Controls.TextBox item in vetDemosEdits)
                    item.Visibility = Visibility.Visible;
                int count = 0;
                foreach (System.Windows.Controls.Label itemL in vetDemosDisplayEdits)
                {
                    vetDemosEdits[count].Text = itemL.Content.ToString();
                    count++;
                }
            }
            else if (buttonState.Content.ToString() == "Save")
            {
                buttonState.Content = "Edit";
                foreach (System.Windows.Controls.TextBox item in vetDemosEdits)
                    item.Visibility = Visibility.Hidden;
                int count = 0;
                foreach (System.Windows.Controls.TextBox item in vetDemosEdits)
                {
                    vetDemosDisplayEdits[count].Content = item.Text;
                    count++;
                }                
            }
        }

        public void saveVetDemosToFile(List<System.Windows.Controls.TextBox> vetDemosEdits)
        {
            string pathName = ( dirPathName + activeProfile + @"\" + activeProfile + "vet.csv").ToString();
            List<string[]> openedFile = new List<string[]>();
            var openVetStream = new StreamReader(File.OpenRead(pathName));
            int count = 0;
            while (!openVetStream.EndOfStream)
            {
                var line = openVetStream.ReadLine();
                var lines = line.Split(';');
                openedFile.Add(lines);
                count++;
            }
            openVetStream.Close();
            count = 0;
            foreach(System.Windows.Controls.TextBox item in vetDemosEdits)
            {
                openedFile[0][count] = vetDemosEdits[count].Text;
                count++;
            }
            string concatinatedPrintVetDemo = "";
            foreach(string[] line in openedFile)
            {
                concatinatedPrintVetDemo += line + ";";
            }
            concatinatedPrintVetDemo = concatinatedPrintVetDemo.Remove(concatinatedPrintVetDemo.Length - 1);
            File.WriteAllText(pathName,concatinatedPrintVetDemo + Environment.NewLine);
  
        }

        public void newVetRecordDateEntry(string date,string title)
        {
            string entry = date + ";" + title + ";" + "New Entry" + " ~v";
            File.AppendAllText(dirPathName + activeProfile + @"\" + activeProfile + "vet.csv", entry + Environment.NewLine);
        }

        public List<string[]> loadVetEntryDate(string date)
        {
            List<string[]> foundList = new List<string[]>();
            var browser = File.ReadAllLines(dirPathName + activeProfile + @"\" + activeProfile + "vet.csv");
            foreach (string item in browser)
            {
                if (item.Contains(date))
                {
                    var lines = item.Split(';');
                    if (lines[0] == date)
                        foundList.Add(lines);
                }
            }
            return foundList;
        }

        public string saveVetEntry(string editEntry, string date)
        {
            List<string[]> list = new List<string[]>();
            var browser = File.ReadAllLines(dirPathName + activeProfile + @"\" + activeProfile + "vet.csv");
            foreach (string item in browser)
            {
                var lines = item.Split(';');
                list.Add(lines);
            }
            string toPrint = "";
            foreach (string[] item in list)
            {
                if (item.Contains(date)){
                    item[2] = editEntry + " ~v";
                }
                foreach (string subitem in item)
                {
                    toPrint += subitem + ";";
                }
                toPrint = toPrint.Remove(toPrint.Length - 1);
                toPrint += Environment.NewLine;
            }
            try
            {
                File.WriteAllText(dirPathName + activeProfile + @"\" + activeProfile + "vet.csv", toPrint);
                return "Save Successful";
            }
            catch
            {
                return "Save Failed";
            }
        }
    }
}
