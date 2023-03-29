namespace ModelViewPresenter_1.Wizard.ElementSelection
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ElementSelectionView : Dialog
	{
		public ElementSelectionView(IEngine engine)
			: base(engine)
		{
			FilterTextBox = new TextBox { MaxWidth = 200 };
			ElementsCheckBoxList = new CheckBoxList { IsSorted = true, Width = 200, MaxHeight = 400 };
			BackButton = new Button("Back");
			FinishButton = new Button("Finish");
			ValidationLabel = new Label { Style = TextStyle.Bold, IsVisible = false };

			AddWidget(new Label("Filter"), 0, 0);
			AddWidget(FilterTextBox, 0, 1);
			AddWidget(new Label("Elements"), 1, 0, verticalAlignment: VerticalAlignment.Top);
			AddWidget(ElementsCheckBoxList, 1, 1);
			AddWidget(BackButton, 2, 0);
			AddWidget(FinishButton, 2, 1);
			AddWidget(ValidationLabel, 3, 0, 1, 2);
		}

		public TextBox FilterTextBox { get; }

		public CheckBoxList ElementsCheckBoxList { get; }

		public Label ValidationLabel { get; }

		public Button BackButton { get; }

		public Button FinishButton { get; }
	}
}