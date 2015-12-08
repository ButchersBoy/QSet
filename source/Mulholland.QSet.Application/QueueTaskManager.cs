using System;
using System.Messaging;
using WindowsForms = System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Resources;
using Mulholland.QSet.Model;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Provides tools for assisting with queue operations.
	/// </summary>
	internal class QueueTaskManager
	{
		private TaskManager _taskManager;
		private PrimaryControls _primaryControls;
		private PrimaryObjects _primaryObjects;
		private PrimaryForms _primaryForms;

		/// <summary>
		/// Constructs QueueTaskManager.
		/// </summary>
		public QueueTaskManager(
			TaskManager taskManager,
			PrimaryControls primaryControls,
			PrimaryObjects primaryObjects,
			PrimaryForms primaryForms)
		{
			_taskManager = taskManager;
			_primaryObjects = primaryObjects;
			_primaryControls = primaryControls;						
			_primaryForms = primaryForms;
		}


		/// <summary>
		/// Forwards the messages selected in the active message browser.
		/// </summary>
		/// <param name="delete">Set to true to delete the messages after they have been forwarded.</param>
		public void ForwardSelectedMessagesFromQueue(bool delete)
		{
			//check we have the correct control selected to work with
			if (_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument != null)
			{
				MessageBrowser messageBrowser = _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0] as MessageBrowser;			
				if (messageBrowser != null && messageBrowser.SelectedItems.Count > 0)
				{
					//get an array of all the messages we want to forward
					Message[] messages = new Message[messageBrowser.SelectedItems.Count];
					for (int i = 0; i < messageBrowser.SelectedItems.Count; i ++)
					{
						messages[i] = ((MessageListViewItem)messageBrowser.SelectedItems[i]).Message;
					}

					NewMessageForm form = new NewMessageForm(_primaryObjects.License, this, messages, messageBrowser.QSetQueueItem);
					form.SmallImageList = _primaryControls.Images.Icon16ImageList;
					if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
						if (delete)
							DoDeleteSelectedMessagesFromQueue(messageBrowser);
					}				
				}
			}
		}


		/// <summary>
		/// Deletes selected message from a queue, prompting the user before actioning.
		/// </summary>
		public void DeleteSelectedMessagesFromQueue()
		{			

			//validate license
			if (_primaryObjects.License.ValidateFeatureUse(Licensing.Feature.DeleteMessage))
			{

				//check we have the correct control selected to work with
				if (_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument != null)
				{
                    MessageBrowser messageBrowser = _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0] as MessageBrowser;			
					if (messageBrowser != null && messageBrowser.SelectedItems.Count > 0)
					{
						//confirm the delete
						if (WindowsForms.MessageBox.Show(_primaryForms.EnvironmentForm, 
							string.Format(Locale.UserMessages.ConfirmMessageDelete, messageBrowser.SelectedItems.Count > 1 ? "s" : ""), 
							Locale.ApplicationName, 
							WindowsForms.MessageBoxButtons.YesNoCancel, 
							WindowsForms.MessageBoxIcon.Question, 
							WindowsForms.MessageBoxDefaultButton.Button3) == WindowsForms.DialogResult.Yes)
						{
							DoDeleteSelectedMessagesFromQueue(messageBrowser);
						}
					}
				}
			
			}
		}


		/// <summary>
		/// Displays the new message form to send a message.
		/// </summary>
		public void SendNewMessage()
		{
			SendNewMessage(null);
		}


		/// <summary>
		/// Displays the new message form to send a message.
		/// </summary>
		/// <param name="queue">Initial queue to show in the recipients list.</param>
		public void SendNewMessage(QSetQueueItem queue)
		{
			NewMessageForm form = new NewMessageForm(_primaryObjects.License, this, queue);
			form.SmallImageList = _primaryControls.Images.Icon16ImageList;
			form.Show();
		}


		/// <summary>
		/// Performs a bulk message send.
		/// </summary>
		/// <param name="recipientQueues">Array of queues to send to.</param>
		/// <param name="message">Message to send.</param>
		/// <param name="copies">Number of copies of the message to send to each queue.</param>
		public void BulkSend(MessageQueue[] recipientQueues, Message message, int copies)
		{			
			//TODO get this method to receive an array of messages, and return an object conataining all messages, queues which were not sent

			VisualizableProcess process = new VisualizableProcess(string.Format("Sending message{0}...", copies > 1 ? "s" : ""), false);
			_primaryObjects.ProcessVisualizer.ProcessStarting(process);

			try
			{
				foreach (MessageQueue queue in recipientQueues)
				{
					try
					{
						for (int i = 0; i < copies; i++)
							SendMessage(queue, message);
					}
					catch (Exception exc)
					{
						string errorMsg = string.Format("Unable to send message to {0}.", queue.QueueName);
						if (copies > 1)
							errorMsg += "\n\nNo further attempt will be made to send messages to this queue.";
						errorMsg += string.Format("\n\n{0}", exc);

						WindowsForms.MessageBox.Show(
							_primaryForms.EnvironmentForm, 
							errorMsg,
							Locale.ApplicationName, 
							System.Windows.Forms.MessageBoxButtons.OK, 
							System.Windows.Forms.MessageBoxIcon.Exclamation);
					}
				}
			}
			catch (Exception exc)
			{
				WindowsForms.MessageBox.Show(
					_primaryForms.EnvironmentForm, 
					string.Format("Encoutered an error during send:\n\n{0}", exc.Message),
					Locale.ApplicationName, 
					System.Windows.Forms.MessageBoxButtons.OK, 
					System.Windows.Forms.MessageBoxIcon.Exclamation);
			}

			_primaryObjects.ProcessVisualizer.ProcessCompleted(process);

		}


		/// <summary>
		/// Refreshes the active queue.
		/// </summary>
		public void RefreshActiveQueue()
		{
            if (_primaryControls.DocumentContainer.Manager.ActiveTabbedDocument != null)
			{
                MessageBrowser messageBrowser = _primaryControls.DocumentContainer.Manager.ActiveTabbedDocument.Controls[0] as MessageBrowser;			
				if (messageBrowser != null)
				{
					messageBrowser.Refresh();
				}
			}
		}


		/// <summary>
		/// Pugres the queue contents from the QSetQueueItem.
		/// </summary>
		/// <param name="queueItem">Item which contains the quue to be purged.</param>
		public void PurgeQueue(QSetQueueItem queueItem)
		{
			QSetMessageQueue queue = queueItem.QSetMessageQueue;

			string confirmationMessage = string.Format(Locale.UserMessages.ConfirmQueuePurge, string.Format(@"{0}\{1}", queue.MachineName, queue.QueueName));					

			if (WindowsForms.MessageBox.Show(_primaryForms.EnvironmentForm, confirmationMessage, Locale.ApplicationName, WindowsForms.MessageBoxButtons.YesNoCancel, WindowsForms.MessageBoxIcon.Question, WindowsForms.MessageBoxDefaultButton.Button3) == WindowsForms.DialogResult.Yes)
			{
				VisualizableProcess process = new VisualizableProcess(Locale.UserMessages.PurgingQueue, false);
				try
				{													
					_primaryObjects.ProcessVisualizer.ProcessStarting(process);
					queue.Purge();													
				}
				catch (Exception exc)
				{
					WindowsForms.MessageBox.Show(_primaryForms.EnvironmentForm, string.Format(Locale.UserMessages.UnableToPurgeQueue, exc.Message), Locale.ApplicationName, WindowsForms.MessageBoxButtons.OK, WindowsForms.MessageBoxIcon.Exclamation);
				}
				finally
				{
					_primaryObjects.ProcessVisualizer.ProcessCompleted(process);							
				}										
			}
		}


		/// <summary>
		/// Displays a dialogue to the user, allowing them to create a new queue.
		/// </summary>
		public void CreateQueue()
		{
			NewQueueForm newQueueForm = new NewQueueForm();
			newQueueForm.ShowDialog(_primaryForms.EnvironmentForm);
			newQueueForm.Dispose();
		}


		/// <summary>
		/// Attempts to delete a MSMQ message queue.
		/// </summary>		
		public void DeleteActiveQueue()
		{
			if (_primaryControls.QSetExplorer.ActiveItem != null)
			{
				QSetQueueItem queueItem = _primaryControls.QSetExplorer.ActiveItem as QSetQueueItem;
				if (queueItem != null)
				{
					//confirm the delete
					if (WindowsForms.MessageBox.Show(_primaryForms.EnvironmentForm, 
						string.Format(Locale.UserMessages.ConfirmQueueDelete, queueItem.Name), 
						Locale.ApplicationName, 
						WindowsForms.MessageBoxButtons.YesNoCancel, 
						WindowsForms.MessageBoxIcon.Question, 
						WindowsForms.MessageBoxDefaultButton.Button3) == WindowsForms.DialogResult.Yes)
					{
						try
						{
							MessageQueue.Delete(queueItem.Name);

							if (queueItem.ParentItem != null)
							{
								queueItem.ParentItem.ChildItems.Remove(queueItem.Name);
							}
						}
						catch (Exception exc)
						{
							WindowsForms.MessageBox.Show(
								_primaryForms.EnvironmentForm, 
								string.Format(Locale.UserMessages.UnableToDeleteQueue, exc.Message),
								Locale.ApplicationName, 
								System.Windows.Forms.MessageBoxButtons.OK, 
								System.Windows.Forms.MessageBoxIcon.Exclamation);
						}
					}
				}
			}
		}


		/// <summary>
		/// Duplicates a message.
		/// </summary>
		/// <param name="sourceQueue">Queue which contains the message to duplicate.</param>
		/// <param name="sourceMessageId">ID of message to duplicate.</param>
		/// <returns>New <see cref="Message"/>.</returns>
		public Message DuplicateMessage(QSetMessageQueue sourceQueue, string sourceMessageId)
		{
			sourceQueue.MessageReadPropertyFilter.Body = true;
			sourceQueue.MessageReadPropertyFilter.Label = true;
			Message sourceMessage = sourceQueue.PeekById(sourceMessageId);
			sourceMessage.Formatter = new XmlMessageFormatter();
			System.Messaging.Message newMessage = new System.Messaging.Message();			
			Mulholland.Core.IOUtilities.CopyStream(sourceMessage.BodyStream, newMessage.BodyStream);
			newMessage.Label = sourceMessage.Label;

			return newMessage;
		}


		/// <summary>
		/// Copies or moves messages from one queue to another.
		/// </summary>
		/// <param name="fromQueueItem">Source queue.</param>
		/// <param name="toQueueItem">Destination queue.</param>
		/// <param name="messages">Messages to move or copy.</param>
		/// <param name="deleteSourceMessagesOnComplete">Set to true to move the messages, false to copy the messages.</param>
		public void CopyMessages(QSetQueueItem fromQueueItem, QSetQueueItem toQueueItem, Message[] messages, bool deleteSourceMessagesOnComplete)
		{
			try
			{
				//validate license
				if (_primaryObjects.License.ValidateFeatureUse(Licensing.Feature.DragAndDropMessage))
				{

					if (fromQueueItem.QSetMessageQueue.CanRead)
					{
						if (toQueueItem.QSetMessageQueue.CanWrite)
						{						
							//attempt to get message browsers for the queues
							MessageBrowser fromMessageBrowser = null;
							MessageBrowser toMessageBrowser = null;
							foreach (MessageBrowser messageBrowser in _primaryControls.MessageBrowserCollection)
							{
								if (messageBrowser.QSetQueueItem == fromQueueItem)
									fromMessageBrowser = messageBrowser;
								else if (messageBrowser.QSetQueueItem == toQueueItem)
									toMessageBrowser = messageBrowser;

								if (toMessageBrowser != null && (fromMessageBrowser != null || !deleteSourceMessagesOnComplete))
									break;
							}

							//move/copy the messages
							foreach (Message message in messages)
							{
								SendMessage((MessageQueue)toQueueItem.QSetMessageQueue, fromQueueItem.QSetMessageQueue.PeekById(message.Id));
								//SendMessage((MessageQueue)toQueueItem.QSetMessageQueue, DuplicateMessage(fromQueueItem.QSetMessageQueue, message.Id));
								if (deleteSourceMessagesOnComplete)							
									fromQueueItem.QSetMessageQueue.ReceiveById(message.Id);							
							}

							//update the message browsers
							if (toMessageBrowser != null)
								toMessageBrowser.Refresh();
							if (fromMessageBrowser != null && deleteSourceMessagesOnComplete && toMessageBrowser != fromMessageBrowser)
								fromMessageBrowser.Refresh();
						}
						else
						{
							//TODO use locale 
							WindowsForms.MessageBox.Show(
								_primaryForms.EnvironmentForm, 
								"Insufficient permissions to write to the target queue.",
								Locale.ApplicationName, 
								System.Windows.Forms.MessageBoxButtons.OK, 
								System.Windows.Forms.MessageBoxIcon.Information);
						}
					}
					else
					{
						//TODO use locale 
						WindowsForms.MessageBox.Show(
							_primaryForms.EnvironmentForm, 
							"Insufficient permissions to read from queue.",
							Locale.ApplicationName, 
							System.Windows.Forms.MessageBoxButtons.OK, 
							System.Windows.Forms.MessageBoxIcon.Information);
					}
				}
			}
			catch (Exception exc)
			{
				//TODO use locale and have a message that can distinguish between move and copy
				WindowsForms.MessageBox.Show(
					_primaryForms.EnvironmentForm, 
					string.Format("Encountered an error during move.\n\n{0}", exc.Message),
					Locale.ApplicationName, 
					System.Windows.Forms.MessageBoxButtons.OK, 
					System.Windows.Forms.MessageBoxIcon.Information);
			}
		}


		/// <summary>
		/// Sends a message to a message queue.
		/// </summary>
		/// <param name="target"></param>
		/// <param name="message"></param>
		private void SendMessage(MessageQueue target, Message message)
		{
			if (target.Transactional && target.Path.IndexOf("\\private$\\") != -1)
			{
				MessageQueueTransaction tran = new MessageQueueTransaction();	
				try
				{
					tran.Begin();
					target.Send(message, tran);
					tran.Commit();
				}
				catch
				{
					tran.Abort();
				}
				finally
				{
					tran.Dispose();
				}
			}
			else
				target.Send(message);
		}


		/// <summary>
		/// Deletes selected messages from a queue without prompting the user.
		/// </summary>
		private void DoDeleteSelectedMessagesFromQueue(MessageBrowser messageBrowser)
		{
			//visualize
			VisualizableProcess process = new VisualizableProcess(
				string.Format(Locale.UserMessages.RetrievingMessages, messageBrowser.SelectedItems.Count > 1 ? "s" : "") , false);
			_primaryObjects.ProcessVisualizer.ProcessStarting(process);
						
			try
			{
				//delete all selected messages
				while (messageBrowser.SelectedItems.Count > 0)
				{
					messageBrowser.QSetQueueItem.QSetMessageQueue.ReceiveById(((MessageListViewItem)messageBrowser.SelectedItems[0]).Message.Id);
					messageBrowser.SelectedItems[0].Remove();
				}
			}
			catch (Exception exc)
			{
				WindowsForms.MessageBox.Show(_primaryForms.EnvironmentForm, string.Format(Locale.UserMessages.UnableToDeleteMessage, exc.Message), Locale.ApplicationName, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
			}						
						
			//tidyup UI
			_taskManager.MenuStateManger.SetAllMenusState();
			_primaryObjects.ProcessVisualizer.ProcessCompleted(process);
		}

	}
}
