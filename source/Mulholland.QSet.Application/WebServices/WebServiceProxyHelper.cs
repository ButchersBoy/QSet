using System;
using System.IO;
using System.Net;
using System.Text;
using Mulholland.Core;

namespace Mulholland.QSet.Application.WebServices
{
	/// <summary>
	/// Provides utilities for working with dynamix web service proxies.
	/// </summary>
	internal class WebServiceProxyHelper
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public WebServiceProxyHelper() {}


		/// <summary>
		/// Retrieves the WSDL for a web service.
		/// </summary>
		/// <param name="uri"></param>
		/// <returns></returns>
		/// <exception cref="NotSupportedException">The request scheme specified in requestUriString has not been registered.</exception>
		/// <exception cref="ArgumentNullException">uri is a null reference (Nothing in Visual Basic).</exception> 
		/// <exception cref="SecurityException">The caller does not have permission to connect to the requested URI or a URI that the request is redirected to.</exception>
		/// <exception cref="UriFormatException">The URI specified in uri is not a valid URI.</exception>
		/// <exception cref="InvalidOperationException">See <see cref="HttpWebRequest.GetResponse"/> documentation.</exception>  
		/// <exception cref="ProtocolViolationException">See <see cref="HttpWebRequest.GetResponse"/> documentation.</exception>
		/// <exception cref="WebException">See <see cref="HttpWebRequest.GetResponse"/> documentation.</exception>
		public string GetWsdl(string uri)
		{
			string wsdl = null;

			using (WebResponse webResponse = WebRequest.Create(uri).GetResponse())
			{
				StreamReader sr = new StreamReader(webResponse.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")); 
				wsdl = sr.ReadToEnd();
			}
	
			return wsdl;
			
		}


		/// <summary>
		/// Gets the MD5 checksum for the passed WSDL, in hexadecimal format.
		/// </summary>
		/// <param name="wsdl">WSDL to generate checksum.</param>
		/// <returns>Checksum in hexadecimal format.</returns>
		public string GetWsdlChecksum(string wsdl)
		{
			Cryptographer crypto = new Cryptographer();
			return IOUtilities.ConvertToHex(crypto.Hash(wsdl));
		}

	}
}
