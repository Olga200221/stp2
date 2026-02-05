namespace PhoneBook
{
    partial class FormHelp
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
			this.labelTitle = new System.Windows.Forms.Label();
			this.textBoxInfo = new System.Windows.Forms.TextBox();
			this.buttonClose = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelTitle
			// 
			this.labelTitle.AutoSize = true;
			this.labelTitle.Location = new System.Drawing.Point(101, 9);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new System.Drawing.Size(50, 13);
			this.labelTitle.TabIndex = 0;
			this.labelTitle.Text = "Справка";
			// 
			// textBoxInfo
			// 
			this.textBoxInfo.Location = new System.Drawing.Point(12, 42);
			this.textBoxInfo.Multiline = true;
			this.textBoxInfo.Name = "textBoxInfo";
			this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBoxInfo.Size = new System.Drawing.Size(252, 140);
			this.textBoxInfo.TabIndex = 1;
			// 
			// buttonClose
			// 
			this.buttonClose.Location = new System.Drawing.Point(93, 210);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(75, 23);
			this.buttonClose.TabIndex = 2;
			this.buttonClose.Text = "Закрыть";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// FormHelp
			// 
			this.ClientSize = new System.Drawing.Size(284, 261);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.textBoxInfo);
			this.Controls.Add(this.labelTitle);
			this.Name = "FormHelp";
			this.Load += new System.EventHandler(this.FormHelp_Load_1);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button buttonClose;
    }
}