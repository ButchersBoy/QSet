using System;
using System.IO;
using System.Web.Services.Protocols;

namespace Mulholland.QSet.Application.WebServices
{
	public class SoapHttpClientProtocolEx : SoapHttpClientProtocol
	{
		private byte[] m_SoapRequestMsg = null;
		private byte[] m_SoapResponseMsg = null;

		public SoapHttpClientProtocolEx()
		{
		}

		public byte[] SoapRequest
		{
			get
			{
				return m_SoapRequestMsg;
			}
			set
			{
				m_SoapRequestMsg = value;
			}
		}
		
		public byte[] SoapResponse
		{
			get
			{
				return m_SoapResponseMsg;
			}
			set
			{
				m_SoapResponseMsg = value;
			}
		}

	}

	/// <summary>
	/// Summary description for SoapMessageAccessClientExtension.
	/// </summary>
	public class SoapMessageAccessClientExtension : SoapExtension
	{

		Stream oldStream = null;
		Stream newStream = null;
	
		public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute) 
		{
			return null;
		}

		public override object GetInitializer(Type t) 
		{
			return typeof(SoapMessageAccessClientExtension);
		}
    
		public override void Initialize(object initializer) 
		{
			//
		}
	    
		public override void ProcessMessage(SoapMessage message) 
		{
			switch (message.Stage) 
			{
				case SoapMessageStage.BeforeSerialize:
					break;

				case SoapMessageStage.AfterSerialize:
					StoreRequestMessage(message);
					// Pass it off as the actual stream
					Copy(newStream, oldStream);
					// Indicate for the return that we don't wish to chain anything in
					break;

				case SoapMessageStage.BeforeDeserialize:
					StoreResponseMessage(message);
					// Pass it off as the actual stream
					break;

				case SoapMessageStage.AfterDeserialize:
					break;

				default:
					throw new ArgumentException("Invalid Soap Message stage [" + message.Stage + "]", "message");
			}
		}
	    
		public override Stream ChainStream(Stream stream) 
		{
			// Store old
			oldStream = stream;
			newStream = new MemoryStream();

			// Return new stream
			return newStream;
		}

		private void StoreRequestMessage(SoapMessage message) 
		{
			// Rewind the source stream
			newStream.Position = 0;

			// Store message in our slot in the SoapHttpClientProtocol-derived class
			byte[] bufEncSoap = new Byte[newStream.Length];
			newStream.Read(bufEncSoap, 0, bufEncSoap.Length);
			((SoapHttpClientProtocolEx)(((SoapClientMessage)message).Client)).SoapRequest = bufEncSoap;
		}

		private void StoreResponseMessage(SoapMessage message) 
		{
			Stream tempStream = new MemoryStream();
			Copy(oldStream, tempStream);

			// Store message in our slot in the SoapHttpClientProtocol-derived class
			byte[] bufEncSoap = new Byte[tempStream.Length];
			tempStream.Read(bufEncSoap, 0, bufEncSoap.Length);
			((SoapHttpClientProtocolEx)(((SoapClientMessage)message).Client)).SoapResponse = bufEncSoap;

			Copy(tempStream, newStream);
		}

		void Copy(Stream from, Stream to) 
		{
			if (from.CanSeek == true)
				from.Position = 0;
			TextReader reader = new StreamReader(from);
			TextWriter writer = new StreamWriter(to);
			writer.WriteLine(reader.ReadToEnd());
			writer.Flush();
			if (to.CanSeek == true)
				to.Position = 0;
		}
	}
}
