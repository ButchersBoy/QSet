using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using Mulholland.Core;

namespace Mulholland.WinForms.Controls
{
	/// <summary>
	/// Displays pretty-printed XML.
	/// </summary>
	public class XmlViewer : System.Windows.Forms.UserControl
	{		
		private string _xml;
		private XmlColours _xmlColours;
		private int _tabWidth = 6;
		private SimpleBorderStyle _borderStyle = SimpleBorderStyle.None;

		private System.Windows.Forms.RichTextBox _xmlRichTextBox;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public XmlViewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			_xml = null;

			_xmlColours.SpecialCharacter = Color.MediumSeaGreen;
			_xmlColours.ElementName = Color.Maroon;
			_xmlColours.AttributeName = Color.DarkOliveGreen;
			_xmlColours.ElementValue = Color.Black;
			_xmlColours.AttributeValue = Color.Black;			

			SetBorder();
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
			this._xmlRichTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// _xmlRichTextBox
			// 
			this._xmlRichTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this._xmlRichTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this._xmlRichTextBox.Location = new System.Drawing.Point(1, 1);
			this._xmlRichTextBox.Name = "_xmlRichTextBox";
			this._xmlRichTextBox.ReadOnly = true;
			this._xmlRichTextBox.Size = new System.Drawing.Size(354, 262);
			this._xmlRichTextBox.TabIndex = 0;
			this._xmlRichTextBox.Text = "";
			// 
			// XmlViewer
			// 
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.Controls.Add(this._xmlRichTextBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "XmlViewer";
			this.Size = new System.Drawing.Size(356, 264);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Gets and sets the currently displayed XML string.
		/// </summary>
		/// <exception cref="XmlException">There is a load or parse error in the XML.</exception>
		[Category("Appearance"), DefaultValue(""),
			Description("The XML to display in the control.")]
		public string Xml
		{
			get
			{
				return _xml;
			}
			set
			{
				//load xml into a doc to validate it
				if (value != null && value.Length > 0)
				{
					XmlDocument xmlDoc = new XmlDocument();
					xmlDoc.LoadXml(value);
				}

				//display the control
				_xml = value;
				if (_xml != null)
					DisplayXml(_xml);
			}
		}


		/// <summary>
		/// Gets and sets the colour settings for the XML output.
		/// </summary>
		[Category("Appearance"), Description("The colour settings for the XML display.")]
		public XmlColours XmlColours
		{
			get
			{
				return _xmlColours;
			}
			set
			{
				_xmlColours = value;
			}
		}


		/// <summary>
		/// Gets or sets the size of a tab, in characters.
		/// </summary>
		[Category("Appearance"), Description("The tab width to use when indenting XML nodes.")]
		public int TabWidth
		{
			get
			{
				return _tabWidth;
			}
			set
			{
				_tabWidth = value;
			}
		}


		/// <summary>
		/// Gets or sets the border style of the control.
		/// </summary>
		[Category("Appearance"), Description("The border style of the control.")]
		public SimpleBorderStyle BorderStyle
		{
			get
			{
				return _borderStyle;
			}
			set
			{
				_borderStyle = value;
				SetBorder();
			}
		}


		/// <summary>
		/// Configures the way the border is displayed.
		/// </summary>
		private void SetBorder()
		{
			if (_borderStyle == SimpleBorderStyle.FixedSingle)
			{
				_xmlRichTextBox.Location = new Point(1, 1);
				_xmlRichTextBox.Size = new Size(Width - 2, Height - 2);
				_xmlRichTextBox.Dock = DockStyle.None;
				_xmlRichTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
			}
			else
			{
				_xmlRichTextBox.Dock = DockStyle.Fill;
			}
		}


		/// <summary>
		/// Displays XML in the main rich text box.
		/// </summary>
		/// <param name="xml">XML to display.</param>
		private void DisplayXml(string xml)
		{
			//clear text
			_xmlRichTextBox.Text = "";						
			
			//stop redrawing
			User32Api.SendMessage(_xmlRichTextBox.Handle, User32Api.WM_SETREDRAW, 0, IntPtr.Zero);

			//if we have some xml, display it
			if (xml != null && xml.Length > 0)
			{
				StringReader sr = null;
				try
				{				
					//create a new bold font
					Font standardFont = _xmlRichTextBox.SelectionFont;
					Font boldFont = new Font(standardFont.Name, standardFont.Size, (FontStyle)((int)standardFont.Style + (int)FontStyle.Bold));

					//load xml into a text reader
					XmlTextReader reader = null;
					sr = new StringReader(xml);
					reader = new XmlTextReader(sr);
					int tabCount = -1;
					XmlNodeType previousNodeType = XmlNodeType.XmlDeclaration;
					bool previousElementEmpty = false;

					//itterate thru the XML, drawing out
					while (reader.Read()) 
					{						
						//figure out if we need to start a new line, and configure the tabcount
						bool isNewLineRequired = false;
						if (previousNodeType == XmlNodeType.EndElement || previousElementEmpty) //(previousElementEmpty && reader.NodeType == XmlNodeType.EndElement)
						{
							tabCount --;
						}
						if (reader.NodeType != XmlNodeType.XmlDeclaration)
						{
							if (reader.IsStartElement())
							{							
								tabCount ++;						
								if (tabCount > 0)
									isNewLineRequired = true;
							} 
							else if (previousNodeType == XmlNodeType.EndElement || previousElementEmpty)									
								isNewLineRequired = true;													

							//if necessary, create a new line
							if (isNewLineRequired)
							{
								_xmlRichTextBox.SelectedText = "\n" + GetIndentString(tabCount);
							}
						}
						previousElementEmpty = false;

						//write the XML node
						switch (reader.NodeType) 
						{
							//TODO validate workings of entities, notations 
							case XmlNodeType.Document:
							case XmlNodeType.DocumentFragment:
							case XmlNodeType.DocumentType:
							case XmlNodeType.Entity:
							case XmlNodeType.EndEntity:
							case XmlNodeType.EntityReference:
							case XmlNodeType.Notation:
							case XmlNodeType.XmlDeclaration:
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;								
								_xmlRichTextBox.SelectedText = string.Format("<{0}/>\n", reader.Value);
								break;

							case XmlNodeType.Element: 								
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;
								_xmlRichTextBox.SelectedText = "<" ;
								_xmlRichTextBox.SelectionColor = _xmlColours.ElementName;
								_xmlRichTextBox.SelectedText = reader.Name;		
								bool isEmptyElement = reader.IsEmptyElement;
								while (reader.MoveToNextAttribute()) 
								{
									_xmlRichTextBox.SelectionColor = _xmlColours.AttributeName;
									_xmlRichTextBox.SelectedText = " " + reader.Name;
									_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;	
									_xmlRichTextBox.SelectedText = "=\"";
									_xmlRichTextBox.SelectionColor = _xmlColours.AttributeValue;	
									_xmlRichTextBox.SelectionFont = boldFont;
									_xmlRichTextBox.SelectedText = reader.Value;
									_xmlRichTextBox.SelectionFont = standardFont;
									_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;		
									_xmlRichTextBox.SelectedText = "\"";
								}
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;
								if (isEmptyElement)
								{
									_xmlRichTextBox.SelectedText = " /";									
									previousElementEmpty = true;
								}
								_xmlRichTextBox.SelectedText = ">";							
								break;

							case XmlNodeType.Text: 
								_xmlRichTextBox.SelectionColor = _xmlColours.ElementValue;
								_xmlRichTextBox.SelectionFont = boldFont;
								_xmlRichTextBox.SelectedText = reader.Value;
								_xmlRichTextBox.SelectionFont = standardFont;
								break;

							case XmlNodeType.EndElement: 			
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;					
								_xmlRichTextBox.SelectedText = "</";
								_xmlRichTextBox.SelectionColor = _xmlColours.ElementName;
								_xmlRichTextBox.SelectedText = reader.Name;
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;	
								_xmlRichTextBox.SelectedText = ">";															
								break;

							case XmlNodeType.CDATA:
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;
								_xmlRichTextBox.SelectedText = "<![CDATA[";
								_xmlRichTextBox.SelectionColor = _xmlColours.ElementValue;
								_xmlRichTextBox.SelectionFont = boldFont;
								_xmlRichTextBox.SelectedText = reader.Value;
								_xmlRichTextBox.SelectionFont = standardFont;
								_xmlRichTextBox.SelectionColor = _xmlColours.SpecialCharacter;
								_xmlRichTextBox.SelectedText = "]]>";
								break;
						}	
			
						//store the current node for later use
						previousNodeType = reader.NodeType;
					}			
		 			
					//tidy up and position the cursor					
					_xmlRichTextBox.SelectedText = "\n";
					_xmlRichTextBox.Select(0,0);
					
				}
				catch 
				{
					_xmlRichTextBox.Text = xml;
				}
				finally
				{
					if (sr != null)
						sr.Close();
				}
			}	

			//stop redrawing
			User32Api.SendMessage(_xmlRichTextBox.Handle, User32Api.WM_SETREDRAW, 1, IntPtr.Zero);			
			_xmlRichTextBox.Refresh();
		}


		/// <summary>
		/// Creates a string containing the specified number of tabs.
		/// </summary>
		/// <param name="tabCount"></param>
		/// <returns></returns>
		private string GetIndentString(int tabCount)
		{
			string result = "";

			for (int i = 1;  i <= tabCount; i ++)				
				result += new string(' ', _tabWidth);

			return result;			
		}

	}

}
