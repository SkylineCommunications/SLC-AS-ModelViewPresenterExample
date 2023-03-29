namespace ModelViewPresenter_1.Wizard.ElementSelection
{
	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public interface IElementSelectionView : IDialog
	{
		ITextBox FilterTextBox { get; }

		ICheckBoxList<IDmsElement> ElementsCheckBoxList { get; }

		ILabel ValidationLabel { get; }

		IButton BackButton { get; }

		IButton FinishButton { get; }
	}
}