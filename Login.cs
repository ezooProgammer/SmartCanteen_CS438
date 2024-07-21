using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartCanteen
{
    public partial class login : Form
    {
      
        

        public login()
        {
            InitializeComponent();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Minimizebtn_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private bool ContainsOnlyNumbers(string input){
            return input.All(char.IsDigit);
        }
        private bool ContainsOnlyLetters(string input)
        {
            return input.All(char.IsLetter);
        }

        private bool ContainsOnlyUpperCase(string input)
        {
            return input.All(char.IsUpper);
        }

        private bool ContainsOnlyLowerCase(string input)
        {
            return input.All(char.IsLower);
        }


        public void button2_Click(object sender, EventArgs e){
            if (userNameButton.Text == ""){
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("User Name Is Empty ..");
            }
            else if (string.IsNullOrEmpty(userNameButton.Text) || 
                      ContainsOnlyNumbers(userNameButton.Text)){
                guna2MessageDialog1.Buttons = 
                    Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon =
                    Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("User Name is empty or contains only numbers.");
            }

            else if (passwordbutton.Text == "")
            {
                guna2MessageDialog1.Buttons =
                    Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon =
                    Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Password Is Empty..");
            }
            else if (passwordbutton.Text.Length < 8)
            {
                guna2MessageDialog1.Buttons =
                    Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon =
                    Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Password must be at least 8 characters");
            }

            else if (ContainsOnlyNumbers(userNameButton.Text))
            {
                guna2MessageDialog1.Buttons =
                    Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon =
                    Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("contains only numbers.");
            }
            

            else if (MainClass.IsValidUser(userNameButton.Text, passwordbutton.Text) == false)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Invalid username or password. Please try again");
            }
            else if (MainClass.active == false)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Your account has been deactivated for administrative reasons. Please contact with administration. Thank you");
            }
            else if (MainClass.USER == "Casheir")
            {
                this.Hide();
                BuyForm bf = new BuyForm();
                bf.ShowDialog();
            }
            else if (MainClass.USER == "Manager")
            {
                MainForm mf = new MainForm();
                this.Hide();
                mf.ShowDialog();
            }
        }

        private bool HasUpperAndLowerCase(string password)
        {
            bool hasUpper = false, hasLower = false;
            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUpper = true;
                else if (char.IsLower(c))
                    hasLower = true;
            }
            return hasUpper && hasLower;
        }

        private void login_Load(object sender, EventArgs e)
        {//انشاء مستخدم افتراضي ادا كان الجدول فارغ
            MainClass.con.Open();
            string query = @"select User_Name, password from Employee";
            SqlCommand cmd = new SqlCommand(query, MainClass.con);
            object role = cmd.ExecuteScalar();
            MainClass.con.Close();
            if (role != null)
            {
                this.Show();
            }
            else
            {
                string us = "Admin";
                string pas = "12345";
                string ro = "Manager";
                string qu = "insert into Employee(User_Name, password , Role) values(@username , @passord , @role)";
                MainClass.con.Open();
                using (SqlCommand cmd2 = new SqlCommand(qu, MainClass.con))
                {
                    cmd2.Parameters.AddWithValue("@username", us);
                    cmd2.Parameters.AddWithValue("@passord", pas);
                    cmd2.Parameters.AddWithValue("@role", ro);
                    cmd2.ExecuteNonQuery();
                    MainClass.con.Close();
                }
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
                guna2MessageDialog1.Show("A username has been created which is 'Admin' and a password which is '12345'.");

            }
          
        }

        private void userNameButton_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
