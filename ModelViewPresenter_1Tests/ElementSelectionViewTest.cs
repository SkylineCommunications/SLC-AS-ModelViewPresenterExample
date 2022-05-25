namespace Tests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using ModelViewPresenter_1.Wizard.ElementSelection;

	using Moq;

	using Skyline.DataMiner.Automation;

	[TestClass]
	public class ElementSelectionViewTest
	{
		[TestMethod]
		public void ShowTest()
		{
			// arrange
			var view = new ElementSelectionView(Mock.Of<IEngine>());

			// act
			try
			{
				view.Show(false);
			}
			catch (Exception e)
			{
				// assert
				Assert.Fail("Expected no exception, but got: " + e);
			}
		}
	}
}