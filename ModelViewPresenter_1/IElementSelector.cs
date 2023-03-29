namespace ModelViewPresenter_1
{
	using System.Collections.Generic;

	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	public interface IElementSelector
	{
		/// <summary>
		/// Gets the names of all protocols in the system.
		/// </summary>
		IReadOnlyCollection<IDmsProtocol> Protocols { get; }

		/// <summary>
		/// Gets or sets the name of the selected protocol.
		/// </summary>
		IDmsProtocol SelectedProtocol { get; set; }

		/// <summary>
		/// Gets the list of elements running the selected protocol.
		/// </summary>
		IReadOnlyCollection<IDmsElement> Elements { get; }

		/// <summary>
		/// Gets the list of selected elements.
		/// </summary>
		ICollection<IDmsElement> SelectedElements { get; }
	}
}