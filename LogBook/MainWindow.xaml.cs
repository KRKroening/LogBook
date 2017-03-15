using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System;
using System.IO;
using System.Timers;
using System.Windows.Controls;

/*TODO:
/ CHANGE FROM .CSV TO SQLITE
/ -DB connects, tables created.
/ - Finish fully converting
/ - Create Model class for selects
/ 
    */
namespace LogBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        databaseAccess dbAccess = new databaseAccess();
        createNewProfileOperations createNewProfileOps = new createNewProfileOperations();
        loadExistingProfile loadExistingProfile = new loadExistingProfile();
        MiscHandler MiscHandler = new MiscHandler();
        VetPage vetPage = new VetPage();
        FarrierView farrierPage = new FarrierView();

        private string _loadOrCreate = "";
        public string loadOrCreate
        {
            get { return _loadOrCreate; }
            set { _loadOrCreate = value; }
        }
        private static string _activeProfile;
        public static string activeProfile
        {
            get { return _activeProfile; }
            set { _activeProfile = value; }
        }

        private static string _dirPathName = @"Profiles\";

        public static string dirPathName {
            get { return _dirPathName; }
        }


        public MainWindow()
        {
            InitializeComponent();
            dbAccess.doesDbExists();
        }

        //Demographics
        #region#

        private void createNew_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow.IsOpen = true;
            loadOrCreate = "Create";
            profileNameWelcome.Content = "Enter new profile name:";

            ProfileTextBox.Focus();
            ProfileTextBox.SelectionStart = 0;
            ProfileTextBox.SelectionLength = ProfileTextBox.Text.Length;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow.IsOpen = false;
            imagePopUp.IsOpen = false;
            errorMessage.Content = "";
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            string profileNameText = ProfileTextBox.Text;

            if (loadOrCreate == "Create")
            {
                MiscHandler.doesNameExist(ref errorMessage, profileNameText);

                if (errorMessage.Content.ToString() == "Does Not Exist")
                {
                    createNewProfileOps.createProfile(profileNameText);
                    headProfileLabel.Content = activeProfile = profileNameText;

                    string insertBarnName = "SELECT barnName from Demographics WHERE barnName = '" + activeProfile + "';";
                    var insertDefaults = dbAccess.ExecuteRead(insertBarnName);
                    while (insertDefaults.Read())
                    {
                        barnNameEntry.Content = insertDefaults["barnName"].ToString();
                    }
                    PopUpWindow.IsOpen = false;
                    errorMessage.Content = "";
                }
                if (errorMessage.Content.ToString() == "Already Exists")
                    activeProfile = null;
                dbAccess.sql_con.Close();
            }
            else if (loadOrCreate == "Load")
            {
                MiscHandler.doesNameExist(ref errorMessage, profileNameText);

                if (errorMessage.Content.ToString() == "Already Exists")
                {
                    headProfileLabel.Content = activeProfile = profileNameText;
                    PopUpWindow.IsOpen = false;
                    loadIntoPages();
                }
                if (errorMessage.Content.ToString() == "Does Not Exist")
                    activeProfile = null;
            }
        }

        private void loadExisting_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow.IsOpen = true;
            loadOrCreate = "Load";
            profileNameWelcome.Content = "Enter existing profile name:";

            ProfileTextBox.Focus();
            ProfileTextBox.SelectionStart = 0;
            ProfileTextBox.SelectionLength = ProfileTextBox.Text.Length;
        }

        private void loadIntoPages()
        {
            var loadDemos = loadExistingProfile.LoadIntoDemos();
            var loadVet = loadExistingProfile.LoadIntoVet();
            var loadVetVisits = loadExistingProfile.vetEntryList();
            var loadFarrier = loadExistingProfile.LoadIntoFarrier();
            var loadFarrierVisits = loadExistingProfile.farrierEntryList(); 
            var loadVax = loadExistingProfile.LoadIntoVax();
            var loadTraining = loadExistingProfile.LoadIntoTraining();

            barnNameEntry.Content = loadDemos[0];
            regNameEntry.Content = loadDemos[1];
            regNumEntry.Content = loadDemos[2];
            sexEntry.Content = loadDemos[3];
            foalingDateEntry.Content = loadDemos[4];
            heightEntry.Content = loadDemos[5];
            colourEntry.Content = loadDemos[6];
            markingsEntry.Content = loadDemos[7];
            brandEntry.Content = loadDemos[8];
            //miscLabelOneEntry.Content = loadDemos[0][10];
            //miscLabelTwoEntry.Content = loadDemos[0][11];
            //miscLabelThreeEntry.Content = loadDemos[0][12];

            pClinicEntry.Content = loadVet[0][3];
            pClinicPhoneEntry.Content = loadVet[0][4];
            pVetNameEntry.Content = loadVet[0][1];
            pVetNumEntry.Content = loadVet[0][2];
            sVetNameEntry.Content = loadVet[1][1];
            sVetNumEntry.Content = loadVet[1][2];

            foreach(string item in loadVetVisits)
            {
                selectDateVet.Items.Add(item);
            }

            pFarrierNameEntry.Content = loadFarrier[0][0];
            pFarrierNumEntry.Content = loadFarrier[0][1];
            pFarrierNameEntry.Content = loadFarrier[1][0];
            pFarrierNumEntry.Content = loadFarrier[1][1];

            foreach (string item in loadFarrierVisits)
            {
                farrierDateContainer.Items.Add(item);
            }

            //foreach (string item in loadVax)
            //{
            //    vaxDateContainer.Items.Add(item);
            //}

            //foreach (string item in loadTraining)
            //{
            //    trainingDateContainer.Items.Add(item);
            //}
        }

        private void loadDemoPic_Click(object sender, RoutedEventArgs e)
        {
            imagePopUp.IsOpen = true;
        }

        private void imageUploadBrowse_Click(object sender, RoutedEventArgs e)
        {
            MiscHandler.uploadNewPhotoDemo(activeProfile, ref uploadImagePathName, ref demoPicture);
        }

        private void uploadImageUpload_Click(object sender, RoutedEventArgs e)
        {
            MiscHandler.uploadImageUpload(ref uploadImagePathName, ref demoPicture);
            imagePopUp.IsOpen = false;
        }

        private void editDemo_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> editBoxControl = new List<TextBox>
            {
                barnNameEdit, regNameEdit,regNumEdit,sexEdit,foalingDateEdit,heightEdit, colourEdit,markingsEdit,brandEdit
            };
            List<Label> toUpdateLabels = new List<Label>
            {
                barnNameEntry,regNameEntry,regNumEntry, sexEntry,foalingDateEntry,heightEntry,colourEntry,markingsEntry,brandEntry
            };
            if (editDemo.Content.ToString() == "Edit")
            {
                editDemo.Content = "Save";
                MiscHandler.changeVisibilityBox(editBoxControl);
                MiscHandler.ensureActiveEditField(editBoxControl, toUpdateLabels);
            }
            else if (editDemo.Content.ToString() == "Save")
            {
                editDemo.Content = "Edit";
                MiscHandler.changeVisibilityBox(editBoxControl);
                MiscHandler.saveFromBoxToLabel(editBoxControl, toUpdateLabels);
                List<TextBox> editBoxControlUpdated = new List<TextBox>
            {
                barnNameEdit, regNameEdit,regNumEdit,sexEdit,foalingDateEdit,heightEdit, colourEdit,markingsEdit,brandEdit
            };
                MiscHandler.saveDataToFile(editBoxControlUpdated, "Demo");

            }
            
        }

        private void entryBox_HighlightText(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            MiscHandler.boxHighlighter(sender);
        }
         private void entryBox_HighlightText(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MiscHandler.boxHighlighter(sender);
        }

        #endregion
        //Vet Page
        #region
        private void editSaveVetDemo_Click(object sender, RoutedEventArgs e)
        {
            List<TextBox> vetDemos = new List<TextBox>
            {
                pClinicEdit,pClinicPhEdit,pVetNameEdit,pVetPhEdit,sVetNameEdit,sVetPhEdit
            };
            List<Label> vetDemosDisplay = new List<Label>
            {
                pClinicEntry,pClinicPhoneEntry,pVetNameEntry,pVetNumEntry,sVetNameEntry,sVetNumEntry
            };
            Button buttonState = (sender as Button);
            vetPage.editSaveVetDemo(buttonState,vetDemos, vetDemosDisplay);
            MiscHandler.saveDataToFile(vetDemos,"vet");
        }

        private void createNewVetEntryButton_Click(object sender, RoutedEventArgs e)
        {
            if (createNewVetEntryButton.Content.ToString() == "New Entry")
            {
                vetEnterDateNew.Visibility = Visibility.Visible;
                vetEnterDateNewBox.Visibility = Visibility.Visible;
                vetEnterTitleNew.Visibility = Visibility.Visible;
                vetEnterTitleNewBox.Visibility = Visibility.Visible;
                createNewVetEntryButton.Content = "Save Entry";
            }
            else if (createNewVetEntryButton.Content.ToString() == "Save Entry")
            {
                vetEnterDateNew.Visibility = Visibility.Hidden;
                vetEnterDateNewBox.Visibility = Visibility.Hidden;
                vetEnterTitleNew.Visibility = Visibility.Hidden;
                vetEnterTitleNewBox.Visibility = Visibility.Hidden;
                vetPage.newVetRecordDateEntry(vetEnterDateNewBox.Text, vetEnterTitleNewBox.Text);
                dateLabel.Content = vetEnterDateNewBox.Text;
                descriptionLabel.Content = vetEnterTitleNewBox.Text;
                selectDateVet.Items.Add(vetEnterDateNewBox.Text);
                createNewVetEntryButton.Content = "New Entry";
            }

        }

        private void selectDateVet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var box = (sender as System.Windows.Controls.ComboBox);
            List<string[]> foundList = vetPage.loadVetEntryDate(box.SelectedValue.ToString());
            dateLabel.Content = foundList[0][0].ToString();
            descriptionLabel.Content = foundList[0][1].ToString();
            foundList[0][2] = foundList[0][2].Remove(foundList[0][2].Length-3);
            entryBoxVet.Text = foundList[0][2];
        }

        private void saveEntryBoxVet_Click(object sender, RoutedEventArgs e)
        {
            vetSaveNotification.Content = vetPage.saveVetEntry(entryBoxVet.Text,dateLabel.Content.ToString());
            vetSaveNotification.Foreground = vetSaveNotification.Equals("Save Failed") ? Brushes.Red:Brushes.Green;
        }

        #endregion

        //Farrier
        #region
        private void editSaveFarrierDemos_Click(object sender, RoutedEventArgs e)
        {
            var buttonState = (sender as Button);
            if (buttonState.Content.ToString() == "Edit")
            {
                pFarrierNameEdit.Text = pFarrierNameEntry.Content.ToString();
                pFarrierNumEdit.Text = pFarrierNumEntry.Content.ToString();
                sFarrierNameEdit.Text = sFarrierNameEntry.Content.ToString();
                pFarrierNumEdit.Text = sFarrierNumEntry.Content.ToString();

                buttonState.Content = "Save";
                pFarrierNameEdit.Visibility = Visibility.Visible;
                pFarrierNumEdit.Visibility = Visibility.Visible;
                sFarrierNameEdit.Visibility = Visibility.Visible;
                sFarrierNumEdit.Visibility = Visibility.Visible;
            }
            else if (buttonState.Content.ToString() == "Save")
            {
                List<string> dataToSaveFarrierDemos = new List<string>();
                buttonState.Content = "Edit";
                pFarrierNameEdit.Visibility = Visibility.Hidden;
                dataToSaveFarrierDemos.Add(pFarrierNameEdit.Text);
                pFarrierNameEntry.Content = pFarrierNameEdit.Text;

                pFarrierNumEdit.Visibility = Visibility.Hidden;
                dataToSaveFarrierDemos.Add(pFarrierNumEdit.Text);
                pFarrierNumEntry.Content = pFarrierNumEdit.Text;

                sFarrierNameEdit.Visibility = Visibility.Hidden;
                dataToSaveFarrierDemos.Add(sFarrierNameEdit.Text);
                sFarrierNameEntry.Content = sFarrierNameEdit.Text;

                sFarrierNumEdit.Visibility = Visibility.Hidden;
                dataToSaveFarrierDemos.Add(sFarrierNumEdit.Text);
                sFarrierNumEntry.Content = sFarrierNumEdit.Text;

                farrierPage.saveEditFarrierDemos(dataToSaveFarrierDemos);
            }
        }

        private void addNewDateEntry_Click(object sender, RoutedEventArgs e)
        {
            Button addEditButton = (sender as Button);
            if (addEditButton.Content.ToString() == "Add New")
            {
                addEditButton.Content = "Save";
                addNewFarrierDateEntry.Visibility = Visibility.Visible;

            }
            else if (addEditButton.Content.ToString() == "Save")
            {
                addEditButton.Content = "Add New";
                addNewFarrierDateEntry.Visibility = Visibility.Hidden;
                farrierPage.addNewFarrierDate(addNewFarrierDateEntry.Text);
                farrierDateContainer.Items.Add(addNewFarrierDateEntry.Text);
            }
        }

        


        #endregion

    }
}
