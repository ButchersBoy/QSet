namespace Mulholland.QSet.Application
{
    partial class NewMessageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewMessageForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
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
            this.mainToolBar = new System.Windows.Forms.ToolStrip();
            this.sendButtonItem = new System.Windows.Forms.ToolStripLabel();
            this.sendAndKeepOpenButtonItem = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.numberOfCopiesComboBoxItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.formatterComboBoxItem = new System.Windows.Forms.ToolStripComboBox();
            this.optionsDropDownMenuItem = new System.Windows.Forms.ToolStripSplitButton();
            this.optionsPriorityMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityHighestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityVeryHighMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityHighMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityAboveNormalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityNormalMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityLowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityVeryLowMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.priorityLowestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsRecoverableMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsUseAuthenticationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsUseDeadLetterQueueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsUseEncryptionMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsUseJournalQueueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsUseTracingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsStreamDirectlyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.formPanel.SuspendLayout();
            this.newMessagePanel.SuspendLayout();
            this.sourcePanel.SuspendLayout();
            this.existingMessagePanel.SuspendLayout();
            this.mainToolBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.formPanel);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(591, 421);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(591, 446);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.mainToolBar);
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
            this.formPanel.Location = new System.Drawing.Point(0, 0);
            this.formPanel.Name = "formPanel";
            this.formPanel.Size = new System.Drawing.Size(591, 421);
            this.formPanel.TabIndex = 13;
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
            this.newMessagePanel.Size = new System.Drawing.Size(575, 305);
            this.newMessagePanel.TabIndex = 16;
            // 
            // textSourceTextBox
            // 
            this.textSourceTextBox.AcceptsReturn = true;
            this.textSourceTextBox.AcceptsTab = true;
            this.textSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textSourceTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textSourceTextBox.Location = new System.Drawing.Point(68, 49);
            this.textSourceTextBox.Multiline = true;
            this.textSourceTextBox.Name = "textSourceTextBox";
            this.textSourceTextBox.Size = new System.Drawing.Size(507, 255);
            this.textSourceTextBox.TabIndex = 19;
            // 
            // fileSourceTextBox
            // 
            this.fileSourceTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSourceTextBox.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSourceTextBox.Location = new System.Drawing.Point(68, 21);
            this.fileSourceTextBox.Name = "fileSourceTextBox";
            this.fileSourceTextBox.Size = new System.Drawing.Size(471, 21);
            this.fileSourceTextBox.TabIndex = 17;
            // 
            // fileSourceBrowseButton
            // 
            this.fileSourceBrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fileSourceBrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fileSourceBrowseButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSourceBrowseButton.Location = new System.Drawing.Point(543, 21);
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
            this.xmlCheckBox.CheckedChanged += xmlCheckBox_CheckedChanged;
            // 
            // sourceLabel
            // 
            this.sourceLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sourceLabel.Location = new System.Drawing.Point(4, 4);
            this.sourceLabel.Name = "sourceLabel";
            this.sourceLabel.Size = new System.Drawing.Size(60, 16);
            this.sourceLabel.TabIndex = 0;
            this.sourceLabel.Text = "Source:";
            // 
            // fileSourceRadioButton
            // 
            this.fileSourceRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.fileSourceRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileSourceRadioButton.Location = new System.Drawing.Point(4, 24);
            this.fileSourceRadioButton.Name = "fileSourceRadioButton";
            this.fileSourceRadioButton.Size = new System.Drawing.Size(60, 16);
            this.fileSourceRadioButton.TabIndex = 1;
            this.fileSourceRadioButton.Text = "&File";
            // 
            // textSourceRadioButton
            // 
            this.textSourceRadioButton.Checked = true;
            this.textSourceRadioButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textSourceRadioButton.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.existingMessagePanel.Size = new System.Drawing.Size(575, 333);
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
            this.messagesListView.Size = new System.Drawing.Size(507, 68);
            this.messagesListView.TabIndex = 10;
            this.messagesListView.UseCompatibleStateImageBehavior = false;
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
            this._messageViewerHostPanel.Size = new System.Drawing.Size(575, 257);
            this._messageViewerHostPanel.TabIndex = 12;
            // 
            // recipientsListView
            // 
            this.recipientsListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.recipientsListView.Location = new System.Drawing.Point(76, 8);
            this.recipientsListView.Name = "recipientsListView";
            this.recipientsListView.Size = new System.Drawing.Size(507, 68);
            this.recipientsListView.TabIndex = 9;
            this.recipientsListView.UseCompatibleStateImageBehavior = false;
            this.recipientsListView.View = System.Windows.Forms.View.SmallIcon;
            this.recipientsListView.KeyUp += new System.Windows.Forms.KeyEventHandler(this.recipientsListView_KeyUp);
            // 
            // labelTextBox
            // 
            this.labelTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTextBox.Location = new System.Drawing.Point(76, 80);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(507, 20);
            this.labelTextBox.TabIndex = 11;
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
            // mainToolBar
            // 
            this.mainToolBar.Dock = System.Windows.Forms.DockStyle.None;
            this.mainToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendButtonItem,
            this.sendAndKeepOpenButtonItem,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.numberOfCopiesComboBoxItem,
            this.toolStripLabel4,
            this.formatterComboBoxItem,
            this.optionsDropDownMenuItem});
            this.mainToolBar.Location = new System.Drawing.Point(3, 0);
            this.mainToolBar.Name = "mainToolBar";
            this.mainToolBar.Size = new System.Drawing.Size(567, 25);
            this.mainToolBar.TabIndex = 0;
            // 
            // sendButtonItem
            // 
            this.sendButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("sendButtonItem.Image")));
            this.sendButtonItem.Name = "sendButtonItem";
            this.sendButtonItem.Size = new System.Drawing.Size(49, 22);
            this.sendButtonItem.Text = "&Send";
            this.sendButtonItem.Click += new System.EventHandler(this.sendButtonItem_Activate);
            // 
            // sendAndKeepOpenButtonItem
            // 
            this.sendAndKeepOpenButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("sendAndKeepOpenButtonItem.Image")));
            this.sendAndKeepOpenButtonItem.Name = "sendAndKeepOpenButtonItem";
            this.sendAndKeepOpenButtonItem.Size = new System.Drawing.Size(133, 22);
            this.sendAndKeepOpenButtonItem.Text = "Send and &Keep Open";
            this.sendAndKeepOpenButtonItem.Click += new System.EventHandler(this.sendButtonItem_Activate);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripLabel3.Enabled = false;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel3.Text = "Copies";
            // 
            // numberOfCopiesComboBoxItem
            // 
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
            this.numberOfCopiesComboBoxItem.Name = "numberOfCopiesComboBoxItem";
            this.numberOfCopiesComboBoxItem.Size = new System.Drawing.Size(75, 25);
            this.numberOfCopiesComboBoxItem.Text = "1";
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Enabled = false;
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(59, 22);
            this.toolStripLabel4.Text = "Formatter";
            // 
            // formatterComboBoxItem
            // 
            this.formatterComboBoxItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.formatterComboBoxItem.Name = "formatterComboBoxItem";
            this.formatterComboBoxItem.Size = new System.Drawing.Size(121, 25);
            // 
            // optionsDropDownMenuItem
            // 
            this.optionsDropDownMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.optionsDropDownMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsPriorityMenuItem,
            this.toolStripMenuItem1,
            this.optionsRecoverableMenuItem,
            this.optionsUseAuthenticationMenuItem,
            this.optionsUseDeadLetterQueueMenuItem,
            this.optionsUseEncryptionMenuItem,
            this.optionsUseJournalQueueMenuItem,
            this.optionsUseTracingMenuItem,
            this.toolStripMenuItem2,
            this.optionsStreamDirectlyMenuItem});
            this.optionsDropDownMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.optionsDropDownMenuItem.Name = "optionsDropDownMenuItem";
            this.optionsDropDownMenuItem.Size = new System.Drawing.Size(65, 22);
            this.optionsDropDownMenuItem.Text = "&Options";
            // 
            // optionsPriorityMenuItem
            // 
            this.optionsPriorityMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.priorityHighestMenuItem,
            this.priorityVeryHighMenuItem,
            this.priorityHighMenuItem,
            this.priorityAboveNormalMenuItem,
            this.priorityNormalMenuItem,
            this.priorityLowMenuItem,
            this.priorityVeryLowMenuItem,
            this.priorityLowestMenuItem});
            this.optionsPriorityMenuItem.Name = "optionsPriorityMenuItem";
            this.optionsPriorityMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsPriorityMenuItem.Text = "Priority";
            // 
            // priorityHighestMenuItem
            // 
            this.priorityHighestMenuItem.Name = "priorityHighestMenuItem";
            this.priorityHighestMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityHighestMenuItem.Text = "Highest";
            this.priorityHighestMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityVeryHighMenuItem
            // 
            this.priorityVeryHighMenuItem.Name = "priorityVeryHighMenuItem";
            this.priorityVeryHighMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityVeryHighMenuItem.Text = "Very High";
            this.priorityVeryHighMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityHighMenuItem
            // 
            this.priorityHighMenuItem.Name = "priorityHighMenuItem";
            this.priorityHighMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityHighMenuItem.Text = "High";
            this.priorityHighMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityAboveNormalMenuItem
            // 
            this.priorityAboveNormalMenuItem.Name = "priorityAboveNormalMenuItem";
            this.priorityAboveNormalMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityAboveNormalMenuItem.Text = "Above Normal";
            this.priorityAboveNormalMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityNormalMenuItem
            // 
            this.priorityNormalMenuItem.Name = "priorityNormalMenuItem";
            this.priorityNormalMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityNormalMenuItem.Text = "Normal";
            this.priorityNormalMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityLowMenuItem
            // 
            this.priorityLowMenuItem.Name = "priorityLowMenuItem";
            this.priorityLowMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityLowMenuItem.Text = "Low";
            this.priorityLowMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityVeryLowMenuItem
            // 
            this.priorityVeryLowMenuItem.Name = "priorityVeryLowMenuItem";
            this.priorityVeryLowMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityVeryLowMenuItem.Text = "Very Low";
            this.priorityVeryLowMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // priorityLowestMenuItem
            // 
            this.priorityLowestMenuItem.Name = "priorityLowestMenuItem";
            this.priorityLowestMenuItem.Size = new System.Drawing.Size(152, 22);
            this.priorityLowestMenuItem.Text = "Lowest";
            this.priorityLowestMenuItem.Click += new System.EventHandler(this.priorityMenuItem_Activate);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(191, 6);
            // 
            // optionsRecoverableMenuItem
            // 
            this.optionsRecoverableMenuItem.Name = "optionsRecoverableMenuItem";
            this.optionsRecoverableMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsRecoverableMenuItem.Text = "Recoverable";
            this.optionsRecoverableMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // optionsUseAuthenticationMenuItem
            // 
            this.optionsUseAuthenticationMenuItem.Name = "optionsUseAuthenticationMenuItem";
            this.optionsUseAuthenticationMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsUseAuthenticationMenuItem.Text = "Use Authentication";
            this.optionsUseAuthenticationMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // optionsUseDeadLetterQueueMenuItem
            // 
            this.optionsUseDeadLetterQueueMenuItem.Name = "optionsUseDeadLetterQueueMenuItem";
            this.optionsUseDeadLetterQueueMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsUseDeadLetterQueueMenuItem.Text = "Use Dead Letter Queue";
            this.optionsUseDeadLetterQueueMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // optionsUseEncryptionMenuItem
            // 
            this.optionsUseEncryptionMenuItem.Name = "optionsUseEncryptionMenuItem";
            this.optionsUseEncryptionMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsUseEncryptionMenuItem.Text = "Use Encryption";
            this.optionsUseEncryptionMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // optionsUseJournalQueueMenuItem
            // 
            this.optionsUseJournalQueueMenuItem.Name = "optionsUseJournalQueueMenuItem";
            this.optionsUseJournalQueueMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsUseJournalQueueMenuItem.Text = "Use Journal Queue";
            this.optionsUseJournalQueueMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // optionsUseTracingMenuItem
            // 
            this.optionsUseTracingMenuItem.Name = "optionsUseTracingMenuItem";
            this.optionsUseTracingMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsUseTracingMenuItem.Text = "Use Tracing";
            this.optionsUseTracingMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(191, 6);
            // 
            // optionsStreamDirectlyMenuItem
            // 
            this.optionsStreamDirectlyMenuItem.Name = "optionsStreamDirectlyMenuItem";
            this.optionsStreamDirectlyMenuItem.Size = new System.Drawing.Size(194, 22);
            this.optionsStreamDirectlyMenuItem.Text = "Stream Directly";
            this.optionsStreamDirectlyMenuItem.Click += new System.EventHandler(this.optionsMenuItem_Activate);
            // 
            // NewMessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 446);
            this.Controls.Add(this.toolStripContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewMessageForm";
            this.Text = "New Message";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.formPanel.ResumeLayout(false);
            this.formPanel.PerformLayout();
            this.newMessagePanel.ResumeLayout(false);
            this.newMessagePanel.PerformLayout();
            this.sourcePanel.ResumeLayout(false);
            this.existingMessagePanel.ResumeLayout(false);
            this.mainToolBar.ResumeLayout(false);
            this.mainToolBar.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip mainToolBar;
        private System.Windows.Forms.Panel formPanel;
        private System.Windows.Forms.Panel newMessagePanel;
        private System.Windows.Forms.TextBox textSourceTextBox;
        private System.Windows.Forms.TextBox fileSourceTextBox;
        private System.Windows.Forms.Button fileSourceBrowseButton;
        private System.Windows.Forms.Panel sourcePanel;
        private System.Windows.Forms.CheckBox xmlCheckBox;
        private System.Windows.Forms.Label sourceLabel;
        private System.Windows.Forms.RadioButton fileSourceRadioButton;
        private System.Windows.Forms.RadioButton textSourceRadioButton;
        private System.Windows.Forms.Panel existingMessagePanel;
        private System.Windows.Forms.Label existingMessagesLabel;
        private System.Windows.Forms.ListView messagesListView;
        private System.Windows.Forms.Panel _messageViewerHostPanel;
        private System.Windows.Forms.ListView recipientsListView;
        private System.Windows.Forms.TextBox labelTextBox;
        private System.Windows.Forms.Label labelLabel;
        private System.Windows.Forms.Button recipientsButton;
        private System.Windows.Forms.ToolStripLabel sendButtonItem;
        private System.Windows.Forms.ToolStripLabel sendAndKeepOpenButtonItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox formatterComboBoxItem;
        private System.Windows.Forms.ToolStripSplitButton optionsDropDownMenuItem;
        private System.Windows.Forms.ToolStripComboBox numberOfCopiesComboBoxItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripMenuItem optionsPriorityMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityHighestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityVeryHighMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityHighMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityAboveNormalMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityNormalMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityLowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityVeryLowMenuItem;
        private System.Windows.Forms.ToolStripMenuItem priorityLowestMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsRecoverableMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsUseAuthenticationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsUseDeadLetterQueueMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsUseEncryptionMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsUseJournalQueueMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsUseTracingMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem optionsStreamDirectlyMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
    }
}