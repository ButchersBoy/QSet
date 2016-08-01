using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Management;
using System.Messaging;
using System.Threading;
using System.Windows.Forms;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;

namespace Mulholland.QSet.Application.Controls
{
    /// <summary>
    /// Summary description for QSetMonitor.
    /// </summary>
    internal class QSetMonitor : System.Windows.Forms.UserControl
    {
        private QSetModel _qset = null;
        private QSetMonitor.QSetMonitorWorker _qsetMonitor = null;

        private System.Windows.Forms.ListView monitorListView;
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public QSetMonitor()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            _qsetMonitor = new QSetMonitorWorker(monitorListView);
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
            this.monitorListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // monitorListView
            // 
            this.monitorListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                | System.Windows.Forms.AnchorStyles.Left) 
                | System.Windows.Forms.AnchorStyles.Right)));
            this.monitorListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.monitorListView.Location = new System.Drawing.Point(1, 1);
            this.monitorListView.Name = "monitorListView";
            this.monitorListView.Size = new System.Drawing.Size(394, 254);
            this.monitorListView.TabIndex = 0;
            // 
            // QSetMonitor
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.monitorListView);
            this.Name = "QSetMonitor";
            this.Size = new System.Drawing.Size(396, 256);
            this.ResumeLayout(false);

        }
        #endregion


        /// <summary>
        /// Gets or sets the Q Set associated with the control.
        /// </summary>
        public QSetModel QSet
        {
            get
            {
                return _qset;
            }
            set
            {
                if (_qset != value)
                {
                    _qsetMonitor.Stop();
                    _qset = value;
                    _qsetMonitor.Start(_qset);
                }
            }
        }


        /// <summary>
        /// Gets or sets the image list associated with the control.
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return monitorListView.SmallImageList;
            }
            set
            {
                monitorListView.SmallImageList = value;
            }
        }	
        
        #region private class QSetMonitorWorker

        /// <summary>
        /// Monitors a Q Set, displaying the information in a list view.
        /// </summary>
        private class QSetMonitorWorker
        {			
            private ListView _monitorListView;
            private bool _isStopRequested = false;
            private Hashtable _itemPairHashTable = null;			
            private AutoResetEvent _resetEvent = new AutoResetEvent(true); 			
            private Queue _deleteItemPairQueue = new Queue();						

            private const int _COLUMNS = 3;

            private enum SubItemList
            {	
                MessageQueueName = 0,
                OutgoingMessageCount = 1,
                OutgoingBytes = 2
            }

            private enum ResetEvents
            {
                QSetMonitor,
                RedundantQueueMonitor
            }

            #region private class QSetMonitorWorker

            /// <summary>
            /// Class which pairs a QSetQueueItem, and a ListViewItem.
            /// </summary>
            private class QueueItemListViewItemPair
            {
                private QSetQueueItem _qsetQueueItem;
                private ListViewItem _listViewItem;

                /// <summary>
                /// Constructs the pair.
                /// </summary>
                /// <param name="qsetQueueItem">QSetQueueItem of the pair.</param>
                /// <param name="listViewItem">ListViewItem of the pair.</param>
                public QueueItemListViewItemPair(QSetQueueItem qsetQueueItem, ListViewItem listViewItem)
                {
                    _qsetQueueItem = qsetQueueItem;
                    _listViewItem = listViewItem;
                }


                /// <summary>
                /// Gets the QSetQueueItem of the pair.
                /// </summary>
                public QSetQueueItem QSetQueueItem
                {
                    get
                    {
                        return _qsetQueueItem;
                    }
                }


                /// <summary>
                /// Gets ListViewItem of the pair.
                /// </summary>
                public ListViewItem ListViewItem
                {
                    get
                    {
                        return _listViewItem;
                    }
                }
            }

            #endregion

            /// <summary>
            /// Constructs the monitor.
            /// </summary>
            /// <param name="monitorListView">ListView in w</param>
            public QSetMonitorWorker(ListView monitorListView) 
            {
                _monitorListView = monitorListView;			

                _monitorListView.View = View.Details;

                _monitorListView.Columns.Add(Locale.Terms.MessageQueue, (_monitorListView.Width - 4) / _COLUMNS, HorizontalAlignment.Left);
                _monitorListView.Columns.Add(Locale.Terms.OutgoingMessagesCount, (_monitorListView.Width - 4) / _COLUMNS, HorizontalAlignment.Left);
                _monitorListView.Columns.Add(Locale.Terms.OutgoingBytes, (_monitorListView.Width - 4) / _COLUMNS, HorizontalAlignment.Left);				
            }


            /// <summary>
            /// Starts monitoring a specified Q Set.
            /// </summary>
            /// <param name="qset">Q Set to monitor.</param>
            public void Start(QSetModel qset)
            {
                //ensure any previous monitor is finished, before we re-start				
                lock (this)
                {
                    //reset member variables					
                    _resetEvent.WaitOne();					
                    _isStopRequested = false;
                    _itemPairHashTable = new Hashtable();
                    _monitorListView.Items.Clear();				

                    if (qset != null)
                    {
                        //start the monitor
                        _resetEvent.Reset();						
                                                
                        ThreadPool.QueueUserWorkItem(new WaitCallback(MonitorQSetWaitCallBack), qset);				
                    }
                    else
                        _resetEvent.Set();
                }				
            }


            /// <summary>
            /// Stops the monitor.
            /// </summary>
            public void Stop()
            {
                lock (this)
                {
                    _isStopRequested = true;				
                    _resetEvent.WaitOne();				
                    _resetEvent.Set();
                }
            }


            /// <summary>
            /// Runs the Q Set monitor.
            /// </summary>
            /// <param name="state"></param>
            private void MonitorQSetWaitCallBack(object state)
            {									
                bool subscribeToEvents = true;				

                Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                                
                while (!_isStopRequested)
                {
                    //update monitor
                    RecurseQSet((QSetModel)state, subscribeToEvents);
                    subscribeToEvents = false;

                    _monitorListView.Invoke(new Action(() =>
                    {
                        // Delete any queues which have been removed
                        while (_deleteItemPairQueue.Count > 0)
                        {
                            QueueItemListViewItemPair itemPair = (QueueItemListViewItemPair)_deleteItemPairQueue.Dequeue();
                            _itemPairHashTable.Remove(itemPair.QSetQueueItem.ID);
                            _monitorListView.Items.Remove(itemPair.ListViewItem);
                        }
                    }));

                    Thread.Sleep(250);
                }

                _resetEvent.Set();
            }	
        

            /// <summary>
            /// Recurses the Q Set, getting queue information
            /// </summary>
            /// <param name="parentItem"></param>
            /// <param name="subscribeToEvents"></param>
            private void RecurseQSet(QSetFolderItem parentItem, bool subscribeToEvents)
            {
                //subscribe to folders events if necessary
                if (subscribeToEvents)
                {
                    parentItem.ChildItems.AfterItemAdded += new AfterItemAddedEvent(ChildItems_AfterItemAdded);
                    parentItem.ChildItems.BeforeItemRemoved += new BeforeItemRemovedEvent(ChildItems_BeforeItemRemoved);
                }

                //query all children of the parent
                foreach (QSetItemBase item in parentItem.ChildItems)
                {
                    if (_isStopRequested)
                        break;

                    //monitor the queue, or query the folders children
                    if (item is QSetQueueItem)
                        CountQueueMessages(((QSetQueueItem)item));
                    else if (item is QSetFolderItem)
                        RecurseQSet((QSetFolderItem)item, subscribeToEvents);					
                }
            }

            private delegate void Action();

            private void CountQueueMessages(QSetQueueItem queueItem)
            {				
                //first of all, ensure we have a node to work with
                QueueItemListViewItemPair itemPair = null;
                if (_itemPairHashTable.ContainsKey(queueItem.ID))
                    itemPair = (QSetMonitorWorker.QueueItemListViewItemPair)_itemPairHashTable[queueItem.ID];
                else
                {
                    //TODO create icon
                    itemPair = new QueueItemListViewItemPair(queueItem, new ListViewItem(queueItem.Name, (int)Images.IconType.Queue));					
                    for (int subItemCounter = 0; subItemCounter < _COLUMNS; subItemCounter ++)
                        itemPair.ListViewItem.SubItems.Add(string.Empty);
                    _itemPairHashTable.Add(itemPair.QSetQueueItem.ID, itemPair);
                    
                    Action x = delegate { _monitorListView.Items.Add(itemPair.ListViewItem); };
                    _monitorListView.Invoke(x);
                }
                
                ManagementObject counter = null;
                try
                {										
                    counter = new ManagementObject(String.Format("Win32_PerfRawdata_MSMQ_MSMQQueue.name='{0}'", itemPair.QSetQueueItem.Name));
                    counter.Get();			
                    uint outgoingMessageCount = Convert.ToUInt32(counter.GetPropertyValue("MessagesInQueue"));
                    uint outgoingBytes = Convert.ToUInt32(counter.GetPropertyValue("BytesInQueue"));

                    Action herewegoagain = () =>
                        {
                            if (itemPair.ListViewItem.SubItems[(int)SubItemList.OutgoingMessageCount].Text != outgoingMessageCount.ToString()) //note: only do if necessary, to avoid flicker
                                itemPair.ListViewItem.SubItems[(int)SubItemList.OutgoingMessageCount].Text = outgoingMessageCount.ToString();

                            if (itemPair.ListViewItem.SubItems[(int)SubItemList.OutgoingBytes].Text != outgoingBytes.ToString()) //note: only do if necessary, to avoid flicker
                                itemPair.ListViewItem.SubItems[(int)SubItemList.OutgoingBytes].Text = outgoingBytes.ToString();
                        };


                    _monitorListView.Invoke(herewegoagain);
                }
                catch
                {
                    //exception will occur when cannot get access to performance counters
                }
                finally
                {
                    if (counter != null)
                        counter.Dispose();
                }
            }


            private void ChildItems_AfterItemAdded(object sender, AfterItemAddedEventArgs e)
            {
                QSetFolderItem folderItem = e.Item as QSetFolderItem;
                if (folderItem != null)
                {
                    folderItem.ChildItems.AfterItemAdded += new AfterItemAddedEvent(ChildItems_AfterItemAdded);
                    folderItem.ChildItems.BeforeItemRemoved += new BeforeItemRemovedEvent(ChildItems_BeforeItemRemoved);
                }
                    
            }


            private void ChildItems_BeforeItemRemoved(object sender, BeforeItemRemovedEventArgs e)
            {
                RecursivelyMarkListItemsForDeletion(e.Item);
            }


            /// <summary>
            /// Given a QSetItemBase, marks the corresponding QueueItemListViewItemPair for deletion,
            /// and repeats for any children of the QSetItemBase object.
            /// </summary>
            /// <param name="item">QSetItemBase to delete.</param>
            private void RecursivelyMarkListItemsForDeletion(QSetItemBase item)
            {
                //if we have a queue, remove it
                QSetQueueItem queueItem = item as QSetQueueItem;
                if (queueItem != null)
                {
                    if (_itemPairHashTable.ContainsKey(queueItem.ID))
                    {						
                        _deleteItemPairQueue.Enqueue(_itemPairHashTable[queueItem.ID]);
                    }
                }
                
                //if we have a folder, check children
                QSetFolderItem folderItem = item as QSetFolderItem;
                if (folderItem != null)
                    foreach (QSetItemBase childItem in folderItem.ChildItems)
                        RecursivelyMarkListItemsForDeletion(childItem);

            }

        }

        #endregion

    }
}
