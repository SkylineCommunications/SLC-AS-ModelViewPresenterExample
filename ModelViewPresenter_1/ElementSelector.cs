namespace ModelViewPresenter_1
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Library.Common;

	public class ElementSelector : IElementSelector
	{
		private readonly IDms dms;
		private readonly List<IDmsElement> selectedElements = new List<IDmsElement>();

		private IDmsProtocol[] protocols;
		private IDmsProtocol selectedProtocol;
		private ICollection<IDmsElement> allElements;
		private IDmsElement[] elements;

		public ElementSelector(IDms dms)
		{
			this.dms = dms ?? throw new ArgumentNullException(nameof(dms));
		}

		/// <summary>
		/// Gets the names of all protocols in the system.
		/// </summary>
		public IReadOnlyCollection<IDmsProtocol> Protocols
		{
			get
			{
				return protocols ?? (protocols = dms.GetProtocols().ToArray());
			}
		}

		/// <summary>
		/// Gets or sets the name of the selected protocol.
		/// </summary>
		public IDmsProtocol SelectedProtocol
		{
			get
			{
				return selectedProtocol ?? (selectedProtocol = Protocols.First());
			}

			set
			{
				if (value == SelectedProtocol)
				{
					return;
				}

				selectedProtocol = value;
				elements = null;
				selectedElements.Clear();
			}
		}

		/// <summary>
		/// Gets the list of elements running the selected protocol.
		/// </summary>
		public IReadOnlyCollection<IDmsElement> Elements
		{
			get
			{
				allElements = allElements ?? dms.GetElements();

				return elements ?? (elements = allElements
					.Where(element => element.Protocol.Name == selectedProtocol.Name)
					.Where(element => element.Protocol.Version == selectedProtocol.Version)
					.ToArray());
			}
		}

		/// <summary>
		/// Gets the list of selected elements.
		/// </summary>
		public ICollection<IDmsElement> SelectedElements
		{
			get
			{
				return selectedElements;
			}
		}
	}
}