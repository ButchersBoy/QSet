using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using TD.SandBar;
using Mulholland.Core;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for NewMessageForm.
	/// </summary>
	internal class NewMessageForm : System.Windows.Forms.Form
	{
		private const string _FORMATTER_ACTIVEX = "Active X";
		private const string _FORMATTER_BINARY = "Binary";
		private const string _FORMATTER_XML = "XML";

		private QueueSearchForm QueueSearchForm = null;
		private QueueTaskManager _queueTaskManager;		
		private Licensing.License _license;

		private System.Windows.Forms.Panel formPanel;
		private System.Windows.Forms.ListView recipientsListView;
		private System.Windows.Forms.TextBox labelTextBox;
		private System.Windows.Forms.Label labelLabel;
		private System.Windows.Forms.Button recipientsButton;
		private TD.SandBar.ToolBarContainer leftSandBarDock;
		private TD.SandBar.ToolBarContainer rightSandBarDock;
		private TD.SandBar.ToolBarContainer bottomSandBarDock;
		private TD.SandBar.ToolBarContainer topSandBarDock;
		private TD.SandBar.ButtonItem sendButtonItem;
		private TD.SandBar.ButtonItem sendAndKeepOpenButtonItem;
		private TD.SandBar.ComboBoxItem numberOfCopiesComboBoxItem;
		private TD.SandBar.ToolBar mainToolBar;
		private TD.SandBar.ComboBoxItem formatterComboBoxItem;
		private TD.SandBar.SandBarManager sandBarManager;
		private TD.SandBar.DropDownMenuItem optionsDropDownMenuItem;
		private TD.SandBar.MenuButtonItem optionsRecoverableMenuItem;
		private TD.SandBar.MenuButtonItem optionsUseAuthenticationMenuItem;
		private TD.SandBar.MenuButtonItem optionsUseDeadLetterQueueMenuItem;
		private TD.SandBar.MenuButtonItem optionsUseJournalQueueMenuItem;
		private TD.SandBar.MenuButtonItem optionsUseTracingMenuItem;
		private TD.SandBar.MenuButtonItem optionsUseEncryptionMenuItem;
		private TD.SandBar.MenuButtonItem optionsPriorityMenuItem;
		private TD.SandBar.MenuButtonItem priorityHighestMenuItem;
		private TD.SandBar.MenuButtonItem priorityVeryHighMenuItem;
		private TD.SandBar.MenuButtonItem priorityHighMenuItem;
		private TD.SandBar.MenuButtonItem priorityAboveNormalMenuItem;
		private TD.SandBar.MenuButtonItem priorityNormalMenuItem;
		private TD.SandBar.MenuButtonItem priorityLowMenuItem;
		private TD.SandBar.MenuButtonItem priorityVeryLowMenuItem;
		private TD.SandBar.MenuButtonItem priorityLowestMenuItem;
		private System.Windows.Forms.TextBox textSourceTextBox;
		private System.Windows.Forms.TextBox fileSourceTextBox;
		private System.Windows.Forms.Button fileSourceBrowseButton;
		private System.Windows.Forms.Panel sourcePanel;
		private System.Windows.Forms.Label sourceLabel;
		private System.Windows.Forms.RadioButton fileSourceRadioButton;
		private System.Windows.Forms.RadioButton textSourceRadioButton;
		private System.Windows.Forms.Panel newMessagePanel;
		private System.Windows.Forms.Panel existingMessagePanel;

		private enum Mode
		{
			NewMessage,
			ExistingMessage
		}

		private Mode _mode;
		private System.Windows.Forms.Label existingMessagesLabel;
		private System.Windows.Forms.ListView messagesListView;

		private QSetQueueItem _sourceQueueItem = null;
		private Mulholland.QSet.Application.Controls.MessageViewer _messageViewer;
		private System.Windows.Forms.Panel _messageViewerHostPanel;
		private TD.SandBar.MenuButtonItem optionsStreamDirectlyMenuItem;
		private System.Windows.Forms.CheckBox xmlCheckBox;
		private System.Windows.Forms.ToolTip toolTip;
		private System.ComponentModel.IContainer components;

		public NewMessageForm(Licensing.License license, QueueTaskManager queueTaskManager)
		{
			Initialize(license, queueTaskManager, null, null, null);
		}

		public NewMessageForm(Licensing.License license, QueueTaskManager queueTaskManager, System.Messaging.Message[] messages, QSetQueueItem sourceQueueItem)
		{
			Initialize(license, queueTaskManager, null, messages, sourceQueueItem);
		}


		public NewMessageForm(Licensing.License license, QueueTaskManager queueTaskManager, QSetQueueItem initialRecipient)
		{
			Initialize(license, queueTaskManager, initialRecipient, null, null);
		}


		private void Initialize(Licensing.License license, QueueTaskManager queueTaskManager, QSetQueueItem initialRecipient, System.Messaging.Message[] messages, QSetQueueItem sourceQueueItem)
		{		
			_license = license;

			InitializeComponent();

			if (messages != null && messages.Length > 0)
				_mode = Mode.ExistingMessage;
			else
				_mode = Mode.NewMessage;

			_queueTaskManager = queueTaskManager;
			_sourceQueueItem = sourceQueueItem;
			
			base.Closing += new CancelEventHandler(NewMessageForm_Closing);
			numberOfCopiesComboBoxItem.ToolTipText = "Select the number of copies to send to each recipient queue.";
			numberOfCopiesComboBoxItem.ComboBox.SelectedIndex = 0;
			numberOfCopiesComboBoxItem.ComboBox.KeyPress += new KeyPressEventHandler(ComboBox_KeyPress);
			numberOfCopiesComboBoxItem.ComboBox.MaxLength = 4;

			_messageViewer = new MessageViewer(license);
			_messageViewer.Dock = DockStyle.Fill;
			_messageViewerHostPanel.Controls.Add(_messageViewer);

			recipientsListView.Items.Clear();
			messagesListView.Items.Clear();

			if (_mode == Mode.NewMessage)
			{
				FormatterComboItem[] formatterItems = new FormatterComboItem[3]; 
				formatterItems[0] = new FormatterComboItem(_FORMATTER_ACTIVEX, MessageFormatterType.ActiveX);
				formatterItems[1] = new FormatterComboItem(_FORMATTER_BINARY, MessageFormatterType.Binary);
				formatterItems[2] = new FormatterComboItem(_FORMATTER_XML, MessageFormatterType.Xml);
				formatterComboBoxItem.ComboBox.DisplayMember = "Name";
				formatterComboBoxItem.ComboBox.ValueMember = "FormatterType";
				formatterComboBoxItem.ComboBox.DataSource = formatterItems;			
				formatterComboBoxItem.ComboBox.SelectedIndex = 2;
				formatterComboBoxItem.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

				foreach (MenuItemBase item in optionsPriorityMenuItem.Items)
					item.Checked = false;
				priorityNormalMenuItem.Checked = true;

				ConfigureMessagePriorityMenuItems();
				SelectMessagePriorityMenuItem(System.Messaging.MessagePriority.Normal);
			}
			else
			{
				if (messages.Length == 1)
					existingMessagesLabel.Text = "Message:";
				else
					existingMessagesLabel.Text = "Messages:";

				foreach (System.Messaging.Message message in messages)
				{
					ListViewItem item = new ListViewItem(message.Label, (int)Resources.Images.IconType.Message);
					item.Tag = message;
					messagesListView.Items.Add(item);
				}
				
				messagesListView.Items[0].Selected = true;				
			}

			//TODO when multiple messages are supplied leave all the options blank so that the existing message properties take effect
			//		but let the options be selectable so they can be overridde.  When a single message is passed into the form all
			//		of the options can be defaulted to that messages options
			sendAndKeepOpenButtonItem.Visible = _mode == Mode.NewMessage;
			formatterComboBoxItem.Visible = _mode == Mode.NewMessage;
			optionsDropDownMenuItem.Visible = _mode == Mode.NewMessage;
			labelLabel.Visible = _mode == Mode.NewMessage;
			labelTextBox.Visible =  _mode == Mode.NewMessage;			
			newMessagePanel.Visible = _mode == Mode.NewMessage;
			existingMessagePanel.Visible = _mode == Mode.ExistingMessage;

			ConfigureControls();

			SetupToolTips();

			if (initialRecipient != null)
				AddQueueItemToRecipientsList(initialRecipient);
		}


		/// <summary>
		/// Configure control state according to user options.
		/// </summary>
		private void ConfigureControls()
		{
			if (textSourceRadioButton.Checked)
			{
				textSourceTextBox.Enabled = true;
				fileSourceBrowseButton.Enabled = false;
				fileSourceTextBox.Enabled = false;
			}
			else
			{
				textSourceTextBox.Enabled = false;
				fileSourceBrowseButton.Enabled = true;
				fileSourceTextBox.Enabled = true;
			}

			formatterComboBoxItem.Enabled = (!optionsStreamDirectlyMenuItem.Checked && !xmlCheckBox.Checked);
			xmlCheckBox.Enabled = !optionsStreamDirectlyMenuItem.Checked;

			if (xmlCheckBox.Checked)
				formatterComboBoxItem.ComboBox.SelectedIndex = formatterComboBoxItem.ComboBox.FindStringExact(_FORMATTER_XML);
		}


		private void SetupToolTips()
		{
			toolTip.SetToolTip(xmlCheckBox, "Send source as an XmlDocument.");
		}


		/// <summary>
		/// Gets or sets the image list associated with the form.
		/// </summary>
		public ImageList SmallImageList
		{
			set
			{
				recipientsListView.SmallImageList = value;
				messagesListView.SmallImageList = value;
			}
			get
			{
				return recipientsListView.SmallImageList;
			}
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewMessageForm));
			this.formPanel = new System.Windows.Forms.Panel();
			this.newMessagePanel = new System.Windows.Forms.Panel();
			this.textSourceTextBox = new System.Windows.Forms.TextBox();
			this.fileSourceTextBox = new System.Windows.Forms.TextBox();
			this.fileSourceBrowseButton = new System.Windows.Forms.Button();
			this.sourcePanel = new System.Windows.Forms.Panel();
			this.xmlCheckBox = new System.Windows.Forms.CheckBox();
			this.sourceLabel = new System.Windows.Forms.Label();
			this.fileSourceRadioButton = new System.Windows.Forms.RadioButton();
			this.textSourceRadioButton = new System.Windows.Forms.RadioButton();
			this.existingMessagePanel = new System.Windows.Forms.Panel();
			this.existingMessagesLabel = new System.Windows.Forms.Label();
			this.messagesListView = new System.Windows.Forms.ListView();
			this._messageViewerHostPanel = new System.Windows.Forms.Panel();
			this.recipientsListView = new System.Windows.Forms.ListView();
			this.labelTextBox = new System.Windows.Forms.TextBox();
			this.labelLabel = new System.Windows.Forms.Label();
			this.recipientsButton = new System.Windows.Forms.Button();
			this.sandBarManager = new TD.SandBar.SandBarManager();
			this.leftSandBarDock = new TD.SandBar.ToolBarContainer();
			this.rightSandBarDock = new TD.SandBar.ToolBarContainer();
			this.bottomSandBarDock = new TD.SandBar.ToolBarContainer();
			this.topSandBarDock = new TD.SandBar.ToolBarContainer();
			this.mainToolBar = new TD.SandBar.ToolBar();
			this.sendButtonItem = new TD.SandBar.ButtonItem();
			this.sendAndKeepOpenButtonItem = new TD.SandBar.ButtonItem();
			this.numberOfCopiesComboBoxItem = new TD.SandBar.ComboBoxItem();
			this.formatterComboBoxItem = new TD.SandBar.ComboBoxItem();
			this.optionsDropDownMenuItem = new TD.SandBar.DropDownMenuItem();
			this.optionsPriorityMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityHighestMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityVeryHighMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityHighMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityAboveNormalMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityNormalMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityLowMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityVeryLowMenuItem = new TD.SandBar.MenuButtonItem();
			this.priorityLowestMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsRecoverableMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsUseAuthenticationMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsUseDeadLetterQueueMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsUseEncryptionMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsUseJournalQueueMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsUseTracingMenuItem = new TD.SandBar.MenuButtonItem();
			this.optionsStreamDirectlyMenuItem = new TD.SandBar.MenuButtonItem();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.formPanel.SuspendLayout();
			this.newMessagePanel.SuspendLayout();
			this.sourcePanel.SuspendLayout();
			this.existingMessagePanel.SuspendLayout();
			this.topSandBarDock.SuspendLayout();
			this.SuspendLayout();
			// 
			// formPanel
			// 
			this.formPanel.Controls.Add(this.newMessagePanel);
			this.formPanel.Controls.Add(this.existingMessagePanel);
			this.formPanel.Controls.Add(this.recipientsListView);
			this.formPanel.Controls.Add(this.labelTextBox);
			this.formPanel.Controls.Add(this.labelLabel);
			this.formPanel.Controls.Add(this.recipientsButton);
			this.formPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.formPanel.Location = new System.Drawing.Point(0, 26);
			this.formPanel.Name = "formPanel";
			this.formPanel.Size = new System.Drawing.Size(616, 444);
			this.formPanel.TabIndex = 12;
			// 
			// newMessagePanel
			// 
			this.newMessagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.newMessagePanel.Controls.Add(this.textSourceTextBox);
			this.newMessagePanel.Controls.Add(this.fileSourceTextBox);
			this.newMessagePanel.Controls.Add(this.fileSourceBrowseButton);
			this.newMessagePanel.Controls.Add(this.sourcePanel);
			this.newMessagePanel.Location = new System.Drawing.Point(8, 108);
			this.newMessagePanel.Name = "newMessagePanel";
			this.newMessagePanel.Size = new System.Drawing.Size(600, 328);
			this.newMessagePanel.TabIndex = 16;
			// 
			// textSourceTextBox
			// 
			this.textSourceTextBox.AcceptsReturn = true;
			this.textSourceTextBox.AcceptsTab = true;
			this.textSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.textSourceTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textSourceTextBox.Location = new System.Drawing.Point(68, 49);
			this.textSourceTextBox.Multiline = true;
			this.textSourceTextBox.Name = "textSourceTextBox";
			this.textSourceTextBox.Size = new System.Drawing.Size(532, 278);
			this.textSourceTextBox.TabIndex = 19;
			this.textSourceTextBox.Text = "";
			// 
			// fileSourceTextBox
			// 
			this.fileSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.fileSourceTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fileSourceTextBox.Location = new System.Drawing.Point(68, 21);
			this.fileSourceTextBox.Name = "fileSourceTextBox";
			this.fileSourceTextBox.Size = new System.Drawing.Size(496, 21);
			this.fileSourceTextBox.TabIndex = 17;
			this.fileSourceTextBox.Text = "";
			// 
			// fileSourceBrowseButton
			// 
			this.fileSourceBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.fileSourceBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.fileSourceBrowseButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fileSourceBrowseButton.Location = new System.Drawing.Point(568, 21);
			this.fileSourceBrowseButton.Name = "fileSourceBrowseButton";
			this.fileSourceBrowseButton.Size = new System.Drawing.Size(32, 21);
			this.fileSourceBrowseButton.TabIndex = 18;
			this.fileSourceBrowseButton.Text = "...";
			this.fileSourceBrowseButton.Click += new System.EventHandler(this.fileSourceBrowseButton_Click);
			// 
			// sourcePanel
			// 
			this.sourcePanel.Controls.Add(this.xmlCheckBox);
			this.sourcePanel.Controls.Add(this.sourceLabel);
			this.sourcePanel.Controls.Add(this.fileSourceRadioButton);
			this.sourcePanel.Controls.Add(this.textSourceRadioButton);
			this.sourcePanel.Location = new System.Drawing.Point(0, 1);
			this.sourcePanel.Name = "sourcePanel";
			this.sourcePanel.Size = new System.Drawing.Size(64, 103);
			this.sourcePanel.TabIndex = 16;
			// 
			// xmlCheckBox
			// 
			this.xmlCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.xmlCheckBox.Location = new System.Drawing.Point(4, 80);
			this.xmlCheckBox.Name = "xmlCheckBox";
			this.xmlCheckBox.Size = new System.Drawing.Size(60, 24);
			this.xmlCheckBox.TabIndex = 3;
			this.xmlCheckBox.Text = "&XML";
			this.xmlCheckBox.CheckedChanged += new System.EventHandler(this.xmlCheckBox_CheckedChanged);
			// 
			// sourceLabel
			// 
			this.sourceLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.sourceLabel.Location = new System.Drawing.Point(4, 4);
			this.sourceLabel.Name = "sourceLabel";
			this.sourceLabel.Size = new System.Drawing.Size(60, 16);
			this.sourceLabel.TabIndex = 0;
			this.sourceLabel.Text = "Source:";
			// 
			// fileSourceRadioButton
			// 
			this.fileSourceRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.fileSourceRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.fileSourceRadioButton.Location = new System.Drawing.Point(4, 24);
			this.fileSourceRadioButton.Name = "fileSourceRadioButton";
			this.fileSourceRadioButton.Size = new System.Drawing.Size(60, 16);
			this.fileSourceRadioButton.TabIndex = 1;
			this.fileSourceRadioButton.Text = "&File";
			this.fileSourceRadioButton.CheckedChanged += new System.EventHandler(this.SourceRadioButton_CheckedChanged);
			// 
			// textSourceRadioButton
			// 
			this.textSourceRadioButton.Checked = true;
			this.textSourceRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.textSourceRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.textSourceRadioButton.Location = new System.Drawing.Point(4, 48);
			this.textSourceRadioButton.Name = "textSourceRadioButton";
			this.textSourceRadioButton.Size = new System.Drawing.Size(60, 16);
			this.textSourceRadioButton.TabIndex = 2;
			this.textSourceRadioButton.TabStop = true;
			this.textSourceRadioButton.Text = "Text";
			this.textSourceRadioButton.CheckedChanged += new System.EventHandler(this.SourceRadioButton_CheckedChanged);
			// 
			// existingMessagePanel
			// 
			this.existingMessagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.existingMessagePanel.Controls.Add(this.existingMessagesLabel);
			this.existingMessagePanel.Controls.Add(this.messagesListView);
			this.existingMessagePanel.Controls.Add(this._messageViewerHostPanel);
			this.existingMessagePanel.Location = new System.Drawing.Point(8, 80);
			this.existingMessagePanel.Name = "existingMessagePanel";
			this.existingMessagePanel.Size = new System.Drawing.Size(600, 356);
			this.existingMessagePanel.TabIndex = 17;
			// 
			// existingMessagesLabel
			// 
			this.existingMessagesLabel.Location = new System.Drawing.Point(4, 4);
			this.existingMessagesLabel.Name = "existingMessagesLabel";
			this.existingMessagesLabel.Size = new System.Drawing.Size(60, 23);
			this.existingMessagesLabel.TabIndex = 11;
			this.existingMessagesLabel.Text = "Message:";
			// 
			// messagesListView
			// 
			this.messagesListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.messagesListView.HideSelection = false;
			this.messagesListView.Location = new System.Drawing.Point(68, 0);
			this.messagesListView.Name = "messagesListView";
			this.messagesListView.Size = new System.Drawing.Size(532, 68);
			this.messagesListView.TabIndex = 10;
			this.messagesListView.View = System.Windows.Forms.View.SmallIcon;
			this.messagesListView.SelectedIndexChanged += new System.EventHandler(this.messagesListView_SelectedIndexChanged);
			// 
			// _messageViewerHostPanel
			// 
			this._messageViewerHostPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._messageViewerHostPanel.Location = new System.Drawing.Point(0, 76);
			this._messageViewerHostPanel.Name = "_messageViewerHostPanel";
			this._messageViewerHostPanel.Size = new System.Drawing.Size(600, 280);
			this._messageViewerHostPanel.TabIndex = 12;
			// 
			// recipientsListView
			// 
			this.recipientsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.recipientsListView.Location = new System.Drawing.Point(76, 8);
			this.recipientsListView.Name = "recipientsListView";
			this.recipientsListView.Size = new System.Drawing.Size(532, 68);
			this.recipientsListView.TabIndex = 9;
			this.recipientsListView.View = System.Windows.Forms.View.SmallIcon;
			this.recipientsListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.recipientsListView_KeyUp);
			// 
			// labelTextBox
			// 
			this.labelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.labelTextBox.Location = new System.Drawing.Point(76, 80);
			this.labelTextBox.Name = "labelTextBox";
			this.labelTextBox.Size = new System.Drawing.Size(532, 21);
			this.labelTextBox.TabIndex = 11;
			this.labelTextBox.Text = "";
			// 
			// labelLabel
			// 
			this.labelLabel.Location = new System.Drawing.Point(12, 84);
			this.labelLabel.Name = "labelLabel";
			this.labelLabel.Size = new System.Drawing.Size(52, 16);
			this.labelLabel.TabIndex = 10;
			this.labelLabel.Text = "&Label:";
			// 
			// recipientsButton
			// 
			this.recipientsButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.recipientsButton.Location = new System.Drawing.Point(8, 8);
			this.recipientsButton.Name = "recipientsButton";
			this.recipientsButton.Size = new System.Drawing.Size(60, 23);
			this.recipientsButton.TabIndex = 8;
			this.recipientsButton.Text = "&To...";
			this.recipientsButton.Click += new System.EventHandler(this.recipientsButton_Click);
			// 
			// sandBarManager
			// 
			this.sandBarManager.OwnerForm = this;
			// 
			// leftSandBarDock
			// 
			this.leftSandBarDock.Dock = System.Windows.Forms.DockStyle.Left;
			this.leftSandBarDock.Guid = new System.Guid("ce92933a-b6c2-4136-9cad-1d50f047a7b4");
			this.leftSandBarDock.Location = new System.Drawing.Point(0, 26);
			this.leftSandBarDock.Manager = this.sandBarManager;
			this.leftSandBarDock.Name = "leftSandBarDock";
			this.leftSandBarDock.Size = new System.Drawing.Size(0, 444);
			this.leftSandBarDock.TabIndex = 13;
			// 
			// rightSandBarDock
			// 
			this.rightSandBarDock.Dock = System.Windows.Forms.DockStyle.Right;
			this.rightSandBarDock.Guid = new System.Guid("700c810c-1dda-42e5-ab48-881c72c966a6");
			this.rightSandBarDock.Location = new System.Drawing.Point(616, 26);
			this.rightSandBarDock.Manager = this.sandBarManager;
			this.rightSandBarDock.Name = "rightSandBarDock";
			this.rightSandBarDock.Size = new System.Drawing.Size(0, 444);
			this.rightSandBarDock.TabIndex = 14;
			// 
			// bottomSandBarDock
			// 
			this.bottomSandBarDock.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.bottomSandBarDock.Guid = new System.Guid("565be2ba-62fa-4d8e-a041-ce649cf5adb3");
			this.bottomSandBarDock.Location = new System.Drawing.Point(0, 470);
			this.bottomSandBarDock.Manager = this.sandBarManager;
			this.bottomSandBarDock.Name = "bottomSandBarDock";
			this.bottomSandBarDock.Size = new System.Drawing.Size(616, 0);
			this.bottomSandBarDock.TabIndex = 15;
			// 
			// topSandBarDock
			// 
			this.topSandBarDock.Controls.Add(this.mainToolBar);
			this.topSandBarDock.Dock = System.Windows.Forms.DockStyle.Top;
			this.topSandBarDock.Guid = new System.Guid("8c813572-b5bb-4bdd-af9b-e2e66ad1a878");
			this.topSandBarDock.Location = new System.Drawing.Point(0, 0);
			this.topSandBarDock.Manager = this.sandBarManager;
			this.topSandBarDock.Name = "topSandBarDock";
			this.topSandBarDock.Size = new System.Drawing.Size(616, 26);
			this.topSandBarDock.TabIndex = 16;
			// 
			// mainToolBar
			// 
			this.mainToolBar.DockLine = 1;
			this.mainToolBar.DrawActionsButton = false;
			this.mainToolBar.Guid = new System.Guid("71951701-c70e-430c-a8d2-6bba5bc2b776");
			this.mainToolBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
																				 this.sendButtonItem,
																				 this.sendAndKeepOpenButtonItem,
																				 this.numberOfCopiesComboBoxItem,
																				 this.formatterComboBoxItem,
																				 this.optionsDropDownMenuItem});
			this.mainToolBar.Location = new System.Drawing.Point(2, 0);
			this.mainToolBar.Name = "mainToolBar";
			this.mainToolBar.Size = new System.Drawing.Size(499, 26);
			this.mainToolBar.TabIndex = 1;
			this.mainToolBar.Text = "Main";
			// 
			// sendButtonItem
			// 
			this.sendButtonItem.Icon = ((System.Drawing.Icon)(resources.GetObject("sendButtonItem.Icon")));
			this.sendButtonItem.Text = "&Send";
			this.sendButtonItem.Activate += new System.EventHandler(this.sendButtonItem_Activate);
			// 
			// sendAndKeepOpenButtonItem
			// 
			this.sendAndKeepOpenButtonItem.Icon = ((System.Drawing.Icon)(resources.GetObject("sendAndKeepOpenButtonItem.Icon")));
			this.sendAndKeepOpenButtonItem.Text = "Send and &Keep Open";
			this.sendAndKeepOpenButtonItem.Activate += new System.EventHandler(this.sendButtonItem_Activate);
			// 
			// numberOfCopiesComboBoxItem
			// 
			this.numberOfCopiesComboBoxItem.BeginGroup = true;
			this.numberOfCopiesComboBoxItem.DefaultText = "1";
			this.numberOfCopiesComboBoxItem.Items.AddRange(new object[] {
																			"1",
																			"2",
																			"3",
																			"4",
																			"5",
																			"10",
																			"25",
																			"50",
																			"100",
																			"250",
																			"500",
																			"1000"});
			this.numberOfCopiesComboBoxItem.MinimumControlWidth = 55;
			this.numberOfCopiesComboBoxItem.Padding.Bottom = 0;
			this.numberOfCopiesComboBoxItem.Padding.Left = 1;
			this.numberOfCopiesComboBoxItem.Padding.Right = 1;
			this.numberOfCopiesComboBoxItem.Padding.Top = 0;
			this.numberOfCopiesComboBoxItem.Text = "Copies";
			// 
			// formatterComboBoxItem
			// 
			this.formatterComboBoxItem.MinimumControlWidth = 75;
			this.formatterComboBoxItem.Padding.Bottom = 0;
			this.formatterComboBoxItem.Padding.Left = 1;
			this.formatterComboBoxItem.Padding.Right = 1;
			this.formatterComboBoxItem.Padding.Top = 0;
			this.formatterComboBoxItem.Text = "Formatter";
			// 
			// optionsDropDownMenuItem
			// 
			this.optionsDropDownMenuItem.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
																							 this.optionsPriorityMenuItem,
																							 this.optionsRecoverableMenuItem,
																							 this.optionsUseAuthenticationMenuItem,
																							 this.optionsUseDeadLetterQueueMenuItem,
																							 this.optionsUseEncryptionMenuItem,
																							 this.optionsUseJournalQueueMenuItem,
																							 this.optionsUseTracingMenuItem,
																							 this.optionsStreamDirectlyMenuItem});
			this.optionsDropDownMenuItem.Text = "&Options";
			// 
			// optionsPriorityMenuItem
			// 
			this.optionsPriorityMenuItem.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
																							 this.priorityHighestMenuItem,
																							 this.priorityVeryHighMenuItem,
																							 this.priorityHighMenuItem,
																							 this.priorityAboveNormalMenuItem,
																							 this.priorityNormalMenuItem,
																							 this.priorityLowMenuItem,
																							 this.priorityVeryLowMenuItem,
																							 this.priorityLowestMenuItem});
			this.optionsPriorityMenuItem.Text = "Priority";
			// 
			// priorityHighestMenuItem
			// 
			this.priorityHighestMenuItem.MergeAction = TD.SandBar.ItemMergeAction.Insert;
			this.priorityHighestMenuItem.Text = "Highest";
			this.priorityHighestMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityVeryHighMenuItem
			// 
			this.priorityVeryHighMenuItem.Text = "Very High";
			this.priorityVeryHighMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityHighMenuItem
			// 
			this.priorityHighMenuItem.Text = "High";
			this.priorityHighMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityAboveNormalMenuItem
			// 
			this.priorityAboveNormalMenuItem.Text = "Above Normal";
			this.priorityAboveNormalMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityNormalMenuItem
			// 
			this.priorityNormalMenuItem.Text = "Normal";
			this.priorityNormalMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityLowMenuItem
			// 
			this.priorityLowMenuItem.Text = "Low";
			this.priorityLowMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityVeryLowMenuItem
			// 
			this.priorityVeryLowMenuItem.Text = "Very Low";
			this.priorityVeryLowMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// priorityLowestMenuItem
			// 
			this.priorityLowestMenuItem.Text = "Lowest";
			this.priorityLowestMenuItem.Activate += new System.EventHandler(this.priorityMenuItem_Activate);
			// 
			// optionsRecoverableMenuItem
			// 
			this.optionsRecoverableMenuItem.BeginGroup = true;
			this.optionsRecoverableMenuItem.Text = "Recoverable";
			this.optionsRecoverableMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// optionsUseAuthenticationMenuItem
			// 
			this.optionsUseAuthenticationMenuItem.Text = "Use Authentication";
			this.optionsUseAuthenticationMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// optionsUseDeadLetterQueueMenuItem
			// 
			this.optionsUseDeadLetterQueueMenuItem.Text = "Use Dead Letter Queue";
			this.optionsUseDeadLetterQueueMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// optionsUseEncryptionMenuItem
			// 
			this.optionsUseEncryptionMenuItem.Text = "Use Encryption";
			this.optionsUseEncryptionMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// optionsUseJournalQueueMenuItem
			// 
			this.optionsUseJournalQueueMenuItem.Text = "Use Journal Queue";
			this.optionsUseJournalQueueMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// optionsUseTracingMenuItem
			// 
			this.optionsUseTracingMenuItem.Text = "Use Tracing";
			this.optionsUseTracingMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// optionsStreamDirectlyMenuItem
			// 
			this.optionsStreamDirectlyMenuItem.BeginGroup = true;
			this.optionsStreamDirectlyMenuItem.Text = "Stream Directly";
			this.optionsStreamDirectlyMenuItem.Activate += new System.EventHandler(this.optionsMenuItem_Activate);
			// 
			// NewMessageForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(616, 470);
			this.Controls.Add(this.formPanel);
			this.Controls.Add(this.leftSandBarDock);
			this.Controls.Add(this.rightSandBarDock);
			this.Controls.Add(this.bottomSandBarDock);
			this.Controls.Add(this.topSandBarDock);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "NewMessageForm";
			this.Text = "New Message";
			this.formPanel.ResumeLayout(false);
			this.newMessagePanel.ResumeLayout(false);
			this.sourcePanel.ResumeLayout(false);
			this.existingMessagePanel.ResumeLayout(false);
			this.topSandBarDock.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion


		private void SelectMessagePriorityMenuItem(System.Messaging.MessagePriority priority)
		{
			foreach (MenuItemBase item in optionsPriorityMenuItem.Items)
				if ((System.Messaging.MessagePriority)item.Tag == priority)
					item.Checked = true;
				else
					item.Checked = false;
		}

		
		private void ConfigureMessagePriorityMenuItems()
		{
			priorityHighestMenuItem.Tag = System.Messaging.MessagePriority.Highest;
			priorityVeryHighMenuItem.Tag = System.Messaging.MessagePriority.VeryHigh;
			priorityHighMenuItem.Tag = System.Messaging.MessagePriority.High;
			priorityAboveNormalMenuItem.Tag = System.Messaging.MessagePriority.AboveNormal;
			priorityNormalMenuItem.Tag = System.Messaging.MessagePriority.Normal;
			priorityLowMenuItem.Tag = System.Messaging.MessagePriority.Low;
			priorityVeryLowMenuItem.Tag = System.Messaging.MessagePriority.VeryLow;
			priorityLowestMenuItem.Tag = System.Messaging.MessagePriority.Lowest;
		}
		

		private System.Messaging.MessagePriority GetSelectedMessagePriority()
		{
			System.Messaging.MessagePriority result = System.Messaging.MessagePriority.Normal;

			foreach (MenuItemBase item in optionsPriorityMenuItem.Items)			
				if (item.Checked)
				{
					result = (System.Messaging.MessagePriority)item.Tag;
					break;
				}

			return result;
		}


		private void recipientsButton_Click(object sender, System.EventArgs e)
		{
			if (QueueSearchForm == null)
			{
				QueueSearchForm = new QueueSearchForm();				
				QueueSearchForm.ImageList = recipientsListView.SmallImageList;
				QueueSearchForm.AllowMachineSelect = true;
				QueueSearchForm.QueueImageIndex = (int)Images.IconType.Queue;
				QueueSearchForm.ComputerImageIndex = (int)Images.IconType.Server;
			}			
			if (QueueSearchForm.ShowDialog() == DialogResult.OK)
			{
				foreach (QSetItemBase item in QueueSearchForm.SelectedItems)
				{
					QSetFolderItem machineItem = item as QSetFolderItem;
					if (machineItem != null)					
						foreach (QSetQueueItem childQueue in machineItem.ChildItems)						
						{
							AddQueueItemToRecipientsList(childQueue);
						}
					else
					{
						AddQueueItemToRecipientsList((QSetQueueItem)item);
					}
				}
			}
		}


		private void NewMessageForm_Closing(object sender, CancelEventArgs e)
		{
			if (QueueSearchForm != null)
			{
				QueueSearchForm.Close();
				QueueSearchForm.Dispose();
			}
		}


		private void AddQueueItemToRecipientsList(QSetQueueItem queueItem)
		{
			ListViewItem recipientItem = new ListViewItem(queueItem.Name, (int)Resources.Images.IconType.Queue);
			recipientItem.Tag = queueItem;	
			recipientsListView.Items.Add(recipientItem);
		}


		private void SourceRadioButton_CheckedChanged(object sender, System.EventArgs e)
		{
			ConfigureControls();
		}


		private class FormatterComboItem
		{
			private string _name;
			private MessageFormatterType _formatterType;

			public FormatterComboItem(string name,  MessageFormatterType formatterType)
			{
				_name = name;
				_formatterType = formatterType;
			}


			public string Name
			{
				get
				{
					return _name;
				}
			}


			public MessageFormatterType FormatterType
			{
				get
				{
					return _formatterType;
				}
			}
		}


		private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			int ascii = (int)e.KeyChar;

			if (!((ascii >= 48 && ascii <= 57) || ascii == 8))
				e.Handled = true;
		}


		private void fileSourceBrowseButton_Click(object sender, System.EventArgs e)
		{
			//open new Q Set
			using (OpenFileDialog openDialog = new OpenFileDialog())
			{
				//prompt user to select file
				openDialog.Title = "Open message source file";								
				openDialog.InitialDirectory = "\\My Documents";
				openDialog.Filter = "All files|*.*";
				if (openDialog.ShowDialog() == DialogResult.OK)
				{
					fileSourceTextBox.Text = openDialog.FileName;
				}
			}		
		}


		private void sendButtonItem_Activate(object sender, System.EventArgs e)
		{			
			if (ValidatePage())
			{								
				SendMessage(sender == sendAndKeepOpenButtonItem);
			}
		}


		/// <summary>
		/// Sends the message to all recipients.
		/// </summary>
		/// <param name="keepWindowOpen">Set to true to keep the window open, else the window will close.</param>
		private void SendMessage(bool keepWindowOpen)
		{
			System.Messaging.Message[] messages = null; 
			bool success = false;

			try
			{				
				//check the license allows the operation
				if (_license.ValidateFeatureUse(_mode == Mode.NewMessage ? Licensing.Feature.NewMessage : Licensing.Feature.FowardMessage))	
				{
			
					//generate an array of all target queues
					System.Messaging.MessageQueue[] messageQueues = new System.Messaging.MessageQueue[recipientsListView.Items.Count];
					for (int i = 0; i < recipientsListView.Items.Count; i ++)
					{
						messageQueues[i] = (System.Messaging.MessageQueue)((QSetQueueItem)recipientsListView.Items[i].Tag).QSetMessageQueue;
					}

					//create a new message, or prepare existing ones, to send to recipients
					if (_mode == Mode.NewMessage)
					{
						//create the message from the releavent source
						messages = new System.Messaging.Message[1] {new System.Messaging.Message()};
						FillMessageBody(messages[0]);
						messages[0].Label = labelTextBox.Text;					

						//set message options
						messages[0].Recoverable = optionsRecoverableMenuItem.Checked;
						messages[0].UseAuthentication = optionsUseAuthenticationMenuItem.Checked;
						messages[0].UseDeadLetterQueue = optionsUseDeadLetterQueueMenuItem.Checked;
						messages[0].UseEncryption = optionsUseEncryptionMenuItem.Checked;
						messages[0].UseJournalQueue = optionsUseJournalQueueMenuItem.Checked;
						messages[0].UseTracing = optionsUseTracingMenuItem.Checked;								
						messages[0].Priority = GetSelectedMessagePriority();
					}
					else
					{
						//prepare existing messages...

						messages = new System.Messaging.Message[messagesListView.Items.Count];
						for (int i = 0; i < messagesListView.Items.Count; i ++)					
						{
							messages[i] = _queueTaskManager.DuplicateMessage(_sourceQueueItem.QSetMessageQueue, ((System.Messaging.Message)messagesListView.Items[i].Tag).Id);
						}
					}

					if (keepWindowOpen)
						Cursor = Cursors.WaitCursor;					
					else
						this.Hide();	

					//send the messages
					foreach (System.Messaging.Message message in messages)
						_queueTaskManager.BulkSend(messageQueues, message, Convert.ToInt32(numberOfCopiesComboBoxItem.ComboBox.Text));

					success = true;
				}			
			}
			catch (Exception exc)
			{
				MessageBox.Show(
					this, 
					string.Format("Encountered an error while preparing to send message:\n\n{0}", exc.Message), 
					this.Text, 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			finally
			{
				if (_mode == Mode.NewMessage && messages != null)
					foreach (System.Messaging.Message message in messages)
						message.Dispose();
				
				Cursor = Cursors.Default;
				
				if (!keepWindowOpen && success)
				{
					DialogResult = DialogResult.OK;
					this.Close();
				}
			}
		}


		/// <summary>
		/// Fills a messages body or body stream, according to the on screen options.
		/// </summary>
		/// <param name="message">Message body.</param>
		private void FillMessageBody(System.Messaging.Message message)
		{
			//should we write directly to the body stream?
			if (optionsStreamDirectlyMenuItem.Checked)
			{
				if (textSourceRadioButton.Checked)
					message.BodyStream = IOUtilities.StringToMemoryStream(textSourceTextBox.Text);					
				else
					message.BodyStream = new FileStream(fileSourceTextBox.Text, FileMode.Open, FileAccess.Read);
			}
			else
			{
				//set up the formatter
				System.Messaging.IMessageFormatter formatter;
				switch ((MessageFormatterType)formatterComboBoxItem.ComboBox.SelectedValue)
				{
					case MessageFormatterType.ActiveX:
						formatter = new System.Messaging.ActiveXMessageFormatter();
						break;
					case MessageFormatterType.Binary:
						formatter = new System.Messaging.BinaryMessageFormatter();
						break;
					case MessageFormatterType.Xml:
					default:
						formatter = new System.Messaging.XmlMessageFormatter();
						break;
				}					
				message.Formatter = formatter;

				//set up the body
				object body;
				if (textSourceRadioButton.Checked)
				{
					if (xmlCheckBox.Checked)
					{
						body = CreateXmlDocument(textSourceTextBox.Text);
					}
					else
						body = textSourceTextBox.Text;					
				}
				else
				{
					using (StreamReader sr = new StreamReader(fileSourceTextBox.Text))
					{
						body = sr.ReadToEnd();
					}
					if (xmlCheckBox.Checked)
						body = CreateXmlDocument(body.ToString());
				}
				message.Body = body;
			}
		}


		/// <summary>
		/// Creates an XML document.
		/// </summary>
		/// <param name="xml">XML.</param>
		/// <returns>New XML document.</returns>
		private XmlDocument CreateXmlDocument(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			return xmlDocument;
		}


		private bool ValidatePage()
		{
			bool result = true;

			string validationMsg = "";

			if (recipientsListView.Items.Count == 0)
			{
				validationMsg = "Please select a destination queue.";
			}
			else if (fileSourceRadioButton.Checked && !System.IO.File.Exists(fileSourceTextBox.Text))
			{
				validationMsg = "Cannot find the file specified.  Please enter a valid filename.";
			}

			if (validationMsg != "")
			{
				MessageBox.Show(this, validationMsg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				result = false;
			}

			return result;
		}



		private void optionsMenuItem_Activate(object sender, System.EventArgs e)
		{
			((MenuButtonItem)sender).Checked = !((MenuButtonItem)sender).Checked;
			ConfigureControls();
		}


		private void priorityMenuItem_Activate(object sender, System.EventArgs e)
		{
			SelectMessagePriorityMenuItem((System.Messaging.MessagePriority)((MenuItemBase)sender).Tag);
		}


		private void recipientsListView_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete) 
				while (recipientsListView.SelectedItems.Count > 0)
					recipientsListView.SelectedItems[0].Remove();
		}

		private void messagesListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (messagesListView.SelectedItems.Count > 0)
				_messageViewer.DisplayMessage(_sourceQueueItem, (System.Messaging.Message)messagesListView.SelectedItems[0].Tag);
		}

		private void xmlCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			ConfigureControls();
		}
	}
}
