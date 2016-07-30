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
			MenuItemBag.QSetCtxDeleteQueue.Visible = state.IsQSetActiveItemQueue;			
			MenuItemBag.QSetCtxNewFolder.Visible = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet) && !state.IsQSetActiveItemMachine);
			MenuItemBag.QSetCtxRenameFolder.Visible = (state.IsQSetOpen && (state.IsQSetActiveItemFolder || state.IsQSetActiveItemQSet || state.IsQSetActiveItemWebService) && !state.IsQSetActiveItemMachine);
			MenuItemBag.QSetCtxPurgeQueue.Visible = state.IsQSetActiveItemQueue;
			MenuItemBag.QSetCtxNewWebServiceClient.Visible = false;
			//MenuItemBag.QSetCtxNewWebServiceClient.Visible = state.IsQSetActiveItemFolder && !state.IsQSetActiveItemMachine;
		}


		private EnvironmentState GetEnvironmentState()
		{
			//ascertain the state of the environment
			EnvironmentState environmentState = new EnvironmentState();
			environmentState.IsQSetOpen = _primaryControls.QSetExplorer.QSet != null;
			environmentState.IsQSetDirty = environmentState.IsQSetOpen && _primaryControls.QSetExplorer.QSet.IsDirty;
			environmentState.IsQSetItemActive = (environmentState.IsQSetOpen && _primaryControls.QSetExplorer.ActiveItem != null);
			if (environmentState.IsQSetItemActive)
			{				
				environmentState.IsQSetActiveItemQueue = _primaryControls.QSetExplorer.ActiveItem is QSetQueueItem;
				environmentState.IsQSetActiveItemFolder = _primaryControls.QSetExplorer.ActiveItem is QSetFolderItem;
				environmentState.IsQSetActiveItemMachine = _primaryControls.QSetExplorer.ActiveItem is QSetMachineItem;
				environmentState.IsQSetActiveItemQSet = _primaryControls.QSetExplorer.ActiveItem is QSetModel;
				environmentState.IsQSetActiveItemQueue = _primaryControls.QSetExplorer.ActiveItem is QSetQueueItem;
				environmentState.IsQSetActiveItemWebService = _primaryControls.QSetExplorer.ActiveItem is QSetWebServiceItem;

				if (_primaryControls.QSetExplorer.ActiveItem.ParentItem != null)
				{					
					environmentState.IsQSetParentItemMachine = _primaryControls.QSetExplorer.ActiveItem.ParentItem is QSetMachineItem;
					environmentState.IsQSetParentItemQSet = _primaryControls.QSetExplorer.ActiveItem.ParentItem is QSetModel;					
				}
			}
            environmentState.IsMessageBrowserActive = _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument != null &&
                _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls.Count > 0 &&
                _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0] is MessageBrowser;
			if (environmentState.IsMessageBrowserActive)
			{
                environmentState.ActiveMessageBrowserSelectedMessageCount = ((MessageBrowser)_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0]).SelectedItems.Count;

				environmentState.IsMessageBrowserQueueChildOfActiveQSetItem
                    = environmentState.IsQSetActiveItemFolder && ((QSetFolderItem)_primaryControls.QSetExplorer.ActiveItem).ChildItems.Exists(((MessageBrowser)_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0]).QSetQueueItem.Name);
			}

			return environmentState;
		}
	}
}
