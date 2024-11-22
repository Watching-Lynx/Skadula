namespace Skadula
{
    partial class Form2Docs
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.docsdataGridView2 = new System.Windows.Forms.DataGridView();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.surname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.docsdataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // docsdataGridView2
            // 
            this.docsdataGridView2.AllowUserToAddRows = false;
            this.docsdataGridView2.AllowUserToDeleteRows = false;
            this.docsdataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.docsdataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.docsdataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.surname,
            this.type,
            this.date});
            this.docsdataGridView2.Location = new System.Drawing.Point(0, 1);
            this.docsdataGridView2.Name = "docsdataGridView2";
            this.docsdataGridView2.ReadOnly = true;
            this.docsdataGridView2.RowHeadersVisible = false;
            this.docsdataGridView2.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.docsdataGridView2.RowTemplate.Height = 28;
            this.docsdataGridView2.Size = new System.Drawing.Size(494, 326);
            this.docsdataGridView2.TabIndex = 0;
            this.docsdataGridView2.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.docsdataGridView2_CellMouseClick);
            // 
            // id
            // 
            this.id.HeaderText = "ID";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // surname
            // 
            this.surname.HeaderText = "Фамилия И. О.";
            this.surname.Name = "surname";
            this.surname.ReadOnly = true;
            // 
            // type
            // 
            this.type.HeaderText = "Название";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            // 
            // date
            // 
            this.date.HeaderText = "Окончание";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            // 
            // Form2Docs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 328);
            this.Controls.Add(this.docsdataGridView2);
            this.Name = "Form2Docs";
            this.Text = "Сертификаты";
            this.Load += new System.EventHandler(this.Form2Docs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.docsdataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView docsdataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn surname;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
    }
}