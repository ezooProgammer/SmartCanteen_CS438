using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartCanteen
{
    public partial class AddStudent : Form
    {
        public AddStudent()
        {
            InitializeComponent();
        }
        public int id = 0;
        private void AddStudent_Load(object sender, EventArgs e)
        {
            if (id > 0)
            {
                forupdateloaddata();
            }
        }
        string fillpath;
        Byte[] ImageByteArray;
        private void saveimg_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images(.jpg , .png)|* .png; *.jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fillpath = ofd.FileName;
                pict.Image = new Bitmap(fillpath);
            }
        }
        public int idT = 0;
        private void addtur()
        {
            string tryrype = "Add new student";
            string query = "Insert into TransactionID Values (@Acc , @Type , @action , @Date)";
            Hashtable ht = new Hashtable();
            ht.Add("@idt", idT);
            ht.Add("@Acc", MainClass.names);
            ht.Add("@Type", tryrype);
            ht.Add("@action", txtid.Text);
            ht.Add("@Date", DateTime.Today.Date);
            MainClass.SQL(query, ht);
        }
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            string query = "";
            if (txtid.Text == "")
            {
                txtid.BorderColor = Color.Red;
                txtid.FocusedState.BorderColor = Color.Red;
                txtid.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please enter student registration number.....");
                txtid.Focus();
                return;
            }
            else if (txtname.Text == "")
            {
                txtname.BorderColor = Color.Red;
                txtname.FocusedState.BorderColor = Color.Red;
                txtname.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please enter Student name....");
                txtname.Focus();
                return;
            }
            else if (txtg.Text == "")
            {
                txtg.BorderColor = Color.Red;
                txtg.FocusedState.BorderColor = Color.Red;
                txtg.HoverState.BorderColor = Color.Red;
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Please enter Student Grade .....");
                txtg.Focus();
                return;
            }
            else
            {
                if (id == 0)//insert
                {
                    query = "Insert into Student Values (@id , @Name , @Grade , @ParentNumber , @image)";
                }
                else//update
                {
                    query = "Update Student Set Student_ID=@id , Student_Name= @Name , Grade=@Grade , Parent_Number=@ParentNumber , Image_Stu=@image   where Student_ID = @id";
                }
                Image temp = new Bitmap(pict.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                ImageByteArray = ms.ToArray();
                Hashtable ht = new Hashtable();
                ht.Add("@id", txtid.Text);
                ht.Add("@Name", txtname.Text);
                ht.Add("@Grade", txtg.Text);
                ht.Add("@ParentNumber", txtnum.Text);
                ht.Add("@image", ImageByteArray);
                if (MainClass.SQL(query, ht) > 0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
                    guna2MessageDialog1.Show("saved successfullly...");
                    addtur();
                    txtid.Text = "";
                    txtname.Text = "";
                    txtg.Text = "";
                    txtnum.Text = "";
                    txtid.Focus();
                }
            }
        }
        private void guna2PictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void forupdateloaddata()
        {
            string query = @"select *from  Student where Student_ID = " + id + "";
            SqlCommand cmd = new SqlCommand(query, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                txtid.Text = dt.Rows[0]["Student_ID"].ToString();
                txtname.Text = dt.Rows[0]["Student_Name"].ToString();
                txtg.Text = dt.Rows[0]["Grade"].ToString();
                txtnum.Text = dt.Rows[0]["Parent_Number"].ToString();
                Byte[] imagearray = (byte[])(dt.Rows[0]["Image_Stu"]);
                byte[] imagebytearray = imagearray;
                pict.Image = Image.FromStream(new MemoryStream(imagearray));
            }
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2PictureBox3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void txtid_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }
    }
}
