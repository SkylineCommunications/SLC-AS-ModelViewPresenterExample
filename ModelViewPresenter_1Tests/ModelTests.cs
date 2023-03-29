namespace Tests
{
	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using ModelViewPresenter_1;

	using Moq;

	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	[TestClass]
	public class ModelTests
	{
		private MockRepository mocks;
		private ProtocolMocks protocolMocks;
		private ElementMocks elementMocks;

		[TestInitialize]
		public void Setup()
		{
			mocks = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };
			protocolMocks = new ProtocolMocks();
			elementMocks = new ElementMocks(protocolMocks);
		}

		[TestMethod]
		public void NoNullsAfterConstructionTest()
		{
			// arrange
			var dms = mocks.OneOf<IDms>(d =>
				d.GetProtocols() == protocolMocks.All &&
				d.GetElements() == elementMocks.All);

			// act
			var model = new ElementSelector(dms);

			// assert
			model.Protocols.Should().BeEquivalentTo(protocolMocks.All);
			model.SelectedProtocol.Should().NotBeNull();
			model.Elements.Should().NotBeNull();
			model.SelectedElements.Should().BeEmpty();
		}

		[TestMethod]
		public void ChangeSelectedProtocolTest()
		{
			// arrange
			var dms = mocks.OneOf<IDms>(d =>
				d.GetProtocols() == protocolMocks.All &&
				d.GetElements() == elementMocks.All);

			var model = new ElementSelector(dms);

			// act
			model.SelectedProtocol = protocolMocks.MicrosoftPlatform;

			// assert
			model.Elements.Should().BeEquivalentTo(elementMocks.MicrosoftPlatformElements);
			model.SelectedElements.Should().BeEmpty();
		}

		[TestMethod]
		public void ElementSelectionClearsIfProtocolChangesTest()
		{
			// arrange
			var dms = mocks.OneOf<IDms>(d =>
				d.GetProtocols() == protocolMocks.All &&
				d.GetElements() == elementMocks.All);

			var model = new ElementSelector(dms);
			model.SelectedProtocol = protocolMocks.MicrosoftPlatform;
			model.SelectedElements.Add(elementMocks.MicrosoftPlatformA);

			// act
			model.SelectedProtocol = protocolMocks.CiscoCbr8;

			// assert
			model.Elements.Should().NotBeEmpty();
			model.SelectedElements.Should().BeEmpty();
		}

		[TestMethod]
		public void KeepElementSelectionIfProtocolDoesNotChangeTest()
		{
			// arrange
			var dms = mocks.OneOf<IDms>(d =>
				d.GetProtocols() == protocolMocks.All &&
				d.GetElements() == elementMocks.All);

			var model = new ElementSelector(dms);
			model.SelectedProtocol = protocolMocks.MicrosoftPlatform;
			model.SelectedElements.Add(elementMocks.MicrosoftPlatformA);

			// act
			model.SelectedProtocol = protocolMocks.MicrosoftPlatform;

			// assert
			model.SelectedElements.Should().BeEquivalentTo(new[] { elementMocks.MicrosoftPlatformA });
		}
	}
}