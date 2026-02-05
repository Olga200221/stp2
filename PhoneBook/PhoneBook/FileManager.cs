using System.IO;
using PhoneBook.Model;

namespace PhoneBook.Service
{
	public class FileManager
	{
		public static void Save(string path, AbonentList list)
		{
			using (StreamWriter sw = new StreamWriter(path))
			{
				foreach (var item in list.GetAll())
				{
					sw.WriteLine($"{item.Name}|{item.Phone}");
				}
			}
		}

		public static AbonentList Load(string path)
		{
			AbonentList list = new AbonentList();

			if (!File.Exists(path))
				return list;

			using (StreamReader sr = new StreamReader(path))
			{
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					var parts = line.Split('|');
					if (parts.Length == 2)
					{
						Abonent a = new Abonent(parts[0], parts[1]);
						list.Add(a);
					}
				}
			}

			return list;
		}
	}
}
