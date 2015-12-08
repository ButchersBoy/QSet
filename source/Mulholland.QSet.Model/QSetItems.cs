using System;
using System.ComponentModel;
using System.IO;
using System.Diagnostics;
using System.Messaging;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Mulholland.Core;
using Mulholland.QSet.Resources;

/* READ ME
 * =======
 *  
 * 1) When new classes, derived from QSetItemBase are added, ensure QSetItemBase.GetFromDataObject is updated.
 * 2) Constructors in derived classes with a (Guid, string) signature are used in the QSetItemBase.Clone, so should be amended with caution, and new objects should consider using these.
 * 
 * 
 */

namespace Mulholland.QSet.Model
{
	#region public abstract class QSetItemBase

	/// <summary>
	/// Abstract base class for QSetItems.
	/// </summary>
	public abstract class QSetItemBase : ICloneable
	{
		private Guid _guid;
		private string _name;
		private Images.IconType _icon;		
		private Images.IconType _openIcon;		
		private QSetFolderItem _parentItem;
		private bool _isDirty;
		private bool _disableParentItemSetCheck = false;

		private event ItemRenamedEvent _itemRenamed;
		private event ItemDirtiedEvent _itemDirtied;

		static QSetItemBase() {}		

		#region ICloneable Members

		/// <summary>
		/// Clones the object.
		/// </summary>
		/// <returns>A cloned copy of the object.</returns>
		public object Clone()
		{
			ConstructorInfo constructorInfo = 
				this.GetType().GetConstructor(new Type[] {typeof(Guid), typeof(string)});
		
			object clone = constructorInfo.Invoke(new object[] {this.ID, this.Name});

			_disableParentItemSetCheck = true;
			((QSetItemBase)clone).DisableParentItemSetCheck = true;
			Utilities.CloneProperties(this, clone);
			((QSetItemBase)clone).DisableParentItemSetCheck = false;

			return clone;
		}

		#endregion

		#region events

		/// <summary>
		/// Raised when the item is renamed.
		/// </summary>
		public event ItemRenamedEvent ItemRenamed
		{
			add
			{
				_itemRenamed += value;
			}
			remove
			{
				_itemRenamed -= value;
			}
		}


		/// <summary>
		/// Raised when the model becomes dirty.
		/// </summary>
		public event ItemDirtiedEvent ItemDirtied
		{
			add
			{
				_itemDirtied += value;
			}
			remove
			{
				_itemDirtied -= value;
			}
		}


		/// <summary>
		/// Raises the ItemDirtiedEvent event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected void OnItemDirtied(ItemDirtiedEventArgs e)
		{
			try
			{
				if (_itemDirtied != null)
					_itemDirtied(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the ItemRenamed event.
		/// </summary>
		/// <param name="e"></param>
		protected void OnItemRenamed(ItemRenamedEventArgs e)
		{
			try
			{
				if (_itemRenamed != null)
					_itemRenamed(this, e);
			}
			catch {}
		}


		#endregion

		/// <summary>
		/// Constructs the object with the minumum requirements.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Name of QSet item.</param>
		/// <param name="icon">Index of icon related to item.</param>		
		/// <remarks>This method is marked as public for cloning purposes.</remarks>
		protected QSetItemBase(Guid guid, string name, Images.IconType icon)
		{
			_guid = guid;
			_name = name;
			_icon = icon;
			_openIcon = icon;
			_isDirty = false;
		}


		/// <summary>
		/// Constructs the object with the minumum requirements, and an additional icon to use when the item is open.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Name of QSet item.</param>
		/// <param name="icon">Index of icon related to item.</param>
		/// <param name="openIcon">Icon related to the items open state.</param>
		protected QSetItemBase(Guid guid, string name, Images.IconType icon, Images.IconType openIcon)
		{
			_guid = guid;
			_name = name;
			_icon = icon;
			_openIcon = openIcon;
		}


		/// <summary>
		/// Constructs the object with the minumum requirements.  A new ID (GUID) will be created for the item.
		/// </summary>
		/// <param name="name">Name of QSet item.</param>
		/// <param name="icon">Index of icon related to item.</param>		
		protected QSetItemBase(string name, Images.IconType icon)
		{
			_guid = Guid.NewGuid();
			_name = name;
			_icon = icon;
			_openIcon = icon;
			_isDirty = false;
		}


		/// <summary>
		/// Constructs the object with the minumum requirements, and an additional icon 
		/// to use when the item is open. A new ID (GUID) will be created for the object.
		/// </summary>
		/// <param name="name">Name of QSet item.</param>
		/// <param name="icon">Index of icon related to item.</param>
		/// <param name="openIcon">Icon related to the items open state.</param>
		protected QSetItemBase(string name, Images.IconType icon, Images.IconType openIcon)
		{
			_guid = Guid.NewGuid();
			_name = name;
			_icon = icon;
			_openIcon = openIcon;
		}


		/// <summary>
		/// Gets or sets the name of the item.
		/// </summary>
		[Description("Name of the item."), Category("Q Set")]
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name != value)
				{
					string previousName = _name;
					_name = value;
					IsDirty = true;
					OnItemRenamed(new ItemRenamedEventArgs(this, previousName));
				}
			}
		}	


		/// <summary>
		/// Index of icon related to item.
		/// </summary>
		[Browsable(false)]
		public Images.IconType Icon
		{
			get
			{
				return _icon;
			}
			set
			{
				_icon = value;
			}
		}


		/// <summary>
		/// Index of icon related to item in its open form..
		/// </summary>
		[Browsable(false)]
		public Images.IconType OpenIcon
		{
			get
			{
				return _openIcon;
			}
			set
			{
				_openIcon = value;
			}
		}


		/// <summary>
		/// Gets the parent of the item if the item has a parent, else null.
		/// </summary>
		/// <remarks>Only a <see cref="QSetItemCollection">QSetItemCollection</see> can modify this property.</remarks>
		[Browsable(false)]			
		public QSetFolderItem ParentItem
		{	
			get
			{
				return _parentItem;
			}
			set
			{			
				StackTrace stackTrace = new StackTrace(1);
				StackFrame stackFrame = stackTrace.GetFrame(0);
				if (stackFrame.GetMethod().DeclaringType != typeof(QSetItemCollection) &&
					DisableParentItemSetCheck == false)
					throw new InvalidOperationException("ParentItem cannot be set.");
				else
					_parentItem = value;
			}
		}


		/// <summary>
		/// Gets or sets the flag which if set to true, stops the ParentItem property validating that it has been set correctly.
		/// </summary>
		internal bool DisableParentItemSetCheck
		{
			get
			{
				return _disableParentItemSetCheck;
			}
			set
			{
				_disableParentItemSetCheck = value;
			}
		}


		/// <summary>
		/// Gets or sets the value which indicates that the object is dirty.
		/// </summary>
		/// <remarks>
		/// If the flag is set to true, the parent item (if available) 
		/// will also be set to true,cascading to the top of the tree.
		/// </remarks>
		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
			set
			{
				bool previousValue = _isDirty;
				_isDirty = value;
				if (value == true && previousValue == false)
					OnItemDirtied(new ItemDirtiedEventArgs(this));				
				if (_isDirty == true && _parentItem != null)
					_parentItem.IsDirty = true;
			}
		}


		/// <summary>
		/// Gets the ID (GUID) of the item.
		/// </summary>
		[Browsable(false)]
		public Guid ID
		{
			get
			{
				return _guid;
			}
		}


		/// <summary>
		/// Attempts to extract a QSetItemBase object from a DataObject.
		/// </summary>
		/// <param name="data">DataObject.</param>
		/// <returns>QSetItemBase object if extract was succesful, else null.</returns>
		public static QSetItemBase GetFromDataObject(IDataObject data)
		{
			QSetItemBase item = null;

			item = (QSetItemBase)data.GetData(typeof(QSetItemBase));
			if (item == null)
				item = (QSetQueueItem)data.GetData(typeof(QSetQueueItem));
			if (item == null)
				item = (QSetFolderItem)data.GetData(typeof(QSetFolderItem));
			if (item == null)
				item = (QSetMachineItem)data.GetData(typeof(QSetMachineItem));
			if (item == null)
				item = (QSetModel)data.GetData(typeof(QSetModel));

			return item;
		}
		
	}

	#endregion

	#region public class QSetFolderItem : QSetItemBase

	/// <summary>
	/// Queue set item which is a folder, or container for other queue set items.
	/// </summary>
	public class QSetFolderItem : QSetItemBase
	{		
		private QSetItemCollection _childItems;

		static QSetFolderItem() {}

		/// <summary>
		/// Constructs the folder item.  A new ID (GUID) will be created for the item.
		/// </summary>
		/// <param name="name">Folder name.</param>		
		public QSetFolderItem(string name) 
			: base(name, Images.IconType.Folder, Images.IconType.FolderOpen) 
		{
			CreateChildItemsCollection();
		}


		/// <summary>
		/// Constructs the folder item.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Folder name.</param>		
		public QSetFolderItem(Guid guid, string name) 
			: base(guid, name, Images.IconType.Folder, Images.IconType.FolderOpen) 
		{
			CreateChildItemsCollection();
		}


		/// <summary>
		/// Allows inherited classes to construct the object, overiding default icons.
		/// </summary>
		/// <param name="name">Name of item.</param>
		/// <param name="icon">Index of icon related to item.</param>
		protected QSetFolderItem(string name, Images.IconType icon) 
			: base(name, icon)
		{
			CreateChildItemsCollection();
		}


		/// <summary>
		/// Allows inherited classes to construct the object, overiding default icons.
		/// </summary>
		/// <param name="name">Name of item.</param>
		/// <param name="icon">Index of icon related to item.</param>
		/// <param name="openIcon">Icon related to the items open state.</param>
		protected QSetFolderItem(string name, Images.IconType icon, Images.IconType openIcon) 
			: base(name, icon, openIcon)
		{
			CreateChildItemsCollection();
		}



		/// <summary>
		/// Allows inherited classes to construct the object, overiding default icons.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Name of item.</param>
		/// <param name="icon">Index of icon related to item.</param>
		protected QSetFolderItem(Guid guid, string name, Images.IconType icon) 
			: base(guid, name, icon)
		{
			CreateChildItemsCollection();
		}


		/// <summary>
		/// Allows inherited classes to construct the object, overiding default icons.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Name of item.</param>
		/// <param name="icon">Index of icon related to item.</param>
		/// <param name="openIcon">Icon related to the items open state.</param>
		protected QSetFolderItem(Guid guid, string name, Images.IconType icon, Images.IconType openIcon) 
			: base(guid, name, icon, openIcon)
		{
			CreateChildItemsCollection();
		}

		
		/// <summary>
		/// Gets a simple description of the item type.
		/// </summary>
		public static string TypeName
		{
			get
			{
				return "folder";
			}			
		}


		/// <summary>
		/// Gets the collection of child items.
		/// </summary>
		[Browsable(false)]
		public QSetItemCollection ChildItems
		{
			get
			{
				return _childItems;
			}
			set
			{
				_childItems = value;
			}
		}		


		/// <summary>
		/// Creates the collection which contains the folder items child items.
		/// </summary>
		private void CreateChildItemsCollection()
		{
			_childItems = new QSetItemCollection();
			_childItems.OwnerItem = this;
			_childItems.AfterItemAdded += new AfterItemAddedEvent(_childItems_AfterItemAdded);
			_childItems.BeforeItemRemoved += new BeforeItemRemovedEvent(_childItems_BeforeItemRemoved);
			_childItems.ItemRepositioned += new ItemRepositionedEvent(_childItems_ItemRepositioned);
		}

		private void _childItems_ItemRepositioned(object sender, ItemRepositionedEventArgs e)
		{
			IsDirty = true;
		}

		private void _childItems_BeforeItemRemoved(object sender, BeforeItemRemovedEventArgs e)
		{
			IsDirty = true;
		}

		private void _childItems_AfterItemAdded(object sender, AfterItemAddedEventArgs e)
		{
			IsDirty = true;
		}
		
	}

	#endregion

	#region public class QSetMachineItem : QSetFolderItem

	/// <summary>
	/// Queue set item which is a machine, and container for queues.
	/// </summary>
	public class QSetMachineItem : QSetFolderItem
	{		
		static QSetMachineItem() {}

		/// <summary>
		/// Constructs the machine item.  A new ID (GUID) will be created for the item.
		/// </summary>
		/// <param name="name">Machine name.</param>		
		public QSetMachineItem(string name) 
			: base(name, Images.IconType.Server, Images.IconType.Server) 
		{}


		/// <summary>
		/// Constructs the machine item.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Machine name.</param>		
		public QSetMachineItem(Guid guid, string name) 
			: base(guid, name, Images.IconType.Server, Images.IconType.Server) 
		{}


		/// <summary>
		/// Gets a simple description of the item type.
		/// </summary>
		public new static string TypeName
		{
			get
			{
				return "machine";
			}			
		}
	}

	#endregion

	#region public class QSetQueueItem : QSetItemBase

	/// <summary>
	/// Queue set item which represents a message queue.
	/// </summary>
	public class QSetQueueItem : QSetItemBase
	{
		private const string _FORMAT_PREFIX_DIRECT = "FormatName:DIRECT=OS:"; 
		
		private QSetMessageQueue _qsetMessageQueue;				
		private string _xslt = null;

		static QSetQueueItem() {}

		/// <summary>
		/// Constructs the queue item.  A new ID (GUID) will be created for the item.
		/// </summary>
		/// <param name="name">Full path/ name of queue.</param>		
		public QSetQueueItem(string name) 
			: base(name, Images.IconType.Queue) 
		{
			_qsetMessageQueue = null;						
		}


		/// <summary>
		/// Constructs the queue item.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">Full path/ name of queue.</param>		
		public QSetQueueItem(Guid guid, string name) 
			: base(guid, name, Images.IconType.Queue) 
		{
			_qsetMessageQueue = null;						
		}


		/// <summary>
		/// Gets a simple description of the item type.
		/// </summary>
		[Browsable(false)]
		public static string TypeName
		{
			get
			{
				return "queue";
			}			
		}

		
		/// <summary>
		/// Gets the message queue associated with the item
		/// </summary>		
		[Browsable(true),
			Description("The message queue associated with this item of the queue set."), 
			Category("Q Set")]
		public QSetMessageQueue QSetMessageQueue
		{
			get
			{
				if (_qsetMessageQueue == null)
				{
					/*
					if (base.Name.Length > _FORMAT_PREFIX_DIRECT.Length && base.Name.Substring(0, _FORMAT_PREFIX_DIRECT.Length).ToUpper() == _FORMAT_PREFIX_DIRECT.ToUpper())
					{
						_qsetMessageQueue = new QSetMessageQueue(this);
						_qsetMessageQueue.Path = base.Name;
					}
					else
						_qsetMessageQueue = new QSetMessageQueue(this, base.Name);
					*/
					
					
					if (base.Name.IndexOf(@"\private$\") != -1)
						_qsetMessageQueue = new QSetMessageQueue(this, "FormatName:Direct=OS:" + base.Name);
					else
						_qsetMessageQueue = new QSetMessageQueue(this, base.Name);
					
				}

				return _qsetMessageQueue;
			}
		}


		/// <summary>
		/// Gets or sets an XSLT file which can be used to transform and view messages as HTML in the message viewer.
		/// </summary>
		[Description("XSLT file which can be used to transform and view messages as HTML in the message viewer."), Category("Q Set")]
		public string MessageViewerXslt
		{
			get
			{
				return _xslt;
			}
			set
			{
				if (_xslt != value)
				{
					_xslt = value;
					base.IsDirty = true;
				}
			}
		}
	}

	#endregion

	#region public class QSetWebServiceItem : QSetItemBase

	/// <summary>
	/// Web service Q Set item.
	/// </summary>
	public class QSetWebServiceItem : QSetItemBase
	{
		private string _uri = null;
		
		static QSetWebServiceItem() {}
		
		/// <summary>
		/// Constructs the item.
		/// </summary>
		/// <param name="name">User friendly name of web service.</param>
		public QSetWebServiceItem(string name) 
			: base(name, Images.IconType.WebService)
		{}


		/// <summary>
		/// Constructs the queue item.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">User friendly name of web service.</param>		
		public QSetWebServiceItem(Guid guid, string name) 
			: base(guid, name, Images.IconType.WebService) 
		{}


		/// <summary>
		/// Gets a simple description of the item type.
		/// </summary>
		[Browsable(false)]
		public static string TypeName
		{
			get
			{
				return "webService";
			}			
		}


		/// <summary>
		/// Gets or sets the URI of the web service.
		/// </summary>
		[Browsable(true),
			Description("The URI of the web service."), 
			Category("Q Set"), ReadOnly(true)]
		public string Uri
		{
			get
			{
				return _uri;
			}
			set
			{
				_uri = value;
			}
		}


		//public
	}

	#endregion

	#region public sealed class QSetModel : QSetFolderItem

	/// <summary>
	/// Main QSet class.
	/// </summary>
	public sealed class QSetModel : QSetFolderItem
	{
		private string _fileName = null;

		static QSetModel() {}

		/// <summary>
		/// Constructs a new QSet item.  A new ID is automatically generated.
		/// </summary>
		/// <param name="name">QSet name.</param>		
		public QSetModel(string name) : base(name, Images.IconType.QSet) {}


		/// <summary>
		/// Constructs the QSet item.
		/// </summary>
		/// <param name="guid">The items ID (GUID).</param>
		/// <param name="name">QSet name.</param>		
		public QSetModel(Guid guid, string name) : base(guid, name, Images.IconType.QSet) {}


		/// <summary>
		/// Gets or sets the filename associated with the Q Set.
		/// </summary>
		/// <remarks>If a file name has not been set, this property may return null.</remarks>
		[ReadOnly(true), Description("File name."), Category("Q Set")]
		public string FileName
		{
			get
			{
				return _fileName;
			}
			set
			{
				_fileName = value;
			}
		}


		/// <summary>
		/// Creates a new Q Set given the content of a .qset file.
		/// </summary>
		/// <param name="qSetFileContent">qset file content, as an XML string.</param>
		/// <returns>New Q Set.</returns>
		public static QSetModel CreateQSet(string qSetFileContent)
		{
			QSetModel newQSet = null;

			//start reading xml
			XPathDocument xpathDoc = new XPathDocument(IOUtilities.StringToMemoryStream(qSetFileContent));
			XPathNavigator xpathNav = xpathDoc.CreateNavigator();
			
			//get root
			XPathNodeIterator ni = xpathNav.Select(QSetXmlFileFormat.RootElement.Name);
			if (ni.MoveNext())
			{
				//create qset with existing guids, or create new ones
				if (ni.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Guid, "") != string.Empty)
					newQSet = new QSetModel
						(
						new Guid(ni.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Guid, "")),
						ni.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Name, "")
						);
				else
					newQSet = new QSetModel(ni.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Name, ""));				
				
				//create all of the child items
				CreateChildItems(newQSet, ni);				
				
				newQSet.IsDirty = false;
			}			

			return newQSet;
		}


		/// <summary>
		/// Serialises the QSet to XML.
		/// </summary>
		/// <returns>XML string representing the Q Set.</returns>
		public string ToXml()
		{
			string xmlResult = null;

			using (MemoryStream stream = new MemoryStream())
			{
				//create text writer
				XmlTextWriter xmlWriter = new XmlTextWriter(stream, System.Text.Encoding.UTF8);

				//write root element start
				xmlWriter.WriteStartElement(QSetModel.TypeName);					
				xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Name, base.Name);
				xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Guid, base.ID.ToString());

				//write all child items
				WriteChildItems(this, xmlWriter);

				//write root element end
				xmlWriter.WriteEndElement();

				//finish off and tidy up
				xmlWriter.Flush();
				stream.Position = 0;
				StreamReader sr = new StreamReader(stream);
				xmlResult = sr.ReadToEnd();
				sr.Close();
			}

			return xmlResult;
		}

		
		/// <summary>
		/// Gets a simple description of the item type.
		/// </summary>
		public new static string TypeName
		{
			get
			{
				return "qset";
			}			
		}


		/// <summary>
		/// Recursively creates all of the child items of a Q Set item.
		/// </summary>
		/// <param name="parentItem">Parent item.</param>
		/// <param name="parentNodeIterator">XPathNodeIterator positioned at the parent item. </param>
		private static void CreateChildItems(QSetFolderItem parentItem, XPathNodeIterator parentNodeIterator)
		{			
			//iterate through all child items
			XPathNodeIterator childNodeIterator = parentNodeIterator.Current.Select("*");
			while (childNodeIterator.MoveNext())
			{
				//get the item details	
				string itemName = childNodeIterator.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Name, "");	
				Guid itemGuid = Guid.Empty;
				if (childNodeIterator.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Guid, "") != string.Empty)
					itemGuid = new Guid(childNodeIterator.Current.GetAttribute(QSetXmlFileFormat.ItemAttributes.Guid, ""));
				
				//create a new item of the required type, specifying the guid if available				
				QSetItemBase newItem = null;
				if (childNodeIterator.Current.Name == QSetFolderItem.TypeName)
				{
					//create folder
					if (itemGuid == Guid.Empty)
						newItem = new QSetFolderItem(itemName);
					else
						newItem = new QSetFolderItem(itemGuid, itemName);						
				}
				else if (childNodeIterator.Current.Name == QSetMachineItem.TypeName)
				{
					//create machine
					if (itemGuid == Guid.Empty)
						newItem = new QSetMachineItem(itemName);
					else
						newItem = new QSetMachineItem(itemGuid, itemName);
				}
				else if (childNodeIterator.Current.Name == QSetQueueItem.TypeName)
				{
					//create queue
					if (itemGuid == Guid.Empty)
						newItem = new QSetQueueItem(itemName);
					else
						newItem = new QSetQueueItem(itemGuid, itemName);
					if (childNodeIterator.Current.GetAttribute(QSetXmlFileFormat.QueueElement.Attributes.MessageViewerXslt, "") != string.Empty)
						((QSetQueueItem)newItem).MessageViewerXslt = childNodeIterator.Current.GetAttribute(QSetXmlFileFormat.QueueElement.Attributes.MessageViewerXslt, "");
				}
				else if (childNodeIterator.Current.Name == QSetWebServiceItem.TypeName)
				{
					//web service item
					if (itemGuid == Guid.Empty)
						newItem = new QSetWebServiceItem(itemName);
					else
						newItem = new QSetWebServiceItem(itemGuid, itemName);
				}
			
				//apply finaly settings and actions to new item
				if (newItem != null)
				{
					//finish type specific setup
					if (newItem is QSetFolderItem)
					{
						//create child items
						CreateChildItems((QSetFolderItem)newItem, childNodeIterator);
					}

					//final setup common to all item types
					parentItem.ChildItems.Add(newItem);
					newItem.IsDirty = false;			
				}		
			}
		}


		/// <summary>
		/// Writes child item XML to the provided writer.
		/// </summary>
		/// <param name="folderItem">Parent item.</param>
		/// <param name="xmlWriter">XmlTextWriter to output to.</param>
		private void WriteChildItems(QSetFolderItem folderItem, XmlTextWriter xmlWriter)
		{
			//write all folders
			foreach(QSetItemBase childItem in folderItem.ChildItems)
			{				
				if (childItem.GetType() == typeof(QSetFolderItem))
				{
					QSetFolderItem childFolderItem = (QSetFolderItem)childItem;
					xmlWriter.WriteStartElement(QSetFolderItem.TypeName);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Name, childFolderItem.Name);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Guid, childFolderItem.ID.ToString());
					WriteChildItems(childFolderItem, xmlWriter);
					xmlWriter.WriteEndElement();
				}
			}

			//write all machines
			foreach(QSetItemBase childItem in folderItem.ChildItems)
			{				
				if (childItem.GetType() == typeof(QSetMachineItem))
				{
					QSetMachineItem childMachineItem = (QSetMachineItem)childItem;
					xmlWriter.WriteStartElement(QSetMachineItem.TypeName);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Name, childMachineItem.Name);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Guid, childMachineItem.ID.ToString());
					WriteChildItems(childMachineItem, xmlWriter);
					xmlWriter.WriteEndElement();
				}
			}

			//write all queues			
			foreach(QSetItemBase childItem in folderItem.ChildItems)
			{
				QSetQueueItem childQueueItem = childItem as QSetQueueItem;
				if (childQueueItem != null)
				{
					xmlWriter.WriteStartElement(QSetQueueItem.TypeName);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Name, childQueueItem.Name);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Guid, childQueueItem.ID.ToString());					
					if (childQueueItem.MessageViewerXslt != null && childQueueItem.MessageViewerXslt.Trim().Length > 0)
						xmlWriter.WriteAttributeString(QSetXmlFileFormat.QueueElement.Attributes.MessageViewerXslt, childQueueItem.MessageViewerXslt.Trim());
					xmlWriter.WriteEndElement();
				}
			}

			//write all web services
			foreach(QSetItemBase childItem in folderItem.ChildItems)
			{
				QSetWebServiceItem childWebServiceItem = childItem as QSetWebServiceItem;
				if (childWebServiceItem != null)
				{
					xmlWriter.WriteStartElement(QSetWebServiceItem.TypeName);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Name, childWebServiceItem.Name);
					xmlWriter.WriteAttributeString(QSetXmlFileFormat.ItemAttributes.Guid, childWebServiceItem.ID.ToString());
					xmlWriter.WriteEndElement();
				}
			}
		}
	}

	#endregion

}
