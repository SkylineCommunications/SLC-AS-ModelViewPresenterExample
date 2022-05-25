namespace ModelViewPresenter_1.Wizard.ElementSelection
{
	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;

	public interface IElementSelectionView : IDialog
	{
		ITextBox FilterTextBox { get; }

		ICheckBoxList ElementsCheckBoxList { get; }

		ILabel ValidationLabel { get; }

		IButton BackButton { get; }

		IButton FinishButton { get; }
	}
}