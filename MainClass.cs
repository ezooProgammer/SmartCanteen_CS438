using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmartCanteen
{
    internal class MainClass
    {
        public static readonly string con_string = @"Data Source = DESKTOP-6KER5J3\MSSQLSERVER1 ; Initial Catalog = SmartDB; Integrated Security = True";
        public static SqlConnection con = new SqlConnection(con_string);


        public static bool IsValidUser(string user, string pass)
        {
            bool isValid = false;
            string query = "SELECT *FROM Employee WHERE User_Name='" + user + "' and password='" + pass + "' ";//أمر ال sql
            //يستخدم هذه الامر للتعامل مع جمل sql statment 
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            //يستخدم للاحتفاظ ب اوامر select, insert, update, and delete التي تستخدم بعد ذلك للتعامل مع DataSet
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            //دالة لاسترجاع البيانات المخزنة في المصدر 
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                //تخزنهم بواسطة الدالة
                isValid = true;
                emploieeid = Convert.ToInt32(dt.Rows[0]["Emp_ID"].ToString());
                names = dt.Rows[0]["Emp_Name"].ToString();
                USER = dt.Rows[0]["Role"].ToString();
                active = Convert.ToBoolean(dt.Rows[0]["IsActive"].ToString());
            }
            return isValid;
        }
        public static int idemp;
        public static int emploieeid
        {
            get { return idemp; }
            private set {idemp = value; }
        }
        public static string na;
        public static string names
        {
            get { return na; }
            private set { na = value; }
        }
        public static string user;
        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }
        public static bool act;
        public static bool active
        {
            get { return act; }
            private set { act = value; }
        }
        //method for curd operation
        public static int SQL(string query, Hashtable ht)
        {
            int res = 0;// بنستخدموه باش نخزنو عدد السصوف في في الاستعلام 
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                    cmd.CommandType = CommandType.Text;
                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                    res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                con.Close();
            }
            return res;
        }
        //تحميل البيانات من اداتابيس الى اداة datagridview 
        public static void LoadData(string query, DataGridView gv, ListBox lb)
        {
            //هده السطر لكي نظهر الرقم التسلسلي للاصناف
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);
            //انتهاء السطر 
            try
           {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                for (int i = 0; i < lb.Items.Count; i++)
                {
                    string colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();

                }
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

                con.Close();

            }
        }
        // هده الدالة لاستدعاء من الداتابيس البيانات
        public static DataTable RetriveData(String query)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            return dt;
        }
        //دالة خاصيه التي تجعل الاي دي تبع الاصناف يظهر 
        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }
        //تعبئة اداة comobox 
        public static void fillcom(string query, ComboBox cb)
        {
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cb.DataSource = dt;
            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.SelectedIndex = -1;
        }
        public static void BlueBackGround(Form modele)
        {
            Form Background = new Form();
            using (modele)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = MainForm.instance.Size;
                Background.Location = MainForm.instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                modele.Owner = Background;
                modele.ShowDialog(Background);
                Background.Dispose();
            }
        }
    }
}

