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

        private void SignIn(object sender, RoutedEventArgs e)
        {
            //Login page visibility
            UsernameLogin.Visibility = Visibility.Hidden;
            UsernameBoxLogin.Visibility = Visibility.Hidden;
            PasswordLogin.Visibility = Visibility.Hidden;
            PasswordBoxLogin.Visibility = Visibility.Hidden;
            SignInButton.Visibility = Visibility.Hidden;
            
            //Main menu visibility
            MainMenuPanel.Visibility = Visibility.Visible;
        }

        private void MMVote_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPanel.Visibility = Visibility.Hidden;
            Vote.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (VoteComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a value", "Error");
                return;
            }
            else{
                //Send vote to database
                if (true){ //check if vote was registered/has already voted in the database else spit an error code
                    MessageBox.Show("Your vote was succesful", "Congratulations");
                    MainMenuPanel.Visibility = Visibility.Visible;
                    Vote.Visibility = Visibility.Hidden;
                }
            }
        }

        private void MMCurrentStandings_Click(object sender, RoutedEventArgs e)
        {
            MainMenuPanel.Visibility = Visibility.Hidden;
            CurrentStandings.Visibility = Visibility.Visible;
        }
    }
}
