using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace gui_calc.ViewModel
{
	/// <summary>
	/// Class to provide interface between GUI and Calculation class
	/// </summary>
	public class GuiInterface : INotifyPropertyChanged
	{
		/// <summary>
		/// a list of Calculation objects to keep track of previous calculations (equations)
		/// </summary>
		private List<Calculation> history;

		/// <summary>
		/// index for the list of Calculation objects
		/// </summary>
		private int histIndex = 0;

		/// <summary>
		/// index for the numList and operList in a Calculation object
		/// </summary>
		private int pos = 0;

		/// <summary>
		/// current number to be displayed on calculator in string
		/// </summary>
		public string DisplayNumber
		{
			get
			{
				return displayNumber;
			}

			private set
			{
				displayNumber = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// required field for DisplayNumber property
		/// </summary>
		private string displayNumber;

		/// <summary>
		/// numbers and oeprators clicked so far for the current calculation
		/// </summary>
		public string Equation
		{
			get
			{
				return equation;
			}

			private set
			{
				equation = value;
				OnPropertyChanged();
			}
		}
		
		/// <summary>
		/// required field for Equation property
		/// </summary>
		private string equation;

		/// <summary>
		/// a list of previous equations to display on view
		/// </summary>
		public ObservableCollection<string> DisplayHistory
		{
			get
			{
				return displayHistory;
			}

			private set
			{
				displayHistory = value;
				OnPropertyChanged();
			}
		}

		/// <summary>
		/// required field for DisplayHistory property
		/// </summary>
		private ObservableCollection<string> displayHistory;

		/// <summary>
		/// currently a number is being processed, as oppsed to an operator
		/// </summary>
		private bool numberProcessing = false;

		/// <summary>
		/// it is a state where "=" is entered most recently
		/// </summary>
		private bool endOfEquation = false;

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// helper method to update View using PropertyChanged
		/// </summary>
		/// <param name="name"></param>
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

		/// <summary>
		/// constructor to instantiate lists and initialize variables
		/// </summary>
		public GuiInterface()
		{
			history = new List<Calculation>();
			history.Add(new Calculation());

			DisplayHistory = new ObservableCollection<string>();

			DisplayNumber = "0";
			Equation = string.Empty;
		}

		/// <summary>
		/// changes signs of the number
		/// </summary>
		private void PlusMinus()
		{
			// nothing to do for zero
			if (DisplayNumber == "0")
				return;

			// convert negative number to positive
			if (DisplayNumber[0] == '-')
			{
				DisplayNumber = DisplayNumber.Remove(0, 1);
			}
			// convert positive number to negative
			else
			{
				DisplayNumber = "-" + DisplayNumber;
			}
		}

		/// <summary>
		/// add number character to number string (DisplayNumber)
		/// </summary>
		/// <param name="sender">the number or symbol entered by user</param>
		/// <returns>DisplayNumber</returns>
		protected void NumberAdd(string sender)
		{
			// start a new equation
			if (endOfEquation)
			{
				endOfEquation = false;
			}

			// if previous entry was an operator, reset the number string
			if (numberProcessing)
			{
				// change of signs is an exception
				if (sender != "+/-")
				{
					DisplayNumber = "";
				}

				numberProcessing = false;
				if (history[histIndex].GetOperSize() == 0)
				{
					Equation = "";
				}
			}

			// change signs of the number
			if (sender == "+/-")
			{
				PlusMinus();
			}
			// clear number string and equation
			else if (sender == "C")
			{
				DisplayNumber = "0";
				Equation = "";
				history[histIndex].Clear();
			}
			// delete one character in number string
			else if (sender == "B")
			{
				if (DisplayNumber.Length > 0)
				{
					DisplayNumber = DisplayNumber.Remove(DisplayNumber.Length - 1);
				}

				if (DisplayNumber == "")
				{
					DisplayNumber = "0";
				}
			}
			// add the new number character to the number string
			else
			{
				// if there is only "0" in the number string, remove it first
				if (DisplayNumber == "0" && sender != ".")
				{
					DisplayNumber = "";
				}

				// if there is already a decimal point, do not add another
				if (!(sender == "." && DisplayNumber.Contains('.')))
				{
					DisplayNumber += sender;
				}

			}
		}

		/// <summary>
		/// add operator to the equation and return 
		/// </summary>
		/// <param name="sender">the operator entered by user</param>
		/// <returns>the whole equation formatted to display</returns>
		protected void OperatorAdd(char oper)
		{
			// add number string to equation
			if (!numberProcessing || endOfEquation)
			{
				if (Double.TryParse(DisplayNumber, out double result))
				{
					history[histIndex].AddNum(result);      // add the number
				}
			}
			numberProcessing = true;

			// do the running calculation
			if (pos == 0)
			{
				history[histIndex].Calculate('+');
			}
			else
			{
				DisplayNumber = Convert.ToString(history[histIndex].Calculate(history[histIndex].GetOper(pos - 1)));
			}

			pos++;

			// if "=" is entered after "=", re-do the previous operation
			if (endOfEquation && history[histIndex - 1].GetOperSize() > 1 && oper == '=')
			{
				// get the last number and operator of the previous equation
				double lastNum = history[histIndex - 1].GetNum(history[histIndex - 1].GetOperSize() - 1);
				char lastOper = history[histIndex - 1].GetOper(history[histIndex - 1].GetOperSize() - 2);

				history[histIndex].AddOper(lastOper);
				history[histIndex].AddNum(lastNum);
				DisplayNumber = Convert.ToString(history[histIndex].Calculate(history[histIndex].GetOper(pos - 1)));
			}

			history[histIndex].AddOper(oper);               // add the operator
			Equation = history[histIndex].GetEquation();    // string to return

			// if "=" is entered, start a new calculation
			if (oper == '=')
			{
				DisplayHistory.Insert(0, Equation + DisplayNumber);     // add the whole equation to the display list

				history.Add(new Calculation());
				histIndex++;
				pos = 0;
				endOfEquation = true;
			}
		}
	}
}
