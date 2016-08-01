using System;
using System.Windows.Forms;

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
        public static ToolStripMenuItem FileMenu;		
        public static ToolStripMenuItem FileNewQSet;
        public static ToolStripMenuItem FileOpenQSet;
        public static ToolStripMenuItem FileCloseQSet;
        public static ToolStripMenuItem FileSaveQSet;
        public static ToolStripMenuItem FileSaveQSetAs;
        public static ToolStripMenuItem FileRecentFileList;
        public static ToolStripMenuItem FileExit;
        public static ToolStripMenuItem FileNewMessage;

        //View menu
        public static ToolStripMenuItem ViewMenu;		
        public static ToolStripMenuItem ViewQSetExplorer;
        public static ToolStripMenuItem ViewProperties;
        public static ToolStripMenuItem ViewMessageViewer;
        public static ToolStripMenuItem ViewQSetMonitor;
        
        //Q Set menu
        public static ToolStripMenuItem QSetMenu;
        public static ToolStripMenuItem QSetAddActiveQueue;
        public static ToolStripMenuItem QSetNewFolder;
        public static ToolStripMenuItem QSetRenameFolder;
        public static ToolStripMenuItem QSetDeleteItem;
        public static ToolStripMenuItem QSetPurgeAllQueues;

        //Queue menu
        public static ToolStripMenuItem QueueMenu;
        public static ToolStripMenuItem QueueOpen;
        public static ToolStripMenuItem QueueBrowse;
        public static ToolStripMenuItem QueueCreate;
        public static ToolStripMenuItem QueueDelete;
        public static ToolStripMenuItem QueueRefresh;
        public static ToolStripMenuItem QueuePurge;

        //Message Menu
        public static ToolStripMenuItem MessageMenu;
        public static ToolStripMenuItem MessageNew;
        public static ToolStripMenuItem MessageForward;
        public static ToolStripMenuItem MessageMove;
        public static ToolStripMenuItem MessageDelete;

        //Tools menu
        public static ToolStripMenuItem ToolsMenu;
        public static ToolStripMenuItem ToolsOptions;
        public static ToolStripMenuItem ToolsNewWebServiceClient;

        //Help menu
        public static ToolStripMenuItem HelpMenu;
        public static ToolStripMenuItem HelpAbout;

        //Message Browser context menu
        public static ContextMenuStrip MessageBrowserCtxMenu;

        public static ToolStripMenuItem MessageBrowserCtxForwardMessage;
        public static ToolStripMenuItem MessageBrowserCtxMoveMessage;
        public static ToolStripMenuItem MessageBrowserCtxDeleteMessage;
        public static ToolStripMenuItem MessageBrowserCtxRefreshMessages;
        public static ToolStripMenuItem MessageBrowserCtxPurgeQueue;
        
        //Q Set context menu
        public static ContextMenuStrip QSetCtxMenu;
        public static ToolStripMenuItem QSetCtxAddActiveQueueToSet;
        public static ToolStripMenuItem QSetCtxNewFolder;
        public static ToolStripMenuItem QSetCtxRenameFolder;
        public static ToolStripMenuItem QSetCtxDeleteItem;
        public static ToolStripMenuItem QSetCtxPurgeAllQueues;
        public static ToolStripMenuItem QSetCtxDeleteQueue;		
        public static ToolStripMenuItem QSetCtxPurgeQueue;	
        public static ToolStripMenuItem QSetCtxNewMessage;
        public static ToolStripMenuItem QSetCtxNewWebServiceClient;

    }
}
