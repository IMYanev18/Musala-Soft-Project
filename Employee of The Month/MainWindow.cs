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
        public static string Id, Email, Username, YourVote, Admin, WinnerPrize;

        int stupidDB;
        DBAccess objDBAccess = new DBAccess();
        DataTable dtUsers = new DataTable();
        DataTable dtPrizes = new DataTable();
        DataTable dtLastMonthPrize = new DataTable();

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
            if (usernameTxt.Equals("")) {
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
                string query = "SELECT Id, Email, Username, Vote,Admin FROM Employees WHERE Username ='" + usernameTxt + "'AND [Password] ='" + passwordTxt + "'";

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
                    MessageBox.Show("Your username or password are wrong");
                    txtPassword.Clear();
                }


            }




        }


        //Forgot Username/Password stuff
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Login.Visibility = Visibility.Hidden;
            ForgotUsernamePassword.Visibility = Visibility.Visible;
        }

        private void BFUPSend_Click(object sender, RoutedEventArgs e)
        {
            string EnteredEmail = EmailTxt.Text;

            string query = "SELECT Username,[Password] FROM Employees WHERE Email='" + EnteredEmail + "'";
            dtUsers.Clear();
            objDBAccess.readDatathroughAdapter(query, dtUsers);

            if (dtUsers.Rows.Count == 1)
            {
                MessageBox.Show("Succesfully sent Username and Password to that email.", "Success");
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

            if (YourVote == "" && stupidDB == 0)
            {

                string query = "SELECT Username FROM Employees WHERE NOT Username='" + Username + "'AND NOT Admin=1;";

                objDBAccess.readDatathroughAdapter(query, dtUsers);

                VoteComboBox.ItemsSource = dtUsers.DefaultView;

                VoteComboBox.DisplayMemberPath = "Username";
                VoteComboBox.SelectedValuePath = "Username";


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
            dtUsers.Clear();
            string query = "SELECT Username, Vote, Votes, ROW_NUMBER() OVER (ORDER BY Votes DESC) AS Place FROM Employees WHERE NOT Admin=1;";

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

            //LastMonthPrize();

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

                string query = "UPDATE Employees SET Vote = '" + selectedItem.ToString() + "' WHERE Id = '"+Id+"'; UPDATE Employees SET Votes = Votes + 1 WHERE Username = '" + selectedItem.ToString() + "'";

                SqlCommand updateCommand = new SqlCommand(query);

                updateCommand.Parameters.AddWithValue("Vote", selectedItem.ToString());
                updateCommand.Parameters.AddWithValue("Votes", 1.ToString());

                objDBAccess.executeQuery(updateCommand);

                stupidDB = 1;

                MessageBox.Show("Your vote was succesful", "Congratulations");

                //Page Change
                MainMenu.Visibility = Visibility.Visible;
                Vote.Visibility = Visibility.Hidden;
                
            }
        }

        private void BVCancel_Click(object sender, RoutedEventArgs e)
        {
            Vote.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
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

        //Not working
        /*private void LastMonthPrize()
        {
            string query = "SELECT * FROM LastMonthPrize";

            objDBAccess.readDatathroughAdapter(query, dtLastMonthPrize);

            WinnerPrize = dtLastMonthPrize.Rows[0]["LastMonthPrize"].ToString();

            TextBlock TBLastMonthPrize = WinnerPrize.txt ;
        }*/


        //Info and Rules stuff
        private void BIRBack_Click(object sender, RoutedEventArgs e)
        {
            //Page Change
            MainMenu.Visibility = Visibility.Visible;
            InfoAndRules.Visibility = Visibility.Hidden;
        }


        //Auto Choose prize
        private void AutoChoosePrize()
        {
            string query = "SELECT * FROM Prizes DELETE FROM Prizes;";

            objDBAccess.readDatathroughAdapter(query, dtPrizes);

            int RowsWithPrizes = dtPrizes.Rows.Count;
            Random rng = new Random();
            int RandomPrize = rng.Next(1, RowsWithPrizes);

            WinnerPrize = dtPrizes.Rows[RandomPrize]["Prize"].ToString();

            string query2 = "Update LastMonthPrize Set LastMonthPrize='" + WinnerPrize + "'";

            SqlCommand updateCommand = new SqlCommand(query);

            updateCommand.Parameters.AddWithValue("LastMonthPrize", WinnerPrize);

            objDBAccess.executeQuery(updateCommand);
        }

        //Auto Draw on the 1st of every month
        private void AutoDraw()
        {
            DateTime Current = DateTime.Now;
            DateTime CurrentDate = Current.Date;

            DateTime test = new DateTime(2021, 7, 1);


            if (Current.Month.ToString() == "1")
            {
                DateTime Jan = new DateTime(2021, 1, 1);

                if (DateTime.Compare(CurrentDate, Jan) == 0)
                {
                    AutoChoosePrize();

                }
            }
            else if (Current.Month.ToString() == "2")
            {
                DateTime Feb = new DateTime(2021, 2, 1);
                if (DateTime.Compare(CurrentDate, Feb) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "3")
            {
                DateTime Mar = new DateTime(2021, 3, 1);
                if (DateTime.Compare(CurrentDate, Mar) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "4")
            {
                DateTime Apr = new DateTime(2021, 4, 1);
                if (DateTime.Compare(CurrentDate, Apr) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "5")
            {
                DateTime May = new DateTime(2021, 5, 1);
                if (DateTime.Compare(CurrentDate, May) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "6")
            {
                DateTime Jun = new DateTime(2021, 6, 1);
                if (DateTime.Compare(CurrentDate, Jun) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "7")
            {
                DateTime Jul = new DateTime(2021, 7, 1);
                if (DateTime.Compare(CurrentDate, Jul) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "8")
            {
                DateTime Aug = new DateTime(2021, 8, 1);
                if (DateTime.Compare(CurrentDate, Aug) == 0)
                {
                    AutoChoosePrize();
                }
            }

            else if (Current.Month.ToString() == "9")
            {
                DateTime Sep = new DateTime(2021, 9, 1);
                if (DateTime.Compare(CurrentDate, Sep) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "10")
            {
                DateTime Oct = new DateTime(2021, 10, 1);
                if (DateTime.Compare(CurrentDate, Oct) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "11")
            {
                DateTime Nov = new DateTime(2021, 11, 1);
                if (DateTime.Compare(CurrentDate, Nov) == 0)
                {
                    AutoChoosePrize();
                }
            }
            else if (Current.Month.ToString() == "12")
            {
                DateTime Dec = new DateTime(2021, 12, 1);
                if (DateTime.Compare(CurrentDate, Dec) == 0)
                {
                    AutoChoosePrize();
                }
            }
        }




        //Admin Stuff
        private void BMMAAddUser_Click(object sender, RoutedEventArgs e)
        {
            MainMenuAdmin.Visibility = Visibility.Hidden;
            AddUserPage.Visibility = Visibility.Visible;
        }

        private void BMMARemoveUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BMMAAddPrize_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BMMARemovePrize_Click(object sender, RoutedEventArgs e)
        {

        }


        //Admin Add User Page
        //Doesn't check If a user with stuff entered already exists
        private void AAUPSubmit_Click(object sender, RoutedEventArgs e)
        {
            string REmail = AUEmail.Text;
            string RUsername = AUUsername.Text;
            string RPassword = AUPassword.Password;

            if (AUEmail.Equals(""))
            {
                MessageBox.Show("Email field can't be empty");
            }

            
            else if (AUUsername.Equals(""))
            {
                MessageBox.Show("Username field can't be empty");
            }

            else if (AUPassword.Equals(""))
            {
                MessageBox.Show("Password field can't be empty");
            }

            else
            {
                string query = " INSERT INTO Employees (Email,Username,Password,Vote,Votes,Admin) VALUES('" + REmail + "', '" + RUsername + "'," + RPassword + ", NULL, 0, 0);";
                
                SqlCommand CreateCommand = new SqlCommand(query);

                CreateCommand.Parameters.AddWithValue("Email", REmail);
                CreateCommand.Parameters.AddWithValue("Username", RUsername);
                CreateCommand.Parameters.AddWithValue("Password", RPassword);
                CreateCommand.Parameters.AddWithValue("Vote", "NULL");
                CreateCommand.Parameters.AddWithValue("Votes", 0);
                CreateCommand.Parameters.AddWithValue("Admin", 0);

                objDBAccess.executeQuery(CreateCommand);
            }
        }

        private void AAUPCancel_Click(object sender, RoutedEventArgs e)
        {
            AddUserPage.Visibility = Visibility.Hidden;
            MainMenuAdmin.Visibility = Visibility.Visible;
        }
    }
}
