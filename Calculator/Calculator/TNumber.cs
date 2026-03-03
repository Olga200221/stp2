using System;
using System.Globalization;

namespace Calculator
{
	public sealed class TNumber : ANumber
	{
		public double Number { get; }

		public TNumber()
		{
			Number = 0;
		}

		public TNumber(double n)
		{
			Number = n;
		}

		public TNumber(string str)
		{
			if (double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out double newNumber))
				Number = newNumber;
			else
				Number = 0;
		}

		public TNumber(TNumber num)
		{
			Number = num.Number;
		}

		public override ANumber Add(ANumber a)
		{
			return new TNumber(Number + (a as TNumber).Number);
		}

		public override ANumber Div(ANumber a)
		{
			if ((a as TNumber).IsZero())
				throw new DivideByZeroException();
			return new TNumber(Number / (a as TNumber).Number);
		}

		public override ANumber Sub(ANumber a)
		{
			return new TNumber(Number - (a as TNumber).Number);
		}

		public override ANumber Mul(ANumber a)
		{
			return new TNumber(Number * (a as TNumber).Number);
		}

		public override bool IsZero()
		{
			return Number == 0;
		}

		public override object Reverse()
		{
			if (Number == 0)
				throw new DivideByZeroException();
			return new TNumber(1 / Number);
		}

		public override object Square()
		{
			return new TNumber(Number * Number);
		}

		public override string ToString()
		{
			return Number.ToString("0.###", CultureInfo.InvariantCulture);
		}

		public override void SetString(string str)
		{
			// В данной реализации поле только для чтения, поэтому метод пустой
			// Если нужно менять значение — сделайте Number settable
		}

		// ───────────────────────────────────────────────
		// Операторы сравнения — с защитой от null
		// ───────────────────────────────────────────────

		public static bool operator ==(TNumber a, TNumber b)
		{
			// Оба null → true
			if (ReferenceEquals(a, null))
				return ReferenceEquals(b, null);

			// a не null → делегируем в Equals
			return a.Equals(b);
		}

		public static bool operator !=(TNumber a, TNumber b)
		{
			return !(a == b);
		}

		// Для сравнения с int (оставляем как было, но добавляем защиту)
		public static bool operator ==(TNumber a, int b)
		{
			if (a is null) return false;
			return a.Number == b;
		}

		public static bool operator !=(TNumber a, int b)
		{
			return !(a == b);
		}

		public static bool operator >(TNumber a, TNumber b)
		{
			if (a is null || b is null) return false;
			return a.Number > b.Number;
		}

		public static bool operator <(TNumber a, TNumber b)
		{
			if (a is null || b is null) return false;
			return a.Number < b.Number;
		}

		public static bool operator >(TNumber a, int b)
		{
			if (a is null) return false;
			return a.Number > b;
		}

		public static bool operator <(TNumber a, int b)
		{
			if (a is null) return false;
			return a.Number < b;
		}

		public static TNumber operator -(TNumber a)
		{
			return new TNumber(-a.Number);
		}

		public static TNumber operator +(TNumber a, TNumber b)
		{
			return new TNumber(a.Number + b.Number);
		}

		public static TNumber operator -(TNumber a, TNumber b)
		{
			return new TNumber(a.Number - b.Number);
		}

		public static TNumber operator *(TNumber a, TNumber b)
		{
			return new TNumber(a.Number * b.Number);
		}

		public static TNumber operator /(TNumber a, TNumber b)
		{
			if (b.IsZero())
				throw new DivideByZeroException();
			return new TNumber(a.Number / b.Number);
		}

		// ───────────────────────────────────────────────
		// Equals и GetHashCode — обязательны при переопределении ==
		// ───────────────────────────────────────────────

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj)) return true;
			if (ReferenceEquals(obj, null)) return false;
			if (obj.GetType() != GetType()) return false;

			return Number == ((TNumber)obj).Number;
		}

		public override int GetHashCode()
		{
			return Number.GetHashCode();
		}
	}
}