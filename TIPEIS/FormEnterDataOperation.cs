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
    public partial class FormEnterDataOperation : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");
        int? ID = null;
        int minLength = 1;
        int maxLength = 20;
        double stavkaNDS = 0.18;
        double stavkaProfit = 0.25;
        public FormEnterDataOperation(int? ID)
        {
            this.ID = ID;
            InitializeComponent();
        }

        private void FormEnterDataOperation_Load(object sender, System.EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
          ";New=False;Version=3";
            String selectSubd = "SELECT ID, Name FROM Material";
            selectCombo(ConnectionString, selectSubd, comboBoxMaterial, "Name", "ID");
            comboBoxMaterial.SelectedIndex = -1;

            selectSubd = "SELECT ID, FIO FROM Buyer";
            selectCombo(ConnectionString, selectSubd, comboBoxBuyer, "FIO", "ID");
            comboBoxBuyer.SelectedIndex = -1;

            selectSubd = "SELECT ID, Name FROM Storage";
            selectCombo(ConnectionString, selectSubd, comboBoxStorage, "Name", "ID");
            comboBoxStorage.SelectedIndex = -1;

            selectSubd = "SELECT ID, FIO FROM MOL";
            selectCombo(ConnectionString, selectSubd, comboBoxMOL, "FIO", "ID");
            comboBoxMOL.SelectedIndex = -1;
            if (ID != null)
            {//запрос к бд для получения имеющейся операции
                String selectCommand = "Select Document.ID, Document.Date, TablePart.ProfitAmount, TablePart.Count, Storage.Name, Material.Name, " +
                    "Buyer.FIO,  MOL.FIO from Document join TablePart on TablePart.DocumentID = Document.ID join Storage on Document.StorageID = Storage.ID " +
                    "join Material on Document.MaterialID = Material.ID join MOL on Document.MOLID = MOL.ID join Buyer on Document.BuyerID = Buyer.ID where Document.ID = " + ID.ToString();
                selectOperation(ConnectionString, selectCommand);

            }
        }
        public void selectOperation(string ConnectionString, String selectCommand)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);// ввод данных из операции в поля для редактирования
            dateTimePicker1.DataBindings.Add(new Binding("Text", ds.Tables[0], "Date", true));
            comboBoxMOL.Text = ds.Tables[0].Rows[0].ItemArray[7].ToString();
            comboBoxBuyer.Text = ds.Tables[0].Rows[0].ItemArray[6].ToString();
            comboBoxMaterial.Text = ds.Tables[0].Rows[0].ItemArray[5].ToString();
            comboBoxStorage.Text = ds.Tables[0].Rows[0].ItemArray[4].ToString();
            textBoxCount.Text = ds.Tables[0].Rows[0].ItemArray[3].ToString();
            connect.Close();
        }
        public void selectCombo(string ConnectionString, String selectCommand,
ComboBox comboBox, string displayMember, string valueMember)
        {
            SQLiteConnection connect = new
           SQLiteConnection(ConnectionString);
            connect.Open();
            SQLiteDataAdapter dataAdapter = new
           SQLiteDataAdapter(selectCommand, connect);
            DataSet ds = new DataSet();
            dataAdapter.Fill(ds);
            comboBox.DataSource = ds.Tables[0];
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            connect.Close();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar <= 47 || e.KeyChar >= 59) && e.KeyChar != 8)
                e.Handled = true;
        }
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            if (!(textBoxCount.Text.Length >= minLength && textBoxCount.Text.Length < maxLength))
            {
                MessageBox.Show("Поле Количество должно содержать не менее 1 и не более 20 символов");
                return;
            }
            if (Convert.ToDouble(textBoxCount.Text) <= 0)
            {
                MessageBox.Show("Поле Количество должно быть > 0");
                return;
            }
            if (comboBoxBuyer.Text == "")
            {
                MessageBox.Show("Выберите покупателя");
                return;
            }
            if (comboBoxMaterial.Text == "")
            {
                MessageBox.Show("Выберите материал");
                return;
            }
            if (comboBoxStorage.Text == "")
            {
                MessageBox.Show("Выберите склад");
                return;
            }
            string ConnectionString = @"Data Source=" + sPath +
        ";New=False;Version=3";
            String selectCommand = "select MAX(ID) from Document";
            object maxValueDocID = selectValue(ConnectionString, selectCommand);
            if (Convert.ToString(maxValueDocID) == "")
                maxValueDocID = 0;

            if (ID != null)//редактирование имеющейся операции
            {
                maxValueDocID = ID;
                string txtSQLQueryDocument = "update Document set Date ='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                    "', StorageID =" + comboBoxStorage.SelectedValue + ", MaterialID =" + comboBoxMaterial.SelectedValue +
                    ", BuyerID =" + comboBoxBuyer.SelectedValue + ", MOLID =" + comboBoxMOL.SelectedValue + " where ID = " + maxValueDocID;
                ExecuteQuery(txtSQLQueryDocument);
                //вычисление стоимости 
                selectCommand = "select Price from Material where ID = " + comboBoxMaterial.SelectedValue;
                object materialPrice = selectValue(ConnectionString, selectCommand);
                double profit = Convert.ToDouble(materialPrice) * Convert.ToDouble(textBoxCount.Text) * (1+stavkaProfit);
                double withoutprofit = Convert.ToDouble(materialPrice) * Convert.ToDouble(textBoxCount.Text);
                selectCommand = "select MAX(ID) from TablePart";
                object maxValueID = selectValue(ConnectionString, selectCommand);
                if (Convert.ToString(maxValueID) == "")
                    maxValueID = 0;
                string txtSQLQueryTablePart = "update TablePart set ProfitAmount ='" + profit +
                    "', Count =" + textBoxCount.Text + " where ID = " + maxValueDocID;
                ExecuteQuery(txtSQLQueryTablePart);


                object minValuePostingJournalID = selectValue(ConnectionString, "select MIN(ID) from PostingJournal where TablePartID = " + maxValueDocID);
                string txtSQLQueryPostingJournal = "update PostingJournal set SubkontoDT1 = '"+ comboBoxBuyer.Text+"', Count = '"+ textBoxCount.Text + "', Sum = '"+ 
                    profit + "', Date = '"+ dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' where ID = " + (Convert.ToInt32(minValuePostingJournalID));
                ExecuteQuery(txtSQLQueryPostingJournal);

                object MaterialType = selectValue(ConnectionString, "select Type from Material where ID = " + comboBoxMaterial.SelectedValue);
                object KT = selectValue(ConnectionString, "select ID from ChartOfAccounts where Description =  '" + MaterialType + "'");

                txtSQLQueryPostingJournal = "update PostingJournal set SubkontoDT1 = '" + comboBoxMaterial.Text + "', SubkontoDT2 = '" + comboBoxStorage.Text + "', " +
                    "SubkontoDT3 = '" + comboBoxMOL.Text + "', Count = '" + textBoxCount.Text + "', Sum = '" + withoutprofit + "', Date = '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") +
                    "', AccountKT_ID = '" + KT + "' where ID = "+ (Convert.ToInt32(minValuePostingJournalID)+1);
                ExecuteQuery(txtSQLQueryPostingJournal);

                string nds = Convert.ToString(Math.Round(profit * 0.18, 2));
                txtSQLQueryPostingJournal = "update PostingJournal set Sum = '"+nds+"', Date = '"+ dateTimePicker1.Value.ToString("yyyy-MM-dd") + 
                    "' where ID = "+(Convert.ToInt32(minValuePostingJournalID)+2);  
                ExecuteQuery(txtSQLQueryPostingJournal);

            }
            else // ---ввод новой операции
            {
                maxValueDocID = Convert.ToInt32(maxValueDocID) + 1;
                string txtSQLQueryDocument = "insert into Document (ID, Date, StorageID, MaterialID, BuyerID, MOLID) values (" +
           (maxValueDocID) + ", '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', " + comboBoxStorage.SelectedValue + "," +
           comboBoxMaterial.SelectedValue + "," + comboBoxBuyer.SelectedValue + "," + comboBoxMOL.SelectedValue + ")";
                ExecuteQuery(txtSQLQueryDocument);
                //вычисление стоимости 
                selectCommand = "select Price from Material where ID = " + comboBoxMaterial.SelectedValue;
                object materialPrice = selectValue(ConnectionString, selectCommand);
                double priceWithProfit = Convert.ToDouble(materialPrice) * Convert.ToDouble(textBoxCount.Text) * (1+stavkaProfit);
                double priceWithoutProfit = Convert.ToDouble(materialPrice) * Convert.ToDouble(textBoxCount.Text);
                selectCommand = "select MAX(ID) from TablePart";
                object maxValueID = selectValue(ConnectionString, selectCommand);
                if (Convert.ToString(maxValueID) == "")
                    maxValueID = 0;
                string txtSQLQueryTablePart = "insert into TablePart (ID, ProfitAmount, Count, DocumentID) values (" +
               maxValueDocID + ", '" + priceWithProfit + "', " + textBoxCount.Text + "," + maxValueDocID + ")";
                Console.WriteLine(txtSQLQueryTablePart);
                ExecuteQuery(txtSQLQueryTablePart);

                object maxValuePostingJournalID = selectValue(ConnectionString, "select MAX(ID) from PostingJournal");
                if (Convert.ToString(maxValuePostingJournalID) == "")
                    maxValuePostingJournalID = 0;

                // 62 91
                string txtSQLQueryPostingJournal = "insert into PostingJournal (ID, SubkontoDT1, Count, Sum, Operation, Date, TablePartID, AccountDT_ID, AccountKT_ID) values (" +
               (Convert.ToInt32(maxValuePostingJournalID) + 1) + ", '" + comboBoxBuyer.Text + "', '" + textBoxCount.Text + "', '" + priceWithProfit +
               "', 'Реализация материалов на сторону', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', '" + maxValueDocID + "', '7', '9')";//62  91 субконто - покупатель
                ExecuteQuery(txtSQLQueryPostingJournal);

                object MaterialType = selectValue(ConnectionString, "select Type from Material where ID = " + comboBoxMaterial.SelectedValue);
                object KT = selectValue(ConnectionString, "select ID from ChartOfAccounts where Description =  '" + MaterialType + "'");// субконто - материал склад мол
                //91 10
                txtSQLQueryPostingJournal = "insert into PostingJournal (ID, SubkontoKT1, SubkontoKT2, SubkontoKT3, Count, Sum, Operation, Date, TablePartID, AccountDT_ID, AccountKT_ID) values (" +
               (Convert.ToInt32(maxValuePostingJournalID) + 2) + ", '" + comboBoxMaterial.Text + "', '" + comboBoxStorage.Text + "', '" + comboBoxMOL.Text + "', '" + textBoxCount.Text + "', '" + priceWithoutProfit +
               "', 'Реализация материалов на сторону', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', '" + maxValueDocID + "', '9', '" + KT + "')";// 91  10  
                ExecuteQuery(txtSQLQueryPostingJournal);

                //91 68
                string nds = Convert.ToString(Math.Round(priceWithProfit * stavkaNDS, 2));
                txtSQLQueryPostingJournal = "insert into PostingJournal (ID, Sum, Operation, Date, TablePartID, AccountDT_ID, AccountKT_ID) values (" +
               (Convert.ToInt32(maxValuePostingJournalID) + 3) + " , '" + nds +
               "', 'Реализация материалов на сторону', '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "', '" + maxValueDocID + "', '9', '8')";// 91 68 
                ExecuteQuery(txtSQLQueryPostingJournal);
            }
            Close();
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
    }
}
