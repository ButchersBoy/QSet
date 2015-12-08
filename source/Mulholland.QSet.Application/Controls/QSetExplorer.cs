using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application.Controls
{
	#region internal class QSetExplorer : System.Windows.Forms.UserControl

	/// <summary>
	/// Summary description for QSetExplorer.
	/// </summary>
	internal class QSetExplorer : System.Windows.Forms.UserControl
	{
		private QSetModel _qset;		

		private TreeView _qsetTreeView = null;
		
		private event QSetItemDoubleClickEvent _qSetItemDoubleClick;
		private event QSetActivatedEvent _qSetActivated;
		private event QSetDeactivatedEvent _qSetDeactivated;
		private event MessagesDragDropEvent _messagesDragDrop;
		private event VisualizableProcessItemAffectedEvent _beforeQSetItemActivated;
		private event VisualizableProcessItemAffectedEvent _afterQSetItemActivated;
		private QSetItemRenamedEvent _qSetItemRenamed;

		private VisualizableProcess _selectingItemProcess = null;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;		

		#region events

		/// <summary>
		/// Occurs when an item in the QSetExplorer is double clicked.
		/// </summary>
		public event QSetItemDoubleClickEvent QSetItemDoubleClick
		{
			add
			{
				_qSetItemDoubleClick += value;
			}
			remove
			{
				_qSetItemDoubleClick -= value;
			}
		}


		/// <summary>
		/// Raised when the explorer is configered to show a Q Set.  <seealso cref="QSet."/>
		/// </summary>		
		public event QSetActivatedEvent QSetActivated
		{
			add
			{
				_qSetActivated += value;
			}
			remove
			{
				_qSetActivated -= value;
			}
		}


		/// <summary>
		/// Raised when the current Q Set associated with the explorer is detached.  <seealso cref="QSet."/>
		/// </summary>		
		public event QSetDeactivatedEvent QSetDeactivated
		{
			add
			{
				_qSetDeactivated += value;
			}
			remove
			{
				_qSetDeactivated -= value;
			}
		}

		
		/// <summary>
		/// Raised when the current Q Set is about to be activated.
		/// </summary>		
		public event VisualizableProcessItemAffectedEvent BeforeQSetItemActivated
		{
			add
			{
				_beforeQSetItemActivated += value;
			}
			remove
			{
				_beforeQSetItemActivated -= value;
			}
		}


		/// <summary>
		/// Raised after a Q Set item has been activated
		/// </summary>		
		public event VisualizableProcessItemAffectedEvent AfterQSetItemActivated
		{
			add
			{
				_afterQSetItemActivated += value;
			}
			remove
			{
				_afterQSetItemActivated -= value;
			}
		}


		/// <summary>
		/// Occurs when messages are dragged between queues.
		/// </summary>
		public event MessagesDragDropEvent MessagesDragDrop
		{
			add
			{
				_messagesDragDrop += value;
			}
			remove
			{
				_messagesDragDrop -= value;
			}
		}


		/// <summary>
		/// Occurs when an item is renamed by using the explorer.
		/// </summary>
		public event QSetItemRenamedEvent ItemRenamed
		{
			add
			{
				_qSetItemRenamed += value;
			}
			remove
			{
				_qSetItemRenamed -= value;
			}
		}
		

		/// <summary>
		/// Raises the QSetItemDoubleClick event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnQSetItemDoubleClick(QSetItemDoubleClickEventArgs e)
		{
			try
			{
				if (_qSetItemDoubleClick != null)
					_qSetItemDoubleClick(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the QSetActivated event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnQSetActivatedEvent(QSetActivatedEventArgs e)
		{
			try
			{
				if (_qSetActivated != null)
					_qSetActivated(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the QSetDeactivated event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnQSetDeactivatedEvent(QSetDeactivatedEventArgs e)
		{
			try
			{
				if (_qSetDeactivated != null)
					_qSetDeactivated(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the BeforeQSetItemActivated event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnBeforeQSetItemActivated(VisualizableProcessItemAffectedEventArgs e)
		{
			try
			{
				if (_beforeQSetItemActivated != null)
					_beforeQSetItemActivated(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the AfterQSetItemActivated event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnAfterQSetItemActivatedEvent(VisualizableProcessItemAffectedEventArgs e)
		{
			try
			{
				if (_afterQSetItemActivated != null)
					_afterQSetItemActivated (this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the <see cref="MessagesDragDrop"/> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnMessagesDragDrop(MessagesDragDropEventArgs e)
		{
			try
			{
				if (_messagesDragDrop != null)
					_messagesDragDrop(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the <see cref="QSetItemRenamed"/> event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void OnQSetItemRenamedEvent(QSetControlItemAffectedEventArgs e)
		{
			try
			{
				if (_qSetItemRenamed != null)
					_qSetItemRenamed(this, e);
			}
			catch {}
		}

		#endregion

		/// <summary>
		/// Constructs the object.
		/// </summary>
		public QSetExplorer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_qset = null;												
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._qsetTreeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// _qsetTreeView
			// 
			this._qsetTreeView.AllowDrop = true;
			this._qsetTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._qsetTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._qsetTreeView.ForeColor = System.Drawing.SystemColors.WindowText;
			this._qsetTreeView.HideSelection = false;
			this._qsetTreeView.ImageIndex = -1;
			this._qsetTreeView.Location = new System.Drawing.Point(1, 1);
			this._qsetTreeView.Name = "_qsetTreeView";
			this._qsetTreeView.SelectedImageIndex = -1;
			this._qsetTreeView.Size = new System.Drawing.Size(310, 282);
			this._qsetTreeView.TabIndex = 0;
			this._qsetTreeView.MouseDown += new System.Windows.Forms.MouseEventHandler(this._qsetTreeView_MouseDown);
			this._qsetTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this._qsetTreeView_AfterExpand);
			this._qsetTreeView.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this._qsetTreeView_AfterCollapse);
			this._qsetTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this._qsetTreeView_DragOver);
			this._qsetTreeView.DoubleClick += new System.EventHandler(this._qsetTreeView_DoubleClick);
			this._qsetTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this._qsetTreeView_AfterSelect);
			this._qsetTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this._qsetTreeView_BeforeSelect);
			this._qsetTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this._qsetTreeView_AfterLabelEdit);
			this._qsetTreeView.DragEnter += new System.Windows.Forms.DragEventHandler(this._qsetTreeView_DragOver);
			this._qsetTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this._qsetTreeView_ItemDrag);
			this._qsetTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this._qsetTreeView_DragDrop);
			// 
			// QSetExplorer
			// 
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.Controls.Add(this._qsetTreeView);
			this.Name = "QSetExplorer";
			this.Size = new System.Drawing.Size(312, 284);
			this.ResumeLayout(false);

		}

		#endregion


		/// <summary>
		/// Gets or sets the QSet displayed in the explorer.
		/// </summary>
		public QSetModel QSet
		{
			get
			{
				return _qset;
			}
			set
			{
				QSetModel previousQSet = _qset;
				
				//activate Q Set
				_qset = value;
				DisplayQSet(_qset);

				//raise deactivation event
				if (previousQSet != null)
					OnQSetDeactivatedEvent(new QSetDeactivatedEventArgs(previousQSet));

				//raise activation event
				if (_qset != null)
					OnQSetActivatedEvent(new QSetActivatedEventArgs(_qset));
			}
		}


		/// <summary>
		/// Gets or sets the active QSetItemBase in the QSetExplorer.
		/// </summary>
		public QSetItemBase ActiveItem
		{
			get
			{
				if (_qsetTreeView.SelectedNode != null)
					return ((QSetItemTreeNode)_qsetTreeView.SelectedNode).QSetItem;
				else
					return null;
			}
			set
			{
				if (_qset != null 
					&& value != null 
					&& (_qsetTreeView.SelectedNode == null || ((QSetItemTreeNode)_qsetTreeView.SelectedNode).QSetItem != value))
				{
					if (value == _qset)
						_qsetTreeView.SelectedNode = _qsetTreeView.Nodes[0];
					else
					{
						QSetItemTreeNode node = FindNode(value, (QSetItemTreeNode)_qsetTreeView.Nodes[0]);
						_qsetTreeView.SelectedNode = (TreeNode)node;
					}
				}
				else
					_qsetTreeView.SelectedNode = null;
			}
		}


		/// <summary>
		/// Allows the user to edit the currently active item.
		/// </summary>
		/// <returns>true if successful, else false.</returns>
		public void BeginEditActiveItem()
		{			
			if (_qsetTreeView.SelectedNode != null)
			{
				_qsetTreeView.LabelEdit = true;
				_qsetTreeView.SelectedNode.BeginEdit();														
			}			
		}


		/// <summary>
		/// Gets or sets the image list associated with the control.
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return _qsetTreeView.ImageList;
			}
			set
			{
				_qsetTreeView.ImageList = value;
			}
		}		

		#region private members

		/// <summary>
		/// Searches for a node which contains a particular QSetItemBase object.
		/// </summary>
		/// <param name="item">QSetItemBase to search for.</param>
		/// <param name="searchRootNode">Tree node to start search from.</param>
		/// <returns>The node hosting the item if found, else null.</returns>
		private QSetItemTreeNode FindNode(QSetItemBase item, QSetItemTreeNode searchRootNode)
		{
			QSetItemTreeNode result = null;
			
			foreach(TreeNode node in searchRootNode.Nodes)
			{
				QSetItemTreeNode checkQSetNode = ((QSetItemTreeNode)node);

				if (checkQSetNode.QSetItem == item)
				{
					result = checkQSetNode;
					break;
				}
				else if (checkQSetNode.QSetItem is QSetFolderItem)
				{
					result = FindNode(item, checkQSetNode);
					if (result != null)
						break;
				}
			}

			return result;
		}
			

		/// <summary>
		/// Displays a QSet in the explorer.
		/// </summary>
		/// <param name="queueSet">QSet to display.</param>
		private void DisplayQSet(QSetModel queueSet)
		{
			_qsetTreeView.Nodes.Clear();
			if (_qset != null)	
			{
				QSetItemTreeNode queueSetNode = new QSetItemTreeNode(_qset);
				_qsetTreeView.Nodes.Add((QSetItemTreeNode)queueSetNode);
				DisplayQSetChildItems(queueSet, queueSetNode);
			}			
		}


		/// <summary>
		/// Recursively populates the tree view with all child items of a folder item.
		/// </summary>
		/// <param name="parentItem">Parent folder item.</param>
		/// <param name="parentNode">Parent node.</param>
		public void DisplayQSetChildItems(QSetFolderItem parentFolderItem, TreeNode parentNode)
		{
			foreach (QSetItemBase item in parentFolderItem.ChildItems)
			{
				QSetItemTreeNode itemNode = new QSetItemTreeNode(item);
				parentNode.Nodes.Add((TreeNode)itemNode);

				if (item is QSetFolderItem)
					DisplayQSetChildItems((QSetFolderItem)item, itemNode);
			}
		}


		/// <summary>
		/// Handles the after label edit event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			_qsetTreeView.LabelEdit = false;

			if (e.Label != null &&  e.Label != e.Node.Text)				
				//validate the new name
				if (e.Node.Parent != null && ((QSetFolderItem)((QSetItemTreeNode)e.Node.Parent).QSetItem).ChildItems.Exists(e.Label))
				{
					//failed validation
					e.CancelEdit = true;
					MessageBox.Show("A folder or queue already exists with this name.  Please enter a new name.");
					_qsetTreeView.LabelEdit = true;
					e.Node.BeginEdit();
				}			
				else
				{
					((QSetItemTreeNode)e.Node).QSetItem.Name = e.Label;					
				}				
		}


		/// <summary>
		/// Handles a double click of the tree view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_DoubleClick(object sender, EventArgs e)
		{
			if (_qsetTreeView.SelectedNode != null)
				OnQSetItemDoubleClick(new QSetItemDoubleClickEventArgs(((QSetItemTreeNode)_qsetTreeView.SelectedNode).QSetItem));
		}


		/// <summary>
		/// Handles event fired just before node is been selected within the tree view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
		{
			if (((QSetItemTreeNode)e.Node).QSetItem is QSetQueueItem)
				_selectingItemProcess = new VisualizableProcess(Locale.UserMessages.RetrievingQueueProperties);
			else
				_selectingItemProcess = new VisualizableProcess();
			OnBeforeQSetItemActivated(new VisualizableProcessItemAffectedEventArgs(_selectingItemProcess, ((QSetItemTreeNode)e.Node).QSetItem));	
		}

		
		/// <summary>
		/// Handles event fired when a node has been selected within the tree view.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_AfterSelect(object sender, TreeViewEventArgs e)
		{			 
			OnAfterQSetItemActivatedEvent(new VisualizableProcessItemAffectedEventArgs(_selectingItemProcess, ((QSetItemTreeNode)e.Node).QSetItem));
			_selectingItemProcess = null;
		}
		

		/// <summary>
		/// Handles tree view mouse down.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_MouseDown(object sender, MouseEventArgs e)
		{
			//make sure a node is selected even when right clicked
			TreeNode selectNode = _qsetTreeView.GetNodeAt(e.X, e.Y);
			if (selectNode != null)			
				_qsetTreeView.SelectedNode = selectNode;								
		}		
		

		/// <summary>
		/// Handles a tree node expand.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_AfterExpand(object sender, TreeViewEventArgs e)
		{
			e.Node.ImageIndex = (int)((QSetItemTreeNode)e.Node).QSetItem.OpenIcon;
			e.Node.SelectedImageIndex = (int)((QSetItemTreeNode)e.Node).QSetItem.OpenIcon;
		}


		/// <summary>
		/// Handles a tree node collapse.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			e.Node.ImageIndex = (int)((QSetItemTreeNode)e.Node).QSetItem.Icon;
			e.Node.SelectedImageIndex = (int)((QSetItemTreeNode)e.Node).QSetItem.Icon;
		}


		/// <summary>
		/// Handles the drag enter/over to provide visual feedback to the user.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//get the item being dragged over
			QSetItemBase dragOverItem = 
				_qsetTreeView.GetNodeAt(_qsetTreeView.PointToClient(new Point(e.X, e.Y))) == null ? null : ((QSetItemTreeNode)_qsetTreeView.GetNodeAt(_qsetTreeView.PointToClient(new Point(e.X, e.Y)))).QSetItem;

			e.Effect = DragDropEffects.None;

			//is this a Q Set item being dragged
			QSetItemBase qSetDragItem = QSetItemBase.GetFromDataObject(e.Data);
			if (qSetDragItem != null)
			{
				//handle a Q Set item drag							
				if (qSetDragItem != null && dragOverItem != null && dragOverItem is QSetFolderItem && 
					!(IsAncestorOf(qSetDragItem, dragOverItem)) &&
					qSetDragItem.ParentItem != dragOverItem &&
					!(dragOverItem is QSetMachineItem) &&
					qSetDragItem != dragOverItem)

					e.Effect = DragDropEffects.Move;
			}
			else
			{
				//is this a message(s) drag?
				MessageDragContainer messageDragContainer = (MessageDragContainer)e.Data.GetData(typeof(MessageDragContainer));
				if (messageDragContainer != null)
				{
					if (dragOverItem is QSetQueueItem)
					{
						e.Effect = DragDropEffects.Move;
					}
				}
			}
		}


		/// <summary>
		/// Checks to see if an item is an ancestor of another.
		/// </summary>
		/// <param name="ancestor">Item to be checked whether is an ancestor.</param>
		/// <param name="ancestorOf">Item to be checked against.</param>
		/// <returns>True if the first item is an ancestor of the second, else false.</returns>
		private bool IsAncestorOf(QSetItemBase ancestor, QSetItemBase ancestorOf)
		{
			bool result = false;

			QSetItemBase check = ancestorOf;

			while (check.ParentItem != null)
			{
				if (check.ParentItem == ancestor)
				{
					result = true;
					break;
				}

				check = check.ParentItem;
			}

			return result;
		}

		
		/// <summary>
		/// Handles the drag drop.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			//get the item which was dragged on to
			QSetItemBase dragOverItem = 
				_qsetTreeView.GetNodeAt(_qsetTreeView.PointToClient(new Point(e.X, e.Y))) == null ? null : ((QSetItemTreeNode)_qsetTreeView.GetNodeAt(_qsetTreeView.PointToClient(new Point(e.X, e.Y)))).QSetItem;

			//was a Q Set item dragged?
			QSetItemBase droppedItem = QSetItemBase.GetFromDataObject(e.Data);			
			if (droppedItem != null)
			{
				//handle Q Set item drag
				QSetFolderItem dragOverFolder = dragOverItem as QSetFolderItem;
				if (dragOverFolder != null && droppedItem.ParentItem != dragOverFolder)
				{
					if (!dragOverFolder.ChildItems.Exists(droppedItem.Name))
					{					
						((QSetFolderItem)droppedItem.ParentItem).ChildItems.Remove(droppedItem.Name);					
						dragOverFolder.ChildItems.Add(droppedItem);
					}
					else
						MessageBox.Show(Locale.UserMessages.CannotAddItemAsAlreadyExists, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else
			{
				//is this a message(s) drag?
				MessageDragContainer messageDragContainer = (MessageDragContainer)e.Data.GetData(typeof(MessageDragContainer));
				if (messageDragContainer != null)
				{
					OnMessagesDragDrop(new MessagesDragDropEventArgs(messageDragContainer.OwnerQueueItem, (QSetQueueItem)dragOverItem, messageDragContainer.Messages));
				}
			}
		}


		/// <summary>
		/// Starts drag and drop.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _qsetTreeView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{			
			QSetItemBase item = ((QSetItemTreeNode)e.Item).QSetItem;
			if (!(item.ParentItem is QSetMachineItem) && !(item is QSetModel))
				_qsetTreeView.DoDragDrop(((QSetItemTreeNode)e.Item).QSetItem, DragDropEffects.Move);						
		}

		#endregion
	}

	#endregion

	#region internal class QSetItemTreeNode : TreeNode

	/// <summary>
	/// Tree node which holds a QSetItemBase object.
	/// </summary>
	internal class QSetItemTreeNode : TreeNode, IDisposable
	{
		private QSetItemBase _queueSetItem;
		private AfterItemAddedEvent _afterItemAddedEventDelegate = null;
		private BeforeItemRemovedEvent _beforeItemRemovedEventDelegate = null;
		private ItemRepositionedEvent _itemRepositionedEventDelegate = null;
		private bool _disposed = false;

		/// <summary>
		/// Constructs the object a QSetItemBase object.
		/// </summary>
		/// <param name="queueSetItem">QSetItemBase object on which the tree node is based.</param>
		public QSetItemTreeNode(QSetItemBase queueSetItem) : base() 
		{			
			this.ImageIndex = (int)queueSetItem.Icon;
			this.SelectedImageIndex = (int)queueSetItem.Icon;
			this.Text = queueSetItem.Name;						
			
			_queueSetItem = queueSetItem;			
			QSetFolderItem folderItem = _queueSetItem as QSetFolderItem;
			if (folderItem != null)
			{	
				_afterItemAddedEventDelegate = new AfterItemAddedEvent(ChildItems_AfterItemAdded);
				_beforeItemRemovedEventDelegate = new BeforeItemRemovedEvent(ChildItems_BeforeItemRemoved);
				_itemRepositionedEventDelegate = new ItemRepositionedEvent(ChildItems_ItemRepositioned);

				folderItem.ChildItems.AfterItemAdded += _afterItemAddedEventDelegate;
				folderItem.ChildItems.BeforeItemRemoved += _beforeItemRemovedEventDelegate;
				folderItem.ChildItems.ItemRepositioned += _itemRepositionedEventDelegate;
			}			

			_queueSetItem.ItemRenamed += new ItemRenamedEvent(_queueSetItem_ItemRenamed);
		}		


		/// <summary>
		/// Gets or sets the queue set item which corresponds to the node.
		/// </summary>
		public QSetItemBase QSetItem
		{
			get
			{				
				return _queueSetItem;
			}
			set
			{
				_queueSetItem = value;
				base.Text = _queueSetItem.Name;
			}
		}


		/// <summary>
		/// Handles event fired when a child is added to the current item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChildItems_AfterItemAdded(object sender, AfterItemAddedEventArgs e)
		{
			QSetItemTreeNode newNode = new QSetItemTreeNode(e.Item);
			QSetItemTreeNode newNodeRef = newNode;
			
			if (e.Item is QSetFolderItem)
				CreateChildNodes((QSetFolderItem)e.Item, newNodeRef);

			base.Nodes.Insert(e.InsertedAt, newNode);
						
			base.Expand();			
			base.TreeView.SelectedNode = newNode;			
			newNode.EnsureVisible();			
		}


		/// <summary>
		/// Recursively adds child nodes to a node.
		/// </summary>
		/// <param name="parentItem">Parent folder item.</param>
		/// <param name="parentNode">Parent node.</param>
		public void CreateChildNodes(QSetFolderItem parentFolderItem, TreeNode parentNode)
		{
			foreach (QSetItemBase item in parentFolderItem.ChildItems)
			{
				QSetItemTreeNode itemNode = new QSetItemTreeNode(item);
				parentNode.Nodes.Add((TreeNode)itemNode);

				if (item is QSetFolderItem)
					CreateChildNodes((QSetFolderItem)item, itemNode);
			}
		}


		/// <summary>
		/// Handles event fired when a child item is removed from the current item.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChildItems_BeforeItemRemoved(object sender, BeforeItemRemovedEventArgs e)
		{
			QSetItemTreeNode removedNode = (QSetItemTreeNode)base.Nodes[e.RemovedAt];			
			base.Nodes.RemoveAt(e.RemovedAt);
			removedNode.Dispose();
		}


		/// <summary>
		/// Handles event fired when a node is repostioned with ints parent collection.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChildItems_ItemRepositioned(object sender, ItemRepositionedEventArgs e)
		{
			TreeNode moveNode = base.Nodes[e.PreviousPosition];
			base.Nodes.RemoveAt(e.PreviousPosition);
			base.Nodes.Insert(e.NewPosition, moveNode);
			moveNode.TreeView.SelectedNode = moveNode;
		}

		
		/// <summary>
		/// Handles event fired when a node is renamed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _queueSetItem_ItemRenamed(object sender, ItemRenamedEventArgs e)
		{
			base.Text = e.Item.Name;
		}

		#region IDisposable Members

		/// <summary>
		/// Disposes the object.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue 
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}


		/// <summary>
		/// Handles the disposing of the node, unnattaching any event handlers.
		/// </summary>
		/// <param name="disposing">Set to true if being disposed from IDispose interface.</param>
		private void Dispose(bool disposing)
		{
			//detach event handlers if required
			if(!_disposed)
			{
				QSetFolderItem folderItem = _queueSetItem as QSetFolderItem;
				if (folderItem != null)
				{	
					if (_afterItemAddedEventDelegate != null)
						folderItem.ChildItems.AfterItemAdded -= _afterItemAddedEventDelegate;
					if (_beforeItemRemovedEventDelegate != null)
						folderItem.ChildItems.BeforeItemRemoved -= _beforeItemRemovedEventDelegate;
					if (_itemRepositionedEventDelegate != null)
						folderItem.ChildItems.ItemRepositioned -= _itemRepositionedEventDelegate;
				}
			}
			_disposed = true;         
		}


		#endregion
	}

	#endregion

	#region internal class QSetExplorerException : ApplicationException

	/// <summary>
	/// Generic exception for QSetExplorer operations.
	/// </summary>
	internal class QSetExplorerException : ApplicationException
	{
		public QSetExplorerException() : base() {}
		
		public QSetExplorerException(string message) : base(message) {}
		
		public QSetExplorerException(string message, Exception innerException) : base(message, innerException) {}
	}

	#endregion

}
