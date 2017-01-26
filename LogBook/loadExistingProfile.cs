using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBook
{
    class loadExistingProfile
    {
        private static string _dirPathName = @"C:\Users\kimbe\Documents\Visual Studio 2015\Projects\Intro App\LogBook\LogBook\Properties\Profiles";
        public static string dirPathName { get { return _dirPathName; } }

        public void doesNameExist(string profileName, ref System.Windows.Controls.Label errorMessage)
        {
            //Directory.Exists(dirPathName + @"\" + profileName) ? assignMessage(ref createNewErrorMessage,"Error") : assignMessage(ref createNewErrorMessage, "Success");

            if (Directory.Exists(dirPathName + @"\" + profileName) == true)
            {
                errorMessage.Content = "Success";
            }
            else if (Directory.Exists(dirPathName + @"\" + profileName) == false)
                errorMessage.Content = "Failure";
        }

        public List<string[]> loadToPages(string profileName)
        {
            string[] topicList = new string[5] { "demo", "vet", "farrier", "vax", "training" };
            List<string[]> seperatedItems = new List<string[]>();
            foreach(string item in topicList)
            {
                var opener = new StreamReader(File.OpenRead(dirPathName + @"\" + profileName + @"\" + profileName +item + ".csv"));

                int count = 0;
                while (!opener.EndOfStream)
                {
                    var line = opener.ReadLine();
                    var lines = line.Split(';');
                    seperatedItems.Add(lines);
                    count++;
                }
            }
            return seperatedItems;
        }
    }
}
