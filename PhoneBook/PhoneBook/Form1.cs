using PhoneBook.Model;
using PhoneBook.Service;
using System;
using System.Windows.Forms;

namespace PhoneBook
{
	public partial class Form1 : Form
	{
		private AbonentList book = new AbonentList();
		private Abonent editingAbonent = null; 

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			UpdateList();
		}

		private void UpdateList()
		{
			listBoxAbonents.DataSource = null;
			listBoxAbonents.DataSource = book.GetAll();
			listBoxAbonents.DisplayMember = "Display";
		}
		private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FormHelp helpForm = new FormHelp();
			helpForm.ShowDialog(); 
		}


		private void buttonAdd_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxPhone.Text))
			{
				MessageBox.Show("Заполните имя и номер", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}

			book.Add(new Abonent(textBoxName.Text, textBoxPhone.Text));
			UpdateList();
			textBoxName.Clear();
			textBoxPhone.Clear();
			textBoxName.Focus();
		}

		private void listBoxAbonents_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listBoxAbonents.SelectedItem == null) return;

			var selected = (Abonent)listBoxAbonents.SelectedItem;
			textBoxName.Text = selected.Name;
			textBoxPhone.Text = selected.Phone;
		}

		private void buttonEdit_Click(object sender, EventArgs e)
		{
			if (editingAbonent == null)
			{
				if (listBoxAbonents.SelectedItem == null)
				{
					MessageBox.Show("Выберите запись для редактирования");
					return;
				}

				editingAbonent = (Abonent)listBoxAbonents.SelectedItem;
				textBoxName.Text = editingAbonent.Name;
				textBoxPhone.Text = editingAbonent.Phone;
				MessageBox.Show("Измените поля и снова нажмите Изменить для сохранения");
			}
			else
			{
				if (string.IsNullOrWhiteSpace(textBoxName.Text) || string.IsNullOrWhiteSpace(textBoxPhone.Text))
				{
					MessageBox.Show("Заполните поля");
					return;
				}

				editingAbonent.Name = textBoxName.Text;
				editingAbonent.Phone = textBoxPhone.Text;

				editingAbonent = null; 
				UpdateList();
				textBoxName.Clear();
				textBoxPhone.Clear();
			}
		}

		private void buttonDelete_Click(object sender, EventArgs e)
		{
			if (listBoxAbonents.SelectedItem == null) return;

			var old = (Abonent)listBoxAbonents.SelectedItem;
			book.Remove(old.Name);
			UpdateList();
		}

		private void buttonClear_Click(object sender, EventArgs e)
		{
			book.Clear();
			UpdateList();
		}

		private void buttonFind_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(textBoxName.Text))
			{
				MessageBox.Show("Введите имя для поиска");
				return;
			}

			var found = book.Find(textBoxName.Text);
			if (found == null)
				MessageBox.Show("Абонент не найден");
			else
				MessageBox.Show($"{found.Name} : {found.Phone}", "Найдено");
		}

		private void buttonCreate_Click(object sender, EventArgs e)
		{
			textBoxName.Clear();
			textBoxPhone.Clear();
			editingAbonent = null; 
			textBoxName.Focus();
		}

		private void buttonSave_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog sfd = new SaveFileDialog())
			{
				sfd.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
				sfd.Title = "Сохранить телефонную книгу";

				if (sfd.ShowDialog() == DialogResult.OK)
				{
					try
					{
						FileManager.Save(sfd.FileName, book);
						MessageBox.Show("Телефонная книга сохранена!", "Сохранено", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					catch (Exception ex)
					{
						MessageBox.Show("Ошибка при сохранении: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
		}


	}
}

