using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mulholland.Core;
using Mulholland.QSet.Application.Controls;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;

namespace Mulholland.QSet.Application
{
    internal partial class NewMessageForm : Form
    {
        private enum Mode
        {
            NewMessage,
            ExistingMessage
        }

        private const string _FORMATTER_ACTIVEX = "Active X";
        private const string _FORMATTER_BINARY = "Binary";
        private const string _FORMATTER_XML = "XML";

        private QueueSearchForm QueueSearchForm = null;
        private QueueTaskManager _queueTaskManager;
        private Licensing.License _license;

        private Mode _mode;
        private QSetQueueItem _sourceQueueItem;
        private Mulholland.QSet.Application.Controls.MessageViewer _messageViewer;

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

            _messageViewer = new MessageViewer { License = license };
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

                foreach (ToolStripItem item in optionsPriorityMenuItem.DropDownItems)
                {
                    var menuItem = item as ToolStripMenuItem;
                    if (menuItem != null)
                    {
                        menuItem.Checked = false;
                    }
                }
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
            labelTextBox.Visible = _mode == Mode.NewMessage;
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

        private void SelectMessagePriorityMenuItem(System.Messaging.MessagePriority priority)
        {
            foreach (ToolStripItem item in optionsPriorityMenuItem.DropDownItems)
            {
                var menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                {
                    if ((System.Messaging.MessagePriority)item.Tag == priority)
                        menuItem.Checked = true;
                    else
                        menuItem.Checked = false;
                }
            }
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
            
            foreach (ToolStripItem item in optionsPriorityMenuItem.DropDownItems)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null && menuItem.Checked)
                {
                    result = (System.Messaging.MessagePriority)item.Tag;
                    break;
                }
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

            public FormatterComboItem(string name, MessageFormatterType formatterType)
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
                    for (int i = 0; i < recipientsListView.Items.Count; i++)
                    {
                        messageQueues[i] = (System.Messaging.MessageQueue)((QSetQueueItem)recipientsListView.Items[i].Tag).QSetMessageQueue;
                    }

                    //create a new message, or prepare existing ones, to send to recipients
                    if (_mode == Mode.NewMessage)
                    {
                        //create the message from the releavent source
                        messages = new System.Messaging.Message[1] { new System.Messaging.Message() };
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
                        for (int i = 0; i < messagesListView.Items.Count; i++)
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
            ((ToolStripMenuItem)sender).Checked = !((ToolStripMenuItem)sender).Checked;
            ConfigureControls();
        }


        private void priorityMenuItem_Activate(object sender, System.EventArgs e)
        {
            SelectMessagePriorityMenuItem((System.Messaging.MessagePriority)((ToolStripMenuItem)sender).Tag);
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
