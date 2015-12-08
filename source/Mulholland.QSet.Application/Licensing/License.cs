using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Mulholland.Core;

namespace Mulholland.QSet.Application.Licensing
{
	/// <summary>
	/// Maintains and edits the applications license.
	/// </summary>
	public class License
	{
		#region Read me

		/*
			Sample, decrypted license file:
			
			<license user="neil.armstrong@themoon.biz" productKey="ABC123DEF456" /> 	
		
			License is save to disk, encrypted, with encryption keyed on the machine 
			name, so it cannot be transferred without the user having to re-enter
			
			License is stored in the standard users application data directory.
		*/

		#endregion

		// constants
		private const string _READABLE_HASH_CHARS = "0123456789ABCDEFGHJKLMNPQRTUVWXYZ";
		private const string _APPLICATION_KEY = "File Q Set Help About 1";
		private const int _ACTIVATION_KEY_LENGTH = 16; //always 16 as this is the fixed length of an MD5 hash
		private const string _XML_LICENSE_NODE = "license";
		private const string _XML_REGISTRATION_ATTRIBUTE = "email";
		private const string _XML_ACTIVATIONKEY_ATTRIBUTE = "key";
	
		//license details
		string _registrationEmail = null;
		string _activationKey = null;
		private bool _isLicenseValid = false;
		private bool[] _featureStates = new bool[Enum.GetValues(typeof(Feature)).Length];

		/// <summary>
		/// Constructs the license object.
		/// </summary>
		public License() 
		{			
			//update the license state
			UpdateLicenseState();
		}


		/// <summary>
		/// Gets a flag indicating whether the user has a valid license.
		/// </summary>
		public bool IsLicenseValid
		{
			get
			{
				return true;
			}
		}


		/// <summary>
		/// Returns wether a users license allows them access to a particular feature.
		/// </summary>
		/// <param name="feature"></param>
		/// <returns></returns>
		public bool IsFeatureEnabled(Feature feature)
		{
			return _featureStates[(int)feature];
		}


		/// <summary>
		/// Displays a form for the user to edit the current license.
		/// </summary>
		/// <exception cref="LicenseSaveException">Occurs when the users inputs a valid license, but the license could not be saved to disk.</exception>
		public void EditLicense()
		{
			LicenseForm licenseForm = null;
			
			try
			{
				licenseForm = new LicenseForm(new LicenseForm.IsActivationKeyValid(IsActivationKeyValid));

				if (licenseForm.ShowDialog() == DialogResult.OK)
				{
					RegistrationEmail = licenseForm.RegistrationEmail;
					ActivationKey = licenseForm.ActivationKey;
					UpdateLicenseState();			
					PersistLicenseToDisk();
				}
			}
			finally
			{
				if (licenseForm != null)
					licenseForm.Dispose();
			}
		}


		/// <summary>
		/// Checks to see if the license if valid for a feature.  If it is not the <see cref="LicenseAboutForm"/> 
		/// is displayed to the user.
		/// </summary>
		/// <param name="feature">Feature to validate.</param>
		/// <returns>True if the license is valid for the feature, else false.</returns>
		public bool ValidateFeatureUse(Feature feature)
		{
			bool result = IsFeatureEnabled(feature);
			
			if (!result)
			{
				LicenseAboutForm licenseAboutForm = new LicenseAboutForm();
				licenseAboutForm.ShowDialog();
				licenseAboutForm.Dispose();
			}

			return result;
		}


		/// <summary>
		/// Saves the license to disk.
		/// </summary>
		protected void PersistLicenseToDisk()
		{
			MemoryStream ms = null;

			try
			{
				//create the license XML
				ms = new MemoryStream();
				XmlTextWriter tw = new XmlTextWriter(ms, System.Text.Encoding.UTF8);				
				tw.WriteStartElement(_XML_LICENSE_NODE);
				tw.WriteAttributeString(_XML_REGISTRATION_ATTRIBUTE, RegistrationEmail);
				tw.WriteAttributeString(_XML_ACTIVATIONKEY_ATTRIBUTE, ActivationKey);
				tw.WriteEndElement();
				tw.Flush();

				//encrypt the XML
				ms.Position = 0;
				StreamReader sr = new StreamReader(ms);
				Cryptographer crypto = new Cryptographer();
				string license = crypto.Encrypt(sr.ReadToEnd(), SystemInformation.ComputerName);

				//write the license to disk
				StreamWriter sw = new StreamWriter(LicensePath, false, System.Text.Encoding.UTF8);
				sw.Write(license);
				sw.Flush();
			}
			catch (Exception exc)
			{
				throw new LicenseSaveException(exc);
			}
			finally
			{
				if (ms != null)
					ms.Close();
			}
		}


		/// <summary>
		/// Gets or sets the users registration email address.
		/// </summary>
		protected string RegistrationEmail
		{
			get
			{
				return _registrationEmail;
			}
			set
			{
				_registrationEmail = value;
			}
		}


		/// <summary>
		/// Gets or sets the users activation key.
		/// </summary>
		protected string ActivationKey
		{
			get
			{
				return _activationKey;
			}
			set
			{
				_activationKey = value;
			}
		}


		/// <summary>
		/// Updates the state of the license according to the current registration email and activation key.
		/// </summary>
		protected void UpdateLicenseState()
		{
			_featureStates[(int)Feature.StartUp] = true;
			_featureStates[(int)Feature.NewMessage] = true;
			_featureStates[(int)Feature.FowardMessage] = true;
			_featureStates[(int)Feature.SaveMessage] = true;
			_featureStates[(int)Feature.DragAndDropMessage] = true;
			_featureStates[(int)Feature.DeleteMessage] = true;
			_featureStates[(int)Feature.SaveQSet] = true;			
		}


		/// <summary>
		/// Checks to see if a registration email and activation key pair are valid.
		/// </summary>
		/// <param name="registrationEmail">Users registration email address.</param>
		/// <param name="activationKey">Users activation key.</param>
		/// <returns>True if the activation key is valid, else false.</returns>
		protected bool IsActivationKeyValid(string registrationEmail, string activationKey)
		{         
			return GenerateActivationKey(registrationEmail).Equals(activationKey, StringComparison.Ordinal);
		}


		/// <summary>
		/// Generates the activation key for a given email address.
		/// </summary>
		/// <param name="registrationName"></param>
		/// <returns></returns>
		protected string GenerateActivationKey(string registrationName)
		{
			Cryptographer crypto = new Cryptographer();
			string keyHash = crypto.Hash(registrationName + _APPLICATION_KEY);	
			
			System.Text.StringBuilder sb = new System.Text.StringBuilder(_ACTIVATION_KEY_LENGTH);
			foreach (char c in keyHash)
				sb.Append(_READABLE_HASH_CHARS[Convert.ToInt32((int)c / ((float)127 / (float)32))]);

			return sb.ToString();
		}


		/// <summary>
		/// Gets the path of the license file.
		/// </summary>
		protected string LicensePath
		{
			get
			{
				return string.Format("{0}\\{1}", System.Windows.Forms.Application.CommonAppDataPath, Resources.Constants.LicenseFile);
			}
		}


		/// <summary>
		/// Imports license from previous builds into the license storage location for this build, if required.
		/// </summary>
		private void CheckLicenseForCurrentVersion()
		{			
			//if we dont have a license we will look for one belonging to a previous version
			if (!File.Exists(LicensePath))
			{				
				//get license from the previous version
				string sourceLicensePath = System.Windows.Forms.Application.CommonAppDataPath;				
				sourceLicensePath = GetLatestLicensePath(sourceLicensePath.Substring(0, sourceLicensePath.LastIndexOf("\\")), true, null);
				if (sourceLicensePath == null)
				{
					//if we cant find it, check for version 1.0 which was held in user section as opposed to common section
					Version version = new Version(System.Windows.Forms.Application.ProductVersion);
					if (version.Major == 1 && version.Minor == 1)
					{
						sourceLicensePath = System.Windows.Forms.Application.LocalUserAppDataPath;				
						sourceLicensePath = GetLatestLicensePath(sourceLicensePath.Substring(0, sourceLicensePath.LastIndexOf("\\")), false, "1.0.*");
					}
				}

				//import the previous license
				if (sourceLicensePath != null)
					File.Copy(sourceLicensePath, LicensePath);
			}
		}


		/// <summary>
		/// Gets the path of the latest license under a parent directory.
		/// </summary>
		/// <param name="parentDirectory">Parent settings path.</param>
		/// <param name="previous">Set to true to look for the previous version, false for the latest/current.</param>
		/// <param name="pattern">Patter to filter sub directories by.  Use null if not required.</param>
		/// <returns>Path if available, else null.</returns>
		private string GetLatestLicensePath(string parentDirectory, bool previous, string pattern)
		{
			string path = null;

			int offset = previous ? 2 : 1;
			string[] versionDirectories;
			if (pattern == null)
				versionDirectories = Directory.GetDirectories(parentDirectory);
			else
				versionDirectories = Directory.GetDirectories(parentDirectory, pattern);			
			if (versionDirectories.Length >= offset)
			{
				path = versionDirectories[versionDirectories.Length - offset] + "\\" + Resources.Constants.LicenseFile;
				if (!File.Exists(path))
					path = null;
			}

			return path;
		}

	}


	/// <summary>
	/// Signifies that a license could not be persisted to disk.
	/// </summary>
	internal class LicenseSaveException : Exception
	{
		/// <summary>
		/// Constructs the exception.
		/// </summary>
		/// <param name="innerException">Exception which caused the save failure.</param>
		public LicenseSaveException(Exception innerException)
			: base ("Unable to save the license to disk.", innerException)
		{}
	}
}
