using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.IO;

/*TODO:
/ Save indicator
/ 
/ 
/ 
    */
namespace LogBook
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        private static string _activeProfile = "";
        public static string activeProfile
        {
            get { return _activeProfile; }
            set { _activeProfile = value; }
        }
         
        public MainWindow()
        {
            InitializeComponent();
        }

        //Demographics
        #region#

        private void createNew_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow.IsOpen = true;
            loadOrCreate = "Create";
            
            ProfileTextBox.Focus();
            ProfileTextBox.SelectionStart = 0;
            ProfileTextBox.SelectionLength = ProfileTextBox.Text.Length;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow.IsOpen = false;
            imagePopUp.IsOpen = false;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            string profileNameText = ProfileTextBox.Text;

            if (loadOrCreate == "Create")
            {
                createNewProfileOps.doesNameExist(profileNameText, ref errorMessage);

                if (errorMessage.Content.ToString() == "Success")
                {
                    createNewProfileOps.createProfile(profileNameText);
                    headProfileLabel.Content = activeProfile = profileNameText;
                    PopUpWindow.IsOpen = false;
                }
            }
            else if (loadOrCreate == "Load")
            {
                loadExistingProfile.doesNameExist(profileNameText, ref errorMessage);

                if (errorMessage.Content.ToString() == "Success")
                {
                    var pagesList = loadExistingProfile.loadToPages(profileNameText);
                    headProfileLabel.Content = activeProfile = profileNameText;
                    PopUpWindow.IsOpen = false;
                    loadIntoPages(pagesList);
                }
            }
        }

        private void loadExisting_Click(object sender, RoutedEventArgs e)
        {
            PopUpWindow.IsOpen = true;
            loadOrCreate = "Load";
            ProfileTextBox.Focus();
            ProfileTextBox.SelectionStart = 0;
            ProfileTextBox.SelectionLength = ProfileTextBox.Text.Length;
        }



        private void loadIntoPages(List<string[]> loadIntoPages)
        {
            if (loadIntoPages[0][0].Contains("."))
                demoPicture.Source = new ImageSourceConverter().ConvertFromString(loadIntoPages[0][0]) as ImageSource;
            barnNameEntry.Content = loadIntoPages[0][1];
            regNameEntry.Content = loadIntoPages[0][2];
            regNumEntry.Content = loadIntoPages[0][3];
            sexEntry.Content = loadIntoPages[0][4];
            foalingDateEntry.Content = loadIntoPages[0][5];
            heightEntry.Content = loadIntoPages[0][6];
            colourEntry.Content = loadIntoPages[0][7];
            markingsEntry.Content = loadIntoPages[0][8];
            brandEntry.Content = loadIntoPages[0][9];
            //miscLabelOneEntry.Content = loadIntoPages[0][10];
            //miscLabelTwoEntry.Content = loadIntoPages[0][11];
            //miscLabelThreeEntry.Content = loadIntoPages[0][12];

            List<System.Windows.Controls.Label> vetDemosDisplay = new List<System.Windows.Controls.Label>
            {
                pClinicEntry,pClinicPhoneEntry,pVetNameEntry,pVetNumEntry,sVetNameEntry,sVetNumEntry
            };
            int count = 0;
            foreach (System.Windows.Controls.Label item in vetDemosDisplay)
            {
                item.Content = loadIntoPages[1][count].ToString();
            }
            foreach (string[] list in loadIntoPages)
            {
                if (list[2].EndsWith("~v"))
                {
                    selectDateVet.Items.Add(list[0]);
                }
            }
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
            MiscHandler.uploadImageUpload(activeProfile, ref uploadImagePathName, ref demoPicture);
            imagePopUp.IsOpen = false;
        }

        private void editDemo_Click(object sender, RoutedEventArgs e)
        {
            List<System.Windows.Controls.TextBox> editBoxControl = new List<System.Windows.Controls.TextBox>
            {
                barnNameEdit, regNameEdit,regNumEdit,sexEdit,foalingDateEdit,heightEdit, colourEdit,markingsEdit,brandEdit
            };
            List<System.Windows.Controls.Label> toUpdateLabels = new List<System.Windows.Controls.Label>
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
                List<System.Windows.Controls.TextBox> editBoxControlUpdated = new List<System.Windows.Controls.TextBox>
            {
                barnNameEdit, regNameEdit,regNumEdit,sexEdit,foalingDateEdit,heightEdit, colourEdit,markingsEdit,brandEdit
            };
                MiscHandler.saveDataToFile(editBoxControlUpdated, activeProfile, "Demo");

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
            List<System.Windows.Controls.TextBox> vetDemos = new List<System.Windows.Controls.TextBox>
            {
                pClinicEdit,pClinicPhEdit,pVetNameEdit,pVetPhEdit,sVetNameEdit,sVetPhEdit
            };
            List<System.Windows.Controls.Label> vetDemosDisplay = new List<System.Windows.Controls.Label>
            {
                pClinicEntry,pClinicPhoneEntry,pVetNameEntry,pVetNumEntry,sVetNameEntry,sVetNumEntry
            };
            System.Windows.Controls.Button buttonState = (sender as System.Windows.Controls.Button);
            vetPage.editSaveVetDemo(buttonState,vetDemos, vetDemosDisplay);
            MiscHandler.saveDataToFile(vetDemos, activeProfile,"vet");
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

        private void selectDateVet_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
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
            
            vetPage.saveVetEntry(entryBoxVet.Text,dateLabel.Content.ToString());
        }

        #endregion

        //Farrier
        #region


        #endregion

        private void editSaveFarrierDemos_Click(object sender, RoutedEventArgs e)
        {
            var buttonState = (sender as System.Windows.Controls.Button);
            if (buttonState.Content.ToString() == "Edit")
            {
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
    }
}
