using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Messaging;
using System.Threading;
using System.Windows.Forms;
using Mulholland.WinForms;
using Mulholland.QSet.Model;
using System.Collections.Generic;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for QueueSearchForm.
	/// </summary>
	internal class QueueSearchForm : System.Windows.Forms.Form
	{		
		private Size _defaultExpandedSize = new Size(536, 500);		
		private bool _rememberCreatedBeforeCheckState = false;
		private bool _rememberCreatedAfterCheckState = false;
		private bool _rememberModifiedBeforeCheckState = false;
		private bool _rememberModifiedAfterCheckState = false;		
		private int _computerImageIndex = -1;
		private int _queueImageIndex = -1;		
		private QueueSearchForm.QueueSearcher _queueSearcher = new QueueSearcher();
		private ProcessVisualizer _processVisualizer = null;
		private Hashtable _computerNodesHashTable = null;
		private QSetItemCollection _selectedItems = null;			
		private bool _allowMachineSelect = false;		
		private float _selectedTreeViewWidthPercentage = 50;

		private TreeViewEventHandler _resultsTreeViewAfterCheckHandler;

		private delegate void AddQueueTreeNodeDelegate(MessageQueue queue);
		private delegate void AddMachineTreeNodeDelegate(string machineName);
		private delegate void SetControlVisiblePropertyDelegate(Control control, bool visible);
		private event OKClickedEvent _okClicked;

		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label createdBeforeLabel;
		private System.Windows.Forms.Label createdAfterLabel;
		private System.Windows.Forms.Label categoryLabel;
		private System.Windows.Forms.Label labelLabel;
		private System.Windows.Forms.Label machineNameLabel;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.Button searchButton;
		private System.Windows.Forms.Label includeLabel1;
		private System.Windows.Forms.CheckBox categoryCheckBox;
		private System.Windows.Forms.CheckBox labelCheckBox;
		private System.Windows.Forms.CheckBox machineNameCheckBox;
		private System.Windows.Forms.Label modifiedAfterLabel;
		private System.Windows.Forms.DateTimePicker modifiedAfterDateTimePicker;
		private System.Windows.Forms.Label modifiedBeforeLabel;
		private System.Windows.Forms.DateTimePicker modifiedBeforeDateTimePicker;
		private System.Windows.Forms.DateTimePicker createdBeforeDateTimePicker;
		private System.Windows.Forms.DateTimePicker createdAfterDateTimePicker;
		private System.Windows.Forms.TextBox categoryTextBox;
		private System.Windows.Forms.TextBox labelTextBox;
		private System.Windows.Forms.TextBox machineNameTextBox;
		private System.Windows.Forms.GroupBox searchOptionsGroupBox;
		private System.Windows.Forms.StatusBar statusBar;
		private System.Windows.Forms.StatusBarPanel mainStatusBarPanel;
		private System.Windows.Forms.CheckBox privateQueuesCheckBox;
		private System.Windows.Forms.Label selectedLabel;
		private System.Windows.Forms.Label resultsLabel;
		private System.Windows.Forms.Panel resultsPanel;
		private System.Windows.Forms.TreeView resultsTreeView;
		private System.Windows.Forms.Splitter resultsSplitter;
		private System.Windows.Forms.ListView selectedListView;
		private System.Windows.Forms.CheckBox searchEntireNetworkCheckBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public QueueSearchForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			InitializeQueueSearcher();
			InitializeFormState();

			_resultsTreeViewAfterCheckHandler = new TreeViewEventHandler(resultsTreeView_AfterCheck);
			resultsTreeView.AfterCheck += _resultsTreeViewAfterCheckHandler;
		}

		#region events

		/// <summary/>
		public delegate void OKClickedEvent(object sender, OKClickedEventArgs e);

		/// <summary>
		/// Occurs when a queue is selected and the OK button is clicked.
		/// </summary>
		public event OKClickedEvent OKClicked
		{
			add
			{
				_okClicked += value;
			}
			remove
			{
				_okClicked -= value;
			}
		}


		/// <summary>
		/// Event arguments for OKClickedEvent event.
		/// </summary>
		public class OKClickedEventArgs : EventArgs
		{
			private QSetItemCollection _selectedItems;

			/// <summary>
			/// Constructs the arguments class, with a single selecte message queue.
			/// </summary>
			/// <param name="selectedItems">A collection of selected items.</param>
			public OKClickedEventArgs(QSetItemCollection selectedItems)
			{
				_selectedItems = selectedItems;
			}



			/// <summary>
			/// Gets a collection of selected items.
			/// </summary>
			public QSetItemCollection SelectedItems
			{
				get
				{
					return _selectedItems;
				}
			}
		}


		/// <summary>
		/// Event arguments used for events relating to message queues.
		/// </summary>
		public class MessageQueueSelectEventArgs : EventArgs
		{
			private MessageQueue _queue;

			/// <summary>
			/// Constructs the event arguments object.
			/// </summary>
			/// <param name="queue"></param>
			public MessageQueueSelectEventArgs(MessageQueue queue)
			{
				_queue = queue;
			}


			/// <summary>
			/// Gets the queue associated with the event.
			/// </summary>
			public MessageQueue Queue
			{
				get
				{
					return _queue;
				}
			}
		}


		/// <summary>
		/// Event argumens for MachineDoubleClicked event.
		/// </summary>
		public class MachineDoubleClickedEventArgs : EventArgs
		{
			private string _machineName;
			private MessageQueue[] _messageQueues;

			/// <summary>
			/// Constructs the event arguments class.
			/// </summary>
			/// <param name="machineName">Name of machine which was double clicked.</param>
			/// <param name="messageQueues">Messages belonging to the machine.</param>
			public MachineDoubleClickedEventArgs(string machineName, MessageQueue[] messageQueues)
			{
				_machineName = machineName;
				_messageQueues = messageQueues;
			}


			/// <summary>
			/// Name of machine which was double clicked.
			/// </summary>
			public string MachineName
			{
				get
				{
					return _machineName;
				}
			}


			/// <summary>
			/// Messages belonging to the machine.
			/// </summary>
			public MessageQueue[] MessageQueues
			{
				get
				{
					return _messageQueues;
				}
			}
		}


		/// <summary>
		/// Raises the OKClicked event.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		protected void OnOKClicked(OKClickedEventArgs e)
		{
			try
			{
				if (_okClicked != null)
					_okClicked(this, e);
			}
			catch {}
		}

		#endregion

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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueSearchForm));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.searchOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.searchEntireNetworkCheckBox = new System.Windows.Forms.CheckBox();
            this.privateQueuesCheckBox = new System.Windows.Forms.CheckBox();
            this.categoryCheckBox = new System.Windows.Forms.CheckBox();
            this.labelCheckBox = new System.Windows.Forms.CheckBox();
            this.machineNameCheckBox = new System.Windows.Forms.CheckBox();
            this.modifiedAfterLabel = new System.Windows.Forms.Label();
            this.modifiedAfterDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.modifiedBeforeLabel = new System.Windows.Forms.Label();
            this.modifiedBeforeDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.createdBeforeLabel = new System.Windows.Forms.Label();
            this.createdBeforeDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.createdAfterLabel = new System.Windows.Forms.Label();
            this.createdAfterDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.categoryTextBox = new System.Windows.Forms.TextBox();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.labelLabel = new System.Windows.Forms.Label();
            this.labelTextBox = new System.Windows.Forms.TextBox();
            this.machineNameLabel = new System.Windows.Forms.Label();
            this.machineNameTextBox = new System.Windows.Forms.TextBox();
            this.includeLabel1 = new System.Windows.Forms.Label();
            this.stopButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.statusBar = new System.Windows.Forms.StatusBar();
            this.mainStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
            this.selectedLabel = new System.Windows.Forms.Label();
            this.resultsLabel = new System.Windows.Forms.Label();
            this.resultsPanel = new System.Windows.Forms.Panel();
            this.resultsTreeView = new System.Windows.Forms.TreeView();
            this.resultsSplitter = new System.Windows.Forms.Splitter();
            this.selectedListView = new System.Windows.Forms.ListView();
            this.searchOptionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainStatusBarPanel)).BeginInit();
            this.resultsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(364, 409);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 6;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Location = new System.Drawing.Point(444, 409);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // searchOptionsGroupBox
            // 
            this.searchOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchOptionsGroupBox.Controls.Add(this.searchEntireNetworkCheckBox);
            this.searchOptionsGroupBox.Controls.Add(this.privateQueuesCheckBox);
            this.searchOptionsGroupBox.Controls.Add(this.categoryCheckBox);
            this.searchOptionsGroupBox.Controls.Add(this.labelCheckBox);
            this.searchOptionsGroupBox.Controls.Add(this.machineNameCheckBox);
            this.searchOptionsGroupBox.Controls.Add(this.modifiedAfterLabel);
            this.searchOptionsGroupBox.Controls.Add(this.modifiedAfterDateTimePicker);
            this.searchOptionsGroupBox.Controls.Add(this.modifiedBeforeLabel);
            this.searchOptionsGroupBox.Controls.Add(this.modifiedBeforeDateTimePicker);
            this.searchOptionsGroupBox.Controls.Add(this.createdBeforeLabel);
            this.searchOptionsGroupBox.Controls.Add(this.createdBeforeDateTimePicker);
            this.searchOptionsGroupBox.Controls.Add(this.createdAfterLabel);
            this.searchOptionsGroupBox.Controls.Add(this.createdAfterDateTimePicker);
            this.searchOptionsGroupBox.Controls.Add(this.categoryTextBox);
            this.searchOptionsGroupBox.Controls.Add(this.categoryLabel);
            this.searchOptionsGroupBox.Controls.Add(this.labelLabel);
            this.searchOptionsGroupBox.Controls.Add(this.labelTextBox);
            this.searchOptionsGroupBox.Controls.Add(this.machineNameLabel);
            this.searchOptionsGroupBox.Controls.Add(this.machineNameTextBox);
            this.searchOptionsGroupBox.Controls.Add(this.includeLabel1);
            this.searchOptionsGroupBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchOptionsGroupBox.Location = new System.Drawing.Point(8, 32);
            this.searchOptionsGroupBox.Name = "searchOptionsGroupBox";
            this.searchOptionsGroupBox.Size = new System.Drawing.Size(512, 148);
            this.searchOptionsGroupBox.TabIndex = 3;
            this.searchOptionsGroupBox.TabStop = false;
            this.searchOptionsGroupBox.Text = "Search Options";
            // 
            // searchEntireNetworkCheckBox
            // 
            this.searchEntireNetworkCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.searchEntireNetworkCheckBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchEntireNetworkCheckBox.Location = new System.Drawing.Point(92, 16);
            this.searchEntireNetworkCheckBox.Name = "searchEntireNetworkCheckBox";
            this.searchEntireNetworkCheckBox.Size = new System.Drawing.Size(136, 20);
            this.searchEntireNetworkCheckBox.TabIndex = 19;
            this.searchEntireNetworkCheckBox.Text = "Search &Entire Network?";
            this.searchEntireNetworkCheckBox.CheckedChanged += new System.EventHandler(this.searchEntireNetworkCheckBox_CheckedChanged);
            // 
            // privateQueuesCheckBox
            // 
            this.privateQueuesCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.privateQueuesCheckBox.Location = new System.Drawing.Point(92, 68);
            this.privateQueuesCheckBox.Name = "privateQueuesCheckBox";
            this.privateQueuesCheckBox.Size = new System.Drawing.Size(144, 16);
            this.privateQueuesCheckBox.TabIndex = 4;
            this.privateQueuesCheckBox.Text = "Private Queues Only?";
            this.privateQueuesCheckBox.CheckedChanged += new System.EventHandler(this.privateQueuesCheckBox_CheckedChanged);
            // 
            // categoryCheckBox
            // 
            this.categoryCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.categoryCheckBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryCheckBox.Location = new System.Drawing.Point(264, 112);
            this.categoryCheckBox.Name = "categoryCheckBox";
            this.categoryCheckBox.Size = new System.Drawing.Size(16, 21);
            this.categoryCheckBox.TabIndex = 10;
            this.categoryCheckBox.CheckedChanged += new System.EventHandler(this.searchCheckBox_CheckedChanged);
            // 
            // labelCheckBox
            // 
            this.labelCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelCheckBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCheckBox.Location = new System.Drawing.Point(264, 88);
            this.labelCheckBox.Name = "labelCheckBox";
            this.labelCheckBox.Size = new System.Drawing.Size(16, 21);
            this.labelCheckBox.TabIndex = 7;
            this.labelCheckBox.CheckedChanged += new System.EventHandler(this.searchCheckBox_CheckedChanged);
            // 
            // machineNameCheckBox
            // 
            this.machineNameCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.machineNameCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.machineNameCheckBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineNameCheckBox.Location = new System.Drawing.Point(264, 40);
            this.machineNameCheckBox.Name = "machineNameCheckBox";
            this.machineNameCheckBox.Size = new System.Drawing.Size(16, 21);
            this.machineNameCheckBox.TabIndex = 3;
            this.machineNameCheckBox.CheckedChanged += new System.EventHandler(this.searchCheckBox_CheckedChanged);
            // 
            // modifiedAfterLabel
            // 
            this.modifiedAfterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modifiedAfterLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifiedAfterLabel.Location = new System.Drawing.Point(296, 116);
            this.modifiedAfterLabel.Name = "modifiedAfterLabel";
            this.modifiedAfterLabel.Size = new System.Drawing.Size(88, 16);
            this.modifiedAfterLabel.TabIndex = 17;
            this.modifiedAfterLabel.Text = "Modified After:";
            // 
            // modifiedAfterDateTimePicker
            // 
            this.modifiedAfterDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modifiedAfterDateTimePicker.CalendarFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifiedAfterDateTimePicker.Checked = false;
            this.modifiedAfterDateTimePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifiedAfterDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.modifiedAfterDateTimePicker.Location = new System.Drawing.Point(384, 112);
            this.modifiedAfterDateTimePicker.Name = "modifiedAfterDateTimePicker";
            this.modifiedAfterDateTimePicker.ShowCheckBox = true;
            this.modifiedAfterDateTimePicker.Size = new System.Drawing.Size(120, 21);
            this.modifiedAfterDateTimePicker.TabIndex = 18;
            this.modifiedAfterDateTimePicker.Value = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            // 
            // modifiedBeforeLabel
            // 
            this.modifiedBeforeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modifiedBeforeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifiedBeforeLabel.Location = new System.Drawing.Point(296, 92);
            this.modifiedBeforeLabel.Name = "modifiedBeforeLabel";
            this.modifiedBeforeLabel.Size = new System.Drawing.Size(88, 16);
            this.modifiedBeforeLabel.TabIndex = 15;
            this.modifiedBeforeLabel.Text = "&Modified Before:";
            // 
            // modifiedBeforeDateTimePicker
            // 
            this.modifiedBeforeDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.modifiedBeforeDateTimePicker.CalendarFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifiedBeforeDateTimePicker.Checked = false;
            this.modifiedBeforeDateTimePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modifiedBeforeDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.modifiedBeforeDateTimePicker.Location = new System.Drawing.Point(384, 88);
            this.modifiedBeforeDateTimePicker.Name = "modifiedBeforeDateTimePicker";
            this.modifiedBeforeDateTimePicker.ShowCheckBox = true;
            this.modifiedBeforeDateTimePicker.Size = new System.Drawing.Size(120, 21);
            this.modifiedBeforeDateTimePicker.TabIndex = 16;
            this.modifiedBeforeDateTimePicker.Value = new System.DateTime(2004, 10, 19, 16, 29, 51, 146);
            // 
            // createdBeforeLabel
            // 
            this.createdBeforeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createdBeforeLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdBeforeLabel.Location = new System.Drawing.Point(296, 44);
            this.createdBeforeLabel.Name = "createdBeforeLabel";
            this.createdBeforeLabel.Size = new System.Drawing.Size(84, 16);
            this.createdBeforeLabel.TabIndex = 11;
            this.createdBeforeLabel.Text = "&Created Before:";
            // 
            // createdBeforeDateTimePicker
            // 
            this.createdBeforeDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createdBeforeDateTimePicker.CalendarFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdBeforeDateTimePicker.Checked = false;
            this.createdBeforeDateTimePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdBeforeDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.createdBeforeDateTimePicker.Location = new System.Drawing.Point(384, 40);
            this.createdBeforeDateTimePicker.Name = "createdBeforeDateTimePicker";
            this.createdBeforeDateTimePicker.ShowCheckBox = true;
            this.createdBeforeDateTimePicker.Size = new System.Drawing.Size(120, 21);
            this.createdBeforeDateTimePicker.TabIndex = 12;
            this.createdBeforeDateTimePicker.Value = new System.DateTime(2004, 10, 19, 16, 29, 51, 197);
            // 
            // createdAfterLabel
            // 
            this.createdAfterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createdAfterLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdAfterLabel.Location = new System.Drawing.Point(296, 68);
            this.createdAfterLabel.Name = "createdAfterLabel";
            this.createdAfterLabel.Size = new System.Drawing.Size(84, 16);
            this.createdAfterLabel.TabIndex = 13;
            this.createdAfterLabel.Text = "Created After:";
            // 
            // createdAfterDateTimePicker
            // 
            this.createdAfterDateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.createdAfterDateTimePicker.CalendarFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdAfterDateTimePicker.Checked = false;
            this.createdAfterDateTimePicker.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createdAfterDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.createdAfterDateTimePicker.Location = new System.Drawing.Point(384, 64);
            this.createdAfterDateTimePicker.Name = "createdAfterDateTimePicker";
            this.createdAfterDateTimePicker.ShowCheckBox = true;
            this.createdAfterDateTimePicker.Size = new System.Drawing.Size(120, 21);
            this.createdAfterDateTimePicker.TabIndex = 14;
            this.createdAfterDateTimePicker.Value = new System.DateTime(1980, 1, 1, 0, 0, 0, 0);
            // 
            // categoryTextBox
            // 
            this.categoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.categoryTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryTextBox.Location = new System.Drawing.Point(92, 112);
            this.categoryTextBox.Name = "categoryTextBox";
            this.categoryTextBox.Size = new System.Drawing.Size(168, 21);
            this.categoryTextBox.TabIndex = 9;
            this.categoryTextBox.TextChanged += new System.EventHandler(this.searchEntireNetworkCheckBox_CheckedChanged);
            this.categoryTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            // 
            // categoryLabel
            // 
            this.categoryLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryLabel.Location = new System.Drawing.Point(8, 116);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(84, 16);
            this.categoryLabel.TabIndex = 8;
            this.categoryLabel.Text = "C&ategory:";
            // 
            // labelLabel
            // 
            this.labelLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLabel.Location = new System.Drawing.Point(8, 92);
            this.labelLabel.Name = "labelLabel";
            this.labelLabel.Size = new System.Drawing.Size(84, 16);
            this.labelLabel.TabIndex = 5;
            this.labelLabel.Text = "&Label:";
            // 
            // labelTextBox
            // 
            this.labelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTextBox.Location = new System.Drawing.Point(92, 88);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(168, 21);
            this.labelTextBox.TabIndex = 6;
            this.labelTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            // 
            // machineNameLabel
            // 
            this.machineNameLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineNameLabel.Location = new System.Drawing.Point(8, 44);
            this.machineNameLabel.Name = "machineNameLabel";
            this.machineNameLabel.Size = new System.Drawing.Size(84, 16);
            this.machineNameLabel.TabIndex = 0;
            this.machineNameLabel.Text = "Machine &Name:";
            // 
            // machineNameTextBox
            // 
            this.machineNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.machineNameTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.machineNameTextBox.Location = new System.Drawing.Point(92, 40);
            this.machineNameTextBox.Name = "machineNameTextBox";
            this.machineNameTextBox.Size = new System.Drawing.Size(168, 21);
            this.machineNameTextBox.TabIndex = 1;
            this.machineNameTextBox.Text = ".";
            this.machineNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            // 
            // includeLabel1
            // 
            this.includeLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.includeLabel1.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.includeLabel1.Location = new System.Drawing.Point(252, 24);
            this.includeLabel1.Name = "includeLabel1";
            this.includeLabel1.Size = new System.Drawing.Size(40, 16);
            this.includeLabel1.TabIndex = 2;
            this.includeLabel1.Text = "Include?";
            // 
            // stopButton
            // 
            this.stopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.stopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.stopButton.Location = new System.Drawing.Point(444, 8);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 23);
            this.stopButton.TabIndex = 2;
            this.stopButton.Text = "S&top";
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.searchButton.Location = new System.Drawing.Point(364, 8);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 23);
            this.searchButton.TabIndex = 1;
            this.searchButton.Text = "&Search";
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 440);
            this.statusBar.Name = "statusBar";
            this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.mainStatusBarPanel});
            this.statusBar.ShowPanels = true;
            this.statusBar.Size = new System.Drawing.Size(528, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 8;
            // 
            // mainStatusBarPanel
            // 
            this.mainStatusBarPanel.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.mainStatusBarPanel.Name = "mainStatusBarPanel";
            this.mainStatusBarPanel.Text = "Ready";
            this.mainStatusBarPanel.Width = 528;
            // 
            // selectedLabel
            // 
            this.selectedLabel.Location = new System.Drawing.Point(268, 188);
            this.selectedLabel.Name = "selectedLabel";
            this.selectedLabel.Size = new System.Drawing.Size(100, 16);
            this.selectedLabel.TabIndex = 11;
            this.selectedLabel.Text = "Selected:";
            // 
            // resultsLabel
            // 
            this.resultsLabel.Location = new System.Drawing.Point(8, 188);
            this.resultsLabel.Name = "resultsLabel";
            this.resultsLabel.Size = new System.Drawing.Size(100, 16);
            this.resultsLabel.TabIndex = 10;
            this.resultsLabel.Text = "Results:";
            // 
            // resultsPanel
            // 
            this.resultsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.resultsPanel.Controls.Add(this.resultsTreeView);
            this.resultsPanel.Controls.Add(this.resultsSplitter);
            this.resultsPanel.Controls.Add(this.selectedListView);
            this.resultsPanel.Location = new System.Drawing.Point(8, 204);
            this.resultsPanel.Name = "resultsPanel";
            this.resultsPanel.Size = new System.Drawing.Size(512, 200);
            this.resultsPanel.TabIndex = 9;
            this.resultsPanel.Resize += new System.EventHandler(this.resultsPanel_Resize);
            // 
            // resultsTreeView
            // 
            this.resultsTreeView.CheckBoxes = true;
            this.resultsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsTreeView.HideSelection = false;
            this.resultsTreeView.Location = new System.Drawing.Point(0, 0);
            this.resultsTreeView.Name = "resultsTreeView";
            this.resultsTreeView.Size = new System.Drawing.Size(256, 200);
            this.resultsTreeView.Sorted = true;
            this.resultsTreeView.TabIndex = 4;
            this.resultsTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.resultsTreeView_AfterSelect);
            // 
            // resultsSplitter
            // 
            this.resultsSplitter.Dock = System.Windows.Forms.DockStyle.Right;
            this.resultsSplitter.Location = new System.Drawing.Point(256, 0);
            this.resultsSplitter.Name = "resultsSplitter";
            this.resultsSplitter.Size = new System.Drawing.Size(4, 200);
            this.resultsSplitter.TabIndex = 1;
            this.resultsSplitter.TabStop = false;
            this.resultsSplitter.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.resultsSplitter_SplitterMoved);
            // 
            // selectedListView
            // 
            this.selectedListView.Dock = System.Windows.Forms.DockStyle.Right;
            this.selectedListView.HideSelection = false;
            this.selectedListView.Location = new System.Drawing.Point(260, 0);
            this.selectedListView.Name = "selectedListView";
            this.selectedListView.Size = new System.Drawing.Size(252, 200);
            this.selectedListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.selectedListView.TabIndex = 0;
            this.selectedListView.UseCompatibleStateImageBehavior = false;
            this.selectedListView.View = System.Windows.Forms.View.List;
            this.selectedListView.SelectedIndexChanged += new System.EventHandler(this.selectedListView_SelectedIndexChanged);
            this.selectedListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.selectedListView_KeyUp);
            this.selectedListView.Resize += new System.EventHandler(this.selectedListView_Resize);
            // 
            // QueueSearchForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(528, 462);
            this.Controls.Add(this.selectedLabel);
            this.Controls.Add(this.resultsLabel);
            this.Controls.Add(this.resultsPanel);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.searchOptionsGroupBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.searchButton);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(536, 272);
            this.Name = "QueueSearchForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Queue Search";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.QueueSearchForm_Closing);
            this.Load += new System.EventHandler(this.QueueSearchForm_Load);
            this.searchOptionsGroupBox.ResumeLayout(false);
            this.searchOptionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainStatusBarPanel)).EndInit();
            this.resultsPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Shows the form as a modal dialog box with no owner window.
		/// </summary>
		/// <returns>One of the <see cref="DialogResult"/> values.</returns>
		public new DialogResult ShowDialog()
		{			
			return this.ShowDialog(null);
		}


		/// <summary>
		/// Shows the form as a modal dialog with the specified owner.
		/// </summary>
		/// <param name="owner">Any object that implements <see cref="IWin32Window"/> that represents the top-level window that will own the modal dialog.</param>
		/// <returns>One of the <see cref="DialogResult"/> values.</returns>
		public new DialogResult ShowDialog(IWin32Window owner)		
		{
			DialogResult result; 
			
			//initial setup			
			this.ShowInTaskbar = false;
			this.AcceptButton = okButton;
			RecallDateTimePickerCheckStates();			
			_selectedItems = null;

			//show form
			result = base.ShowDialog(owner);

			if (_selectedItems != null && _selectedItems.Count > 0)
				result = DialogResult.OK;
		
			//close down tasks
			StoreDateTimePickerCheckStates();

			return result;
		}


		/// <summary>
		/// Shows the dialog.
		/// </summary>
		public new void Show()
		{
			//initial setup
			this.ShowInTaskbar = true;
			this.AcceptButton = searchButton;
			_selectedItems = null;

			//show form
			base.Show();
		}


		/// <summary>
		/// Gets or sets the <see cref="System.Windows.Forms.ImageList"/> associated with the control.
		/// </summary>
		public ImageList ImageList
		{
			get
			{
				return resultsTreeView.ImageList;
			}
			set
			{
				resultsTreeView.ImageList = value;
				selectedListView.SmallImageList = value;
			}
		}


		/// <summary>
		/// Gets or sets the index of the image used to display a computer in the results tree view. 		
		/// </summary>
		public int ComputerImageIndex
		{
			get
			{
				return _computerImageIndex;
			}
			set
			{
				_computerImageIndex = value;
			}
		}


		/// <summary>
		/// Gets or sets the index of the image used to display a queue in the results tree view. 		
		/// </summary>
		public int QueueImageIndex
		{
			get
			{
				return _queueImageIndex;
			}
			set
			{
				_queueImageIndex = value;
			}
		}


		/// <summary>
		/// Gets the selected items.
		/// </summary>
		public QSetItemCollection SelectedItems
		{
			get
			{
				return _selectedItems;
			}
		}


		/// <summary>
		/// Gets or sets flag which indicates whether a machine/computer may be selected.
		/// </summary>
		public bool AllowMachineSelect
		{
			get
			{
				return _allowMachineSelect;
			}
			set
			{
				_allowMachineSelect = value;
			}
		}


		/// <summary>
		/// Handles check change of search entire network box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchEntireNetworkCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{			
			ConfigureSearchOptions();
		}

		
		/// <summary>
		/// Stores the state of the date time controls.
		/// </summary>
		private void StoreDateTimePickerCheckStates()
		{
			_rememberCreatedBeforeCheckState = createdBeforeDateTimePicker.Checked;
			_rememberCreatedAfterCheckState = createdAfterDateTimePicker.Checked;
			_rememberModifiedBeforeCheckState = modifiedBeforeDateTimePicker.Checked;
			_rememberModifiedAfterCheckState = modifiedAfterDateTimePicker.Checked;
		}


		/// <summary>
		/// Recalss the state of the date time controls.  
		/// </summary>
		/// <remarks>
		/// This is recquired as the controls were overriding there design time/initialize code settings.		
		/// </remarks>
		private void RecallDateTimePickerCheckStates()
		{
			createdBeforeDateTimePicker.Checked = _rememberCreatedBeforeCheckState;
			createdAfterDateTimePicker.Checked = _rememberCreatedAfterCheckState;
			modifiedBeforeDateTimePicker.Checked = _rememberModifiedBeforeCheckState;
			modifiedAfterDateTimePicker.Checked = _rememberModifiedAfterCheckState;
		}


		/// <summary>
		/// Configures event handlers for the search process.
		/// </summary>
		private void InitializeQueueSearcher()
		{
			_processVisualizer = new ProcessVisualizer(this);
			_processVisualizer.StatusBarPanel = this.mainStatusBarPanel;
			_processVisualizer.AwaitingText = "Ready";

			_queueSearcher.SearchStarted += new VisualizableProcessEvent(_queueSearcher_SearchStarted);
			_queueSearcher.MessageQueueFound += new QueueSearchForm.QueueSearcher.MessageQueueFoundEvent(_queueSearcher_MessageQueueFound);
			_queueSearcher.SearchException += new QueueSearchForm.QueueSearcher.SearchExceptionEvent(_queueSearcher_SearchException);
			_queueSearcher.SearchFinished += new QueueSearchForm.QueueSearcher.SearchFinishedEvent(_queueSearcher_SearchFinished);
			_queueSearcher.HostMachineFound += new Mulholland.QSet.Application.QueueSearchForm.QueueSearcher.HostMachineFoundEvent(_queueSearcher_HostMachineFound);
		}


		/// <summary>
		/// Sets the initial state of the form.
		/// </summary>
		private void InitializeFormState()
		{
			//shrink to pre search size
			this.Size = this.MinimumSize;			
			this.MaximumSize = this.MinimumSize;
			resultsLabel.Visible = false;
			selectedLabel.Visible = false;
			resultsPanel.Visible = false;
			
			//link search options and include check boxes
			machineNameTextBox.Tag = machineNameCheckBox;
			labelTextBox.Tag = labelCheckBox;
			categoryTextBox.Tag = categoryCheckBox;			
			machineNameCheckBox.Tag = machineNameTextBox;
			labelCheckBox.Tag = labelTextBox;
			categoryCheckBox.Tag = categoryTextBox;			

			//default values
			createdBeforeDateTimePicker.Value = DateTime.Now.AddDays(1);
			modifiedBeforeDateTimePicker.Value = DateTime.Now.AddDays(1);
			RecallDateTimePickerCheckStates();

			//enable/disable search options
			searchEntireNetworkCheckBox.Checked = true;
			ConfigureSearchOptions();
			ConfigureButtonStates();
		}


		/// <summary>
		/// Configures the search options group box.
		/// </summary>
		private void ConfigureSearchOptions()
		{			
			foreach (Control control in searchOptionsGroupBox.Controls)
			{
				if (control != searchEntireNetworkCheckBox)
					control.Enabled = 
						(
						!searchEntireNetworkCheckBox.Checked 
						&&	(
							(control == machineNameLabel || control == machineNameTextBox || control == privateQueuesCheckBox) 
							|| 
							!privateQueuesCheckBox.Checked
							)
						);				
				}

			if (!searchEntireNetworkCheckBox.Checked && privateQueuesCheckBox.Checked)
				machineNameCheckBox.Checked = true;
		}


		/// <summary>
		/// Sets the text colour of a search control according to its current state.
		/// </summary>
		/// <param name="textBox">Search option TextBox.</param>
		/// <param name="checkBox">Search option CheckBox.</param>
		/// <param name="latestEditedControl">The latest control which was edited by the user.</param>
		private void ConfigureSearchOption(TextBox textBox, CheckBox checkBox, Control latestEditedControl)
		{
			if (latestEditedControl == textBox)
				checkBox.Checked = textBox.Text.Trim().Length > 0;				
		
			textBox.ForeColor = checkBox.Checked ? SystemColors.ControlText :  SystemColors.ControlDark;
		}


		/// <summary>
		/// Handles search text box keypress.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (sender is TextBox && ((TextBox)sender).Tag != null && ((TextBox)sender).Tag is CheckBox)
				ConfigureSearchOption((TextBox)sender, (CheckBox)((TextBox)sender).Tag, (TextBox)sender);					
		}


		/// <summary>
		/// Handles search include check box change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			if (sender is CheckBox && ((CheckBox)sender).Tag != null && ((CheckBox)sender).Tag is TextBox)
				ConfigureSearchOption((TextBox)((CheckBox)sender).Tag, (CheckBox)sender, (CheckBox)sender);					
		}
		

		/// <summary>
		/// Configures the state of all buttons on the form according to what is currently happening.
		/// </summary>
		private void ConfigureButtonStates()
		{			
			stopButton.Enabled = _queueSearcher.IsSearchInProgress;
			searchButton.Enabled = !_queueSearcher.IsSearchInProgress;
			okButton.Enabled = (resultsTreeView.SelectedNode != null 
								&& resultsTreeView.SelectedNode.Tag != null 
								&& resultsTreeView.SelectedNode.Tag is MessageQueue || _allowMachineSelect);			
		}


		/// <summary>
		/// Validates the search criteria.
		/// </summary>
		/// <returns>true of the criteria is valid, else false.</returns>
		private bool ValidateSearchCriteria()
		{
			bool valid = true;			

			if (categoryCheckBox.Enabled && categoryCheckBox.Checked)
			{				
				try
				{
					Guid validateGuid = new Guid(categoryTextBox.Text);
				}
				catch 
				{
					valid = false;
					MessageBox.Show(this, "Category must be in GUID format.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
					categoryTextBox.Focus();
					categoryTextBox.SelectAll();				
				}
			}

			return valid;
		}


		/// <summary>
		/// Starts the search.
		/// </summary>
		private void StartSearch()
		{
			try
			{
				//validate the search options
				if (ValidateSearchCriteria())
				{
					_computerNodesHashTable = new Hashtable();
					resultsTreeView.Nodes.Clear();
					selectedListView.Items.Clear();

					//start the search according to the search options
					if (searchEntireNetworkCheckBox.Checked)
						//global search
						_queueSearcher.StartSearch();
					else if (privateQueuesCheckBox.Checked)
						//search machine for private queues
						_queueSearcher.StartSearch(machineNameTextBox.Text);
					else
					{
						//search according to criteria
						MessageQueueCriteria criteria = new MessageQueueCriteria();
						if (machineNameCheckBox.Checked && machineNameTextBox.Text.Length > 0)
							criteria.MachineName = machineNameTextBox.Text;
						if (labelCheckBox.Checked && labelTextBox.Text.Length > 0)
							criteria.Label = labelTextBox.Text;
						if (categoryCheckBox.Checked && categoryTextBox.Text.Length > 0)
							criteria.Category = new Guid(categoryTextBox.Text);
						if (createdBeforeDateTimePicker.Checked)
							criteria.CreatedBefore = createdBeforeDateTimePicker.Value;
						if (createdAfterDateTimePicker.Checked)
							criteria.CreatedAfter = createdAfterDateTimePicker.Value;
						if (modifiedBeforeDateTimePicker.Checked)
							criteria.ModifiedBefore = modifiedBeforeDateTimePicker.Value;
						if (modifiedAfterDateTimePicker.Checked)
							criteria.ModifiedAfter = modifiedAfterDateTimePicker.Value;

						_queueSearcher.StartSearch(criteria);				
					}					
				}
			}
			catch (Exception exc)
			{
				DisplayException(exc);
			}
		}


		/// <summary>
		/// Handles the search button click, and starts the search
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void searchButton_Click(object sender, System.EventArgs e)
		{			
			StartSearch();
		}

        private delegate void Action();

		/// <summary>
		/// Handles the search started event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _queueSearcher_SearchStarted(object sender, VisualizableProcessEventArgs e)
		{
            Action x = () =>
                {
                    ConfigureButtonStates();
                    if (Size == MinimumSize)
                    {
                        MaximumSize = new Size(0, 0);
                        MinimumSize = new Size(536, 308);
                        Size = _defaultExpandedSize;
                        MaximizeBox = true;
                    }
                    resultsLabel.Invoke(new SetControlVisiblePropertyDelegate(SetControlVisibleProperty), new object[] { resultsLabel, true });
                    selectedLabel.Invoke(new SetControlVisiblePropertyDelegate(SetControlVisibleProperty), new object[] { selectedLabel, true });
                    resultsPanel.Invoke(new SetControlVisiblePropertyDelegate(SetControlVisibleProperty), new object[] { resultsPanel, true });

                    _processVisualizer.ProcessStarting(e.Process);
                };

            this.Invoke(x);
		}


		/// <summary>
		/// Handles the message queue found event of the search, adding the queue into the results tree.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _queueSearcher_MessageQueueFound(object sender, QueueSearchForm.QueueSearcher.MessageQueueFoundEventArgs e)
		{
			if (resultsTreeView.InvokeRequired)
				resultsTreeView.Invoke(new AddQueueTreeNodeDelegate(AddQueueTreeNode), new object[] {e.MessageQueue});
			else
				AddQueueTreeNode(e.MessageQueue);				
		}


		/// <summary>
		/// Handles the host machine found event of the search, adding it to.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _queueSearcher_HostMachineFound(object sender, Mulholland.QSet.Application.QueueSearchForm.QueueSearcher.HostMachineFoundEventArgs e)
		{
			if (resultsTreeView.InvokeRequired)
				resultsTreeView.Invoke(new AddMachineTreeNodeDelegate(AddMachineNode), new object[] {e.MachineName});
			else
				AddMachineNode(e.MachineName);
		}


		/// <summary>
		/// Sets the Visible property of a control.
		/// </summary>
		/// <param name="control"></param>
		/// <param name="visible"></param>
		private void SetControlVisibleProperty(Control control, bool visible)
		{
			control.Visible = visible;
		}


		/// <summary>
		/// Adds a queue node to the specified parent.
		/// </summary>		
		/// <param name="queue">The queu to add to the tree view.</param>
		private void AddQueueTreeNode(MessageQueue queue)
		{						
			//add a queue node to the computer node
			ResultTreeNode queueNode = null;
			string queueName = string.Format(@"{0}\{1}", queue.MachineName, queue.QueueName); //reformat as private queues can come out with extra data in name
			if (resultsTreeView.ImageList != null)
				queueNode = new ResultTreeNode(queueName, _queueImageIndex, _queueImageIndex);				
			else
				queueNode = new ResultTreeNode(queueName);
			queueNode.Tag = queue;
			((ResultTreeNode)_computerNodesHashTable[queue.MachineName]).Nodes.Add(queueNode);
		}	
	

		/// <summary>
		/// Adds a machine node to the results tree view.
		/// </summary>
		/// <param name="machineName"></param>
		private void AddMachineNode(string machineName)
		{
			TreeNode machineNode;
			if (resultsTreeView.ImageList != null)
				machineNode = new ResultTreeNode(machineName, _computerImageIndex, _computerImageIndex);
			else
				machineNode = new ResultTreeNode(machineName);
			resultsTreeView.Nodes.Add(machineNode);
			_computerNodesHashTable.Add(machineName, machineNode);						
		}


		/// <summary>
		/// Handles event raised if an excpetion occurs during the search.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _queueSearcher_SearchException(object sender, QueueSearchForm.QueueSearcher.SearchExceptionEventArgs e)
		{
			//DisplayException(e.Exception);
		}


		/// <summary>
		/// Displays an exception to the user.
		/// </summary>
		/// <param name="exc">Exception to display.</param>
		private void DisplayException(Exception exc)
		{
            Action x = () =>
                {
                    _processVisualizer.SeizeCursor(Cursors.Arrow);
                    MessageBox.Show(this, exc.Message + "\n\nWorkgroup computers may only search for private queues on the local machine.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _processVisualizer.ReleaseCursor();
                };

            this.Invoke(x);
		}


		/// <summary>
		/// Handles the search finished event of the search.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _queueSearcher_SearchFinished(object sender, QueueSearchForm.QueueSearcher.SearchFinishedEventArgs e)
		{
            Action x = () =>
                {
                    ConfigureButtonStates();
                    _processVisualizer.ProcessCompleted(e.Process);
                };

            this.Invoke(x);
		}


		/// <summary>
		/// Handles the stop button click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void stopButton_Click(object sender, System.EventArgs e)
		{
			_queueSearcher.Abort();
		}


		/// <summary>
		/// Handles the cancel button click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			if (_queueSearcher.IsSearchInProgress)
				_queueSearcher.Abort();
			if (!this.Modal)
				this.Hide();
		}


		/// <summary>
		/// Handles node selection change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void resultsTreeView_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			ConfigureButtonStates();
		}


		/// <summary>
		/// Handles the OK button click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, System.EventArgs e)
		{						
			_selectedItems = new QSetItemCollection();
			foreach (ListViewItem selectedItem in selectedListView.Items)
			{
				//create a machine or queue item
				TreeNode resultNode = ((TreeNode)selectedItem.Tag);
				QSetItemBase newQSetItem;					
				if (resultNode.Parent == null)
				{
					newQSetItem = new QSetMachineItem(resultNode.Text);
					foreach (TreeNode childNode in resultNode.Nodes)
					{
						((QSetMachineItem)newQSetItem).ChildItems.Add(new QSetQueueItem(childNode.Text));
					}
				}
				else
					newQSetItem = new QSetQueueItem(resultNode.Text);					

				//add the item to the selection
				_selectedItems.Add(newQSetItem);
			}

			//raise ok click event
			OnOKClicked(new OKClickedEventArgs(_selectedItems));			

			if (!Modal)
				Hide();
		}


		/// <summary>
		/// Handles tree view lost focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void resultsTreeView_Leave(object sender, System.EventArgs e)
		{
			if (!this.Modal)
				this.AcceptButton = searchButton;
		}


		/// <summary>
		/// Handles tree view got focus.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void resultsTreeView_Enter(object sender, System.EventArgs e)
		{
			this.AcceptButton = okButton;
		}


		/// <summary>
		/// Handles private queues check change.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void privateQueuesCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			ConfigureSearchOptions();
		}


		/// <summary>
		/// Handles the form closing event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void QueueSearchForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			e.Cancel = true;
			this.Hide();
		}


		/// <summary>
		/// Handles the stay op on click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void stayOnTopCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			ConfigureButtonStates();
		}

		
		/// <summary>
		/// Adds a tree node to the selected list view.
		/// </summary>
		/// <param name="node"></param>
		private void AddNodeToSelection(TreeNode node)
		{
			ListViewItem newSelectedItem = new ListViewItem(node.Text, node.ImageIndex);
			newSelectedItem.Tag = node;
			((ResultTreeNode)node).SelectedItem = newSelectedItem;
			selectedListView.Items.Add(newSelectedItem);
		}


		/// <summary>
		/// Removes a selectd list item entry, which corresponds to a tree node.
		/// </summary>
		/// <param name="node"></param>
		private void RemoveNodeFromSelection(TreeNode node)
		{
			selectedListView.Items.Remove(((ResultTreeNode)node).SelectedItem);			
			((ResultTreeNode)node).SelectedItem = null;
		}


		/// <summary>
		/// Handles a 'check' of a result tree node.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void resultsTreeView_AfterCheck(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			resultsTreeView.AfterCheck -= _resultsTreeViewAfterCheckHandler;
			
			//update the selection selection.
			if (e.Node.Checked)
				AddNodeToSelection(e.Node);
			else
				RemoveNodeFromSelection(e.Node);
			
			//update the node selction according to what was selected
			if (e.Node.Parent == null)
			{
				//machine was clicked
				foreach (TreeNode child in e.Node.Nodes)
				{
					child.ForeColor = e.Node.Checked ? SystemColors.GrayText : SystemColors.WindowText;
					child.Checked = e.Node.Checked;
					if (e.Node.Checked && child.Tag != null)
						RemoveNodeFromSelection(child);
				}
			}
			else
			{
				//queue was clicked
				if (e.Node.Parent.Checked)
				{
					e.Node.Parent.Checked = false;
					RemoveNodeFromSelection(e.Node.Parent);
					foreach (TreeNode sibling in e.Node.Parent.Nodes)
					{
						sibling.ForeColor = SystemColors.WindowText;
						if (sibling != e.Node)
							AddNodeToSelection(sibling);
					}					
				}
			}

			resultsTreeView.AfterCheck += _resultsTreeViewAfterCheckHandler;
		}


		private void resultsPanel_Resize(object sender, System.EventArgs e)
		{
			selectedListView.Width = (int)(((float)resultsPanel.Width / (float)100) * _selectedTreeViewWidthPercentage);
		}


		private void UpdateSelectedTreeViewWidthPercentage()
		{
			_selectedTreeViewWidthPercentage = ((float)100 / (float)resultsPanel.Width) * selectedListView.Width;
		}


		private void resultsSplitter_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			UpdateSelectedTreeViewWidthPercentage();
		}


		private void selectedListView_Resize(object sender, System.EventArgs e)
		{
			SetSelectedLabelPosition();
		}


		private void SetSelectedLabelPosition()
		{
			selectedLabel.Left = resultsPanel.Left + selectedListView.Left;
		}


		private void selectedListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (selectedListView.FocusedItem != null)
				resultsTreeView.SelectedNode = (TreeNode)selectedListView.FocusedItem.Tag;			
		}


		private void selectedListView_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
				foreach (ListViewItem selectedItem in selectedListView.SelectedItems)
					((TreeNode)selectedItem.Tag).Checked = false;
		}

		private void QueueSearchForm_Load(object sender, System.EventArgs e)
		{
		
		}

		#region private class QueueSearcher

		/// <summary>
		/// Performs message queue searches.
		/// </summary>
		private class QueueSearcher
		{
			private event VisualizableProcessEvent _searchStarted;
			private event QueueSearcher.SearchFinishedEvent _searchFinished;
			private event QueueSearcher.MessageQueueFoundEvent _messageQueueFound;
			private event QueueSearcher.SearchExceptionEvent _searchException;
			private event QueueSearcher.HostMachineFoundEvent _machineFound;
			private Hashtable _computerNodesHashTable = null;

			private bool _abortRequested = false;
			private bool _isSearchInProgress = false;

			/// <summary>
			/// Default constructor.
			/// </summary>
			public QueueSearcher() {}

			#region events

			/// <summary>
			/// SearchFinished event delegate.
			/// </summary>
			public delegate void SearchFinishedEvent(object sender, SearchFinishedEventArgs e);


			/// <summary>
			/// MessageQueueFound event delegate.
			/// </summary>
			public delegate void MessageQueueFoundEvent(object sender, MessageQueueFoundEventArgs e);


			/// <summary>
			/// SearchException event delegate.
			/// </summary>
			public delegate void SearchExceptionEvent(object sender, SearchExceptionEventArgs e);


			/// <summary>
			/// HostMachineFound event delegate.
			/// </summary>
			public delegate void HostMachineFoundEvent(object sender, HostMachineFoundEventArgs e);


			#region public class SearchFinishedEventArgs : VisualizableProcessEventArgs

			/// <summary>
			/// SearchFinished event arguments.
			/// </summary>
			public class SearchFinishedEventArgs : VisualizableProcessEventArgs
			{
				bool _aborted;

				/// <summary>
				/// Constructs event arguments object.
				/// </summary>
				/// <param name="process"></param>
				/// <param name="aborted"></param>
				public SearchFinishedEventArgs(VisualizableProcess process, bool aborted)
					: base(process)
				{
					_aborted = aborted;
				}


				/// <summary>
				/// Gets flag indicating if the search was prematurely.  True if aborted.
				/// </summary>
				public bool Aborted
				{
					get
					{
						return _aborted;
					}
				}
			}					

			#endregion
			
			#region public class MessageQueueFoundEventArgs : EventArgs

			/// <summary>
			/// MessageQueueFoundEvent event arguments.
			/// </summary>
			public class MessageQueueFoundEventArgs : EventArgs
			{
				private MessageQueue _queue;

				/// <summary>
				/// Constructs event arguments object.
				/// </summary>
				/// <param name="queue">Queue which was found.</param>
				public MessageQueueFoundEventArgs(MessageQueue queue)					
				{
					_queue = queue;
				}


				/// <summary>
				/// Gets the message queue which was found
				/// </summary>
				public MessageQueue MessageQueue
				{
					get
					{
						return _queue;
					}
				}
			}

			#endregion
	
			#region public class SearchExceptionEventArgs : EventArgs

			/// <summary>
			/// SearchException event arguments.
			/// </summary>
			public class SearchExceptionEventArgs : EventArgs
			{
				private Exception _exception;

				/// <summary>
				/// Constructs event arguments object.
				/// </summary>
				/// <param name="exception">Exception related to event.</param>
				public SearchExceptionEventArgs(Exception exception)					
				{
					_exception = exception;
				}


				/// <summary>
				/// Gets the exception related to the event.
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

			#region public class HostMachineFoundEventArgs : EventArgs

			/// <summary>
			/// HostMachineFound event arguments.
			/// </summary>
			public class HostMachineFoundEventArgs : EventArgs
			{
				private string _machineName;

				/// <summary>
				/// Constructs the object.
				/// </summary>
				/// <param name="machineName">Name of machine hosting MSMQ message queues qhich has been found.</param>
				public HostMachineFoundEventArgs(string machineName)
				{
					_machineName = machineName;
				}


				/// <summary>
				/// Gets the name of machine hosting MSMQ message queues qhich has been found.
				/// </summary>
				public string MachineName
				{
					get
					{
						return _machineName;
					}
				}
			}

			#endregion
			
			/// <summary>
			/// Occurs when the asynchronous search process has started.
			/// </summary>
			public event VisualizableProcessEvent SearchStarted
			{
				add
				{
					_searchStarted += value;
				}
				remove
				{
					_searchStarted -= value;
				}
			}


			/// <summary>
			/// Occurs when the asynchronous search process has completed, 
			/// either as the search as finished or was aborted.
			/// </summary>
			public event QueueSearcher.SearchFinishedEvent SearchFinished
			{
				add
				{
					_searchFinished += value;
				}
				remove
				{
					_searchFinished -= value;
				}
			}


			/// <summary>
			/// Occurs when a <see cref="MessageQueue"/> has been found.
			/// </summary>
			public event QueueSearcher.MessageQueueFoundEvent MessageQueueFound
			{
				add
				{
					_messageQueueFound += value;
				}
				remove
				{
					_messageQueueFound -= value;
				}
			}


			/// <summary>
			/// Occurs when an exception occurs during the search process.
			/// </summary>
			public event QueueSearcher.SearchExceptionEvent SearchException
			{
				add
				{
					_searchException += value;
				}
				remove
				{
					_searchException -= value;
				}
			}


			/// <summary>
			/// Occurs when a MSMQ host machine maching the search criteria has been found.
			/// </summary>
			public event QueueSearcher.HostMachineFoundEvent HostMachineFound
			{
				add
				{
					_machineFound += value;
				}
				remove
				{
					_machineFound -= value;
				}
			}


			/// <summary>
			/// Raises the SearchStarted event.
			/// </summary>
			/// <param name="e">Event arguments.</param>
			private void OnSearchStarted(VisualizableProcessEventArgs e)
			{
				try
				{
					if (_searchStarted != null)
						_searchStarted(this, e);
				}
				catch {}
			}


			/// <summary>
			/// Raises the SearchFinished event.
			/// </summary>
			/// <param name="e">Event arguments.</param>
			private void OnSearchFinished(QueueSearcher.SearchFinishedEventArgs e)
			{
				try
				{
					if (_searchFinished != null)
						_searchFinished(this, e);
				}
				catch {}
			}


			/// <summary>
			/// Raises the <see cref="MessageQueueFound"/> event.
			/// </summary>
			/// <param name="e">Event arguments</param>
			private void OnMessageQueueFound(QueueSearcher.MessageQueueFoundEventArgs e)
			{
				try
				{
					if (_messageQueueFound != null)
						_messageQueueFound(this, e);
				}
				catch {}
			}


			/// <summary>
			/// Raises the <see cref="SearchException"/> event.
			/// </summary>
			/// <param name="e">Event arguments.</param>
			private void OnSearchException(QueueSearcher.SearchExceptionEventArgs e)
			{
				try
				{
					if (_searchException != null)
						_searchException(this, e);
				}
				catch {}
			}


			/// <summary>
			/// Raises the <see cref="HostMachineFound"/> event.
			/// </summary>
			/// <param name="e">Event arguments.</param>
			private void OnHostMachineFound(QueueSearcher.HostMachineFoundEventArgs e)
			{
				try
				{
					if (_machineFound != null)
						_machineFound(this, e);
				}
				catch {}
			}

			#endregion

			/// <summary>
			/// Starts an asynchronous search for queues on the network using the specified criteria.
			/// </summary>
			/// <param name="criteria">Search criteria.</param>
			public void StartSearch(MessageQueueCriteria criteria)
			{
				if (!_isSearchInProgress)
					ThreadPool.QueueUserWorkItem(new WaitCallback(StartSearchWaitCallback), criteria);
			}


			/// <summary>
			/// Starts an aysncrhonous search for private queues on the specified machine.
			/// </summary>
			/// <param name="privateQueuesMachineName">Machine to search.</param>
			public void StartSearch(string privateQueuesMachineName)
			{
				if (!_isSearchInProgress)
					ThreadPool.QueueUserWorkItem(new WaitCallback(StartSearchWaitCallback), privateQueuesMachineName);
			}


			/// <summary>
			/// Starts a search for all queues on the network.
			/// </summary>
			public void StartSearch()
			{
				if (!_isSearchInProgress)
					ThreadPool.QueueUserWorkItem(new WaitCallback(StartSearchWaitCallback));
			}


			/// <summary>
			/// Cancels the asynchronous search process.
			/// </summary>
			public void Abort()
			{
				_abortRequested = true;
			}


			/// <summary>
			/// Gets a flag notifiying if a search is currently in progress.
			/// </summary>
			public bool IsSearchInProgress
			{
				get
				{
					return _isSearchInProgress;
				}
			}


			/// <summary>
			/// Call back method which is called from the thread pool, and starts the search process.
			/// </summary>
			/// <param name="state"></param>
			private void StartSearchWaitCallback(object state)
			{
				if (state is MessageQueueCriteria)
					DoSearch((MessageQueueCriteria)state);
				else if (state is string)
					DoPrivateQueueSearch((string)state);
				else
					DoSearch(null);
			}


			/// <summary>
			/// Performs search start initialization.
			/// </summary>
			/// <returns>A <see cref="VisualizableProcess"/> to identify the job.</returns>
			private VisualizableProcess InitializeSearch()
			{
				VisualizableProcess process = new VisualizableProcess("Searching...", true);				

				_isSearchInProgress = true;
				_abortRequested = false;				
				OnSearchStarted(new VisualizableProcessEventArgs(process));

				return process;
			}


			/// <summary>
			/// Finalizes the search.
			/// </summary>
			/// <param name="process">The <see cref="VisualizableProcess"/> associated with the job.</param>
			private void FinalizeSearch(VisualizableProcess process)
			{
				_isSearchInProgress = false;
				OnSearchFinished(new QueueSearcher.SearchFinishedEventArgs(process, _abortRequested));				
			}


			/// <summary>
			/// Searches a machine for all of its private queues.
			/// </summary>
			/// <param name="machineName">Name of machine.</param>
			private void DoPrivateQueueSearch(string machineName)
			{
				VisualizableProcess process = InitializeSearch();

				try
				{
					MessageQueue[] queueList = MessageQueue.GetPrivateQueuesByMachine(machineName);
					
					bool machineEventRaised = false;
					foreach(MessageQueue queue in queueList)
					{
						if (!machineEventRaised)
						{
							OnHostMachineFound(new HostMachineFoundEventArgs(queue.MachineName));
							machineEventRaised = true;
						}

						OnMessageQueueFound(new QueueSearcher.MessageQueueFoundEventArgs(queue));
					}
				}
				catch (Exception exc)
				{
					OnSearchException(new SearchExceptionEventArgs(exc));
				}
				finally
				{
					FinalizeSearch(process);
				}
			}


			/// <summary>
			/// Performs the message queue search using the search criteria.
			/// </summary>
			/// <param name="criteria">Search criteria.  This value may be null.</param>
			private void DoSearch(MessageQueueCriteria criteria)
			{								
				MessageQueueServer enumerator = null;
				_computerNodesHashTable = new Hashtable();

				VisualizableProcess process = InitializeSearch();

				try
				{

                    enumerator = new MessageQueueServer(criteria, this);

					//itterate thru all public queues					
					while(enumerator.MoveNext() && !_abortRequested)
					{
                        var current = ((IEnumerator<MessageQueue>)enumerator).Current;

						//whenever we find a new computer, raise the host found event, and list all of the private queues
						if (!_computerNodesHashTable.ContainsKey(current.MachineName))
						{
							_computerNodesHashTable.Add(current.MachineName, current.MachineName);
							OnHostMachineFound(new HostMachineFoundEventArgs(current.MachineName));

							try
							{
								MessageQueue[] queueList = MessageQueue.GetPrivateQueuesByMachine(current.MachineName);
								foreach (MessageQueue privateQueue in queueList)
									OnMessageQueueFound(new QueueSearcher.MessageQueueFoundEventArgs(privateQueue));						
							}
							catch (Exception exc)
							{
								OnSearchException(new SearchExceptionEventArgs(exc));
							}
						}

						//raise queue found notification
						OnMessageQueueFound(new QueueSearcher.MessageQueueFoundEventArgs(current));						
					}
				}
				catch (Exception exc)
				{
					OnSearchException(new SearchExceptionEventArgs(exc));
				}
				finally 
				{
					if (enumerator != null)
						enumerator.Dispose();
					FinalizeSearch(process);
				}
			}


            private class MessageQueueServer : IEnumerator<MessageQueue>
            {
                private readonly MessageQueueCriteria criteria;
                private readonly QueueSearcher parent;
                private readonly List<MessageQueue> queues = new List<MessageQueue>();
                private int pos;

                public MessageQueueServer(MessageQueueCriteria criteria, QueueSearcher parent)
                {
                    this.criteria = criteria;
                    this.parent = parent;
                    Reset();
                }

                public object Current
                {
                    get { return this.queues[pos]; }
                }

                public bool MoveNext()
                {
                    if (pos < this.queues.Count - 1)
                    {
                        pos++;
                        return true;
                    }

                    return false;
                }

                public void Reset()
                {
                    queues.Clear();

                    try
                    {
                        //open an enumerator to read all of the queues					
                        var enumerator = criteria == null
                            ? MessageQueue.GetMessageQueueEnumerator()
                            : MessageQueue.GetMessageQueueEnumerator(criteria);                        

                        while (enumerator.MoveNext() && !parent._abortRequested)
                            queues.Add(enumerator.Current);
                    }
                    catch
                    {
                        queues.AddRange(criteria == null ? MessageQueue.GetPublicQueues() : MessageQueue.GetPublicQueues(criteria));                        
                    }

                    pos = -1;
                }

                MessageQueue IEnumerator<MessageQueue>.Current
                {
                    get { return this.queues[pos]; }
                }

                public void Dispose()
                {
                    
                }
            }

		}

		#endregion

		#region private class ResultTreeNode : TreeNode

		private class ResultTreeNode : TreeNode
		{
			private ListViewItem _selectedItem = null;

			public ResultTreeNode() {}

			public ResultTreeNode(string text) 
				: base(text) {}

			public ResultTreeNode(string text, int imageIndex, int selectedImageIndex, TreeNode[] children)
				: base(text, imageIndex, selectedImageIndex, children) {}

			public ResultTreeNode(string text, int imageIndex, int selectedImageIndex)
				: base(text, imageIndex, selectedImageIndex) {}

			public ResultTreeNode(string text, TreeNode[] children)
				: base(text, children) {}

			public ListViewItem SelectedItem
			{
				get
				{
					return _selectedItem;
				}
				set
				{
					_selectedItem = value;
				}
			}			
		}

		#endregion


       

	}
}
