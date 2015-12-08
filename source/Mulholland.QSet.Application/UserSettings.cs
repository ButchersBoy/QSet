using System;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using Mulholland.QSet.Resources;
using Mulholland.Core;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Contains user settings.
	/// </summary>
	public class UserSettings
	{
		private bool _showQSetExplorerWindow = true;
		private bool _showPropertiesWindow = true;
		private bool _showMessageViewerWindow = true;
		private bool _showQSetMonitorWindow = true;
		private StringCollection _messageBrowserColumnListCollection = null;
		private StringCollection _recentFileList = new StringCollection();		
		private int _recentFileListMaximumEntries = -1;		

		/// <summary>
		/// Consructs the UserSettings class.
		/// </summary>
		public UserSettings() {}


		/// <summary>
		/// Creates a <see cref="UserSettings"/> object, loading from settings the default location.
		/// </summary>
		/// <returns></returns>
		public static UserSettings Create()
		{
			UserSettings userSettings = null;
			FileStream stream = null;
			
			try
			{
				CheckSettingsForCurrentVersion();

				XmlSerializer xmlSerializer = new XmlSerializer(typeof(UserSettings));
				FileInfo fi = new FileInfo(SettingsFile);			
				if(fi.Exists)
				{
					stream = fi.OpenRead();
					userSettings = (UserSettings)xmlSerializer.Deserialize(stream);										
				}
			}
			catch {}
			finally
			{
				if (stream != null)
					stream.Close();
			}

			if (userSettings == null)
				userSettings = new UserSettings();

			userSettings.SetDefaults();

			return userSettings;
		}


		/// <summary>
		/// Imports settings from previous builds into the settings storage location for this build, if required.
		/// </summary>
		private static void CheckSettingsForCurrentVersion()
		{
			if (!File.Exists(SettingsFile))
			{
				string parentAppDataPath = System.Windows.Forms.Application.UserAppDataPath.Substring(0, System.Windows.Forms.Application.UserAppDataPath.LastIndexOf("\\"));				
				string[] siblingDirectories = Directory.GetDirectories(parentAppDataPath);
				if (siblingDirectories.Length >= 2)
				{
					string previousSettingsFile = siblingDirectories[siblingDirectories.Length - 2] + "\\" + Constants.SettingsFile;
					if (File.Exists(previousSettingsFile))
						File.Copy(previousSettingsFile, SettingsFile);					
				}
			}
		}


		/// <summary>
		/// Gets the name of the settings file for the user.
		/// </summary>
		protected static string SettingsFile
		{
			get
			{
				return string.Format("{0}\\{1}", System.Windows.Forms.Application.UserAppDataPath, Constants.SettingsFile);
			}
		}


		/// <summary>
		/// Attempts to save the user settings file.
		/// </summary>
		public void Save()
		{
			//TODO only save if required

			StreamWriter streamWriter = null;
			XmlSerializer xmlSerializer = null;
			try
			{
				xmlSerializer = new XmlSerializer(typeof(UserSettings));
				streamWriter = new StreamWriter(SettingsFile, false);
				xmlSerializer.Serialize(streamWriter, this);
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message); 
			}
			finally
			{				
				if(streamWriter != null)				
					streamWriter.Close();
			}
		}


		/// <summary>
		/// Gets or sets the flag indicating if the user has the Q Set Explorer displayed.
		/// </summary>
		public bool ShowQSetExplorerWindow
		{
			get
			{
				return _showQSetExplorerWindow;
			}
			set
			{
				_showQSetExplorerWindow = value;
			}
		}


		/// <summary>
		/// Gets or sets the flag indicating if the user has the properties window displayed.
		/// </summary>
		public bool ShowPropertiesWindow
		{
			get
			{
				return _showPropertiesWindow;
			}
			set
			{
				_showPropertiesWindow = value;
			}
		}
		
		
		/// <summary>
		/// Gets or sets the flag indicating if the user has the Message Viewer displayed.
		/// </summary>
		public bool ShowMessageViewerWindow
		{
			get
			{
				return _showMessageViewerWindow;
			}
			set
			{
				_showMessageViewerWindow = value;
			}
		}


		/// <summary>
		/// Gets or sets the flag indicating if the user has the Q Set Monitor displayed.
		/// </summary>
		public bool ShowQSetMonitorWindow
		{
			get
			{
				return _showQSetMonitorWindow;
			}
			set
			{
				_showQSetMonitorWindow = value;
			}
		}


		/// <summary>
		/// Inserts a file into the <see cref="RecentFileList"/>, or moves it to the top of the list if it already exists.
		/// </summary>
		/// <param name="path">Full path & filename of file.</param>
		public void IndicateFileUsed(string path)
		{
			if (path != null)
			{
				if (_recentFileList.Contains(path))
					_recentFileList.Remove(path);
			
				_recentFileList.Insert(0, path);			

				if (_recentFileList.Count > _recentFileListMaximumEntries)
					_recentFileList.RemoveAt(_recentFileList.Count - 1);
			}
		}


		/// <summary>
		/// The maximum number of entries allowed in the recent file list.
		/// </summary>
		public int RecentFileListMaximumEntries
		{
			get
			{
				return _recentFileListMaximumEntries;
			}
			set
			{
				_recentFileListMaximumEntries = value;
			}
		}


		/// <summary>
		/// Updates a filename in the <see cref="RecentFileList"/>.
		/// </summary>
		/// <param name="oldPath">Full path & filename of the old file name.</param>
		/// <param name="newPath">Full path & filename of the new file name.</param>
		public void IndicateFileRenamed(string oldPath, string newPath)
		{
			if (_recentFileList.Contains(oldPath))
				_recentFileList.Remove(oldPath);			
			
			IndicateFileUsed(newPath);
		}				


		/// <summary>
		/// Returns a list of Message properties which should be displayed as columns 
		/// in the message browser, ordered as they should be displayed.
		/// </summary>
		public StringCollection MessageBrowserColumnListCollection
		{
			get 
			{
				//default column list if necessary
				if (_messageBrowserColumnListCollection == null)				
					_messageBrowserColumnListCollection = new StringCollection();

				return _messageBrowserColumnListCollection;
			}
		}		


		/// <summary>
		/// Sets any defauls where required.
		/// </summary>
		public void SetDefaults()
		{
			if (RecentFileListMaximumEntries == -1) RecentFileListMaximumEntries = 4;
			
			if (MessageBrowserColumnListCollection.Count == 0)
			{
				MessageBrowserColumnListCollection.Add("ArrivedTime");					
				MessageBrowserColumnListCollection.Add("Label");
				MessageBrowserColumnListCollection.Add("Id");				
			}
		}
		

		/// <summary>
		/// Gets a list of the users most recently used files.
		/// </summary>
		public StringCollection RecentFileList
		{
			get
			{
				return _recentFileList;
			}
		}


		/// <summary>
		/// Checks that all required file asssociates are set up, and configures them if required.
		/// </summary>
		public void CheckFileAssociations()
		{			
			try
			{
				IOUtilities.RegisterFileType(
					Constants.QSetFileExtension, 
					Constants.QSetFileExtension + "file", 
					System.Windows.Forms.Application.ExecutablePath, 
					System.Windows.Forms.Application.ProductName, 
					System.Windows.Forms.Application.ProductName + " File");
			}
			catch {}
		}

	}

}
