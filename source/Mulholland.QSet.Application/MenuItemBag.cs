using System;
using TD.SandBar;

namespace Mulholland.QSet.Application
{
    /// <summary>
    /// Singleton, which provides a reference to each menu item.
    /// </summary>
    /// <remarks>
    /// The purpose of this object is to provide a single point of type safe access
    /// to all menu items, throughout the code.
    /// </remarks>
    internal class MenuItemBag
    {
        static MenuItemBag() {}

        //File menu
        public static TD.SandBar.MenuBarItem FileMenu;		
        public static TD.SandBar.MenuButtonItem FileNewQSet;
        public static TD.SandBar.MenuButtonItem FileOpenQSet;
        public static TD.SandBar.MenuButtonItem FileCloseQSet;
        public static TD.SandBar.MenuButtonItem FileSaveQSet;
        public static TD.SandBar.MenuButtonItem FileSaveQSetAs;
        public static TD.SandBar.MenuButtonItem FileRecentFileList;
        public static TD.SandBar.MenuButtonItem FileExit;
        public static TD.SandBar.MenuButtonItem FileNewMessage;

        //View menu
        public static TD.SandBar.MenuBarItem ViewMenu;		
        public static TD.SandBar.MenuButtonItem ViewQSetExplorer;
        public static TD.SandBar.MenuButtonItem ViewProperties;
        public static TD.SandBar.MenuButtonItem ViewMessageViewer;
        public static TD.SandBar.MenuButtonItem ViewQSetMonitor;
        
        //Q Set menu
        public static TD.SandBar.MenuBarItem QSetMenu;
        public static TD.SandBar.MenuButtonItem QSetAddActiveQueue;
        public static TD.SandBar.MenuButtonItem QSetNewFolder;
        public static TD.SandBar.MenuButtonItem QSetRenameFolder;
        public static TD.SandBar.MenuButtonItem QSetDeleteItem;
        public static TD.SandBar.MenuButtonItem QSetPurgeAllQueues;

        //Queue menu
        public static TD.SandBar.MenuBarItem QueueMenu;
        public static TD.SandBar.MenuButtonItem QueueOpen;
        public static TD.SandBar.MenuButtonItem QueueBrowse;
        public static TD.SandBar.MenuButtonItem QueueCreate;
        public static TD.SandBar.MenuButtonItem QueueDelete;
        public static TD.SandBar.MenuButtonItem QueueRefresh;
        public static TD.SandBar.MenuButtonItem QueuePurge;

        //Message Menu
        public static TD.SandBar.MenuBarItem MessageMenu;
        public static TD.SandBar.MenuButtonItem MessageNew;
        public static TD.SandBar.MenuButtonItem MessageForward;
        public static TD.SandBar.MenuButtonItem MessageMove;
        public static TD.SandBar.MenuButtonItem MessageDelete;

        //Tools menu
        public static TD.SandBar.MenuBarItem ToolsMenu;
        public static TD.SandBar.MenuButtonItem ToolsOptions;
        public static TD.SandBar.MenuButtonItem ToolsNewWebServiceClient;

        //Help menu
        public static TD.SandBar.MenuBarItem HelpMenu;
        public static TD.SandBar.MenuButtonItem HelpAbout;

        //Message Browser context menu
        public static TD.SandBar.ContextMenuBarItem MessageBrowserCtxMenu;

        public static TD.SandBar.MenuButtonItem MessageBrowserCtxForwardMessage;
        public static TD.SandBar.MenuButtonItem MessageBrowserCtxMoveMessage;
        public static TD.SandBar.MenuButtonItem MessageBrowserCtxDeleteMessage;
        public static TD.SandBar.MenuButtonItem MessageBrowserCtxRefreshMessages;
        public static TD.SandBar.MenuButtonItem MessageBrowserCtxPurgeQueue;
        
        //Q Set context menu
        public static TD.SandBar.ContextMenuBarItem QSetCtxMenu;
        public static TD.SandBar.MenuButtonItem QSetCtxAddActiveQueueToSet;
        public static TD.SandBar.MenuButtonItem QSetCtxNewFolder;
        public static TD.SandBar.MenuButtonItem QSetCtxRenameFolder;
        public static TD.SandBar.MenuButtonItem QSetCtxDeleteItem;
        public static TD.SandBar.MenuButtonItem QSetCtxDeleteQueue;		
        public static TD.SandBar.MenuButtonItem QSetCtxPurgeQueue;	
        public static TD.SandBar.MenuButtonItem QSetCtxNewMessage;
        public static TD.SandBar.MenuButtonItem QSetCtxNewWebServiceClient;

    }
}
