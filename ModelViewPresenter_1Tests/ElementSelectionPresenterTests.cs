namespace Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using FluentAssertions;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using ModelViewPresenter_1;
	using ModelViewPresenter_1.Wizard.ElementSelection;

	using Moq;

	using Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit;
	using Skyline.DataMiner.Library.Common;

	[TestClass]
	public class ElementSelectionPresenterTests
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
		public void LoadTest()
		{
			// arrange
			var view = mocks.OneOf<IElementSelectionView>(v =>
				v.ElementsCheckBoxList == new CheckBoxList() &&
				v.FilterTextBox == new TextBox());

			IDmsElement[] microsoftPlatformElements =
			{
				elementMocks.MicrosoftPlatformA, elementMocks.MicrosoftPlatformB,
				elementMocks.MicrosoftPlatformC
			};
			var model = mocks.OneOf<IElementSelector>(m =>
				m.Elements == microsoftPlatformElements &&
				m.SelectedElements == new List<IDmsElement> { elementMocks.MicrosoftPlatformB }
			);

			var presenter = new ElementSelectionPresenter(view, model);

			// act
			presenter.LoadFromModel();

			// assert
			string[] expectedOptions = microsoftPlatformElements.Select(element => element.Name).ToArray();
			view.ElementsCheckBoxList.Options.Should().BeEquivalentTo(expectedOptions);
			view.ElementsCheckBoxList.Checked.Should().BeEquivalentTo(elementMocks.MicrosoftPlatformB.Name);
		}

		[TestMethod]
		public void StoreTest()
		{
			// arrange
			var view = mocks.OneOf<IElementSelectionView>(v =>
				v.ElementsCheckBoxList == new CheckBoxList() &&
				v.FilterTextBox == new TextBox());

			var model = mocks.OneOf<IElementSelector>(m =>
				m.Elements == elementMocks.MicrosoftPlatformElements &&
				m.SelectedElements == new List<IDmsElement> { elementMocks.MicrosoftPlatformB });

			var presenter = new ElementSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ElementsCheckBoxList.Check(elementMocks.MicrosoftPlatformC.Name);
			Mock.Get(view).Raise(v => v.Interacted += null, EventArgs.Empty); // mock dialog interacted event
			Mock.Get(view.FinishButton).Raise(button => button.Pressed += null, EventArgs.Empty); // mock button press

			// assert
			model.SelectedElements.Should()
				.BeEquivalentTo(new[] { elementMocks.MicrosoftPlatformB, elementMocks.MicrosoftPlatformC });
		}

		[TestMethod]
		public void StoreInvalidTest()
		{
			// arrange
			var view = mocks.OneOf<IElementSelectionView>(v =>
				v.ElementsCheckBoxList == new CheckBoxList() &&
				v.FilterTextBox == new TextBox());

			var model = mocks.OneOf<IElementSelector>(m =>
				m.Elements == elementMocks.MicrosoftPlatformElements &&
				m.SelectedElements == new List<IDmsElement> { elementMocks.MicrosoftPlatformB });

			var presenter = new ElementSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ElementsCheckBoxList.Uncheck(elementMocks.MicrosoftPlatformB.Name);
			Mock.Get(view).Raise(selectionView => selectionView.Interacted += null, EventArgs.Empty);
			Mock.Get(view.FinishButton).Raise(button => button.Pressed += null, EventArgs.Empty); // mock button press

			// assert
			Mock.Get(view.ValidationLabel).VerifySet(label => label.Text = It.IsAny<string>());
			Mock.Get(view.ValidationLabel).VerifySet(label => label.IsVisible = true);
		}

		[TestMethod]
		public void FilterHasNoInfluenceOnSelectedItemsTest()
		{
			// arrange
			var view = mocks.OneOf<IElementSelectionView>(v =>
				v.ElementsCheckBoxList == new CheckBoxList() &&
				v.FilterTextBox == mocks.OneOf<ITextBox>(textBox =>
					textBox.Text == String.Empty));

			var model = mocks.OneOf<IElementSelector>(m =>
				m.Elements == elementMocks.MicrosoftPlatformElements &&
				m.SelectedElements == new List<IDmsElement>());

			var presenter = new ElementSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ElementsCheckBoxList.Check(elementMocks.MicrosoftPlatformC.Name);

			view.FilterTextBox.Text = "Element that does not exist"; // filter that will match with zero elements
			Mock.Get(view).Raise(v => v.Interacted += null, EventArgs.Empty);
			Mock.Get(view.FilterTextBox).Raise(textBox => textBox.Changed += null, new TextBox.ChangedEventArgs("Element that does not exist", String.Empty));

			Mock.Get(view.FinishButton).Raise(button => button.Pressed += null, EventArgs.Empty);

			// assert
			view.ElementsCheckBoxList.Options.Should().BeEmpty();
			model.SelectedElements.Should().BeEquivalentTo(new[] { elementMocks.MicrosoftPlatformC });
		}
	}
}