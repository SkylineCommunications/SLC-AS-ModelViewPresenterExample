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

	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

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
			var view = mocks.OneOf<IElementSelectionView>(
				v =>
					v.ElementsCheckBoxList == new CheckBoxList<IDmsElement>() &&
					v.FilterTextBox == new TextBox());

			IDmsElement[] microsoftPlatformElements =
			{
				elementMocks.MicrosoftPlatformA,
				elementMocks.MicrosoftPlatformB,
				elementMocks.MicrosoftPlatformC,
			};
			var model = mocks.OneOf<IElementSelector>(
				m =>
					m.Elements == microsoftPlatformElements &&
					m.SelectedElements == new List<IDmsElement> { elementMocks.MicrosoftPlatformB }
			);

			var presenter = new ElementSelectionPresenter(view, model);

			// act
			presenter.LoadFromModel();

			// assert
			string[] expectedOptions = microsoftPlatformElements.Select(element => element.Name).ToArray();
			view.ElementsCheckBoxList.Options.Select(option => option.Name).Should().BeEquivalentTo(expectedOptions);
			view.ElementsCheckBoxList.CheckedOptions.Select(option => option.Name).Should().BeEquivalentTo(elementMocks.MicrosoftPlatformB.Name);
		}

		[TestMethod]
		public void StoreTest()
		{
			// arrange
			var view = mocks.OneOf<IElementSelectionView>(
				v =>
					v.ElementsCheckBoxList == new CheckBoxList<IDmsElement>() &&
					v.FilterTextBox == new TextBox());

			var model = mocks.OneOf<IElementSelector>(
				m =>
					m.Elements == elementMocks.MicrosoftPlatformElements &&
					m.SelectedElements == new List<IDmsElement> { elementMocks.MicrosoftPlatformB });

			var presenter = new ElementSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ElementsCheckBoxList.CheckedOptions.Add(Option.Create(elementMocks.MicrosoftPlatformC.Name, elementMocks.MicrosoftPlatformC));
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
			var view = mocks.OneOf<IElementSelectionView>(
				v =>
					v.ElementsCheckBoxList == new CheckBoxList<IDmsElement>() &&
					v.FilterTextBox == new TextBox());

			var model = mocks.OneOf<IElementSelector>(
				m =>
					m.Elements == elementMocks.MicrosoftPlatformElements &&
					m.SelectedElements == new List<IDmsElement> { elementMocks.MicrosoftPlatformB });

			var presenter = new ElementSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ElementsCheckBoxList.CheckedOptions.Remove(Option.Create(elementMocks.MicrosoftPlatformB.Name, elementMocks.MicrosoftPlatformB));
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
			var view = mocks.OneOf<IElementSelectionView>(
				v =>
					v.ElementsCheckBoxList == new CheckBoxList<IDmsElement>() &&
					v.FilterTextBox == mocks.OneOf<ITextBox>(
						textBox =>
							textBox.Text == String.Empty));

			var model = mocks.OneOf<IElementSelector>(
				m =>
					m.Elements == elementMocks.MicrosoftPlatformElements &&
					m.SelectedElements == new List<IDmsElement>());

			var presenter = new ElementSelectionPresenter(view, model);
			presenter.LoadFromModel();

			// act
			view.ElementsCheckBoxList.CheckedOptions.Add(Option.Create(elementMocks.MicrosoftPlatformC.Name, elementMocks.MicrosoftPlatformC));

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