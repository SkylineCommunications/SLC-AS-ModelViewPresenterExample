namespace ModelViewPresenter_1.Wizard.ProtocolSelection
{
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	public interface IProtocolSelectionView : IDialog
	{
		IDropDown ProtocolsDropDown { get; }

		IButton NextButton { get; }
	}
}