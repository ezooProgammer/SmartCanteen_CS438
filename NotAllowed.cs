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
    public partial class NotAllowed : Form
    {
        public NotAllowed()
        {
            InitializeComponent();
            lbcardid.Text = SmartCard.textsearch;
        }
        public int id;
        public int cid;
        public int idT = 0;
        private void addtur()
        {
            string tryrype = "amending card prohibitions";


            string query = "Insert into TransactionID Values (@Acc , @Type , @action , @Date)";

            Hashtable ht = new Hashtable();
            ht.Add("@idt", idT);
            ht.Add("@Acc", MainClass.names);
            ht.Add("@Type", tryrype);
            ht.Add("@action", lbcardid.Text);
            ht.Add("@Date", DateTime.Today.Date);

            MainClass.SQL(query, ht);

        }

        public void GetData()
        {
            string query = "select ID_Not_Allowed , ID_Card , ID_Pro , p.Pro_Name from Not_Allowed a inner join Product p on p.Pro_ID = a.ID_Pro";
            query += " where ID_Card like '%" + lbcardid.Text + "%' ";
            ListBox lb = new ListBox();
            lb.Items.Add(dgvida);
            lb.Items.Add(dgvidcard);
            lb.Items.Add(dgvidprod);
            lb.Items.Add(dgvproh);
            MainClass.LoadData(query, gridprohibited, lb);
        }

        private void NotAllowed_Load(object sender, EventArgs e)
        { 
            string query = "Select Pro_ID 'id', Pro_Name 'name' from Product";
            MainClass.fillcom(query, cmbpro);
            if (cid > 0)// for update
            {
                cmbpro.SelectedValue = cid;
            }
            if (id > 0)
            {

            }

            GetData();
        }
        private void lbcardid_Click(object sender, EventArgs e)
        {
            GetData();
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
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            guna2MessageDialog1.Show("saved successfullly...");
            this.Close();
        }

        private void btndone_Click(object sender, EventArgs e)
        {
               MainClass.con.Open();
            if (cmbpro.SelectedIndex == -1)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please Select Products..");

                return;
            }
            else { 

            string qu = "insert into Not_Allowed(ID_Card, ID_Pro) values(@idc, @idp)";
                using (SqlCommand cmd = new SqlCommand(qu, MainClass.con))
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("@idc", lbcardid.Text);
                    ht.Add("@idp", Convert.ToInt32(cmbpro.SelectedValue));
                    if (MainClass.SQL(qu, ht) > 0)
                    {
                        MainClass.con.Close();
                        addtur();
                        id = 0;
                        cmbpro.SelectedIndex = 0;
                        cmbpro.SelectedIndex = -1;
                        cmbpro.Focus();
                    }
                }
            }
            GetData();
        }

        private void gridprohibited_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = gridprohibited.Columns[e.ColumnIndex].Name;
            if (col == "dgvedeit")
            {
                DataGridViewRow row = gridprohibited.Rows[e.RowIndex];
                if (MessageBox.Show(string.Format("DO you want delet this row?.", row.Cells["dgvida"].Value), "Confegration", MessageBoxButtons.YesNo) ==DialogResult.Yes) 
                {
                    string query = "Delete from Not_Allowed where ID_Not_Allowed = @ida";
                    SqlCommand cmd = new SqlCommand(query, MainClass.con);
                    cmd.Parameters.AddWithValue("@ida", row.Cells["dgvida"].Value);
                    MainClass.con.Open();
                    cmd.ExecuteNonQuery();
                    MainClass.con.Close();
                }
            }
            GetData();   
        }
    }
}
    