using System;
using System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
    #region internal abstract class PrimaryControlListenerBase

    /// <summary>
    /// Provides a base class for contol listeners.
    /// </summary>
    internal abstract class PrimaryControlListenerBase
    {		
        private TaskManager _taskManager;
        private PrimaryObjects _primaryObjects;
        private PrimaryForms _primaryForms;
        private PrimaryControls _primaryControls;
        private PrimaryMenus _primaryMenus;

        /// <summary>
        /// Constructs the object with the minumum requirements.
        /// </summary>
        /// <param name="primaryControls"></param>
        /// <param name="taskManager"></param>
        /// <param name="primaryObjects"></param>
        /// <param name="primaryForms"></param>
        /// <param name="primaryMenus"></param>
        protected PrimaryControlListenerBase
            (			
            TaskManager taskManager,			
            PrimaryObjects primaryObjects,
            PrimaryForms primaryForms,
            PrimaryControls primaryControls,
            PrimaryMenus primaryMenus
            )
        {			
            _taskManager = taskManager;
            _primaryObjects = primaryObjects;
            _primaryForms = primaryForms;
            _primaryControls = primaryControls;
            _primaryMenus = primaryMenus;
        }


        /// <summary>
        /// Gets the PrimaryControls associated with the object.
        /// </summary>
        protected PrimaryControls PrimaryControls
        {
            get
            {
                return _primaryControls;
            }
        }


        /// <summary>
        /// Gets the TaskManager associated with the object.
        /// </summary>
        protected TaskManager TaskManager
        {
            get
            {
                return _taskManager;
            }
        }


        /// <summary>
        /// Gets the PrimaryObjects object associated with the object.
        /// </summary>
        protected PrimaryObjects PrimaryObjects
        {
            get
            {
                return _primaryObjects;
            }
        }


        /// <summary>
        /// Gets the PrimaryForms object associated with the object.
        /// </summary>
        protected PrimaryForms PrimaryForms
        {
            get
            {
                return _primaryForms;
            }
        }


        /// <summary>
        /// Gets the PrimaryMenus object associated with the object.
        /// </summary>
        protected PrimaryMenus PrimaryMenus
        {
            get
            {
                return _primaryMenus;
            }
        }
    }

    #endregion

    #region internal class PrimaryMenuListener : PrimaryControlListenerBase

    /// <summary>
    /// Listens and reacts to the environments primary menus.
    /// </summary>
    internal class PrimaryMenuListener : PrimaryControlListenerBase
    {
        /// <summary>
        /// Constructs the listener.
        /// </summary>		
        /// <param name="taskManager">Task manager.</param>
        /// <param name="primaryObjects">Environments PrimaryObjects object.</param>		
        /// <param name="primaryForms">Environments PrimaryForms object.</param>
        /// <param name="primaryControls">Environments PrimaryControls object.</param>
        /// <param name="primaryMenus">Environments PrimaryMenus object.</param>
        public PrimaryMenuListener
            (
            TaskManager taskManager,			
            PrimaryObjects primaryObjects,
            PrimaryForms primaryForms,
            PrimaryControls primaryControls,
            PrimaryMenus primaryMenus
            ) 
            : base(taskManager, primaryObjects, primaryForms, primaryControls, primaryMenus) 
        {
            //wire up all event handlers

            foreach (ToolStripItem item in base.PrimaryMenus.FileMenu.DropDownItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem != null && menuItem != MenuItemBag.FileRecentFileList)
                {
                    menuItem.Click += FileMenuItem_Activate;
                }
            }

            WireUpMenuEvent(base.PrimaryMenus.ViewMenu.DropDownItems, ViewMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.QSetMenu.DropDownItems, QSetMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.QueueMenu.DropDownItems, QueueMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.MessageMenu.DropDownItems, MessageMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.ToolsMenu.DropDownItems, ToolsMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.HelpMenu.DropDownItems, HelpMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.MessageBrowserContextMenu.Items, MessageBrowserContextMenuItem_Activate);
            WireUpMenuEvent(base.PrimaryMenus.QSetContextMenu.Items, QSetContextMenuItem_Activate);

            base.PrimaryMenus.RecentFileListChanged += PrimaryMenus_RecentFileListChanged;
        }

        private static void WireUpMenuEvent(ToolStripItemCollection toolstripItems, EventHandler eventHandler)
        {
            foreach (ToolStripItem item in toolstripItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if(menuItem != null)
                {
                    menuItem.Click += new EventHandler(eventHandler);
                }
            }
        }

        /// <summary>
        /// Wires up events listening to the recent file list.
        /// </summary>
        private void WireUpRecentFileListEvents()
        {
            //foreach(MenuItemBase menuItem in MenuItemBag.FileRecentFileList.Items)
            //	menuItem.Activate += new EventHandler(RecentFileListItem_Activate);
        }


        /// <summary>
        /// Handles a File menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.FileNewQSet)
                base.TaskManager.CreateNewQSet();
            else if (sender == MenuItemBag.FileOpenQSet)
                base.TaskManager.OpenQSet();
            else if (sender == MenuItemBag.FileCloseQSet)				
                base.TaskManager.CloseQSet();
            else if (sender == MenuItemBag.FileSaveQSet)				
                base.TaskManager.SaveQSet();			
            else if (sender == MenuItemBag.FileSaveQSetAs)
                base.TaskManager.SaveQSetAs();		
            else if (sender == MenuItemBag.FileExit)
                base.TaskManager.ShutDown();
            else if (sender == MenuItemBag.FileNewMessage)			
                base.TaskManager.QueueTaskManager.SendNewMessage();			
        }


        /// <summary>
        /// Handles a Q Set menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.QSetAddActiveQueue)
                base.TaskManager.AddActiveQueueToQSet();
            else if (sender == MenuItemBag.QSetDeleteItem)
                base.TaskManager.DeleteActiveItemFromQSet();
            else if (sender == MenuItemBag.QSetNewFolder)
                base.TaskManager.AddNewFolderToQSet();
            else if (sender == MenuItemBag.QSetRenameFolder)
                base.TaskManager.EditActiveQSetFolder();
            else if (sender == MenuItemBag.QSetPurgeAllQueues)
                base.TaskManager.PurgeAllQueuesFromQSet();
        }


        /// <summary>
        /// Handles a Queue menu item click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.QueueOpen)
                base.TaskManager.OpenQueue();
            else if (sender == MenuItemBag.QueueBrowse)
                base.TaskManager.BrosweForQueue();
            else if (sender == MenuItemBag.QueueCreate)
                base.TaskManager.QueueTaskManager.CreateQueue();
            else if (sender == MenuItemBag.QueueDelete)
                base.TaskManager.QueueTaskManager.DeleteActiveQueue();
            else if (sender == MenuItemBag.QueueRefresh)
                base.TaskManager.QueueTaskManager.RefreshActiveQueue();
            else if (sender == MenuItemBag.QueuePurge)
                base.TaskManager.PurgeActiveQueue();
        }


        private void MessageMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.MessageNew)
                base.TaskManager.QueueTaskManager.SendNewMessage((QSetQueueItem)base.PrimaryControls.GetQSetExplorerActiveItem());
            else if (sender == MenuItemBag.MessageForward)
                base.TaskManager.QueueTaskManager.ForwardSelectedMessagesFromQueue(false);				
            else if (sender == MenuItemBag.MessageMove)
                base.TaskManager.QueueTaskManager.ForwardSelectedMessagesFromQueue(true);				
            else if (sender == MenuItemBag.MessageDelete)
                base.TaskManager.QueueTaskManager.DeleteSelectedMessagesFromQueue();
        }


        /// <summary>
        /// Handles a Tools menu click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolsMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.ToolsOptions)
                base.TaskManager.DisplayOptions();
            else if (sender == MenuItemBag.ToolsNewWebServiceClient)
                base.TaskManager.WebTaskManager.AddNewWebServiceClient();
        }

        
        /// <summary>
        /// Handles a Help menu click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HelpMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.HelpAbout)
                base.TaskManager.ShowAboutBox();
        }


        /// <summary>
        /// Handles a View menu click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewMenuItem_Activate(object sender, EventArgs e)
        {			
            if (sender == MenuItemBag.ViewQSetExplorer)
                base.TaskManager.ToggleQSetExplorer(!MenuItemBag.ViewQSetExplorer.Checked);
            else if (sender == MenuItemBag.ViewProperties)
                base.TaskManager.TogglePropertiesWindow(!MenuItemBag.ViewProperties.Checked);
            else if (sender == MenuItemBag.ViewMessageViewer)
                base.TaskManager.ToggleMessageViewer(!MenuItemBag.ViewMessageViewer.Checked);
            else if (sender == MenuItemBag.ViewQSetMonitor)
                base.TaskManager.ToggleQSetMonitor(!MenuItemBag.ViewQSetMonitor.Checked);
        }


        /// <summary>
        /// Handles a click event of message browser context menu button item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowserContextMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.MessageBrowserCtxForwardMessage)
                base.TaskManager.QueueTaskManager.ForwardSelectedMessagesFromQueue(false);
            else if (sender == MenuItemBag.MessageBrowserCtxMoveMessage)
                base.TaskManager.QueueTaskManager.ForwardSelectedMessagesFromQueue(true);
            else if	(sender == MenuItemBag.MessageBrowserCtxDeleteMessage)
                base.TaskManager.QueueTaskManager.DeleteSelectedMessagesFromQueue();			
            else if (sender == MenuItemBag.MessageBrowserCtxRefreshMessages)
                base.TaskManager.QueueTaskManager.RefreshActiveQueue();
            else if (sender == MenuItemBag.MessageBrowserCtxPurgeQueue)
                base.TaskManager.PurgeActiveQueue();
        }


        /// <summary>
        /// Handles a click event of q set context button item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetContextMenuItem_Activate(object sender, EventArgs e)
        {
            if (sender == MenuItemBag.QSetCtxNewMessage)
            {
                if (base.PrimaryControls.GetQSetExplorerActiveItem() != null && base.PrimaryControls.GetQSetExplorerActiveItem() is QSetQueueItem)
                    base.TaskManager.QueueTaskManager.SendNewMessage((QSetQueueItem)base.PrimaryControls.GetQSetExplorerActiveItem());
                else
                    base.TaskManager.QueueTaskManager.SendNewMessage();
            }
            else if (sender == MenuItemBag.QSetCtxAddActiveQueueToSet)
                base.TaskManager.AddActiveQueueToQSet();
            else if (sender == MenuItemBag.QSetCtxNewFolder)
                base.TaskManager.AddNewFolderToQSet();
            else if (sender == MenuItemBag.QSetCtxRenameFolder)
                base.TaskManager.EditActiveQSetFolder();
            else if (sender == MenuItemBag.QSetCtxDeleteItem)
                base.TaskManager.DeleteActiveItemFromQSet();
            else if (sender == MenuItemBag.QSetCtxPurgeAllQueues)
                base.TaskManager.PurgeAllQueuesFromQSet();
            else if (sender == MenuItemBag.QSetCtxDeleteQueue)
                base.TaskManager.QueueTaskManager.DeleteActiveQueue();
            else if (sender == MenuItemBag.QSetCtxPurgeQueue)
                base.TaskManager.PurgeActiveQSetExplorerQueue();
            else if (sender == MenuItemBag.QSetCtxNewWebServiceClient)
                base.TaskManager.WebTaskManager.AddNewWebServiceClient();
        }

        /// <summary>
        /// Handles a click event of recent file list button item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RecentFileListItem_Activate(object sender, EventArgs e)
        {
            base.TaskManager.OpenQSet((string)((ToolStripMenuItem)sender).Tag);
        }


        /// <summary>
        /// Handles update of recent file list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrimaryMenus_RecentFileListChanged(object sender, EventArgs e)
        {
            WireUpRecentFileListEvents();
        }

    }

    #endregion

    #region internal class PrimaryControlListener : PrimaryControlListenerBase

    /// <summary>
    /// Listens and reacts to the environments primary controls.
    /// </summary>
    internal class PrimaryControlListener : PrimaryControlListenerBase
    {			
        /// <summary>
        /// Constructs the listener.
        /// </summary>
        /// <param name="taskManager">Task manager.</param>
        /// <param name="primaryObjects">Environments PrimaryObjects object.</param>		
        /// <param name="primaryForms">Environments PrimaryForms object.</param>
        /// <param name="primaryControls">Environments PrimaryControls object.</param>
        /// <param name="primaryMenus">Environments PrimaryMenus object.</param>
        public PrimaryControlListener
            (
            TaskManager taskManager,			
            PrimaryObjects primaryObjects,
            PrimaryForms primaryForms,
            PrimaryControls primaryControls,
            PrimaryMenus primaryMenus
            ) 
            : base(taskManager, primaryObjects, primaryForms, primaryControls, primaryMenus) 
        {					
            base.PrimaryControls.WireupQSetExplorer(qse =>
            {
                qse.QSetItemDoubleClick += QSetExplorer_QSetItemDoubleClick;
                qse.QSetActivated += QSetExplorer_QSetActivated;
                qse.QSetDeactivated += QSetExplorer_QSetDeactivated;
                qse.BeforeQSetItemActivated += QSetExplorer_BeforeQSetItemActivated;
                qse.AfterQSetItemActivated += QSetExplorer_AfterQSetItemActivated;
                qse.MessagesDragDrop += QSetExplorer_MessagesDragDrop;
            });

            base.PrimaryControls.WireupDockPanel(dp => 
            {
                dp.ActiveDocumentChanged += DocumentContainer_ActiveDocumentChanged;
                dp.MouseDown += DocumentContainer_MouseDown;
            });

            base.PrimaryControls.MessageBrowserCollection.ItemAdded += MessageBrowserCollection_ItemAdded;
            base.PrimaryControls.MessageBrowserCollection.ItemRemoved += MessageBrowserCollection_ItemRemoved;

            base.PrimaryControls.WebServiceClientControlCollection.ItemAdded += WebServiceClientControlCollection_ItemAdded;
            base.PrimaryControls.WebServiceClientControlCollection.ItemRemoved += WebServiceClientControlCollection_ItemRemoved;

            base.PrimaryControls.WireupPropertyGrid(grid => 
            {
                // This is a fix for an issue with the grid where the property dissappeared after being edited in the grid.
                // There is nothing wrong with any code, but this solves the issue, causing the grid to refresh itself.
                grid.PropertyValueChanged += (s, e) => grid.SelectedGridItem = grid.SelectedGridItem;
            });

            base.PrimaryControls.QSetExplorerClosed += (s, e) => base.TaskManager.ToggleQSetExplorer(false);
            base.PrimaryControls.PropertyGridClosed += (s, e) => base.TaskManager.TogglePropertiesWindow(false);
            base.PrimaryControls.MessageViewerClosed += (s, e) => base.TaskManager.ToggleMessageViewer(false);
            base.PrimaryControls.QSetMonitorClosed += (s, e) => base.TaskManager.ToggleQSetMonitor(false);
        }

        /// <summary>
        /// Provides types access to the menus being listened to.
        /// </summary>
        private new PrimaryControls PrimaryControls
        {
            get
            {
                return (PrimaryControls)base.PrimaryControls;
            }
        }


        /// <summary>
        /// Handles event fired when a MessageBrowser is added to the main MessageBrowserCollection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowserCollection_ItemAdded(object sender, MessageBrowserCollection.ItemMovedEventArgs e)
        {			
            e.Item.SelectedMessageChanged += new SelectedMessageChangedEvent(MessageBrowser_SelectedMessageChanged);
            e.Item.BeforeMessageListLoaded += new VisualizableProcessEvent(MessageBrowser_BeforeMessageListLoaded);
            e.Item.AfterMessageListLoaded += new VisualizableProcessEvent(MessageBrowser_AfterMessageListLoaded);
            e.Item.MessageLoadException += new MessageLoadExceptionEvent(MessageBrowser_MessageLoadException);
            e.Item.MouseDown += new MouseEventHandler(MessageBrowser_MouseDown);
            e.Item.MessagesDragDrop += new MessagesDragDropEvent(Item_MessagesDragDrop);
        }


        /// <summary>
        /// Handles event fired when a MessageBrowser is removed from the main MessageBrowserCollection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowserCollection_ItemRemoved(object sender, MessageBrowserCollection.ItemMovedEventArgs e)
        {
            e.Item.Dispose();			
            base.TaskManager.MenuStateManger.SetAllMenusState();
        }


        /// <summary>
        /// Handles event fired when an item in a MessageBrowser is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowser_SelectedMessageChanged(Mulholland.QSet.Application.Controls.MessageBrowser sender, SelectedMessageChangedEventArgs e)
        {			
            VisualizableProcess process = new VisualizableProcess(Locale.UserMessages.DisplayingMessage);
            base.PrimaryObjects.ProcessVisualizer.ProcessStarting(process);						
            
            PrimaryControls.DisplayQSetMessage(e.QSetQueueItem, e.Message);		
            
            base.TaskManager.MenuStateManger.SetAllMenusState();				
            base.PrimaryObjects.ProcessVisualizer.ProcessCompleted(process);			
        }


        /// <summary>
        /// Handles event fired by Q Set Explorer when an item is double clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetExplorer_QSetItemDoubleClick(object sender, QSetItemDoubleClickEventArgs e)
        {
            if (e.Item is QSetQueueItem)
                base.TaskManager.OpenQueue((QSetQueueItem)e.Item);
            else if (e.Item is QSetWebServiceItem)
                base.TaskManager.WebTaskManager.OpenWebServiceClient((QSetWebServiceItem)e.Item);

            base.TaskManager.MenuStateManger.SetQSetMenuState();
        }

        //TODO restore close handle
        /*
        /// <summary>
        /// Handles event fired when a document is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentContainer_DocumentClosing(object sender, TD.SandDock.DocumentClosingEventArgs e)
        {
            if (e.DockControl.Controls.Count > 0)
            {
                MessageBrowser messageBrowser = e.DockControl.Controls[0] as MessageBrowser;
                if (messageBrowser != null)
                    base.PrimaryControls.MessageBrowserCollection.Remove(messageBrowser.QSetQueueItem.ID.ToString());
                else
                {
                    WebServiceClientControl webServiceClientControl = e.DockControl.Controls[0] as WebServiceClientControl;
                    if (webServiceClientControl != null)
                        base.PrimaryControls.WebServiceClientControlCollection.Remove(webServiceClientControl.QSetItem.ID.ToString());	
                }
            }

            base.TaskManager.MenuStateManger.SetQueueMenuState();
        }
         * */


        /// <summary>
        /// Raised when a Q Set is activated in the Q Set Explorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetExplorer_QSetActivated(object sender, QSetActivatedEventArgs e)
        {						
            e.Item.ItemRenamed += new ItemRenamedEvent(QSet_ItemRenamed);
            e.Item.ItemDirtied += new ItemDirtiedEvent(QSet_ItemDirtied);

            base.TaskManager.SetTitleBarText();
            base.TaskManager.MenuStateManger.SetQSetMenuState();
            base.PrimaryControls.SetQSetMonitorData((QSetModel)e.Item);
        }

        /// <summary>
        /// Raised when a Q Set is de-activated in the Q Set Explorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetExplorer_QSetDeactivated(object sender, QSetDeactivatedEventArgs e)
        {						
            e.Item.ItemRenamed -= new ItemRenamedEvent(QSet_ItemRenamed);
            e.Item.ItemDirtied -= new ItemDirtiedEvent(QSet_ItemDirtied);

            base.TaskManager.SetTitleBarText();
            base.TaskManager.MenuStateManger.SetQSetMenuState();
            base.PrimaryControls.SetQSetMonitorData(null);
        }


        /// <summary>
        /// Handles event fired when the Q Set in the Q Set Explorer becomes dirty.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSet_ItemDirtied(object sender, ItemDirtiedEventArgs e)
        {
            base.TaskManager.MenuStateManger.SetFileMenuState();
        }


        /// <summary>
        /// Handles event raised when Q Set attached to the explorer is renamed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSet_ItemRenamed(object sender, ItemRenamedEventArgs e)
        {
            base.TaskManager.SetTitleBarText();
            base.TaskManager.MenuStateManger.SetQSetMenuState();
        }


        /// <summary>
        /// Handles event fired before an item is activated in the Q Set Explorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetExplorer_BeforeQSetItemActivated(object sender, VisualizableProcessItemAffectedEventArgs e)
        {
            base.PrimaryObjects.ProcessVisualizer.ProcessStarting(e.Process);
        }


        /// <summary>
        /// Handles event fired after an item is activated in the Q Set Explorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetExplorer_AfterQSetItemActivated(object sender, VisualizableProcessItemAffectedEventArgs e)
        {
            base.TaskManager.MenuStateManger.SetAllMenusState();
            if (e.Item is QSetQueueItem)
                try
                {
                    base.PrimaryControls.SetPropertyGridObject(e.Item);
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    base.PrimaryControls.SetPropertyGridObject(null);
                }
            else
                base.PrimaryControls.SetPropertyGridObject(e.Item);
    
            base.TaskManager.MenuStateManger.SetQSetCtxMenuState();
            
            base.PrimaryObjects.ProcessVisualizer.ProcessCompleted(e.Process);
        }	


        /// <summary>
        /// Handles notification that messages have been dropped onto a queue in the QSetExplorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QSetExplorer_MessagesDragDrop(object sender, MessagesDragDropEventArgs e)
        {
            base.TaskManager.QueueTaskManager.CopyMessages(e.FromQueueItem, e.ToQueueItem, e.Messages, true);
        }



        /// <summary>
        /// Handles event fired when the active document is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentContainer_ActiveDocumentChanged(object sender, EventArgs e)
        {
            base.TaskManager.MenuStateManger.SetQueueMenuState();

            base.PrimaryControls.UpdateQSetExplorerOnDocumentChange();
        }

        
        /// <summary>
        /// Handles the mouse down event of the main document container.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DocumentContainer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                base.TaskManager.MenuStateManger.SetMessageBrowserCtxMenuState();
        }

        /// <summary>
        /// Handles event fired when a MessageBroswer starts retrieving messages from a queue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowser_BeforeMessageListLoaded(object sender, VisualizableProcessEventArgs e)
        {
            base.PrimaryObjects.ProcessVisualizer.ProcessStarting(e.Process);
        }


        /// <summary>
        /// Handles event fired when a MessageBrowser has finished loading messages from a queue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowser_AfterMessageListLoaded(object sender, VisualizableProcessEventArgs e)
        {
            base.PrimaryObjects.ProcessVisualizer.ProcessCompleted(e.Process);
        }


        /// <summary>
        /// Handles messages drag drop notification of a message browser window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Item_MessagesDragDrop(object sender, MessagesDragDropEventArgs e)
        {
            base.TaskManager.QueueTaskManager.CopyMessages(e.FromQueueItem, e.ToQueueItem, e.Messages, true);
        }


        /// <summary>
        /// Handles an exception whilst a message browser is loading messages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="a"></param>
        private void MessageBrowser_MessageLoadException(MessageBrowser sender, MessageLoadExceptionEventArgs e)
        {
            base.PrimaryObjects.ProcessVisualizer.SeizeCursor(Cursors.Arrow);
            MessageBox.Show(e.Exception.Message, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            base.PrimaryObjects.ProcessVisualizer.ReleaseCursor();
        }


        /// <summary>
        /// Handles the mouse down event of a message browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBrowser_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                base.TaskManager.MenuStateManger.SetMessageBrowserCtxMenuState();
        }

        /// <summary>
        /// Handles event fired when a new <see cref="WebServiceClientControl"/> is added to the <see cref="WebServiceClientControlCollection"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebServiceClientControlCollection_ItemAdded(object sender, Mulholland.QSet.Application.Controls.WebServiceClientControlCollection.ItemMovedEventArgs e)
        {

        }


        /// <summary>
        /// Handles event fired when a <see cref="WebServiceClientControl"/> is removed from the <see cref="WebServiceClientControlCollection"/>.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebServiceClientControlCollection_ItemRemoved(object sender, Mulholland.QSet.Application.Controls.WebServiceClientControlCollection.ItemMovedEventArgs e)
        {	
            e.Item.Dispose();
        }
    }

    #endregion

    #region internal class PrimaryFormsListener : PrimaryControlListenerBase

    /// <summary>
    /// Listens and reacts to environments persistable forms which are not part of the main GUI.
    /// </summary>
    internal class PrimaryFormsListener : PrimaryControlListenerBase
    {		
        /// <summary>
        /// Constructs the listener.
        /// </summary>
        /// <param name="taskManager">Task manager.</param>
        /// <param name="primaryObjects">Environments PrimaryObjects object.</param>		
        /// <param name="primaryForms">Environments PrimaryForms object.</param>
        /// <param name="primaryControls">Environments PrimaryControls object.</param>
        /// <param name="primaryMenus">Environments PrimaryMenus object.</param>
        public PrimaryFormsListener
            (
            TaskManager taskManager,			
            PrimaryObjects primaryObjects,
            PrimaryForms primaryForms,
            PrimaryControls primaryControls,
            PrimaryMenus primaryMenus
            )
            : base(taskManager, primaryObjects, primaryForms, primaryControls, primaryMenus) 
        {			
            base.PrimaryForms.QueueSearchForm.OKClicked += new QueueSearchForm.OKClickedEvent(QueueSearchForm_OKClicked);
            base.PrimaryForms.EnvironmentForm.Load += new EventHandler(EnvironmentForm_Load);
        }


        /// <summary>
        /// Handles the double click of a queue node in the queue search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueSearchForm_QueueDoubleClicked(object sender, QueueSearchForm.MessageQueueSelectEventArgs e)
        {
            base.TaskManager.OpenQueue(new QSetQueueItem(string.Format(@"{0}\{1}", e.Queue.MachineName, e.Queue.QueueName))); //reformat as private queues can come out with extra data in name));
        }


        /// <summary>
        /// Handles the ok click of the queue search results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueSearchForm_OKClicked(object sender, QueueSearchForm.OKClickedEventArgs e)
        {
            if (base.PrimaryControls.GetQSetExplorerSet() == null)
                base.TaskManager.CreateNewQSet();

            if (base.PrimaryControls.GetQSetExplorerActiveItem() == null || 
                !(base.PrimaryControls.GetQSetExplorerActiveItem() is QSetFolderItem) ||
                base.PrimaryControls.GetQSetExplorerActiveItem() is QSetMachineItem)
                base.PrimaryControls.SetQSetExplorerActiveItem(base.PrimaryControls.GetQSetExplorerSet());

            QSetFolderItem parentItem = (QSetFolderItem)base.PrimaryControls.GetQSetExplorerActiveItem();
            foreach (QSetItemBase item in e.SelectedItems)
            {
                if (!parentItem.ChildItems.Exists(item.Name))
                    parentItem.ChildItems.Add(item);
            }
        }


        /// <summary>
        /// Handles the load event of the main form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnvironmentForm_Load(object sender, EventArgs e)
        {			 
            if (!base.PrimaryObjects.License.IsLicenseValid)
            {
                base.PrimaryObjects.License.EditLicense();
                base.TaskManager.SetTitleBarText();
            }
        }
    }

    #endregion
}

