public class Abonent
{
	public string Name { get; set; }
	public string Phone { get; set; }

	public Abonent(string name, string phone)
	{
		Name = name;
		Phone = phone;
	}

	public string Display => $"{Name} : {Phone}";

	public override string ToString() => Display;
}
