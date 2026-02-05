using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoneBook.Model;
using PhoneBook.Service;
using System.IO;

namespace PhoneBook.Tests
{
	[TestClass]
	public class AbonentListTests
	{
		private AbonentList list;
		private string testFile = "test_phonebook.txt";

		[TestInitialize]
		public void Setup()
		{
			list = new AbonentList();
			if (File.Exists(testFile)) File.Delete(testFile);
		}

		[TestCleanup]
		public void Cleanup()
		{
			if (File.Exists(testFile)) File.Delete(testFile);
		}

		[TestMethod] public void Add_SingleAbonent() { list.Add(new Abonent("Иван", "111")); Assert.AreEqual(1, list.GetAll().Count); }
		[TestMethod] public void Add_MultipleAbonents() { list.Add(new Abonent("Иван", "111")); list.Add(new Abonent("Петр", "222")); Assert.AreEqual(2, list.GetAll().Count); }
		[TestMethod] public void Add_EmptyName() { list.Add(new Abonent("", "333")); Assert.AreEqual(1, list.GetAll().Count); }
		[TestMethod] public void Add_EmptyPhone() { list.Add(new Abonent("Анна", "")); Assert.AreEqual(1, list.GetAll().Count); }

		[TestMethod] public void Find_Existing() { list.Add(new Abonent("Иван", "111")); var a = list.Find("Иван"); Assert.IsNotNull(a); Assert.AreEqual("111", a.Phone); }
		[TestMethod] public void Find_NotExisting() { var a = list.Find("Нет"); Assert.IsNull(a); }
		[TestMethod] public void Find_EmptyName() { list.Add(new Abonent("Иван", "111")); var a = list.Find(""); Assert.IsNull(a); }

		[TestMethod] public void Remove_Existing() { list.Add(new Abonent("Иван", "111")); list.Remove("Иван"); Assert.AreEqual(0, list.GetAll().Count); }
		[TestMethod] public void Remove_NotExisting() { list.Remove("Нет"); Assert.AreEqual(0, list.GetAll().Count); }
		[TestMethod] public void Remove_EmptyName() { list.Add(new Abonent("Иван", "111")); list.Remove(""); Assert.AreEqual(1, list.GetAll().Count); }

		[TestMethod] public void Clear_EmptyList() { list.Clear(); Assert.AreEqual(0, list.GetAll().Count); }
		[TestMethod] public void Clear_WithItems() { list.Add(new Abonent("Иван", "111")); list.Add(new Abonent("Петр", "222")); list.Clear(); Assert.AreEqual(0, list.GetAll().Count); }

		[TestMethod] public void GetAll_Empty() { Assert.AreEqual(0, list.GetAll().Count); }
		[TestMethod] public void GetAll_WithItems() { list.Add(new Abonent("Иван", "111")); list.Add(new Abonent("Петр", "222")); Assert.AreEqual(2, list.GetAll().Count); }

		[TestMethod] public void Edit_ChangeName() { var a = new Abonent("Иван", "111"); list.Add(a); a.Name = "Петр"; Assert.AreEqual("Петр", list.GetAll()[0].Name); }
		[TestMethod] public void Edit_ChangePhone() { var a = new Abonent("Иван", "111"); list.Add(a); a.Phone = "999"; Assert.AreEqual("999", list.GetAll()[0].Phone); }

		[TestMethod]
		public void FileManager_SaveAndLoad()
		{
			list.Add(new Abonent("Иван", "111"));
			FileManager.Save(testFile, list);
			var loaded = FileManager.Load(testFile);
			Assert.AreEqual(1, loaded.GetAll().Count);
			Assert.AreEqual("Иван", loaded.GetAll()[0].Name);
		}

		[TestMethod]
		public void FileManager_SaveEmptyList()
		{
			FileManager.Save(testFile, list);
			var loaded = FileManager.Load(testFile);
			Assert.AreEqual(0, loaded.GetAll().Count);
		}

		[TestMethod]
		public void FileManager_LoadNonExistingFile()
		{
			var loaded = FileManager.Load("нет_файла.txt");
			Assert.AreEqual(0, loaded.GetAll().Count);
		}


		[TestMethod] public void Find_NullName() { var a = list.Find(null); Assert.IsNull(a); }

		[TestMethod]
		public void AddRemove_Multiple()
		{
			list.Add(new Abonent("А", "1"));
			list.Add(new Abonent("Б", "2"));
			list.Add(new Abonent("В", "3"));
			list.Remove("Б");
			Assert.AreEqual(2, list.GetAll().Count);
			Assert.AreEqual("А", list.GetAll()[0].Name);
			Assert.AreEqual("В", list.GetAll()[1].Name);
		}

		[TestMethod]
		public void AddDuplicateNames()
		{
			list.Add(new Abonent("А", "1"));
			list.Add(new Abonent("А", "2"));
			Assert.AreEqual(2, list.GetAll().Count);
		}

		[TestMethod]
		public void AddManyAbonents()
		{
			for (int i = 0; i < 50; i++) list.Add(new Abonent($"Имя{i}", $"{i}"));
			Assert.AreEqual(50, list.GetAll().Count);
		}

		[TestMethod]
		public void RemoveAllAfterMany()
		{
			for (int i = 0; i < 20; i++) list.Add(new Abonent($"Имя{i}", $"{i}"));
			for (int i = 0; i < 20; i++) list.Remove($"Имя{i}");
			Assert.AreEqual(0, list.GetAll().Count);
		}

		[TestMethod]
		public void ClearAfterMany()
		{
			for (int i = 0; i < 30; i++) list.Add(new Abonent($"Имя{i}", $"{i}"));
			list.Clear();
			Assert.AreEqual(0, list.GetAll().Count);
		}

		[TestMethod]
		public void AddEditRemoveSequence()
		{
			list.Add(new Abonent("Иван", "111"));
			var a = list.Find("Иван");
			a.Phone = "222";
			list.Remove("Иван");
			Assert.AreEqual(0, list.GetAll().Count);
		}

		[TestMethod]
		public void AddFindEditFindSequence()
		{
			list.Add(new Abonent("Петр", "333"));
			var a = list.Find("Петр");
			Assert.AreEqual("333", a.Phone);
			a.Phone = "999";
			var b = list.Find("Петр");
			Assert.AreEqual("999", b.Phone);
		}

		[TestMethod]
		public void FileManager_MultipleSaveLoad()
		{
			list.Add(new Abonent("А", "1"));
			list.Add(new Abonent("Б", "2"));
			FileManager.Save(testFile, list);
			var loaded = FileManager.Load(testFile);
			Assert.AreEqual(2, loaded.GetAll().Count);
		}

		[TestMethod]
		public void FileManager_SaveLoadEmpty()
		{
			list.Clear();
			FileManager.Save(testFile, list);
			var loaded = FileManager.Load(testFile);
			Assert.AreEqual(0, loaded.GetAll().Count);
		}

		

		[TestMethod]
		public void Find_CaseExact()
		{
			list.Add(new Abonent("Иван", "1"));
			Assert.IsNotNull(list.Find("Иван"));
		}

		[TestMethod]
		public void SaveLoad_Stress()
		{
			for (int i = 0; i < 50; i++) list.Add(new Abonent($"Имя{i}", $"{i}"));
			FileManager.Save(testFile, list);
			var loaded = FileManager.Load(testFile);
			Assert.AreEqual(50, loaded.GetAll().Count);
		}
	}
}
