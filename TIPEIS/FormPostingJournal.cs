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
    public partial class FormPostingJournal : Form
    {
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");
        String selectAllCommand = "Select PostingJournal.TablePartID, PostingJournal.Operation as Операция, ChartOfAccountsDT.Account AS ДТ, PostingJournal.SubkontoDT1 as СубконтоДТ1, " +
            "PostingJournal.SubkontoDT2 as СубконтоДТ2 , PostingJournal.SubkontoDT3 as СубконтоДТ3 , ChartOfAccountsKT.Account AS КТ, " +
            "PostingJournal.SubkontoKT1 as СубконтоКТ1 , PostingJournal.SubkontoKT2 as СубконтоКТ2 , PostingJournal.SubkontoKT3 as СубконтоКТ3 , " +
            "PostingJournal.Count as 'Кол-во', PostingJournal.Sum as Сумма, PostingJournal.Date  as Дата " +
            "from PostingJournal " +
            "join ChartOfAccounts AS ChartOfAccountsDT on ChartOfAccountsDT.ID = PostingJournal.AccountDT_ID " +
            "join ChartOfAccounts AS ChartOfAccountsKT on ChartOfAccountsKT.ID = PostingJournal.AccountKT_ID";
        int? ID = null;

        public FormPostingJournal(int? ID)
        {
            this.ID = ID;
            InitializeComponent();
        }

        private void FormPostingJournal_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            if (ID != null)
            {
                selectAllCommand = "Select PostingJournal.TablePartID, PostingJournal.Operation as Операция, ChartOfAccountsDT.Account AS ДТ, PostingJournal.SubkontoDT1 as СубконтоДТ1, " +
                    "PostingJournal.SubkontoDT2 as СубконтоДТ2 , PostingJournal.SubkontoDT3 as СубконтоДТ3 , ChartOfAccountsKT.Account AS КТ, " +
                    "PostingJournal.SubkontoKT1 as СубконтоКТ1 , PostingJournal.SubkontoKT2 as СубконтоКТ2 , PostingJournal.SubkontoKT3 as СубконтоКТ3 , " +
                    "PostingJournal.Count as 'Кол-во', PostingJournal.Sum as Сумма, PostingJournal.Date  as Дата " +
                    "from PostingJournal " +
                    "join ChartOfAccounts AS ChartOfAccountsDT on ChartOfAccountsDT.ID = PostingJournal.AccountDT_ID " +
                    "join ChartOfAccounts AS ChartOfAccountsKT on ChartOfAccountsKT.ID = PostingJournal.AccountKT_ID where TablePartID = '" + ID + "'";
            }
            selectTable(ConnectionString, selectAllCommand);
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
            dataGridView1.AutoResizeColumns();
            dataGridView1.RowHeadersVisible = false;
            connect.Close();
        }
        private void updateGrid(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.Value.Date > dateTimePickerTo.Value.Date)
            {
                MessageBox.Show("Дата начала периода должна быть меньше даты конца периода");
                return;
            }
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";

            selectAllCommand = "Select PostingJournal.TablePartID, PostingJournal.Operation as Операция, ChartOfAccountsDT.Account AS ДТ, PostingJournal.SubkontoDT1 as СубконтоДТ1, " +
                "PostingJournal.SubkontoDT2 as СубконтоДТ2 , PostingJournal.SubkontoDT3 as СубконтоДТ3 , ChartOfAccountsKT.Account AS КТ, " +
                "PostingJournal.SubkontoKT1 as СубконтоКТ1 , PostingJournal.SubkontoKT2 as СубконтоКТ2 , PostingJournal.SubkontoKT3 as СубконтоКТ3 , " +
                "PostingJournal.Count as 'Кол-во', PostingJournal.Sum as Сумма, PostingJournal.Date  as Дата " +
                "from PostingJournal " +
                "join ChartOfAccounts AS ChartOfAccountsDT on ChartOfAccountsDT.ID = PostingJournal.AccountDT_ID " +
                "join ChartOfAccounts AS ChartOfAccountsKT on ChartOfAccountsKT.ID = PostingJournal.AccountKT_ID where PostingJournal.Date >= '"+ 
                dateTimePickerFrom.Value.ToString("dd-MM-yyyy") + "' and PostingJournal.Date <= '"+ dateTimePickerTo.Value.ToString("dd-MM-yyyy") + "'";
            if (ID != null)
            {
                if (selectAllCommand.Contains("where"))
                {
                    selectAllCommand += " and ";
                }
                else
                {
                    selectAllCommand += " where ";
                }
                selectAllCommand+= "TablePartID = '" + ID + "'";
            }
            selectTable(ConnectionString, selectAllCommand);

        }
    }
}
