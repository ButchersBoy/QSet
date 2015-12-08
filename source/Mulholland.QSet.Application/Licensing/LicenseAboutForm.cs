using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mulholland.QSet.Application.Licensing
{
	/// <summary>
	/// Summary description for LicenseAboutForm.
	/// </summary>
	public class LicenseAboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox mainPictureBox;
		private System.Windows.Forms.LinkLabel licenseLinkLabel;
		private System.Windows.Forms.Button okButton;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		

		public LicenseAboutForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			licenseLinkLabel.Links.Add(58, 4, "http://www.mulhollandsoftware.com/QSet/Licensing.aspx");
			licenseLinkLabel.Links.Add(73, 26, "http://www.mulhollandsoftware.com");
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LicenseAboutForm));
			this.mainPictureBox = new System.Windows.Forms.PictureBox();
			this.licenseLinkLabel = new System.Windows.Forms.LinkLabel();
			this.okButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// mainPictureBox
			// 
			this.mainPictureBox.BackColor = System.Drawing.Color.OldLace;
			this.mainPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.mainPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("mainPictureBox.Image")));
			this.mainPictureBox.Location = new System.Drawing.Point(8, 8);
			this.mainPictureBox.Name = "mainPictureBox";
			this.mainPictureBox.Size = new System.Drawing.Size(304, 196);
			this.mainPictureBox.TabIndex = 1;
			this.mainPictureBox.TabStop = false;
			// 
			// licenseLinkLabel
			// 
			this.licenseLinkLabel.BackColor = System.Drawing.Color.OldLace;
			this.licenseLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(0, 0);
			this.licenseLinkLabel.Location = new System.Drawing.Point(16, 164);
			this.licenseLinkLabel.Name = "licenseLinkLabel";
			this.licenseLinkLabel.Size = new System.Drawing.Size(288, 28);
			this.licenseLinkLabel.TabIndex = 3;
			this.licenseLinkLabel.Text = "To find out more about Q Set licensing via the web, click here, or visit www.mulh" +
				"ollandsoftware.com";
			this.licenseLinkLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.licenseLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.licenseLinkLabel_LinkClicked);
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(244, 212);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(68, 23);
			this.okButton.TabIndex = 4;
			this.okButton.Text = "&OK";
			// 
			// LicenseAboutForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(320, 242);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.licenseLinkLabel);
			this.Controls.Add(this.mainPictureBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LicenseAboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Q Set";
			this.ResumeLayout(false);

		}
		#endregion

		private void licenseLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
			}
			catch {}			
		}
	}
}
