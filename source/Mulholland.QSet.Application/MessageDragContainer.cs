using System;
using System.Messaging;
using Mulholland.QSet.Model;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Container object used for drag drop operations with messages..
	/// </summary>
	internal class MessageDragContainer
	{
		private QSetQueueItem _ownerQueueItem; 
		private Message[] _messages;

		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="ownerQueueItem">Queue which the messages belong to.</param>
		/// <param name="messages">Messages which are being dragged.</param>
		public MessageDragContainer(QSetQueueItem ownerQueueItem, Message[] messages)
		{
			_ownerQueueItem = ownerQueueItem; 
			_messages = messages;
		}


		/// <summary>
		/// Gets the queue which the dragged messages exist in.
		/// </summary>
		public QSetQueueItem OwnerQueueItem
		{
			get
			{
				return _ownerQueueItem;
			}
		}


		/// <summary>
		/// Gets the dragged messages.
		/// </summary>
		public Message[] Messages
		{
			get
			{
				return _messages;
			}
		}
	}
}
