using System;
using System.Threading;
using System.Windows.Forms;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
    public partial class QSetEnvironmentForm : Form
    {
        EnvironmentCoordinator _environmentCoordinator;

        public QSetEnvironmentForm()
        {
            InitializeComponent();

            ConfigureEnvironment();
        }

        [STAThread]
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

            PrimaryMenus primaryMenus = new PrimaryMenus(
                MenuItemBag.FileMenu,
                MenuItemBag.ViewMenu,
                MenuItemBag.QSetMenu,
                MenuItemBag.QueueMenu,
                MenuItemBag.MessageMenu,
                MenuItemBag.ToolsMenu,
                MenuItemBag.HelpMenu,
                MenuItemBag.MessageBrowserCtxMenu,
                MenuItemBag.QSetCtxMenu);

            PrimaryControls primaryControls = new PrimaryControls(
                license,
                this.dockPanel,
                new Images());

            PrimaryForms primaryForms = new PrimaryForms(this, new QueueSearchForm());

            UserSettings userSettings = UserSettings.Create();
            PrimaryObjects primaryObjects = new PrimaryObjects(new ProcessVisualizer(this), userSettings, license);
            primaryObjects.ProcessVisualizer.StatusBarPanel = this.workingStatusBarPanel;

            _environmentCoordinator = new EnvironmentCoordinator(primaryMenus, primaryControls, primaryForms, primaryObjects);
            _environmentCoordinator.SetUp();
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

        private void fileNewQSetToolButton_Click(object sender, EventArgs e)
        {
            fileNewQSetMenuButton.PerformClick();
        }

        private void fileOpenQSetToolButton_Click(object sender, EventArgs e)
        {
            fileOpenQSetMenuButton.PerformClick();
        }

        private void fileSaveQSetToolButton_Click(object sender, EventArgs e)
        {
            fileSaveQSetAsMenuButton.PerformClick();
        }

        private void fileNewMessageToolButton_Click(object sender, EventArgs e)
        {
            fileNewMessageMenuButton.PerformClick();
        }

        private void qSetAddActiveQueueToolButton_Click(object sender, EventArgs e)
        {
            qSetAddActiveQueueMenuButton.PerformClick();
        }

        private void viewQSetExplorerToolButton_Click(object sender, EventArgs e)
        {
            viewQSetExplorerMenuButton.PerformClick();
        }

        private void viewPropertiesWindowToolButton_Click(object sender, EventArgs e)
        {
            viewPropertiesWindowMenuButton.PerformClick();
        }

        private void viewMessageViewerToolButton_Click(object sender, EventArgs e)
        {
            viewMessageViewerMenuButton.PerformClick();
        }

        private void viewQSetMonitorToolButton_Click(object sender, EventArgs e)
        {
            viewQSetMonitorMenuButton.PerformClick();
        }

        private void qSetNewFolderToolButton_Click(object sender, EventArgs e)
        {
            qSetNewFolderMenuButton.PerformClick();
        }

        private void qSetRenameFolderToolButton_Click(object sender, EventArgs e)
        {
            qSetRenameFolderMenuButton.PerformClick();
        }

        private void queueOpenToolButton_Click(object sender, EventArgs e)
        {
            queueOpenMenuButton.PerformClick();
        }

        private void queueBrowseToolButton_Click(object sender, EventArgs e)
        {
            queueBrowseMenuButton.PerformClick();
        }

        private void queueCreateToolButton_Click(object sender, EventArgs e)
        {
            queueCreateMenuButton.PerformClick();
        }

        private void queueDeleteToolButton_Click(object sender, EventArgs e)
        {
            queueDeleteMenuButton.PerformClick();
        }

        private void queueRefreshToolButton_Click(object sender, EventArgs e)
        {
            queueRefreshMenuButton.PerformClick();
        }

        private void queuePurgeToolButton_Click(object sender, EventArgs e)
        {
            queuePurgeMenuButton.PerformClick();
        }

        private void messageNewToolButton_Click(object sender, EventArgs e)
        {
            messageNewMenuButton.PerformClick();
        }

        private void messageForwardToolButton_Click(object sender, EventArgs e)
        {
            messageForwardMenuButton.PerformClick();
        }

        private void messageMoveToolButton_Click(object sender, EventArgs e)
        {
            messageMoveMenuButton.PerformClick();
        }

        private void messageDeleteToolButton_Click(object sender, EventArgs e)
        {
            messageDeleteMenuButton.PerformClick();
        }

        private void viewQSetExplorerMenuButton_CheckedChanged(object sender, EventArgs e)
        {
            viewQSetExplorerToolButton.Checked = viewQSetExplorerMenuButton.Checked;
        }

        private void viewPropertiesWindowMenuButton_CheckedChanged(object sender, EventArgs e)
        {
            viewPropertiesWindowToolButton.Checked = viewPropertiesWindowMenuButton.Checked;
        }

        private void viewMessageViewerMenuButton_CheckedChanged(object sender, EventArgs e)
        {
            viewMessageViewerToolButton.Checked = viewMessageViewerMenuButton.Checked;
        }

        private void viewQSetMonitorMenuButton_CheckedChanged(object sender, EventArgs e)
        {
            viewQSetMonitorToolButton.Checked = viewQSetMonitorMenuButton.Checked;
        }

        private void queueOpenMenuButton_VisibleChanged(object sender, EventArgs e)
        {
            queueOpenToolButton.Visible = queueOpenMenuButton.Visible;
        }

        private void queueOpenMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            queueOpenToolButton.Enabled = queueOpenMenuButton.Enabled;
        }

        private void queueBrowseMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            queueBrowseToolButton.Enabled = queueBrowseMenuButton.Enabled;
        }

        private void queueCreateMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            queueCreateToolButton.Enabled = queueCreateMenuButton.Enabled;
        }

        private void queueDeleteMenuButton_EnabledChanged(object sender, EventArgs e)
        {
             queueDeleteToolButton.Enabled = queueDeleteMenuButton.Enabled;
        }

        private void queueRefreshMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            queueRefreshToolButton.Enabled = queueRefreshMenuButton.Enabled;
        }

        private void queuePurgeMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            queuePurgeToolButton.Enabled = queuePurgeMenuButton.Enabled;
        }

        private void messageNewMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            messageNewToolButton.Enabled = messageNewMenuButton.Enabled;
        }

        private void messageForwardMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            messageForwardToolButton.Enabled = messageForwardMenuButton.Enabled;
        }

        private void messageMoveMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            messageMoveToolButton.Enabled = messageMoveMenuButton.Enabled;
        }

        private void messageDeleteMenuButton_EnabledChanged(object sender, EventArgs e)
        {
            messageDeleteToolButton.Enabled = messageDeleteMenuButton.Enabled;
        }
    }
}
