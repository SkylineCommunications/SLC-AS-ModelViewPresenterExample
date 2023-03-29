namespace ModelViewPresenter_1.Wizard.ElementSelection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Core.DataMinerSystem.Common;

	public class ElementSelectionPresenter
	{
		private readonly ElementSelectionView view;
		private readonly IElementSelector model;

		private HashSet<string> selectedElements = new HashSet<string>();
		private Dictionary<string, IDmsElement> elementsByName = new Dictionary<string, IDmsElement>();

		public ElementSelectionPresenter(ElementSelectionView view, IElementSelector model)
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
			elementsByName = model.Elements.ToDictionary(element => element.Name, element => element);
			selectedElements = new HashSet<string>(model.SelectedElements.Select(element => element.Name));
			FilterShownOptions();
		}

		private IEnumerable<string> Filter(IEnumerable<string> source)
		{
			return source
				.Where(name => name.ToUpperInvariant().Contains(view.FilterTextBox.Text.ToUpperInvariant()));
		}

		private void UpdateSetOfSelectedElements()
		{
			foreach (string element in view.ElementsCheckBoxList.Checked)
			{
				selectedElements.Add(element);
			}

			foreach (string element in view.ElementsCheckBoxList.Unchecked)
			{
				selectedElements.Remove(element);
			}
		}

		private void FilterShownOptions()
		{
			view.ElementsCheckBoxList.SetOptions(Filter(elementsByName.Keys));
			view.ElementsCheckBoxList.UncheckAll();
			foreach (string element in Filter(selectedElements))
			{
				view.ElementsCheckBoxList.Check(element);
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
			foreach (string selectedElement in selectedElements)
			{
				model.SelectedElements.Add(elementsByName[selectedElement]);
			}
		}
	}
}