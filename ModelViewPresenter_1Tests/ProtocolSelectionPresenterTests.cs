namespace Tests
{
	using System;
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using ModelViewPresenter_1;
	using ModelViewPresenter_1.Wizard.ProtocolSelection;

	using Moq;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	[TestClass]
	public class ProtocolSelectionPresenterTests
	{
		private MockRepository mocks;
		private ProtocolMocks protocolMocks;

		[TestInitialize]
		public void Setup()
		{
			mocks = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
			protocolMocks = new ProtocolMocks();
		}

		[TestMethod]
		public void LoadTest()
		{
			// arrange
			var view = mocks.OneOf<IProtocolSelectionView>(
				v =>
					v.ProtocolsDropDown == new DropDown());

			var model = mocks.OneOf<IElementSelector>(
				m =>
					m.Protocols == protocolMocks.All &&
					m.SelectedProtocol == protocolMocks.GenericPing);

			var protocolSelectionPresenter = new ProtocolSelectionPresenter(view, model);

			// act
			protocolSelectionPresenter.LoadFromModel();

			// assert
			string[] expectedOptions = protocolMocks.All.Select(protocol => $"{protocol.Name} ({protocol.Version})").ToArray();
			view.ProtocolsDropDown.Options.Should().BeEquivalentTo(expectedOptions);
			view.ProtocolsDropDown.Selected.Should().Be("Generic Ping (1.0.0.2)");
		}

		[TestMethod]
		public void StoreTest()
		{
			// arrange
			var view = mocks.OneOf<IProtocolSelectionView>(
				v =>
					v.ProtocolsDropDown == new DropDown());

			var model = mocks.OneOf<IElementSelector>(
				m =>
					m.Protocols == protocolMocks.All &&
					m.SelectedProtocol == protocolMocks.GenericPing);

			var presenter = new ProtocolSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ProtocolsDropDown.Selected = "CISCO CBR-8 (2.0.3.16)";
			Mock.Get(view.NextButton).Raise(button => button.Pressed += null, EventArgs.Empty); // mock the button press

			// assert
			Mock.Get(model).VerifySet(m => m.SelectedProtocol = protocolMocks.CiscoCbr8);
		}
	}
}