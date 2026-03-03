using System;

namespace Calculator
{
	public sealed class TCtrl<T, Editor>
		where T : ANumber, new()
		where Editor : AEditor, new()
	{
		public enum TCtrlState
		{
			cStart, cEditing, FunDone, cOperDone, cExpDone, cOpChange, cError
		}

		Editor edit;
		TProc<T> proc;
		TMemory<T> memory;
		TCtrlState curState;

		public TCtrlState CurState { get => curState; set => curState = value; }
		public TProc<T> Proc { get => proc; set => proc = value; }
		public TMemory<T> Memory { get => memory; set => memory = value; }
		public Editor Edit { get => edit; set => edit = value; }

		public TCtrl()
		{
			Edit = new Editor();
			Proc = new TProc<T>();
			Memory = new TMemory<T>();
			curState = TCtrlState.cStart;
		}

		public string ExecCommandEditor(AEditor.Command command)
		{
			string ToReturn;

			if (CurState == TCtrlState.cExpDone)
			{
				Proc.Reset();
				CurState = TCtrlState.cStart;
			}

			if (CurState != TCtrlState.cStart)
				CurState = TCtrlState.cEditing;

			ToReturn = Edit.Edit(command);

			T TempObj = new T();

			// Специальная обработка только для TPNumber
			if (typeof(T) == typeof(TPNumber) && Edit is PNumberEditor pEditor)
			{
				// Создаём новый экземпляр TPNumber с правильным основанием и точностью
				var newPNumber = new TPNumber(
					ToReturn,                    // текущая строка из редактора
					pEditor.Notation,            // текущее основание
					pEditor.Precision            // текущая точность
				);

				// Приводим к типу T (это безопасно, т.к. мы уже проверили typeof(T))
				TempObj = (T)(ANumber)newPNumber;
			}
			else
			{
				// Для всех остальных типов просто устанавливаем строку
				TempObj.SetString(ToReturn);
			}

			Proc.Rop = TempObj;
			return ToReturn;
		}

		public string ExecOperation(TProc<T>.Oper oper)
		{
			if (oper == TProc<T>.Oper.None)
				return Edit.Number;

			string ToReturn;
			try
			{
				switch (CurState)
				{
					case TCtrlState.cStart:
						Proc.Lop_Res = Proc.Rop;
						Proc.Operation = oper;
						CurState = TCtrlState.cOperDone;
						Edit.Clear();
						break;

					case TCtrlState.cEditing:
						Proc.DoOper();
						Proc.Operation = oper;
						Edit.Clear();
						CurState = TCtrlState.cOperDone;
						break;

					case TCtrlState.FunDone:
						if (Proc.Operation == TProc<T>.Oper.None)
							Proc.Lop_Res = Proc.Rop;
						else
							Proc.DoOper();
						Proc.Operation = oper;
						Edit.Clear();
						CurState = TCtrlState.cOpChange;
						break;

					case TCtrlState.cOperDone:
						CurState = TCtrlState.cOpChange;
						Edit.Clear();
						break;

					case TCtrlState.cExpDone:
						Proc.Operation = oper;
						Proc.Rop = Proc.Lop_Res;
						CurState = TCtrlState.cOpChange;
						Edit.Clear();
						break;

					case TCtrlState.cError:
						Proc.Reset();
						return "ERR";

					case TCtrlState.cOpChange:
						Proc.Operation = oper;
						Edit.Clear();
						break;

					default:
						break;
				}

				ToReturn = Proc.Lop_Res.ToString();
			}
			catch
			{
				Reset();
				return "ERROR";
			}

			return ToReturn;
		}

		public string ExecFunction(TProc<T>.Func func)
		{
			string ToReturn;
			try
			{
				if (CurState == TCtrlState.cExpDone)
				{
					Proc.Rop = Proc.Lop_Res;
					Proc.Operation = TProc<T>.Oper.None;
				}

				Proc.DoFunc(func);
				CurState = TCtrlState.FunDone;
				ToReturn = Proc.Rop.ToString();
			}
			catch
			{
				Reset();
				return "ERROR";
			}

			return ToReturn;
		}

		public string Calculate()
		{
			string ToReturn;
			try
			{
				if (CurState == TCtrlState.cStart)
					Proc.Lop_Res = Proc.Rop;

				Proc.DoOper();
				CurState = TCtrlState.cExpDone;
				ToReturn = Proc.Lop_Res.ToString();
			}
			catch
			{
				Reset();
				return "ERROR";
			}

			return ToReturn;
		}

		public string Reset()
		{
			Edit.Clear();
			Proc.Reset();
			Memory.Clear();
			curState = TCtrlState.cStart;
			return Edit.ToString();
		}

		public (T, TMemory<T>.NumStates) ExecCommandMemory(TMemory<T>.Commands command, string str)
		{
			T TempObj = new T();
			TempObj.SetString(str);

			(T, TMemory<T>.NumStates) obj = (default, TMemory<T>.NumStates.OFF);

			try
			{
				obj = Memory.Edit(command, TempObj);
			}
			catch
			{
				Reset();
				return obj;
			}

			if (command == TMemory<T>.Commands.Copy)
			{
				Edit.Number = obj.Item1.ToString();
				Proc.Rop = obj.Item1;
			}

			return obj;
		}
	}
}