namespace ModelViewPresenter_1.Wizard.ProtocolSelection
{
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public interface IProtocolSelectionView : IDialog
	{
		IDropDown ProtocolsDropDown { get; }

		IButton NextButton { get; }
	}
}