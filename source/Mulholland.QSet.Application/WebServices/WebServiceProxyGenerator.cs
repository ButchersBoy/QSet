using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services.Discovery;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using Microsoft.CSharp;

namespace Mulholland.QSet.Application.WebServices
{
	/// <summary>
	/// Summary description for WebServiceProxyGenerator.
	/// </summary>
	internal class WebServiceProxyGenerator
	{
		private int _assemblyID = 0;
		private string _url = null;

		private const string _WSDL = "?wsdl";

		/// <summary>
		/// Constructs the WebServiceProxyGenerator.
		/// </summary>
		/// <param name="assemblyStorageFolder">Folder where proxy assemblies are to be stored.</param>
		/// <param name="url">URL of web service.</param>
		public WebServiceProxyGenerator(string assemblyStorageFolder, string url)
		{
			_url = url;
		}


		/// <summary>
		/// Gets the URL of the web service.
		/// </summary>
		public string Url
		{
			get
			{
				return _url;
			}
		}


		/// <summary>
		/// Gets the assmebly ID of the proxy.  This is set to zero prior to the proxy being generated.
		/// </summary>
		public int AssemblyID
		{
			get
			{
				return _assemblyID;
			}
		}


		/// <summary>
		/// Generates the proxy.
		/// </summary>
		/// <returns>True if successful, else false.</returns>
		public bool GenerateProxy()
		{
			try
			{
				//TODO exception handling!

				// inject SOAP extensions in (client side) ASMX pipeline
				InjectExtension(typeof(SoapMessageAccessClientExtension));

				string wsdl = GetWsdlFromUri(WsdlUrl);
		
				BuildAssemblyFromWsdl(wsdl);

				Console.WriteLine(wsdl);

			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
			}
			//TODO implement
			return false;
		}


		/// <summary>
		/// Returns the path of the proxy assembly.  Returns null until the proxy is generated.
		/// </summary>
		public string AssemblyPath
		{
			//TODO implement
			get
			{
				return null;
			}			
		}


		/// <summary>
		/// Gets the next available assembly ID.
		/// </summary>
		/// <returns>Assembly ID</returns>
		protected int GetNextAssemblyID()
		{
			//TODO implement
			return 1;
		}


		/// <summary>
		/// Gets the path of the WSDL.
		/// </summary>
		protected string WsdlUrl
		{
			get
			{
				return _url + _WSDL;
			}
		}


		/// <summary>
		/// Gets the WSDL from a URI.
		/// </summary>
		/// <param name="uri">URI.</param>
		/// <returns>WSDL</returns>
		private string GetWsdlFromUri(string uri)
		{
			string wsdlSource = null;

			using (WebResponse result = WebRequest.Create(uri).GetResponse())
			{
				StreamReader sr = new StreamReader(result.GetResponseStream(), System.Text.Encoding.GetEncoding("utf-8")); //ID					
				wsdlSource = sr.ReadToEnd();
			}
					
			return wsdlSource;
		}


		private void InjectExtension(Type extension)
		{
			Assembly assBase;
			Type webServiceConfig;
			object currentProp;
			PropertyInfo propInfo;
			object[] value;
			Type myType;
			object[] objArray;
			object myObj;
			FieldInfo myField;

			try
			{
				assBase = typeof(SoapExtensionAttribute).Assembly;
				webServiceConfig =
					assBase.GetType("System.Web.Services.Configuration.WebServicesConfiguration");

				if (webServiceConfig == null)
					throw new Exception("Error ...");

				currentProp = webServiceConfig.GetProperty("Current",
					BindingFlags.NonPublic | BindingFlags.Static |
					BindingFlags.Instance | BindingFlags.Public
					).GetValue(null, null);
				propInfo = webServiceConfig.GetProperty("SoapExtensionTypes",
					BindingFlags.NonPublic | BindingFlags.Static |
					BindingFlags.Instance | BindingFlags.Public
					);
				value = (object[])propInfo.GetValue(currentProp, null);
				myType = value.GetType().GetElementType();
				objArray = (object[])Array.CreateInstance(myType, (int)value.Length + 1);
				Array.Copy(value, objArray, (int)value.Length);

				myObj = Activator.CreateInstance(myType);
				myField = myType.GetField("Type",
					BindingFlags.NonPublic | BindingFlags.Static |
					BindingFlags.Instance | BindingFlags.Public
					);
				myField.SetValue(myObj, extension,
					BindingFlags.NonPublic | BindingFlags.Static |
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField,
					null, null
					);
				objArray[(int)objArray.Length - 1] = myObj;
				propInfo.SetValue(currentProp, objArray,
					BindingFlags.NonPublic | BindingFlags.Static |
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
					null, null, null
					);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}


		private Assembly BuildAssemblyFromWsdl(string strWsdl)
		{
			// Use an XmlTextReader to get the Web Service descompilerResultsiption
			StringReader  wsdlStringReader = new StringReader(strWsdl);
			XmlTextReader tr = new XmlTextReader(wsdlStringReader);
			ServiceDescription sd = ServiceDescription.Read(tr);
			tr.Close();

			// WSDL service descompilerResultsiption importer 
			CodeNamespace cns = new CodeNamespace("eYesoft.Tools.WebServices.DynamicProxy");
			//ServiceDescriptionImporter serviceDescriptionImporter;
			ServiceDescriptionImporter serviceDescriptionImporter = new ServiceDescriptionImporter(); // (was modular)
			//serviceDescompilerResultsiptorImporter.AddServiceDescompilerResultsiption(sd, null, null);
			
			// check for optional imports in the root WSDL
			CheckForImports(WsdlUrl, serviceDescriptionImporter);

			serviceDescriptionImporter.ProtocolName = "Soap";
			serviceDescriptionImporter.Import(cns, null);

			// change the base class assembly
			/*
			CodeTypeDeclaration ctDecl = cns.Types[0];
			cns.Types.Remove(ctDecl);
			ctDecl.BaseTypes[0] = new CodeTypeReference("eYesoft.Tools.WebServices.SoapHttpClientProtocolEx");
			cns.Types.Add(ctDecl);
			*/

			// source code generation
			CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
			ICodeGenerator codeGenerator = cSharpCodeProvider.CreateGenerator();
			StringBuilder srcStringBuilder = new StringBuilder();
			StringWriter sw = new StringWriter(srcStringBuilder);
			codeGenerator.GenerateCodeFromNamespace(cns, sw, null);
			string proxySource = srcStringBuilder.ToString();
			//TODO delete following three lines
			StreamWriter temp = new StreamWriter("c:\\source.cs");
			temp.Write(proxySource);
			temp.Close();
			sw.Close();

			// assemblyembly compilation
			CompilerParameters compilerParameters = new CompilerParameters();
			compilerParameters.ReferencedAssemblies.Add("System.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Xml.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Web.Services.dll");
			compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
			//compilerParameters.ReferencedAssemblies.Add(System.Reflection.Assembly.GetExecutingAssembly().Location);
	
			compilerParameters.GenerateExecutable = false;
			compilerParameters.GenerateInMemory = false;
			compilerParameters.IncludeDebugInformation = false; 

			ICodeCompiler codeCompiler = cSharpCodeProvider.CreateCompiler();
			CompilerResults compilerResults = codeCompiler.CompileAssemblyFromSource(compilerParameters, proxySource);
			
			if(compilerResults.Errors.Count > 0)
				throw new Exception(string.Format(@"Build failed: {0} errors", compilerResults.Errors.Count));

			Assembly assembly = compilerResults.CompiledAssembly;
			
			//rename temporary assemblyembly in order to cache it for later use
			RenameTempAssembly(compilerResults.PathToAssembly, strWsdl);
			 
			// compilerResultseate proxy instance
			//object proxyInstance = CreateInstance(typeName);
			
			return assembly;
		}


		private void RenameTempAssembly(string pathToAssembly, string strWsdl)
		{			
			string path = Path.GetDirectoryName(pathToAssembly);
			string newFilename = path + @"\" + GetMd5Sum(strWsdl) + "_QSetWebProxy.dll";
			
			File.Copy(pathToAssembly, newFilename);
		}


		private string GetMd5Sum(string str)
		{
			// First we need to convert the string into bytes, which
			// means using a text encoder
			Encoder enc = System.Text.Encoding.Unicode.GetEncoder();

			// Create a buffer large enough to hold the string
			byte[] unicodeText = new byte[str.Length * 2];
			enc.GetBytes(str.ToCharArray(), 0, str.Length, unicodeText, 0, true);

			// Now that we have a byte array we can ask the CSP to hash it
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(unicodeText);

			// Build the final string by converting each byte
			// into hex and appending it to a StringBuilder
			StringBuilder sb = new StringBuilder();
			for (int i=0;i<result.Length;i++)
			{
				sb.Append(result[i].ToString("X2"));
			}

			return sb.ToString();
		}


		private void CheckForImports(string baseWSDLUrl, ServiceDescriptionImporter sdi)
		{
			DiscoveryClientProtocol dcp = new DiscoveryClientProtocol();
			dcp.DiscoverAny(baseWSDLUrl);
			dcp.ResolveAll();

			foreach (object osd in dcp.Documents.Values)
			{
				if (osd is ServiceDescription) sdi.AddServiceDescription((ServiceDescription)osd, null, null);;
				if (osd is XmlSchema) sdi.Schemas.Add((XmlSchema)osd);
			}
		}

	}	
}
