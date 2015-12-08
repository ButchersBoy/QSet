using System;
using System.ComponentModel;
using System.Messaging;

namespace Mulholland.QSet.Model
{
	/// <summary>
	/// Derives a standard message queue to provide extra properties.
	/// </summary>
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class QSetMessageQueue : MessageQueue
	{
		private QSetQueueItem _ownerItem = null;		

		/// <summary>
		/// Initializes a new instance of the <see cref="System.Messaging.MessageQueue"/> class. After the default constructor initializes the new instance, you must set the instance's Path property before you can use the instance.
		/// </summary>
		/// <param name="ownerItem">QSetQueueItem which owns the <see cref="System.Messaging.MessageQueue"/>.</param>
		public QSetMessageQueue(QSetQueueItem ownerItem)
			: base ()		
		{
			_ownerItem = ownerItem;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="System.Messaging.MessageQueue"/> class that references the Message Queuing queue at the specified path.
		/// </summary>
		/// <param name="ownerItem">QSetQueueItem which owns the <see cref="System.Messaging.MessageQueue"/>.</param>
		/// <param name="path">The location of the queue referenced by this <see cref="System.Messaging.MessageQueue"/>.</param>
		public QSetMessageQueue(QSetQueueItem ownerItem, string path)
			: base(path)
		{
			_ownerItem = ownerItem;
		}


		/// <summary>
		/// Initializes a new instance of the <see cref="System.Messaging.MessageQueue"/>MessageQueue class that references the Message Queuing queue at the specified path and with the specified read-access restriction.
		/// </summary>
		/// <param name="ownerItem">QSetQueueItem which owns the <see cref="System.Messaging.MessageQueue"/>.</param>
		/// <param name="path">The location of the queue referenced by this <see cref="System.Messaging.MessageQueue"/>.</param>
		/// <param name="sharedModeDenyReceive">true to grant exclusive read access to the first application that accesses the queue; otherwise, false.</param>
		public QSetMessageQueue(QSetQueueItem ownerItem, string path, bool sharedModeDenyReceive)
			: base(path, sharedModeDenyReceive)
		{
			_ownerItem = ownerItem;			
		}



		/// <summary>
		/// Gets or sets the QSetQueueItem associated with the message queue.  Property may be null.
		/// </summary>
		[Browsable(false)]
		public QSetQueueItem OwnerItem
		{
			get
			{
				return _ownerItem;
			}
			set
			{
				_ownerItem = value;
			}
		}


		/// <summary>
		/// Gets the name of the message queue.
		/// </summary>
		[Browsable(false)]
		public string Name
		{
			get
			{
				return _ownerItem.Name;				
			}
		}


		/// <summary>
		/// Gets the unique queue name that Message Queuing generated at the time of the queue's creation.
		/// </summary>
		public new string FormatName
		{
			get
			{
				return base.FormatName;
			}
		}

	}
}
