namespace PR3
{
    partial class ClientsForm
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
            salesList = new FlowLayoutPanel();
            npgsqlCommandBuilder1 = new Npgsql.NpgsqlCommandBuilder();
            back = new Button();
            SuspendLayout();
            // 
            // salesList
            // 
            salesList.AutoScroll = true;
            salesList.BackColor = Color.FromArgb(251, 205, 203);
            salesList.Font = new Font("Times New Roman", 10.2F);
            salesList.Location = new Point(16, 16);
            salesList.Name = "salesList";
            salesList.Size = new Size(675, 288);
            salesList.TabIndex = 0;
            // 
            // npgsqlCommandBuilder1
            // 
            npgsqlCommandBuilder1.QuotePrefix = "\"";
            npgsqlCommandBuilder1.QuoteSuffix = "\"";
            // 
            // back
            // 
            back.BackColor = Color.FromArgb(2, 27, 178);
            back.Font = new Font("Times New Roman", 10.2F);
            back.ForeColor = Color.White;
            back.Location = new Point(523, 311);
            back.Name = "back";
            back.Size = new Size(168, 30);
            back.TabIndex = 7;
            back.Text = "Назад";
            back.UseVisualStyleBackColor = false;
            back.Click += back_Click;
            // 
            // ClientsForm
            // 
            AutoScaleDimensions = new SizeF(9F, 19F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(705, 350);
            Controls.Add(back);
            Controls.Add(salesList);
            Font = new Font("Times New Roman", 10.2F);
            Name = "ClientsForm";
            Text = "SalesForm";
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel salesList;
        private Npgsql.NpgsqlCommandBuilder npgsqlCommandBuilder1;
        private Button back;
    }
}