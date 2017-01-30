using System;
using Microsoft.Win32;
using System.Windows.Media;
using System.IO;
using System.Collections.Generic;


namespace LogBook
{
    class MiscHandler
    {
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


        public void doesNameExist(ref System.Windows.Controls.Label errorMessage)
        {

            errorMessage.Content = Directory.Exists(dirPathName + @"\" + activeProfile) ? "Error" : "Success";

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

            List<string[]> updateDefaultPicture = new List<string[]>();
            var readToFile = new StreamReader(File.OpenRead(dirPathName + @"\" + activeProfile + "demo.csv"));
            int count = 0;
            while (!readToFile.EndOfStream)
            {
                var line = readToFile.ReadLine();
                var lines = line.Split(';');
                updateDefaultPicture.Add(lines);
                count++;
            }
            readToFile.Close();
            updateDefaultPicture[0][0] = newFileLocation;
            string toSaveCsv = "";
            foreach (string csv in updateDefaultPicture[0])
            {
                toSaveCsv += csv + ";";
            }
            File.WriteAllText(dirPathName + @"\" + activeProfile + "demo.csv", toSaveCsv);


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
                box.Content = editBoxControl[itemCount].Text.ToString();
                itemCount++;
            }
        }

        public void saveDataToFile(List<System.Windows.Controls.TextBox> editBoxControl, string page)
        {
            List<string[]> toSave = new List<string[]>();
            switch (page)
            {
                case "Demo":
                    var readToFile = new StreamReader(File.OpenRead(dirPathName + activeProfile + @"\" + activeProfile + "demo.csv"));
                    int count = 0;
                    while (!readToFile.EndOfStream)
                    {
                        var line = readToFile.ReadLine();
                        var lines = line.Split(';');
                        toSave.Add(lines);
                        count++;
                    }
                    readToFile.Close();

                    int counter = 1;
                    foreach (System.Windows.Controls.TextBox boxes in editBoxControl)
                    {
                        toSave[0][counter] = boxes.Text.ToString();
                        counter++;
                    }
                    string toSaveCsv = "";
                    foreach (string csv in toSave[0])
                    {
                        toSaveCsv += csv + ";";
                    }
                    toSaveCsv = toSaveCsv.TrimEnd().Substring(0, toSaveCsv.Length - 1);
                    File.WriteAllText(dirPathName + @"\" + activeProfile + "demo.csv", toSaveCsv);

                    break;
                case "vet":
                    var readToFileV = new StreamReader(File.OpenRead(dirPathName + @"\" + activeProfile + "vet.csv"));
                    int countV = 0;
                    while (!readToFileV.EndOfStream)
                    {
                        var line = readToFileV.ReadLine();
                        var lines = line.Split(';');
                        toSave.Add(lines);
                        countV++;
                    }
                    readToFileV.Close();

                    int counterV = 0;
                    foreach (System.Windows.Controls.TextBox boxes in editBoxControl)
                    {
                        toSave[0][counterV] = boxes.Text.ToString();
                        counterV++;
                    }
                    string toSaveCsvV = "";
                    foreach (string[] csv in toSave)
                    {
                        foreach (string subcsv in csv)
                        {
                            toSaveCsvV += subcsv + ";";
                        }
                        toSaveCsvV = toSaveCsvV.Remove(toSaveCsvV.Length - 1);
                        toSaveCsvV += Environment.NewLine;
                    }
                    File.WriteAllText(dirPathName + @"\" + activeProfile + "vet.csv", toSaveCsvV);
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
