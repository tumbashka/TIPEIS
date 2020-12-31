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
    public partial class FormJournalOperation : Form
    {
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");
        String selectAllCommand = "Select Document.ID, Document.Date, TablePart.ProfitAmount as Выручка, TablePart.Count as 'Кол-во', Storage.Name as Склад, Material.Name as Материал, " +
                "Buyer.FIO as Покупатель, MOL.FIO as МОЛ from Document join TablePart on TablePart.DocumentID = Document.ID join Storage on Document.StorageID = Storage.ID " +
                "join Material on Document.MaterialID = Material.ID join Buyer on Document.BuyerID = Buyer.ID join MOL on Document.MOLID = MOL.ID";

        public FormJournalOperation()
        {
            InitializeComponent();
        }

        private void FormJournalOperation_Load(object sender, System.EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            selectTable(ConnectionString, selectAllCommand);
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            FormEnterDataOperation form = new FormEnterDataOperation(null);
            form.ShowDialog();
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            selectTable(ConnectionString, selectAllCommand);
        }
         
        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex >= 0)
            {
                //выбрана строка CurrentRow
                int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
                //получить значение Name выбранной строки
                int valueId = Convert.ToInt32(dataGridView1[0, CurrentRow].Value.ToString());
                FormEnterDataOperation form = new FormEnterDataOperation(valueId);
                form.ShowDialog();
                string ConnectionString = @"Data Source=" + sPath +
               ";New=False;Version=3";
                selectTable(ConnectionString, selectAllCommand);
            }
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
            connect.Close();

            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = ds.Tables[0].ToString();
            dataGridView1.AutoResizeColumns();
            dataGridView1.RowHeadersVisible = false;
           
            connect.Close();
        }


        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string ConnectionString = @"Data Source=" + sPath +
           ";New=False;Version=3";
            //выбрана строка CurrentRow
            int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
            //получить значение id выбранной строки
            string valueId = dataGridView1[0, CurrentRow].Value.ToString();
            String selectCommand = "delete from TablePart where ID=" + valueId;
            changeValue(ConnectionString, selectCommand);
            selectCommand = "delete from Document where ID=" + valueId;
            changeValue(ConnectionString, selectCommand);


            selectCommand = "delete from PostingJournal where TablePartID=" + valueId;
            changeValue(ConnectionString, selectCommand);


            //обновление dataGridView1
            selectTable(ConnectionString, selectAllCommand);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells[0].RowIndex >= 0)
            {
                //выбрана строка CurrentRow
                int CurrentRow = dataGridView1.SelectedCells[0].RowIndex;
                //получить значение Name выбранной строки
                int valueId = Convert.ToInt32(dataGridView1[0, CurrentRow].Value.ToString());
                FormPostingJournal form = new FormPostingJournal(valueId);
                form.ShowDialog();
                string ConnectionString = @"Data Source=" + sPath +
               ";New=False;Version=3";
                selectTable(ConnectionString, selectAllCommand);
            }
        }
    }
}
