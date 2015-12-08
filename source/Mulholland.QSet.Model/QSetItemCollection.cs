using System;
using System.Collections;
using System.Collections.Specialized;

namespace Mulholland.QSet.Model
{
	/// <summary>
	/// Summary description for QSetItemCollection.
	/// </summary>	
	public class QSetItemCollection : NameObjectCollectionBase 
	{
		private QSetFolderItem _ownerItem = null;

		private event AfterItemAddedEvent _afterItemAdded;
		private event BeforeItemRemovedEvent _beforeItemRemoved;		
		private event ItemRepositionedEvent _itemRepositioned;
		
		#region events

		/// <summary>
		/// Raised when an item is added to the collection.
		/// </summary>
		public event AfterItemAddedEvent AfterItemAdded
		{
			add
			{
				_afterItemAdded += value;
			}
			remove
			{
				_afterItemAdded -= value;
			}
		}


		/// <summary>
		/// Raised when an item is removed from the collection.
		/// </summary>
		public event BeforeItemRemovedEvent BeforeItemRemoved
		{
			add
			{
				_beforeItemRemoved += value;
			}
			remove
			{
				_beforeItemRemoved -= value;
			}
		}


		/// <summary>
		/// Raised when an item within the collection is repositioned.
		/// </summary>
		public event ItemRepositionedEvent ItemRepositioned
		{
			add
			{
				_itemRepositioned += value;
			}
			remove
			{
				_itemRepositioned -= value;
			}
		}


		/// <summary>
		/// Raises the AfterItemAdded event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnAfterItemAdded(AfterItemAddedEventArgs e)
		{
			try
			{
				if (_afterItemAdded != null)
					_afterItemAdded(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the BeforeItemRemoved event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnBeforeItemRemoved(BeforeItemRemovedEventArgs e)
		{
			try
			{
				if (_beforeItemRemoved != null)
					_beforeItemRemoved(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the OnItemRepositioned event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnItemRepositioned(ItemRepositionedEventArgs e)
		{
			try
			{
				if (_itemRepositioned != null)
					_itemRepositioned(this, e);
			}
			catch {}
		}

		#endregion

		#region enumeration support
		
		/// <summary>
		/// Provides foreach enumeration support.
		/// </summary>
		/// <returns></returns>
		public new IEnumerator GetEnumerator()
		{			
			return new Enumerator(this);
		}
		
		
		private class Enumerator : IEnumerator
		{		
			private QSetItemCollection _col;
			private int _index;

			public Enumerator(QSetItemCollection col)
			{
				_col = col;
				_index = -1;
			}

			public void Reset()
			{
				_index = -1;
			}

			public object Current
			{
				get
				{					
					return _col[_index];
				}
			}

			public bool MoveNext()
			{
				_index ++;
				if (_index < _col.Count)
					return true;
				else
					return false;
			}					
		}		

		#endregion

		/// <summary>
		/// Constructs an empty QSetItemCollection.
		/// </summary>
		public QSetItemCollection() {}


		/// <summary>
		/// Constructs an empty QSetItemCollection, specifying an owner item.
		/// </summary>
		/// <param name="ownerItem">The item which owns the collection.</param>
		public QSetItemCollection(QSetFolderItem ownerItem) 
		{
			OwnerItem = ownerItem;
		}


		/// <summary>
		/// Gets ot sets the item which owns the collection.
		/// </summary>
		public QSetFolderItem OwnerItem
		{
			get
			{
				return _ownerItem;
			}
			set
			{
				_ownerItem = value;				
				foreach (QSetItemBase childItem in this)
				{
					((QSetFolderItem)childItem).ParentItem = _ownerItem;
				}
			}
		}


		/// <summary>
		/// Adds a QSetItemBase object to the collection.
		/// </summary>
		/// <param name="qSetItem">QSetItemBase object to add.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if any arguments are set to null.</exception>
		public void Add(QSetItemBase qSetItem)
		{
			if (qSetItem == null) 
				throw new ArgumentNullException("QSetItem");
			else if (qSetItem is QSetModel) 
				throw new ArgumentOutOfRangeException("queueSetItem", "QSet cannot belong to a collection.");
			else if (base.BaseGet(qSetItem.Name) != null)
				throw new ArgumentOutOfRangeException("queueSetItem", "An item with the supplied name already exists in the collection.");
			
			if (OwnerItem != null)
				qSetItem.ParentItem = OwnerItem;

			int insertedAt;
			InsertItemOrdered(qSetItem, out insertedAt);			
			
			qSetItem.ItemRenamed += new ItemRenamedEvent(qSetItem_ItemRenamed);

			OnAfterItemAdded(new AfterItemAddedEventArgs(qSetItem, insertedAt));
		}


		/// <summary>
		/// Removes the QSetItemBase object with the specified key.
		/// </summary>
		/// <param name="key">Key of QSetItemBase object to remove.</param>
		public void Remove(string key)
		{			
			lock(this)
			{				
				QSetItemBase item = (QSetItemBase)base.BaseGet(key);
				if (item != null)
				{
					OnBeforeItemRemoved(new BeforeItemRemovedEventArgs(item, GetIndexOfKey(item.Name)));
					item.ParentItem = null;
					item.ItemRenamed -= new ItemRenamedEvent(qSetItem_ItemRenamed);
					base.BaseRemove(key);					
				}
			}
		}


		/// <summary>
		/// Removes the QSetItemBase object at the specified index.
		/// </summary>
		/// <param name="index">Index to remove at.</param>
		public void RemoveAt(int index)
		{
			lock(this)
			{
				QSetItemBase item = (QSetItemBase)base.BaseGet(index);
				if (item != null)
				{
					OnBeforeItemRemoved(new BeforeItemRemovedEventArgs(item, index));
					item.ParentItem = null;
					item.ItemRenamed -= new ItemRenamedEvent(qSetItem_ItemRenamed);
					base.BaseRemoveAt(index);
				}
			}
		}


		/// <summary>
		/// Checks to see if an item with the given key exists.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Exists (string key)
		{
			return (base.BaseGet(key) != null);
		}

		
		/// <summary>
		/// Gets or sets the QSetItemBase object at/with the specified key.
		/// </summary>
		public QSetItemBase this[string key]
		{
			get
			{
				return (QSetItemBase)base.BaseGet(key);
			}
			set
			{
				base.BaseSet(key, value);
			}
		}


		/// <summary>
		/// Gets or sets the QSetItemBase object at/with the specified index.
		/// </summary>
		public QSetItemBase this[int index]
		{
			get
			{
				return (QSetItemBase)base.BaseGet(index);
			}
			set
			{
				base.BaseSet(index, value);							
			}
		}


		/// <summary>
		/// Alphabetically inserts a new item into the collection.
		/// </summary>
		/// <param name="qSetItem">Item to insert.</param>
		/// <param name="insertedAt">Returns the index of where the item was inserted.</param>
		private void InsertItemOrdered(QSetItemBase qSetItem, out int insertedAt)
		{			
			//ascertain where the item should be inserted
			int insertPos = GetInsertPosition(qSetItem);			

			//insert the item at the desired position
			if (insertPos == base.Count)
				base.BaseAdd(qSetItem.Name, qSetItem);
			else
			{
				//we need to shunt all items at the insert pos and 
				//after up one space, so temporarily put them onto a stack
				Stack shuntItemsStack = new Stack();				
				int currentCount = base.Count;
				for (int counter = insertPos; counter <= currentCount - 1; counter ++)
				{
					shuntItemsStack.Push(base.BaseGet(base.Count - 1));
					base.BaseRemoveAt(base.Count - 1);
				}

				//insert the new item
				base.BaseAdd(qSetItem.Name, qSetItem);

				//put everything from the stack backinto the collection
				while (shuntItemsStack.Count > 0)
				{					
					QSetItemBase replaceItem = (QSetItemBase)shuntItemsStack.Pop();
					base.BaseAdd(replaceItem.Name, replaceItem);
				}
			}

			insertedAt = insertPos;
		}


		/// <summary>
		/// Returns the position where an item should be inserted in the queue.
		/// </summary>
		/// <param name="item">Item which requires inserting.</param>
		/// <returns>Position where the item should be inserted.</returns>
		private int GetInsertPosition(QSetItemBase item)
		{
			int insertPos = 0;
						
			if (base.Count == 1)
			{
				if (((QSetItemBase)base.BaseGet(0)).Name.CompareTo(item.Name) < 0)					
				{
					insertPos = 1;
				}
			}					
			else if (base.Count >= 1)
			{
				for (int checkPos = 0; checkPos <= base.Count - 1; checkPos ++)
				{					
					if (((QSetItemBase)base.BaseGet(checkPos)).Name.CompareTo(item.Name) == 0)
					{
						insertPos = checkPos;
						break;
					}
					else if ((((QSetItemBase)base.BaseGet(checkPos)).Name.CompareTo(item.Name) < 0) 
						&& (checkPos + 1 > base.Count - 1 || (((QSetItemBase)base.BaseGet(checkPos + 1)).Name.CompareTo(item.Name) > 0)))					
					{
						insertPos = checkPos + 1;					
						break;
					}
				}
			}
			
			return insertPos;
		}


		/// <summary>
		/// Returns the index of a key in the collection
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private int GetIndexOfKey(string key)
		{
			int result = -1;

			string[] keys = base.BaseGetAllKeys();
			for (int i = keys.GetLowerBound(0); i <= keys.GetUpperBound(0); i ++)
				if (keys[i] == key)
				{
					result = i;
					break;
				}

			return result;
		}


		/// <summary>
		/// Handles event raised when any Q Set item is renamed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void qSetItem_ItemRenamed(object sender, ItemRenamedEventArgs e)
		{
			int oldPos = GetIndexOfKey(e.PreviousName);
			base.BaseRemove(e.PreviousName);
			int insertedAt;
			InsertItemOrdered(e.Item, out insertedAt);
			OnItemRepositioned(new ItemRepositionedEventArgs(e.Item, oldPos, insertedAt));
		}
	}
}
