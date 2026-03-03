using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Calculator
{
	public partial class Form1 : Form
	{
		TCtrl<TFrac, FracEditor> fracController;
		TCtrl<TPNumber, PNumberEditor> pNumberController;
		TCtrl<TComplex, ComplexEditor> complexController;

		const string TAG_FRAC = "FRAC_";
		const string TAG_COMPLEX = "COMPLEX_";
		const string TAG_PNUMBER = "PNUMBER_";

		const string OPERATIONS = "+-/*";

		bool PNumberMode = true;
		bool FracMode = true;
		bool ComplexMode = true;

		enum ComplexFunctions
		{
			Pwr, Root, Abs, Dgr, Rad
		}

		// ======== История вычислений ========
		private List<TMemory<TFrac>> fracHistory = new List<TMemory<TFrac>>();
		private List<TMemory<TComplex>> complexHistory = new List<TMemory<TComplex>>();
		private List<TMemory<TPNumber>> pNumberHistory = new List<TMemory<TPNumber>>();
		// =====================================

		private string NumberBeatifier(string Tag, string str)
		{
			if (str == "ERROR")
				return str;

			string ToReturn = str;
			switch (Tag)
			{
				case TAG_PNUMBER:
					break;
				case TAG_FRAC:
					if (FracMode == true)
						ToReturn = str;
					else if (new TFrac(str).Denominator == 1)
						ToReturn = new TFrac(str).Numerator.ToString();
					break;
				case TAG_COMPLEX:
					if (ComplexMode == true)
						ToReturn = str;
					else if (new TComplex(str).Imaginary == 0)
						ToReturn = new TComplex(str).Real.ToString();
					break;
			}
			return ToReturn;
		}

		private static AEditor.Command CharToEditorCommand(char ch)
		{
			AEditor.Command command = AEditor.Command.cNone;
			switch (ch)
			{
				case '0': command = AEditor.Command.cZero; break;
				case '1': command = AEditor.Command.cOne; break;
				case '2': command = AEditor.Command.cTwo; break;
				case '3': command = AEditor.Command.cThree; break;
				case '4': command = AEditor.Command.cFour; break;
				case '5': command = AEditor.Command.cFive; break;
				case '6': command = AEditor.Command.cSix; break;
				case '7': command = AEditor.Command.cSeven; break;
				case '8': command = AEditor.Command.cEight; break;
				case '9': command = AEditor.Command.cNine; break;
				case 'A': command = AEditor.Command.cA; break;
				case 'B': command = AEditor.Command.cB; break;
				case 'C': command = AEditor.Command.cC; break;
				case 'D': command = AEditor.Command.cD; break;
				case 'E': command = AEditor.Command.cE; break;
				case 'F': command = AEditor.Command.cF; break;
				case '.': command = AEditor.Command.cSeparator; break;
				case '-': command = AEditor.Command.cSign; break;
			}
			return command;
		}

		private static TProc<T>.Oper CharToOperationsCommand<T>(char ch) where T : ANumber, new()
		{
			TProc<T>.Oper command = TProc<T>.Oper.None;
			switch (ch)
			{
				case '+': command = TProc<T>.Oper.Add; break;
				case '-': command = TProc<T>.Oper.Sub; break;
				case '*': command = TProc<T>.Oper.Mul; break;
				case '/': command = TProc<T>.Oper.Div; break;
			}
			return command;
		}

		private static AEditor.Command KeyCodeToEditorCommand(Keys ch)
		{
			AEditor.Command command = AEditor.Command.cNone;
			switch (ch)
			{
				case Keys.Back: command = AEditor.Command.cBS; break;
				case Keys.Delete:
				case Keys.Escape: command = AEditor.Command.CE; break;
			}
			return command;
		}

		public Form1()
		{
			fracController = new TCtrl<TFrac, FracEditor>();
			pNumberController = new TCtrl<TPNumber, PNumberEditor>();
			complexController = new TCtrl<TComplex, ComplexEditor>();

			InitializeComponent();

			Size = new System.Drawing.Size(310, 382);

			// ====== подключаем обработчик кнопки истории ======
			историяToolStripMenuItem.Click += историяToolStripMenuItem_Click;

			// ====== подключаем остальные обработчики ======
			tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
			trackBar_PNumber.ValueChanged += TrackBar_PNumber_ValueChanged;
			KeyPress += Form1_KeyPress;
			KeyDown += Form1_KeyDown;

			дробьFracTSMI.Click += дробьFracTSMI_Click;
			числоFracTSMI.Click += числоFracTSMI_Click;
			комплексноеComplexTSMI.Click += комплексноеComplexTSMI_Click;
			действительноеComplexTSMI.Click += действительноеComplexTSMI_Click;

			// ← Инициализация доступности кнопок при запуске
			UpdatePNumberDigitButtons();
		}

		// Новый метод — управление доступностью кнопок 0–9, A–F
		private void UpdatePNumberDigitButtons()
		{
			int currentBase = trackBar_PNumber.Value;

			// Цифры 0–7 всегда доступны (для оснований ≥2)
			b_PNumber_0.Enabled = true;
			b_PNumber_1.Enabled = true;
			b_PNumber_2.Enabled = true;
			b_PNumber_3.Enabled = true;
			b_PNumber_4.Enabled = true;
			b_PNumber_5.Enabled = true;
			b_PNumber_6.Enabled = true;
			b_PNumber_7.Enabled = true;

			// 8 и 9 — только если основание > 8 и > 9
			b_PNumber_8.Enabled = currentBase > 8;
			b_PNumber_9.Enabled = currentBase > 9;

			// Шестнадцатеричные буквы A–F
			b_PNumber_A.Enabled = currentBase > 10;
			b_PNumber_B.Enabled = currentBase > 11;
			b_PNumber_C.Enabled = currentBase > 12;
			b_PNumber_D.Enabled = currentBase > 13;
			b_PNumber_E.Enabled = currentBase > 14;
			b_PNumber_F.Enabled = currentBase > 15;
		}

		private void Button_Number_Edit(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string FullTag = button.Tag.ToString();

			if (FullTag.StartsWith(TAG_FRAC))
			{
				Enum.TryParse(FullTag.Replace(TAG_FRAC, string.Empty), out AEditor.Command ParsedEnum);
				tB_Frac.Text = fracController.ExecCommandEditor(ParsedEnum);
			}
			else if (FullTag.StartsWith(TAG_COMPLEX))
			{
				Enum.TryParse(FullTag.Replace(TAG_COMPLEX, string.Empty), out AEditor.Command ParsedEnum);
				tB_Complex.Text = complexController.ExecCommandEditor(ParsedEnum);
			}
			else if (FullTag.StartsWith(TAG_PNUMBER))
			{
				pNumberController.Edit.Notation = new TNumber(trackBar_PNumber.Value);
				Enum.TryParse(FullTag.Replace(TAG_PNUMBER, string.Empty), out AEditor.Command ParsedEnum);
				tB_PNumber.Text = pNumberController.ExecCommandEditor(ParsedEnum);
			}
		}

		private void Button_Number_Operation(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string FullTag = button.Tag.ToString();

			if (FullTag.StartsWith(TAG_FRAC))
			{
				string Command = FullTag.Replace(TAG_FRAC, string.Empty);
				Enum.TryParse(Command, out TProc<TFrac>.Oper ParsedEnum);
				tB_Frac.Text = NumberBeatifier(TAG_FRAC, fracController.ExecOperation(ParsedEnum));
			}
			else if (FullTag.StartsWith(TAG_COMPLEX))
			{
				string Command = FullTag.Replace(TAG_COMPLEX, string.Empty);
				Enum.TryParse(Command, out TProc<TComplex>.Oper ParsedEnum);
				tB_Complex.Text = NumberBeatifier(TAG_COMPLEX, complexController.ExecOperation(ParsedEnum));
			}
			else if (FullTag.StartsWith(TAG_PNUMBER))
			{
				string Command = FullTag.Replace(TAG_PNUMBER, string.Empty);
				Enum.TryParse(Command, out TProc<TPNumber>.Oper ParsedEnum);
				tB_PNumber.Text = pNumberController.ExecOperation(ParsedEnum);
			}
		}

		private void Button_Number_Function(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string FullTag = button.Tag.ToString();

			if (FullTag.StartsWith(TAG_FRAC))
			{
				string Command = FullTag.Replace(TAG_FRAC, string.Empty);
				Enum.TryParse(Command, out TProc<TFrac>.Func ParsedEnum);
				tB_Frac.Text = NumberBeatifier(TAG_FRAC, fracController.ExecFunction(ParsedEnum));
			}
			else if (FullTag.StartsWith(TAG_COMPLEX))
			{
				string Command = FullTag.Replace(TAG_COMPLEX, string.Empty);
				Enum.TryParse(Command, out TProc<TComplex>.Func ParsedEnum);
				tB_Complex.Text = NumberBeatifier(TAG_COMPLEX, complexController.ExecFunction(ParsedEnum));
			}
			else if (FullTag.StartsWith(TAG_PNUMBER))
			{
				string Command = FullTag.Replace(TAG_PNUMBER, string.Empty);
				Enum.TryParse(Command, out TProc<TPNumber>.Func ParsedEnum);
				tB_PNumber.Text = pNumberController.ExecFunction(ParsedEnum);
			}
		}

		private void Button_Reset(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string FullTag = button.Tag.ToString();

			if (FullTag.StartsWith(TAG_FRAC))
			{
				tB_Frac.Text = fracController.Reset();
				label_Frac_Memory.Text = string.Empty;
			}
			else if (FullTag.StartsWith(TAG_COMPLEX))
			{
				tB_Complex.Text = complexController.Reset();
				label_Complex_Memory.Text = string.Empty;
			}
			else if (FullTag.StartsWith(TAG_PNUMBER))
			{
				tB_PNumber.Text = pNumberController.Reset();
				label_PNumber_Memory.Text = string.Empty;
			}
		}

		private void Button_FinishEval(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string FullTag = button.Tag.ToString();

			if (FullTag.StartsWith(TAG_FRAC))
			{
				string result = NumberBeatifier(TAG_FRAC, fracController.Calculate());
				tB_Frac.Text = result;
				TFrac fracResult = new TFrac(result);
				fracHistory.Add(new TMemory<TFrac>(fracResult));
			}
			else if (FullTag.StartsWith(TAG_COMPLEX))
			{
				string result = NumberBeatifier(TAG_COMPLEX, complexController.Calculate());
				tB_Complex.Text = result;
				TComplex complexResult = new TComplex(result);
				complexHistory.Add(new TMemory<TComplex>(complexResult));
			}
			else if (FullTag.StartsWith(TAG_PNUMBER))
			{
				string result = pNumberController.Calculate();
				tB_PNumber.Text = result;
				// TPNumber через конструктор string + разрядность + точность
				TPNumber tpNum = new TPNumber(result, trackBar_PNumber.Value, 5);
				pNumberHistory.Add(new TMemory<TPNumber>(tpNum));
			}
		}

		private void Button_Memory(object sender, EventArgs e)
		{
			Button button = (Button)sender;
			string FullTag = button.Tag.ToString();

			if (FullTag.StartsWith(TAG_FRAC))
			{
				string Command = FullTag.Replace(TAG_FRAC, string.Empty);
				Enum.TryParse(Command, out TMemory<TFrac>.Commands ParsedEnum);
				dynamic exec = fracController.ExecCommandMemory(ParsedEnum, tB_Frac.Text);
				if (ParsedEnum == TMemory<TFrac>.Commands.Copy)
					tB_Frac.Text = exec.Item1.ToString();
				label_Frac_Memory.Text = exec.Item2 == TMemory<TFrac>.NumStates.ON ? "M" : string.Empty;
			}
			else if (FullTag.StartsWith(TAG_COMPLEX))
			{
				string Command = FullTag.Replace(TAG_COMPLEX, string.Empty);
				Enum.TryParse(Command, out TMemory<TComplex>.Commands ParsedEnum);
				dynamic exec = complexController.ExecCommandMemory(ParsedEnum, tB_Complex.Text);
				if (ParsedEnum == TMemory<TComplex>.Commands.Copy)
					tB_Complex.Text = exec.Item1.ToString();
				label_Complex_Memory.Text = exec.Item2 == TMemory<TComplex>.NumStates.ON ? "M" : string.Empty;
			}
			else if (FullTag.StartsWith(TAG_PNUMBER))
			{
				string Command = FullTag.Replace(TAG_PNUMBER, string.Empty);
				Enum.TryParse(Command, out TMemory<TPNumber>.Commands ParsedEnum);
				dynamic exec = pNumberController.ExecCommandMemory(ParsedEnum, tB_PNumber.Text);
				if (ParsedEnum == TMemory<TPNumber>.Commands.Copy)
					tB_PNumber.Text = exec.Item1.ToString();
				label_PNumber_Memory.Text = exec.Item2 == TMemory<TPNumber>.NumStates.ON ? "M" : string.Empty;
			}
		}

		// ====== новый метод: открытие истории ======
		private void историяToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form historyForm = new Form();
			historyForm.Text = "История вычислений";
			historyForm.Size = new System.Drawing.Size(400, 300);
			DataGridView dgv = new DataGridView();
			dgv.Dock = DockStyle.Fill;
			dgv.ColumnCount = 2;
			dgv.Columns[0].Name = "Тип числа";
			dgv.Columns[1].Name = "Значение";

			foreach (var mem in fracHistory)
				dgv.Rows.Add("Дробь", mem.FNumber.ToString());
			foreach (var mem in complexHistory)
				dgv.Rows.Add("Комплекс", mem.FNumber.ToString());
			foreach (var mem in pNumberHistory)
				dgv.Rows.Add("P-число", mem.FNumber.ToString());

			historyForm.Controls.Add(dgv);
			historyForm.ShowDialog();
		}

		private void СправкаToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Выполнили:\nШелегина Ольга, Смолевская Елизавета.\nГруппа: ИП-212.", "Универсальный калькулятор", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		// ====== ОСТАЛЬНЫЕ ОБРАБОТЧИКИ ======
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (tabControl.SelectedIndex)
			{
				case 0: Size = new System.Drawing.Size(310, 382); break;
				case 1: Size = new System.Drawing.Size(355, 382); break;
				case 2: Size = new System.Drawing.Size(355, 433); break;
			}

			// ← Обновляем кнопки, когда переключаемся на вкладку p-ичных чисел
			if (tabControl.SelectedIndex == 2) // предполагаем, что вкладка P-чисел — индекс 2
			{
				UpdatePNumberDigitButtons();
			}
		}

		private void TrackBar_PNumber_ValueChanged(object sender, EventArgs e)
		{
			label_PNumber_P.Text = trackBar_PNumber.Value.ToString();
			pNumberController.Edit.Notation = new TNumber(trackBar_PNumber.Value);
			tB_PNumber.Text = pNumberController.Reset();
			label_PNumber_Memory.Text = string.Empty;

			// ← Самое важное место: обновляем доступность кнопок при смене основания
			UpdatePNumberDigitButtons();
		}

		private void Form1_KeyPress(object sender, KeyPressEventArgs e)
		{
			switch (tabControl.SelectedIndex)
			{
				case 0:
					tB_PNumber.Text = pNumberController.ExecCommandEditor(CharToEditorCommand(e.KeyChar)); break;
				case 1:
					tB_Frac.Text = fracController.ExecCommandEditor(CharToEditorCommand(e.KeyChar)); break;
				case 2:
					tB_Complex.Text = complexController.ExecCommandEditor(CharToEditorCommand(e.KeyChar)); break;
			}
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			// обработка Enter и Backspace/CE
		}

		private void дробьFracTSMI_Click(object sender, EventArgs e)
		{
			дробьFracTSMI.Checked = true;
			числоFracTSMI.Checked = false;
			FracMode = true;
		}

		private void числоFracTSMI_Click(object sender, EventArgs e)
		{
			дробьFracTSMI.Checked = false;
			числоFracTSMI.Checked = true;
			FracMode = false;
		}

		private void комплексноеComplexTSMI_Click(object sender, EventArgs e)
		{
			комплексноеComplexTSMI.Checked = true;
			действительноеComplexTSMI.Checked = false;
			ComplexMode = true;
		}

		private void действительноеComplexTSMI_Click(object sender, EventArgs e)
		{
			комплексноеComplexTSMI.Checked = false;
			действительноеComplexTSMI.Checked = true;
			ComplexMode = false;
		}

		private void tabPage_Frac_Click(object sender, EventArgs e)
		{
		}
	}
}