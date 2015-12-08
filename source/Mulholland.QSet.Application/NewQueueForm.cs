using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Messaging;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for NewQueueForm.
	/// </summary>
	public class NewQueueForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox computerTextBox;
		private System.Windows.Forms.TextBox queueNameTextBox;
		private System.Windows.Forms.CheckBox privateCheckBox;
		private System.Windows.Forms.CheckBox transactionalCheckBox;
		private System.Windows.Forms.Label computerLabel;
		private System.Windows.Forms.Label queueNameLabel;
		private System.Windows.Forms.Label headerLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.CheckBox localCheckBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NewQueueForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewQueueForm));
			this.computerTextBox = new System.Windows.Forms.TextBox();
			this.queueNameTextBox = new System.Windows.Forms.TextBox();
			this.privateCheckBox = new System.Windows.Forms.CheckBox();
			this.transactionalCheckBox = new System.Windows.Forms.CheckBox();
			this.computerLabel = new System.Windows.Forms.Label();
			this.queueNameLabel = new System.Windows.Forms.Label();
			this.headerLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.localCheckBox = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// computerTextBox
			// 
			this.computerTextBox.Location = new System.Drawing.Point(88, 52);
			this.computerTextBox.Name = "computerTextBox";
			this.computerTextBox.Size = new System.Drawing.Size(184, 21);
			this.computerTextBox.TabIndex = 3;
			this.computerTextBox.Text = "";
			// 
			// queueNameTextBox
			// 
			this.queueNameTextBox.Location = new System.Drawing.Point(88, 100);
			this.queueNameTextBox.Name = "queueNameTextBox";
			this.queueNameTextBox.Size = new System.Drawing.Size(184, 21);
			this.queueNameTextBox.TabIndex = 6;
			this.queueNameTextBox.Text = "";
			// 
			// privateCheckBox
			// 
			this.privateCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.privateCheckBox.Location = new System.Drawing.Point(12, 80);
			this.privateCheckBox.Name = "privateCheckBox";
			this.privateCheckBox.Size = new System.Drawing.Size(92, 16);
			this.privateCheckBox.TabIndex = 4;
			this.privateCheckBox.Text = "&Private:";			
			// 
			// transactionalCheckBox
			// 
			this.transactionalCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.transactionalCheckBox.Location = new System.Drawing.Point(12, 128);
			this.transactionalCheckBox.Name = "transactionalCheckBox";
			this.transactionalCheckBox.Size = new System.Drawing.Size(92, 20);
			this.transactionalCheckBox.TabIndex = 7;
			this.transactionalCheckBox.Text = "&Transactional:";
			// 
			// computerLabel
			// 
			this.computerLabel.Location = new System.Drawing.Point(12, 56);
			this.computerLabel.Name = "computerLabel";
			this.computerLabel.Size = new System.Drawing.Size(60, 16);
			this.computerLabel.TabIndex = 2;
			this.computerLabel.Text = "C&omputer:";
			// 
			// queueNameLabel
			// 
			this.queueNameLabel.Location = new System.Drawing.Point(12, 104);
			this.queueNameLabel.Name = "queueNameLabel";
			this.queueNameLabel.Size = new System.Drawing.Size(76, 16);
			this.queueNameLabel.TabIndex = 5;
			this.queueNameLabel.Text = "&Queue Name:";
			// 
			// headerLabel
			// 
			this.headerLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.headerLabel.Location = new System.Drawing.Point(12, 8);
			this.headerLabel.Name = "headerLabel";
			this.headerLabel.Size = new System.Drawing.Size(156, 16);
			this.headerLabel.TabIndex = 0;
			this.headerLabel.Text = "Create New Queue:";
			// 
			// okButton
			// 
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(148, 160);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(60, 23);
			this.okButton.TabIndex = 8;
			this.okButton.Text = "&OK";			
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(212, 160);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(60, 23);
			this.cancelButton.TabIndex = 9;
			this.cancelButton.Text = "&Cancel";
			// 
			// localCheckBox
			// 
			this.localCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.localCheckBox.Location = new System.Drawing.Point(12, 32);
			this.localCheckBox.Name = "localCheckBox";
			this.localCheckBox.Size = new System.Drawing.Size(92, 16);
			this.localCheckBox.TabIndex = 1;
			this.localCheckBox.Text = "&Local:";
			this.localCheckBox.CheckedChanged += new System.EventHandler(this.localCheckBox_CheckedChanged);
			// 
			// NewQueueForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(282, 192);
			this.Controls.Add(this.localCheckBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.headerLabel);
			this.Controls.Add(this.queueNameTextBox);
			this.Controls.Add(this.computerTextBox);
			this.Controls.Add(this.queueNameLabel);
			this.Controls.Add(this.computerLabel);
			this.Controls.Add(this.transactionalCheckBox);
			this.Controls.Add(this.privateCheckBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewQueueForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Queue";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.NewQueueForm_Closing);
			this.ResumeLayout(false);

		}
		#endregion

		private bool ValidateForm(out string queuePath)
		{
			bool result = false;
			queuePath = null;

			TextBox invalidControl = null;
			string errorMsg = null;

			if (computerTextBox.Text.Length == 0 && localCheckBox.Checked == false)
			{
				errorMsg = "Computer";
				invalidControl = computerTextBox;
			}
			else if (queueNameTextBox.Text.Length == 0)
			{
				errorMsg = "Queue Name";
				invalidControl = queueNameTextBox;
			}

			if (invalidControl != null)
			{
				MessageBox.Show(
					this, 
					string.Format("Please enter a valid {0}.", errorMsg), 
					this.Text, 
					MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				queuePath = localCheckBox.Checked ? "." : computerTextBox.Text;

				if (privateCheckBox.Checked)
					queuePath += @"\private$";

				queuePath += "\\" + queueNameTextBox.Text;
				result = true;
			}

			return result;
		}


		private bool CreateQueue(string queuePath)
		{
			bool result = false;

			try
			{
				//TODO this should use process visualisation.  it may have to be moved into a manager
				MessageQueue.Create(queuePath, transactionalCheckBox.Checked);

				result = true;
			}
			catch (Exception exc)
			{
				MessageBox.Show(
					this, 
					string.Format("Unable to create queue:\n\n{0}", exc.Message), 
					this.Text, 
					MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}

			return result;
		}


		private void ConfigureForm()
		{
			if (localCheckBox.Checked == true)
			{
				computerTextBox.Enabled = false;
				computerTextBox.Text = "";
			}
			else
				computerTextBox.Enabled = true;
		}

		private void localCheckBox_CheckedChanged(object sender, System.EventArgs e)
		{
			ConfigureForm();
		}

		private void NewQueueForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (DialogResult == DialogResult.OK)
			{
				
				string queuePath;
				if (ValidateForm(out queuePath))
					e.Cancel = !CreateQueue(queuePath);	
				else
					e.Cancel = true;
			}
		}
	}
}
