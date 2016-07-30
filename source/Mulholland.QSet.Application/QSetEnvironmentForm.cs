using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for Form1.PrimaryControls
	/// </summary>
	public class QSetEnvironmentForm : System.Windows.Forms.Form
	{		
		EnvironmentCoordinator _environmentCoordinator;
		private TD.SandDock.DockContainer leftSandDock;
		private TD.SandDock.DockContainer rightSandDock;
		private TD.SandDock.DockContainer bottomSandDock;
		private TD.SandDock.DockContainer topSandDock;		
		private TD.SandDock.DocumentContainer mainDocumentContainer;
		private Mulholland.QSet.Application.Controls.MessageViewer defaultMessageViewer;
		private Mulholland.QSet.Application.Controls.QSetExplorer queueSetExplorer;
		private TD.SandBar.MenuButtonItem menuButton2;
		private TD.SandBar.MenuButtonItem menuButton4;
		private TD.SandBar.MenuButtonItem menuButton6;
        private System.Windows.Forms.PropertyGrid propertyGrid;
        private TD.SandDock.DockableWindow qSetExplorerDock;
        private TD.SandDock.DockableWindow propertyGridDock;
        private TD.SandDock.DockableWindow messageViewerDock;
		private TD.SandBar.SandBarManager sandBarManager;
		private TD.SandBar.ToolBarContainer leftSandBarDock;
		private TD.SandBar.ToolBarContainer rightSandBarDock;
		private TD.SandBar.ToolBarContainer bottomSandBarDock;
		private TD.SandBar.ToolBarContainer topSandBarDock;
		private TD.SandBar.MenuBar mainMenuBar;
		private TD.SandBar.MenuBarItem fileMenuBar;
		private TD.SandBar.MenuBarItem helpMenuBar;
		private TD.SandBar.MenuBarItem qSetMenuBar;
		private TD.SandBar.MenuBarItem queueMenuBar;
		private TD.SandBar.ToolBar mainToolBar;
		private TD.SandBar.MenuButtonItem fileNewQSetMenuButton;
		private TD.SandBar.MenuButtonItem fileOpenQSetMenuButton;
		private TD.SandBar.MenuButtonItem fileCloseQSetMenuButton;
		private TD.SandBar.MenuButtonItem fileSaveQSetMenuButton;
		private TD.SandBar.MenuButtonItem fileSaveQSetAsMenuButton;
		private TD.SandBar.MenuButtonItem fileExitMenuButton;
		private TD.SandBar.MenuButtonItem qSetAddActiveQueueMenuButton;
		private TD.SandBar.MenuButtonItem qSetNewFolderMenuButton;
		private TD.SandBar.MenuButtonItem qSetRenameFolderMenuButton;
		private TD.SandBar.MenuButtonItem qSetDeleteItemMenuButton;
        private TD.SandBar.MenuButtonItem qSetPurgeAllQueuesMenuButton;
        private TD.SandBar.MenuButtonItem queueOpenMenuButton;
		private TD.SandBar.MenuButtonItem queueRefreshMenuButton;
		private TD.SandBar.MenuButtonItem helpAboutMenuButton;
		private TD.SandBar.ButtonItem fileNewQSetToolButton;
		private TD.SandBar.ButtonItem fileOpenQSetToolButton;
		private TD.SandBar.ButtonItem fileSaveQSetToolButton;
		private TD.SandBar.ButtonItem qSetAddActiveQueueToolButton;
		private TD.SandBar.ButtonItem qSetNewFolderToolButton;
		private TD.SandBar.ButtonItem qSetRenameFolderToolButton;
		private TD.SandBar.ButtonItem qSetDeleteItemToolButton;
		private TD.SandBar.ButtonItem queueOpenToolButton;
		private TD.SandBar.ButtonItem queueRefreshToolButton;
		private TD.SandDock.SandDockManager sandDockManager;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel workingStatusBarPanel;
		private TD.SandBar.MenuButtonItem queueBrowseMenuButton;
        private TD.SandBar.ButtonItem queueBrowseToolButton;
        private TD.SandDock.DockableWindow qSetMonitorDockControl;
		private Mulholland.QSet.Application.Controls.QSetMonitor qSetMonitor;
		private TD.SandBar.MenuBarItem viewMenuBar;
		private TD.SandBar.MenuButtonItem viewQSetExplorerMenuButton;
		private TD.SandBar.MenuButtonItem viewPropertiesWindowMenuButton;
		private TD.SandBar.MenuButtonItem viewMessageViewerMenuButton;
		private TD.SandBar.MenuButtonItem viewQSetMonitorMenuButton;
		private TD.SandBar.ButtonItem viewQSetMonitorToolButton;
		private TD.SandBar.ButtonItem viewMessageViewerToolButton;
		private TD.SandBar.ButtonItem viewPropertiesWindowToolButton;
		private TD.SandBar.ButtonItem viewQSetExplorerToolButton;
		private TD.SandBar.MenuButtonItem fileRecentFilesMenuButton;
		private TD.SandBar.MenuBarItem toolsMenuBar;
		private TD.SandBar.MenuButtonItem toolsOptionsMenuButton;
		private TD.SandBar.MenuButtonItem queuePurgeMenuButton;
		private TD.SandBar.ButtonItem queuePurgeToolButton;
		private TD.SandBar.MenuButtonItem messageBrowserCtxDeleteMessageMenuButton;
		private TD.SandBar.MenuButtonItem messageBrowserCtxRefreshMessagesMenuButton;
		private TD.SandBar.ContextMenuBarItem messageBrowserCtxMenu;
		private TD.SandBar.ButtonItem fileNewMessageToolButton;
		private TD.SandBar.MenuButtonItem fileNewMessageMenuButton;
		private TD.SandBar.ContextMenuBarItem qSetCtxMenu;
		private TD.SandBar.MenuButtonItem qSetCtxSendMessage;
		private TD.SandBar.MenuButtonItem queueCreateMenuButton;
		private TD.SandBar.ButtonItem queueCreateToolButton;
		private TD.SandBar.ButtonItem queueDeleteToolButton;
		private TD.SandBar.MenuButtonItem queueDeleteMenuButton;
		private TD.SandBar.MenuButtonItem qSetCtxAddActiveQueueToSet;
		private TD.SandBar.MenuButtonItem qSetCtxNewFolder;
		private TD.SandBar.MenuButtonItem qSetCtxRenameFolder;
		private TD.SandBar.MenuButtonItem qSetCtxDeleteItem;
		private TD.SandBar.MenuButtonItem qSetCtxDeleteQueue;
		private TD.SandBar.MenuBarItem messageMenuBar;
		private TD.SandBar.MenuButtonItem messageNewMenuButton;
		private TD.SandBar.MenuButtonItem messageForwardMenuButton;
		private TD.SandBar.MenuButtonItem messageMoveMenuButton;
		private TD.SandBar.MenuButtonItem messageDeleteMenuButton;
		private TD.SandBar.ButtonItem messageNewToolButton;
		private TD.SandBar.ButtonItem messageDeleteToolButton;
		private TD.SandBar.ButtonItem messageMoveToolButton;
		private TD.SandBar.ButtonItem messageForwardToolButton;
		private TD.SandBar.MenuButtonItem messageBrowserCtxForwardMessageMenuButton;
		private TD.SandBar.ButtonItem buttonItem1;
		private TD.SandBar.MenuButtonItem messageBrowserCtxMoveMessageMenuButton;
		private TD.SandBar.MenuButtonItem messageBrowserCtxPurgeQueueMenuButton;
		private TD.SandBar.MenuButtonItem qSetCtxPurgeQueue;
		private TD.SandBar.MenuButtonItem toolsNewWebServiceClientMenuButton;
        private TD.SandBar.MenuButtonItem qSetCtxNewWebServiceClient;
        private IContainer components;

		
		/// <summary>
		/// Constructs the environment form.
		/// </summary>
		public QSetEnvironmentForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
					
			ConfigureEnvironment();
			
			//property kept resetting in designer, so set here
			propertyGrid.HelpVisible = true;
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QSetEnvironmentForm));
            this.mainDocumentContainer = new TD.SandDock.DocumentContainer();
            this.sandDockManager = new TD.SandDock.SandDockManager();
            this.leftSandDock = new TD.SandDock.DockContainer();
            this.qSetExplorerDock = new TD.SandDock.DockableWindow();
            this.propertyGridDock = new TD.SandDock.DockableWindow();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.rightSandDock = new TD.SandDock.DockContainer();
            this.messageViewerDock = new TD.SandDock.DockableWindow();
            this.bottomSandDock = new TD.SandDock.DockContainer();
            this.qSetMonitorDockControl = new TD.SandDock.DockableWindow();
            this.topSandDock = new TD.SandDock.DockContainer();
            this.menuButton2 = new TD.SandBar.MenuButtonItem();
            this.menuButton4 = new TD.SandBar.MenuButtonItem();
            this.menuButton6 = new TD.SandBar.MenuButtonItem();
            this.sandBarManager = new TD.SandBar.SandBarManager(this.components);
            this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
            this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
            this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
            this.topSandBarDock = new TD.SandBar.ToolBarContainer();
            this.mainMenuBar = new TD.SandBar.MenuBar();
            this.messageBrowserCtxMenu = new TD.SandBar.ContextMenuBarItem();
            this.messageBrowserCtxForwardMessageMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageBrowserCtxMoveMessageMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageBrowserCtxDeleteMessageMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageBrowserCtxRefreshMessagesMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageBrowserCtxPurgeQueueMenuButton = new TD.SandBar.MenuButtonItem();
            this.qSetCtxMenu = new TD.SandBar.ContextMenuBarItem();
            this.qSetCtxAddActiveQueueToSet = new TD.SandBar.MenuButtonItem();
            this.qSetCtxNewFolder = new TD.SandBar.MenuButtonItem();
            this.qSetCtxRenameFolder = new TD.SandBar.MenuButtonItem();
            this.qSetCtxDeleteItem = new TD.SandBar.MenuButtonItem();
            this.qSetCtxSendMessage = new TD.SandBar.MenuButtonItem();
            this.qSetCtxNewWebServiceClient = new TD.SandBar.MenuButtonItem();
            this.qSetCtxPurgeQueue = new TD.SandBar.MenuButtonItem();
            this.qSetCtxDeleteQueue = new TD.SandBar.MenuButtonItem();
            this.fileMenuBar = new TD.SandBar.MenuBarItem();
            this.fileNewQSetMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileOpenQSetMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileCloseQSetMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileNewMessageMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileSaveQSetMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileSaveQSetAsMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileRecentFilesMenuButton = new TD.SandBar.MenuButtonItem();
            this.fileExitMenuButton = new TD.SandBar.MenuButtonItem();
            this.viewMenuBar = new TD.SandBar.MenuBarItem();
            this.viewQSetExplorerMenuButton = new TD.SandBar.MenuButtonItem();
            this.viewPropertiesWindowMenuButton = new TD.SandBar.MenuButtonItem();
            this.viewMessageViewerMenuButton = new TD.SandBar.MenuButtonItem();
            this.viewQSetMonitorMenuButton = new TD.SandBar.MenuButtonItem();
            this.qSetMenuBar = new TD.SandBar.MenuBarItem();
            this.qSetAddActiveQueueMenuButton = new TD.SandBar.MenuButtonItem();
            this.qSetNewFolderMenuButton = new TD.SandBar.MenuButtonItem();
            this.qSetRenameFolderMenuButton = new TD.SandBar.MenuButtonItem();
            this.qSetDeleteItemMenuButton = new TD.SandBar.MenuButtonItem();
            this.qSetPurgeAllQueuesMenuButton = new TD.SandBar.MenuButtonItem();
            this.queueMenuBar = new TD.SandBar.MenuBarItem();
            this.queueOpenMenuButton = new TD.SandBar.MenuButtonItem();
            this.queueBrowseMenuButton = new TD.SandBar.MenuButtonItem();
            this.queueCreateMenuButton = new TD.SandBar.MenuButtonItem();
            this.queueDeleteMenuButton = new TD.SandBar.MenuButtonItem();
            this.queueRefreshMenuButton = new TD.SandBar.MenuButtonItem();
            this.queuePurgeMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageMenuBar = new TD.SandBar.MenuBarItem();
            this.messageNewMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageForwardMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageMoveMenuButton = new TD.SandBar.MenuButtonItem();
            this.messageDeleteMenuButton = new TD.SandBar.MenuButtonItem();
            this.toolsMenuBar = new TD.SandBar.MenuBarItem();
            this.toolsNewWebServiceClientMenuButton = new TD.SandBar.MenuButtonItem();
            this.toolsOptionsMenuButton = new TD.SandBar.MenuButtonItem();
            this.helpMenuBar = new TD.SandBar.MenuBarItem();
            this.helpAboutMenuButton = new TD.SandBar.MenuButtonItem();
            this.mainToolBar = new TD.SandBar.ToolBar();
            this.fileNewQSetToolButton = new TD.SandBar.ButtonItem();
            this.fileOpenQSetToolButton = new TD.SandBar.ButtonItem();
            this.fileSaveQSetToolButton = new TD.SandBar.ButtonItem();
            this.fileNewMessageToolButton = new TD.SandBar.ButtonItem();
            this.viewQSetExplorerToolButton = new TD.SandBar.ButtonItem();
            this.viewPropertiesWindowToolButton = new TD.SandBar.ButtonItem();
            this.viewMessageViewerToolButton = new TD.SandBar.ButtonItem();
            this.viewQSetMonitorToolButton = new TD.SandBar.ButtonItem();
            this.qSetAddActiveQueueToolButton = new TD.SandBar.ButtonItem();
            this.qSetNewFolderToolButton = new TD.SandBar.ButtonItem();
            this.qSetRenameFolderToolButton = new TD.SandBar.ButtonItem();
            this.qSetDeleteItemToolButton = new TD.SandBar.ButtonItem();
            this.queueOpenToolButton = new TD.SandBar.ButtonItem();
            this.queueBrowseToolButton = new TD.SandBar.ButtonItem();
            this.queueCreateToolButton = new TD.SandBar.ButtonItem();
            this.queueDeleteToolButton = new TD.SandBar.ButtonItem();
            this.queueRefreshToolButton = new TD.SandBar.ButtonItem();
            this.queuePurgeToolButton = new TD.SandBar.ButtonItem();
            this.messageNewToolButton = new TD.SandBar.ButtonItem();
            this.messageForwardToolButton = new TD.SandBar.ButtonItem();
            this.messageMoveToolButton = new TD.SandBar.ButtonItem();
            this.messageDeleteToolButton = new TD.SandBar.ButtonItem();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.workingStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.buttonItem1 = new TD.SandBar.ButtonItem();
            this.leftSandDock.SuspendLayout();
            this.propertyGridDock.SuspendLayout();
            this.bottomSandDock.SuspendLayout();
            this.topSandBarDock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workingStatusBarPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // mainDocumentContainer
            // 
            this.mainDocumentContainer.ContentSize = 400;
            this.mainDocumentContainer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainDocumentContainer.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[0]);
            this.mainDocumentContainer.Location = new System.Drawing.Point(228, 49);
            this.mainDocumentContainer.Manager = null;
            this.mainDocumentContainer.Name = "mainDocumentContainer";
            this.mainMenuBar.SetSandBarMenu(this.mainDocumentContainer, this.messageBrowserCtxMenu);
            this.mainDocumentContainer.Size = new System.Drawing.Size(632, 396);
            this.mainDocumentContainer.TabIndex = 0;
            // 
            // sandDockManager
            // 
            this.sandDockManager.DockSystemContainer = this;
            this.sandDockManager.OwnerForm = this;
            // 
            // leftSandDock
            // 
            this.leftSandDock.ContentSize = 250;
            this.leftSandDock.Controls.Add(this.qSetExplorerDock);
            this.leftSandDock.Controls.Add(this.propertyGridDock);
            this.leftSandDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(224F, 394F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.qSetExplorerDock)),
                        ((TD.SandDock.DockControl)(this.propertyGridDock))}, this.qSetExplorerDock)))});
            this.leftSandDock.Location = new System.Drawing.Point(0, 49);
            this.leftSandDock.Manager = this.sandDockManager;
            this.leftSandDock.Name = "leftSandDock";
            this.leftSandDock.Size = new System.Drawing.Size(228, 396);
            this.leftSandDock.TabIndex = 1;
            // 
            // qSetExplorerDock
            // 
            this.qSetExplorerDock.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.qSetExplorerDock.Guid = new System.Guid("901719fd-4be9-4dcf-b58a-9b47b7eafe84");
            this.qSetExplorerDock.Location = new System.Drawing.Point(0, 18);
            this.qSetExplorerDock.Name = "qSetExplorerDock";
            this.mainMenuBar.SetSandBarMenu(this.qSetExplorerDock, this.qSetCtxMenu);
            this.qSetExplorerDock.Size = new System.Drawing.Size(224, 354);
            this.qSetExplorerDock.TabImage = ((System.Drawing.Image)(resources.GetObject("qSetExplorerDock.TabImage")));
            this.qSetExplorerDock.TabIndex = 0;
            this.qSetExplorerDock.Text = "Q Set Explorer";
            // 
            // propertyGridDock
            // 
            this.propertyGridDock.Controls.Add(this.propertyGrid);
            this.propertyGridDock.Guid = new System.Guid("e53b1c73-efe6-400d-bbab-bcc38ddd3e5a");
            this.propertyGridDock.Location = new System.Drawing.Point(0, 0);
            this.propertyGridDock.Name = "propertyGridDock";
            this.propertyGridDock.Size = new System.Drawing.Size(224, 355);
            this.propertyGridDock.TabImage = ((System.Drawing.Image)(resources.GetObject("propertyGridDock.TabImage")));
            this.propertyGridDock.TabIndex = 1;
            this.propertyGridDock.Text = "Properties";
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.HelpVisible = false;
            this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(224, 355);
            this.propertyGrid.TabIndex = 0;
            this.propertyGrid.ToolbarVisible = false;
            // 
            // rightSandDock
            // 
            this.rightSandDock.ContentSize = 250;
            this.rightSandDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[0]);
            this.rightSandDock.Location = new System.Drawing.Point(860, 49);
            this.rightSandDock.Manager = this.sandDockManager;
            this.rightSandDock.Name = "rightSandDock";
            this.rightSandDock.Size = new System.Drawing.Size(0, 396);
            this.rightSandDock.TabIndex = 2;
            // 
            // messageViewerDock
            // 
            this.messageViewerDock.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageViewerDock.Guid = new System.Guid("10722582-3282-45a3-b28e-a62d4bb86d0f");
            this.messageViewerDock.Location = new System.Drawing.Point(0, 22);
            this.messageViewerDock.Name = "messageViewerDock";
            this.messageViewerDock.Size = new System.Drawing.Size(860, 152);
            this.messageViewerDock.TabImage = ((System.Drawing.Image)(resources.GetObject("messageViewerDock.TabImage")));
            this.messageViewerDock.TabIndex = 1;
            this.messageViewerDock.Text = "Message Viewer";
            // 
            // bottomSandDock
            // 
            this.bottomSandDock.ContentSize = 400;
            this.bottomSandDock.Controls.Add(this.messageViewerDock);
            this.bottomSandDock.Controls.Add(this.qSetMonitorDockControl);
            this.bottomSandDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[] {
            ((TD.SandDock.LayoutSystemBase)(new TD.SandDock.ControlLayoutSystem(new System.Drawing.SizeF(860F, 194F), new TD.SandDock.DockControl[] {
                        ((TD.SandDock.DockControl)(this.messageViewerDock)),
                        ((TD.SandDock.DockControl)(this.qSetMonitorDockControl))}, this.messageViewerDock)))});
            this.bottomSandDock.Location = new System.Drawing.Point(0, 445);
            this.bottomSandDock.Manager = this.sandDockManager;
            this.bottomSandDock.Name = "bottomSandDock";
            this.bottomSandDock.Size = new System.Drawing.Size(860, 198);
            this.bottomSandDock.TabIndex = 3;
            // 
            // qSetMonitorDockControl
            // 
            this.qSetMonitorDockControl.Guid = new System.Guid("e0ae9663-99b8-4f45-a83c-79ef1eb64349");
            this.qSetMonitorDockControl.Location = new System.Drawing.Point(0, 0);
            this.qSetMonitorDockControl.Name = "qSetMonitorDockControl";
            this.qSetMonitorDockControl.Size = new System.Drawing.Size(860, 155);
            this.qSetMonitorDockControl.TabImage = ((System.Drawing.Image)(resources.GetObject("qSetMonitorDockControl.TabImage")));
            this.qSetMonitorDockControl.TabIndex = 2;
            this.qSetMonitorDockControl.Text = "Q Set Monitor";
            // 
            // topSandDock
            // 
            this.topSandDock.ContentSize = 400;
            this.topSandDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandDock.LayoutSystem = new TD.SandDock.SplitLayoutSystem(new System.Drawing.SizeF(250F, 400F), System.Windows.Forms.Orientation.Horizontal, new TD.SandDock.LayoutSystemBase[0]);
            this.topSandDock.Location = new System.Drawing.Point(0, 49);
            this.topSandDock.Manager = this.sandDockManager;
            this.topSandDock.Name = "topSandDock";
            this.topSandDock.Size = new System.Drawing.Size(860, 0);
            this.topSandDock.TabIndex = 4;
            // 
            // menuButton2
            // 
            this.menuButton2.Icon = ((System.Drawing.Icon)(resources.GetObject("menuButton2.Icon")));
            this.menuButton2.Text = "&New";
            // 
            // menuButton4
            // 
            this.menuButton4.Icon = ((System.Drawing.Icon)(resources.GetObject("menuButton4.Icon")));
            this.menuButton4.Text = "&New";
            // 
            // menuButton6
            // 
            this.menuButton6.Icon = ((System.Drawing.Icon)(resources.GetObject("menuButton6.Icon")));
            this.menuButton6.Text = "&New";
            // 
            // sandBarManager
            // 
            this.sandBarManager.OwnerForm = this;
            // 
            // bottomSandBarDock
            // 
            this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomSandBarDock.Guid = new System.Guid("e1404dca-91bf-493c-8bd5-5431aedc87b6");
            this.bottomSandBarDock.Location = new System.Drawing.Point(0, 643);
            this.bottomSandBarDock.Manager = this.sandBarManager;
            this.bottomSandBarDock.Name = "bottomSandBarDock";
            this.bottomSandBarDock.Size = new System.Drawing.Size(860, 0);
            this.bottomSandBarDock.TabIndex = 7;
            // 
            // leftSandBarDock
            // 
            this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftSandBarDock.Guid = new System.Guid("be9bb126-ec86-4e15-aa71-2f24d3145edf");
            this.leftSandBarDock.Location = new System.Drawing.Point(0, 49);
            this.leftSandBarDock.Manager = this.sandBarManager;
            this.leftSandBarDock.Name = "leftSandBarDock";
            this.leftSandBarDock.Size = new System.Drawing.Size(0, 594);
            this.leftSandBarDock.TabIndex = 5;
            // 
            // rightSandBarDock
            // 
            this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
            this.rightSandBarDock.Guid = new System.Guid("aa2d046a-aeb4-40b4-ae28-68f644b1d3f4");
            this.rightSandBarDock.Location = new System.Drawing.Point(860, 49);
            this.rightSandBarDock.Manager = this.sandBarManager;
            this.rightSandBarDock.Name = "rightSandBarDock";
            this.rightSandBarDock.Size = new System.Drawing.Size(0, 594);
            this.rightSandBarDock.TabIndex = 6;
            // 
            // topSandBarDock
            // 
            this.topSandBarDock.Controls.Add(this.mainMenuBar);
            this.topSandBarDock.Controls.Add(this.mainToolBar);
            this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
            this.topSandBarDock.Guid = new System.Guid("dae86233-28e4-4cee-9d86-44195791f09a");
            this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
            this.topSandBarDock.Manager = this.sandBarManager;
            this.topSandBarDock.Name = "topSandBarDock";
            this.topSandBarDock.Size = new System.Drawing.Size(860, 49);
            this.topSandBarDock.TabIndex = 8;
            // 
            // mainMenuBar
            // 
            this.mainMenuBar.Guid = new System.Guid("de990727-c3dc-49d7-b312-567eea1991ef");
            this.mainMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.fileMenuBar,
            this.viewMenuBar,
            this.qSetMenuBar,
            this.queueMenuBar,
            this.messageMenuBar,
            this.toolsMenuBar,
            this.helpMenuBar,
            this.messageBrowserCtxMenu,
            this.qSetCtxMenu});
            this.mainMenuBar.Location = new System.Drawing.Point(2, 0);
            this.mainMenuBar.Name = "mainMenuBar";
            this.mainMenuBar.OwnerForm = this;
            this.mainMenuBar.Size = new System.Drawing.Size(858, 23);
            this.mainMenuBar.TabIndex = 0;
            // 
            // messageBrowserCtxMenu
            // 
            this.messageBrowserCtxMenu.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.messageBrowserCtxForwardMessageMenuButton,
            this.messageBrowserCtxMoveMessageMenuButton,
            this.messageBrowserCtxDeleteMessageMenuButton,
            this.messageBrowserCtxRefreshMessagesMenuButton,
            this.messageBrowserCtxPurgeQueueMenuButton});
            // 
            // messageBrowserCtxForwardMessageMenuButton
            // 
            this.messageBrowserCtxForwardMessageMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageBrowserCtxForwardMessageMenuButton.Icon")));
            this.messageBrowserCtxForwardMessageMenuButton.Text = "Forward";
            // 
            // messageBrowserCtxMoveMessageMenuButton
            // 
            this.messageBrowserCtxMoveMessageMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageBrowserCtxMoveMessageMenuButton.Icon")));
            this.messageBrowserCtxMoveMessageMenuButton.Text = "Move";
            // 
            // messageBrowserCtxDeleteMessageMenuButton
            // 
            this.messageBrowserCtxDeleteMessageMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageBrowserCtxDeleteMessageMenuButton.Icon")));
            this.messageBrowserCtxDeleteMessageMenuButton.Text = "Delete";
            // 
            // messageBrowserCtxRefreshMessagesMenuButton
            // 
            this.messageBrowserCtxRefreshMessagesMenuButton.BeginGroup = true;
            this.messageBrowserCtxRefreshMessagesMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageBrowserCtxRefreshMessagesMenuButton.Icon")));
            this.messageBrowserCtxRefreshMessagesMenuButton.Text = "Refresh";
            // 
            // messageBrowserCtxPurgeQueueMenuButton
            // 
            this.messageBrowserCtxPurgeQueueMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageBrowserCtxPurgeQueueMenuButton.Icon")));
            this.messageBrowserCtxPurgeQueueMenuButton.Text = "Purge";
            // 
            // qSetCtxMenu
            // 
            this.qSetCtxMenu.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.qSetCtxAddActiveQueueToSet,
            this.qSetCtxNewFolder,
            this.qSetCtxRenameFolder,
            this.qSetCtxDeleteItem,
            this.qSetCtxSendMessage,
            this.qSetCtxNewWebServiceClient,
            this.qSetCtxPurgeQueue,
            this.qSetCtxDeleteQueue});
            // 
            // qSetCtxAddActiveQueueToSet
            // 
            this.qSetCtxAddActiveQueueToSet.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetCtxAddActiveQueueToSet.Icon")));
            this.qSetCtxAddActiveQueueToSet.Image = ((System.Drawing.Image)(resources.GetObject("qSetCtxAddActiveQueueToSet.Image")));
            this.qSetCtxAddActiveQueueToSet.Text = "Add Active Queue To Set";
            // 
            // qSetCtxNewFolder
            // 
            this.qSetCtxNewFolder.BeginGroup = true;
            this.qSetCtxNewFolder.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetCtxNewFolder.Icon")));
            this.qSetCtxNewFolder.ImageIndex = 1;
            this.qSetCtxNewFolder.Text = "New Folder";
            // 
            // qSetCtxRenameFolder
            // 
            this.qSetCtxRenameFolder.Image = ((System.Drawing.Image)(resources.GetObject("qSetCtxRenameFolder.Image")));
            this.qSetCtxRenameFolder.Text = "Rename";
            // 
            // qSetCtxDeleteItem
            // 
            this.qSetCtxDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("qSetCtxDeleteItem.Image")));
            this.qSetCtxDeleteItem.Text = "Delete Item";
            // 
            // qSetCtxSendMessage
            // 
            this.qSetCtxSendMessage.BeginGroup = true;
            this.qSetCtxSendMessage.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetCtxSendMessage.Icon")));
            this.qSetCtxSendMessage.Text = "&New Message";
            // 
            // qSetCtxNewWebServiceClient
            // 
            this.qSetCtxNewWebServiceClient.BeginGroup = true;
            this.qSetCtxNewWebServiceClient.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetCtxNewWebServiceClient.Icon")));
            this.qSetCtxNewWebServiceClient.Text = "New Web Service Client";
            // 
            // qSetCtxPurgeQueue
            // 
            this.qSetCtxPurgeQueue.BeginGroup = true;
            this.qSetCtxPurgeQueue.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetCtxPurgeQueue.Icon")));
            this.qSetCtxPurgeQueue.Text = "Purge";
            // 
            // qSetCtxDeleteQueue
            // 
            this.qSetCtxDeleteQueue.BeginGroup = true;
            this.qSetCtxDeleteQueue.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetCtxDeleteQueue.Icon")));
            this.qSetCtxDeleteQueue.Text = "Delete Queue...";
            // 
            // fileMenuBar
            // 
            this.fileMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.fileNewQSetMenuButton,
            this.fileOpenQSetMenuButton,
            this.fileCloseQSetMenuButton,
            this.fileNewMessageMenuButton,
            this.fileSaveQSetMenuButton,
            this.fileSaveQSetAsMenuButton,
            this.fileRecentFilesMenuButton,
            this.fileExitMenuButton});
            this.fileMenuBar.Text = "&File";
            // 
            // fileNewQSetMenuButton
            // 
            this.fileNewQSetMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("fileNewQSetMenuButton.Image")));
            this.fileNewQSetMenuButton.Text = "&New";
            // 
            // fileOpenQSetMenuButton
            // 
            this.fileOpenQSetMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("fileOpenQSetMenuButton.Image")));
            this.fileOpenQSetMenuButton.Text = "&Open...";
            // 
            // fileCloseQSetMenuButton
            // 
            this.fileCloseQSetMenuButton.Text = "&Close";
            // 
            // fileNewMessageMenuButton
            // 
            this.fileNewMessageMenuButton.BeginGroup = true;
            this.fileNewMessageMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("fileNewMessageMenuButton.Icon")));
            this.fileNewMessageMenuButton.Text = "New Message";
            // 
            // fileSaveQSetMenuButton
            // 
            this.fileSaveQSetMenuButton.BeginGroup = true;
            this.fileSaveQSetMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("fileSaveQSetMenuButton.Image")));
            this.fileSaveQSetMenuButton.Text = "&Save";
            // 
            // fileSaveQSetAsMenuButton
            // 
            this.fileSaveQSetAsMenuButton.Text = "Save &As...";
            // 
            // fileRecentFilesMenuButton
            // 
            this.fileRecentFilesMenuButton.BeginGroup = true;
            this.fileRecentFilesMenuButton.Text = "Recent Files";
            // 
            // fileExitMenuButton
            // 
            this.fileExitMenuButton.BeginGroup = true;
            this.fileExitMenuButton.Text = "E&xit";
            // 
            // viewMenuBar
            // 
            this.viewMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.viewQSetExplorerMenuButton,
            this.viewPropertiesWindowMenuButton,
            this.viewMessageViewerMenuButton,
            this.viewQSetMonitorMenuButton});
            this.viewMenuBar.Text = "&View";
            // 
            // viewQSetExplorerMenuButton
            // 
            this.viewQSetExplorerMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewQSetExplorerMenuButton.Icon")));
            this.viewQSetExplorerMenuButton.Text = "Q Set Explorer";
            // 
            // viewPropertiesWindowMenuButton
            // 
            this.viewPropertiesWindowMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewPropertiesWindowMenuButton.Icon")));
            this.viewPropertiesWindowMenuButton.Text = "Properties Window";
            // 
            // viewMessageViewerMenuButton
            // 
            this.viewMessageViewerMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewMessageViewerMenuButton.Icon")));
            this.viewMessageViewerMenuButton.Text = "Message Viewer";
            // 
            // viewQSetMonitorMenuButton
            // 
            this.viewQSetMonitorMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewQSetMonitorMenuButton.Icon")));
            this.viewQSetMonitorMenuButton.Text = "Q Set Monitor";
            // 
            // qSetMenuBar
            // 
            this.qSetMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.qSetAddActiveQueueMenuButton,
            this.qSetNewFolderMenuButton,
            this.qSetRenameFolderMenuButton,
            this.qSetDeleteItemMenuButton,
            this.qSetPurgeAllQueuesMenuButton});
            this.qSetMenuBar.Text = "Q &Set";
            // 
            // qSetAddActiveQueueMenuButton
            // 
            this.qSetAddActiveQueueMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetAddActiveQueueMenuButton.Icon")));
            this.qSetAddActiveQueueMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("qSetAddActiveQueueMenuButton.Image")));
            this.qSetAddActiveQueueMenuButton.Text = "Add Active Queue To Set";
            this.qSetAddActiveQueueMenuButton.Visible = false;
            // 
            // qSetNewFolderMenuButton
            // 
            this.qSetNewFolderMenuButton.BeginGroup = true;
            this.qSetNewFolderMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetNewFolderMenuButton.Icon")));
            this.qSetNewFolderMenuButton.ImageIndex = 1;
            this.qSetNewFolderMenuButton.Text = "New Folder";
            // 
            // qSetRenameFolderMenuButton
            // 
            this.qSetRenameFolderMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("qSetRenameFolderMenuButton.Image")));
            this.qSetRenameFolderMenuButton.Text = "Rename";
            // 
            // qSetDeleteItemMenuButton
            // 
            this.qSetDeleteItemMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("qSetDeleteItemMenuButton.Image")));
            this.qSetDeleteItemMenuButton.Text = "Delete Item";
            //
            // qSetPurgeAllQueuesMenuButton
            //
            this.qSetPurgeAllQueuesMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queuePurgeMenuButton.Icon")));
            this.qSetPurgeAllQueuesMenuButton.Text = "Purge All Queues";
            // 
            // queueMenuBar
            // 
            this.queueMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.queueOpenMenuButton,
            this.queueBrowseMenuButton,
            this.queueCreateMenuButton,
            this.queueDeleteMenuButton,
            this.queueRefreshMenuButton,
            this.queuePurgeMenuButton});
            this.queueMenuBar.Text = "&Queue";
            // 
            // queueOpenMenuButton
            // 
            this.queueOpenMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("queueOpenMenuButton.Image")));
            this.queueOpenMenuButton.Text = "Open...";
            // 
            // queueBrowseMenuButton
            // 
            this.queueBrowseMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queueBrowseMenuButton.Icon")));
            this.queueBrowseMenuButton.Text = "Search...";
            // 
            // queueCreateMenuButton
            // 
            this.queueCreateMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queueCreateMenuButton.Icon")));
            this.queueCreateMenuButton.Text = "Create...";
            // 
            // queueDeleteMenuButton
            // 
            this.queueDeleteMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queueDeleteMenuButton.Icon")));
            this.queueDeleteMenuButton.Text = "Delete...";
            // 
            // queueRefreshMenuButton
            // 
            this.queueRefreshMenuButton.BeginGroup = true;
            this.queueRefreshMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("queueRefreshMenuButton.Image")));
            this.queueRefreshMenuButton.Text = "Refresh";
            // 
            // queuePurgeMenuButton
            // 
            this.queuePurgeMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queuePurgeMenuButton.Icon")));
            this.queuePurgeMenuButton.Text = "Purge";
            // 
            // messageMenuBar
            // 
            this.messageMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.messageNewMenuButton,
            this.messageForwardMenuButton,
            this.messageMoveMenuButton,
            this.messageDeleteMenuButton});
            this.messageMenuBar.Text = "Message";
            // 
            // messageNewMenuButton
            // 
            this.messageNewMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageNewMenuButton.Icon")));
            this.messageNewMenuButton.Text = "New...";
            // 
            // messageForwardMenuButton
            // 
            this.messageForwardMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageForwardMenuButton.Icon")));
            this.messageForwardMenuButton.Text = "Forward...";
            // 
            // messageMoveMenuButton
            // 
            this.messageMoveMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageMoveMenuButton.Icon")));
            this.messageMoveMenuButton.Text = "Move...";
            // 
            // messageDeleteMenuButton
            // 
            this.messageDeleteMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageDeleteMenuButton.Icon")));
            this.messageDeleteMenuButton.Text = "Delete...";
            // 
            // toolsMenuBar
            // 
            this.toolsMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.toolsNewWebServiceClientMenuButton,
            this.toolsOptionsMenuButton});
            this.toolsMenuBar.Text = "&Tools";
            // 
            // toolsNewWebServiceClientMenuButton
            // 
            this.toolsNewWebServiceClientMenuButton.Icon = ((System.Drawing.Icon)(resources.GetObject("toolsNewWebServiceClientMenuButton.Icon")));
            this.toolsNewWebServiceClientMenuButton.Text = "New Web Service Client";
            // 
            // toolsOptionsMenuButton
            // 
            this.toolsOptionsMenuButton.BeginGroup = true;
            this.toolsOptionsMenuButton.Text = "&Options...";
            // 
            // helpMenuBar
            // 
            this.helpMenuBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.helpAboutMenuButton});
            this.helpMenuBar.Text = "&Help";
            // 
            // helpAboutMenuButton
            // 
            this.helpAboutMenuButton.Text = "&About";
            // 
            // mainToolBar
            // 
            this.mainToolBar.DockLine = 1;
            this.mainToolBar.DrawActionsButton = false;
            this.mainToolBar.Guid = new System.Guid("ad09efa5-d72b-4316-b20c-e175d6249283");
            this.mainToolBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.fileNewQSetToolButton,
            this.fileOpenQSetToolButton,
            this.fileSaveQSetToolButton,
            this.fileNewMessageToolButton,
            this.viewQSetExplorerToolButton,
            this.viewPropertiesWindowToolButton,
            this.viewMessageViewerToolButton,
            this.viewQSetMonitorToolButton,
            this.qSetAddActiveQueueToolButton,
            this.qSetNewFolderToolButton,
            this.qSetRenameFolderToolButton,
            this.qSetDeleteItemToolButton,
            this.queueOpenToolButton,
            this.queueBrowseToolButton,
            this.queueCreateToolButton,
            this.queueDeleteToolButton,
            this.queueRefreshToolButton,
            this.queuePurgeToolButton,
            this.messageNewToolButton,
            this.messageForwardToolButton,
            this.messageMoveToolButton,
            this.messageDeleteToolButton});
            this.mainToolBar.Location = new System.Drawing.Point(2, 23);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(634, 26);
            this.mainToolBar.TabIndex = 1;
            this.mainToolBar.Text = "Standard";
            // 
            // fileNewQSetToolButton
            // 
            this.fileNewQSetToolButton.BuddyMenu = this.fileNewQSetMenuButton;
            this.fileNewQSetToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileNewQSetToolButton.Image")));
            this.fileNewQSetToolButton.ToolTipText = "New";
            // 
            // fileOpenQSetToolButton
            // 
            this.fileOpenQSetToolButton.BuddyMenu = this.fileOpenQSetMenuButton;
            this.fileOpenQSetToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileOpenQSetToolButton.Image")));
            this.fileOpenQSetToolButton.ToolTipText = "Open...";
            // 
            // fileSaveQSetToolButton
            // 
            this.fileSaveQSetToolButton.BuddyMenu = this.fileSaveQSetMenuButton;
            this.fileSaveQSetToolButton.Image = ((System.Drawing.Image)(resources.GetObject("fileSaveQSetToolButton.Image")));
            this.fileSaveQSetToolButton.ToolTipText = "Save";
            // 
            // fileNewMessageToolButton
            // 
            this.fileNewMessageToolButton.BeginGroup = true;
            this.fileNewMessageToolButton.BuddyMenu = this.fileNewMessageMenuButton;
            this.fileNewMessageToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("fileNewMessageToolButton.Icon")));
            this.fileNewMessageToolButton.Text = "New Message";
            this.fileNewMessageToolButton.ToolTipText = "New Message";
            // 
            // viewQSetExplorerToolButton
            // 
            this.viewQSetExplorerToolButton.BeginGroup = true;
            this.viewQSetExplorerToolButton.BuddyMenu = this.viewQSetExplorerMenuButton;
            this.viewQSetExplorerToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewQSetExplorerToolButton.Icon")));
            this.viewQSetExplorerToolButton.ToolTipText = "Q Set Explorer";
            // 
            // viewPropertiesWindowToolButton
            // 
            this.viewPropertiesWindowToolButton.BuddyMenu = this.viewPropertiesWindowMenuButton;
            this.viewPropertiesWindowToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewPropertiesWindowToolButton.Icon")));
            this.viewPropertiesWindowToolButton.ToolTipText = "Properties Window";
            // 
            // viewMessageViewerToolButton
            // 
            this.viewMessageViewerToolButton.BuddyMenu = this.viewMessageViewerMenuButton;
            this.viewMessageViewerToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewMessageViewerToolButton.Icon")));
            this.viewMessageViewerToolButton.ToolTipText = "Message Viewer";
            // 
            // viewQSetMonitorToolButton
            // 
            this.viewQSetMonitorToolButton.BuddyMenu = this.viewQSetMonitorMenuButton;
            this.viewQSetMonitorToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("viewQSetMonitorToolButton.Icon")));
            this.viewQSetMonitorToolButton.ToolTipText = "Q Set Monitor";
            // 
            // qSetAddActiveQueueToolButton
            // 
            this.qSetAddActiveQueueToolButton.BeginGroup = true;
            this.qSetAddActiveQueueToolButton.BuddyMenu = this.qSetAddActiveQueueMenuButton;
            this.qSetAddActiveQueueToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetAddActiveQueueToolButton.Icon")));
            this.qSetAddActiveQueueToolButton.Image = ((System.Drawing.Image)(resources.GetObject("qSetAddActiveQueueToolButton.Image")));
            this.qSetAddActiveQueueToolButton.ToolTipText = "Add Active Queue To Set";
            this.qSetAddActiveQueueToolButton.Visible = false;
            // 
            // qSetNewFolderToolButton
            // 
            this.qSetNewFolderToolButton.BeginGroup = true;
            this.qSetNewFolderToolButton.BuddyMenu = this.qSetNewFolderMenuButton;
            this.qSetNewFolderToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("qSetNewFolderToolButton.Icon")));
            this.qSetNewFolderToolButton.ImageIndex = 1;
            this.qSetNewFolderToolButton.ToolTipText = "New Folder";
            // 
            // qSetRenameFolderToolButton
            // 
            this.qSetRenameFolderToolButton.BuddyMenu = this.qSetRenameFolderMenuButton;
            this.qSetRenameFolderToolButton.Image = ((System.Drawing.Image)(resources.GetObject("qSetRenameFolderToolButton.Image")));
            this.qSetRenameFolderToolButton.ToolTipText = "Rename Folder";
            // 
            // qSetDeleteItemToolButton
            // 
            this.qSetDeleteItemToolButton.BuddyMenu = this.qSetDeleteItemMenuButton;
            this.qSetDeleteItemToolButton.Image = ((System.Drawing.Image)(resources.GetObject("qSetDeleteItemToolButton.Image")));
            this.qSetDeleteItemToolButton.ToolTipText = "Delete Item";
            // 
            // queueOpenToolButton
            // 
            this.queueOpenToolButton.BeginGroup = true;
            this.queueOpenToolButton.BuddyMenu = this.queueOpenMenuButton;
            this.queueOpenToolButton.Image = ((System.Drawing.Image)(resources.GetObject("queueOpenToolButton.Image")));
            this.queueOpenToolButton.ToolTipText = "Open Queue";
            // 
            // queueBrowseToolButton
            // 
            this.queueBrowseToolButton.BuddyMenu = this.queueBrowseMenuButton;
            this.queueBrowseToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queueBrowseToolButton.Icon")));
            this.queueBrowseToolButton.ToolTipText = "Search for Queue";
            // 
            // queueCreateToolButton
            // 
            this.queueCreateToolButton.BuddyMenu = this.queueCreateMenuButton;
            this.queueCreateToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queueCreateToolButton.Icon")));
            this.queueCreateToolButton.ToolTipText = "Create Queue";
            // 
            // queueDeleteToolButton
            // 
            this.queueDeleteToolButton.BuddyMenu = this.queueDeleteMenuButton;
            this.queueDeleteToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queueDeleteToolButton.Icon")));
            this.queueDeleteToolButton.ToolTipText = "Delete Queue";
            // 
            // queueRefreshToolButton
            // 
            this.queueRefreshToolButton.BeginGroup = true;
            this.queueRefreshToolButton.BuddyMenu = this.queueRefreshMenuButton;
            this.queueRefreshToolButton.Image = ((System.Drawing.Image)(resources.GetObject("queueRefreshToolButton.Image")));
            this.queueRefreshToolButton.ToolTipText = "Refresh Queue";
            // 
            // queuePurgeToolButton
            // 
            this.queuePurgeToolButton.BuddyMenu = this.queuePurgeMenuButton;
            this.queuePurgeToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("queuePurgeToolButton.Icon")));
            this.queuePurgeToolButton.ToolTipText = "Purge Queue";
            // 
            // messageNewToolButton
            // 
            this.messageNewToolButton.BeginGroup = true;
            this.messageNewToolButton.BuddyMenu = this.messageNewMenuButton;
            this.messageNewToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageNewToolButton.Icon")));
            this.messageNewToolButton.ToolTipText = "New Message";
            // 
            // messageForwardToolButton
            // 
            this.messageForwardToolButton.BuddyMenu = this.messageForwardMenuButton;
            this.messageForwardToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageForwardToolButton.Icon")));
            this.messageForwardToolButton.ToolTipText = "Forward...";
            // 
            // messageMoveToolButton
            // 
            this.messageMoveToolButton.BuddyMenu = this.messageMoveMenuButton;
            this.messageMoveToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageMoveToolButton.Icon")));
            this.messageMoveToolButton.ToolTipText = "Move...";
            // 
            // messageDeleteToolButton
            // 
            this.messageDeleteToolButton.BuddyMenu = this.messageDeleteMenuButton;
            this.messageDeleteToolButton.Icon = ((System.Drawing.Icon)(resources.GetObject("messageDeleteToolButton.Icon")));
            this.messageDeleteToolButton.ToolTipText = "Delete...";
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 643);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.workingStatusBarPanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(860, 22);
            this.statusBar.TabIndex = 9;
            // 
            // workingStatusBarPanel
            // 
            this.workingStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.workingStatusBarPanel.Name = "workingStatusBarPanel";
            this.workingStatusBarPanel.Width = 843;
            // 
            // buttonItem1
            // 
            this.buttonItem1.BuddyMenu = this.messageForwardMenuButton;
            this.buttonItem1.Icon = ((System.Drawing.Icon)(resources.GetObject("buttonItem1.Icon")));
            this.buttonItem1.ToolTipText = "Forward...";
            // 
            // QSetEnvironmentForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.ClientSize = new System.Drawing.Size(860, 665);
            this.Controls.Add(this.mainDocumentContainer);
            this.Controls.Add(this.leftSandDock);
            this.Controls.Add(this.rightSandDock);
            this.Controls.Add(this.bottomSandDock);
            this.Controls.Add(this.topSandDock);
            this.Controls.Add(this.leftSandBarDock);
            this.Controls.Add(this.rightSandBarDock);
            this.Controls.Add(this.bottomSandBarDock);
            this.Controls.Add(this.topSandBarDock);
            this.Controls.Add(this.statusBar);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QSetEnvironmentForm";
            this.Text = "Q Set";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.QSetEnvironmentForm_Closing);
            this.leftSandDock.ResumeLayout(false);
            this.propertyGridDock.ResumeLayout(false);
            this.bottomSandDock.ResumeLayout(false);
            this.topSandBarDock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.workingStatusBarPanel)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
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

			queueSetExplorer = new Mulholland.QSet.Application.Controls.QSetExplorer();
			queueSetExplorer.Dock = DockStyle.Fill;
			qSetExplorerDock.Controls.Add(queueSetExplorer);
			
			defaultMessageViewer = new Mulholland.QSet.Application.Controls.MessageViewer(license);
			defaultMessageViewer.Dock = DockStyle.Fill;
			messageViewerDock.Controls.Add(defaultMessageViewer);
			
			qSetMonitor = new Mulholland.QSet.Application.Controls.QSetMonitor();
			qSetMonitor .Dock = DockStyle.Fill;
			qSetMonitorDockControl.Controls.Add(qSetMonitor);

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

            mainDocumentContainer.Manager = new TD.SandDock.SandDockManager();

			PrimaryControls primaryControls = new PrimaryControls(
				queueSetExplorer, 
				qSetMonitor,
				propertyGrid,
				defaultMessageViewer, 
				mainDocumentContainer,
				new Images());			
			
			PrimaryForms primaryForms = new PrimaryForms(this, new QueueSearchForm());			
			
			UserSettings userSettings = UserSettings.Create();
			PrimaryObjects primaryObjects = new PrimaryObjects(new ProcessVisualizer(this), userSettings, license);
			primaryObjects.ProcessVisualizer.StatusBarPanel = this.workingStatusBarPanel;				

			_environmentCoordinator = new EnvironmentCoordinator(this, primaryMenus, primaryControls, primaryForms, primaryObjects);
			_environmentCoordinator.SetUp();
		}


		/// <summary>
		/// Populates the singleton MenuItemBag with all of the menu items.
		/// </summary>
		private void SetupMenuItemBag()
		{
			//File menu
			MenuItemBag.FileMenu  = fileMenuBar;		
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
