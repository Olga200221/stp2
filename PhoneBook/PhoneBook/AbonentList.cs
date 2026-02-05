using System;
using System.Collections.Generic;
using System.Linq;

namespace PhoneBook.Model
{
	public class AbonentList
	{
		private List<Abonent> list = new List<Abonent>();

		public void Add(Abonent a)
		{
			list.Add(a);
			Sort();
		}

		public void Remove(string name)
		{
			var a = list.Find(x => x.Name == name);
			if (a != null) list.Remove(a);
		}

		public void Clear()
		{
			list.Clear();
		}

		public Abonent Find(string name)
		{
			return list.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
		}

		public List<Abonent> GetAll()
		{
			return list; 
		}

		private void Sort()
		{
			list.Sort((a, b) => a.Name.CompareTo(b.Name));
		}
	}
}
