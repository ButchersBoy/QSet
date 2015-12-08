using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Messaging;
using System.Windows.Forms;
using System.Xml;
using TD.SandBar;
using Mulholland.Core.Xml;
using Mulholland.QSet.Model;
using Mulholland.QSet.Resources;
using Mulholland.WinForms.Controls;

namespace Mulholland.QSet.Application.Controls
{
	/// <summary>
	/// Summary description for MessageViewer.
	/// </summary>
	internal class MessageViewer : System.Windows.Forms.UserControl
	{
		private System.Messaging.Message _message;		
		private Transformer _transfomer = null;
		private string _currentXsltPath = null;
		private string _messageHtml = null;
		private string _defaultHtml;
		private bool _loadingNewHtml = false;
		private Licensing.License _license;
		private System.Windows.Forms.Panel mainPanel;
		private TD.SandBar.ToolBar toolBar;
		private TD.SandBar.ButtonItem dataViewButtonItem;
		private TD.SandBar.ButtonItem xsltViewButtonItem;
		private TD.SandBar.ButtonItem binaryViewButtonItem;
		private Mulholland.WinForms.Controls.BinaryViewer binaryViewer;
		private Mulholland.WinForms.Controls.XmlViewer messageXmlViewer;
        private System.Windows.Forms.RichTextBox messageRichTextBox;
		private TD.SandBar.ButtonItem saveButtonItem;
        private WebBrowser webBrowser1;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MessageViewer(Licensing.License license)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_license = license;
			_message = null;			
			messageXmlViewer.Dock = DockStyle.Fill;
			binaryViewer.Dock = DockStyle.Fill;		
			webBrowser1.Dock = DockStyle.Fill;
			messageRichTextBox.Dock = DockStyle.Fill;

			_defaultHtml = Documents.MessageViewerDefaultPage();
			
			SizePanelForBorder(mainPanel);		
			
			ConfigureView();
			ConfigureButtons();

			base.Load += new EventHandler(MessageViewer_Load);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageViewer));
            this.mainPanel = new System.Windows.Forms.Panel();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.binaryViewer = new Mulholland.WinForms.Controls.BinaryViewer();
            this.messageXmlViewer = new Mulholland.WinForms.Controls.XmlViewer();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.toolBar = new TD.SandBar.ToolBar();
            this.dataViewButtonItem = new TD.SandBar.ButtonItem();
            this.xsltViewButtonItem = new TD.SandBar.ButtonItem();
            this.binaryViewButtonItem = new TD.SandBar.ButtonItem();
            this.saveButtonItem = new TD.SandBar.ButtonItem();
            this.mainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.mainPanel.Controls.Add(this.webBrowser1);
            this.mainPanel.Controls.Add(this.binaryViewer);
            this.mainPanel.Controls.Add(this.messageXmlViewer);
            this.mainPanel.Controls.Add(this.messageRichTextBox);
            this.mainPanel.Controls.Add(this.toolBar);
            this.mainPanel.Location = new System.Drawing.Point(40, 24);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(544, 480);
            this.mainPanel.TabIndex = 6;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(276, 68);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(300, 150);
            this.webBrowser1.TabIndex = 7;
            // 
            // binaryViewer
            // 
            this.binaryViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.binaryViewer.BorderStyle = Mulholland.WinForms.Controls.SimpleBorderStyle.None;
            this.binaryViewer.Location = new System.Drawing.Point(72, 276);
            this.binaryViewer.Name = "binaryViewer";
            this.binaryViewer.Size = new System.Drawing.Size(268, 116);
            this.binaryViewer.TabIndex = 9;
            // 
            // messageXmlViewer
            // 
            this.messageXmlViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.messageXmlViewer.BorderStyle = Mulholland.WinForms.Controls.SimpleBorderStyle.None;
            this.messageXmlViewer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageXmlViewer.Location = new System.Drawing.Point(52, 56);
            this.messageXmlViewer.Name = "messageXmlViewer";
            this.messageXmlViewer.Size = new System.Drawing.Size(168, 72);
            this.messageXmlViewer.TabIndex = 6;
            this.messageXmlViewer.TabWidth = 6;
            this.messageXmlViewer.Xml = null;
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageRichTextBox.Location = new System.Drawing.Point(80, 152);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.ReadOnly = true;
            this.messageRichTextBox.Size = new System.Drawing.Size(120, 104);
            this.messageRichTextBox.TabIndex = 8;
            this.messageRichTextBox.Text = "";
            // 
            // toolBar
            // 
            this.toolBar.Guid = new System.Guid("64c2e04c-5d5c-42d1-b60b-5ae6c5377653");
            this.toolBar.Items.AddRange(new TD.SandBar.ToolbarItemBase[] {
            this.dataViewButtonItem,
            this.xsltViewButtonItem,
            this.binaryViewButtonItem,
            this.saveButtonItem});
            this.toolBar.Location = new System.Drawing.Point(0, 0);
            this.toolBar.Name = "toolBar";
            this.toolBar.Size = new System.Drawing.Size(544, 24);
            this.toolBar.TabIndex = 4;
            this.toolBar.Text = "Main";
            // 
            // dataViewButtonItem
            // 
            this.dataViewButtonItem.Checked = true;
            this.dataViewButtonItem.Icon = ((System.Drawing.Icon)(resources.GetObject("dataViewButtonItem.Icon")));
            this.dataViewButtonItem.ToolTipText = "Data View";
            this.dataViewButtonItem.Activate += new System.EventHandler(this.viewButtonItem_Activate);
            // 
            // xsltViewButtonItem
            // 
            this.xsltViewButtonItem.Icon = ((System.Drawing.Icon)(resources.GetObject("xsltViewButtonItem.Icon")));
            this.xsltViewButtonItem.ToolTipText = "Transform View";
            this.xsltViewButtonItem.Activate += new System.EventHandler(this.viewButtonItem_Activate);
            // 
            // binaryViewButtonItem
            // 
            this.binaryViewButtonItem.Icon = ((System.Drawing.Icon)(resources.GetObject("binaryViewButtonItem.Icon")));
            this.binaryViewButtonItem.ToolTipText = "Binary View";
            this.binaryViewButtonItem.Activate += new System.EventHandler(this.viewButtonItem_Activate);
            // 
            // saveButtonItem
            // 
            this.saveButtonItem.BeginGroup = true;
            this.saveButtonItem.Icon = ((System.Drawing.Icon)(resources.GetObject("saveButtonItem.Icon")));
            this.saveButtonItem.Activate += new System.EventHandler(this.saveButtonItem_Activate);
            // 
            // MessageViewer
            // 
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.mainPanel);
            this.Name = "MessageViewer";
            this.Size = new System.Drawing.Size(600, 524);
            this.mainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Gets the current message displayed by the viewer
		/// </summary>
		public System.Messaging.Message Message
		{			
			get
			{				
				return _message;
			}
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
				webBrowser1.Size = new Size(Width, Height);	//extra call as Fill dockstyle not working until form resized
			}
		}


		/// <summary>
		/// Sizes a control to give the appearance of a border.
		/// </summary>
		/// <param name="control"></param>
		private void SizePanelForBorder(Control control)
		{
			control.Left = 1;
			control.Top = 1;
			control.Width = Width - 2;
			control.Height = Height - 2;
			control.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right);
		}


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
					catch {}
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
			catch {}

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
						message.BodyStream.Position=0;									
						StreamReader sr = new StreamReader(message.BodyStream);	//do not dispose this stream else the underlying stream will close				
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
			catch {}

			return result;
		}


		private void messageRichTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		
		private void viewButtonItem_Activate(object sender, System.EventArgs e)
		{
			
			foreach (ButtonItem button in toolBar.Items)
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
			catch {}
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
