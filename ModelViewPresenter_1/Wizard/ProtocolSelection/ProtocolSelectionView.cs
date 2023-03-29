namespace ModelViewPresenter_1.Wizard.ProtocolSelection
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ProtocolSelectionView : Dialog<GridPanel>, IProtocolSelectionView
	{
		public ProtocolSelectionView(IEngine engine)
			: base(engine)
		{
			ProtocolsDropDown = new DropDown { IsSorted = true, IsDisplayFilterShown = true };
			NextButton = new Button("Next");

			Panel.Add(new Label("Protocol"), 0, 0);
			Panel.Add(ProtocolsDropDown, 0, 1);
			Panel.Add(NextButton, 1, 1);
		}

		public IDropDown ProtocolsDropDown { get; }

		public IButton NextButton { get; }
	}
}