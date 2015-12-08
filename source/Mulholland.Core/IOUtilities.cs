using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.Win32;

namespace Mulholland.Core
{
	/// <summary>
	/// Provides general input/output utilities.
	/// </summary>
	public class IOUtilities
	{
		static IOUtilities() {}


		/// <summary>
		/// Loads an embedded resource from an assembly.
		/// </summary>
		/// <param name="assembly">Assembly containing the resource.</param>
		/// <param name="qualifiedName">Fully qualified name of resource, including namespace and filename.</param>
		/// <returns>Resource content.</returns>
		/// <exception cref="IOUtilitiesException">Throw if the resource could not be accessed for any reason.</exception>
		public static string LoadEmbeddedResourceString(Assembly assembly, string qualifiedName)
		{
			string resourceString = null; 

			try
			{
				using (StreamReader sr = new StreamReader(assembly.GetManifestResourceStream(qualifiedName)))
				{
					resourceString = sr.ReadToEnd();
				}
			}
			catch (Exception exc)
			{
				throw new IOUtilitiesException("Resource could not be loaded.", exc);
			}

			return resourceString;
		}


		/// <summary>
		/// Loads a string into a MemoryStream.
		/// </summary>
		/// <param name="input">String to load.</param>
		/// <returns>A new MemoryStream populated with the contents of the stream, at position 0.</returns>
		public static MemoryStream StringToMemoryStream(string input)
		{
			if (input == null)
				throw new IOUtilitiesException("input is null");
						
			MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(input));
			ms.Flush();
			ms.Position = 0;

			return ms;
		}


		/// <summary>
		/// Copies one stream to another.
		/// </summary>
		/// <param name="from">Source stream.</param>
		/// <param name="to">Destination stream.</param>
		/// <remarks>Performs an entire stream copy, starting the writing at the destination position.  
		/// The position of the source stream as always returned to its original position.</remarks>
		public static void CopyStream(Stream from, Stream to)
		{
			BinaryReader br = new BinaryReader(from);
			BinaryWriter bw = new BinaryWriter(to);

			long fromOriginalPosition = from.Position;

			try
			{
				from.Position = 0;
				
				while (from.Position < from.Length)
					bw.Write(br.ReadByte());

				bw.Flush();
				to.Position = 0;
			}
			finally
			{
				from.Position = fromOriginalPosition;
			}
		}


		/// <summary>
		/// Converts a string to hexadecimal.
		/// </summary>
		/// <param name="input">String to convert.</param>
		/// <returns>Hexadecimal.</returns>
		public static string ConvertToHex(string input)
		{
			// load the input string into an array of bytes
			byte[] bytes = new byte[input.Length * 2];			
			Encoding.Unicode.GetEncoder().GetBytes(input.ToCharArray(), 0, input.Length, bytes, 0, true);

			// convert bytes into hex
			StringBuilder sb = new StringBuilder(bytes.Length * 2);
			for (int i = 0; i < bytes.Length; i ++)
			{
				sb.Append(bytes[i].ToString("X2"));
			}

			//return the result
			return sb.ToString();
		}


		/// <summary>
		/// Assosciate file type with an application in the Registry.
		/// </summary>
		/// <param name="fileExtension">File extension.</param>
		/// <param name="progID">Logival program ID.</param>
		/// <param name="applicationExecutablePath">Full filename of the executable.</param>
		/// <param name="applicationProductName">Application product name.</param>
		/// <param name="typeDisplayName">User friendly description.</param>
		/// <exception cref="IOUtilitiesException">Thrown if the operation fails.</exception>
		public static void RegisterFileType(
			string fileExtension,
			string progID,
			string applicationExecutablePath,
			string applicationProductName,
			string typeDisplayName)
		{
			try
			{
				string s = String.Format(CultureInfo.InvariantCulture, ".{0}", fileExtension);

				// Register custom extension with the shell
				using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(s))
				{
					// Map custom  extension to a ProgID
					key.SetValue(null, progID);
				}

				// create ProgID key with display name
				using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progID)) 
				{
					key.SetValue(null, typeDisplayName);
				}

				// register icon
				using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(progID + @"\DefaultIcon")) 
				{
					key.SetValue(null, applicationExecutablePath + ",0");
				}

				// Register open command with the shell
				string cmdkey = progID + @"\shell\open\command";
				using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(cmdkey)) 
				{
					// Map ProgID to an Open action for the shell
					key.SetValue(null, applicationExecutablePath + " \"%1\"");
				}

				// Register application for "Open With" dialog
				string appkey = "Applications\\" + new FileInfo(applicationExecutablePath).Name + "\\shell";
				using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(appkey) ) 
				{
					key.SetValue("FriendlyCache", applicationProductName);
				}
			}
			catch (Exception exc)
			{
				/* potential exceptions:
				 * 
				 * ArgumentNullException
				 * SecurityException
				 * ArgumentException
				 * ObjectDisposedException
				 * UnauthorizedAccessException
				 */				
				throw new IOUtilitiesException("RegisterFileType exception", exc);
			}
		}
	}


	/// <summary>
	/// Exception which identifies an erro which occured in th IOUtilities class.
	/// </summary>
	public class IOUtilitiesException : MulhollandException
	{

		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		public IOUtilitiesException() : base () {}


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="message">Message exception.</param>
		public IOUtilitiesException(string message) : base(message) {}


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="innerException">Inner exception.</param>
		public IOUtilitiesException(Exception innerException) : base("IOUtilities exception.", innerException) {}		


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="message">Exception message.</param>
		/// <param name="innerException">Inner exception.</param>
		public IOUtilitiesException(string message, Exception innerException) : base(message, innerException) {}		
	}
}
