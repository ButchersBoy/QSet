using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Messaging;
using System.Reflection;
using System.Windows.Forms;
using Mulholland.Core;
using Mulholland.QSet.Resources;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Summary description for OptionsDialog.
	/// </summary>
	public class OptionsDialog : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ListView optionPagesListView;
		private System.Windows.Forms.Panel messageBrowsingPanel;
		private System.Windows.Forms.Button messageBrowsingDownButton;
		private System.Windows.Forms.Button messageBrowsingUpButton;

		private UserSettings _settings = null;
		private System.Windows.Forms.ListView messageBrowsingColumnsAvailableListView;

		private int _DIALOG_WIDTH = 580;
		private int _DIALOG_HEIGHT = 392;
		private int _PAGE_PANEL_TOP = 8;
		private int _PAGE_PANEL_LEFT = 180;
		private System.Windows.Forms.Label selectedDisplayPropertiesLabel;
		private System.Windows.Forms.Label availablePropertiesLabel;
		private System.Windows.Forms.Panel generalPanel;
		private System.Windows.Forms.ListView messageBrowsingColumnsSelectedListView;
		private System.Windows.Forms.Label recentlyUsedFileListLabel2;
		private System.Windows.Forms.DomainUpDown recentlyUsedFileListUpDown;
		private System.Windows.Forms.Label recentlyUsedFileListLabel1;				

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public OptionsDialog()
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
			this.optionPagesListView = new System.Windows.Forms.ListView();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.messageBrowsingPanel = new System.Windows.Forms.Panel();
			this.selectedDisplayPropertiesLabel = new System.Windows.Forms.Label();
			this.messageBrowsingColumnsSelectedListView = new System.Windows.Forms.ListView();
			this.availablePropertiesLabel = new System.Windows.Forms.Label();
			this.messageBrowsingColumnsAvailableListView = new System.Windows.Forms.ListView();
			this.messageBrowsingDownButton = new System.Windows.Forms.Button();
			this.messageBrowsingUpButton = new System.Windows.Forms.Button();
			this.generalPanel = new System.Windows.Forms.Panel();
			this.recentlyUsedFileListLabel2 = new System.Windows.Forms.Label();
			this.recentlyUsedFileListUpDown = new System.Windows.Forms.DomainUpDown();
			this.recentlyUsedFileListLabel1 = new System.Windows.Forms.Label();
			this.messageBrowsingPanel.SuspendLayout();
			this.generalPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// optionPagesListView
			// 
			this.optionPagesListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.optionPagesListView.Location = new System.Drawing.Point(8, 8);
			this.optionPagesListView.Name = "optionPagesListView";
			this.optionPagesListView.Size = new System.Drawing.Size(164, 500);
			this.optionPagesListView.TabIndex = 0;
			this.optionPagesListView.View = System.Windows.Forms.View.List;
			this.optionPagesListView.SelectedIndexChanged += new System.EventHandler(this.optionPagesListView_SelectedIndexChanged);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(408, 524);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 1;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(488, 524);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "&Cancel";
			// 
			// messageBrowsingPanel
			// 
			this.messageBrowsingPanel.Controls.Add(this.selectedDisplayPropertiesLabel);
			this.messageBrowsingPanel.Controls.Add(this.messageBrowsingColumnsSelectedListView);
			this.messageBrowsingPanel.Controls.Add(this.availablePropertiesLabel);
			this.messageBrowsingPanel.Controls.Add(this.messageBrowsingColumnsAvailableListView);
			this.messageBrowsingPanel.Controls.Add(this.messageBrowsingDownButton);
			this.messageBrowsingPanel.Controls.Add(this.messageBrowsingUpButton);
			this.messageBrowsingPanel.Location = new System.Drawing.Point(180, 8);
			this.messageBrowsingPanel.Name = "messageBrowsingPanel";
			this.messageBrowsingPanel.Size = new System.Drawing.Size(384, 314);
			this.messageBrowsingPanel.TabIndex = 4;
			this.messageBrowsingPanel.Tag = "";
			// 
			// selectedDisplayPropertiesLabel
			// 
			this.selectedDisplayPropertiesLabel.Location = new System.Drawing.Point(4, 4);
			this.selectedDisplayPropertiesLabel.Name = "selectedDisplayPropertiesLabel";
			this.selectedDisplayPropertiesLabel.Size = new System.Drawing.Size(180, 16);
			this.selectedDisplayPropertiesLabel.TabIndex = 11;
			this.selectedDisplayPropertiesLabel.Text = "Selected Display Properties:";
			// 
			// messageBrowsingColumnsSelectedListView
			// 
			this.messageBrowsingColumnsSelectedListView.Location = new System.Drawing.Point(4, 20);
			this.messageBrowsingColumnsSelectedListView.Name = "messageBrowsingColumnsSelectedListView";
			this.messageBrowsingColumnsSelectedListView.Size = new System.Drawing.Size(348, 136);
			this.messageBrowsingColumnsSelectedListView.TabIndex = 10;
			this.messageBrowsingColumnsSelectedListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.messageBrowsingColumnsSelectedListView_ItemCheck);
			// 
			// availablePropertiesLabel
			// 
			this.availablePropertiesLabel.Location = new System.Drawing.Point(8, 164);
			this.availablePropertiesLabel.Name = "availablePropertiesLabel";
			this.availablePropertiesLabel.Size = new System.Drawing.Size(160, 16);
			this.availablePropertiesLabel.TabIndex = 9;
			this.availablePropertiesLabel.Text = "Available Properties:";
			// 
			// messageBrowsingColumnsAvailableListView
			// 
			this.messageBrowsingColumnsAvailableListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.messageBrowsingColumnsAvailableListView.Location = new System.Drawing.Point(4, 180);
			this.messageBrowsingColumnsAvailableListView.Name = "messageBrowsingColumnsAvailableListView";
			this.messageBrowsingColumnsAvailableListView.Size = new System.Drawing.Size(376, 132);
			this.messageBrowsingColumnsAvailableListView.TabIndex = 8;
			this.messageBrowsingColumnsAvailableListView.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.messageBrowsingColumnsAvailableListView_ItemCheck);
			// 
			// messageBrowsingDownButton
			// 
			this.messageBrowsingDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.messageBrowsingDownButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.messageBrowsingDownButton.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.messageBrowsingDownButton.Location = new System.Drawing.Point(356, 51);
			this.messageBrowsingDownButton.Name = "messageBrowsingDownButton";
			this.messageBrowsingDownButton.Size = new System.Drawing.Size(20, 17);
			this.messageBrowsingDownButton.TabIndex = 5;
			this.messageBrowsingDownButton.Text = "▼";
			this.messageBrowsingDownButton.Click += new System.EventHandler(this.messageBrowsingUpDownButton_Click);
			// 
			// messageBrowsingUpButton
			// 
			this.messageBrowsingUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.messageBrowsingUpButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.messageBrowsingUpButton.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.messageBrowsingUpButton.ForeColor = System.Drawing.SystemColors.ControlDark;
			this.messageBrowsingUpButton.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			this.messageBrowsingUpButton.Location = new System.Drawing.Point(356, 36);
			this.messageBrowsingUpButton.Name = "messageBrowsingUpButton";
			this.messageBrowsingUpButton.Size = new System.Drawing.Size(20, 16);
			this.messageBrowsingUpButton.TabIndex = 6;
			this.messageBrowsingUpButton.Text = "▲";
			this.messageBrowsingUpButton.Click += new System.EventHandler(this.messageBrowsingUpDownButton_Click);
			// 
			// generalPanel
			// 
			this.generalPanel.Controls.Add(this.recentlyUsedFileListLabel2);
			this.generalPanel.Controls.Add(this.recentlyUsedFileListUpDown);
			this.generalPanel.Controls.Add(this.recentlyUsedFileListLabel1);
			this.generalPanel.Location = new System.Drawing.Point(16, 352);
			this.generalPanel.Name = "generalPanel";
			this.generalPanel.Size = new System.Drawing.Size(384, 314);
			this.generalPanel.TabIndex = 5;
			this.generalPanel.Tag = "";
			// 
			// recentlyUsedFileListLabel2
			// 
			this.recentlyUsedFileListLabel2.Location = new System.Drawing.Point(160, 16);
			this.recentlyUsedFileListLabel2.Name = "recentlyUsedFileListLabel2";
			this.recentlyUsedFileListLabel2.Size = new System.Drawing.Size(52, 16);
			this.recentlyUsedFileListLabel2.TabIndex = 2;
			this.recentlyUsedFileListLabel2.Text = "entries";
			// 
			// recentlyUsedFileListUpDown
			// 
			this.recentlyUsedFileListUpDown.Location = new System.Drawing.Point(120, 12);
			this.recentlyUsedFileListUpDown.Name = "recentlyUsedFileListUpDown";
			this.recentlyUsedFileListUpDown.Size = new System.Drawing.Size(36, 21);
			this.recentlyUsedFileListUpDown.TabIndex = 1;
			this.recentlyUsedFileListUpDown.Text = "4";
			this.recentlyUsedFileListUpDown.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.recentlyUsedFileListUpDown_KeyPress);
			this.recentlyUsedFileListUpDown.Validating += new System.ComponentModel.CancelEventHandler(this.recentlyUsedFileListUpDown_Validating);
			// 
			// recentlyUsedFileListLabel1
			// 
			this.recentlyUsedFileListLabel1.Location = new System.Drawing.Point(8, 16);
			this.recentlyUsedFileListLabel1.Name = "recentlyUsedFileListLabel1";
			this.recentlyUsedFileListLabel1.Size = new System.Drawing.Size(116, 16);
			this.recentlyUsedFileListLabel1.TabIndex = 0;
			this.recentlyUsedFileListLabel1.Text = "Recently used file list:";
			// 
			// OptionsDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(570, 555);
			this.Controls.Add(this.generalPanel);
			this.Controls.Add(this.messageBrowsingPanel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.optionPagesListView);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Options";
			this.Load += new System.EventHandler(this.OptionsDialog_Load);
			this.messageBrowsingPanel.ResumeLayout(false);
			this.generalPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public ImageList SmallImageList
		{
			get
			{
				return optionPagesListView.SmallImageList;
			}
			set
			{
				optionPagesListView.SmallImageList = value;
			}
		}


		/// <summary>
		/// Displays the options dialogue.
		/// </summary>
		/// <returns>DialogResult.</returns>
		public DialogResult ShowDialog(UserSettings settings)
		{
			_settings = settings;

			SetupForm();

			return base.ShowDialog();			
		}


		/// <summary>
		/// Displays the options dialogue.
		/// </summary>
		/// <param name="owner">Owner form.</param>
		/// <returns>DialogResult.</returns>
		public DialogResult ShowDialog(IWin32Window owner, UserSettings settings)
		{
			_settings = settings;

			SetupForm();

			return base.ShowDialog(owner);			
		}


		/// <summary>
		/// Configures the form.
		/// </summary>
		private void SetupForm()
		{			
			Size = new Size(_DIALOG_WIDTH, _DIALOG_HEIGHT);

			//set up navigation list view
			optionPagesListView.View = View.SmallIcon;
			optionPagesListView.HideSelection = false;
			optionPagesListView.MultiSelect = false;
			optionPagesListView.Sorting = SortOrder.None;
			optionPagesListView.Items.Clear();
			//add general page item
			ListViewItem pageItem = new ListViewItem(Locale.Terms.GeneralOptionPage, (int)Images.IconType.OptionsTab);
			optionPagesListView.Items.Add(pageItem);			
			pageItem.Tag = generalPanel;
			generalPanel.Tag = pageItem;
			generalPanel.Visible = false;
			//add message browsing page item
			pageItem = new ListViewItem(Locale.Terms.MessageBrowsingOptionPage, (int)Images.IconType.OptionsTab);
			optionPagesListView.Items.Add(pageItem);			
			pageItem.Tag = messageBrowsingPanel;
			messageBrowsingPanel.Tag = pageItem;
			messageBrowsingPanel.Visible = false;
			
			//populate all settings
			SetupGeneralPage();
			SetupMessageBrowserPage();			
		}


		//Displays user current settings in general page.
		private void SetupGeneralPage()
		{
			for (int i = 12; i >= 0; i --)
				recentlyUsedFileListUpDown.Items.Add(i);
			recentlyUsedFileListUpDown.SelectedItem = _settings.RecentFileListMaximumEntries;
		}


		/// <summary>
		/// Display the panel which is currently visible, according to what is selected in the navigation list view.
		/// </summary>
		private void DisplayActivePage()
		{
			if (optionPagesListView.SelectedItems.Count > 0)
			{
				Panel activePagePanel = optionPagesListView.SelectedItems[0].Tag as Panel;
				if (activePagePanel != null)
				{
					activePagePanel.Visible = true;
					activePagePanel.Location = new Point(_PAGE_PANEL_LEFT, _PAGE_PANEL_TOP);
					activePagePanel.BringToFront();
				}
			}
		}


		private void SetMessageBrowserListViewCommonProperties(ListView messageListView)
		{
			messageListView.CheckBoxes = true;
			messageListView.LabelEdit = false;
			messageListView.View = View.Details;
			messageListView.Columns.Add("", messageListView.Width - 25, HorizontalAlignment.Left);
			messageListView.HeaderStyle = ColumnHeaderStyle.None;
			messageListView.Items.Clear();
			messageListView.HideSelection = false;
		}


		/// <summary>
		/// Ppopulates the Message Browsing column list.
		/// </summary>
		private void SetupMessageBrowserPage()
		{			
			//set up control properties
			messageBrowsingUpButton.Tag = true;
			messageBrowsingDownButton.Tag = false;
			SetMessageBrowserListViewCommonProperties(messageBrowsingColumnsAvailableListView);
			SetMessageBrowserListViewCommonProperties(messageBrowsingColumnsSelectedListView);

			//load a list of message filter properties
			MessagePropertyFilter filter = new MessagePropertyFilter();
			PropertyInfo[] propertyInfoArray = filter.GetType().GetProperties();
			foreach (PropertyInfo property in propertyInfoArray)
			{
				if (!Utilities.IsStringInArray(Constants.MessageBrowserColumnExclusionList, property.Name))
				{
					//create new available column list item
					ListViewItem propertyListItem = new ListViewItem(property.Name);								
					messageBrowsingColumnsAvailableListView.Items.Add(propertyListItem);				
				}
			}									

			//auto select properties/columns according to user settings			
			foreach (string columnName in _settings.MessageBrowserColumnListCollection)
			{	
				foreach (ListViewItem checkItem in messageBrowsingColumnsAvailableListView.Items)
				{
					if (checkItem.Text == columnName)
					{
						AddMessageBrowsingColumn(checkItem);
						break;
					}
				}
			}			
		}


		private void optionPagesListView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DisplayActivePage();
		}


		private void OptionsDialog_Load(object sender, System.EventArgs e)
		{
			if (optionPagesListView.SelectedItems.Count == 0)
				optionPagesListView.Items[0].Selected = true;

			DisplayActivePage();
		}

		
		private void AddMessageBrowsingColumn(ListViewItem addItem)
		{			
			ListViewItem selectedDuplicateItem = (ListViewItem)addItem.Clone();
			addItem.Tag = selectedDuplicateItem;						
			selectedDuplicateItem.Tag = addItem;			
			messageBrowsingColumnsSelectedListView.Items.Add(selectedDuplicateItem);			
			addItem.Checked = true;
			selectedDuplicateItem.Checked = true;
		}


		private void RemoveMessageBrowsingColumn(ListViewItem removeItem)
		{
			if (removeItem.Tag != null && removeItem.Tag is ListViewItem)
				messageBrowsingColumnsSelectedListView.Items.Remove((ListViewItem)removeItem.Tag);
			removeItem.Tag = null;
			removeItem.Checked = false;
		}
		

		private void messageBrowsingColumnsAvailableListView_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if (e.NewValue == CheckState.Checked && messageBrowsingColumnsAvailableListView.Items[e.Index].Tag == null)
				AddMessageBrowsingColumn(messageBrowsingColumnsAvailableListView.Items[e.Index]);
			else if (e.NewValue == CheckState.Unchecked && messageBrowsingColumnsAvailableListView.Items[e.Index].Tag != null)
				RemoveMessageBrowsingColumn(messageBrowsingColumnsAvailableListView.Items[e.Index]);
		}


		private void messageBrowsingColumnsSelectedListView_ItemCheck(object sender, System.Windows.Forms.ItemCheckEventArgs e)
		{
			if (e.NewValue == CheckState.Unchecked)
				RemoveMessageBrowsingColumn((ListViewItem)messageBrowsingColumnsSelectedListView.Items[e.Index].Tag);
		}


		private void messageBrowsingUpDownButton_Click(object sender, System.EventArgs e)
		{
			if (messageBrowsingColumnsSelectedListView.SelectedItems.Count == 1)
			{
				ListViewItem moveItem = messageBrowsingColumnsSelectedListView.SelectedItems[0];
				
				int insertPos = -1;
				if (moveItem.Index > 0 && (bool)((Button)sender).Tag == true)				
					insertPos = moveItem.Index - 1;
				else if (moveItem.Index < messageBrowsingColumnsSelectedListView.Items.Count - 1 && (bool)((Button)sender).Tag == false)
					insertPos = moveItem.Index + 1;
				
				if (insertPos != -1)
				{
					moveItem.Remove();
					messageBrowsingColumnsSelectedListView.Items.Insert(insertPos, moveItem);
					moveItem.Selected = true;
				}
			}
		}


		/// <summary>
		/// Updates the memeber level UserSettings object with the users on screen selections.
		/// </summary>
		private void UpdateSettings()
		{
			//general settings
			_settings.RecentFileListMaximumEntries = Convert.ToInt32(recentlyUsedFileListUpDown.SelectedItem);
			
			//message browser settings
			_settings.MessageBrowserColumnListCollection.Clear();
			foreach (ListViewItem selectedMessageBrowserColumnItem in messageBrowsingColumnsSelectedListView.Items)
				_settings.MessageBrowserColumnListCollection.Add(selectedMessageBrowserColumnItem.Text);
		}


		/// <summary>
		/// Validates the users settings
		/// </summary>
		/// <returns></returns>
		private bool ValidateSettings()
		{			
			bool result = false;
			Control invalidControl = null;
			string errorMessage = null;

			//validate general page


			//validate message browsing page
			if (messageBrowsingColumnsSelectedListView.Items.Count == 0)
			{
				invalidControl = messageBrowsingColumnsAvailableListView;
				errorMessage = Locale.UserMessages.OptionsDialog.AtLeastOneMessageBrowsingColumnRequired;
			}

			//navigate to invalid control if necessary
			if (invalidControl != null)
			{
				Control findParentPanel = invalidControl.Parent;

				while (!(findParentPanel is Panel))
					findParentPanel = findParentPanel.Parent;

				((ListViewItem)findParentPanel.Tag).Selected = true;
				DisplayActivePage();
				invalidControl.Focus();

				MessageBox.Show(this, errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
				result = true;

			return result;
		}


		private void okButton_Click(object sender, System.EventArgs e)
		{
			if (ValidateSettings())
			{
				UpdateSettings();
				DialogResult = DialogResult.OK;
				Hide();			
			}
		}


		private void recentlyUsedFileListUpDown_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (!((char.IsControl(e.KeyChar)) || char.IsDigit(e.KeyChar) && recentlyUsedFileListUpDown.Text.Length < 2))
				e.Handled = true;
		}


		private void recentlyUsedFileListUpDown_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (recentlyUsedFileListUpDown.Text.Trim().Length == 0 || 
				Convert.ToInt32(recentlyUsedFileListUpDown.Text) < Convert.ToInt32(recentlyUsedFileListUpDown.Items[recentlyUsedFileListUpDown.Items.Count - 1]) ||
				Convert.ToInt32(recentlyUsedFileListUpDown.Text) > Convert.ToInt32(recentlyUsedFileListUpDown.Items[0]))
			{
				MessageBox.Show(
					this, 
					string.Format(Locale.UserMessages.OptionsDialog.RecentlyUsedFielListLengthInvalid, Convert.ToString(recentlyUsedFileListUpDown.Items[recentlyUsedFileListUpDown.Items.Count - 1]), Convert.ToString(recentlyUsedFileListUpDown.Items[0])),
					Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				e.Cancel = true;
			}
		}
	}
}


