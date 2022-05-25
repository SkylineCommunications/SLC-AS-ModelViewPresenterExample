namespace Tests
{
	using Moq;

	using Skyline.DataMiner.Library.Common;

	public class ProtocolMocks
	{
		public ProtocolMocks()
		{
			var mocks = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

			MicrosoftPlatform = mocks.OneOf<IDmsProtocol>(protocol =>
				protocol.Name == "Microsoft Platform" &&
				protocol.Version == "Production");

			CiscoCbr8 = mocks.OneOf<IDmsProtocol>(protocol =>
				protocol.Name == "CISCO CBR-8" &&
				protocol.Version == "2.0.3.16");

			GenericPing = mocks.OneOf<IDmsProtocol>(protocol =>
				protocol.Name == "Generic Ping" &&
				protocol.Version == "1.0.0.2");

			All = new[] { GenericPing, MicrosoftPlatform, CiscoCbr8 };
		}

		public IDmsProtocol MicrosoftPlatform { get; }
		public IDmsProtocol CiscoCbr8 { get; }
		public IDmsProtocol GenericPing { get; }
		public IDmsProtocol[] All { get; }
	}
}