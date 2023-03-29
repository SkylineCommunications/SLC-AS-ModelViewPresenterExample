namespace ModelViewPresenter_1.Wizard.ProtocolSelection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	public class ProtocolSelectionPresenter
	{
		private readonly ProtocolSelectionView view;
		private readonly IElementSelector model;

		private Dictionary<string, IDmsProtocol> protocolsByName;

		public ProtocolSelectionPresenter(ProtocolSelectionView view, IElementSelector model)
		{
			this.view = view ?? throw new ArgumentNullException(nameof(view));
			this.model = model ?? throw new ArgumentNullException(nameof(model));

			view.NextButton.Pressed += OnNextButtonPressed;
		}

		public event EventHandler<EventArgs> Next;

		public void LoadFromModel()
		{
			protocolsByName = model.Protocols.ToDictionary(protocol => $"{protocol.Name} ({protocol.Version})");

			view.ProtocolsDropDown.SetOptions(protocolsByName.Keys);
			view.ProtocolsDropDown.Selected = $"{model.SelectedProtocol.Name} ({model.SelectedProtocol.Version})";
		}

		private void StoreToModel()
		{
			string selected = view.ProtocolsDropDown.Selected;
			model.SelectedProtocol = protocolsByName[selected];
		}

		private void OnNextButtonPressed(object sender, EventArgs e)
		{
			StoreToModel();

			Next?.Invoke(this, EventArgs.Empty);
		}
	}
}