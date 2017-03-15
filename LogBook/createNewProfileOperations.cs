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
        databaseAccess dbAccess = new databaseAccess();
        public static string activeProfile
        {
            get { return MainWindow.activeProfile; }
        }
        public static string dirPathName
        {
            get { return MainWindow.dirPathName; }
        }

        public void createProfile(string profileNameText)
        {
            string addProfile = "INSERT INTO Demographics (barnName) VALUES('" + profileNameText + "');";

            try
            {
                dbAccess.ExecuteQuery(addProfile);
                Console.WriteLine("profile created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Insert Failed: " + ex.Message);
            }
            dbAccess.sql_con.Close();
        }
    }
}
