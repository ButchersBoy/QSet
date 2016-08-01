using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using Mulholland.Core;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application.Controls
{
    /// <summary>
    /// Summary description for MessageBrowser.
    /// </summary>
    internal class MessageBrowser : System.Windows.Forms.UserControl, IQSetItemControl
    {
        private QSetQueueItem _qSetQueueItem;								
        private Thread _messageLoadThread;		
        private ManualResetEvent _startedEvent = new ManualResetEvent(false);
        private VisualizableProcess _workingProcess;
        private UserSettings _userSettings = new UserSettings(); //utilise defaults until the main settings are provided
        private Rectangle _dragBoxFromMouseDown;

        private event SelectedMessageChangedEvent _selectedMessageChanged;
        private event VisualizableProcessEvent _beforeMessageListLoaded;
        private event VisualizableProcessEvent _afterMessageListLoaded;		
        private event MessageLoadExceptionEvent _messageLoadException;
        private event MessagesDragDropEvent _messagesDragDrop;		
        
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.Windows.Forms.ListView messagesListView;
        private System.Windows.Forms.Panel workingPanel;
        private System.Windows.Forms.Label retrievingMessagesLabel;
        private System.Windows.Forms.LinkLabel cancelLinkLabel;
        private System.ComponentModel.Container components = null;

        #region events

        /// <summary>
        /// Occurs when a message is to be selected in the viewer.
        /// </summary>
        public event SelectedMessageChangedEvent SelectedMessageChanged
        {
            add
            {
                _selectedMessageChanged += value;
            }
            remove
            {
                _selectedMessageChanged -= value;
            }
        }


        /// <summary>
        /// Occurs just before the browser starts retrieving the message list from the queue.
        /// </summary>
        public event VisualizableProcessEvent BeforeMessageListLoaded
        {
            add
            {
                _beforeMessageListLoaded += value;
            }
            remove
            {
                _beforeMessageListLoaded -= value;
            }
        }


        /// <summary>
        /// Occurs after the message list has been loaded and the browser has been populated.
        /// </summary>
        public event VisualizableProcessEvent AfterMessageListLoaded
        {
            add
            {
                _afterMessageListLoaded += value;
            }
            remove
            {
                _afterMessageListLoaded -= value;
            }
        }


        /// <summary>
        /// Occurs when an exception occurs while the control is trying to retrieve messages from the queue.
        /// </summary>
        public event MessageLoadExceptionEvent MessageLoadException
        {
            add
            {
                _messageLoadException += value;
            }
            remove 
            {
                _messageLoadException -= value;
            }
        }


        /// <summary>
        /// Occurs when messages are dragged between queues.
        /// </summary>
        public event MessagesDragDropEvent MessagesDragDrop
        {
            add
            {
                _messagesDragDrop += value;
            }
            remove
            {
                _messagesDragDrop -= value;
            }
        }


        /// <summary>
        /// Raises the MessageLoadException event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected void OnMessageLoadException(MessageLoadExceptionEventArgs e)
        {
            try
            {
                if (_messageLoadException != null)
                    _messageLoadException(this, e);
            }
            catch {}
        }


        /// <summary>
        /// Raises the SelectedMessageChanged event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected void OnSelectedMessageChangedEvent(SelectedMessageChangedEventArgs e)
        {
            try
            {
                if (_selectedMessageChanged != null)
                    _selectedMessageChanged(this, e);
            }
            catch {}
        }


        /// <summary>
        /// Raises the BeforeMessageListLoaded event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected void OnBeforeMessageListLoaded(VisualizableProcessEventArgs e)
        {
            try
            {
                if (_beforeMessageListLoaded != null)
                    _beforeMessageListLoaded(this, e);
            }
            catch {}
        }


        /// <summary>
        /// Raises the AfterMessageListLoaded event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected void OnAfterMessageListLoaded(VisualizableProcessEventArgs e)
        {
            try
            {
                if (_afterMessageListLoaded != null)
                    _afterMessageListLoaded(this, e);
            }
            catch {}
        }		


        /// <summary>
        /// Raises the <see cref="MessagesDragDrop"/> event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected void OnMessagesDragDrop(MessagesDragDropEventArgs e)
        {
            try
            {
                if (_messagesDragDrop != null)
                    _messagesDragDrop(this, e);
            }
            catch {}
        }

        #endregion

        /// <summary>
        /// Constructs the object.
        /// </summary>
        public MessageBrowser()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            _qSetQueueItem = null;			

            base.Disposed += new EventHandler(MessageBrowser_Disposed);			
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
            this.messagesListView = new System.Windows.Forms.ListView();
            this.workingPanel = new System.Windows.Forms.Panel();
            this.cancelLinkLabel = new System.Windows.Forms.LinkLabel();
            this.retrievingMessagesLabel = new System.Windows.Forms.Label();
            this.workingPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // messagesListView
            // 
            this.messagesListView.AllowDrop = true;
            this.messagesListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.messagesListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messagesListView.FullRowSelect = true;
            this.messagesListView.HideSelection = false;
            this.messagesListView.Location = new System.Drawing.Point(1, 1);
            this.messagesListView.Name = "messagesListView";
            this.messagesListView.Size = new System.Drawing.Size(534, 402);
            this.messagesListView.TabIndex = 1;
            this.messagesListView.View = System.Windows.Forms.View.Details;
            this.messagesListView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.messagesListView_MouseDown);
            this.messagesListView.MouseMove += new MouseEventHandler(messagesListView_MouseMove);
            this.messagesListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.messagesListView_DragDrop);
            this.messagesListView.DragOver += new DragEventHandler(messagesListView_DragOver);
            this.messagesListView.SelectedIndexChanged += new System.EventHandler(this.messagesListView_SelectedIndexChanged);
            // 
            // workingPanel
            // 
            this.workingPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.workingPanel.BackColor = System.Drawing.SystemColors.Window;
            this.workingPanel.Controls.Add(this.cancelLinkLabel);
            this.workingPanel.Controls.Add(this.retrievingMessagesLabel);
            this.workingPanel.Location = new System.Drawing.Point(4, 20);
            this.workingPanel.Name = "workingPanel";
            this.workingPanel.Size = new System.Drawing.Size(528, 40);
            this.workingPanel.TabIndex = 2;
            // 
            // cancelLinkLabel
            // 
            this.cancelLinkLabel.Location = new System.Drawing.Point(4, 20);
            this.cancelLinkLabel.Name = "cancelLinkLabel";
            this.cancelLinkLabel.Size = new System.Drawing.Size(100, 16);
            this.cancelLinkLabel.TabIndex = 1;
            this.cancelLinkLabel.TabStop = true;
            this.cancelLinkLabel.Text = "Cancel";
            this.cancelLinkLabel.Click += new System.EventHandler(this.cancelLinkLabel_Click);
            // 
            // retrievingMessagesLabel
            // 
            this.retrievingMessagesLabel.Location = new System.Drawing.Point(4, 4);
            this.retrievingMessagesLabel.Name = "retrievingMessagesLabel";
            this.retrievingMessagesLabel.Size = new System.Drawing.Size(136, 16);
            this.retrievingMessagesLabel.TabIndex = 0;
            this.retrievingMessagesLabel.Text = "Retrieving messages...";
            // 
            // MessageBrowser
            // 
            this.AllowDrop = true;
            this.Controls.Add(this.workingPanel);
            this.Controls.Add(this.messagesListView);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
            this.Name = "MessageBrowser";
            this.Size = new System.Drawing.Size(536, 404);
            this.workingPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        /// <summary>
        /// Refreshes the view.
        /// </summary>
        /// <exception cref="UnableToReadQueueException">Thrown if the queue cannot be read.</exception>
        public override void Refresh()
        {			
            AttemptMessageLoadStart();
        }


        /// <summary>
        /// Gets or sets the queue item being browsed.
        /// </summary>
        /// <exception cref="UnableToReadQueueException">Thrown if the queue cannot be read.</exception>
        public QSetQueueItem QSetQueueItem
        {
            get
            {
                return _qSetQueueItem;
            }
            set
            {
                if (value != QSetQueueItem)
                {
                    _qSetQueueItem = value;
                    AttemptMessageLoadStart();
                }
            }
        }


        /// <summary>
        /// Gets the currently selected message.
        /// </summary>
        public ListView.SelectedListViewItemCollection SelectedItems
        {
            get
            {
                return messagesListView.SelectedItems;
            }
        }


        /// <summary>
        /// Gets or sets the image list associated with the control.
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return messagesListView.SmallImageList;
            }
            set
            {
                messagesListView.SmallImageList = value;
            }
        }
    

        /// <summary>
        /// Gets or sets the user settings object associated with the control.
        /// </summary>
        public UserSettings UserSettings
        {
            get
            {
                return _userSettings;
            }
            set
            {
                _userSettings = value;
            }
        }


        /// <summary>
        /// Finds a list item which holds a message.
        /// </summary>
        /// <param name="message">Message to search for.</param>
        /// <returns>Message which contains the message if found, else null</returns>
        private MessageListViewItem FindListItem(System.Messaging.Message message)
        {
            MessageListViewItem result = null;

            foreach (ListViewItem item in messagesListView.Items)
            {
                MessageListViewItem messageItem = item as MessageListViewItem;
                if (messageItem != null && messageItem.Message == message)
                {
                    result = messageItem;
                    break;
                }
            }

            return result;
        }

        
        /// <summary>
        /// Aborts the current thread loading messages, if a thread is running.
        /// </summary>
        private void AttemptMessageLoadStart()
        {
            lock (this)
            {
                //abort any current load
                if (_messageLoadThread != null)
                    FinishMessageLoad(true);

                messagesListView.Items.Clear();

                if (_qSetQueueItem != null)
                {
                    workingPanel.Visible = true;
                }

                //create the required column headers
                messagesListView.Clear();
                foreach (string columnName in _userSettings.MessageBrowserColumnListCollection)
                    messagesListView.Columns.Add(columnName, (messagesListView.Width - 10) / _userSettings.MessageBrowserColumnListCollection.Count, HorizontalAlignment.Left);

                if (_qSetQueueItem != null)
                {
                    //start the worker thread
                    _startedEvent.Reset();
                    ThreadPool.QueueUserWorkItem(new WaitCallback(GetSnapShotCallBack));
                    _startedEvent.WaitOne();

                    //set up the visualisable process
                    _workingProcess = new VisualizableProcess(Locale.UserMessages.RetrievingMessages, true);
                    OnBeforeMessageListLoaded(new VisualizableProcessEventArgs(_workingProcess));
                }
            }
        }

        private delegate void Action();

        /// <summary>
        /// Finishes the message load process.
        /// </summary>
        /// <param name="abort">Set to true if the aborting a message load prematurely.</param>
        private void FinishMessageLoad(bool abort)
        {
            lock (this)
            {
                if (abort && _messageLoadThread != null)				
                    _messageLoadThread.Abort();				                

                if (_workingProcess != null)
                {
                    Action x = delegate
                    {
                        if (workingPanel.Visible) workingPanel.Visible = false;
                    };

                    OnAfterMessageListLoaded(new VisualizableProcessEventArgs(_workingProcess));

                    workingPanel.Invoke(x);

                    _messageLoadThread = null;				
                }
            }
        }


        /// <summary>
        /// Prepares the list view columns and read property filter according to the user settings, prior to a refresh.
        /// </summary>
        /// <param name="messagePropertyInfoArray">Returns an array of PropertyInfo objects, 
        /// containing the Message properties which map to displayed ColumnHeaders, in the correct order.</param>
        private void PrepareReadPropertyFilterAndListViewColumns(out PropertyInfo[] messagePropertyInfoArray)
        {			
            //set the read filter according to the required columns
            PropertyInfo[] readFilterPropertyInfoArray = _qSetQueueItem.QSetMessageQueue.MessageReadPropertyFilter.GetType().GetProperties();
            messagePropertyInfoArray = new PropertyInfo[_userSettings.MessageBrowserColumnListCollection.Count];
            PropertyInfo[] allMessagePropertyInfoArray = typeof(System.Messaging.Message).GetProperties();			
            for (int filterPropertyIndex = 0; filterPropertyIndex <= readFilterPropertyInfoArray.GetUpperBound(0); filterPropertyIndex ++)
            {
                //set the read filter property
                PropertyInfo readFilterProperty = readFilterPropertyInfoArray[filterPropertyIndex];
                if (!Utilities.IsStringInArray(Constants.MessageBrowserColumnExclusionList, readFilterProperty.Name))
                {
                    bool retrievePropertyValue = _userSettings.MessageBrowserColumnListCollection.Contains(readFilterProperty.Name) ? true : false;
                    readFilterProperty.SetValue(_qSetQueueItem.QSetMessageQueue.MessageReadPropertyFilter, retrievePropertyValue, null);
                
                    //find the corresponding message property and add to the returned list, at the correct position
                    if (retrievePropertyValue)
                        foreach (PropertyInfo messageProperty in allMessagePropertyInfoArray)				
                            if (messageProperty.Name == readFilterProperty.Name)
                            {								
                                messagePropertyInfoArray[_userSettings.MessageBrowserColumnListCollection.IndexOf(messageProperty.Name)] = messageProperty;							
                                break;
                            }
                }
            }			
    
            //always retrieve the body and the formatter
            _qSetQueueItem.QSetMessageQueue.MessageReadPropertyFilter.Body = true;
        }

        
        /// <summary>
        /// Gets a snap shot of the current queue, and loads in the the list view.
        /// </summary>						
        private void GetSnapShotCallBack(object state)
        {									
            bool isTaskAborted = false;			

            //set up reference to worker thread before releasing initiating thread
            _messageLoadThread = Thread.CurrentThread;			
            _startedEvent.Set();			

            try
            {																																													
                if (_userSettings.MessageBrowserColumnListCollection.Count > 0)
                {
                
                    //set the read filter and matching column headers
                    PropertyInfo[] messagePropertyInfoArray;
                    PrepareReadPropertyFilterAndListViewColumns(out messagePropertyInfoArray);				

                    //check we are OK for load
                    if (_qSetQueueItem != null)
                    {
                        //if (_qSetQueueItem.QSetMessageQueue.CanRead)
                        //{					
                        System.Messaging.Message[] messagesSnapShot;
                        try
                        {
                            //retrieve all messages
                            messagesSnapShot = _qSetQueueItem.QSetMessageQueue.GetAllMessages();
                        }
                        catch (System.Messaging.MessageQueueException mQexc)
                        {
                            throw new UnableToReadQueueException(string.Format(Locale.UserMessages.UnableToReadQueueDueToError, 
                                string.Format("{0} HRESULT/MSMQ Error: {1}/{2}.", mQexc.Message, mQexc.ErrorCode, mQexc.MessageQueueErrorCode) ));						
                        }
                        catch (Exception exc)
                        {
                            throw new UnableToReadQueueException(string.Format(Locale.UserMessages.UnableToReadQueueDueToError, exc.Message));						
                        }
                        
                        //load all of the messages into the list view
                        Action ffs = () => workingPanel.Visible = false;
                        workingPanel.Invoke(ffs);                        
                        foreach (System.Messaging.Message message in messagesSnapShot)
                        {						
                            //create the list view item
                            object propertyValue = "Not Available";
                            try
                            {
                                //TODO fine tune this, so we 'black list' properties which fail
                                //		(some are not available on workgroup machines)
                                propertyValue = messagePropertyInfoArray[0].GetValue(message, null);
                            }
                            catch {}
                            string propertyValueString = propertyValue == null ? "" : propertyValue.ToString();
                            MessageListViewItem messageItem = 
                                new MessageListViewItem(message, propertyValueString, (int)Images.IconType.Message);	

                            //add all of the column data
                            if (_userSettings.MessageBrowserColumnListCollection.Count > 1)
                            {
                                for (int i = 1; i <= messagePropertyInfoArray.GetUpperBound(0); i ++)
                                {
                                    string subItemPropertyValue = "Not available";
                                    try
                                    {
                                        //TODO fine tune this, so we 'black list' properties which fail
                                        //		(some are not available on workgroup machines)
                                        subItemPropertyValue = messagePropertyInfoArray[i].GetValue(message, null).ToString();
                                    }
                                    catch {}
                                    messageItem.SubItems.Add(subItemPropertyValue);
                                }
                            }

                            //add the item into the list on a safe thread
                            if (messagesListView.InvokeRequired)
                                messagesListView.Invoke(new AddMessageItemToListViewDelegate(AddMessageItemToListView), new object[] {messageItem});
                            else
                                AddMessageItemToListView(messageItem);																						
                        }
                        //}
                        //else
                        //{						
                            //insufficient access to read queue
                        //	throw new UnableToReadQueueException(Locale.UserMessages.UnableToReadQueueDueToInsufficientAccessRights);						
                        //}
                    }
                }
                
            }				
            catch (Exception exc)
            {				
                if (exc is ThreadAbortException)
                    isTaskAborted = true;									
                else if (exc is UnableToReadQueueException)
                    OnMessageLoadException(new MessageLoadExceptionEventArgs(exc));
                else
                    OnMessageLoadException(new MessageLoadExceptionEventArgs(new UnableToReadQueueException(string.Format(Locale.UserMessages.UnableToReadQueueDueToError, exc.Message), exc)));											
            }
            finally
            {
                if (!isTaskAborted)
                    FinishMessageLoad(false);
            }
        }


        private delegate void AddMessageItemToListViewDelegate(MessageListViewItem messageItem);


        /// <summary>
        /// Adds a message to the list view.
        /// </summary>
        /// <param name="messageItem">Message to add.</param>
        private void AddMessageItemToListView(MessageListViewItem messageItem)
        {
            messagesListView.Items.Add(messageItem);
        }


        /// <summary>
        /// Handles the list view item select.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void messagesListView_SelectedIndexChanged(object sender, System.EventArgs e)
        {			
            if (messagesListView.SelectedItems.Count > 0)
            {				
                messagesListView.SelectedItems[0].ImageIndex = (int)Images.IconType.MessageOpen;
                OnSelectedMessageChangedEvent(new SelectedMessageChangedEventArgs(_qSetQueueItem, ((MessageListViewItem)messagesListView.SelectedItems[0]).Message));
            }
        }


        private void cancelLinkLabel_Click(object sender, System.EventArgs e)
        {
            FinishMessageLoad(true);				
        }


        private void MessageBrowser_Disposed(object sender, EventArgs e)
        {
            FinishMessageLoad(true);				
        }

        private void messagesListView_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnMouseDown(e);
        
            if (messagesListView.GetItemAt(e.X, e.Y) != null)
            {
                                
                // Remember the point where the mouse down occurred. The DragSize indicates
                // the size that the mouse can move before a drag event should be started.                
                Size dragSize = SystemInformation.DragSize;

                // Create a rectangle using the DragSize, with the mouse position being
                // at the center of the rectangle.
                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width /2),
                    e.Y - (dragSize.Height /2)), dragSize);
            } 
            else
                // Reset the rectangle if the mouse is not over an item in the ListBox.
                _dragBoxFromMouseDown = Rectangle.Empty;
        }


        private void messagesListView_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(MessageDragContainer)))
            {
                MessageDragContainer messageDragContainer = (MessageDragContainer)e.Data.GetData(typeof(MessageDragContainer));
                OnMessagesDragDrop(new MessagesDragDropEventArgs(messageDragContainer.OwnerQueueItem, _qSetQueueItem, messageDragContainer.Messages));
            }
        }


        private void messagesListView_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) 
            {
                // If the mouse moves outside the rectangle, start the drag.
                if (_dragBoxFromMouseDown != Rectangle.Empty && 
                    !_dragBoxFromMouseDown.Contains(e.X, e.Y)) 
                {
                    System.Messaging.Message[] messages = new System.Messaging.Message[messagesListView.SelectedItems.Count];
                    for (int i = 0; i < messagesListView.SelectedItems.Count; i ++) 			
                    {
                        messages[i] = ((MessageListViewItem)messagesListView.SelectedItems[i]).Message;
                    }
                    MessageDragContainer messageDragContainer = new MessageDragContainer(_qSetQueueItem, messages);
                    messagesListView.DoDragDrop(messageDragContainer, DragDropEffects.Move);
                }
            }
        }


        private void messagesListView_DragOver(object sender, DragEventArgs e)
        {			
            if (e.Data.GetDataPresent(typeof(MessageDragContainer)))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        #region IQSetItemControl Members

        public QSetItemBase QSetItem
        {
            get
            {
                if (_qSetQueueItem != null)
                    return (QSetItemBase)_qSetQueueItem;
                else
                    return null;
            }
        }

        #endregion
    }
    
    #region events

    /// <summary>
    /// BeforeMessageListLoaded event delegate.
    /// </summary>
    internal delegate void BeforeMessageListLoadedEvent(MessageBrowser sender, EventArgs e);


    /// <summary>
    /// BeforeMessageListLoaded event delegate.
    /// </summary>
    internal delegate void AfterMessageListLoadedEvent(MessageBrowser sender, EventArgs e);


    /// <summary>
    /// BeforeMessageSelected event delegate.
    /// </summary>
    internal delegate void SelectedMessageChangedEvent(MessageBrowser sender, SelectedMessageChangedEventArgs e);


    /// <summary>
    /// MessageLoadException event delegate.
    /// </summary>
    internal delegate void MessageLoadExceptionEvent(MessageBrowser sender, MessageLoadExceptionEventArgs e);


    /// <summary>
    /// Event arguments for BeforeMessageSelectedEvent.
    /// </summary>
    internal class SelectedMessageChangedEventArgs : EventArgs
    {
        private QSetQueueItem _qsetQueueItem;
        private System.Messaging.Message _message;

        /// <summary>
        /// Constructs object, setting required properties.
        /// </summary>
        /// <param name="qsetMessageQueue">The queue set queue item which the message belongs to.</param>
        /// <param name="process">Process associated with the event.</param>
        /// <param name="message">Message being selected.</param>
        public SelectedMessageChangedEventArgs (QSetQueueItem qsetQueueItem, System.Messaging.Message message)			
            : base()
        {
            _message = message;
            _qsetQueueItem = qsetQueueItem;
        }


        /// <summary>
        /// Gets the message being selected.
        /// </summary>
        public System.Messaging.Message Message
        {
            get
            {
                return _message;
            }
        }


        /// <summary>
        /// Gets the queue item that the message belongs to.
        /// </summary>
        public QSetQueueItem QSetQueueItem
        {
            get
            {
                return _qsetQueueItem;
            }
        }
    }


    /// <summary>
    /// Event arguments for MessageLoadException event.
    /// </summary>
    internal class MessageLoadExceptionEventArgs
    {
        private Exception _exception;

        /// <summary>
        /// Constructs the event arguments object.
        /// </summary>
        /// <param name="exc">Exception associated with the event.</param>
        public MessageLoadExceptionEventArgs(Exception exc)
            : base ()
        {
            _exception = exc;
        }


        /// <summary>
        /// Gets the Exception associated with the event.
        /// </summary>
        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }
    }
    
    #endregion

    #region internal class MessageListViewItem : ListViewItem

    /// <summary>
    /// ListViewItem which contains a MSMQ message.
    /// </summary>
    internal class MessageListViewItem : ListViewItem
    {

        private System.Messaging.Message _message;		

        public MessageListViewItem (System.Messaging.Message message, ListViewItem.ListViewSubItem[] subItems, int imageIndex)
            : base(subItems, imageIndex) 
        {			
            _message = message;
        }

        public MessageListViewItem (System.Messaging.Message message, string[] items, int imageIndex, Color foreColor, Color backColor, Font font) 
            : base(items, imageIndex, foreColor, backColor, font) 
        {
            _message = message;
        }

        public MessageListViewItem (System.Messaging.Message message, string[] items, int imageIndex) 
            : base(items, imageIndex) 
        {
            _message = message;
        }

        public MessageListViewItem (System.Messaging.Message message, string text, int imageIndex) 
            : base(text, imageIndex) 
        {
            _message = message;
        }

        public MessageListViewItem (System.Messaging.Message message, string text) 
            : base(text) 
        {
            _message = message;
        }

        public MessageListViewItem (System.Messaging.Message message) 
        {
            _message = message;
        }		

        public System.Messaging.Message Message
        {
            get
            {
                return _message;
            }
        }
    }

    #endregion
}
