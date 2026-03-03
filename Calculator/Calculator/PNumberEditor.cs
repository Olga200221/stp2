using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
	public sealed class PNumberEditor : AEditor
	{
		private string number;

		private TNumber _notation;
		private TNumber _precision;

		public TNumber Notation
		{
			get => _notation ?? new TNumber(10);
			set
			{
				if (value == null || value.Number < 2 || value.Number > 16)
				{
					return;
				}

				// Первая инициализация
				if (_notation == null)
				{
					_notation = value;
					return;
				}

				// Изменение основания → конвертируем число
				bool isChanging = _notation.Number != value.Number;
				if (isChanging)
				{
					try
					{
						// Текущее число в старом основании → в десятичное
						double decVal = TPNumber.Conver_p_10.dval(number, (int)_notation.Number);
						// Меняем основание
						_notation = value;
						// Переводим в новое основание с текущей точностью
						number = TPNumber.Conver_10_p.Do(decVal, (int)_notation.Number, (int)Precision.Number);
					}
					catch
					{
						// При ошибке оставляем как есть или сбрасываем
						number = "0";
					}
				}
				// Если основание не поменялось — ничего не делаем
			}
		}

		public TNumber Precision
		{
			get => _precision ?? new TNumber(5);
			set
			{
				if (value == null || value.Number < 0 || value.Number > 10)
				{
					return;
				}

				// Первая инициализация
				if (_precision == null)
				{
					_precision = value;
					return;
				}

				// Изменение точности → переформатируем
				bool isChanging = _precision.Number != value.Number;
				if (isChanging)
				{
					try
					{
						double decVal = TPNumber.Conver_p_10.dval(number, (int)Notation.Number);
						_precision = value;
						number = TPNumber.Conver_10_p.Do(decVal, (int)Notation.Number, (int)_precision.Number);
					}
					catch
					{
						number = "0";
					}
				}
			}
		}

		const int LeftSideOnlyLimit = 12;
		const int BothSideLimit = 22;

		Regex ZeroPNumber = new Regex("^-?(0+|.?0+|0+.(0+)?)$");
		const string Separator = ".";

		public PNumberEditor()
		{
			number = "0";
			// Инициализация через свойства — безопасно
			Notation = new TNumber(10);
			Precision = new TNumber(5);
		}

		public PNumberEditor(string str, TNumber not, TNumber pre)
		{
			number = "0"; // дефолт на случай ошибки

			if (not == null || pre == null || not.Number < 2 || not.Number > 16 || pre.Number < 0 || pre.Number > 10)
			{
				Notation = new TNumber(10);
				Precision = new TNumber(5);
			}
			else
			{
				Notation = not;
				Precision = pre;
				try
				{
					number = new TPNumber(str, Notation, Precision).ToString();
				}
				catch
				{
					number = "0";
				}
			}
		}

		public PNumberEditor(double num, TNumber not, TNumber pre)
		{
			number = "0";

			if (not == null || pre == null || not.Number < 2 || not.Number > 16 || pre.Number < 0 || pre.Number > 10)
			{
				Notation = new TNumber(10);
				Precision = new TNumber(5);
			}
			else
			{
				Notation = not;
				Precision = pre;
				try
				{
					number = new TPNumber(num, Convert.ToInt32(Notation.Number), Convert.ToInt32(Precision.Number)).ToString();
				}
				catch
				{
					number = "0";
				}
			}
		}

		public PNumberEditor(double num, int not, int pre)
		{
			number = "0";

			if (not < 2 || not > 16 || pre < 0 || pre > 10)
			{
				Notation = new TNumber(10);
				Precision = new TNumber(5);
			}
			else
			{
				Notation = new TNumber(not);
				Precision = new TNumber(pre);
				try
				{
					number = TPNumber.Conver_10_p.Do(num, not, pre);
				}
				catch
				{
					number = "0";
				}
			}
		}

		public PNumberEditor(string str)
		{
			number = "0";
			Notation = new TNumber(10);
			Precision = new TNumber(5);
			try
			{
				number = new TPNumber(str, Notation, Precision).ToString();
			}
			catch
			{
				number = "0";
			}
		}

		public override string Number
		{
			get => number;
			set
			{
				try
				{
					number = new TPNumber(value, Notation, Precision).ToString();
				}
				catch
				{
					number = "0";
				}
			}
		}

		public override bool IsZero()
		{
			return ZeroPNumber.IsMatch(number);
		}

		public override string ToogleSign()
		{
			if (string.IsNullOrEmpty(number))
			{
				number = "0";
				return number;
			}

			if (number[0] == '-')
				number = number.Substring(1);
			else
				number = "-" + number;
			return number;
		}

		public override string AddNumber(int num)
		{
			if (!HaveSeparator() && number.Length > LeftSideOnlyLimit)
				return number;
			if (number.Length > BothSideLimit)
				return number;

			if (num < 0 || num >= Notation.Number)
				return number;

			string digit = TPNumber.Conver_10_p.int_to_Char(num).ToString();

			if (num == 0)
			{
				AddZero();
			}
			else if (number == "0" || number == "-0")
			{
				number = number.StartsWith("-") ? "-" + digit : digit;
			}
			else
			{
				number += digit;
			}
			return number;
		}

		public override string AddZero()
		{
			if (HaveSeparator() && number.EndsWith(Separator))
				return number;
			if (number == "0" || number == "0.")
				return number;
			number += "0";
			return number;
		}

		public override string RemoveSymbol()
		{
			if (string.IsNullOrEmpty(number) || number == "0")
				return "0";

			if (number.Length == 1)
				number = "0";
			else if (number.Length == 2 && number.StartsWith("-"))
				number = "-0";
			else
				number = number.Substring(0, number.Length - 1);

			return number;
		}

		public override string Clear()
		{
			number = "0";
			return number;
		}

		public override string Edit(Enum com)
		{
			switch (com)
			{
				case Command.cZero: AddZero(); break;
				case Command.cOne: AddNumber(1); break;
				case Command.cTwo: AddNumber(2); break;
				case Command.cThree: AddNumber(3); break;
				case Command.cFour: AddNumber(4); break;
				case Command.cFive: AddNumber(5); break;
				case Command.cSix: AddNumber(6); break;
				case Command.cSeven: AddNumber(7); break;
				case Command.cEight: AddNumber(8); break;
				case Command.cNine: AddNumber(9); break;
				case Command.cA: AddNumber(10); break;
				case Command.cB: AddNumber(11); break;
				case Command.cC: AddNumber(12); break;
				case Command.cD: AddNumber(13); break;
				case Command.cE: AddNumber(14); break;
				case Command.cF: AddNumber(15); break;
				case Command.cSign: ToogleSign(); break;
				case Command.cSeparator: AddSeparator(); break;
				case Command.cBS: RemoveSymbol(); break;
				case Command.CE: Clear(); break;
				default: break;
			}
			return Number;
		}

		public override string AddSeparator()
		{
			if (!number.Contains(Separator))
				number += Separator;
			return number;
		}

		public override bool HaveSeparator()
		{
			return number.Contains(Separator);
		}

		public override string ToString()
		{
			return number;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is PNumberEditor editor)) return false;

			return number == editor.number &&
				   EqualityComparer<TNumber>.Default.Equals(Notation, editor.Notation) &&
				   EqualityComparer<TNumber>.Default.Equals(Precision, editor.Precision);
		}
	}
}