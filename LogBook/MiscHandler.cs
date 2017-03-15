using System;
using Microsoft.Win32;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;


namespace LogBook
{
    class MiscHandler
    {
        databaseAccess dbAccess = new databaseAccess();
        private string _uploadedImagePath = "";
        public string uploadedImagePath
        {
            get { return _uploadedImagePath; }
            set { _uploadedImagePath = value; }
        }
        public static string activeProfile
        {
            get { return MainWindow.activeProfile; }
        }
        private string dirPathName = MainWindow.dirPathName ;


        public void doesNameExist(ref System.Windows.Controls.Label errorMessage, string profileNameText)
        {

            string checkText = "SELECT * FROM Demographics WHERE barnName = '" + profileNameText + "';" ;
            if (dbAccess.ExecuteRead(checkText).StepCount >= 1)
            {
                errorMessage.Content = "Already Exists";
            }
            else
                errorMessage.Content = "Does Not Exist";
            dbAccess.sql_con.Close();
        }


        public void uploadNewPhotoDemo(string profileName, ref System.Windows.Controls.Label uploadImageFilePath, ref System.Windows.Controls.Image demoPicture )
        {
            OpenFileDialog explorer = new OpenFileDialog();

            explorer.DefaultExt = ".png";
            explorer.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";
            Nullable<bool> result = explorer.ShowDialog();
            if(result == true)
            {
                string file = explorer.FileName;
                uploadImageFilePath.Content = file;
                uploadedImagePath = file;
            }
        }

        public void uploadImageUpload(ref System.Windows.Controls.Label uploadImageFilePath, ref System.Windows.Controls.Image demoPicture)
        {
            demoPicture.Source = new ImageSourceConverter().ConvertFromString(uploadedImagePath) as ImageSource;

            string fileName = Path.GetFileName(uploadedImagePath);
            string newFileLocation = dirPathName + @"\" + fileName;
            try
            {
                File.Copy(uploadedImagePath, newFileLocation);
            }
            catch { }

            string statment = "UPDATE Pictures" +
                                " set" +
                                " default = 0" +
                                " WHERE profileID = " + loadExistingProfile.profileID + ";";
            var execute = dbAccess.ExecuteCheckQuery(statment);

            statment = "Select * from Pictures where fileLoc = " + uploadedImagePath + ";";
            if (dbAccess.ExecuteCheckQuery(statment) >= 1)
            {//Already exists, set to default
                try
                {
                    statment = "UPDATE Pictures" +
                                " set" +
                                " default = 1" +
                                " WHERE fileLoc = " + uploadedImagePath + ";";
                    dbAccess.ExecuteQuery(statment);
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            else
            {
                try
                {
                    statment = "INSERT INTO Pictures (profileID,fileLoc,defaultPicture)" +
                                " VALUES(" + loadExistingProfile.profileID + "," + uploadedImagePath + "," + fileName + ");";
                    dbAccess.ExecuteQuery(statment);
                }
                catch(Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
            dbAccess.sql_con.Close();
        }

        public void changeVisibilityBox(List<System.Windows.Controls.TextBox> editBoxControl)
        {
            if (editBoxControl[0].Visibility == System.Windows.Visibility.Hidden)
            {
                foreach (System.Windows.Controls.TextBox box in editBoxControl)
                    box.Visibility = System.Windows.Visibility.Visible;
            }
            else if (editBoxControl[0].Visibility == System.Windows.Visibility.Visible)
            {
                foreach (System.Windows.Controls.TextBox box in editBoxControl)
                    box.Visibility = System.Windows.Visibility.Hidden;
            }
           
        }

        public void ensureActiveEditField(List<System.Windows.Controls.TextBox> editBoxControl, List<System.Windows.Controls.Label> toEditDemoLabels)
        {
            int itemCount = 0;
            foreach (System.Windows.Controls.TextBox box in editBoxControl)
            {
                box.Text = toEditDemoLabels[itemCount].Content.ToString();
                itemCount++;
            }
        }

        public void saveFromBoxToLabel(List<System.Windows.Controls.TextBox> editBoxControl, List<System.Windows.Controls.Label> toEditDemoLabels)
        {
            int itemCount = 0;
            foreach (System.Windows.Controls.Label box in toEditDemoLabels)
            {
                if(editBoxControl[itemCount] == null)
                {
                    box.Content = null;
                    itemCount++;
                } else
                {
                    box.Content = editBoxControl[itemCount].Text.ToString();
                    itemCount++;
                }
                
            }
        }

        public void saveDataToFile(List<System.Windows.Controls.TextBox> editBoxControl, string page)
        {
            switch (page)
            {
                case "Demo":
                    string updateDemos = "UPDATE Demographics SET " +
                                        "barnName = '" + editBoxControl[0].Text +
                                        "', RegName = '" + editBoxControl[1].Text +
                                        "', regNumber = " + editBoxControl[2].Text +
                                        ", sex = '" + editBoxControl[3].Text +
                                        "', foalingDate = '" + editBoxControl[4].Text +
                                        "', height = " + editBoxControl[5].Text +
                                        ", colour = '" + editBoxControl[6].Text +
                                        "', markings = '" + editBoxControl[7].Text +
                                        "', brand = '" + editBoxControl[8].Text +
                                        "';";
                    try
                    {
                        var query = dbAccess.ExecuteCheckQuery(updateDemos);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Update Error: " + ex.Message);
                    }
                    break;
                case "vet":
                    string updateVet = "INSERT OR IGNORE INTO Vets(name, primaryPhone,secondaryPhone, clinicName, clinicPhone)" +
                                        "VALUES('Karen', 34)" +
                                        " UPDATE my_table SET" +
                                        " name = " + editBoxControl[2] +
                                        ", primaryPhone = " + editBoxControl[3] +
                                        ", secondaryPhone = " + editBoxControl[4] +
                                        ", clinicName = " + editBoxControl[0] +
                                        ", clinicPhone = " + editBoxControl[1] +
                                        ";";

                    try
                    {
                        var query = dbAccess.ExecuteCheckQuery(updateVet);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Update Error: " + ex.Message);
                    }
                    break;
                case "Farrier":
                    break;
                case "Vax":
                    break;
                case "Training":
                    break;
                default:
                    break;

            }
        }
            public void boxHighlighter(object sender)
        {
            System.Windows.Controls.TextBox send = (sender as System.Windows.Controls.TextBox);
            send.Focus();
            send.SelectionStart = 0;
            send.SelectionLength = send.Text.Length;
        }
            


        }
    
}
