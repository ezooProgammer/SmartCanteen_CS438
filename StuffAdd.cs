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
    public partial class StuffAdd : Form
    {
        public StuffAdd()
        {
            InitializeComponent();
        }
        public int id = 0;
        public int idT = 0;
        private void addtur()
        {
            string tryrype = "add new Employee";


            string query = "Insert into TransactionID Values (@Acc , @Type , @action , @Date)";

            Hashtable ht = new Hashtable();
            ht.Add("@idt", idT);
            ht.Add("@Acc", MainClass.names);
            ht.Add("@Type", tryrype);
            ht.Add("@action", txtname.Text);
            ht.Add("@Date", DateTime.Today.Date);

            MainClass.SQL(query, ht);

        }


        private void SaveBtn_Click(object sender, EventArgs e)
        {

            if (txtname.Text == "")
            {
                txtname.BorderColor = Color.Red;
                txtname.FocusedState.BorderColor = Color.Red;
                txtname.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Employee name Empty ....");
                txtname.Focus();
                return;
            }
          

            else if (txtphone.Text.Length < 10 ) 
            {
                txtpass.BorderColor = 
                    Color.Red;
                txtpass.FocusedState.BorderColor = Color.Red;
                txtpass.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons =
                    Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon =
                    Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("The phone number should be 10 digits...");
                txtphone.Focus();
                return;
            }           
            else if (txtpass.Text == "")
            {
                txtpass.BorderColor = Color.Red;
                txtpass.FocusedState.BorderColor = Color.Red;
                txtpass.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please enter  Employee Password.....");
                txtpass.Focus();
                return;
            }
            else if (txtuser.Text == "")
            {
                txtuser.BorderColor = Color.Red;
                txtuser.FocusedState.BorderColor = Color.Red;
                txtuser.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please enter Account Name.....");
                txtuser.Focus();
                return;
            }
            else
            {
                string query = "";
                if (id == 0)//insert
                {
                    query = "Insert into Employee Values (@Name , @Roles , @phone ,  @username , @password , @startdate, @isactive)";
                }
                else//update
                {
                    query = "Update Employee Set Emp_Name = @Name , Role=@Roles , Phone= @phone , User_Name=@username , password=@password , Start_Date=@startdate , IsActive=@isactive  where Emp_ID= @id";
                }
                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@Name", txtname.Text);
                ht.Add("@Roles", cmbrole.Text);
                ht.Add("@phone ", txtphone.Text);
                ht.Add("@username ", txtuser.Text);
                ht.Add("@password ", txtpass.Text);
                ht.Add("@startdate ", Convert.ToDateTime(dtstart.Value));
                ht.Add("@isactive ", guna2ToggleSwitch1.Checked);

                if (MainClass.SQL(query, ht) > 0){
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
                    guna2MessageDialog1.Show("saved successfullly...");
                    addtur();
                    this.Close();
                    id = 0;
                    txtname.Text = "";
                    txtphone.Text = "";
                    dtstart.Text = "";
                    txtpass.Text = "";
                    txtuser.Text = "";
                    cmbrole.SelectedIndex = -1;
                    txtname.Focus();
                }
            }
        }
        private void forupdateloaddata()
        {
            string query = @"select *from  Employee where Emp_ID = " + id + "";
            SqlCommand cmd = new SqlCommand(query, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);


            if (dt.Rows.Count > 0)
            {
                //txtid.Text = dt.Rows[0]["Emp_ID"].ToString();
                txtname.Text = dt.Rows[0]["Emp_Name"].ToString();
                cmbrole.SelectedItem = dt.Rows[0]["Role"].ToString();
                txtphone.Text = dt.Rows[0]["Phone"].ToString();
                txtuser.Text = dt.Rows[0]["User_Name"].ToString();
                txtpass.Text = dt.Rows[0]["password"].ToString();
                dtstart.Text = dt.Rows[0]["Start_Date"].ToString();
                guna2ToggleSwitch1.Checked = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
            }

        }
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void StuffAdd_Load(object sender, EventArgs e)
        {
#pragma warning disable CS0219 // Variable is assigned but its value is never used
            string query = "Select Emp_ID from Employee";
#pragma warning restore CS0219 // Variable is assigned but its value is never used
       
            if (id > 0)
            {
                forupdateloaddata();
            }

        }

        private void txtphone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsControl(e.KeyChar)&& !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
