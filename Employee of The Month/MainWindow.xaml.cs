using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Employee_of_The_Month
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Login stuff
        private void BLLogin_Click(object sender, RoutedEventArgs e)
        {
            //Login page visibility
            
            BLLogin.Visibility = Visibility.Hidden;

            //Main menu visibility
            MainMenu.Visibility = Visibility.Visible;
        }

        //Main menu stuff
        private void BMMVote_Click(object sender, RoutedEventArgs e)
        {

            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            Vote.Visibility = Visibility.Visible;
        }

        private void BMMCurrentStandings_Click(object sender, RoutedEventArgs e)
        {

            //Main Text Change
            TEmployeeOfTheMonth.Visibility = Visibility.Hidden;

            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            CurrentStandings.Visibility = Visibility.Visible;
        }

        private void BMMPossiblePrizes_Click(object sender, RoutedEventArgs e)
        {

            //Main Text Change
            TEmployeeOfTheMonth.Visibility = Visibility.Hidden;

            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            PossiblePrizes.Visibility = Visibility.Visible;
        }

        private void BMMInfoRules_Click(object sender, RoutedEventArgs e)
        {
            //Main Text Change
            TEmployeeOfTheMonth.Visibility = Visibility.Hidden;

            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            InfoAndRules.Visibility = Visibility.Visible;
        }

        // Vote stuff
        private void BVVote_Click(object sender, RoutedEventArgs e)
        {
            if (VoteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a value", "Error");
                return;
            }
            else
            {
                //Send vote to database
                if (true)
                { //check if vote was registered/has already voted in the database else spit an error code
                    MessageBox.Show("Your vote was succesful", "Congratulations");

                    //Page Change
                    MainMenu.Visibility = Visibility.Visible;
                    Vote.Visibility = Visibility.Hidden;
                }
            }
        }

        //Current Standings stuff
        private void BCSBack_Click(object sender, RoutedEventArgs e)
        {
            //Main Text Change
            TEmployeeOfTheMonth.Visibility = Visibility.Visible;

            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            CurrentStandings.Visibility = Visibility.Hidden;
        }

        //Possible Prizes stuff
        private void BPPBack_Click(object sender, RoutedEventArgs e)
        {
            //Main Text Change
            TEmployeeOfTheMonth.Visibility = Visibility.Visible;

            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            PossiblePrizes.Visibility = Visibility.Hidden;
        }

        //Info and Rules stuff
        private void BIRBack_Click(object sender, RoutedEventArgs e)
        {

            //Main Text Change
            TEmployeeOfTheMonth.Visibility = Visibility.Visible;

            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            InfoAndRules.Visibility = Visibility.Hidden;
        }

    }
}
