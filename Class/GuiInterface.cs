using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace gui_calc.Class
{
	/*
	 * Class to provide interface between GUI and Calculation class
	 */
	class GuiInterface
	{
		private List<Calculation> history = new List<Calculation>();
		private int histIndex = 0, pos = 0;
		private string numString = "0";
		private string equation = "";
		private bool numberProcessing = false, endOfEquation = false;

		public GuiInterface()
		{
			history.Add(new Calculation());
		}
		public string GetNumString()
		{
			return numString;
		}
		public string GetEquation()
		{
			return equation;
		}
		private void PlusMinus()
		{
			// nothing to do for zero
			if (numString == "0")
				return;

			// convert negative number to positive
			if (numString[0] == '-')
			{
				numString = numString.Remove(0, 1);
			}
			// convert positive number to negative
			else
			{
				numString = "-" + numString;
			}
		}

		// add number character to number string
		public string NumberAdd(string sender)
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
					numString = "";
				}

				numberProcessing = false;
				if (history[histIndex].GetOperSize() == 0)
				{
					equation = "";
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
				numString = "0";
				equation = "";
				history[histIndex].Clear();
			}
			// delete one character in number string
			else if (sender == "\u232B")
			{
				numString = numString.Remove(numString.Length - 1);

				if (numString == "")
				{
					numString = "0";
				}
			}
			// add the new number character to the number string
			else
			{
				// if there is only "0" in the number string, remove it first
				if (numString == "0" && sender != ".")
				{
					numString = "";
				}

				// if there is already a decimal point, do not add another
				if (!(sender == "." && numString.Contains('.')))
				{
					numString += sender;
				}

			}

			return numString;
		}

		// add operator to the equation and return the whole equation
		public string OperatorAdd(string sender)
		{
			// add number string to equation
			if (!numberProcessing || endOfEquation)
			{
				if (Double.TryParse(numString, out double result))
				{
					history[histIndex].AddNum(result);		// add the number
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
				numString = Convert.ToString(history[histIndex].Calculate(history[histIndex].GetOper(pos - 1)));
			}

			pos++;

			// add operator to the equation
			char oper;
			switch (sender)
			{
				case "\u00F7":
					oper = '/';
					break;
				case "\u00D7":
					oper = '*';
					break;
				case "\u2212":
					oper = '-';
					break;
				default:
					oper = sender[0];
					break;
			}

			// if "=" is entered after "=", re-do the previous operation
			if (endOfEquation && history[histIndex - 1].GetOperSize() > 1 && oper == '=')
			{
				// get the last number and operator of the previous equation
				double lastNum = history[histIndex - 1].GetNum(history[histIndex - 1].GetOperSize() - 1);
				char lastOper = history[histIndex - 1].GetOper(history[histIndex - 1].GetOperSize() - 2);

				history[histIndex].AddOper(lastOper);
				history[histIndex].AddNum(lastNum);
				numString = Convert.ToString(history[histIndex].Calculate(history[histIndex].GetOper(pos - 1)));
			}

			history[histIndex].AddOper(oper);				// add the operator
			equation = history[histIndex].GetEquation();    // to return

			// if "=" is entered, start a new calculation
			if (oper == '=')
			{
				history.Add(new Calculation());
				histIndex++;
				pos = 0;
				endOfEquation = true;
			}

			return equation;
		}
	}
}
