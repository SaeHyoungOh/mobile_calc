using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gui_calc.Class
{
	/*
	 * Class to hold numbers and calculate them
	 */
	public class Calculation
	{
		private List<double> numList = new List<double>();
		private List<char> operList = new List<char>();
		private double runningResult = 0;

		// add a number to the operand list
		public int AddNum(double input)
		{
			numList.Add(input);
			return numList.Count;
		}

		// get a number from the operand list
		public double GetNum(int pos)
		{
			return numList[pos];
		}

		// add an operator to the list
		public int AddOper(char input)
		{
			if (operList.Count < numList.Count)
			{
				operList.Add(input);
			}
			// if the user inputs an operator more than once, replace it
			else
			{
				operList[operList.Count - 1] = input;
			}
			return operList.Count;
		}

		// get an oeprator from the list
		public char GetOper(int pos)
		{
			if (pos < operList.Count && pos >= 0)
			{
				return operList[pos];
			}
			else
			{
				return '\0';
			}
		}

		public int GetOperSize()
		{
			return numList.Count;
		}

		// set runningResult
		public void SetRunningResult(double num)
		{
			runningResult = num;
		}

		// get runningResult
		public double GetRunningResult()
		{
			return runningResult;
		}

		// get the equation in string
		public string GetEquation()
		{
			string result = "";

			for (int i = 0; i < numList.Count; i++)
			{
				result = result + numList[i] + " " + operList[i] + " ";
				if (i == numList.Count - 1)
				{
					result.TrimEnd();
				}
			}

			return result;
		}

		// clear all lists
		public void Clear()
		{
			numList.Clear();
			operList.Clear();
			runningResult = 0;
		}

		// do the calculation with the operator to the running result and return it
		public double Calculate(char oper)
		{
			switch (oper)
			{
				// add a number to runningResult
				case '+':
					runningResult += numList[numList.Count - 1];
					break;
				// subtract runningResult by a number
				case '-':
					runningResult -= numList[numList.Count - 1];
					break;
				// multiply runningResult by a number
				case '*':
					runningResult *= numList[numList.Count - 1];
					break;
				// divide runningResult by a number
				case '/':
					runningResult /= numList[numList.Count - 1];
					break;
			}
			return runningResult;
		}
	}
}
