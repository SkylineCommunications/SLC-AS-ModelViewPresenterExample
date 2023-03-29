namespace ModelViewPresenter_1.Wizard.ProtocolSelection
{
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ProtocolSelectionView : Dialog
	{
		public ProtocolSelectionView(IEngine engine)
			: base(engine)
		{
			ProtocolsDropDown = new DropDown { IsSorted = true, IsDisplayFilterShown = true };
			NextButton = new Button("Next");

			AddWidget(new Label("Protocol"), 0, 0);
			AddWidget(ProtocolsDropDown, 0, 1);
			AddWidget(NextButton, 1, 1);
		}

		public DropDown ProtocolsDropDown { get; }

		public Button NextButton { get; }
	}
}