using System;
using System.Collections;
using System.Collections.Specialized;

namespace Mulholland.QSet.Application.Controls
{
	/// <summary>
	/// MessageBrowser Collection.
	/// </summary>
	internal class MessageBrowserCollection : NameObjectCollectionBase
	{
		private event MessageBrowserCollection.ItemAddedEvent _itemAdded;
		private event MessageBrowserCollection.ItemRemovedEvent _itemRemoved;

		/// <summary>
		/// Constructs an empty MessageBroswerCollection.
		/// </summary>
		public MessageBrowserCollection() {}

		#region events

		/// <summary>
		/// Delegate for ItemAdded event.
		/// </summary>
		public delegate void ItemAddedEvent(object sender, ItemMovedEventArgs e);		


		/// <summary>
		/// Delegate for ItemRemoved event.
		/// </summary>
		public delegate void ItemRemovedEvent(object sender, ItemMovedEventArgs e);


		/// <summary>
		/// Event arguments for ItemAdded or ItemRemoved events.
		/// </summary>
		public class ItemMovedEventArgs : EventArgs
		{
			private MessageBrowser _item;

			/// <summary>
			/// Constructs the object with the minum requirements.
			/// </summary>
			/// <param name="item">Item that was added or removed.</param>
			public ItemMovedEventArgs(MessageBrowser item)
			{
				_item = item;
			}


			/// <summary>
			/// Item that was added or removed.
			/// </summary>
			public MessageBrowser Item
			{
				get
				{
					return _item;
				}
			}
		}


		/// <summary>
		/// Raised when an item is added to the collection.
		/// </summary>
		public event MessageBrowserCollection.ItemAddedEvent ItemAdded
		{
			add
			{
				_itemAdded += value;
			}
			remove
			{
				_itemAdded -= value;
			}
		}


		/// <summary>
		/// Raised when an item is removed from the collection.
		/// </summary>
		public event MessageBrowserCollection.ItemRemovedEvent ItemRemoved
		{
			add
			{
				_itemRemoved += value;
			}
			remove
			{
				_itemRemoved -= value;
			}
		}


		/// <summary>
		/// Raises the ItemAdded event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnItemAdded(ItemMovedEventArgs e)
		{
			try
			{
				_itemAdded(this, e);
			}
			catch {}
		}


		/// <summary>
		/// Raises the ItemRemoved event.
		/// </summary>
		/// <param name="e">Event arguments</param>
		private void OnItemRemoved(ItemMovedEventArgs e)
		{
			try
			{
				_itemRemoved(this, e);
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
			private MessageBrowserCollection _col;
			private int _index;

			public Enumerator(MessageBrowserCollection col)
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
		/// Constructs a MessageBrowserCollection, populating with the provided collection.
		/// </summary>
		/// <param name="messageBrowserCollection">Collection to build constructed collection from.</param>
		public MessageBrowserCollection(MessageBrowserCollection messageBrowserCollection)
		{
			for(int i = 0; i < messageBrowserCollection.Count - 1; i ++)
			{
				string key = messageBrowserCollection.BaseGetKey(i);
				base.BaseAdd(key, messageBrowserCollection[i]);
			}
		}


		/// <summary>
		/// Adds a MessageBrowser to the collection.
		/// </summary>
		/// <param name="messageBrowser">MessageBrowser to add.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if any arguments are set to null.</exception>
		public void Add(string key, MessageBrowser messageBrowser)
		{
			if (key == null)
				throw new ArgumentNullException("key");
			if (messageBrowser == null)
				throw new ArgumentNullException("messageBrowser");
			
			if (this.Exists(key))
				throw new ArgumentOutOfRangeException("key", "Key already exists in collection.");
					
			base.BaseAdd(key, messageBrowser);

			OnItemAdded(new MessageBrowserCollection.ItemMovedEventArgs(messageBrowser));
		}


		/// <summary>
		/// Removes the MessageBrowser with the specified key.
		/// </summary>
		/// <param name="key">Key of MessageBrowser to remove.</param>
		public void Remove(string key)
		{
			MessageBrowser messageBrowser = null;

			lock (this)
			{
				messageBrowser = (MessageBrowser)base.BaseGet(key);
				base.BaseRemove(key);
			}

			if (messageBrowser != null)
				OnItemRemoved(new MessageBrowserCollection.ItemMovedEventArgs(messageBrowser));
		}


		/// <summary>
		/// Removes the MessageBrowser at the specified index.
		/// </summary>
		/// <param name="index">Index to remove at.</param>
		public void RemoveAt(int index)
		{
			MessageBrowser messageBrowser = null;

			lock (this)
			{
				messageBrowser = (MessageBrowser)base.BaseGet(index);
				base.BaseRemoveAt(index);
			}

			if (messageBrowser != null)
				OnItemRemoved(new MessageBrowserCollection.ItemMovedEventArgs(messageBrowser));
		}

		
		/// <summary>
		/// Gets or sets the Message at/with the specified key.
		/// </summary>
		public MessageBrowser this[string key]
		{
			get
			{
				return (MessageBrowser)base.BaseGet(key);
			}
			set
			{
				base.BaseSet(key, value);
			}
		}


		/// <summary>
		/// Gets or sets the Message at/with the specified index.
		/// </summary>
		public MessageBrowser this[int index]
		{
			get
			{
				return (MessageBrowser)base.BaseGet(index);
			}
			set
			{
				base.BaseSet(index, value);				
			}
		}	


		/// <summary>
		/// Checks to see if a particular key exists witin the collection.
		/// </summary>
		/// <param name="key">Key to search for.</param>
		/// <returns>true if the key exists, else false.</returns>
		public bool Exists(string key)
		{
			bool result = false;

			if (base.BaseGet(key) != null)
				result = true;

			return result;
		}

	}

}
