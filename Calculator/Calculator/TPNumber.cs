using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
	public sealed class TPNumber : ANumber
	{
		public static class Conver_10_p
		{
			public static string Do(double n, int p, int c)
			{
				if (p < 2 || p > 16)
					throw new IndexOutOfRangeException("Основание должно быть от 2 до 16");
				if (c < 0 || c > 10)
					throw new IndexOutOfRangeException("Точность должна быть от 0 до 10");

				string LeftSideString;
				string RightSideString;
				long LeftSide = (long)Math.Truncate(n);
				double RightSide = Math.Abs(n - LeftSide);

				try
				{
					LeftSideString = int_to_P(LeftSide, p);
					RightSideString = flt_to_P(RightSide, p, c);
				}
				catch
				{
					throw new OverflowException();
				}

				string sign = n < 0 ? "-" : "";
				string result = sign + LeftSideString;
				if (!string.IsNullOrEmpty(RightSideString))
					result += "." + RightSideString;

				return result;
			}

			public static char int_to_Char(long d)
			{
				if (d < 0 || d > 15)
					throw new IndexOutOfRangeException("Цифра вне диапазона 0-15");
				return "0123456789ABCDEF"[(int)d];
			}

			public static string int_to_P(long n, long p)
			{
				if (p < 2 || p > 16)
					throw new IndexOutOfRangeException();

				if (n == 0) return "0";

				bool haveMinus = n < 0;
				if (haveMinus) n = -n;

				string pNumber = string.Empty;
				while (n > 0)
				{
					pNumber += int_to_Char(n % p);
					n /= p;
				}

				if (haveMinus) pNumber += "-";

				char[] tempArray = pNumber.ToCharArray();
				Array.Reverse(tempArray);
				return new string(tempArray);
			}

			public static string flt_to_P(double n, int p, int c)
			{
				if (p < 2 || p > 16 || c < 0 || c > 10)
					throw new IndexOutOfRangeException();

				string pNumber = string.Empty;
				for (int i = 0; i < c; i++)
				{
					n *= p;
					int digit = (int)Math.Truncate(n);
					pNumber += int_to_Char(digit);
					n -= digit;
				}
				return pNumber.TrimEnd('0');
			}
		}

		public static class Conver_p_10
		{
			private static int char_To_num(char ch)
			{
				string digits = "0123456789ABCDEF";
				int index = digits.IndexOf(char.ToUpper(ch));
				if (index == -1)
					throw new IndexOutOfRangeException($"Недопустимый символ: {ch}");
				return index;
			}

			public static double dval(string p_num, int p)
			{
				if (p < 2 || p > 16)
					throw new IndexOutOfRangeException("Основание должно быть от 2 до 16");

				if (string.IsNullOrWhiteSpace(p_num))
					return 0;

				bool haveMinus = p_num.StartsWith("-");
				if (haveMinus) p_num = p_num.Substring(1);

				int dotIndex = p_num.IndexOf('.');
				string integerPart = dotIndex >= 0 ? p_num.Substring(0, dotIndex) : p_num;
				string fractionalPart = dotIndex >= 0 ? p_num.Substring(dotIndex + 1) : "";

				// Проверка всех символов
				foreach (char ch in integerPart + fractionalPart)
				{
					if (ch != '.' && char_To_num(ch) >= p)
						throw new FormatException($"Символ '{ch}' недопустим для основания {p}");
				}

				double result = 0;

				// Целая часть
				for (int i = 0; i < integerPart.Length; i++)
				{
					result = result * p + char_To_num(integerPart[i]);
				}

				// Дробная часть
				double frac = 0;
				for (int i = 0; i < fractionalPart.Length; i++)
				{
					frac = frac * p + char_To_num(fractionalPart[i]);
				}
				for (int i = 0; i < fractionalPart.Length; i++)
					frac /= p;

				result += frac;

				return haveMinus ? -result : result;
			}
		}

		public TNumber Number { get; private set; }
		public TNumber Notation { get; private set; }
		public TNumber Precision { get; private set; }

		public TPNumber()
		{
			Number = new TNumber(0);
			Notation = new TNumber(10);
			Precision = new TNumber(5);
		}

		public TPNumber(TNumber num, TNumber not, TNumber pre)
		{
			Number = num ?? new TNumber(0);
			Notation = ValidateNotation(not) ? not : new TNumber(10);
			Precision = ValidatePrecision(pre) ? pre : new TNumber(5);
		}

		public TPNumber(double num, int not, int pre)
		{
			Number = new TNumber(num);
			Notation = ValidateNotation(not) ? new TNumber(not) : new TNumber(10);
			Precision = ValidatePrecision(pre) ? new TNumber(pre) : new TNumber(5);
		}

		public TPNumber(string str, TNumber not, TNumber pre)
		{
			Notation = ValidateNotation(not) ? not : new TNumber(10);
			Precision = ValidatePrecision(pre) ? pre : new TNumber(5);

			try
			{
				Number = new TNumber(Conver_p_10.dval(str, (int)Notation.Number));
			}
			catch
			{
				Number = new TNumber(0);
				throw new OverflowException("Ошибка преобразования строки в P-число");
			}
		}

		public TPNumber(string str, int not, int pre)
		{
			Notation = ValidateNotation(not) ? new TNumber(not) : new TNumber(10);
			Precision = ValidatePrecision(pre) ? new TNumber(pre) : new TNumber(5);

			try
			{
				Number = new TNumber(Conver_p_10.dval(str, (int)Notation.Number));
			}
			catch
			{
				Number = new TNumber(0);
				throw new OverflowException("Ошибка преобразования строки в P-число");
			}
		}

		public TPNumber(TPNumber other)
		{
			Number = other?.Number ?? new TNumber(0);
			Notation = other?.Notation ?? new TNumber(10);
			Precision = other?.Precision ?? new TNumber(5);
		}

		private static bool ValidateNotation(TNumber n) => n != null && n.Number >= 2 && n.Number <= 16;
		private static bool ValidateNotation(int n) => n >= 2 && n <= 16;
		private static bool ValidatePrecision(TNumber p) => p != null && p.Number >= 0 && p.Number <= 10;
		private static bool ValidatePrecision(int p) => p >= 0 && p <= 10;

		// Операторы
		public static TPNumber operator +(TPNumber a, TPNumber b) => new TPNumber(a.Number + b.Number, a.Notation, a.Precision);
		public static TPNumber operator -(TPNumber a, TPNumber b) => new TPNumber(a.Number - b.Number, a.Notation, a.Precision);
		public static TPNumber operator *(TPNumber a, TPNumber b) => new TPNumber(a.Number * b.Number, a.Notation, a.Precision);
		public static TPNumber operator /(TPNumber a, TPNumber b) => new TPNumber(a.Number / b.Number, a.Notation, a.Precision);
		public static TPNumber operator -(TPNumber a) => new TPNumber(-a.Number, a.Notation, a.Precision);

		public static bool operator ==(TPNumber a, TPNumber b)
		{
			if (ReferenceEquals(a, b)) return true;
			if (a is null || b is null) return false;
			return a.Number == b.Number;
		}

		public static bool operator !=(TPNumber a, TPNumber b) => !(a == b);
		public static bool operator >(TPNumber a, TPNumber b) => a?.Number > b?.Number;
		public static bool operator <(TPNumber a, TPNumber b) => a?.Number < b?.Number;

		// Переопределения из ANumber
		public override ANumber Add(ANumber a) => new TPNumber((a as TPNumber)?.Number + Number, Notation, Precision);
		public override ANumber Sub(ANumber a) => new TPNumber(Number - (a as TPNumber)?.Number, Notation, Precision);
		public override ANumber Mul(ANumber a) => new TPNumber(Number * (a as TPNumber)?.Number, Notation, Precision);
		public override ANumber Div(ANumber a) => new TPNumber(Number / (a as TPNumber)?.Number, Notation, Precision);

		public override object Square() => new TPNumber((TNumber)Number.Square(), Notation, Precision);
		public override object Reverse() => new TPNumber((TNumber)Number.Reverse(), Notation, Precision);

		public override bool IsZero() => Number.IsZero();

		public override void SetString(string str)
		{
			try
			{
				Number = new TNumber(Conver_p_10.dval(str, (int)Notation.Number));
			}
			catch
			{
				Number = new TNumber(0);
			}
		}

		public override string ToString()
		{
			try
			{
				return Conver_10_p.Do(
					Number.Number,
					(int)Notation.Number,
					(int)Precision.Number
				);
			}
			catch
			{
				return "0";
			}
		}

		public override bool Equals(object obj)
		{
			if (!(obj is TPNumber other)) return false;
			return Number == other.Number &&
				   Notation == other.Notation &&
				   Precision == other.Precision;
		}
	}
}