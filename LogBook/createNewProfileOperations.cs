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
       
        public static string activeProfile
        {
            get { return MainWindow.activeProfile; }
        }
        public static string dirPathName
        {
            get { return MainWindow.dirPathName; }
        }

        public void createProfile()
        {
            Directory.CreateDirectory(dirPathName + activeProfile);
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
                File.WriteAllText(dirPathName + activeProfile + @"\" + activeProfile + item + ".csv", multiplyer + Environment.NewLine);
            }
        }
    }
}
