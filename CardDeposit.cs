using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartCanteen
{
    public partial class CardDeposit : Form
    {
        private int cardId;
        private float currentBalance;
        public CardDeposit(int cardId)
        {
            InitializeComponent();
            this.cardId = cardId;
        }
        string i = SmartCard.textsearch;
        public int id;
        public int idT = 0;
        private void addtur()
        {
            string tryrype = "Card Deposit ";


            string query = "Insert into TransactionID Values (@Acc , @Type , @action , @Date)";

            Hashtable ht = new Hashtable();
            ht.Add("@idt", idT);
            ht.Add("@Acc", MainClass.names);
            ht.Add("@Type", tryrype);
            ht.Add("@action", txtamount.Text);
            ht.Add("@Date", DateTime.Today.Date);

            MainClass.SQL(query, ht);

        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2PictureBox7_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtamount.Text))
            {
                txtamount.BorderColor = Color.Red;
                txtamount.FocusedState.BorderColor = Color.Red;
                txtamount.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please enter the deposit value.");
                txtamount.Focus();
                return;
            }

            float depositAmount;
            if (!float.TryParse(txtamount.Text, out depositAmount))
            {
                txtamount.BorderColor = Color.Red;
                txtamount.FocusedState.BorderColor = Color.Red;
                txtamount.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Invalid deposit amount. Please enter a valid number.");
                txtamount.Focus();
                return;
            }

            // تحديد الرصيد الحالي
            float currentBalance = 0;

            MainClass.con.Open();

            SqlCommand balanceCommand = new SqlCommand("SELECT Balance FROM Cards WHERE Card_ID = @cardId", MainClass.con);
            balanceCommand.Parameters.AddWithValue("@cardId", cardId);
            object balanceResult = balanceCommand.ExecuteScalar();
            if (balanceResult != DBNull.Value && balanceResult != null)
            {
                currentBalance = Convert.ToSingle(balanceResult);
            }

            MainClass.con.Close();


            // حساب الرصيد الجديد
            float newBalance = currentBalance + depositAmount;

            // تحديث البيانات في قاعدة البيانات

            MainClass.con.Open();

            SqlCommand updateCommand = new SqlCommand("UPDATE Cards SET Balance = @balance, Deposit = @deposit, Deposit_Date = @depositDate, Empid = @empId WHERE Card_ID = @cardId", MainClass.con);
            updateCommand.Parameters.AddWithValue("@balance", newBalance);
            updateCommand.Parameters.AddWithValue("@deposit", depositAmount);
            updateCommand.Parameters.AddWithValue("@depositDate", DateTime.Today.Date);
            updateCommand.Parameters.AddWithValue("@empId", Convert.ToInt32(MainClass.emploieeid));
            updateCommand.Parameters.AddWithValue("@cardId", cardId);
            updateCommand.ExecuteNonQuery();

            MainClass.con.Close();


            // Add transaction
            addtur();

            // Display success message
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            guna2MessageDialog1.Show("The deposit has been added successfully. New balance: " + newBalance);

            // Clear input and close form
            txtamount.Text = "";
            this.Close();
        }
    
        private void CardDeposit_Load(object sender, EventArgs e)
        {
            txtamount.Focus();
        }

        private void txtamount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }
    }
}
