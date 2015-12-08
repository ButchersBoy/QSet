using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Mulholland.WinForms.Controls
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
	public class XmlTextBox : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.RichTextBox xmlRichTextBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public XmlTextBox()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
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
			this.xmlRichTextBox = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// xmlRichTextBox
			// 
			this.xmlRichTextBox.AcceptsTab = true;
			this.xmlRichTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.xmlRichTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.xmlRichTextBox.HideSelection = false;
			this.xmlRichTextBox.Location = new System.Drawing.Point(0, 0);
			this.xmlRichTextBox.Name = "xmlRichTextBox";
			this.xmlRichTextBox.Size = new System.Drawing.Size(328, 276);
			this.xmlRichTextBox.TabIndex = 0;
			this.xmlRichTextBox.Text = "";
			this.xmlRichTextBox.WordWrap = false;
			// 
			// XmlTextBox
			// 
			this.Controls.Add(this.xmlRichTextBox);
			this.Name = "XmlTextBox";
			this.Size = new System.Drawing.Size(328, 276);
			this.ResumeLayout(false);

		}
		#endregion

		
		/// <summary>
		/// Gets ot sets the display text.
		/// </summary>
		public override string Text
		{
			get
			{
				return xmlRichTextBox.Text;	
			}
			set
			{
				xmlRichTextBox.Text = value;
			}
		}


		/// <summary>
		/// Gets the underlying RichTextBox.
		/// </summary>
		public RichTextBox UnderlyingRichTextBox
		{
			get
			{
				return xmlRichTextBox;
			}
		}
	}
}
