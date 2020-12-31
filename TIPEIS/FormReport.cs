using iTextSharp.text;
using iTextSharp.text.pdf;
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
    public partial class FormReport : Form
    {
        private string itogo = "";

        private SQLiteConnection sql_con;
        private SQLiteCommand sql_cmd;
        private DataSet DS = new DataSet();
        private DataTable DT = new DataTable();
        private string sPath = Path.Combine(Application.StartupPath, "C:\\Program Files\\SQLiteStudio\\databases\\tipeis.db");

        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            comboBoxReport.Items.Add("Ведомость продаж материалов покупателям");
            comboBoxReport.Items.Add("Ведомость распределения продаж материалов по месяцам с начала года");
            comboBoxReport.SelectedIndex = -1;
        }

        private void updateTable()
        {
            if (comboBoxReport.SelectedIndex != -1)
            {
                string ConnectionString = @"Data Source=" + sPath + ";New=False;Version=3";
                string dateFrom = dateTimePickerFrom.Value.ToString("dd.MM.yyyy");
                string dateTo = dateTimePickerTo.Value.ToString("dd.MM.yyyy");
                labelSum.Text = "Итого: ";
                itogo = "";

                if (dateTimePickerFrom.Value.Date > dateTimePickerTo.Value.Date)
                {
                    MessageBox.Show("Дата начала периода должна быть меньше даты конца периода");
                    return;
                }
                if (comboBoxReport.SelectedIndex == 0)
                {
                    dateTimePickerFrom.Enabled = true;
                    string selectCommand = "Select Buyer.ID as 'Код покупателя', Buyer.FIO as 'Название покупателя', sum(TablePart.ProfitAmount) as 'Сумма выручки', sum(TablePart.ProfitAmount)/1.25 as 'Сумма себестоимости', "+
                        "(sum(TablePart.ProfitAmount) - sum(TablePart.ProfitAmount) / 1.25) as 'Сумма прибыли/убытка' "+
                        "from Document join TablePart on TablePart.DocumentID = Document.ID join Storage on Document.StorageID = Storage.ID "+
                        "join Buyer on Document.BuyerID = Buyer.ID join MOL on Document.MOLID = MOL.ID where Document.Date >= '" + dateFrom + "'"+
                        "group by Buyer.FIO";
                    selectTable(ConnectionString, selectCommand);

                    double sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                    }
                    itogo += sum + " ";

                    sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[3].Value != DBNull.Value)
                        {
                            sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                        }
                    }

                    itogo += sum + " ";

                    sum = 0;
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (dataGridView1.Rows[i].Cells[4].Value != DBNull.Value)
                        {
                            sum += Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
                        }
                    }
                    itogo += sum;
                    labelSum.Text += itogo;
                }
                if (comboBoxReport.SelectedIndex == 1)
                {
                    dateTimePickerFrom.Enabled = false;
                    string dateTo2 = dateTimePickerTo.Value.ToString("yyyy");
                    //dateTo2 = "01."
                    string selectCommand = "select ID AS 'Код материала' , Name as 'Наименование материала' from Material";
                    selectTable(ConnectionString, selectCommand);

                }
            }
        }

        private void comboBoxReport_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTable();
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
            connect.Close();
        }

        private void dateTimePickerFrom_ValueChanged(object sender, EventArgs e)
        {
            updateTable();
        }

        private void dateTimePickerTo_ValueChanged(object sender, EventArgs e)
        {
            updateTable();
        }

        private void buttonPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string FONT_LOCATION = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.TTF"); //определяем В СИСТЕМЕ(чтобы не копировать файл) расположение шрифта arial.ttf
                    BaseFont baseFont = BaseFont.CreateFont(FONT_LOCATION, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED); //создаем шрифт
                    iTextSharp.text.Font fontParagraph = new iTextSharp.text.Font(baseFont, 17, iTextSharp.text.Font.NORMAL); //регистрируем + можно задать параметры для него(17 - размер, последний параметр - стиль)
                    string title = "";

                    if (comboBoxReport.SelectedIndex == 0)
                    {
                        title = "Ведомость продаж материалов покупателям" + " с " + Convert.ToString(dateTimePickerFrom.Text) + " по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
                    }
                    if (comboBoxReport.SelectedIndex == 1)
                    {
                        title = "Ведомость распределения продаж материалов по месяцам с начала года, по " + Convert.ToString(dateTimePickerTo.Text) + "\n\n";
                    }

                    var phraseTitle = new Phrase(title,
                    new iTextSharp.text.Font(baseFont, 18, iTextSharp.text.Font.BOLD));
                    iTextSharp.text.Paragraph paragraph = new
                   iTextSharp.text.Paragraph(phraseTitle)
                    {
                        Alignment = Element.ALIGN_CENTER,
                        SpacingAfter = 12
                    };

                    PdfPTable table = new PdfPTable(dataGridView1.Columns.Count);

                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        table.AddCell(new Phrase(dataGridView1.Columns[i].HeaderCell.Value.ToString(), fontParagraph));
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            table.AddCell(new Phrase(dataGridView1.Rows[i].Cells[j].Value.ToString(), fontParagraph));
                        }
                    }
                    PdfPTable table2 = new PdfPTable(dataGridView1.Columns.Count);
                    if (comboBoxReport.SelectedIndex == 0)
                    {
                        List<string> words = new List<string>();

                        words.Add("Итого:");
                        words.Add("");
                        if (comboBoxReport.SelectedIndex == 1)
                        {
                            words.Add("");
                        }
                        words.AddRange(itogo.Split(' '));
                        for (int j = 0; j < words.Count; j++)
                        {
                            table2.AddCell(new Phrase(words[j], fontParagraph));
                        }
                    }
                    using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                    {
                        iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A2, 10f, 10f, 10f, 0f);
                        PdfWriter.GetInstance(pdfDoc, stream);
                        pdfDoc.Open();
                        pdfDoc.Add(paragraph);
                        pdfDoc.Add(table);
                        pdfDoc.Add(table2);
                        pdfDoc.Close();
                        stream.Close();
                    }
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
    }
}