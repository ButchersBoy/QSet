using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for OpenQueueDialog.
	/// </summary>
	public class OpenQueueDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox queueNameTextBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label queueNameLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OpenQueueDialog()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OpenQueueDialog));
			this.queueNameTextBox = new System.Windows.Forms.TextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.queueNameLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// queueNameTextBox
			// 
			this.queueNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.queueNameTextBox.Location = new System.Drawing.Point(4, 24);
			this.queueNameTextBox.Name = "queueNameTextBox";
			this.queueNameTextBox.Size = new System.Drawing.Size(298, 21);
			this.queueNameTextBox.TabIndex = 0;
			this.queueNameTextBox.Text = "";
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(246, 48);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(56, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(186, 48);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(56, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// queueNameLabel
			// 
			this.queueNameLabel.Location = new System.Drawing.Point(8, 8);
			this.queueNameLabel.Name = "queueNameLabel";
			this.queueNameLabel.Size = new System.Drawing.Size(144, 16);
			this.queueNameLabel.TabIndex = 3;
			this.queueNameLabel.Text = "Full Queue Name/Path:";
			// 
			// OpenQueueDialog
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(306, 77);
			this.Controls.Add(this.queueNameLabel);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.queueNameTextBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OpenQueueDialog";
			this.Text = "Open Queue";
			this.ResumeLayout(false);

		}
		#endregion

		private void okButton_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}


		/// <summary>
		/// Gets the entered queue name, or sets a default/
		/// </summary>
		public string QueueName
		{
			get
			{
				return queueNameTextBox.Text;
			}
			set
			{
				queueNameTextBox.Text = value;
			}
		}

	}
}
