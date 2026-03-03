using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Calculator
{
	public sealed class TFrac : ANumber
	{
		public TNumber Numerator;
		public TNumber Denominator;

		#region Current Class Things
		static void Swap<T>(ref T lhs, ref T rhs)
		{
			T temp;
			temp = lhs;
			lhs = rhs;
			rhs = temp;
		}

		public static long GCD(long a, long b)
		{
			a = Math.Abs(a);
			b = Math.Abs(b);
			while (b > 0)
			{
				a %= b;
				Swap(ref a, ref b);
			}
			return a;
		}
		#endregion

		#region Constructor
		public TFrac()
		{
			Numerator = new TNumber(0);
			Denominator = new TNumber(1);
		}

		public TFrac(TNumber a, TNumber b)
		{
			try
			{
				if (a < 0 && b < 0)
				{
					a = -a;
					b = -b;
				}
				else if (b < 0 && a > 0)
				{
					b = -b;
					a = -a;
				}
				else if ((a == 0 && b == 0) || b == 0 || (a == 0 && b == 1))
				{
					Numerator = new TNumber(0);
					Denominator = new TNumber(1);
					return;
				}

				Numerator = new TNumber(a);
				Denominator = new TNumber(b);

				if (Denominator.IsZero())
				{
					Numerator = new TNumber(0);
					Denominator = new TNumber(1);
					return;
				}

				long gcdResult = GCD((long)a.Number, (long)b.Number);
				if (gcdResult > 1)
				{
					// исправлено: вместо *= и /=
					Numerator = Numerator * new TNumber(gcdResult);
					Denominator = Denominator * new TNumber(gcdResult);
					Numerator = Numerator / new TNumber(gcdResult);
					Denominator = Denominator / new TNumber(gcdResult);
				}
			}
			catch
			{
				throw new OverflowException();
			}
		}

		public TFrac(int a, int b)
		{
			if (a < 0 && b < 0)
			{
				a = -a;
				b = -b;
			}
			else if (b < 0 && a > 0)
			{
				b = -b;
				a = -a;
			}
			else if ((a == 0 && b == 0) || b == 0 || (a == 0 && b == 1))
			{
				Numerator = new TNumber(0);
				Denominator = new TNumber(1);
				return;
			}

			Numerator = new TNumber(a);
			Denominator = new TNumber(b);

			if (Denominator.IsZero())
			{
				Numerator = new TNumber(0);
				Denominator = new TNumber(1);
				return;
			}

			long gcdResult = GCD(a, b);
			if (gcdResult > 1)
			{
				// исправлено: вместо *= и /=
				Numerator = Numerator * new TNumber(gcdResult);
				Denominator = Denominator * new TNumber(gcdResult);
				Numerator = Numerator / new TNumber(gcdResult);
				Denominator = Denominator / new TNumber(gcdResult);
			}
		}

		public TFrac(string fraction)
		{
			Regex FracRegex = new Regex(@"^-?(\d+)/(\d+)$");
			Regex NumberRegex = new Regex(@"^-?\d+/?$");

			if (FracRegex.IsMatch(fraction))
			{
				List<string> FracParts = fraction.Split('/').ToList();
				Numerator = new TNumber(FracParts[0]);
				Denominator = new TNumber(FracParts[1]);

				if (Denominator.IsZero())
				{
					Numerator = new TNumber(0);
					Denominator = new TNumber(1);
					return;
				}

				long gcdResult = GCD((long)Numerator.Number, (long)Denominator.Number);
				if (gcdResult > 1)
				{
					// исправлено: вместо *= и /=
					Numerator = Numerator * new TNumber(gcdResult);
					Denominator = Denominator * new TNumber(gcdResult);
					Numerator = Numerator / new TNumber(gcdResult);
					Denominator = Denominator / new TNumber(gcdResult);
				}
			}
			else if (NumberRegex.IsMatch(fraction))
			{
				Numerator = new TNumber(fraction);
				Denominator = new TNumber(1);
			}
			else
			{
				Numerator = new TNumber(0);
				Denominator = new TNumber(1);
			}
		}

		public TFrac(TFrac anotherFrac)
		{
			Numerator = anotherFrac.Numerator;
			Denominator = anotherFrac.Denominator;
		}
		#endregion

		#region Override operators
		public static TFrac operator +(TFrac a, TFrac b)
		{
			TFrac temp = new TFrac(
				a.Numerator * b.Denominator + a.Denominator * b.Numerator,
				a.Denominator * b.Denominator
			);
			return temp;
		}

		public static TFrac operator *(TFrac a, TFrac b)
		{
			TFrac temp = new TFrac(
				a.Numerator * b.Numerator,
				a.Denominator * b.Denominator
			);
			return temp;
		}

		public static TFrac operator -(TFrac a, TFrac b)
		{
			TFrac temp = new TFrac(
				a.Numerator * b.Denominator - a.Denominator * b.Numerator,
				a.Denominator * b.Denominator
			);
			return temp;
		}

		public static TFrac operator /(TFrac a, TFrac b)
		{
			if (b.IsZero())
				throw new Exception("Division by zero");

			TFrac temp = new TFrac(
				a.Numerator * b.Denominator,
				a.Denominator * b.Numerator
			);
			return temp;
		}

		public static TFrac operator -(TFrac a)
		{
			return new TFrac(-a.Numerator, a.Denominator);
		}

		public static bool operator ==(TFrac a, TFrac b)
		{
			return a.Numerator == b.Numerator && a.Denominator == b.Denominator;
		}

		public static bool operator !=(TFrac a, TFrac b)
		{
			return !(a == b);
		}

		public static bool operator >(TFrac a, TFrac b)
		{
			return (a.Numerator / a.Denominator) > (b.Numerator / b.Denominator);
		}

		public static bool operator <(TFrac a, TFrac b)
		{
			return (a.Numerator / a.Denominator) < (b.Numerator / b.Denominator);
		}
		#endregion

		#region Abstract Override
		public override ANumber Add(ANumber a)
		{
			TFrac other = a as TFrac;
			return new TFrac(
				Numerator * other.Denominator + Denominator * other.Numerator,
				Denominator * other.Denominator
			);
		}

		public override ANumber Mul(ANumber a)
		{
			TFrac other = a as TFrac;
			return new TFrac(
				other.Numerator * Numerator,
				other.Denominator * Denominator
			);
		}

		public override ANumber Div(ANumber a)
		{
			TFrac other = a as TFrac;
			if (other.IsZero())
				throw new DivideByZeroException();

			return new TFrac(
				other.Numerator * Denominator,
				other.Denominator * Numerator
			);
		}

		public override ANumber Sub(ANumber a)
		{
			TFrac other = a as TFrac;
			return new TFrac(
				Numerator * other.Denominator - Denominator * other.Numerator,
				Denominator * other.Denominator
			);
		}

		public override object Square()
		{
			return new TFrac(
				(TNumber)Numerator.Square(),
				(TNumber)Denominator.Square()
			);
		}

		public override object Reverse()
		{
			return new TFrac(Denominator, Numerator);
		}

		public override bool IsZero()
		{
			return Numerator.IsZero();
		}

		public override void SetString(string str)
		{
			TFrac tempFrac = new TFrac(str);
			Numerator = tempFrac.Numerator;
			Denominator = tempFrac.Denominator;
		}
		#endregion

		public override string ToString()
		{
			return Numerator.ToString() + "/" + Denominator.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj is TFrac frac)
			{
				return Numerator == frac.Numerator &&
					   Denominator == frac.Denominator;
			}
			return false;
		}
	}
}