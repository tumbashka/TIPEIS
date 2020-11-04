using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TIPEIS
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void планСчетовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormChartOfAccounts form = new FormChartOfAccounts();
            form.Show();           
        }

        private void подразделенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormStorages form = new FormStorages();
            form.Show();
        }

        private void материальноответственныеЛицаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMOL form = new FormMOL();
            form.Show();
        }

        private void материалToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMaterial form = new FormMaterial();
            form.Show();
        }

        private void покупательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormBuyer form = new FormBuyer();
            form.Show();
        }

        private void подразделениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormSubdivision form = new FormSubdivision();
            form.Show();
        }
    }
}
