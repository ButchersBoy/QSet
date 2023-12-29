using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Mulholland.WinForms.Controls
{
	/// <summary>
	/// Summary description for BinaryViewer.
	/// </summary>
	public class BinaryViewer : System.Windows.Forms.UserControl
	{
		private System.Windows.Forms.ListView binaryListView;
		private SimpleBorderStyle _borderStyle = SimpleBorderStyle.None;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		/// <summary>
		/// Default constructor.
		/// </summary>
		public BinaryViewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			binaryListView.Left = 1;
			binaryListView.Top = 1;
			binaryListView.Width = Width - 2;
			binaryListView.Height = Height - 2;
			binaryListView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;				
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
			this.binaryListView = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// binaryListView
			// 
			this.binaryListView.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.binaryListView.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.binaryListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.binaryListView.Location = new System.Drawing.Point(48, 56);
			this.binaryListView.Name = "binaryListView";
			this.binaryListView.Size = new System.Drawing.Size(444, 148);
			this.binaryListView.TabIndex = 0;
			this.binaryListView.View = System.Windows.Forms.View.Details;
			// 
			// BinaryViewer
			// 
			this.BackColor = System.Drawing.SystemColors.ControlDark;
			this.Controls.Add(this.binaryListView);
			this.Name = "BinaryViewer";
			this.Size = new System.Drawing.Size(728, 396);
			this.Load += new System.EventHandler(this.BinaryViewer_Load);
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Displays the content of a stream in the binary viewer.
		/// </summary>
		/// <param name="ms"></param>
		public void DisplayStream(MemoryStream ms)
		{
			bool finished = false;

			binaryListView.Items.Clear();			

			long startingStreamPosition = ms.Position;
						
			int totalByteCount = 0;
			while (!finished)
			{
				//read up to 16 bytes to display on a row
				ListViewItem byteRow = null;
				StringBuilder hexBuilder = new StringBuilder(49);
				StringBuilder textBuilder = new StringBuilder(16);
				for (int rowByteCount = 0; rowByteCount < 16; rowByteCount ++)
				{
					int byteCode = ms.ReadByte();
					if (byteCode != -1)
					{
						//create a new row if required
						if (rowByteCount == 0)
						{
							byteRow = new ListViewItem(totalByteCount.ToString("X8"));
						}

						//add the hex to the row
						hexBuilder.Append(byteCode.ToString("X2"));
						if (rowByteCount == 7)
							hexBuilder.Append("  ");
						else
							hexBuilder.Append(" ");

						//add the text to the row
						textBuilder.Append(Convert.ToChar(byteCode));

						totalByteCount ++;
					}
					else
					{
						finished = true;
						break;
					}					
				}

				//add the new row		
        if (byteRow == null)
        {
          continue;
        }

        byteRow.SubItems.Add(hexBuilder.ToString());
				byteRow.SubItems.Add(textBuilder.ToString());
				binaryListView.Items.Add(byteRow);				
			}		
	
			ms.Position = startingStreamPosition;
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
				binaryListView.Location = new Point(1, 1);
				binaryListView.Size = new Size(Width - 2, Height - 2);
				binaryListView.Dock = DockStyle.None;
				binaryListView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Right;
			}
			else
			{
				binaryListView.Dock = DockStyle.Fill;
			}
		}


		private void BinaryViewer_Load(object sender, System.EventArgs e)
		{
			binaryListView.Columns.Add("Address", 100, HorizontalAlignment.Left);
			binaryListView.Columns.Add("Hex", binaryListView.Width - 206, HorizontalAlignment.Left);
			binaryListView.Columns.Add("Text", 100, HorizontalAlignment.Left);
		}
	}
}
