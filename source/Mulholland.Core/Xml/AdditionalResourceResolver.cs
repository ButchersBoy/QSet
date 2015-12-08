using System;
using System.Collections.Specialized;
using System.IO;
using System.Xml;

namespace Mulholland.Core.Xml
{
	/*
		This class maybe freely reused provided that this comment section is unmodified.
	
		XML and XSLT utility classes developed for public use by Mulholland Software Ltd.	
	
		www.mulhollandsoftware.com
	*/

	/// <summary>
	/// Concrete implementation of a XmlResolver which allows user 
	/// defined strings to be resolved as additional resources.
	/// </summary>
	internal class ResourceResolver : XmlUrlResolver
	{
	
		#region Member variables and private properties 

		private const string _PROTOCOL = "custom://";			
		private const char _URI_SUFFIX = '/';
		private const string _ARGUMENT_INVALID = "Argument invalid.";
		private ResourceRequestedEvent _additionalResourceRequestedEvent = null;		

		private NameValueCollection _resources = null;		//* use private Resources property to work with collection within class

		/// <summary>
		/// Internal property for working with resources.
		/// </summary>
		private NameValueCollection Resources
		{
			get
			{
				if (_resources == null)
					_resources = new NameValueCollection();
				return _resources;
			}
		}

		#endregion

		#region Constructors and public interface

		/// <summary>
		/// Default constructor.  Constructs class with no additional resources.
		/// </summary>
		public ResourceResolver() {}


		/// <summary>
		/// Adds an additional resource.
		/// </summary>
		/// <param name="identifier">Identification key, as appears in XSL transform.</param>
		/// <param name="resource">Resource content.</param>
		public void AddResource(string identifier, string resource)
		{				
			Resources.Add(string.Format("{0}{1}{2}", _PROTOCOL, identifier, _URI_SUFFIX), resource);
		}


		/// <summary>
		/// Adds or removes handlers for the ResourceRequested event.
		/// <see cref="ResourceRequestedEvent"/>
		/// </summary>
		/// <remarks></remarks>
		public event ResourceRequestedEvent ResourceRequested
		{
			add
			{
				_additionalResourceRequestedEvent += value;
			}
			remove
			{
				_additionalResourceRequestedEvent -= value;
			}
		}


		#endregion

		/// <summary>
		/// Raises the ResourceRequested event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnResourceRequested(ResourceRequestedEventArgs e)
		{
			try
			{
				_additionalResourceRequestedEvent(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Strips the internal tags which are attached to a resource identifier for internal purposes.
		/// </summary>
		/// <param name="uri">Uri resource identifier string to strip.</param>
		/// <returns>Stripped string.</returns>
		private string StripInternalTags(string uri)
		{
			return uri.Remove(0, _PROTOCOL.Length).Remove(uri.Length - _PROTOCOL.Length - 1, 1);
		}

		#region XmlResolver overrides

		/// <summary>
		/// Returns the request resource as a memory stream.
		/// </summary>
		/// <param name="absoluteUri"></param>
		/// <param name="role"></param>
		/// <param name="ofObjectToReturn"></param>
		/// <returns></returns>
		override public object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
		{				
			try
			{
				//raise the requested event
				ResourceRequestedEventArgs e = new ResourceRequestedEventArgs(StripInternalTags(absoluteUri.ToString()), Resources[absoluteUri.AbsoluteUri]);
				OnResourceRequested(e);
	
				//extract the resource content from the event args (which allow the consumer to override) into the returning stream								
				MemoryStream ms = new MemoryStream(e.ResourceContent.Length);
				StreamWriter sw = new StreamWriter(ms);
				sw.Write(e.ResourceContent);
				sw.Flush();
				ms.Position = 0;

				return ms;
			}
			catch
			{
				return null;				
			}												
		}


		/// <summary>
		/// Resolves the URI so its content can be returned via GetEntitiy.
		/// </summary>
		/// <param name="baseUri"></param>
		/// <param name="relativeUri"></param>
		/// <returns></returns>
		override public Uri ResolveUri(Uri baseUri, string relativeUri)
		{
			if (relativeUri.Length < _PROTOCOL.Length || relativeUri.Substring(0, _PROTOCOL.Length) != _PROTOCOL)
			{
				return new Uri(_PROTOCOL + relativeUri);
			}
			else
			{
				return new Uri(relativeUri);
			}
		}

		#endregion
	}

}
