namespace PhoneBook
{
    partial class Form1
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
			this.buttonClear = new System.Windows.Forms.Button();
			this.buttonSave = new System.Windows.Forms.Button();
			this.buttonDelete = new System.Windows.Forms.Button();
			this.buttonEdit = new System.Windows.Forms.Button();
			this.buttonCreate = new System.Windows.Forms.Button();
			this.labelName = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.labelPhone = new System.Windows.Forms.Label();
			this.textBoxPhone = new System.Windows.Forms.TextBox();
			this.buttonAdd = new System.Windows.Forms.Button();
			this.buttonFind = new System.Windows.Forms.Button();
			this.listBoxAbonents = new System.Windows.Forms.ListBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.справкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// buttonClear
			// 
			this.buttonClear.Location = new System.Drawing.Point(639, 12);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(128, 34);
			this.buttonClear.TabIndex = 1;
			this.buttonClear.Text = "Очистить";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// buttonSave
			// 
			this.buttonSave.Location = new System.Drawing.Point(639, 92);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(128, 34);
			this.buttonSave.TabIndex = 2;
			this.buttonSave.Text = "Сохранить";
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
			// 
			// buttonDelete
			// 
			this.buttonDelete.Location = new System.Drawing.Point(639, 174);
			this.buttonDelete.Name = "buttonDelete";
			this.buttonDelete.Size = new System.Drawing.Size(128, 34);
			this.buttonDelete.TabIndex = 3;
			this.buttonDelete.Text = "Удалить";
			this.buttonDelete.UseVisualStyleBackColor = true;
			this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
			// 
			// buttonEdit
			// 
			this.buttonEdit.Location = new System.Drawing.Point(639, 253);
			this.buttonEdit.Name = "buttonEdit";
			this.buttonEdit.Size = new System.Drawing.Size(128, 34);
			this.buttonEdit.TabIndex = 4;
			this.buttonEdit.Text = "Изменить";
			this.buttonEdit.UseVisualStyleBackColor = true;
			this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
			// 
			// buttonCreate
			// 
			this.buttonCreate.Location = new System.Drawing.Point(12, 302);
			this.buttonCreate.Name = "buttonCreate";
			this.buttonCreate.Size = new System.Drawing.Size(128, 34);
			this.buttonCreate.TabIndex = 5;
			this.buttonCreate.Text = "Создать";
			this.buttonCreate.UseVisualStyleBackColor = true;
			this.buttonCreate.Click += new System.EventHandler(this.buttonCreate_Click);
			// 
			// labelName
			// 
			this.labelName.AutoSize = true;
			this.labelName.Location = new System.Drawing.Point(12, 364);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(34, 13);
			this.labelName.TabIndex = 6;
			this.labelName.Text = "ФИО";
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(15, 390);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(286, 20);
			this.textBoxName.TabIndex = 7;
			// 
			// labelPhone
			// 
			this.labelPhone.AutoSize = true;
			this.labelPhone.Location = new System.Drawing.Point(346, 364);
			this.labelPhone.Name = "labelPhone";
			this.labelPhone.Size = new System.Drawing.Size(41, 13);
			this.labelPhone.TabIndex = 8;
			this.labelPhone.Text = "Номер";
			// 
			// textBoxPhone
			// 
			this.textBoxPhone.Location = new System.Drawing.Point(334, 390);
			this.textBoxPhone.Name = "textBoxPhone";
			this.textBoxPhone.Size = new System.Drawing.Size(286, 20);
			this.textBoxPhone.TabIndex = 9;
			// 
			// buttonAdd
			// 
			this.buttonAdd.Location = new System.Drawing.Point(639, 382);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new System.Drawing.Size(128, 34);
			this.buttonAdd.TabIndex = 10;
			this.buttonAdd.Text = "Добавить";
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
			// 
			// buttonFind
			// 
			this.buttonFind.Location = new System.Drawing.Point(639, 453);
			this.buttonFind.Name = "buttonFind";
			this.buttonFind.Size = new System.Drawing.Size(128, 34);
			this.buttonFind.TabIndex = 11;
			this.buttonFind.Text = "Найти";
			this.buttonFind.UseVisualStyleBackColor = true;
			this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
			// 
			// listBoxAbonents
			// 
			this.listBoxAbonents.FormattingEnabled = true;
			this.listBoxAbonents.Location = new System.Drawing.Point(12, 23);
			this.listBoxAbonents.Name = "listBoxAbonents";
			this.listBoxAbonents.Size = new System.Drawing.Size(592, 264);
			this.listBoxAbonents.TabIndex = 12;
			this.listBoxAbonents.Click += new System.EventHandler(this.listBoxAbonents_SelectedIndexChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справкаToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(800, 24);
			this.menuStrip1.TabIndex = 13;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// справкаToolStripMenuItem
			// 
			this.справкаToolStripMenuItem.Name = "справкаToolStripMenuItem";
			this.справкаToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
			this.справкаToolStripMenuItem.Text = "Справка";
			this.справкаToolStripMenuItem.Click += new System.EventHandler(this.справкаToolStripMenuItem_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 555);
			this.Controls.Add(this.listBoxAbonents);
			this.Controls.Add(this.buttonFind);
			this.Controls.Add(this.buttonAdd);
			this.Controls.Add(this.textBoxPhone);
			this.Controls.Add(this.labelPhone);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.labelName);
			this.Controls.Add(this.buttonCreate);
			this.Controls.Add(this.buttonEdit);
			this.Controls.Add(this.buttonDelete);
			this.Controls.Add(this.buttonSave);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonEdit;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelPhone;
        private System.Windows.Forms.TextBox textBoxPhone;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonFind;
        private System.Windows.Forms.ListBox listBoxAbonents;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справкаToolStripMenuItem;
    }
}

