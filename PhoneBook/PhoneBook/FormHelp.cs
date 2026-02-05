using System;
using System.Windows.Forms;

namespace PhoneBook
{
	public partial class FormHelp : Form
	{
		public FormHelp()
		{
			InitializeComponent(); 
			InitializeHelpUI();    
		}

		private void InitializeHelpUI()
		{
			textBoxInfo.Text =
				"Телефонная книга\r\n\r\n" +
				"• Добавить: Введите имя и номер, нажмите 'Добавить'.\r\n" +
				"• Изменить: Выберите запись, измените поля, нажмите 'Изменить'.\r\n" +
				"• Удалить: Выберите запись и нажмите 'Удалить'.\r\n" +
				"• Очистить: Очистить всю телефонную книгу.\r\n" +
				"• Сохранить: Сохранить телефонную книгу в файл.\r\n" +
				"• Найти: Введите имя и нажмите 'Найти'.\r\n\r\n" +
				"Авторы:Шелегина Ольга, Смолевская Елизавета\r\nГруппа: ИП-212";

		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

        private void FormHelp_Load(object sender, EventArgs e)
        {

        }

        private void FormHelp_Load_1(object sender, EventArgs e)
        {

        }
    }
}
