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
using WeifenLuo.WinFormsUI.Docking;

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
            var qSet = new QSetModel("New Queue Set");
            _primaryControls.SetQSetExplorerActiveItemWithEdit(qSet);
        }


        /// <summary>
        /// Adds the currently active queue to the Q Set.
        /// </summary>
        public void AddActiveQueueToQSet()
        {			
            if (_primaryControls.GetQSetExplorerSet() != null)
            {
                if (_primaryControls.GetQSetExplorerActiveItem() == null)
                    _primaryControls.SetQSetExplorerActiveItem(_primaryControls.GetQSetExplorerSet());

                if (_primaryControls.HasActiveDocument)
                {
                    MessageBrowser messageBrowser = _primaryControls.GetActiveMessageBrowser();
                    if (messageBrowser != null)
                    {
                        QSetFolderItem folderItem = _primaryControls.GetQSetExplorerActiveItem() as QSetFolderItem;
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
            if (_primaryControls.HasActiveDocument)
            {
                MessageBrowser messageBrowser = _primaryControls.GetActiveMessageBrowser();
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
            if (_primaryControls.HasActiveDocument)
            {
                QSetQueueItem queueItem = _primaryControls.GetQSetExplorerActiveItem() as QSetQueueItem;
                if (queueItem != null)
                    QueueTaskManager.PurgeQueue(queueItem);
            }
        }


        /// <summary>
        /// Adds a new folder to the Q Set.
        /// </summary>
        public void AddNewFolderToQSet()
        {
            if (_primaryControls.GetQSetExplorerActiveItem() != null)
            {
                QSetFolderItem folderItem = _primaryControls.GetQSetExplorerActiveItem() as QSetFolderItem;
                if (folderItem != null)
                {
                    QSetFolderItem newFolderItem = new QSetFolderItem(GetNextAvailableNewItemName("New Folder", folderItem.ChildItems));
                    folderItem.ChildItems.Add(newFolderItem);							
                    
                    _primaryControls.QSetExplorerEditActiveItem();
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
            if (_primaryControls.GetQSetExplorerSet() != null)
            {
                //ensure we have a folder item to add to
                if (_primaryControls.GetQSetExplorerActiveItem() == null)
                    _primaryControls.SetQSetExplorerActiveItem(_primaryControls.GetQSetExplorerSet());
                if (!(_primaryControls.GetQSetExplorerActiveItem() is QSetFolderItem))
                    _primaryControls.MoveQSetExplorerActiveItemToParent();
    
                //check the item does not already exist
                QSetFolderItem parentItem = (QSetFolderItem)_primaryControls.GetQSetExplorerActiveItem();
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
            if (_primaryControls.GetQSetExplorerActiveItem() != null && !(_primaryControls.GetQSetExplorerActiveItem() is QSetModel))
            {
                string msg = null;
                if (_primaryControls.GetQSetExplorerActiveItem() is QSetFolderItem)
                {
                    if (((QSetFolderItem)_primaryControls.GetQSetExplorerActiveItem()).ChildItems != null && ((QSetFolderItem)_primaryControls.GetQSetExplorerActiveItem()).ChildItems.Count > 0)
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

                msg = string.Format(msg, _primaryControls.GetQSetExplorerActiveItem().Name);
                msg = string.Format("Are you sure you wish to remove {0} from the Q Set?", msg);

                if (MessageBox.Show(_primaryForms.EnvironmentForm, msg, Locale.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3) == DialogResult.Yes)
                {
                    ((QSetFolderItem)_primaryControls.GetQSetExplorerActiveItem().ParentItem).ChildItems.Remove(_primaryControls.GetQSetExplorerActiveItem().Name);					
                }
            }
        }

        public void PurgeAllQueuesFromQSet()
        {
            if (_primaryControls.GetQSetExplorerActiveItem() != null && !(_primaryControls.GetQSetExplorerActiveItem() is QSetModel))
            {
                var msg = "Are you sure you wish to purge all queues from the selected machine?";

                if (MessageBox.Show(_primaryForms.EnvironmentForm, msg, Locale.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    foreach(QSetQueueItem item in ((QSetFolderItem)_primaryControls.GetQSetExplorerActiveItem()).ChildItems)
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
                _primaryControls.AddTabbedDocumentMessageBrowser(qsetQueueItem, _primaryObjects);
            else
                BringDocumentToFront(qsetQueueItem);

            if (qsetQueueItem.ParentItem == null && _primaryControls.GetQSetExplorerSet() != null)
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
                if (_primaryControls.GetQSetExplorerSet() != null)
                    if (_primaryControls.GetQSetExplorerSet().FileName == null)				
                        result = SaveQSetAs();
                    
                    else				
                    {
                        result = DoSaveQSet(_primaryControls.GetQSetExplorerSet());
                        _primaryObjects.UserSettings.IndicateFileUsed(_primaryControls.GetQSetExplorerSet().FileName);	
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

                if (_primaryControls.GetQSetExplorerSet() != null)
                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {					
                        saveDialog.Title = "Save Q Set";
                        saveDialog.DefaultExt = "qset";
                        saveDialog.Filter = Locale.FileDialogQSetFilter;
                        saveDialog.InitialDirectory = "\\My Documents";
                        if (saveDialog.ShowDialog() == DialogResult.OK)
                        {					
                            string oldFileName = _primaryControls.GetQSetExplorerSet().FileName;
                            _primaryControls.GetQSetExplorerSet().FileName = saveDialog.FileName;
                            result = DoSaveQSet(_primaryControls.GetQSetExplorerSet());

                            //refresh relevant interface components
                            if (oldFileName == null)
                                _primaryObjects.UserSettings.IndicateFileUsed(_primaryControls.GetQSetExplorerSet().FileName);												
                            else
                                _primaryObjects.UserSettings.IndicateFileRenamed(oldFileName, _primaryControls.GetQSetExplorerSet().FileName);
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
            if (_primaryControls.GetQSetExplorerActiveItem() != null && (_primaryControls.GetQSetExplorerActiveItem() is QSetFolderItem || _primaryControls.GetQSetExplorerActiveItem() is QSetWebServiceItem))
                _primaryControls.QSetExplorerEditActiveItem();
        }


        /// <summary>
        /// Sets the text of the applications main title bar.
        /// </summary>
        public void SetTitleBarText()
        {
            string applicationName = Locale.ApplicationName;
            if (!_primaryObjects.License.IsLicenseValid)
                applicationName += " (UNLICENSED)";

            if (_primaryControls.GetQSetExplorerSet() == null)
                _primaryForms.EnvironmentForm.Text = applicationName;
            else if (_primaryControls.GetQSetExplorerSet().FileName == null)
                _primaryForms.EnvironmentForm.Text = string.Format("{0} - {1}", applicationName, _primaryControls.GetQSetExplorerSet().Name);
            else
                _primaryForms.EnvironmentForm.Text = string.Format("{0} - {1}", applicationName, _primaryControls.GetQSetExplorerSet().FileName);							
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
            if (_primaryControls.GetQSetExplorerSet() != null &&
                _primaryControls.GetQSetExplorerActiveItem() == null)
                _primaryControls.SetQSetExplorerActiveItem(_primaryControls.GetQSetExplorerSet());

            if (_primaryControls.GetQSetExplorerSet() != null &&
                _primaryControls.GetQSetExplorerActiveItem() is QSetQueueItem)
                _primaryControls.MoveQSetExplorerActiveItemToParent();
            
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
            optionsDialog.SmallImageList = _primaryControls.GetSize16Icons();
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
            MenuItemBag.ViewQSetExplorer.Checked = show;

            _primaryControls.ToggleQSetExplorerDisplay(show);

            _primaryObjects.UserSettings.ShowQSetExplorerWindow = MenuItemBag.ViewQSetExplorer.Checked;
        }


        /// <summary>
        /// Hides or shows the Properties window.
        /// </summary>
        /// <param name="newState">true to show the control, else false.</param>
        public void TogglePropertiesWindow(bool show)
        {
            MenuItemBag.ViewProperties.Checked = show;

            _primaryControls.TogglePropertiesGridDisplay(show);

            _primaryObjects.UserSettings.ShowPropertiesWindow = MenuItemBag.ViewProperties.Checked;
        }


        /// <summary>
        /// Hides or shows the Message Viewer.
        /// </summary>
        /// <param name="newState">true to show the control, else false.</param>
        public void ToggleMessageViewer(bool show)
        {
            MenuItemBag.ViewMessageViewer.Checked = show;

            _primaryControls.ToggleMessageViewerDisplay(show);

            _primaryObjects.UserSettings.ShowMessageViewerWindow = MenuItemBag.ViewMessageViewer.Checked;
        }


        /// <summary>
        /// Hides or shows the Q Set Monitor.
        /// </summary>
        /// <param name="newState">true to show the control, else false.</param>
        public void ToggleQSetMonitor(bool show)
        {
            MenuItemBag.ViewQSetMonitor.Checked = show;

            _primaryControls.ToggleMonitorDisplay(show);

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

            if (_primaryControls.GetQSetExplorerSet() != null && _primaryControls.IsQSetExplorerDirty)
            {
                string msg = string.Format("Do you wish to save changes to '{0}?'", _primaryControls.GetQSetExplorerSet().Name);
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
            foreach (IDockContent dockControl in _primaryControls.GetOpenDocuments())
            {
                MessageBrowser messageBrowser = dockControl.DockHandler.PanelPane.Controls[0] as MessageBrowser;
                if (messageBrowser != null)
                {
                    if (messageBrowser.QSetQueueItem.ParentItem != null)
                    {
                        _primaryControls.MessageBrowserCollection.Remove(messageBrowser.QSetQueueItem.ID.ToString());
                        dockControl.DockHandler.Close();						
                    }
                }
                else
                {
                    WebServiceClientControl webServiceClientControl = dockControl.DockHandler.PanelPane.Controls[0] as WebServiceClientControl;
                    if (webServiceClientControl != null)
                    {
                        _primaryControls.WebServiceClientControlCollection.Remove(webServiceClientControl.QSetItem.ID.ToString());
                        dockControl.DockHandler.Close();
                    }
                }
            }
            _primaryControls.SetQSetMonitorData(null);
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
                        _primaryControls.SetQSetMonitorData(qSet);
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
            IDockContent dockControl = FindDocument(item);
            if (dockControl != null)
                dockControl.DockHandler.Activate();
        }


        /// <summary>
        /// Finds the docuemtn relating to a particular item.
        /// </summary>
        /// <param name="item">Item for which document we are looking for.</param>
        /// <returns>DockControl if found, else null.</returns>
        public IDockContent FindDocument(QSetItemBase item)
        {
            IDockContent result = null;

            foreach(IDockContent dockControl in _primaryControls.GetOpenDocuments())				
            {
                IQSetItemControl itemControl = dockControl.DockHandler.PanelPane.Controls[0] as IQSetItemControl;
                if (itemControl != null && itemControl.QSetItem == item)
                {					
                    result = dockControl;					
                    break;
                }
            }

            return result;
        }

        #endregion
    }
}


