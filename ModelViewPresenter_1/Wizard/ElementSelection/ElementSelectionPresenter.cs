namespace ModelViewPresenter_1.Wizard.ElementSelection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Core.DataMinerSystem.Common;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript;

	public class ElementSelectionPresenter
	{
		private readonly IElementSelectionView view;
		private readonly IElementSelector model;

		private HashSet<IDmsElement> selectedElements = new HashSet<IDmsElement>();

		public ElementSelectionPresenter(IElementSelectionView view, IElementSelector model)
		{
			this.view = view ?? throw new ArgumentNullException(nameof(view));
			this.model = model ?? throw new ArgumentNullException(nameof(model));

			view.BackButton.Pressed += OnBackButtonPressed;
			view.FinishButton.Pressed += OnFinishPressed;
			view.FilterTextBox.Changed += (sender, e) => FilterShownOptions();
			view.Interacted += OnViewInteracted;
		}

		public event EventHandler<EventArgs> Finish;

		public event EventHandler<EventArgs> Back;

		public void LoadFromModel()
		{
			selectedElements = new HashSet<IDmsElement>(model.SelectedElements);
			FilterShownOptions();
		}

		private IEnumerable<Option<IDmsElement>> FilterOptions()
		{
			return model.Elements
				.Where(element => element.Name.ToUpperInvariant().Contains(view.FilterTextBox.Text.ToUpperInvariant()))
				.Select(element => Option.Create(element.Name, element));
		}

		private IEnumerable<Option<IDmsElement>> FilterSelectedOptions()
		{
			return model.SelectedElements
				.Where(element => element.Name.ToUpperInvariant().Contains(view.FilterTextBox.Text.ToUpperInvariant()))
				.Select(element => Option.Create(element.Name, element));
		}

		private void UpdateSetOfSelectedElements()
		{
			selectedElements.Clear();
			foreach (Option<IDmsElement> option in view.ElementsCheckBoxList.CheckedOptions)
			{
				selectedElements.Add(option.Value);
			}

			IEnumerable<Option<IDmsElement>> uncheckedOptions = view.ElementsCheckBoxList.Options
				.Except(view.ElementsCheckBoxList.CheckedOptions);

			foreach (Option<IDmsElement> option in uncheckedOptions)
			{
				selectedElements.Remove(option.Value);
			}
		}

		private void FilterShownOptions()
		{
			view.ElementsCheckBoxList.Options.Clear();
			foreach (Option<IDmsElement> option in FilterOptions())
			{
				view.ElementsCheckBoxList.Options.Add(option);
			}

			foreach (Option<IDmsElement> element in FilterSelectedOptions())
			{
				view.ElementsCheckBoxList.CheckedOptions.Add(element);
			}
		}

		private void OnViewInteracted(object sender, EventArgs e)
		{
			ClearValidationProblem();
			UpdateSetOfSelectedElements();
		}

		private void OnFinishPressed(object sender, EventArgs e)
		{
			if (selectedElements.Any())
			{
				StoreToModel();
				Finish?.Invoke(this, EventArgs.Empty);
			}
			else
			{
				ShowValidationProblem("At least one element must be selected.");
			}
		}

		private void ShowValidationProblem(string s)
		{
			view.ValidationLabel.Text = s;
			view.ValidationLabel.IsVisible = true;
		}

		private void ClearValidationProblem()
		{
			view.ValidationLabel.IsVisible = false;
		}

		private void OnBackButtonPressed(object sender, EventArgs e)
		{
			StoreToModel();
			Back?.Invoke(this, EventArgs.Empty);
		}

		private void StoreToModel()
		{
			model.SelectedElements.Clear();
			foreach (IDmsElement selectedElement in selectedElements)
			{
				model.SelectedElements.Add(selectedElement);
			}
		}
	}
}