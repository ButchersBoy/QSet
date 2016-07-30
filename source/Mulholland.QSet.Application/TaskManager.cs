using System;
using System.IO;
using System.Messaging;
using System.Reflection;
using System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using Mulholland.Core;
using Mulholland.Core.Xml;
using Mulholland.WinForms;
using TD.SandDock;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Implements all user drive tasks. 
	/// </summary>
	internal class TaskManager
	{		
		private PrimaryMenus _primaryMenus;
		private PrimaryControls _primaryControls;
		private PrimaryObjects _primaryObjects;
		private PrimaryForms _primaryForms;
		private MenuStateManager _menuStateManager;		
		private QueueTaskManager _queueTaskManager;
		private WebTaskManager _webTaskManager;
		private bool _hasShutDown = false;
	
		/// <summary>
		/// Constructs the object with the minum requirements.
		/// </summary>
		/// <param name="primaryMenus">Primary menus.</param>
		/// <param name="primaryControls">Primary controls.</param>
		/// <param name="primaryForms">Primary forms.</param>
		/// <param name="primaryObjects">Primary objects.</param>
		public TaskManager(
			PrimaryMenus primaryMenus, 
			PrimaryControls primaryControls,
			PrimaryForms primaryForms,
			PrimaryObjects primaryObjects)
		{
			_primaryMenus = primaryMenus;
			_primaryControls = primaryControls;			
			_primaryForms = primaryForms;
			_primaryObjects = primaryObjects;
			
			_menuStateManager = new MenuStateManager(_primaryControls);			
			_queueTaskManager = new QueueTaskManager(this, _primaryControls, _primaryObjects, _primaryForms);
			_webTaskManager = new WebTaskManager(this, _primaryControls, _primaryObjects, _primaryForms);
			
			primaryControls.QSetExplorer.ImageList = primaryControls.Images.Icon16ImageList;
			primaryControls.QSetMonitor.ImageList = primaryControls.Images.Icon16ImageList;
		}


		/// <summary>
		/// Gets state manager object for menus.
		/// </summary>
		public MenuStateManager MenuStateManger
		{
			get
			{
				return _menuStateManager;
			}
		}


		/// <summary>
		/// Gets manager class for working with queues
		/// </summary>
		public QueueTaskManager QueueTaskManager
		{
			get
			{
				return _queueTaskManager;
			}
		}


		/// <summary>
		/// Gets the manager class which provides web tools.
		/// </summary>
		public WebTaskManager WebTaskManager
		{
			get
			{
				return _webTaskManager;
			}
		}


		/// <summary>
		/// Creates a new Q Set.
		/// </summary>
		public void CreateNewQSet()
		{			
			_primaryControls.QSetExplorer.QSet = new QSetModel("New Queue Set");						
			_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.QSet;
			_primaryControls.QSetExplorer.BeginEditActiveItem();			
		}


		/// <summary>
		/// Adds the currently active queue to the Q Set.
		/// </summary>
		public void AddActiveQueueToQSet()
		{			
			if (_primaryControls.QSetExplorer.QSet != null)
			{
				if (_primaryControls.QSetExplorer.ActiveItem == null)
					_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.QSet;

                if (_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument != null)
				{
                    MessageBrowser messageBrowser = _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0] as MessageBrowser;			
					if (messageBrowser != null)
					{
						QSetFolderItem folderItem = _primaryControls.QSetExplorer.ActiveItem as QSetFolderItem;
						if (folderItem != null)
						{
							if (!folderItem.ChildItems.Exists(messageBrowser.QSetQueueItem.Name))
								folderItem.ChildItems.Add(messageBrowser.QSetQueueItem);												
						}
						else
							MessageBox.Show(_primaryForms.EnvironmentForm, "Cannot add the queue at this point.  Select a valid folder, or the top level Queue Set to add the queue.", Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
				}
			}
		}



		/// <summary>
		/// Purges the queue which is currently active in the focused MessageBrowser.
		/// </summary>
		public void PurgeActiveQueue()
		{
            if (_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument != null)
			{
                MessageBrowser messageBrowser = _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0] as MessageBrowser;			
				if (messageBrowser != null)
				{
					QueueTaskManager.PurgeQueue(messageBrowser.QSetQueueItem);					
				}
			}
		}


		/// <summary>
		/// Purges the queue which is currently selected in the QSetExplorer.
		/// </summary>
		public void PurgeActiveQSetExplorerQueue()
		{
			if (_primaryControls.QSetExplorer.ActiveItem != null)
			{
				QSetQueueItem queueItem = _primaryControls.QSetExplorer.ActiveItem as QSetQueueItem;
				if (queueItem != null)
					QueueTaskManager.PurgeQueue(queueItem);
			}
		}


		/// <summary>
		/// Adds a new folder to the Q Set.
		/// </summary>
		public void AddNewFolderToQSet()
		{
			if (_primaryControls.QSetExplorer.ActiveItem != null)
			{
				QSetFolderItem folderItem = _primaryControls.QSetExplorer.ActiveItem as QSetFolderItem;
				if (folderItem != null)
				{
					QSetFolderItem newFolderItem = new QSetFolderItem(GetNextAvailableNewItemName("New Folder", folderItem.ChildItems));
					folderItem.ChildItems.Add(newFolderItem);							
					_primaryControls.QSetExplorer.BeginEditActiveItem();
				}
			}
		}


		/// <summary>
		/// Opens a machine and all of its queues, adding to the active Q Set if possible.
		/// </summary>
		/// <param name="machineName">Name of machine.</param>
		/// <param name="messageQueues">Array of message queues belonging to the machine.</param>
		public void OpenMachine(string machineName, MessageQueue[] messageQueues)
		{
			if (_primaryControls.QSetExplorer.QSet != null)
			{
				//ensure we have a folder item to add to
				if (_primaryControls.QSetExplorer.ActiveItem == null)
					_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.QSet;
				if (!(_primaryControls.QSetExplorer.ActiveItem is QSetFolderItem))
					_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.ActiveItem.ParentItem;
	
				//check the item does not already exist
				QSetFolderItem parentItem = (QSetFolderItem)_primaryControls.QSetExplorer.ActiveItem;
				if (!parentItem.ChildItems.Exists(machineName))
				{
					QSetMachineItem newMachineItem = new QSetMachineItem(machineName);
					foreach (MessageQueue queue in messageQueues)
						newMachineItem.ChildItems.Add(new QSetQueueItem(string.Format(@"{0}\{1}", queue.MachineName, queue.QueueName))); //reformat as private queues can come out with extra data in name)					
					parentItem.ChildItems.Add(newMachineItem);					
				}
				else
					MessageBox.Show(Locale.UserMessages.CannotAddItemAsAlreadyExists, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else			
				foreach (MessageQueue queue in messageQueues)
					OpenQueue(new QSetQueueItem(string.Format(@"{0}\{1}", queue.MachineName, queue.QueueName))); //reformat as private queues can come out with extra data in name)
		}

                /// <summary>
        /// Deletes the currently active item from the Q Set.
        /// </summary>
        public void DeleteActiveItemFromQSet()
		{
			if (_primaryControls.QSetExplorer.ActiveItem != null && !(_primaryControls.QSetExplorer.ActiveItem is QSetModel))
			{
				string msg = null;
				if (_primaryControls.QSetExplorer.ActiveItem is QSetFolderItem)
				{
					if (((QSetFolderItem)_primaryControls.QSetExplorer.ActiveItem).ChildItems != null && ((QSetFolderItem)_primaryControls.QSetExplorer.ActiveItem).ChildItems.Count > 0)
					{
						msg = "the folder '{0}' and all of its contents";
					}
					else
					{
						msg = "the folder '{0}'";
					}
				}
				else
					msg = "the queue '{0}'";

				msg = string.Format(msg, _primaryControls.QSetExplorer.ActiveItem.Name);
				msg = string.Format("Are you sure you wish to remove {0} from the Q Set?", msg);

				if (MessageBox.Show(_primaryForms.EnvironmentForm, msg, Locale.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
				{
					((QSetFolderItem)_primaryControls.QSetExplorer.ActiveItem.ParentItem).ChildItems.Remove(_primaryControls.QSetExplorer.ActiveItem.Name);					
				}
			}
		}

        public void PurgeAllQueuesFromQSet()
        {
            if (_primaryControls.QSetExplorer.ActiveItem != null && !(_primaryControls.QSetExplorer.ActiveItem is QSetModel))
            {
                var msg = "Are you sure you wish to purge all queues from the selected machine?";

                if (MessageBox.Show(_primaryForms.EnvironmentForm, msg, Locale.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    foreach(QSetQueueItem item in ((QSetFolderItem)_primaryControls.QSetExplorer.ActiveItem).ChildItems)
                    {
                        QueueTaskManager.PurgeQueueWithoutPrompt(item);
                    }
                }
            }
        }

        /// <summary>
        /// Prompts the user to select a queue, and then opens the queue in the environment.
        /// </summary>
        public void OpenQueue()
		{
			using (OpenQueueDialog selectQueueForm = new OpenQueueDialog())
			{				
				selectQueueForm.Owner = _primaryForms.EnvironmentForm;
				selectQueueForm.StartPosition = FormStartPosition.CenterParent;
				if (selectQueueForm.ShowDialog() == DialogResult.OK)
				{										
					OpenQueue(new QSetQueueItem(selectQueueForm.QueueName));											
				}
			}

			_menuStateManager.SetQSetMenuState();
		}


		/// <summary>
		/// Opens a new queue in the environment for browsing.
		/// </summary>
		/// <param name="queueName">QSetQueueItem which contains the queue to be opened.</param>
		public void OpenQueue(QSetQueueItem qsetQueueItem)
		{			
			if (!IsItemOpen(qsetQueueItem))			
				LoadNewMessageBrowser(qsetQueueItem);	
			else
				BringDocumentToFront(qsetQueueItem);

			if (qsetQueueItem.ParentItem == null && _primaryControls.QSetExplorer.QSet != null)
				AddActiveQueueToQSet();
		}


		/// <summary>
		/// Saves the current Queue Set.
		/// </summary>
		/// <returns>true if the save was successful, else false.</returns>
		public bool SaveQSet()
		{
			bool result = false;

			//validate license
			if (_primaryObjects.License.ValidateFeatureUse(Licensing.Feature.SaveQSet))
			{
				if (_primaryControls.QSetExplorer.QSet != null)
					if (_primaryControls.QSetExplorer.QSet.FileName == null)				
						result = SaveQSetAs();
					
					else				
					{
						result = DoSaveQSet(_primaryControls.QSetExplorer.QSet);
						_primaryObjects.UserSettings.IndicateFileUsed(_primaryControls.QSetExplorer.QSet.FileName);	
						_primaryMenus.RefreshRecentFilesList(_primaryObjects.UserSettings.RecentFileList, _primaryObjects.UserSettings.RecentFileListMaximumEntries);
					}
			}

			return result;
		}

		/// <summary>
		/// Saves the current Queue Set, prompting the user for a filename.
		/// </summary>
		/// <returns>true if the save was successful, else false.</returns>
		public bool SaveQSetAs()
		{
			bool result = false;

			//validate license
			if (_primaryObjects.License.ValidateFeatureUse(Licensing.Feature.DragAndDropMessage))
			{

				if (_primaryControls.QSetExplorer.QSet != null)
					using (SaveFileDialog saveDialog = new SaveFileDialog())
					{					
						saveDialog.Title = "Save Q Set";
						saveDialog.DefaultExt = "qset";
						saveDialog.Filter = Locale.FileDialogQSetFilter;
						saveDialog.InitialDirectory = "\\My Documents";
						if (saveDialog.ShowDialog() == DialogResult.OK)
						{					
							string oldFileName = _primaryControls.QSetExplorer.QSet.FileName;
							_primaryControls.QSetExplorer.QSet.FileName = saveDialog.FileName;
							result = DoSaveQSet(_primaryControls.QSetExplorer.QSet);

							//refresh relevant interface components
							if (oldFileName == null)
								_primaryObjects.UserSettings.IndicateFileUsed(_primaryControls.QSetExplorer.QSet.FileName);												
							else
								_primaryObjects.UserSettings.IndicateFileRenamed(oldFileName, _primaryControls.QSetExplorer.QSet.FileName);
							_primaryMenus.RefreshRecentFilesList(_primaryObjects.UserSettings.RecentFileList, _primaryObjects.UserSettings.RecentFileListMaximumEntries);
						}
					}	
			}

			return result;
		}


		/// <summary>
		/// Attempts to load the specified Q Set.
		/// </summary>
		/// <param name="path">Full path & filename to load.</param>
		public void OpenQSet(string path)
		{
			//check if we need to save the current Q Set before we open a new one
			if (ConfirmQSetClose())
			{
				DoOpenQSet(path);
			}
		}


		/// <summary>
		/// Prompts the user to select a Q Set, and loads it.
		/// </summary>
		public void OpenQSet()
		{
			//check if we need to save the current Q Set before we open a new one
			if (ConfirmQSetClose())
			{
				//open new Q Set
				using (OpenFileDialog openDialog = new OpenFileDialog())
				{
					//prompt user to select file
					openDialog.Title = "Open Q Set";
					openDialog.DefaultExt = "qset";
					openDialog.Filter = Locale.FileDialogQSetFilter;
					openDialog.InitialDirectory = "\\My Documents";
					if (openDialog.ShowDialog() == DialogResult.OK)
					{
						DoOpenQSet(openDialog.FileName);
					}
				}
			}
						
		}


		/// <summary>
		/// Attempts to close the active Q Set, prompting the user to save if necessary.
		/// </summary>
		public void CloseQSet()
		{
			if (ConfirmQSetClose())
				DoCloseQSet();
								
			_menuStateManager.SetFileMenuState();
		}


		/// <summary>
		/// Edits the name of the active Q Set folder
		/// </summary>
		public void EditActiveQSetFolder()
		{
			if (_primaryControls.QSetExplorer.ActiveItem != null && (_primaryControls.QSetExplorer.ActiveItem is QSetFolderItem || _primaryControls.QSetExplorer.ActiveItem is QSetWebServiceItem))
				_primaryControls.QSetExplorer.BeginEditActiveItem();			
		}


		/// <summary>
		/// Sets the text of the applications main title bar.
		/// </summary>
		public void SetTitleBarText()
		{
			string applicationName = Locale.ApplicationName;
			if (!_primaryObjects.License.IsLicenseValid)
				applicationName += " (UNLICENSED)";

			if (_primaryControls.QSetExplorer.QSet == null)
				_primaryForms.EnvironmentForm.Text = applicationName;
			else if (_primaryControls.QSetExplorer.QSet.FileName == null)
				_primaryForms.EnvironmentForm.Text = string.Format("{0} - {1}", applicationName, _primaryControls.QSetExplorer.QSet.Name);
			else
				_primaryForms.EnvironmentForm.Text = string.Format("{0} - {1}", applicationName, _primaryControls.QSetExplorer.QSet.FileName);							
		}


		/// <summary>
		/// Shuts the environment down.
		/// </summary>
		public bool ShutDown()
		{
			bool result = false;

			if (ConfirmQSetClose())
			{
				_hasShutDown = true;
				result = true;
				_primaryForms.QueueSearchForm.Dispose();
				_primaryObjects.UserSettings.Save();
				_primaryForms.EnvironmentForm.Close();				
			}

			return result;
		}


		/// <summary>
		/// Gets  whether the environment has completed all required tasks required before shut down.
		/// </summary>
		public bool HasShutDown
		{
			get
			{
				return _hasShutDown;
			}
		}


		/// <summary>
		/// Displays the queue search dialog.
		/// </summary>
		public void BrosweForQueue()
		{
			if (_primaryControls.QSetExplorer.QSet != null &&
				_primaryControls.QSetExplorer.ActiveItem == null)
				_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.QSet;

			if (_primaryControls.QSetExplorer.QSet != null &&
				_primaryControls.QSetExplorer.ActiveItem is QSetQueueItem)
				_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.ActiveItem.ParentItem;
			
			if (_primaryForms.QueueSearchForm.Visible == false)
				_primaryForms.QueueSearchForm.Show();
			else
				_primaryForms.QueueSearchForm.Activate();
		}


		/// <summary>
		/// Displays the options form to the user.
		/// </summary>
		public void DisplayOptions()
		{
			OptionsDialog optionsDialog = new OptionsDialog();
			optionsDialog.SmallImageList = _primaryControls.Images.Icon16ImageList;
			if (optionsDialog.ShowDialog(_primaryForms.EnvironmentForm, _primaryObjects.UserSettings) == DialogResult.OK)
			{
				_primaryMenus.RefreshRecentFilesList(_primaryObjects.UserSettings.RecentFileList, _primaryObjects.UserSettings.RecentFileListMaximumEntries);
			}
		}
		

		/// <summary>
		/// Displays the applications About box.
		/// </summary>
		public void ShowAboutBox()
		{
			AboutForm aboutForm = new AboutForm();
			aboutForm.ShowDialog(_primaryForms.EnvironmentForm);
			aboutForm.Dispose();
		}


		/// <summary>
		/// Configures the environment according to the user settings.
		/// </summary>
		public void ConfigureEnvironmentAcccordingToUserSettings()
		{
			TogglePropertiesWindow(_primaryObjects.UserSettings.ShowPropertiesWindow);
			ToggleQSetExplorer(_primaryObjects.UserSettings.ShowQSetExplorerWindow);
			ToggleQSetMonitor(_primaryObjects.UserSettings.ShowQSetMonitorWindow);
			ToggleMessageViewer(_primaryObjects.UserSettings.ShowMessageViewerWindow);			

			_primaryMenus.RefreshRecentFilesList(_primaryObjects.UserSettings.RecentFileList, _primaryObjects.UserSettings.RecentFileListMaximumEntries);
		}


		/// <summary>
		/// Hides or shows the Q Set Explorer.
		/// </summary>
		/// <param name="newState">true to show the control, else false.</param>
		public void ToggleQSetExplorer(bool show)
		{
			DockControl parentDockControl = _primaryControls.QSetExplorer.Parent as DockControl;

			if (parentDockControl != null)
			{
				if (show)
				{
					parentDockControl.Open();
					MenuItemBag.ViewQSetExplorer.Checked = true;
				}
				else
				{
					if (parentDockControl.IsOpen)
					{
						parentDockControl.Close();
						MenuItemBag.ViewQSetExplorer.Checked = false;
					}
				}
			}

			_primaryObjects.UserSettings.ShowQSetExplorerWindow = MenuItemBag.ViewQSetExplorer.Checked;
		}


		/// <summary>
		/// Hides or shows the Properties window.
		/// </summary>
		/// <param name="newState">true to show the control, else false.</param>
		public void TogglePropertiesWindow(bool show)
		{
			DockControl parentDockControl = _primaryControls.PropertyGrid.Parent as DockControl;

			if (parentDockControl != null)
			{
				if (show)
				{
					parentDockControl.Open();
					MenuItemBag.ViewProperties.Checked = true;
				}
				else
				{
					if (parentDockControl.IsOpen)
					{
						parentDockControl.Close();
						MenuItemBag.ViewProperties.Checked = false;
					}
				}
			}

			_primaryObjects.UserSettings.ShowPropertiesWindow = MenuItemBag.ViewProperties.Checked;
		}


		/// <summary>
		/// Hides or shows the Message Viewer.
		/// </summary>
		/// <param name="newState">true to show the control, else false.</param>
		public void ToggleMessageViewer(bool show)
		{
			DockControl parentDockControl = _primaryControls.MessageViewer.Parent as DockControl;

			if (parentDockControl != null)
			{
				if (show)
				{
					parentDockControl.Open();
					MenuItemBag.ViewMessageViewer.Checked = true;
				}
				else
				{
					if (parentDockControl.IsOpen)
					{
						parentDockControl.Close();
						MenuItemBag.ViewMessageViewer.Checked = false;
					}
				}
			}

			_primaryObjects.UserSettings.ShowMessageViewerWindow = MenuItemBag.ViewMessageViewer.Checked;
		}


		/// <summary>
		/// Hides or shows the Q Set Monitor.
		/// </summary>
		/// <param name="newState">true to show the control, else false.</param>
		public void ToggleQSetMonitor(bool show)
		{
			DockControl parentDockControl = _primaryControls.QSetMonitor.Parent as DockControl;
			
			if (parentDockControl != null)
			{
				if (show)
				{
					parentDockControl.Open();
					MenuItemBag.ViewQSetMonitor.Checked = true;
				}
				else
				{
					if (parentDockControl.IsOpen)
					{
						parentDockControl.Close();
						MenuItemBag.ViewQSetMonitor.Checked = false;
					}
				}
			}

			_primaryObjects.UserSettings.ShowQSetMonitorWindow = MenuItemBag.ViewQSetMonitor.Checked;
		}


		#region private members


		/// <summary>
		/// Checks if there is a current, dirty Q Set, and prompts for a save if necessary.
		/// </summary>
		/// <returns>true if it is OK to save the Q Set, else false.</returns>
		private bool ConfirmQSetClose()
		{			
			bool result = false;

			if (_primaryControls.QSetExplorer.QSet != null && _primaryControls.QSetExplorer.QSet.IsDirty)
			{
				string msg = string.Format("Do you wish to save changes to '{0}?'", _primaryControls.QSetExplorer.QSet.Name);
				DialogResult msgBoxResult = MessageBox.Show(_primaryForms.EnvironmentForm, msg, Locale.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				if (msgBoxResult == DialogResult.Yes)
					result = SaveQSet();
				else if (msgBoxResult == DialogResult.No)
					result = true;
			}
			else
				result = true;

			return result;
		}

		
		/// <summary>
		/// Closes the currently active Q Set
		/// </summary>
		private void DoCloseQSet()
		{
			foreach (DockControl dockControl in _primaryControls.DocumentContainer.Manager.Documents)
			{
				MessageBrowser messageBrowser = dockControl.Controls[0] as MessageBrowser;
				if (messageBrowser != null)
				{
					if (messageBrowser.QSetQueueItem.ParentItem != null)
					{
						_primaryControls.MessageBrowserCollection.Remove(messageBrowser.QSetQueueItem.ID.ToString());
						dockControl.Close();						
					}
				}
				else
				{
					WebServiceClientControl webServiceClientControl = dockControl.Controls[0] as WebServiceClientControl;
					if (webServiceClientControl != null)
					{
						_primaryControls.WebServiceClientControlCollection.Remove(webServiceClientControl.QSetItem.ID.ToString());
						dockControl.Close();
					}
				}
			}
			_primaryControls.QSetExplorer.QSet = null;
		}


		/// <summary>
		/// Saves a Q Set.
		/// </summary>
		/// <remarks>It is expected that the filename has been set at this point.</remarks>
		/// <param name="qSet">Q Set to save.</param>
		/// <param name="fileName">File name.</param>
		/// <returns>true if save was succeful, else false.</returns>
		private bool DoSaveQSet(QSetModel qSet)
		{
			bool result = false;

			try
			{				
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(qSet.FileName, false))
				{
					sw.Write(qSet.ToXml());
					sw.Flush();
					qSet.IsDirty = false;
					result = true;					
				}
			}					
			catch (Exception exc)
			{
				//TODO check if file is readonly & tidy up message box, handle IOException				
				MessageBox.Show(_primaryForms.EnvironmentForm, "Unable to save file, " + exc.Message, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			SetTitleBarText();
			_menuStateManager.SetFileMenuState();

			return result;
		}


		/// <summary>
		/// Loads the requested Q Set.
		/// </summary>
		/// <param name="file">Full path and filename of Q Set file.</param>
		private void DoOpenQSet(string file)
		{
			try
			{
				//read the file
				string qsetFileContent = null;
				using (StreamReader sr = new StreamReader(file))
				{
					qsetFileContent = sr.ReadToEnd();
				}

				//validate we have a valid Q Set file
				if (qsetFileContent != null)
				{
					Validator validator = new Validator(Documents.QSetFileXsd());
					if (validator.Validate(qsetFileContent, ValidatorShortCircuitType.OnWarning) == ValidatorResultType.Valid)
					{
						QSetModel qSet = QSetModel.CreateQSet(qsetFileContent);
						qSet.FileName = file;
						DoCloseQSet();
						_primaryControls.QSetExplorer.QSet = qSet;												
					}
					else
					{									
						MessageBox.Show(_primaryForms.EnvironmentForm, Locale.UserMessages.SelectedFileNotAQSetFile, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						OpenQSet();
					}
				}
			}
			catch 
			{							
				MessageBox.Show(_primaryForms.EnvironmentForm, Locale.UserMessages.UnableToOpenFile, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				OpenQSet();
			}
 
			//refresh relevant interface components
			_primaryObjects.UserSettings.IndicateFileUsed(file);
			_primaryMenus.RefreshRecentFilesList(_primaryObjects.UserSettings.RecentFileList, _primaryObjects.UserSettings.RecentFileListMaximumEntries);
			_menuStateManager.SetFileMenuState();
		}



		/// <summary>
		/// Checks to see if an item is currently open.
		/// </summary>
		/// <param name="item">Item to search for.</param>
		/// <returns>true if item is open, else false.</returns>
		public bool IsItemOpen(QSetItemBase item)
		{
			return _primaryControls.MessageBrowserCollection.Exists(item.ID.ToString()) || _primaryControls.WebServiceClientControlCollection.Exists(item.ID.ToString());
		}


		/// <summary>
		/// Given a requested item name, checks a collection where the new item will be added,
		/// to see if that name already exists.  If it does, a new unique name is generated.
		/// </summary>
		/// <param name="requestedName">Requested name for new folder.</param>
		/// <param name="items">Collection where folder is to be added</param>
		/// <returns>Un</returns>
		public string GetNextAvailableNewItemName(string requestedName, QSetItemCollection items)
		{
			int nextItemNumber = 0;			
			while (items.Exists(requestedName + (nextItemNumber == 0 ? "" : " " + nextItemNumber.ToString())) 
				&& nextItemNumber < int.MaxValue)
			{
				nextItemNumber ++;
			}

			if (items.Exists(requestedName + (nextItemNumber == 0 ? "" : " " + nextItemNumber.ToString())))
				return Guid.NewGuid().ToString();
			else
				return requestedName + (nextItemNumber == 0 ? "" : " " + nextItemNumber.ToString());
		}


		/// <summary>
		/// Brings a document to the front.
		/// </summary>
		/// <param name="item">Item which we want to activate.</param>
		public void BringDocumentToFront(QSetItemBase item)
		{
			DockControl dockControl = FindDocument(item);
			if (dockControl != null)
				dockControl.Activate();
		}


		/// <summary>
		/// Finds the docuemtn relating to a particular item.
		/// </summary>
		/// <param name="item">Item for which document we are looking for.</param>
		/// <returns>DockControl if found, else null.</returns>
		public DockControl FindDocument(QSetItemBase item)
		{
			DockControl result = null;

			foreach(DockControl dockControl in _primaryControls.DocumentContainer.Manager.Documents)				
			{
				IQSetItemControl itemControl = dockControl.Controls[0] as IQSetItemControl;
				if (itemControl != null && itemControl.QSetItem == item)
				{					
					result = dockControl;					
					break;
				}
			}

			return result;
		}


		/// <summary>
		/// Loads anew MessageBrowser, and displays in the main document window.
		/// </summary>
		/// <param name="qsetQueueItem">QSetQueueItem to display.</param>
		/// <returns>The added DockControl hosting the MessageBrowser if successful, else false.</returns>
		private DockControl LoadNewMessageBrowser(QSetQueueItem qsetQueueItem)
		{									
			DockControl newDockControl = null;

			//set up a new message browser, and create adock for it
			MessageBrowser messageBrowser = new MessageBrowser();
			messageBrowser.UserSettings = _primaryObjects.UserSettings;
			_primaryControls.MessageBrowserCollection.Add(qsetQueueItem.ID.ToString(), messageBrowser);
            newDockControl = new TabbedDocument(_primaryControls.DocumentContainer.Manager, messageBrowser, qsetQueueItem.Name);
            newDockControl.Open();		
			messageBrowser.ImageList = _primaryControls.Images.Icon16ImageList;
			
			//pass the qsetitem to the message browser to load the queue
			try
			{
				messageBrowser.QSetQueueItem = qsetQueueItem;
				newDockControl.Activate();
			}
			catch (Exception exc)
			{
				_primaryObjects.ProcessVisualizer.SeizeCursor(Cursors.Arrow);
				MessageBox.Show(exc.Message, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				_primaryObjects.ProcessVisualizer.ReleaseCursor();
				newDockControl.Close();
				newDockControl = null;
			}
					
			return newDockControl;
		}

		#endregion
	}
}


