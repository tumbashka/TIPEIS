using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIPEIS
{
    public partial class FormStorages : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");
        int minLength = 4;
        int maxLength = 50;
        public FormStorages()
        {
            InitializeComponent();
        }
        private void FormStorages_Load(object sender, System.EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            String selectCommand = "Select * from Storage";
            selectTable(ConnectionString, selectCommand);
        }
        public void selectTable(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = ds.Tables[0].ToString();
            connect.Close();
        }

        private void check()
        {
            if (!(toolStripTextBox2.Text.Length >= minLength && toolStripTextBox1.Text.Length < maxLength))
            {
                MessageBox.Show("Поле Адрес должно содержать не менее 4 и не более 50 символов");
                return;
            }
            if (!(toolStripTextBox1.Text.Length >= minLength && toolStripTextBox1.Text.Length < maxLength))
            {
                MessageBox.Show("Поле Название должно содержать не менее 4 и не более 50 символов");
                return;
            }
        }
        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            check();

            string ConnectionString = @"Data Source=" + sPath +
         ";New=False;Version=3";
            String selectCommand = "select MAX(ID) from Storage";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Storage (ID, Adress, Name) values (" +
           (Convert.ToInt32(maxValue) + 1) + ", '" + toolStripTextBox2.Text +"', '"+ toolStripTextBox1.Text +"')";
            ExecuteQuery(txtSQLQuery);
            //обновление dataGridView1
            selectCommand = "select * from Storage";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox2.Text = "";
            toolStripTextBox1.Text = "";

        }
        private void ExecuteQuery(string txtQuery)
        {
            sql_con = new SQLiteConnection("Data Source=" + sPath +
           ";Version=3;New=False;Compress=True;");
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();
        }
        public void refreshForm(string ConnectionString, String selectCommand)
        {
            selectTable(ConnectionString, selectCommand);
            dataGridView1.Update();
            dataGridView1.Refresh();
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
        }
        public object selectValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteCommand command = new SQLiteCommand(selectCommand,
           connect);
            SQLiteDataReader reader = command.ExecuteReader();
            object value = "";
            while (reader.Read())
            {
                value = reader[0];
            }
            connect.Close();
            return value;
        }
        private void toolStripButtonChange_Click(object sender, EventArgs e)
        {
            check();
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeAdress = toolStripTextBox2.Text;
            string changeName = toolStripTextBox1.Text;
            //обновление Name
            String selectCommand = "update Storage set Adress='" + changeAdress +"', Name ='"+ changeName + "'where ID = " + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Storage";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
        }

        private void toolStripButtonDel_Click(object sender, EventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение idMOL выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Storage where ID=" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Storage";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";
            toolStripTextBox2.Text = "";
        }
        public void changeValue(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteTransaction trans;
            SQLiteCommand cmd = new SQLiteCommand();
            trans = connect.BeginTransaction();
            cmd.Connection = connect;
            cmd.CommandText = selectCommand;
            cmd.ExecuteNonQuery();
            trans.Commit();
            connect.Close();
        }
        private void dataGridView1_CellMouseClick(object sender,
DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            string Adress = dataGridView1[1, CurrentRow].Value.ToString();
            toolStripTextBox2.Text = Adress;
            string Name = dataGridView1[2, CurrentRow].Value.ToString();
            toolStripTextBox1.Text = Name;
        }
    }
}
