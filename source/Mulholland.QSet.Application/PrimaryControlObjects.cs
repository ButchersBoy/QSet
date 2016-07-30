using System;
using System.Collections.Specialized;
using System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Application.DockForms;
using Mulholland.QSet.Application.WebServices;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;
using TD.SandBar;
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
		private QSetExplorer _qSetExplorer;
		private QSetMonitor _qSetMonitor;
		private PropertyGrid _propertyGrid;
		private MessageViewer _messageViewer;
        private DockPanel _dockPanel;
        private MessageBrowserCollection _messageBrowserCollection;
		private WebServiceClientControlCollection _webServiceClientControlCollection;
		private Images _images;

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
            QSetExplorer qSetExplorer,
			QSetMonitor qSetMonitor,
			PropertyGrid propertyGrid,
			MessageViewer messageViewer,
            DockPanel dockPanel,
			Images images)
		{
			_qSetExplorer = qSetExplorer;
			_qSetMonitor = qSetMonitor;
			_propertyGrid = propertyGrid;
			_messageViewer = messageViewer;
            _dockPanel = dockPanel;
			_images = images;

			_messageBrowserCollection = new MessageBrowserCollection();
			_webServiceClientControlCollection = new WebServiceClientControlCollection();
		}


		/// <summary>
		/// Gets the environment's primary QSetExplorer.
		/// </summary>
		public QSetExplorer QSetExplorer
		{
			get
			{
				return _qSetExplorer;
			}
		}


		/// <summary>
		/// Gets the environment's primary QSetMonitor.
		/// </summary>
		public QSetMonitor QSetMonitor
		{
			get
			{
				return _qSetMonitor;
			}
		}


		/// <summary>
		/// Gets the environment's property grid.
		/// </summary>
		public PropertyGrid PropertyGrid
		{
			get
			{
				return _propertyGrid;
			}
		}


		/// <summary>
		/// Gets the environment's primary MessageViewer.
		/// </summary>
		public MessageViewer MessageViewer
		{
			get
			{
				return _messageViewer;
			}
		}


		public DockPanel DockPanel
		{
			get
			{
				return _dockPanel;
			}
		}


		/// <summary>
		/// Gets the main message browser collection.
		/// </summary>
		public MessageBrowserCollection MessageBrowserCollection
		{
			get
			{
				return _messageBrowserCollection;
			}
		}


		/// <summary>
		/// Gets the main web service client control collection.
		/// </summary>
		public WebServiceClientControlCollection WebServiceClientControlCollection
		{
			get
			{
				return _webServiceClientControlCollection;
			}
		}


		/// <summary>
		/// Gets the main application Images component.
		/// </summary>
		public Images Images
		{			
			get 
			{
				return _images;
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
