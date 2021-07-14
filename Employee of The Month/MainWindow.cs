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
using System.Data.SqlClient;
using System.Data;
using DatabaseProject;



namespace Employee_of_The_Month
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Id, Email, Username, YourVote, Admin;

        DBAccess objDBAccess = new DBAccess();
        DataTable dtUsers = new DataTable();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Login stuff
        private void BLLogin_Click(object sender, RoutedEventArgs e)
        {
            //string usernames and passwords 
            string UsernameTxt = txtUsername.Text;
            string PasswordTxt = txtPassword.Password;

            //checks if username text box is empty and spits a message
            if(UsernameTxt.Equals("")){
                MessageBox.Show("Username field can't be empty");
            }
            
            //checks if password text box is empty and spits a message
            else if (PasswordTxt.Equals(""))
            {
                MessageBox.Show("Password field can't be empty");
            }
            
            //means both fields are filled with something
            else
            {
                //creates a string with the query you send the database
                string query = "SELECT Id ,Username, Admin FROM Employees WHERE Username ='" + UsernameTxt + "'AND [Password] ='" + PasswordTxt+"'";
                               
                //sends the query to the database and fills return in the data table dtUsers
                objDBAccess.readDatathroughAdapter(query, dtUsers);

                //if data table has something in it that means that the username and password are correct
                if (dtUsers.Rows.Count == 1)
                {
                    //Admin gets value either False or True depending on the answer we got from the query
                    Admin = dtUsers.Rows[0]["Admin"].ToString();
                    Id = dtUsers.Rows[0]["Id"].ToString();

                    //Checks if the account is admin
                    if (Admin == "False")
                    {
                        //Hides login page
                        Login.Visibility = Visibility.Hidden;

                        //Shows main menu page
                        MainMenu.Visibility = Visibility.Visible;
                        dtUsers.Clear();
                    }

                    else
                    {
                        //hides login page
                        Login.Visibility = Visibility.Hidden;

                        //shows admin main menu
                        MainMenuAdmin.Visibility = Visibility.Visible;
                        dtUsers.Clear();
                    }
                }

                //if the data table is empty that means either the username or the password are wrong
                else
                {
                    MessageBox.Show("Your username or password is wrong");
                }


            }
 
            

                
        }


        //Main menu stuff
        private void BMMVote_Click(object sender, RoutedEventArgs e)
        {
            
            string query = "SELECT Vote FROM Employees WHERE Id='"+Id+"'";


            objDBAccess.readDatathroughAdapter(query, dtUsers);


            YourVote = dtUsers.Rows[0]["Vote"].ToString();


            if (YourVote == "")
            {
                MainMenu.Visibility = Visibility.Hidden;
                Vote.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("You have already voted!");
            }


        }

        private void BMMCurrentStandings_Click(object sender, RoutedEventArgs e)
        {


            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            CurrentStandings.Visibility = Visibility.Visible;
        }

        private void BMMPossiblePrizes_Click(object sender, RoutedEventArgs e)
        {



            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            PossiblePrizes.Visibility = Visibility.Visible;
        }

        private void BMMInfoRules_Click(object sender, RoutedEventArgs e)
        {


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


            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            CurrentStandings.Visibility = Visibility.Hidden;
        }

        //Possible Prizes stuff
        private void BPPBack_Click(object sender, RoutedEventArgs e)
        {


            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            PossiblePrizes.Visibility = Visibility.Hidden;
        }

        //Info and Rules stuff
        private void BIRBack_Click(object sender, RoutedEventArgs e)
        {



            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            InfoAndRules.Visibility = Visibility.Hidden;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Login.Visibility = Visibility.Hidden;
            ForgotUsernamePassword.Visibility = Visibility.Visible;
        }
    }
}
