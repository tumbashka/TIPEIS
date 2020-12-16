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
        String selectAllCommand = "Select PostingJournal.SubkontoDT1, PostingJournal.SubkontoDT2, PostingJournal.SubkontoDT3, PostingJournal.Count, PostingJournal.Sum, PostingJournal.Operation, "+
"PostingJournal.SubkontoKT, PostingJournal.Date, ChartOfAccountsDT.Account AS DT, ChartOfAccountsKT.Account AS KT "+
"from PostingJournal "+
"join ChartOfAccounts AS ChartOfAccountsDT on ChartOfAccountsDT.ID = PostingJournal.AccountDT_ID "+
"join ChartOfAccounts AS ChartOfAccountsKT on ChartOfAccountsKT.ID = PostingJournal.AccountKT_ID";

        public FormPostingJournal()
        {
            InitializeComponent();
        }

        private void FormPostingJournal_Load(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
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
            connect.Close();
        }

       
    }
}
