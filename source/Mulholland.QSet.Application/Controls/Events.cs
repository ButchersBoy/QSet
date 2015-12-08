using System;
using System.Messaging;
using Mulholland.QSet.Model;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application.Controls
{
	
	#region delegates

	/// <summary>
	/// Event signature for the double click of a Q Set item.
	/// </summary>
	internal delegate void QSetItemDoubleClickEvent(object sender, QSetItemDoubleClickEventArgs e);


	/// <summary>
	/// Event signature for the Q Set activated event of the Q Set explorer.
	/// </summary>
	internal delegate void QSetActivatedEvent(object sender, QSetActivatedEventArgs e);


	/// <summary>
	/// Event signature for the Q Set deactivated event of the Q Set explorer.
	/// </summary>
	internal delegate void QSetDeactivatedEvent(object sender, QSetDeactivatedEventArgs e);


	/// <summary>
	/// Event signature for the Q Set item activated event of the Q Set explorer.
	/// </summary>
	internal delegate void AfterQSetItemActivatedEvent(object sender, VisualizableProcessItemAffectedEventArgs e);


	/// <summary>
	/// Event signature for event raised when messages are dragged between <see cref="QSetQueueItem"/> objects.
	/// </summary>
	internal delegate void MessagesDragDropEvent(object sender, MessagesDragDropEventArgs e);


	/// <summary>
	/// Event signature for QSetItemBase related events which signify the start and end of a job for which the lifetime 
	/// of the job can be displayed to the user, using methods such as changing the mouse cursor.
	/// </summary>
	internal delegate void VisualizableProcessItemAffectedEvent(object sender, VisualizableProcessItemAffectedEventArgs e);

	
	/// <summary>
	/// Event signature for event which occurs when an item has been renamed.
	/// </summary>
	internal delegate void QSetItemRenamedEvent(object sender, QSetControlItemAffectedEventArgs e);

	#endregion

	#region event argument classes

	/// <summary>
	/// Base class for Q Set control events.
	/// </summary>
	internal abstract class QSetControlEventArgs : EventArgs
	{
		protected QSetControlEventArgs() 
			: base () {}
	}


	/// <summary>
	/// Event args for VisualizableProcessItemAffectedEvent event delegate.
	/// </summary>
	internal class VisualizableProcessItemAffectedEventArgs : VisualizableProcessEventArgs
	{
		private QSetItemBase _item;

		/// <summary>
		/// Constructs the event arguments class.
		/// </summary>
		/// <param name="process">Process which can be visualized.</param>
		/// <param name="item">Item associated with the event.</param>
		public VisualizableProcessItemAffectedEventArgs(VisualizableProcess process, QSetItemBase item)
			: base(process)
		{
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


	/// <summary>
	/// Base class for event arguments for control events where a single Q Set item has been affected.
	/// </summary>
	internal abstract class QSetControlItemAffectedEventArgs : QSetControlEventArgs
	{
		private QSetItemBase _item;

		/// <summary>
		/// Constructs object.
		/// </summary>
		/// <param name="item">Item associated with the event.</param>
		protected QSetControlItemAffectedEventArgs(QSetItemBase item)
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


	/// <summary>
	/// Event arguments for QSetItemDoubleClickEvent event delegate.
	/// </summary>
	internal class QSetItemDoubleClickEventArgs : QSetControlItemAffectedEventArgs
	{		
		/// <summary>
		/// Constructs object.
		/// </summary>
		/// <param name="item">Item which was double clicked.</param>
		public QSetItemDoubleClickEventArgs(QSetItemBase item) 
			: base (item) {}
	}


	/// <summary>
	/// Event arguments for QSetActivated event delgate.
	/// </summary>
	internal class QSetActivatedEventArgs : QSetControlItemAffectedEventArgs
	{
		/// <summary>
		/// Constructs object.
		/// </summary>
		/// <param name="item">Q Set which was activated.</param>
		public QSetActivatedEventArgs(QSetItemBase item) 
			: base (item) {}
	}


	/// <summary>
	/// Event arguments for MessagesDragDropEvent event delegate.
	/// </summary>
	internal class MessagesDragDropEventArgs : EventArgs
	{
		private QSetQueueItem _fromQueueItem;
		private QSetQueueItem _toQueueItem;
		private Message[] _messages;

		/// <summary>
		/// Constructs the event arguments.
		/// </summary>
		/// <param name="fromQueueItem">Queue where messages were dragged from.</param>
		/// <param name="toQueueItem">Queue where messages were dragged to.</param>
		/// <param name="messages">Messages which were dragged.</param>
		public MessagesDragDropEventArgs(QSetQueueItem fromQueueItem, QSetQueueItem toQueueItem, Message[] messages)
		{
			_fromQueueItem = fromQueueItem;
			_toQueueItem = toQueueItem;
			_messages = messages;
		}


		/// <summary>
		/// Gets the queue where messages were dragged from.
		/// </summary>
		public QSetQueueItem FromQueueItem
		{
			get
			{
				return _fromQueueItem;
			}
		}


		/// <summary>
		/// Gets the queue where messages were dragged to.
		/// </summary>
		public QSetQueueItem ToQueueItem
		{
			get
			{
				return _toQueueItem;
			}
		}


		/// <summary>
		/// Gets the messages which were dragged.
		/// </summary>
		public Message[] Messages
		{
			get
			{
				return _messages;
			}
		}
	}


	/// <summary>
	/// Event arguments for QSetDeactivated event delgate.
	/// </summary>
	internal class QSetDeactivatedEventArgs : QSetControlItemAffectedEventArgs
	{
		/// <summary>
		/// Constructs object.
		/// </summary>
		/// <param name="item">Q Set which was deactivated.</param>
		public QSetDeactivatedEventArgs(QSetItemBase item) 
			: base (item) {}
	}

	#endregion
}
