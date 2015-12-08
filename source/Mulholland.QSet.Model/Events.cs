using System;

namespace Mulholland.QSet.Model
{
	#region delegates

	/// <summary>
	/// Event signature for the ItemDirtied of a Q Set item.
	/// </summary>
	public delegate void ItemDirtiedEvent(object sender, ItemDirtiedEventArgs e);
	

	/// <summary>
	/// ItemRenamedEvent event delegate.
	/// </summary>
	public delegate void ItemRenamedEvent(object sender, ItemRenamedEventArgs e);


	/// <summary>
	/// Delegate for AfterItemAdded event.
	/// </summary>
	public delegate void AfterItemAddedEvent(object sender, AfterItemAddedEventArgs e);		

	
	/// <summary>
	/// Delegate for BeforeItemRemoved event.
	/// </summary>
	public delegate void BeforeItemRemovedEvent(object sender, BeforeItemRemovedEventArgs e);

	
	/// <summary>
	/// Delegate for ItemRepositioned event.
	/// </summary>
	public delegate void ItemRepositionedEvent(object sender, ItemRepositionedEventArgs e);

	#endregion

	#region event argument base classes

	/// <summary>
	/// Event arguments abstract base class for QSet Model events
	/// </summary>
	public abstract class QSetModelEventArgs : EventArgs 	
	{
		/// <summary/>
		protected QSetModelEventArgs() 
			: base() {}
	}


	/// <summary>
	/// Event arguments abstract base class for QSet Model events which have an associated item.
	/// </summary>
	public abstract class AssociateItemEventArgs : QSetModelEventArgs
	{
		private QSetItemBase _item;

		/// <summary>
		/// Constructs object.
		/// </summary>
		/// <param name="item">Item associated with the event.</param>
		protected AssociateItemEventArgs(QSetItemBase item)
			: base()
		{
			if (item == null) throw new ArgumentNullException("item");

			_item = item;
		}


		/// <summary>
		/// Item associated with the event.
		/// </summary>
		public QSetItemBase Item
		{
			get
			{
				return _item;
			}	
		}
	}

	#endregion

	#region event argument concrete classes

	/// <summary>
	/// Event arguments associated with ItemDirtied event.
	/// </summary>
	public class ItemDirtiedEventArgs : AssociateItemEventArgs
	{
		/// <summary>
		/// COnstructs event arguments class.
		/// </summary>
		/// <param name="item">Item which has become dirty.</param>
		public ItemDirtiedEventArgs(QSetItemBase item)
			: base(item) {}
	}


	/// <summary>
	/// Event arguments for ItemRenamedEvent event delegate.
	/// </summary>
	public class ItemRenamedEventArgs : AssociateItemEventArgs
	{		
		private string _previousName;

		/// <summary>
		/// Constructs object.
		/// </summary>
		/// <param name="item">Item which was renamed.</param>
		/// <param name="previousName">The previous name of the item.</param>
		public ItemRenamedEventArgs(QSetItemBase item, string previousName)
			: base(item)
		{			
			_previousName = previousName;
		}


		/// <summary>
		/// The previous name of the item.
		/// </summary>
		public string PreviousName
		{
			get
			{
				return _previousName;
			}
		}
	}


	/// <summary>
	/// Event arguments for AfterItemAdded event.
	/// </summary>
	public class AfterItemAddedEventArgs : AssociateItemEventArgs
	{
			
		private int _insertedAt;

		/// <summary>
		/// Constructs the object with the minum requirements.
		/// </summary>
		/// <param name="item">Item that was added.</param>
		/// <param name="insertedAt">Gets the index at which the item as added.</param>
		public AfterItemAddedEventArgs(QSetItemBase item, int insertedAt)
			: base(item)
		{			
			_insertedAt = insertedAt;
		}


		/// <summary>
		/// Gets the index at which the item as added.
		/// </summary>
		public int InsertedAt
		{
			get
			{
				return _insertedAt;
			}
		}
	}

	/// <summary>
	/// Event arguments for BeforeItemRemoved events.
	/// </summary>
	public class BeforeItemRemovedEventArgs : AssociateItemEventArgs
	{
		private int _removedAt;

		/// <summary>
		/// Constructs the object with the minum requirements.
		/// </summary>
		/// <param name="item">Item that was added or removed.</param>
		/// <param name="removedAt">Gets the index from which the item was removed.</param>
		public BeforeItemRemovedEventArgs(QSetItemBase item, int removedAt)
			: base(item)
		{
			_removedAt = removedAt;
		}


		/// <summary>
		/// Gets the index from which the item was removed.
		/// </summary>
		public int RemovedAt
		{
			get
			{
				return _removedAt;
			}
		}
	}
		

	/// <summary>
	/// Event arguments for ItemRepositioned event.
	/// </summary>
	public class ItemRepositionedEventArgs : AssociateItemEventArgs
	{
		private int _previousPosition;
		private int _newPosition;

		/// <summary>
		/// Constructs the object with the minum requirements.
		/// </summary>
		/// <param name="item">Item that was repositioned.</param>
		/// <param name="previousPosition">The previous index of the item in the collection.</param>
		/// <param name="newPosition">The new index of the item in the collection.</param>
		public ItemRepositionedEventArgs(QSetItemBase item, int previousPosition, int newPosition)
			: base(item)							
		{
			_previousPosition = previousPosition;
			_newPosition = newPosition;
		}


		/// <summary>
		/// The previous index of the item in the collection.
		/// </summary>
		public int PreviousPosition
		{
			get
			{
				return _previousPosition;
			}
		}


		/// <summary>
		/// The new index of the item in the collection.
		/// </summary>
		public int NewPosition
		{
			get
			{
				return _newPosition;
			}
		}
	}

	#endregion
}
