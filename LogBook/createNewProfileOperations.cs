using System;
using System.Windows;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBook
{
    class createNewProfileOperations
    {
        private string _dirPathName = @"C:\Users\kimbe\Documents\Visual Studio 2015\Projects\Intro App\LogBook\LogBook\Properties\Profiles";
        string dirPathName { get { return _dirPathName; } }

        public void doesNameExist(string profileName, ref System.Windows.Controls.Label errorMessage)
        {

            errorMessage.Content = Directory.Exists(dirPathName + @"\" + profileName) ? "Error" : "Success";
        }

        public void createProfile(string profileNameText)
        {
            Directory.CreateDirectory(dirPathName + @"\" + profileNameText);
            string[] topicList = new string[5] { "demo", "vet", "farrier", "vax", "training" };

            foreach (string item in topicList)
            {
                int multi = 0;
                switch (item)
                {
                    case "demo":
                        multi = 9;
                        break;
                    case "vet":
                        multi = 5;
                        break;
                    case "farrier":
                        multi = 4;
                        break;
                    case "vax":
                        multi = 4;
                        break;
                    case "training":
                        multi = 4;
                        break;
                    default:
                        break;
                }
                string multiplyer = "_";
                if (item == "demo")
                    multiplyer = "Img" + string.Concat(Enumerable.Repeat(";_", multi));
                else
                    multiplyer += string.Concat(Enumerable.Repeat(";_", multi));
                File.WriteAllText(dirPathName + @"\" + profileNameText + @"\" + profileNameText + item + ".csv", multiplyer + Environment.NewLine);
            }
        }
    }
}
