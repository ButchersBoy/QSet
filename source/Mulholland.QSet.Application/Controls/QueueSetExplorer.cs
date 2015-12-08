using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Mulholland.Applications.QSet.Controls
{
	/// <summary>
	/// Summary description for QSetExplorer.
	/// </summary>
	internal class QSetExplorer : System.Windows.Forms.UserControl
	{
		private QSet _queueSet;		

		private System.Windows.Forms.TreeView _queueSetTreeView = null;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		/// <summary>
		/// Constructs the object.
		/// </summary>
		public QSetExplorer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_queueSet = null;									
			_queueSetTreeView.AfterLabelEdit += new NodeLabelEditEventHandler(_queueSetTreeView_AfterLabelEdit);
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
			this._queueSetTreeView = new System.Windows.Forms.TreeView();
			this.SuspendLayout();
			// 
			// _queueSetTreeView
			// 
			this._queueSetTreeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._queueSetTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._queueSetTreeView.ImageIndex = -1;
			this._queueSetTreeView.Location = new System.Drawing.Point(0, 0);
			this._queueSetTreeView.Name = "_queueSetTreeView";
			this._queueSetTreeView.SelectedImageIndex = -1;
			this._queueSetTreeView.Size = new System.Drawing.Size(312, 284);
			this._queueSetTreeView.TabIndex = 0;
			// 
			// QSetExplorer
			// 
			this.Controls.Add(this._queueSetTreeView);
			this.Name = "QSetExplorer";
			this.Size = new System.Drawing.Size(312, 284);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Gets or sets the QSet displayed in the explorer.
		/// </summary>
		public QSet QSet
		{
			get
			{
				return _queueSet;
			}
			set
			{
				_queueSet = value;
				DisplayQSet(_queueSet);
			}
		}


		/// <summary>
		/// Gets or sets the active QSetItemBase in the QSetExplorer.
		/// </summary>
		public QSetItemBase ActiveItem
		{
			get
			{
				if (_queueSetTreeView.SelectedNode != null)
					return ((QSetItemTreeNode)_queueSetTreeView.SelectedNode).QSetItem;
				else
					return null;
			}
			set
			{
				if (value != null)
				{
					QSetItemTreeNode node = FindNode(value);
					if (node != null)
						_queueSetTreeView.SelectedNode = (TreeNode)node;
					else
						throw new QSetExplorerException("Item does not exist within tree.");
				}
				else
					_queueSetTreeView.SelectedNode = null;
			}
		}


		/// <summary>
		/// Allows the user to edit the currently active item.
		/// </summary>
		/// <returns>true if successful, else false.</returns>
		public void BeginEditActiveItem()
		{			
			if (_queueSetTreeView.SelectedNode != null)
			{
				_queueSetTreeView.LabelEdit = true;
				_queueSetTreeView.SelectedNode.BeginEdit();														
			}			
		}


		/// <summary>
		/// Gets or sets the image list associated with the control.
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return _queueSetTreeView.ImageList;
			}
			set
			{
				_queueSetTreeView.ImageList = value;
			}
		}


		/// <summary>
		/// Searches for a node which contains a particular QSetItemBase object.
		/// </summary>
		/// <param name="item">QSetItemBase to search for.</param>
		/// <returns>The node hosting the item if found, else null.</returns>
		private QSetItemTreeNode FindNode(QSetItemBase item)
		{
			QSetItemTreeNode result = null;
			
			foreach(TreeNode node in _queueSetTreeView.Nodes)
			{
				if (((QSetItemTreeNode)node).QSetItem == item)
				{
					result = (QSetItemTreeNode)node;
					break;
				}
			}

			return result;
		}
			

		/// <summary>
		/// Displays a QSet in the explorer.
		/// </summary>
		/// <param name="queueSet">QSet to display.</param>
		private void DisplayQSet(QSet queueSet)
		{
			_queueSetTreeView.Nodes.Clear();
			if (_queueSet != null)	
			{
				QSetItemTreeNode queueSetNode = new QSetItemTreeNode(_queueSet);
				_queueSetTreeView.Nodes.Add((QSetItemTreeNode)queueSetNode);
				DisplayQSetChildItems(queueSet, queueSetNode);
			}			
		}


		/// <summary>
		/// Recursively populates the tree view with all child items of a folder item.
		/// </summary>
		/// <param name="parentItem">Parent folder item.</param>
		/// <param name="parentNode">Parent node.</param>
		private void DisplayQSetChildItems(QSetFolderItem parentFolderItem, TreeNode parentNode)
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
		private void _queueSetTreeView_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			_queueSetTreeView.LabelEdit = false;

			if (e.Label != e.Node.Text)
				if (e.Node.Parent != null && ((QSetFolderItem)((QSetItemTreeNode)e.Node.Parent).QSetItem).ChildItems.Exists(e.Label))
				{
					e.CancelEdit = true;
					MessageBox.Show("A folder or queue already exists with this name.  Please enter a new name.");
					_queueSetTreeView.LabelEdit = true;
					e.Node.BeginEdit();
				}							
		}
	}


	/// <summary>
	/// Tree node which holds a QSetItemBase object.
	/// </summary>
	internal class QSetItemTreeNode : TreeNode
	{
		private QSetItemBase _queueSetItem;


		/// <summary>
		/// Constructs the object a QSetItemBase object.
		/// </summary>
		/// <param name="queueSetItem">QSetItemBase object on which the tree node is based.</param>
		public QSetItemTreeNode(QSetItemBase queueSetItem) : base() 
		{
			this.ImageIndex = (int)queueSetItem.Icon;
			this.SelectedImageIndex = (int)queueSetItem.OpenIcon;
			this.Text = queueSetItem.Name;

			_queueSetItem = queueSetItem;			
			QSetFolderItem folderItem = _queueSetItem as QSetFolderItem;
			if (folderItem != null)
			{
				folderItem.ChildItems.ItemAdded += new Mulholland.Applications.QSet.QSetItemCollection.ItemAddedEvent(ChildItems_ItemAdded);
				folderItem.ChildItems.ItemRemoved += new Mulholland.Applications.QSet.QSetItemCollection.ItemRemovedEvent(ChildItems_ItemRemoved);
			}
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
		private void ChildItems_ItemAdded(object sender, Mulholland.Applications.QSet.QSetItemCollection.ItemMovedEventArgs e)
		{
			QSetItemTreeNode newNode = new QSetItemTreeNode(e.Item);
			base.Nodes.Add(newNode);
			base.Expand();
			newNode.EnsureVisible();
			newNode.TreeView.SelectedNode = newNode;
		}

		private void ChildItems_ItemRemoved(object sender, Mulholland.Applications.QSet.QSetItemCollection.ItemMovedEventArgs e)
		{

		}
	}


	/// <summary>
	/// Generic exception for QSetExplorer operations.
	/// </summary>
	internal class QSetExplorerException : ApplicationException
	{
		public QSetExplorerException() : base() {}
		
		public QSetExplorerException(string message) : base(message) {}
		
		public QSetExplorerException(string message, Exception innerException) : base(message, innerException) {}
	}
}
