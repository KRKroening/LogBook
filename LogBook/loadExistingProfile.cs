using System;
using System.IO;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBook
{
    class loadExistingProfile
    {
        databaseAccess dbAccess = new databaseAccess();
        public static string activeProfile
        {
            get { return MainWindow.activeProfile; }
        }
        private string dirPathName = MainWindow.dirPathName;

        public static string profileID = "";
        string pVetID = "";
        string sVetID = "";
        string pFarrierID = "";
        string sFarrierID = "";
        string pClinicID = "";

        public List<string> LoadIntoDemos()
        {
            List<string> demoLoadList = new List<string>();
            string queryStatement = "SELECT * FROM Demographics" +
                                " WHERE barnName = '" + activeProfile + "';";
            var query = dbAccess.ExecuteRead(queryStatement);
            while (query.Read())
            { 
                profileID = query[0].ToString();
                while (true)
                {
                    for (int x = 1; true; x++)
                    {
                        try
                        {
                            demoLoadList.Add(query[x].ToString());
                        }
                        catch
                        {
                            break;
                        }
                    }
                    break;
                }

                pVetID = demoLoadList[9];
                sVetID = demoLoadList[10];
                pFarrierID = demoLoadList[11];
                //sFarrierID = demoLoadList[12];
            }
            dbAccess.sql_con.Close();
            return demoLoadList;
        }

        public List<string[]> LoadIntoVet()
        {
            List<string[]> vetLoadList = new List<string[]>();
            for (int x=0; x <= 2; x++)
            {
                string[] microVetList = new string[5];

                string VetID = x.Equals(0) ? pVetID : sVetID;
                string queryStatement = "SELECT * FROM Vets" +
                                        " WHERE vetID = '" + VetID + "';";
                var query = dbAccess.ExecuteRead(queryStatement);
                int count = 0;
                while (query.Read() && count == 0)
                {
                    microVetList[0] = (query[1].ToString());
                    microVetList[1] = (query[2].ToString());
                    microVetList[2] = (query[3].ToString());
                    microVetList[3] = (query[4].ToString());
                    microVetList[4] = (query[5].ToString());
                    count++;
                }
                vetLoadList.Add(microVetList);
            }
            dbAccess.sql_con.Close();
            return vetLoadList;
        }

        public List<string> vetEntryList()
        {
            List<string> vetEntryList = new List<string>();

            string queryStatement = "SELECT * FROM VetVisits" +
                                    " WHERE profileID = '" + profileID + "';";
            var query = dbAccess.ExecuteRead(queryStatement);
            foreach(var item in query)
            {
                vetEntryList.Add(query[1].ToString());
            }

            return vetEntryList;
        }

        public List<string[]> LoadIntoFarrier()
        {
            List<string[]> farrierLoadList = new List<string[]>();
            for (int x = 0; x <= 2; x++)
            {
                string[] microFarrierList = new string[5];

                string farrierID = x.Equals(0) ? pFarrierID : sFarrierID;
                string queryStatement = "SELECT * FROM Farriers" +
                                                " WHERE farrierID = '" + farrierID + "';";
                var query = dbAccess.ExecuteRead(queryStatement);
                int count = 0;
                while (query.Read() && count == 0)
                {
                    microFarrierList[0] = (query[1].ToString());
                    microFarrierList[1] = (query[2].ToString());
                    microFarrierList[2] = (query[3].ToString());
                    count++;
                }
                farrierLoadList.Add(microFarrierList);
            }

            return farrierLoadList;
        }

        public List<string> farrierEntryList()
        {
            List<string> farrierEntryList = new List<string>();

            string queryStatement = "SELECT * FROM FarrierVisits" +
                                    " WHERE profileID = '" + profileID + "';";
            var query = dbAccess.ExecuteRead(queryStatement);
            foreach (var item in query)
            {
                farrierEntryList.Add(query[1].ToString());
            }

            return farrierEntryList;
        }

        public List<string> LoadIntoVax()
        {
            List<string> vaxEntryList = new List<string>();

            string queryStatement = "SELECT * FROM Vax" +
                                    " WHERE profileID = '" + profileID + "';";
            var query = dbAccess.ExecuteRead(queryStatement);
            foreach (var item in query)
            {
                vaxEntryList.Add(query[1].ToString());
            }

            return vaxEntryList;
        }
        public List<string> LoadIntoTraining()
        {
            List<string> trainingEntryList = new List<string>();

            string queryStatement = "SELECT * FROM TrainingVisits" +
                                    " WHERE profileID = '" + profileID + "';";
            var query = dbAccess.ExecuteRead(queryStatement);
            foreach (var item in query)
            {
                trainingEntryList.Add(query[1].ToString());
            }

            return trainingEntryList;
        }

    }
}
