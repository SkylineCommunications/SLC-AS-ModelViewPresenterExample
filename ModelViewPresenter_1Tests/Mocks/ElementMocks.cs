namespace Tests
{
	using Moq;

	using Skyline.DataMiner.Library.Common;

	public class ElementMocks
	{
		public IDmsElement[] MicrosoftPlatformElements { get; }

		public ElementMocks(ProtocolMocks protocolMocks)
		{
			var mocks = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

			MicrosoftPlatformA = mocks.OneOf<IDmsElement>(element =>
				element.Name == "Microsoft Platform A" &&
				element.DmsElementId == new DmsElementId(581, 5) &&
				element.Protocol == protocolMocks.MicrosoftPlatform);

			MicrosoftPlatformB = mocks.OneOf<IDmsElement>(element =>
				element.Name == "Microsoft Platform B" &&
				element.DmsElementId == new DmsElementId(582, 6) &&
				element.Protocol == protocolMocks.MicrosoftPlatform);

			MicrosoftPlatformC = mocks.OneOf<IDmsElement>(element =>
				element.Name == "Microsoft Platform C" &&
				element.DmsElementId == new DmsElementId(583, 7) &&
				element.Protocol == protocolMocks.MicrosoftPlatform);

			Cbr8Main = mocks.OneOf<IDmsElement>(element =>
				element.Name == "CBR8 Main" &&
				element.DmsElementId == new DmsElementId(581, 12) &&
				element.Protocol == protocolMocks.CiscoCbr8);

			Cbr8Backup = mocks.OneOf<IDmsElement>(element =>
				element.Name == "CBR8 Backup" &&
				element.DmsElementId == new DmsElementId(581, 13) &&
				element.Protocol == protocolMocks.CiscoCbr8);

			All = new[] { MicrosoftPlatformA, MicrosoftPlatformB, MicrosoftPlatformC, Cbr8Main, Cbr8Backup };

			MicrosoftPlatformElements = new[] { MicrosoftPlatformA, MicrosoftPlatformB, MicrosoftPlatformC };
		}

		public IDmsElement MicrosoftPlatformA { get; }
		public IDmsElement MicrosoftPlatformB { get; }
		public IDmsElement MicrosoftPlatformC { get; }
		public IDmsElement Cbr8Main { get; }
		public IDmsElement Cbr8Backup { get; }
		public IDmsElement[] All { get; }
	}
}