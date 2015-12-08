using System;

namespace Mulholland.Core.Xml
{
	/*
		This class maybe freely reused provided that this comment section is unmodified.
	
		XML and XSLT utility classes developed for public use by Mulholland Software Ltd.	
	
		www.mulhollandsoftware.com
	*/

	/// <summary>
	/// ResourceAccessedEvent event delegate.
	/// <see cref="ResourceRequestedEventArgs"/>
	/// </summary>	
	public delegate void ResourceRequestedEvent(object sender, ResourceRequestedEventArgs e);


	/// <summary>
	/// Event arguments for ResourceAccessedEvent.
	/// </summary>
	public class ResourceRequestedEventArgs : EventArgs
	{
		private string _identifier;
		private string _resourceContent;

		/// <summary>
		/// Constructs the object with the minumum requirements.
		/// </summary>
		/// <param name="identifier">Identifier of the additional resource that was accessed.</param>
		/// <param name="resourceContent">The string content of the resource that has been pre-configured.</param>
		public ResourceRequestedEventArgs(string identifier, string resourceContent)
		{
			_identifier = identifier;
			_resourceContent = resourceContent;
		}


		/// <summary>
		/// Gets the identifier of the resource.
		/// </summary>
		public string Identifier
		{
			get
			{
				return _identifier;
			}
		}


		/// <summary>
		/// Gets or sets the content of the resource string.
		/// </summary>
		/// <remarks>
		/// The resource content initially configured when preparing a <c>Transformer</c> class using <c>AddResource</c>.
		/// Setting this property will overide the initial value and the new value will be passed into the transform.
		/// <see cref="Transformer.AddResource"/>
		/// </remarks>	
		public string ResourceContent
		{
			get
			{
				return _resourceContent;
			}
			set
			{
				_resourceContent = value;
			}
		}
 
	}

}
