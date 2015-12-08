using System;

namespace Mulholland.QSet.Resources
{
	/// <summary>
	/// Summary description for Locale.
	/// </summary>
	public class Locale
	{
		static Locale() {}

		
		/// <summary>
		/// Gets the company name.
		/// </summary>
		public static readonly string CompanyName = "Mulholland Software";


		/// <summary>
		/// Gets the name of the application.
		/// </summary>
		public static readonly string ApplicationName = "Q Set";

	
		/// <summary>
		/// Gets the file dialog filter which shyould be used when working with Q Set files.
		/// </summary>
		public static readonly string FileDialogQSetFilter = "Q Set files (*.qset)|*.qset|All files (*.*)|*.*";


		/// <summary>
		/// Contains terms for items which are presented to the user.
		/// </summary>
		public class Terms
		{
			static Terms() {}

			/// <summary>
			/// Term for a message queue.
			/// </summary>
			public static readonly string MessageQueue = "Message Queue";

			
			/// <summary>
			/// Outbound messages count term.
			/// </summary>
			public static readonly string OutgoingMessagesCount = "Outbound Messages";


			/// <summary>
			/// Outbound messages count term.
			/// </summary>
			public static readonly string OutgoingBytes = "Outbound Bytes";


			/// <summary>
			/// Message browsing option page.
			/// </summary>
			public static readonly string MessageBrowsingOptionPage = "Message Browsing";


			/// <summary>
			/// General options page.
			/// </summary>
			public static readonly string GeneralOptionPage = "General";
		}


		/// <summary>
		/// Provides messages to be displayed to the user.
		/// </summary>
		public class UserMessages
		{
			static UserMessages() {}			


			/// <summary>
			/// Message to display when a file could not be opened.
			/// </summary>
			public static readonly string UnableToOpenFile = "Unable to open file.";


			/// <summary>
			/// Cannot add existing item message.
			/// </summary>
			public static readonly string CannotAddItemAsAlreadyExists = "Cannot add item at this point as an item with the same name already exists.";


			/// <summary>
			/// Invalid Q Set file message.
			/// </summary>
			public static readonly string SelectedFileNotAQSetFile = "The selected file does not appear to be a valid Q Set file.  Please select a valid Q Set file.";


			/// <summary>
			/// Unable to read queue message becuase of access rights message.
			/// </summary>
			public static readonly string UnableToReadQueueDueToInsufficientAccessRights = "Insufficient access rights to read queue.";


			/// <summary>
			/// Unable to read queue message becuase of an error.
			/// </summary>
			public static readonly string UnableToReadQueueDueToError = "Unable to open or read queue.  {0}";


			/// <summary>
			/// Unable to display binary message.
			/// </summary>
			public static readonly string UnableToDisplayBinaryMessage = "Unable to display binary message.";


			/// <summary>
			/// Status bar retrieving messsages message.
			/// </summary>
			public static readonly string RetrievingMessages = "Retrieving messages...";

			
			/// <summary>
			/// Status bar retrieving properties message.
			/// </summary>
			public static readonly string RetrievingQueueProperties = "Retrieving queue properties...";

		
			/// <summary>
			/// Status bar retrieving deleting message message.
			/// </summary>
			public static readonly string DeletingMessage = "Deleting message{0}...";


			/// <summary>
			/// Status bar ready message.
			/// </summary>
			public static readonly string Ready = "Ready";


			/// <summary>
			/// Displaying message.
			/// </summary>
			public static readonly string DisplayingMessage = "Displaying message...";


			/// <summary>
			/// Unable to save user settings file message.
			/// </summary>
			public static readonly string UnableToSaveUserSettings = "Application was unable to save user settings, the file '{0}' may be marked as read only.";


			/// <summary>
			/// Confirmation message before purging message queues.
			/// </summary>
			public static readonly string ConfirmQueuePurge = "Are you sure you wish to purge all messages from the message queue '{0}'?";


			/// <summary>
			/// Confirmation for deleting messages from a queue.
			/// </summary>
			public static readonly string ConfirmMessageDelete = "Are you sure you wish to delete the selected message{0}?";


			/// <summary>
			/// Confirmation for a queue.
			/// </summary>
			public static readonly string ConfirmQueueDelete = "Are you sure you wish to delete the queue '{0}'?";


			/// <summary>
			/// Unable to purge queue error message.
			/// </summary>
			public static readonly string UnableToPurgeQueue = "Unable to purge queue:\n\n{0}";


			/// <summary>
			/// Unable to delete queue error message.
			/// </summary>
			public static readonly string UnableToDeleteQueue = "Unable to delete queue:\n\n{0}";


			/// <summary>
			/// Unable to delete queue error message.
			/// </summary>
			public static readonly string UnableToDeleteMessage = "Unable to delete message, operation did not complete:\n\n{0}";


			/// <summary>
			/// Purging queue error message.
			/// </summary>
			public static readonly string PurgingQueue = "Purging queue...";


			/// <summary>
			/// Messages displayed in options dialog.
			/// </summary>
			public class OptionsDialog
			{
				static OptionsDialog () {}


				/// <summary>
				/// Requires at least on display porperty column.
				/// </summary>
				public static readonly string AtLeastOneMessageBrowsingColumnRequired = "At least one display property must be selected for message browsing.";


				/// <summary>
				/// Invalid value for recently used file list.
				/// </summary>
				public static readonly string RecentlyUsedFielListLengthInvalid = "Recently used file list length must be a number in the range {0} to {1}.";
			}

		}
	}
}
