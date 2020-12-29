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
    public partial class FormMaterial : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");
        private int minLength = 4;
        private int maxLength = 50;
        public FormMaterial()
        {
            InitializeComponent();
        }
        private void FormMaterial_Load(object sender, System.EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            String selectCommand = "Select Material.ID, Material.Name, Material.Type, Material.Price from Material";
            selectTable(ConnectionString, selectCommand);
            String selectType = "SELECT Account, Description FROM ChartOfAccounts WHERE Account<11 AND Account>10";
            selectCombo(ConnectionString, selectType, toolStripComboBoxType, "Description", "Account");
            toolStripComboBoxType.SelectedIndex = -1;
        }
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
        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            if (!(toolStripTextBoxName.Text.Length >= minLength && toolStripTextBoxName.Text.Length < maxLength))
            {
                MessageBox.Show("Поле Название должно содержать не менее 4 и не более 50 символов");
                return;
            }
            if (toolStripComboBoxType.Text == "")
            {
                MessageBox.Show("Выберите тип");
                return;
            }
            if (toolStripTextBoxPrice.Text.IndexOf('.') > 0)
            {
                if (toolStripTextBoxPrice.Text.Substring(toolStripTextBoxPrice.Text.IndexOf('.')).Length > 3)
                {
                    MessageBox.Show("Цена должна быть не более 15 символов и иметь не более 2-ух знаков после запятой");
                    return;
                }
            }
            string ConnectionString = @"Data Source=" + sPath +
         ";New=False;Version=3";
            String selectCommand = "select MAX(ID) from Material";
            object maxValue = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValue) == "")
                maxValue = 0;
            string txtSQLQuery = "insert into Material (ID, Name, Type, Price) values (" +
           (Convert.ToInt32(maxValue) + 1) + ", '" + toolStripTextBoxName.Text + "', '" + toolStripComboBoxType.ComboBox.Text + "', " + toolStripTextBoxPrice.Text +")";
            ExecuteQuery(txtSQLQuery);
            //обновление dataGridView1
            selectCommand = "Select Material.ID, Material.Name, Material.Type, Material.Price from Material";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBoxPrice.Text = "";
            toolStripTextBoxName.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
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
            toolStripTextBoxPrice.Text = "";
            toolStripTextBoxName.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
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
            if (!(toolStripTextBoxName.Text.Length >= minLength && toolStripTextBoxName.Text.Length < maxLength))
            {
                MessageBox.Show("Поле Название должно содержать не менее 4 и не более 50 символов");
                return;
            }
            if (toolStripComboBoxType.Text == "")
            {
                MessageBox.Show("Выберите тип");
                return;
            }
            if (toolStripTextBoxPrice.Text.IndexOf('.') > 0)
            {
                if (toolStripTextBoxPrice.Text.Substring(toolStripTextBoxPrice.Text.IndexOf('.')).Length > 3)
                {
                    MessageBox.Show("Цена должна быть не более 15 символов и иметь не более 2-ух знаков после запятой");
                    return;
                }
            }
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение Name выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            string changeName = toolStripTextBoxName.Text;
            string changePrice = toolStripTextBoxPrice.Text;
            string changeType = toolStripComboBoxType.ComboBox.Text;
            //обновление Name
            String selectCommand = "update Material set Name='" + changeName + "', Type='" + changeType + "', Price ='" + changePrice + "' where ID = " + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "Select Material.ID, Material.Name, Material.Type, Material.Price from Material";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBoxPrice.Text = "";
            toolStripTextBoxName.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;
        }

        private void toolStripButtonDel_Click(object sender, EventArgs e)
        {
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение idMOL выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from Material where ID=" + valueId;
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            changeValue(ConnectionString, selectCommand);
            //обновление dataGridView1
            selectCommand = "Select Material.ID, Material.Name, Material.Type, Material.Price from Material";
            refreshForm(ConnectionString, selectCommand);
            toolStripTextBoxPrice.Text = "";
            toolStripTextBoxName.Text = "";
            toolStripComboBoxType.SelectedIndex = -1;

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
            string Name = dataGridView1[1, CurrentRow].Value.ToString();
            toolStripTextBoxName.Text = Name;
            string Type = dataGridView1[2, CurrentRow].Value.ToString();
            toolStripComboBoxType.Text = Type;
            string Price = dataGridView1[3, CurrentRow].Value.ToString();
            toolStripTextBoxPrice.Text = Price;
        }
    }
}
