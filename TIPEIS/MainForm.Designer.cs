namespace TIPEIS
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.планСчетовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.подразделенияToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.материальноответственныеЛицаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.материалToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.покупательToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.журналОперацийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.журналПроводокToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отчетToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справочникиToolStripMenuItem,
            this.журналОперацийToolStripMenuItem,
            this.журналПроводокToolStripMenuItem,
            this.отчетToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(649, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.планСчетовToolStripMenuItem,
            this.подразделенияToolStripMenuItem,
            this.материальноответственныеЛицаToolStripMenuItem,
            this.материалToolStripMenuItem,
            this.покупательToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(117, 24);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // планСчетовToolStripMenuItem
            // 
            this.планСчетовToolStripMenuItem.Name = "планСчетовToolStripMenuItem";
            this.планСчетовToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
            this.планСчетовToolStripMenuItem.Text = "План счетов";
            this.планСчетовToolStripMenuItem.Click += new System.EventHandler(this.планСчетовToolStripMenuItem_Click);
            // 
            // подразделенияToolStripMenuItem
            // 
            this.подразделенияToolStripMenuItem.Name = "подразделенияToolStripMenuItem";
            this.подразделенияToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
            this.подразделенияToolStripMenuItem.Text = "Склад";
            this.подразделенияToolStripMenuItem.Click += new System.EventHandler(this.подразделенияToolStripMenuItem_Click);
            // 
            // материальноответственныеЛицаToolStripMenuItem
            // 
            this.материальноответственныеЛицаToolStripMenuItem.Name = "материальноответственныеЛицаToolStripMenuItem";
            this.материальноответственныеЛицаToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
            this.материальноответственныеЛицаToolStripMenuItem.Text = "Материально-ответственные лица";
            this.материальноответственныеЛицаToolStripMenuItem.Click += new System.EventHandler(this.материальноответственныеЛицаToolStripMenuItem_Click);
            // 
            // материалToolStripMenuItem
            // 
            this.материалToolStripMenuItem.Name = "материалToolStripMenuItem";
            this.материалToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
            this.материалToolStripMenuItem.Text = "Материал";
            this.материалToolStripMenuItem.Click += new System.EventHandler(this.материалToolStripMenuItem_Click);
            // 
            // покупательToolStripMenuItem
            // 
            this.покупательToolStripMenuItem.Name = "покупательToolStripMenuItem";
            this.покупательToolStripMenuItem.Size = new System.Drawing.Size(334, 26);
            this.покупательToolStripMenuItem.Text = "Покупатель";
            this.покупательToolStripMenuItem.Click += new System.EventHandler(this.покупательToolStripMenuItem_Click);
            // 
            // журналОперацийToolStripMenuItem
            // 
            this.журналОперацийToolStripMenuItem.Name = "журналОперацийToolStripMenuItem";
            this.журналОперацийToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.журналОперацийToolStripMenuItem.Text = "Журнал операций";
            this.журналОперацийToolStripMenuItem.Click += new System.EventHandler(this.журналОперацийToolStripMenuItem_Click);
            // 
            // журналПроводокToolStripMenuItem
            // 
            this.журналПроводокToolStripMenuItem.Name = "журналПроводокToolStripMenuItem";
            this.журналПроводокToolStripMenuItem.Size = new System.Drawing.Size(149, 24);
            this.журналПроводокToolStripMenuItem.Text = "Журнал проводок";
            this.журналПроводокToolStripMenuItem.Click += new System.EventHandler(this.журналПроводокToolStripMenuItem_Click);
            // 
            // отчетToolStripMenuItem
            // 
            this.отчетToolStripMenuItem.Name = "отчетToolStripMenuItem";
            this.отчетToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.отчетToolStripMenuItem.Text = "Отчет";
            this.отчетToolStripMenuItem.Click += new System.EventHandler(this.отчетToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 356);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Реализация материалов на сторону";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem журналПроводокToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem планСчетовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem подразделенияToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem материальноответственныеЛицаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem материалToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem покупательToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem отчетToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem журналОперацийToolStripMenuItem;
    }
}

