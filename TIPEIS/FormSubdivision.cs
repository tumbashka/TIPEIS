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
    public partial class FormSubdivision : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");
        private int minLength = 4;
        private int maxLength = 50;

        public FormSubdivision()
        {
            InitializeComponent();
        }

        private void FormSubdivision_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "Select * from Subdivision";
            selectTable(ConnectionString, selectCommand);
            String selectSubd = "SELECT ID, Account FROM ChartOfAccounts WHERE Account<30 AND Account>19";
            selectCombo(ConnectionString, selectSubd, toolStripComboBox1, "Account", "ID");
            toolStripComboBox1.SelectedIndex = -1;
        }
        //метод для отображения счетов в комбобоксе
        public void selectCombo(string ConnectionString, String selectCommand,
ToolStripComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            comboBox.ComboBox.DataSource = ds.Tables[0];
            comboBox.ComboBox.DisplayMember = displayMember;
            comboBox.ComboBox.ValueMember = valueMember;
            connect.Close();
        }
        private void check()
        {
            if (!(toolStripTextBox1.Text.Length > minLength && toolStripTextBox1.Text.Length < maxLength))
            {
                MessageBox.Show("Поле Название должно содержать не менее 4 и не более 50 символов");
                return;
            }
            if (toolStripComboBox1.Text == "")
            {
                MessageBox.Show("Выберите счет затрат");
                return;
            }
        }
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            check();
            string ConnectionString = @"Data Source=" + sPath +
";New=False;Version=3";
            String selectCommand = "select MAX(ID) from Subdivision";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Subdivision (ID,Name,ChartOfAccountsID) values (" +
           (Convert.ToInt32(maxValue) + 1) + ", '" + toolStripTextBox1.Text + "','" + toolStripComboBox1.ComboBox.Text + "')";
            ExecuteQuery(txtSQLQuery);
            //обновление dataGridView1
            selectCommand = "select * from Subdivision";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";
            toolStripComboBox1.SelectedIndex = -1;
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
            toolStripComboBox1.SelectedIndex = -1;
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
        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение idEmployees выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Subdivision where ID=" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Subdivision";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";
            toolStripComboBox1.SelectedIndex = -1;
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
        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            check();
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение FIO выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeName = toolStripTextBox1.Text;
            //обновление Name
            String selectCommand = "update Subdivision set Name='" + changeName + "' where ID = " + valueId;
            string ConnectionString = @"Data Source=" + sPath +
       ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            string changeidChartOfAccounts = toolStripComboBox1.Text;
            selectCommand = "update Subdivision set ChartOfAccountsID='" + changeidChartOfAccounts + "' where ID = " + valueId;
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "select * from Subdivision";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBox1.Text = "";
            toolStripComboBox1.SelectedIndex = -1;
        }
        private void dataGridView1_CellMouseClick(object sender,
DataGridViewCellMouseEventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получение значения
            string NameId = dataGridView1[1, CurrentRow].Value.ToString();
            toolStripTextBox1.Text = NameId;
            string ChartOfAccountsId = dataGridView1[2, CurrentRow].Value.ToString();
            toolStripComboBox1.Text = ChartOfAccountsId;
        }
    }
}
