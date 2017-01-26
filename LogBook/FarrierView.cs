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
        static string profileName
        {
            get { return MainWindow.activeProfile; }
        }
        string dirPathName
        {
            get { return loadExistingProfile.dirPathName; }
        }

        public void saveEditFarrierDemos(List<string> dataToSave)
        {
            List<string[]> openedFile = new List<string[]>();
            var openFarrierStream = new StreamReader(File.OpenRead(dirPathName));
            int count = 0;
            while (!openFarrierStream.EndOfStream)
            {
                var line = openFarrierStream.ReadLine();
                var lines = line.Split(';');
                openedFile.Add(lines);
                count++;
            }
        }


    }
}
