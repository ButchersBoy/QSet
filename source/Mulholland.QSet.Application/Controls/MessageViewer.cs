using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Messaging;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Mulholland.Core.Xml;
using Mulholland.QSet.Application.Licensing;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using WeifenLuo.WinFormsUI.Docking;

namespace Mulholland.QSet.Application.Controls
{
    public partial class MessageViewer : System.Windows.Forms.UserControl
    {
        private System.Messaging.Message _message;
        private Transformer _transfomer = null;
        private string _currentXsltPath = null;
        private string _messageHtml = null;
        private string _defaultHtml;
        private bool _loadingNewHtml = false;
        private Licensing.License _license;

        public MessageViewer()
        {
            InitializeComponent();

            _message = null;
            messageXmlViewer.Dock = DockStyle.Fill;
            binaryViewer.Dock = DockStyle.Fill;
            webBrowser1.Dock = DockStyle.Fill;
            messageRichTextBox.Dock = DockStyle.Fill;

            _defaultHtml = Documents.MessageViewerDefaultPage();

            //SizePanelForBorder(mainPanel);

            ConfigureView();
            ConfigureButtons();

            base.Load += new EventHandler(MessageViewer_Load);
        }

        public System.Messaging.Message Message
        {
            get
            {
                return _message;
            }
        }

        public Licensing.License License
        {
            get { return this._license; }
            set { this._license = value; }
        }
        
        /// <summary>
        /// Displays a new message.
        /// </summary>
        /// <param name="qsetMessageQueue">Queue that the message belongs to.</param>
        /// <param name="message">Message to display.</param>
        public void DisplayMessage(QSetQueueItem qsetQueueItem, System.Messaging.Message message)
        {
            //set the memeber level message reference
            _message = message;

            //check if an xslt is assigned to the queue			
            if (qsetQueueItem.MessageViewerXslt != null && qsetQueueItem.MessageViewerXslt.Length > 0)
            {
                //is this xslt already loaded?
                if (_currentXsltPath != qsetQueueItem.MessageViewerXslt)
                {
                    //load the new xslt
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(new FileStream(qsetQueueItem.MessageViewerXslt, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                        _transfomer = new Transformer(sr.ReadToEnd());
                        _currentXsltPath = qsetQueueItem.MessageViewerXslt;
                    }
                    catch
                    {
                        _currentXsltPath = null;
                        _transfomer = null;
                    }
                    finally
                    {
                        if (sr != null)
                            sr.Close();
                    }
                }
            }
            else
            {
                _currentXsltPath = null;
                _transfomer = null;
            }

            //now display the message
            DoDisplayMessage();
            ConfigureButtons();
        }

        /// <summary>
        /// Configures the tool buttons.
        /// </summary>
        private void ConfigureButtons()
        {
            dataViewButtonItem.Enabled = _message != null;
            xsltViewButtonItem.Enabled = _message != null;
            binaryViewButtonItem.Enabled = _message != null;
            saveButtonItem.Enabled = _message != null;
        }


        /// <summary>
        /// Configures the display according to the users current setting.
        /// </summary>
        private void ConfigureView()
        {
            messageXmlViewer.Visible = false;
            messageRichTextBox.Visible = false;
            webBrowser1.Visible = false;
            binaryViewer.Visible = false;

            if (dataViewButtonItem.Checked)
            {
                messageXmlViewer.Visible = true;
                messageRichTextBox.Visible = true;
            }
            else if (binaryViewButtonItem.Checked)
            {
                binaryViewer.Visible = true;
            }
            else
            {
                webBrowser1.Visible = true;
                webBrowser1.Size = new Size(Width, Height); //extra call as Fill dockstyle not working until form resized
            }
        }


        /// <summary>
        /// Sizes a control to give the appearance of a border.
        /// </summary>
        /// <param name="control"></param>
        //private void SizePanelForBorder(Control control)
        //{
        //    control.Left = 1;
        //    control.Top = 1;
        //    control.Width = Width - 2;
        //    control.Height = Height - 2;
        //    control.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
        //}


        /// <summary>
        /// Displays a message in the viewer.
        /// </summary>
        /// <param name="message">Message to display.</param>
        private void DoDisplayMessage()
        {
            messageRichTextBox.Text = "";
            messageXmlViewer.Xml = "";

            _messageHtml = _defaultHtml;

            //extract the message content
            string messageContent = ExtractMessageContent(_message);

            //set up the data view (text or xml auto detect)
            if (IsXml(messageContent))
            {
                //display the xml content
                messageXmlViewer.Xml = messageContent;
                messageXmlViewer.BringToFront();

                //transform if necessary
                if (_transfomer != null)
                {
                    try
                    {
                        _transfomer.Xml = messageContent;
                        _messageHtml = _transfomer.Transform();
                    }
                    catch { }
                }
            }
            else
            {
                messageRichTextBox.Text = messageContent;
                messageRichTextBox.BringToFront();
            }

            //set up the web browser
            try
            {
                object noValue = System.Reflection.Missing.Value;
                string url = "about:blank";
                _loadingNewHtml = true;
                webBrowser1.Navigate(url);
            }
            catch (Exception exc)
            {
                MessageBox.Show("DisplayMessage error:\n\n" + exc.Message + "\n" + exc.GetType().ToString());
            }

            //set up the binary view
            try
            {
                _message.BodyStream.Position = 0;
                binaryViewer.DisplayStream((MemoryStream)_message.BodyStream);
            }
            catch { }

            ConfigureView();
        }


        /// <summary>
        /// Attempts to extract the content of a message in string format.
        /// </summary>
        /// <param name="message">Message to extract.</param>
        /// <param name="usedMessageFormatterType">Informs which formatter was used to extract the message.</param>
        /// <returns>A string if successful else null.</returns>
        private string ExtractMessageContent(System.Messaging.Message message)
        {
            string result = null;

            //create an array of formatters, ordered as we are going to attempt to use them
            IMessageFormatter[] formatterArray = new IMessageFormatter[3];
            formatterArray[0] = new ActiveXMessageFormatter();
            formatterArray[1] = new XmlMessageFormatter();
            formatterArray[2] = new BinaryMessageFormatter();

            //attempt to read the message body using the different formatters			
            foreach (IMessageFormatter formatter in formatterArray)
            {
                try
                {
                    //attempt to extract the message
                    message.Formatter = formatter;
                    if (message.Formatter is ActiveXMessageFormatter)
                        result = Convert.ToString(message.Body);
                    else
                    {
                        message.BodyStream.Position = 0;
                        StreamReader sr = new StreamReader(message.BodyStream); //do not dispose this stream else the underlying stream will close				
                        result = sr.ReadToEnd();
                    }

                    //message has been successfully extracted (else we would have thrown an exception)					

                    //check the xml formatter has given us valid xml
                    if (!(formatter is XmlMessageFormatter && !IsXml(result)))
                        break;
                }
                catch
                {
                    result = null;
                }
            }
            if (result == null)
                result = Locale.UserMessages.UnableToDisplayBinaryMessage;

            return result;
        }


        /// <summary>
        /// Checks to see if a string is valid XML.
        /// </summary>
        /// <param name="test">The string to test.</param>
        /// <returns>true if the passed value is XML, else false.</returns>
        private bool IsXml(string test)
        {
            bool result = false;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(test);

                result = true;
            }
            catch { }

            return result;
        }


        private void messageRichTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = true;
        }


        private void viewButtonItem_Activate(object sender, System.EventArgs e)
        {

            foreach (ToolStripButton button in toolStrip.Items)
                button.Checked = (button == sender);

            ConfigureView();
        }


        private void webBrowser_NavigateComplete2(object sender, WebBrowserNavigatedEventArgs e)
        {
            try
            {
                if (_loadingNewHtml)
                {
                    var doc = webBrowser1.Document;
                    doc.Write(_messageHtml);
                    _loadingNewHtml = false;
                }
            }
            catch { }
        }

        private void MessageViewer_Load(object sender, EventArgs e)
        {
            Refresh();
        }


        /// <summary>
        /// Prompts the user for a file name, and then saves the message to disk.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButtonItem_Activate(object sender, System.EventArgs e)
        {
            //validate license before saving
            if (_license.ValidateFeatureUse(Licensing.Feature.SaveMessage))
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Save message";
                    saveDialog.Filter = "All files (*.*)|*.*";
                    saveDialog.InitialDirectory = "\\My Documents";
                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        BinaryWriter bw = null;
                        try
                        {
                            _message.BodyStream.Position = 0;
                            BinaryReader br = new BinaryReader(_message.BodyStream);
                            //TODO handle int/long conversion
                            byte[] bodyBytes = br.ReadBytes((int)_message.BodyStream.Length);
                            _message.BodyStream.Position = 0;
                            FileStream fs = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                            bw = new BinaryWriter(fs);
                            bw.Write(bodyBytes);
                            bw.Flush();
                            bw.Close();
                        }
                        catch (Exception exc)
                        {
                            //TODO check if file is readonly & tidy up message box, handle IOException				
                            MessageBox.Show("Unable to save file, " + exc.Message, Locale.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        finally
                        {
                            if (bw != null)
                                bw.Close();
                        }
                    }
                }
            }
        }
    }
}
