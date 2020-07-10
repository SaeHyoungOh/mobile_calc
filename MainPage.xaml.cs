using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using gui_calc.Class;
using System.Xml;

namespace mobile_calc
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : ContentPage
	{
		private GuiInterface ui = new GuiInterface();
		public List<Label> HistoryItems { get; set; } = new List<Label>();

		// button click action
		private void NumberClick(object sender, EventArgs e)
		{
			// get the content of the button
			var content = (sender as Button).Text;
			// update the number box
			CurrentNumber.Text = ui.NumberAdd(content);
			CurrentCalculation.Text = ui.GetEquation();
		}
		private void OperatorClick(object sender, EventArgs e)
		{
			// get the content of the button
			var content = (sender as Button).Text;
			// update the current calculation box and the number box
			CurrentCalculation.Text = ui.OperatorAdd(content);
			CurrentNumber.Text = ui.GetNumString();

			// if "=" is entered, add it to the listbox
			if (content == "=")
			{
				Label newItem = new Label
				{
					Text = ui.GetEquation() + ui.GetNumString(),
					LineBreakMode = LineBreakMode.WordWrap,
					HorizontalTextAlignment = TextAlignment.End,
					TextColor = Color.White
				};
				HistoryItems.Insert(0, newItem);
			}
		}

		public MainPage()
		{
			InitializeComponent();
		}
	}
}
