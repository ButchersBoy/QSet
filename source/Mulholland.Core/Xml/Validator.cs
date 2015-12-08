using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace Mulholland.Core.Xml
{
	/*
		This class maybe freely reused provided that this comment section is unmodified.
	
		XML and XSLT utility classes developed for public use by Mulholland Software Ltd.	
	
		www.mulhollandsoftware.com
	*/

	#region Validator enumerations

	/// <summary>
	/// Validator result type.
	/// </summary>
	public enum ValidatorResultType
	{
		/// <summary>XML is valid against schema.</summary>
		Valid = 0,
		/// <summary>XML is valid against schema, but there are warnings.</summary>
		ValidWithWarnings,
		/// <summary>XML is invalid against schema.</summary>
		InValid
	}


	/// <summary>
	/// Validation short circuit type.  Specifies how the validator should react when a validation error is encountered.
	/// </summary>
	public enum ValidatorShortCircuitType
	{
		/// <summary>Use to notify that the validator should continue validating if a warning or error is encountered.</summary>
		None = 0,
		/// <summary>Use to notify that the validator should cease if a warning or error is encountered.</summary>
		OnWarning,
		/// <summary>Use to notify that the validator should cease if an error is encountered, but ignore warnings.</summary>
		OnError,
	}

	#endregion

	/// <summary>
	/// Allows validation of XML against a XSD.
	/// </summary>
	public class Validator
	{
		//validation type and result variables
		ValidatorResultType m_result;		
		ValidationType m_validationType; 
		ArrayList m_warnings = null;
		ArrayList m_errors = null;

		//schema settings
		string m_schema = null;		
		ArrayList m_additionalSchemas = null;
		XmlSchemaCollection m_schemas = null;
		bool m_isSchemaCollectionPrepared = false;			
		ResourceResolver m_additionalResourceResolver = null;
		string m_dtdDocTypeName = null;

		#region AdditionalSchema sub class

		/// <summary>
		/// Maintains an additional resource
		/// </summary>
		private class AdditionalSchema
		{
			private string m_schema = null;			
			private string m_identifier = null;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="identifier">Identifer of imported/included XML as appears in XSD.</param>
			/// <param name="schema">XSD or DTD schema.</param>
			public AdditionalSchema(string identifier, string schema)
			{
				m_identifier = identifier;
				m_schema = schema;
			}


			/// <summary>
			/// String representation of XSD.
			/// </summary>
			public string Xsd
			{
				get
				{
					return m_schema;
				}
			}


			/// <summary>
			/// Identifer of imported/included XML as appears in XSD.
			/// </summary>
			public string Identifier
			{
				get
				{
					return m_identifier;
				}
			}
		}

		#endregion

		#region Public interface

		/// <summary>
		/// Constructs the validator with a XSD schema.
		/// </summary>
		/// <param name="xsd">XSD schema.</param>
		public Validator(string xsd) 
		{
			//validate arguments
			if (xsd == null)
				throw new ArgumentNullException("xsd");
			else if (xsd.Length == 0)
				throw new ArgumentOutOfRangeException("xsd");

			//store schema
			m_schema = xsd;
			m_validationType = ValidationType.Schema;
		}


		/// <summary>
		/// Constructs the validator with a DTD schema.
		/// </summary>
		/// <param name="dtd">DTD schema.</param>
		/// <param name="docTypeName">The name of the document type declaration.</param>
		public Validator(string dtd, string docTypeName)
		{
			//validate arguments
			if (dtd == null )
				throw new ArgumentNullException("dtd");
			else if (dtd.Length == 0)
				throw new ArgumentOutOfRangeException("dtd");
			else if (docTypeName == null )
				throw new ArgumentNullException("docTypeName");
			else if (docTypeName.Length == 0)
				throw new ArgumentOutOfRangeException("docTypeName");

			//store schema
			m_schema = dtd.Substring(dtd.IndexOf("<!ELEMENT"));
			m_dtdDocTypeName = docTypeName;
			m_validationType = ValidationType.DTD;
		}


		/// <value>
		/// Returns the validation type of the class instance.
		/// </value>
		public ValidationType ValidationType
		{
			get
			{
				return m_validationType;
			}
		}


		/// <summary>
		/// Adds an additional schema to be included in the validation.
		/// </summary>
		/// <param name="identifier">Identification/URI/filename of imported/included XML as appears in XSD.</param>
		/// <param name="schema">XSD or DTD schema.</param>
		/// <author>James Willock</author>
		/// <creationdate>8th January 2004</creationdate>		
		public void AddAdditionalSchema(string identifier, string schema)
		{
			//validate arguments
			if (schema == null)
				throw new ArgumentNullException("schema");
			if (m_validationType == ValidationType.DTD)
				throw new InvalidOperationException("Cannot validate with multiple schemas for DTD's.");

			//enforce recreation of schema collection as it is now out-of-sync
			m_isSchemaCollectionPrepared = false;
			
			//add the additional schema to arraylist containing all schemas
			if (m_additionalSchemas == null)
				m_additionalSchemas = new ArrayList();
			m_additionalSchemas.Add(new AdditionalSchema(identifier, schema));					
		}


		/// <summary>
		/// Validates XML against a schema.
		/// </summary>
		/// <param name="xml">XML to validate.</param>
		/// <param name="shortCircuit">
		///		Defines validation behaviour should invalid XML be encountered.  Setting to a type other than None 
		///		forces the validation process to immedietely quit according to the type of problem encountered.
		///	</param>		
		/// <returns>Result as ValidatorResultType.  Extra information can be extracted from Warnings and Errors properties.</returns>		
		/// <overloaded/>
		public ValidatorResultType Validate(string xml, ValidatorShortCircuitType shortCircuit)
		{			
			// NB overloaded

			//validate arguments
			if (xml == null)
				throw new ArgumentNullException("xml");
			else if (xml.Length == 0)
				throw new ArgumentOutOfRangeException("xml");

			//initialise variables			
			m_warnings = null;
			m_errors = null;
			m_result = ValidatorResultType.Valid;

			try
			{
				//load the xml into validating reader
				XmlValidatingReader xmlValidatingReader = null;

				//configure validating reader and schema(s) according to schema type
				if (m_validationType == ValidationType.Schema)
				{				
					//if the XSD schema collection is not yet prepared, do so now
					if (!m_isSchemaCollectionPrepared)
					{
						//prepare the additional resources resolver with all of the extra XSD's
						PrepareResourceResolver();

						//load main XSD into schemas collection
						m_schemas = new XmlSchemaCollection();								
						m_schemas.Add(null, GetXmlTextReader(m_schema), m_additionalResourceResolver);
					
						//flag that we have already prepared the schemas to improve
						//performance on subsequent validation requests
						m_isSchemaCollectionPrepared = true;					
					}

					//create the validating reader and assign schemas
					xmlValidatingReader = new XmlValidatingReader(GetXmlTextReader(xml)); 								
					xmlValidatingReader.ValidationType = ValidationType.Schema;				
					xmlValidatingReader.Schemas.Add(m_schemas);
				}
				else
				{
					//set up validating reader for DTD
					XmlParserContext context = new XmlParserContext(null, null, m_dtdDocTypeName, null, null, m_schema, "", "", XmlSpace.None);					
					xmlValidatingReader = new XmlValidatingReader(xml, XmlNodeType.Element, context);
					xmlValidatingReader.ValidationType = ValidationType.DTD;         										
					xmlValidatingReader.EntityHandling = EntityHandling.ExpandCharEntities;
				}

				//assign event handler which will receive validation errors
				xmlValidatingReader.ValidationEventHandler += new ValidationEventHandler (ValidationHandler);

				//read the xml to perform validation				
				try
				{
					while (xmlValidatingReader.Read()) 
					{
						//check if we need to quit
						if (shortCircuit == ValidatorShortCircuitType.OnWarning 
							&& (m_result == ValidatorResultType.ValidWithWarnings || m_result == ValidatorResultType.InValid))
							break;
						else if (shortCircuit == ValidatorShortCircuitType.OnError && m_result == ValidatorResultType.InValid)
							break;
					}
				}
				catch (Exception exc)
				{
					//xml form error
					AddToArrayList(ref m_errors, exc.Message);
					m_result = ValidatorResultType.InValid;
				}
				
				return m_result;		
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}


		/// <summary>
		/// Validates XML against an XSD.  Equivelent to calling Validate(yourXml, ValidatorShortCircuitType.OnWarning, ValidationType.Schema)
		/// </summary>
		/// <param name="xml">XML to validate.</param>
		/// <returns>Result as ValidatorResultType.  Extra information can be extracted from Warnings and Errors properties.</returns>
		/// <overloaded/>		
		public ValidatorResultType Validate(string xml)
		{			
			return Validate(xml, ValidatorShortCircuitType.OnWarning);
		}


		/// <summary>
		/// Returns any errors encountered during the last validation run.
		/// </summary>
		public ArrayList Errors
		{
			get
			{
				return m_errors;
			}
		}


		/// <summary>
		/// Returns any warnings encountered during the last validation run.
		/// </summary>
		public ArrayList Warnings
		{
			get
			{
				return m_warnings;
			}
		}

		#endregion

		#region Private implementation
		
		/// <summary>
		/// Opens a XML text reader for the given XML.
		/// </summary>
		/// <param name="xml">XML to work with.</param>
		/// <returns>Opened XMLTextReader.</returns>
		private XmlTextReader GetXmlTextReader(string xml)
		{
			MemoryStream ms = null;
			StreamWriter sw = null;
			
			try
			{
				ms = new MemoryStream();
				sw = new StreamWriter(ms);			
				sw.Write(xml);
				sw.Flush();
				ms.Position = 0;			
				return new XmlTextReader(ms);			
			}
			catch
			{
				return null;
			}
		}


		/// <summary>
		/// Prepares the member level additional resource resolver with all of the additional schemas
		/// </summary>
		private void PrepareResourceResolver()
		{
			if (m_additionalSchemas != null)
			{
				m_additionalResourceResolver = new ResourceResolver();
				foreach (object additionalSchemaObject in m_additionalSchemas)
				{
					AdditionalSchema additionalSchema = additionalSchemaObject as AdditionalSchema;
					if (additionalSchema != null)
					{
						m_additionalResourceResolver.AddResource(additionalSchema.Identifier, additionalSchema.Xsd);
					}
				}				
			}			
		}

		/// <summary>
		/// Receives notification of a validation error.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void ValidationHandler(object sender, ValidationEventArgs args)
		{
			//store the validation error message and set result flag
			if (args.Severity == XmlSeverityType.Warning)
			{
				AddToArrayList(ref m_warnings, args.Message);
				if (m_result == ValidatorResultType.Valid)
					m_result = ValidatorResultType.ValidWithWarnings;
			}
			else
			{
				AddToArrayList(ref m_errors, args.Message);
				m_result = ValidatorResultType.InValid;
			}
		}


		/// <summary>
		/// Adds a string to an array list, ensuring that the object has been initialised.
		/// </summary>
		/// <param name="arrayList">Array to add to.</param>
		/// <param name="item">Item to add.</param>
		private void AddToArrayList(ref ArrayList arrayList, string item)
		{
			if (arrayList == null)
				arrayList = new ArrayList();
			arrayList.Add(item);
		}

		#endregion

	}

}


