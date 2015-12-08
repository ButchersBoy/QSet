using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for AboutForm.
	/// </summary>
	public class AboutForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox mainPictureBox;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.LinkLabel mulhollandLinkLabel;
		private System.Windows.Forms.LinkLabel qSetLinkLabel;
		private System.Windows.Forms.Label versionLabel;
        private Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public AboutForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			versionLabel.Text = System.Windows.Forms.Application.ProductVersion;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.okButton = new System.Windows.Forms.Button();
            this.mulhollandLinkLabel = new System.Windows.Forms.LinkLabel();
            this.qSetLinkLabel = new System.Windows.Forms.LinkLabel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
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
            this.mainPictureBox.TabIndex = 0;
            this.mainPictureBox.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(244, 212);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(68, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "&OK";
            // 
            // mulhollandLinkLabel
            // 
            this.mulhollandLinkLabel.BackColor = System.Drawing.Color.OldLace;
            this.mulhollandLinkLabel.Location = new System.Drawing.Point(12, 160);
            this.mulhollandLinkLabel.Name = "mulhollandLinkLabel";
            this.mulhollandLinkLabel.Size = new System.Drawing.Size(160, 16);
            this.mulhollandLinkLabel.TabIndex = 2;
            this.mulhollandLinkLabel.TabStop = true;
            this.mulhollandLinkLabel.Text = "www.mulhollandsoftware.com";
            this.mulhollandLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.mulhollandLinkLabel_LinkClicked);
            // 
            // qSetLinkLabel
            // 
            this.qSetLinkLabel.BackColor = System.Drawing.Color.OldLace;
            this.qSetLinkLabel.Location = new System.Drawing.Point(12, 176);
            this.qSetLinkLabel.Name = "qSetLinkLabel";
            this.qSetLinkLabel.Size = new System.Drawing.Size(160, 16);
            this.qSetLinkLabel.TabIndex = 3;
            this.qSetLinkLabel.TabStop = true;
            this.qSetLinkLabel.Text = "qset@mulhollandsoftware.com";
            this.qSetLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.qSetLinkLabel_LinkClicked);
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.OldLace;
            this.versionLabel.Location = new System.Drawing.Point(168, 176);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(140, 12);
            this.versionLabel.TabIndex = 4;
            this.versionLabel.Text = "Version";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(296, 23);
            this.label1.TabIndex = 5;
            this.label1.Text = "QSet Copyright © 2004-2011 Mulholland Software Ltd";
            // 
            // AboutForm
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.CancelButton = this.okButton;
            this.ClientSize = new System.Drawing.Size(322, 244);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.qSetLinkLabel);
            this.Controls.Add(this.mulhollandLinkLabel);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.mainPictureBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Q Set";
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		private void mulhollandLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("http://www.mulhollandsoftware.com");
			}
			catch {}
		}

		private void qSetLinkLabel_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process.Start("mailto:qset@mulhollandsoftware.com");
			}
			catch {}
		}
	}
}
