using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gui_calc.ViewModel

{
	/// <summary>
	/// Class to hold numbers and calculate them
	/// </summary>
	public class Calculation
	{
		private List<double> numList = new List<double>();
		private List<char> operList = new List<char>();

		/// <summary>
		/// holds the running result of the calculation equation so far
		/// </summary>
		public double runningResult { get; private set; }

		/// <summary>
		/// constructor, initializes runningResult to 0
		/// </summary>
		public Calculation()
		{
			runningResult = 0;
		}

		/// <summary>
		/// add a number to the operand list
		/// </summary>
		/// <param name="input">number to add to the list</param>
		/// <returns>count of items in the list</returns>
		public int AddNum(double input)
		{
			numList.Add(input);
			return numList.Count;
		}

		/// <summary>
		/// get a number from the operand list
		/// </summary>
		/// <param name="pos">position of the number to get</param>
		/// <returns>the number at the position</returns>
		public double GetNum(int pos)
		{
			return numList[pos];
		}

		/// <summary>
		/// add an operator to the list
		/// </summary>
		/// <param name="input">operator to add to the list</param>
		/// <returns>count of items in the list</returns>
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

		/// <summary>
		/// get an oeprator from the list
		/// </summary>
		/// <param name="pos">position of the operator to get</param>
		/// <returns>the operator at the position, '\0' if position is out of bounds</returns>
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

		/// <summary>
		/// get the number of elements in the numbers (operands) or operators
		/// </summary>
		/// <returns></returns>
		public int GetOperSize()
		{
			return numList.Count;
		}

		/// <summary>
		/// get the formatted equation in string
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// clear all lists (numList and operList) and set runningResult to 0
		/// </summary>
		public void Clear()
		{
			numList.Clear();
			operList.Clear();
			runningResult = 0;
		}

		/// <summary>
		/// do the calculation with the operator to the running result and return it
		/// </summary>
		/// <param name="oper">operator</param>
		/// <returns>runningResult</returns>
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
