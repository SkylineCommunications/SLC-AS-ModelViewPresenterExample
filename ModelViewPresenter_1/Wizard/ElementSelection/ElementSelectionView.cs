namespace ModelViewPresenter_1.Wizard.ElementSelection
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ElementSelectionView : Dialog<GridPanel>, IElementSelectionView
	{
		public ElementSelectionView(IEngine engine)
			: base(engine)
		{
			FilterTextBox = new TextBox { MaxWidth = 200 };
			ElementsCheckBoxList = new CheckBoxList<IDmsElement> { IsSorted = true, Width = 200, MaxHeight = 400 };
			BackButton = new Button("Back");
			FinishButton = new Button("Finish");
			ValidationLabel = new Label { Style = TextStyle.Bold, IsVisible = false };

			Panel.Add(new Label("Filter"), 0, 0);
			Panel.Add(FilterTextBox, 0, 1);
			Panel.Add(new Label("Elements") { VerticalAlignment = VerticalAlignment.Top }, 1, 0);
			Panel.Add(ElementsCheckBoxList, 1, 1);
			Panel.Add(BackButton, 2, 0);
			Panel.Add(FinishButton, 2, 1);
			Panel.Add(ValidationLabel, 3, 0, 1, 2);
		}

		public ITextBox FilterTextBox { get; }

		public ICheckBoxList<IDmsElement> ElementsCheckBoxList { get; }

		public ILabel ValidationLabel { get; }

		public IButton BackButton { get; }

		public IButton FinishButton { get; }
	}
}