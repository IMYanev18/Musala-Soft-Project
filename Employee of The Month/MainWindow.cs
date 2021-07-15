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

        int stupidDB;
        DBAccess objDBAccess = new DBAccess();
        DataTable dtUsers = new DataTable();
        DataTable dtPrizes = new DataTable();

        public MainWindow()
        {
            InitializeComponent();
        }


        // Login stuff
        private void BLLogin_Click(object sender, RoutedEventArgs e)
        {
            //string usernames and passwords 
            string usernameTxt = txtUsername.Text;
            string passwordTxt = txtPassword.Password;

            //checks if username text box is empty and spits a message
            if(usernameTxt.Equals("")){
                MessageBox.Show("Username field can't be empty");
            }
            
            //checks if password text box is empty and spits a message
            else if (passwordTxt.Equals(""))
            {
                MessageBox.Show("Password field can't be empty");
            }
            
            //means both fields are filled with something
            else
            {
                //creates a string with the query you send the database
                string query = "SELECT Id, Email, Username, Vote,Admin FROM Employees WHERE Username ='" + usernameTxt + "'AND [Password] ='" + passwordTxt+"'";
                               
                //sends the query to the database and fills return in the data table dtUsers
                objDBAccess.readDatathroughAdapter(query, dtUsers);

                //if data table has something in it that means that the username and password are correct
                if (dtUsers.Rows.Count == 1)
                {
                    // We assign the information from the table to variables and we clear the data table
                    Id = dtUsers.Rows[0]["Id"].ToString();
                    Email = dtUsers.Rows[0]["Email"].ToString();
                    Username = dtUsers.Rows[0]["Username"].ToString();
                    YourVote = dtUsers.Rows[0]["Vote"].ToString();
                    Admin = dtUsers.Rows[0]["Admin"].ToString();
                    
                    dtUsers.Clear();


                    //Checks if the account is admin
                    if (Admin == "False")
                    {
                        //Hides login page
                        Login.Visibility = Visibility.Hidden;

                        //Shows main menu page
                        MainMenu.Visibility = Visibility.Visible;
                        
                    }

                    else
                    {
                        //hides login page
                        Login.Visibility = Visibility.Hidden;

                        //shows admin main menu
                        MainMenuAdmin.Visibility = Visibility.Visible;
                        
                    }
                }

                //if the data table is empty that means either the username or the password are wrong
                else
                {
                    MessageBox.Show("Your username or password is wrong");
                }


            }
 
            

                
        }


        //Forgot Username/Password stuff
        private void BFUPSend_Click(object sender, RoutedEventArgs e)
        {
            string EnteredEmail = EmailTxt.Text;

            string query = "SELECT Username,[Password] FROM Employees WHERE Email='" + EnteredEmail+"'";
            dtUsers.Clear();
            objDBAccess.readDatathroughAdapter(query, dtUsers);

            if(dtUsers.Rows.Count == 1)
            {
                MessageBox.Show("Succesfully sent Username and Password to that email.","Success");
                dtUsers.Clear();
                ForgotUsernamePassword.Visibility = Visibility.Hidden;
                Login.Visibility = Visibility.Visible;
                EmailTxt.Clear();
            }
            else
            {
                MessageBox.Show("A user with that email doesn't exist!", "Error");
                dtUsers.Clear();
                ForgotUsernamePassword.Visibility = Visibility.Hidden;
                Login.Visibility = Visibility.Visible;
                EmailTxt.Clear();
            }

        }


        //Main menu stuff
        private void BMMVote_Click(object sender, RoutedEventArgs e)
        {
            string query2 = "SELECT Vote FROM Employees WHERE Username ='" + Username + "'";

            objDBAccess.readDatathroughAdapter(query2, dtUsers);

            YourVote = dtUsers.Rows[0]["Vote"].ToString();
            dtUsers.Clear();

            if (YourVote == "" && stupidDB==0)
            {

                string query = "SELECT Username FROM Employees WHERE NOT Username='" + Username + "'";

                objDBAccess.readDatathroughAdapter(query, dtUsers);

                VoteComboBox.ItemsSource = dtUsers.DefaultView;

                VoteComboBox.DisplayMemberPath = "Username";
                VoteComboBox.SelectedValuePath = "Username";


                MainMenu.Visibility = Visibility.Hidden;
                Vote.Visibility = Visibility.Visible;
                stupidDB = 1;
                
            }
            else
            {
                MessageBox.Show("You have already voted!");
            }


        }

        private void BMMCurrentStandings_Click(object sender, RoutedEventArgs e)
        {
            dtUsers.Clear();
            string query= "SELECT Username, Vote, Votes, ROW_NUMBER() OVER (ORDER BY Votes DESC) AS Place FROM Employees";

            objDBAccess.readDatathroughAdapter(query, dtUsers);

            DGCurrentStandings.ItemsSource = dtUsers.AsDataView();
            objDBAccess.closeConn();
            //Page Change
            MainMenu.Visibility = Visibility.Hidden;
            CurrentStandings.Visibility = Visibility.Visible;
        }

        private void BMMPossiblePrizes_Click(object sender, RoutedEventArgs e)
        {
            dtPrizes.Clear();
            string query = "SELECT Id, Prize FROM Prizes";

            objDBAccess.readDatathroughAdapter(query, dtPrizes);

            DGPossiblePrizes.ItemsSource = dtPrizes.AsDataView();
            objDBAccess.closeConn();

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
                object selectedItem = VoteComboBox.SelectedValue;

                string query = "UPDATE Employees SET Vote = '"+ selectedItem.ToString() +"' WHERE Id = 1; UPDATE Employees SET Votes = Votes + 1 WHERE Username = '"+ selectedItem.ToString()+"'";

                SqlCommand updateCommand = new SqlCommand(query);

                updateCommand.Parameters.AddWithValue("Vote",selectedItem.ToString());
                updateCommand.Parameters.AddWithValue("Votes", selectedItem.ToString());

                objDBAccess.executeQuery(updateCommand);

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
        private void DataGridCurrentStandings(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Id")
            {
                e.Column = null;
            }
            if (e.PropertyName == "Email")
            {
                e.Column = null;
            }
            if (e.PropertyName == "Password")
            {
                e.Column = null;
            }
            if (e.PropertyName == "Admin")
            {
                e.Column = null;
            }

        }

        private void BCSBack_Click(object sender, RoutedEventArgs e)
        {
            dtUsers.Clear();

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
