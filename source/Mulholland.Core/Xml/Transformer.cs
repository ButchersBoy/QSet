using System;
using System.IO;
using System.Xml;
using System.Xml.Xsl; 
using System.Collections.Specialized;
using System.Xml.XPath;

namespace Mulholland.Core.Xml
{
	/*
		This class maybe freely reused provided that this comment section is unmodified.
	
		XML and XSLT utility classes developed for public use by Mulholland Software Ltd.	
	
		www.mulhollandsoftware.com
	*/

	/// <summary>
	/// Transforms XML documents using XSLT.
	/// </summary>
	public class Transformer
	{
		#region Member variables

		private string _xml = null;
		private string _xslt = null;
		private XsltArgumentList _xsltArgumentList = null;
		private ResourceResolver _additionalResourceResolver = new ResourceResolver();
		private ResourceRequestedEvent _additionalResourceRequestedEvent = null;

		#endregion

		#region Constructors

		/// <summary>
		/// Constructors transformer with minimum required details.
		/// </summary>		
		/// <param name="xslt">XSLT.</param>
		public Transformer(string xslt)
		{
			//validate input parameters
			if (xslt == null)
				throw new ArgumentNullException("xslt");
			else if (xslt.Length == 0)
				throw new ArgumentOutOfRangeException("xslt");
			
			//set member variables			
			_xslt = xslt;
		}

		#endregion

		#region Public interface

		/// <summary>
		/// Sets the XML to be transformed.
		/// </summary>
		public string Xml
		{
			set
			{
				if (value == null)
					throw new ArgumentNullException("Xml");
				else
					_xml = value;
			}
			get
			{
				return _xml;
			}
		}


		/// <summary>
		/// Adds a parameter to be included in the transform.
		/// </summary>
		/// <param name="name">Name of parameter as appears within XSLT.</param>
		/// <param name="namespaceUri">The namespace URI to associate with the parameter. To use the default namespace, specify an empty string.</param>
		/// <param name="parameter">The parameter value or object.</param>
		public void AddParameter(string name, string namespaceUri, object parameter)
		{			
			// NB overloaded

			//validate input parameters
			if (name == null)
				throw new ArgumentNullException("name");
			else if (name.Length == 0)
				throw new ArgumentOutOfRangeException("name");
			else if (parameter == null)
				throw new ArgumentNullException("parameter");

			//add parameter
			if (_xsltArgumentList == null)
				_xsltArgumentList = new XsltArgumentList();
			_xsltArgumentList.AddParam(name, namespaceUri, parameter);
		}	


		/// <summary>
		/// Adds a parameter to be included in the transform.
		/// </summary>
		/// <param name="name">Name of parameter as appears within XSLT.</param>		
		/// <param name="parameter">The parameter value or object.</param>
		public void AddParameter(string name, object parameter)
		{
			// NB overloaded

			AddParameter(name, "", parameter);
		}
		

		/// <summary>
		/// Adds an additional resource.  This could be extra XML which is imported into the transformation using the XSL document function.
		/// </summary>
		/// <param name="identifier">Identification or URI, as appears in XSL transform.</param>
		/// <param name="resource">Content of additional resource.</param>
		public void AddResource(string identifier, string resource)
		{
			//validate input parameters
			if (identifier == null)
				throw new ArgumentNullException("identifier");
			else if (identifier.Length == 0)
				throw new ArgumentOutOfRangeException("name");
			else if (resource == null)
				throw new ArgumentNullException("resource");
			else if (resource.Length == 0)
				throw new ArgumentOutOfRangeException("resource");

			//add the resouce
			_additionalResourceResolver.AddResource(identifier, resource);			
		}


		/// <summary>
		/// Adds a new object to the Transformer and associates it with the namespace URI.
		/// </summary>
		/// <param name="namespaceUri">The namespace URI to associate with the object. To use the default namespace, specify an empty string.</param>
		/// <param name="extension">The object to add to the list.</param>
		public void AddExtensionObject(string namespaceUri, object extension)
		{
			if (_xsltArgumentList == null)
				_xsltArgumentList = new XsltArgumentList();

			_xsltArgumentList.AddExtensionObject(namespaceUri, extension);			
		}


		/// <summary>
		/// Clears the currently configured parameters, resources, and extension objects.
		/// </summary>
		public void ClearAllAdditionalInput()
		{
			_xsltArgumentList = null;
			_additionalResourceResolver = null;
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
				_additionalResourceResolver.ResourceRequested += value;
			}
			remove
			{
				_additionalResourceResolver.ResourceRequested -= value;
			}
		}


		/// <summary>
		/// Transforms the currently loaded XML using the current XSLT.
		/// </summary>
		/// <returns>Transformed result.</returns>
		public string Transform()
		{			
			//initialise variables
			StringReader applicationStringReader = null;
			StringReader transformStringReader = null;
			StringWriter resultWriter = null;
			XmlTextReader transformXmlTextReader = null;

			try
			{				
				//validate object state
				if (_xml == null)
					throw new InvalidOperationException("Required XML is missing.");

				//read xml string into xpath document
				applicationStringReader = new StringReader(_xml);
				XPathDocument applicationXPathDocument = new XPathDocument(applicationStringReader);
				XPathNavigator applicationXPathNavigator = applicationXPathDocument.CreateNavigator();			
			
				//read xsl string into xml text reader			
				transformStringReader = new StringReader(_xslt);
				transformXmlTextReader = new XmlTextReader(transformStringReader);						
				
				//prepare the transform object
				System.Security.Policy.Evidence evidence = new System.Security.Policy.Evidence();
				XslTransform transform = new XslTransform();			
				transform.Load(transformXmlTextReader, _additionalResourceResolver, evidence);				

				//do the transform
				resultWriter = new StringWriter();
				transform.Transform(applicationXPathNavigator, _xsltArgumentList, resultWriter, _additionalResourceResolver);

				//return the result
				return resultWriter.ToString();
			}
			catch (Exception exc)
			{
				throw exc;				
			}
			finally
			{
				if (transformXmlTextReader != null)
					transformXmlTextReader.Close();
				DisposeObjects(applicationStringReader, transformStringReader, resultWriter);				
			}
		}

		#endregion

		#region Private implementation

		/// <summary>
		/// Dispose objects.
		/// </summary>
		/// <param name="objects">Objects to dispose.</param>
		private void DisposeObjects(params object[] objects)
		{
			foreach (object dispose in objects)
			{
				IDisposable disposeThis = dispose as IDisposable;
				if (disposeThis != null)
					disposeThis.Dispose();
			}
		}


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

		#endregion
	}
}
