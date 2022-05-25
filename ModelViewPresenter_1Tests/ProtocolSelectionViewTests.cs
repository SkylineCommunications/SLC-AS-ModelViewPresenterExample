namespace Tests
{
	using System;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using ModelViewPresenter_1.Wizard.ProtocolSelection;

	using Moq;

	using Skyline.DataMiner.Automation;

	[TestClass]
	public class ProtocolSelectionViewTests
	{
		[TestMethod]
		public void ShowTest()
		{
			// arrange
			var view = new ProtocolSelectionView(Mock.Of<IEngine>());

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