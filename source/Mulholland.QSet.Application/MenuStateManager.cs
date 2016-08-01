using System;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Model;

namespace Mulholland.QSet.Application
{
    /// <summary>
    /// Organises menu state according to according to the state of primary controls.
    /// </summary>
    internal class MenuStateManager
    {		
        private PrimaryControls _primaryControls;

        private struct EnvironmentState
        {			
            public bool IsQSetOpen;
            public bool IsQSetDirty;
            public bool IsQSetItemActive;	
            public bool IsQSetActiveItemQueue;
            public bool IsQSetActiveItemFolder;
            public bool IsQSetActiveItemMachine;
            public bool IsQSetActiveItemQSet;
            public bool IsQSetActiveItemWebService;
            public bool IsQSetParentItemMachine;
            public bool IsQSetParentItemQSet;
            public bool IsMessageBrowserActive;
            public bool IsMessageBrowserQueueChildOfActiveQSetItem;			
            public int ActiveMessageBrowserSelectedMessageCount;
        }		

        /// <summary>
        /// Constructs the object.
        /// </summary>		
        /// <param name="primaryControls">Primary controls.</param>
        public MenuStateManager(PrimaryControls primaryControls)
        {
            if (primaryControls == null) throw new ArgumentNullException("primaryControls");
    
            _primaryControls = primaryControls;
        }


        /// <summary>
        /// sets the state of all menus.
        /// </summary>
        public void SetAllMenusState()
        {			
            EnvironmentState state = GetEnvironmentState();

            SetFileMenuState(state);
            SetQSetMenuState(state);
            SetQueueMenuState(state);
            SetMessageMenuState(state);
            SetToolsMenuState(state);
            SetMessageBrowserCtxMenuState(state);
            SetQSetCtxMenuState(state);
        }


        /// <summary>
        /// Sets the state of the File menu items.
        /// </summary>
        public void SetFileMenuState()
        {						
            SetFileMenuState(GetEnvironmentState());
        }


        /// <summary>
        /// Sets the state of the Q Set menu items.
        /// </summary>
        public void SetQSetMenuState()
        {						
            SetQSetMenuState(GetEnvironmentState());
        }


        /// <summary>
        /// Sets the state of the Queue menu items.
        /// </summary>
        public void SetQueueMenuState()
        {
            SetQueueMenuState(GetEnvironmentState());
        }


        /// <summary>
        /// Sets the state of the message menu items.
        /// </summary>
        public void SetMessageMenuState()
        {
            SetMessageMenuState(GetEnvironmentState());
        }


        /// <summary>
        /// Sets the state of the Tools menu.
        /// </summary>
        public void SetToolsMenuState()
        {
            SetToolsMenuState(GetEnvironmentState());			
        }


        /// <summary>
        /// Sets the state of MessageBrowserCtxMenu menu items.
        /// </summary>
        public void SetMessageBrowserCtxMenuState()
        {
            SetMessageBrowserCtxMenuState(GetEnvironmentState());

        }


        /// <summary>
        /// Sets the state of the Q Set menu items
        /// </summary>
        public void SetQSetCtxMenuState()
        {
            SetQSetCtxMenuState(GetEnvironmentState());
        }


        private void SetFileMenuState(EnvironmentState state)
        {						
            MenuItemBag.FileCloseQSet.Enabled = state.IsQSetOpen;
            MenuItemBag.FileSaveQSet.Enabled = state.IsQSetOpen && state.IsQSetDirty;
            MenuItemBag.FileSaveQSetAs.Enabled = state.IsQSetOpen;			
        }


        private void SetQSetMenuState(EnvironmentState state)
        {
            MenuItemBag.QSetCtxAddActiveQueueToSet.Visible = false;
            //MenuItemBag.QSetAddActiveQueue.Enabled = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet) && !state.IsQSetActiveItemMachine && state.IsMessageBrowserActive && !state.IsMessageBrowserQueueChildOfActiveQSetItem);
            MenuItemBag.QSetNewFolder.Enabled = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet) && !state.IsQSetActiveItemMachine);
            MenuItemBag.QSetRenameFolder.Enabled = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet || state.IsQSetActiveItemWebService) && !state.IsQSetActiveItemMachine);
            MenuItemBag.QSetDeleteItem.Enabled = (state.IsQSetOpen && !state.IsQSetActiveItemQSet && !state.IsQSetParentItemMachine);

            MenuItemBag.QSetPurgeAllQueues.Enabled = (state.IsQSetOpen && !state.IsQSetActiveItemQSet && !state.IsQSetParentItemMachine);
        }


        private void SetQueueMenuState(EnvironmentState state)
        {
            MenuItemBag.QueueRefresh.Enabled = state.IsQSetActiveItemQueue && state.IsMessageBrowserActive;
            MenuItemBag.QueuePurge.Enabled = state.IsMessageBrowserActive;			
            MenuItemBag.QueueDelete.Enabled = state.IsQSetActiveItemQueue;
        }


        private void SetMessageMenuState(EnvironmentState state)
        {
            MenuItemBag.MessageNew.Enabled = state.IsQSetActiveItemQueue;
            MenuItemBag.MessageForward.Enabled = state.ActiveMessageBrowserSelectedMessageCount > 0;
            MenuItemBag.MessageMove.Enabled = state.ActiveMessageBrowserSelectedMessageCount > 0;
            MenuItemBag.MessageDelete.Enabled = state.ActiveMessageBrowserSelectedMessageCount > 0;
        }


        private void SetToolsMenuState(EnvironmentState state)
        {
            MenuItemBag.ToolsNewWebServiceClient.Visible = false; //TODO remove this line for 1.2
            MenuItemBag.ToolsNewWebServiceClient.Enabled = state.IsQSetActiveItemFolder && !state.IsQSetActiveItemMachine;
        }


        private void SetMessageBrowserCtxMenuState(EnvironmentState state)
        {
            MenuItemBag.MessageBrowserCtxForwardMessage.Enabled = state.ActiveMessageBrowserSelectedMessageCount > 0;
            MenuItemBag.MessageBrowserCtxMoveMessage.Enabled = state.ActiveMessageBrowserSelectedMessageCount > 0;
            MenuItemBag.MessageBrowserCtxDeleteMessage.Enabled = state.ActiveMessageBrowserSelectedMessageCount > 0;
            MenuItemBag.MessageBrowserCtxRefreshMessages.Enabled = state.IsMessageBrowserActive;
            MenuItemBag.MessageBrowserCtxPurgeQueue.Enabled = state.IsMessageBrowserActive;
        }


        private void SetQSetCtxMenuState(EnvironmentState state)
        {
            MenuItemBag.QSetCtxNewMessage.Visible = state.IsQSetActiveItemQueue;							
            MenuItemBag.QSetCtxAddActiveQueueToSet.Visible = false;
            //MenuItemBag.QSetCtxAddActiveQueueToSet.Visible = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet) && !state.IsQSetActiveItemMachine && state.IsMessageBrowserActive && !state.IsMessageBrowserQueueChildOfActiveQSetItem);
            MenuItemBag.QSetCtxDeleteItem.Visible = (state.IsQSetOpen && !state.IsQSetActiveItemQSet && !state.IsQSetParentItemMachine);
            MenuItemBag.QSetCtxPurgeAllQueues.Visible = (state.IsQSetOpen && !state.IsQSetActiveItemQSet && !state.IsQSetParentItemMachine);
            MenuItemBag.QSetCtxDeleteQueue.Visible = state.IsQSetActiveItemQueue;			
            MenuItemBag.QSetCtxNewFolder.Visible = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet) && !state.IsQSetActiveItemMachine);
            MenuItemBag.QSetCtxRenameFolder.Visible = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet || state.IsQSetActiveItemWebService) && !state.IsQSetActiveItemMachine);
            MenuItemBag.QSetCtxPurgeQueue.Visible = state.IsQSetActiveItemQueue;
            MenuItemBag.QSetCtxNewWebServiceClient.Visible = false;
            //MenuItemBag.QSetCtxNewWebServiceClient.Visible = state.IsQSetActiveItemFolder && !state.IsQSetActiveItemMachine;
        }


        private EnvironmentState GetEnvironmentState()
        {
            var qSetExplorerActiveItem = _primaryControls.GetQSetExplorerActiveItem();

            //ascertain the state of the environment
            EnvironmentState environmentState = new EnvironmentState();
            environmentState.IsQSetOpen = _primaryControls.IsQSetExplorerOpen;
            environmentState.IsQSetDirty = environmentState.IsQSetOpen && _primaryControls.IsQSetExplorerDirty;
            environmentState.IsQSetItemActive = (environmentState.IsQSetOpen && qSetExplorerActiveItem != null);
            if (environmentState.IsQSetItemActive)
            {
                environmentState.IsQSetActiveItemQueue = qSetExplorerActiveItem is QSetQueueItem;
                environmentState.IsQSetActiveItemFolder = qSetExplorerActiveItem is QSetFolderItem;
                environmentState.IsQSetActiveItemMachine = qSetExplorerActiveItem is QSetMachineItem;
                environmentState.IsQSetActiveItemQSet = qSetExplorerActiveItem is QSetModel;
                environmentState.IsQSetActiveItemQueue = qSetExplorerActiveItem is QSetQueueItem;
                environmentState.IsQSetActiveItemWebService = qSetExplorerActiveItem is QSetWebServiceItem;

                if (qSetExplorerActiveItem.ParentItem != null)
                {					
                    environmentState.IsQSetParentItemMachine = qSetExplorerActiveItem.ParentItem is QSetMachineItem;
                    environmentState.IsQSetParentItemQSet = qSetExplorerActiveItem.ParentItem is QSetModel;					
                }
            }

            environmentState.IsMessageBrowserActive = _primaryControls.HasActiveMessageBrowser();

            if (environmentState.IsMessageBrowserActive)
            {
                environmentState.ActiveMessageBrowserSelectedMessageCount = _primaryControls.GetActiveMessageBrowser().SelectedItems.Count;

                environmentState.IsMessageBrowserQueueChildOfActiveQSetItem = 
                    environmentState.IsQSetActiveItemFolder
                    &&
                    ((QSetFolderItem)_primaryControls.GetQSetExplorerActiveItem())
                    .ChildItems
                    .Exists((_primaryControls.GetActiveMessageBrowser()).QSetQueueItem.Name);
            }

            return environmentState;
        }
    }
}
