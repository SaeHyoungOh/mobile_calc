using Prism.Commands;
using System.Linq;

namespace gui_calc.ViewModel
{
	class DelegatedMobileGUI : GuiInterface
	{
		public DelegateCommand<string> NumberClick { get; private set; }
		public DelegateCommand<string> OperatorClick { get; private set; }

		/// <summary>
		/// button click action for numbers and symbols
		/// </summary>
		/// <param name="content">content of the button</param>
		private void NumberClickExecute(string content)
		{
			NumberAdd(content);
		}

		private bool NumberClickCanExecute(string content)
		{
			string[] example = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "+/-", ".", "C", "B" };
			return example.Contains(content);
		}

		/// <summary>
		/// button click action for operators
		/// </summary>
		/// <param name="content"></param>
		private void OperatorClickExecute(string content)
		{
			OperatorAdd(content[0]);
		}

		private bool OperatorClickCanExecute(string content)
		{
			string[] example = { "/", "*", "-", "+", "=" };
			return example.Contains(content);
		}

		public DelegatedMobileGUI()
		{
			NumberClick = new DelegateCommand<string>(NumberClickExecute, NumberClickCanExecute);
			OperatorClick = new DelegateCommand<string>(OperatorClickExecute, OperatorClickCanExecute);
		}
	}
}
