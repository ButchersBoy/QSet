using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
    public partial class QSetEnvironmentForm : Form
    {
        EnvironmentCoordinator _environmentCoordinator;

        public QSetEnvironmentForm()
        {
            InitializeComponent();
        }

        static void Main(string[] args)
        {
            System.Windows.Forms.Application.ThreadException += new ThreadExceptionEventHandler(ApplicationExceptionHandler);

            QSetEnvironmentForm mainForm = new QSetEnvironmentForm();
            if (args.Length == 1)
                mainForm.OpenQSet(args[0]);
            System.Windows.Forms.Application.Run(mainForm);
        }


        internal static void ApplicationExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show("Q Set has encountered an error.", "Q Set Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        /// <summary>
        /// Delegates to EnvironmentCoordinator.OpenQSet.
        /// </summary>
        /// <param name="path">Q Set filename.</param>
        public void OpenQSet(string path)
        {
            _environmentCoordinator.OpenQSet(path);
        }


        /// <summary>
        /// Configures the environment ready for use.
        /// </summary>		
        private void ConfigureEnvironment()
        {
            SetupMenuItemBag();

            Licensing.License license = new Licensing.License();

            //queueSetExplorer = new Mulholland.QSet.Application.Controls.QSetExplorer();
            //queueSetExplorer.Dock = DockStyle.Fill;
            //qSetExplorerDock.Controls.Add(queueSetExplorer);

            //defaultMessageViewer = new Mulholland.QSet.Application.Controls.MessageViewer(license);
            //defaultMessageViewer.Dock = DockStyle.Fill;
            //messageViewerDock.Controls.Add(defaultMessageViewer);

            //qSetMonitor = new Mulholland.QSet.Application.Controls.QSetMonitor();
            //qSetMonitor.Dock = DockStyle.Fill;
            //qSetMonitorDockControl.Controls.Add(qSetMonitor);

            //PrimaryMenus primaryMenus = new PrimaryMenus(
            //    MenuItemBag.FileMenu,
            //    MenuItemBag.ViewMenu,
            //    MenuItemBag.QSetMenu,
            //    MenuItemBag.QueueMenu,
            //    MenuItemBag.MessageMenu,
            //    MenuItemBag.ToolsMenu,
            //    MenuItemBag.HelpMenu,
            //    MenuItemBag.MessageBrowserCtxMenu,
            //    MenuItemBag.QSetCtxMenu);

            //mainDocumentContainer.Manager = new TD.SandDock.SandDockManager();

            //PrimaryControls primaryControls = new PrimaryControls(
            //    queueSetExplorer,
            //    qSetMonitor,
            //    propertyGrid,
            //    defaultMessageViewer,
            //    mainDocumentContainer,
            //    new Images());

            PrimaryForms primaryForms = new PrimaryForms(this, new QueueSearchForm());

            UserSettings userSettings = UserSettings.Create();
            PrimaryObjects primaryObjects = new PrimaryObjects(new ProcessVisualizer(this), userSettings, license);
            primaryObjects.ProcessVisualizer.StatusBarPanel = this.workingStatusBarPanel;

            //_environmentCoordinator = new EnvironmentCoordinator(primaryMenus, primaryControls, primaryForms, primaryObjects);
            //_environmentCoordinator.SetUp();
        }


        /// <summary>
        /// Populates the singleton MenuItemBag with all of the menu items.
        /// </summary>
        private void SetupMenuItemBag()
        {
            //File menu
            MenuItemBag.FileMenu = fileMenuBar;
            MenuItemBag.FileNewQSet = fileNewQSetMenuButton;
            MenuItemBag.FileOpenQSet = fileOpenQSetMenuButton;
            MenuItemBag.FileSaveQSet = fileSaveQSetMenuButton;
            MenuItemBag.FileSaveQSetAs = fileSaveQSetAsMenuButton;
            MenuItemBag.FileCloseQSet = fileCloseQSetMenuButton;
            MenuItemBag.FileRecentFileList = fileRecentFilesMenuButton;
            MenuItemBag.FileExit = fileExitMenuButton;
            MenuItemBag.FileNewMessage = fileNewMessageMenuButton;

            //View menu
            MenuItemBag.ViewMenu = viewMenuBar;
            MenuItemBag.ViewQSetExplorer = viewQSetExplorerMenuButton;
            MenuItemBag.ViewProperties = viewPropertiesWindowMenuButton;
            MenuItemBag.ViewMessageViewer = viewMessageViewerMenuButton;
            MenuItemBag.ViewQSetMonitor = viewQSetMonitorMenuButton;

            //Q Set menu
            MenuItemBag.QSetMenu = qSetMenuBar;
            MenuItemBag.QSetAddActiveQueue = qSetAddActiveQueueMenuButton;
            MenuItemBag.QSetNewFolder = qSetNewFolderMenuButton;
            MenuItemBag.QSetRenameFolder = qSetRenameFolderMenuButton;
            MenuItemBag.QSetDeleteItem = qSetDeleteItemMenuButton;
            MenuItemBag.QSetPurgeAllQueues = qSetPurgeAllQueuesMenuButton;

            //Queue menu
            MenuItemBag.QueueMenu = queueMenuBar;
            MenuItemBag.QueueOpen = queueOpenMenuButton;
            MenuItemBag.QueueCreate = queueCreateMenuButton;
            MenuItemBag.QueueDelete = queueDeleteMenuButton;
            MenuItemBag.QueueBrowse = queueBrowseMenuButton;
            MenuItemBag.QueueRefresh = queueRefreshMenuButton;
            MenuItemBag.QueuePurge = queuePurgeMenuButton;

            //Message menu
            MenuItemBag.MessageMenu = messageMenuBar;
            MenuItemBag.MessageNew = messageNewMenuButton;
            MenuItemBag.MessageForward = messageForwardMenuButton;
            MenuItemBag.MessageMove = messageMoveMenuButton;
            MenuItemBag.MessageDelete = messageDeleteMenuButton;

            //Tools menu
            MenuItemBag.ToolsMenu = toolsMenuBar;
            MenuItemBag.ToolsOptions = toolsOptionsMenuButton;
            MenuItemBag.ToolsNewWebServiceClient = toolsNewWebServiceClientMenuButton;

            //File menu
            MenuItemBag.HelpMenu = helpMenuBar;
            MenuItemBag.HelpAbout = helpAboutMenuButton;

            //Message Browser context menu
            MenuItemBag.MessageBrowserCtxMenu = messageBrowserCtxMenu;
            MenuItemBag.MessageBrowserCtxForwardMessage = messageBrowserCtxForwardMessageMenuButton;
            MenuItemBag.MessageBrowserCtxMoveMessage = messageBrowserCtxMoveMessageMenuButton;
            MenuItemBag.MessageBrowserCtxDeleteMessage = messageBrowserCtxDeleteMessageMenuButton;
            MenuItemBag.MessageBrowserCtxRefreshMessages = messageBrowserCtxRefreshMessagesMenuButton;
            MenuItemBag.MessageBrowserCtxPurgeQueue = messageBrowserCtxPurgeQueueMenuButton;

            //QSet context menu
            MenuItemBag.QSetCtxMenu = qSetCtxMenu;
            MenuItemBag.QSetCtxAddActiveQueueToSet = qSetCtxAddActiveQueueToSet;
            MenuItemBag.QSetCtxNewFolder = qSetCtxNewFolder;
            MenuItemBag.QSetCtxRenameFolder = qSetCtxRenameFolder;
            MenuItemBag.QSetCtxDeleteItem = qSetCtxDeleteItem;
            MenuItemBag.QSetCtxPurgeAllQueues = qSetCtxPurgeAllQueues;
            MenuItemBag.QSetCtxDeleteQueue = qSetCtxDeleteQueue;
            MenuItemBag.QSetCtxNewMessage = qSetCtxSendMessage;
            MenuItemBag.QSetCtxPurgeQueue = qSetCtxPurgeQueue;
            MenuItemBag.QSetCtxNewWebServiceClient = qSetCtxNewWebServiceClient;
        }


        private void QSetEnvironmentForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_environmentCoordinator.HasShutDown)
                e.Cancel = !_environmentCoordinator.ShutDown();
        }
    }
}
