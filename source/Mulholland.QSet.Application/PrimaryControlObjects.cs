using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Messaging;
using System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Application.DockForms;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using WeifenLuo.WinFormsUI.Docking;

namespace Mulholland.QSet.Application
{
    /// <summary>
    /// Provides a base class to identify primary control holder classes.
    /// </summary>
    internal abstract class PrimaryControlsBase {}
    
    #region internal class PrimaryMenus : PrimaryControlsBase

    /// <summary>
    /// Groups together all primary environment menus.
    /// </summary>
    internal class PrimaryMenus : PrimaryControlsBase
    {
        private ToolStripMenuItem _fileMenu;
        private ToolStripMenuItem _viewMenu;
        private ToolStripMenuItem _qSetMenu;
        private ToolStripMenuItem _queueMenu;
        private ToolStripMenuItem _messageMenu;
        private ToolStripMenuItem _toolsMenu;
        private ToolStripMenuItem _helpMenu;		
        private ContextMenuStrip _messageBrowserContextMenu;
        private ContextMenuStrip _qSetContextMenu;

        private event PrimaryMenus.MenuItemsChangedEvent _recentFileListChanged;

        #region events

        /// <summary>
        /// Event delegate for MenuItemsChanged event.
        /// </summary>
        public delegate void MenuItemsChangedEvent(object sender, EventArgs e);


        /// <summary>
        /// Event arguments for MenuItemsChanged event.
        /// </summary>
        public class MenuItemsChangedEventArgs : EventArgs
        {
            private ToolStripMenuItem _parentItem;

            /// <summary>
            /// Constructs the arguments class.
            /// </summary>
            /// <param name="parentItem">Parent item for which child items changed.</param>
            public MenuItemsChangedEventArgs(ToolStripMenuItem parentItem)
            {
                _parentItem = parentItem;
            }


            /// <summary>
            /// Gets the parent item for which child items changed.
            /// </summary>
            public ToolStripMenuItem ParentItem
            {
                get
                {
                    return _parentItem;
                }
            }
        }


        /// <summary>
        /// Occurs when the recent file list is changed.
        /// </summary>
        public event PrimaryMenus.MenuItemsChangedEvent RecentFileListChanged
        {
            add
            {
                _recentFileListChanged += value;
            }
            remove 
            {
                _recentFileListChanged -= value;
            }
        }


        /// <summary>
        /// Raises the RecentFileListChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected void OnRecentFileListChanged(MenuItemsChangedEventArgs e)
        {
            try
            {
                if (_recentFileListChanged != null)
                    _recentFileListChanged(this, e);
            }
            catch {}
        }

        #endregion

        /// <summary>
        /// Constructs the object with all menus.
        /// </summary>
        /// <param name="fileMenu">File menu.</param>
        /// <param name="viewMenu">View menu.</param>
        /// <param name="qSetMenu">Q Set menu.</param>
        /// <param name="queueMenu">Queue menu.</param>
        /// <param name="messageMenu">Message menu.</param>
        /// <param name="toolsMenu">Tools menu.</param>
        /// <param name="helpMenu">Help menu.</param>		
        /// <param name="messageBrowserContextMenu">Message browser context menu.</param>
        /// <param name="qSetContextMenu">Q Set context menu.</param>
        public PrimaryMenus(
            ToolStripMenuItem fileMenu,
            ToolStripMenuItem viewMenu,
            ToolStripMenuItem qSetMenu,
            ToolStripMenuItem queueMenu,
            ToolStripMenuItem messageMenu,
            ToolStripMenuItem toolsMenu,
            ToolStripMenuItem helpMenu,
            ContextMenuStrip messageBrowserContextMenu,
            ContextMenuStrip qSetContextMenu)
        {
            if (fileMenu == null) throw new ArgumentNullException("fileMenu");
            else if (viewMenu == null) throw new ArgumentNullException("viewMenu");
            else if (qSetMenu == null) throw new ArgumentNullException("qSetMenu");
            else if (queueMenu == null) throw new ArgumentNullException("queueMenu");
            else if (messageMenu == null) throw new ArgumentNullException("messageMenu");
            else if (toolsMenu == null) throw new ArgumentNullException("toolsMenu");
            else if (helpMenu == null) throw new ArgumentNullException("helpMenu");
            else if (messageBrowserContextMenu == null) throw new ArgumentNullException("messageBrowserContextMenu");

            _fileMenu = fileMenu;
            _viewMenu = viewMenu;
            _qSetMenu = qSetMenu;
            _queueMenu = queueMenu;
            _messageMenu = messageMenu;
            _toolsMenu = toolsMenu;
            _helpMenu = helpMenu;
            _messageBrowserContextMenu = messageBrowserContextMenu;
            _qSetContextMenu = qSetContextMenu;
        }


        /// <summary>
        /// Gets the environment File menu.
        /// </summary>
        public ToolStripMenuItem FileMenu
        {
            get
            {
                return _fileMenu;
            }
        }


        /// <summary>
        /// Gets the environment View menu.
        /// </summary>
        public ToolStripMenuItem ViewMenu
        {
            get
            {
                return _viewMenu;
            }
        }


        /// <summary>
        /// Gets the environment Q Set menu.
        /// </summary>
        public ToolStripMenuItem QSetMenu
        {
            get
            {
                return _qSetMenu;
            }
        }


        /// <summary>
        /// Gets the environment Queue menu.
        /// </summary>
        public ToolStripMenuItem QueueMenu
        {
            get
            {
                return _queueMenu;
            }
        }


        /// <summary>
        /// Gets the environment Message menu.
        /// </summary>
        public ToolStripMenuItem MessageMenu
        {
            get
            {
                return _messageMenu;
            }
        }


        /// <summary>
        /// Gets the environment Tools menu.
        /// </summary>
        public ToolStripMenuItem ToolsMenu
        {
            get
            {
                return _toolsMenu;
            }
        }


        /// <summary>
        /// Gets the environment Help menu.
        /// </summary>
        public ToolStripMenuItem HelpMenu
        {
            get
            {
                return _helpMenu;
            }
        }


        /// <summary>
        /// Message browser context menu.
        /// </summary>
        public ContextMenuStrip MessageBrowserContextMenu
        {
            get
            {
                return _messageBrowserContextMenu;
            }
        }


        /// <summary>
        /// Q Set context menu.
        /// </summary>
        public ContextMenuStrip QSetContextMenu
        {
            get
            {
                return _qSetContextMenu;
            }
        }


        /// <summary>
        /// Refreshes the recent file list.
        /// </summary>
        /// <param name="recentFiles">List of recent files, chronoligically ordered.</param>
        /// <param name="maximumListSize">The maximum size of the list.</param>
        public void RefreshRecentFilesList(StringCollection recentFiles, int maximumListSize)
        {
            MenuItemBag.FileRecentFileList.DropDownItems.Clear();

            for (int file = 0; file < (recentFiles.Count < maximumListSize ? recentFiles.Count : maximumListSize); file++)
            {
                ToolStripMenuItem recentFileButton = new ToolStripMenuItem(string.Format("&{0} {1}", file + 1, recentFiles[file]));
                recentFileButton.Tag = recentFiles[file];
                MenuItemBag.FileRecentFileList.DropDownItems.Add(recentFileButton);
            }

            MenuItemBag.FileRecentFileList.Visible = (MenuItemBag.FileRecentFileList.DropDownItems.Count > 0);

            OnRecentFileListChanged(new PrimaryMenus.MenuItemsChangedEventArgs(MenuItemBag.FileRecentFileList));
        }
    }

    #endregion

    #region internal class PrimaryControls : PrimaryControlsBase

    /// <summary>
    /// Groups together the primary controls of the environment.
    /// </summary>
    /// <remarks>This does not contain menu or toolbar controls.</remarks>
    internal class PrimaryControls : PrimaryControlsBase
    {
        private QueueSetExplorerForm _qSetExplorerForm;
        private QueueSetMonitorForm _qSetMonitorForm;
        private PropertyGridForm _propertyGridForm;
        private MessageViewerForm _messageViewerForm;
        private DockPanel _dockPanel;
        private MessageBrowserCollection _messageBrowserCollection;
        private WebServiceClientControlCollection _webServiceClientControlCollection;
        private Images _images;
        private Action<DockContent> _wireupActionForTabbedDocuments;
        private Action<DockContent> _wiredownActionForTabbedDocuments;

        /// <summary>
        /// Constructs the object with all of the environments primary controls.
        /// </summary>
        /// <param name="qSetExplorer">Primary QSetExplorer.</param>
        /// <param name="qSetMonitor">Primary QSetMonitor.</param>
        /// <param name="propertyGrid">Primary property grid.</param>
        /// <param name="messageViewer">Primary MessageViewer.</param>
        /// <param name="documentContainer">Primary DocumentContainer.</param>
        /// <param name="images">Images component.</param>
        public PrimaryControls(
            Licensing.License license,
            DockPanel dockPanel,
            Images images)
        {
            _qSetExplorerForm = new QueueSetExplorerForm();
            _qSetMonitorForm = new QueueSetMonitorForm();
            _propertyGridForm = new PropertyGridForm();
            _messageViewerForm = new MessageViewerForm(license);
            _dockPanel = dockPanel;
            _images = images;

            _messageBrowserCollection = new MessageBrowserCollection();
            _webServiceClientControlCollection = new WebServiceClientControlCollection();

            _qSetExplorerForm.VisibleChanged += QSetExplorerForm_VisibleChanged;
            _propertyGridForm.VisibleChanged += PropertyGridForm_VisibleChanged;
            _messageViewerForm.VisibleChanged += MessageViewerForm_VisibleChanged;
            _qSetMonitorForm.VisibleChanged += QSetMonitorForm_VisibleChanged;

            _qSetExplorerForm.QSetExplorer.ImageList = this.GetSize16Icons();
            _qSetMonitorForm.QSetMonitor.ImageList = this.GetSize16Icons();

            ToggleQSetExplorerDisplay(true);
            TogglePropertiesGridDisplay(true);
            ToggleMessageViewerDisplay(true);
            ToggleMonitorDisplay(true);
        }

        ~PrimaryControls()
        {
            _qSetExplorerForm.VisibleChanged -= QSetExplorerForm_VisibleChanged;
            _propertyGridForm.VisibleChanged -= PropertyGridForm_VisibleChanged;
            _messageViewerForm.VisibleChanged -= MessageViewerForm_VisibleChanged;
            _qSetMonitorForm.VisibleChanged -= QSetMonitorForm_VisibleChanged;
        }

        public event EventHandler QSetExplorerClosed;
        public event EventHandler PropertyGridClosed;
        public event EventHandler MessageViewerClosed;
        public event EventHandler QSetMonitorClosed;

        public ImageList GetSize16Icons()
        {
            return _images.Icon16ImageList;
        }

        public QSetItemBase GetQSetExplorerActiveItem()
        {
            return _qSetExplorerForm.QSetExplorer.ActiveItem;
        }

        public bool HasActiveMessageBrowser()
        {
            var result = _dockPanel.ActiveDocument != null && _dockPanel.ActiveDocument is MessageBrowserForm;

            return result;
        }

        public MessageBrowser GetActiveMessageBrowser()
        {
            var form = (_dockPanel.ActiveDocument as MessageBrowserForm);
            if (form != null)
            {
                return form.MessageBrowser;
            }
            return null;
        }

        public bool IsQSetExplorerOpen
        {
            get
            {
                return _qSetExplorerForm.QSetExplorer.QSet != null;
            }
        }

        public bool IsQSetExplorerDirty
        {
            get
            {
                return _qSetExplorerForm.QSetExplorer.QSet.IsDirty;
            }
        }

        public bool HasActiveDocument
        {
            get
            {
                return _dockPanel.ActiveDocument != null;
            }
        }

        public MessageBrowserCollection MessageBrowserCollection
        {
            get
            {
                return _messageBrowserCollection;
            }
        }

        public WebServiceClientControlCollection WebServiceClientControlCollection
        {
            get
            {
                return _webServiceClientControlCollection;
            }
        }

        public void WireupQSetExplorer(Action<QSetExplorer> wireupAction)
        {
            wireupAction(_qSetExplorerForm.QSetExplorer);
        }

        public void WireupDockPanel(Action<DockPanel> wireupAction)
        {
            wireupAction(_dockPanel);
        }

        public void WireupPropertyGrid(Action<PropertyGrid> wireupAction)
        {
            wireupAction(_propertyGridForm.PropertyGrid);
        }

        public void DisplayQSetMessage(QSetQueueItem qSetQueueItem, System.Messaging.Message message)
        {
            _messageViewerForm.MessageViewer.DisplayMessage(qSetQueueItem, message);
        }

        public void SetQSetMonitorData(QSetModel item)
        {
            _qSetMonitorForm.QSetMonitor.QSet = item;
        }

        public void SetPropertyGridObject(QSetItemBase item)
        {
            _propertyGridForm.PropertyGrid.SelectedObject = item;
        }

        public QSetModel GetQSetExplorerSet()
        {
            return _qSetExplorerForm.QSetExplorer.QSet;
        }

        public void SetQSetExplorerActiveItem(QSetModel qSetModel)
        {
            _qSetExplorerForm.QSetExplorer.ActiveItem = qSetModel;
        }

        public void UpdateQSetExplorerOnDocumentChange()
        {
            if (_dockPanel.ActiveDocument != null)
            {
                var messageBrowserForm = _dockPanel.ActiveDocument as MessageBrowserForm;
                var webServiceForm = _dockPanel.ActiveDocument as WebServiceClientForm;

                IQSetItemControl itemControl = null;

                if (messageBrowserForm != null)
                {
                    itemControl = messageBrowserForm.MessageBrowser;
                }
                else if(webServiceForm != null)
                {
                    itemControl = webServiceForm.WebServiceClientControl;
                }

                if (itemControl != null)
                    _qSetExplorerForm.QSetExplorer.ActiveItem = itemControl.QSetItem;
            }
            else
            {
                _qSetExplorerForm.QSetExplorer.ActiveItem = null;
            }
        }

        public void AddTabbedDocumentWebService(QSetWebServiceItem webServiceItem)
        {
            var webForm = new WebServiceClientForm();
            WebServiceClientControlCollection.Add(webServiceItem.ID.ToString(), webForm.WebServiceClientControl);

            webForm.Show(_dockPanel, DockState.Document);
            webForm.WebServiceClientControl.QSetWebServiceItem = webServiceItem;

            webForm.FormClosed += WebForm_FormClosed;

            if (_wireupActionForTabbedDocuments != null)
            {
                _wireupActionForTabbedDocuments(webForm);
            }
        }


        private string ShortQueueTextName(QSetQueueItem qsetQueueItem)
        {
          var nameSplit = qsetQueueItem.Name.Split('\\');
          return nameSplit[nameSplit.Length - 1];
        }

        public void AddTabbedDocumentMessageBrowser(QSetQueueItem qsetQueueItem, PrimaryObjects primaryObjects)
        {
            var messageForm = new MessageBrowserForm();
            messageForm.MessageBrowser.UserSettings = primaryObjects.UserSettings;
            messageForm.MessageBrowser.ImageList = this.GetSize16Icons();

            try
            {
                messageForm.Text = ShortQueueTextName(qsetQueueItem);
                messageForm.Show(_dockPanel, DockState.Document);
                messageForm.MessageBrowser.QSetQueueItem = qsetQueueItem;

                MessageBrowserCollection.Add(qsetQueueItem.ID.ToString(), messageForm.MessageBrowser);

                if (_wireupActionForTabbedDocuments != null)
                {
                    _wireupActionForTabbedDocuments(messageForm);
                }
                messageForm.FormClosed += MessageForm_FormClosed;
            }
            catch (Exception exc)
            {
                primaryObjects.ProcessVisualizer.SeizeCursor(Cursors.Arrow);
                MessageBox.Show(exc.Message, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                primaryObjects.ProcessVisualizer.ReleaseCursor();
                messageForm.Close();
            }
        }

        public void SetQSetExplorerActiveItemWithEdit(QSetModel qSet)
        {
            _qSetExplorerForm.QSetExplorer.QSet = qSet;
            _qSetExplorerForm.QSetExplorer.ActiveItem = qSet;
            _qSetExplorerForm.QSetExplorer.BeginEditActiveItem();
        }

        
        public void WireupNewTabbedDocuments(Action<DockContent> wireupAction, Action<DockContent> wiredownAction)
        {
            _wireupActionForTabbedDocuments = wireupAction;
            _wiredownActionForTabbedDocuments = wiredownAction;
        }

        public void QSetExplorerEditActiveItem()
        {
            _qSetExplorerForm.QSetExplorer.BeginEditActiveItem();
        }

        public void MoveQSetExplorerActiveItemToParent()
        {
            _qSetExplorerForm.QSetExplorer.ActiveItem = _qSetExplorerForm.QSetExplorer.ActiveItem.ParentItem;
        }

        public void ToggleQSetExplorerDisplay(bool show)
        {
            if(show)
            {
                _qSetExplorerForm.Show(_dockPanel, DockState.DockLeft);
            }
            else
            {
                _qSetExplorerForm.Hide();
            }
        }

        public void TogglePropertiesGridDisplay(bool show)
        {
            if(show)
            {
                _propertyGridForm.Show(_dockPanel, DockState.DockLeft);
            }
            else
            {
                _propertyGridForm.Hide();
            }
        }

        public void ToggleMessageViewerDisplay(bool show)
        {
            if(show)
            {
                _messageViewerForm.Show(_dockPanel, DockState.DockBottom);
            }
            else
            {
                _messageViewerForm.Hide();
            }
        }

        public void ToggleMonitorDisplay(bool show)
        {
            if(show)
            {
                _qSetMonitorForm.Show(_dockPanel, DockState.DockBottom);
            }
            else
            {
                _qSetMonitorForm.Hide();
            }
        }

        public void SetQSetExplorerData(QSetModel qSet)
        {
            _qSetExplorerForm.QSetExplorer.QSet = qSet;
        }

        public void ClearOpenedDocuments()
        {
            foreach (IDockContent dockControl in new List<IDockContent>(_dockPanel.Documents))
            {
                var messageBrowserForm = dockControl as MessageBrowserForm;
                var webServiceForm = dockControl as WebServiceClientForm;

                if (messageBrowserForm != null)
                {
                    messageBrowserForm.Close();
                }
                else if (webServiceForm != null)
                {
                    webServiceForm.Close();
                }
            }
        }

        public IDockContent FindOpenDocumentForItem(QSetItemBase item)
        {
            foreach (IDockContent dockControl in _dockPanel.Documents)
            {
                var messageBrowserForm = dockControl as MessageBrowserForm;
                var webServiceForm = dockControl as WebServiceClientForm;

                if (messageBrowserForm != null)
                {
                    if (messageBrowserForm.MessageBrowser.QSetItem == item)
                    {
                        return dockControl;
                    }
                }
                else if (webServiceForm != null)
                {
                    if (webServiceForm.WebServiceClientControl.QSetItem == item)
                    {
                        return dockControl;
                    }
                }
            }

            return null;
        }

        private void MessageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = (MessageBrowserForm)sender;
            RemoveTabbedMessageBrowser(form);
        }

        private void RemoveTabbedMessageBrowser(MessageBrowserForm form)
        {
            form.FormClosed -= MessageForm_FormClosed;
            MessageBrowserCollection.Remove(form.MessageBrowser.QSetQueueItem.ID.ToString());
            if (_wiredownActionForTabbedDocuments != null)
            {
                _wiredownActionForTabbedDocuments(form);
            }
        }

        private void WebForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = (WebServiceClientForm)sender;
            RemoveTabbedWebServiceForm(form);
        }

        private void RemoveTabbedWebServiceForm(WebServiceClientForm form)
        {
            form.FormClosed -= WebForm_FormClosed;
            WebServiceClientControlCollection.Remove(form.WebServiceClientControl.QSetWebServiceItem.ID.ToString());
            if (_wiredownActionForTabbedDocuments != null)
            {
                _wiredownActionForTabbedDocuments(form);
            }
        }

        private void QSetExplorerForm_VisibleChanged(object sender, EventArgs e)
        {
            var form = (QueueSetExplorerForm)sender;
            if (form.VisibleState == DockState.Hidden)
            {
                if (QSetExplorerClosed != null)
                {
                    QSetExplorerClosed(sender, e);
                }
            }
        }

        private void PropertyGridForm_VisibleChanged(object sender, EventArgs e)
        {
            var form = (PropertyGridForm)sender;
            if (form.VisibleState == DockState.Hidden)
            {
                if (PropertyGridClosed != null)
                {
                    PropertyGridClosed(sender, e);
                }
            }
        }

        private void MessageViewerForm_VisibleChanged(object sender, EventArgs e)
        {
            var form = (MessageViewerForm)sender;
            if (form.VisibleState == DockState.Hidden)
            {
                if (MessageViewerClosed != null)
                {
                    MessageViewerClosed(sender, e);
                }
            }
        }

        private void QSetMonitorForm_VisibleChanged(object sender, EventArgs e)
        {
            var form = (QueueSetMonitorForm)sender;
            if (form.VisibleState == DockState.Hidden)
            {
                if (QSetMonitorClosed != null)
                {
                    QSetMonitorClosed(sender, e);
                }
            }
        }
    }

    #endregion

    #region internal class PrimaryForms : PrimaryControlsBase

    /// <summary>
    /// Groups together persistable forms and dialogues that are not 
    /// </summary>
    internal class PrimaryForms : PrimaryControlsBase
    {
        private QSetEnvironmentForm _environmentForm;
        private QueueSearchForm _QueueSearchForm;

        /// <summary>
        /// Constructs the object with the minumum requirements.
        /// </summary>
        /// <param name="environmentForm">Main environment form.</param>
        /// <param name="QueueSearchForm">Persistant search dialog.</param>
        public PrimaryForms(QSetEnvironmentForm environmentForm, QueueSearchForm QueueSearchForm)
        {
            if (QueueSearchForm == null) throw new ArgumentNullException("QueueSearchForm");
            else if (environmentForm == null) throw new ArgumentNullException("environmentForm");

            _environmentForm = environmentForm;
            _QueueSearchForm = QueueSearchForm;
        }


        /// <summary>
        /// Gets the main environment form.
        /// </summary>
        public QSetEnvironmentForm EnvironmentForm 
        {
            get
            {
                return _environmentForm;
            }			
        }


        /// <summary>
        /// Gets the dialogue use for searching for queues.
        /// </summary>
        public QueueSearchForm QueueSearchForm
        {
            get
            {
                return _QueueSearchForm;
            }
        }

    }

    #endregion
}
