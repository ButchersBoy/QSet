using System;
using System.Reflection;
using Mulholland.Core;

namespace Mulholland.QSet.Resources
{	
	/// <summary>
	/// Provides internal application constants.
	/// </summary>
	public class Constants
	{
		static Constants() {}

		/// <summary>
		/// Gets the resource name of the Q Set file XSD.
		/// </summary>
		public static readonly string QSetXsdResource = "Mulholland.QSet.Resources.qset.xsd";


		/// <summary>
		/// Gets the resource name of the default html page to be displayed in the message viewer.
		/// </summary>
		public static readonly string MessageViewerDefaultPageResource = "Mulholland.QSet.Resources.MessageViewerDefault.htm";


		/// <summary>
		/// Gets the filename of the user settings file.
		/// </summary>
		public static readonly string SettingsFile = "settings.xml";


		/// <summary>
		/// Gets the filename of the license file.
		/// </summary>
		public static readonly string LicenseFile = "QSet.lic";


		/// <summary>
		/// Q Set file file extension.
		/// </summary>
		public static readonly string QSetFileExtension = "qset";


		/// <summary>
		/// Columns that should not appear in message browser.
		/// </summary>
		public static string[] MessageBrowserColumnExclusionList = 
			new string[] {"Body", "DefaultBodySize", "DefaultExtensionSize", "DefaultLabelSize", "UseJournalQueue"};
		
	}


	/// <summary>
	/// Provides access to documents embedded in the assembly.
	/// </summary>
	public class Documents
	{
		static Documents() {}

		/// <summary>
		/// Returns the contents of the Q Set file XSD.
		/// </summary>
		/// <returns>Content of the schema.</returns>
		public static string QSetFileXsd()
		{
			return IOUtilities.LoadEmbeddedResourceString(Assembly.GetExecutingAssembly(), Constants.QSetXsdResource);
		}


		/// <summary>
		/// Returns the contents of the Message Viewer default page.
		/// </summary>
		/// <returns>HTML source.</returns>
		public static string MessageViewerDefaultPage()
		{
			return IOUtilities.LoadEmbeddedResourceString(Assembly.GetExecutingAssembly(), Constants.MessageViewerDefaultPageResource);
		}
	}


	/// <summary>
	/// Enumerates the vailable message formatters.
	/// </summary>
	public enum MessageFormatterType
	{
		/// <summary>
		/// Active X.
		/// </summary>
		ActiveX,
		/// <summary>
		/// XML.
		/// </summary>
		Xml,
		/// <summary>
		/// Binary.
		/// </summary>
		Binary,
		/// <summary>
		/// No formatter (used when BodyStream is populated directly).
		/// </summary>
		None,
	}


	/// <summary>
	/// Provides constants regarding the Q Set XML file format.
	/// </summary>
	public class QSetXmlFileFormat
	{
		static QSetXmlFileFormat() {}


		/// <summary>
		/// Root element details.
		/// </summary>
		public class RootElement
		{
			static RootElement() {}

			/// <summary>
			/// Element name.
			/// </summary>
			public static readonly string Name = "qset";
		}


		/// <summary>
		/// Folder element details.
		/// </summary>
		public class FolderElement
		{
			static FolderElement() {}

			/// <summary>
			/// Element name.
			/// </summary>
			public static readonly string Name = "folder";
		}


		/// <summary>
		/// Machine element details.
		/// </summary>
		public class MachineElement
		{
			static MachineElement() {}

			/// <summary>
			/// Element name.
			/// </summary>
			public static readonly string Name = "machine";
		}


		/// <summary>
		/// Folder element details.
		/// </summary>
		public class QueueElement
		{
			static QueueElement() {}

			/// <summary>
			/// Element name.
			/// </summary>
			public static readonly string Name = "queue";

			
			/// <summary>
			/// Attribute details.
			/// </summary>
			public class Attributes
			{
				static Attributes() {}

				/// <summary>
				/// MessageFormatterTypeHint attribute.
				/// </summary>
				public static readonly string MessageViewerXslt = "xslt";				
			}
		}


		/// <summary>
		/// Web Service element details.
		/// </summary>
		public class WebServiceEelement
		{
			static WebServiceEelement() {}

			/// <summary>
			/// Element name.
			/// </summary>
			public static readonly string Name = "webService";


			/// <summary>
			/// Attribute details.
			/// </summary>
			public class Attributes
			{
				static Attributes() {}

				/// <summary>
				/// uri attribute.
				/// </summary>
				public static readonly string Uri = "uri";				
			}
		}


		/// <summary>
		/// Common item attributes.
		/// </summary>
		public class ItemAttributes
		{
			static ItemAttributes() {}

			/// <summary>
			/// Name attribute.
			/// </summary>
			public static readonly string Name = "name";
			
			/// <summary>
			/// GUID attribute.
			/// </summary>
			public static readonly string Guid = "guid";
		}
	
	}
	
}
