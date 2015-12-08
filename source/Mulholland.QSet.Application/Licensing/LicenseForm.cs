using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Mulholland.QSet.Application.Licensing
{
	/// <summary>
	/// Form fow allowing user to enter an activation key.
	/// </summary>
	public class LicenseForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox activationKeyPart1TextBox;
		private System.Windows.Forms.TextBox activationKeyPart2TextBox;
		private System.Windows.Forms.TextBox activationKeyPart3TextBox;
		private System.Windows.Forms.TextBox activationKeyPart4TextBox;
		private System.Windows.Forms.TextBox registrationEmailAddressTextBox;
		private System.Windows.Forms.Label registrationEmailAddressLabel;
		private System.Windows.Forms.Label activationKeyLabel;
		private System.Windows.Forms.Label headerLabel;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label footerLabel;

		private const int _LICENSE_LENGTH = 16;
		private const int _LICENSE_PART_LENGTH = 4;

		private string _registrationEmail = null;
		private string _activationKey = null;
		private TextBox[] _activationKeyTextBoxes = new TextBox[_LICENSE_LENGTH / _LICENSE_PART_LENGTH];

		/// <summary>
		/// Delegate which form relies on to validate if an activation key is valid when the user clicks the OK button.
		/// </summary>
		public delegate bool IsActivationKeyValid(string registrationEmail, string activationKey);

		private IsActivationKeyValid _isActivationKeyValid;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		/// <summary>
		/// Constructs the form.
		/// </summary>
		/// <param name="isActivationKeyValid">Delegate to method which the form can use to validate an activation key
		/// when the user clicks the OK button.  The form is not capable of validating an activation key, and therefore
		/// this method must implement the required logic.</param>
		public LicenseForm(IsActivationKeyValid isActivationKeyValid)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			if (isActivationKeyValid == null)
				throw new ArgumentNullException("isActivationKeyValid");

			_isActivationKeyValid = isActivationKeyValid;

			_activationKeyTextBoxes[0] = activationKeyPart1TextBox;
			_activationKeyTextBoxes[1] = activationKeyPart2TextBox;
			_activationKeyTextBoxes[2] = activationKeyPart3TextBox;
			_activationKeyTextBoxes[3] = activationKeyPart4TextBox;

			foreach (TextBox activationKeyTextBox in _activationKeyTextBoxes)
				activationKeyTextBox.MaxLength = _LICENSE_PART_LENGTH;
		}


		/// <summary>
		/// Displays the dialog modally.
		/// </summary>
		/// <returns>DialogResult.</returns>
		/// <remarks>Use of this method is preferred over <see cref="Show"/>.</remarks>
		public new DialogResult ShowDialog()
		{
			return ShowDialog(null);
		}


		/// <summary>
		/// Displays the dialog modally.
		/// </summary>
		/// <param name="owner">The window that will own the modal dialog.</param>
		/// <returns>DialogResult.</returns>
		/// <remarks>Use of this method is preferred over <see cref="Show"/>.</remarks>
		public new DialogResult ShowDialog(IWin32Window owner)
		{
			//prepare form
			foreach (Control ctrl in Controls)
				if (ctrl is TextBox)
					ctrl.Text = "";
			
			_registrationEmail = null;
			_activationKey = null;
			
			ConfigureControlsStates();

			//show form
			DialogResult result;
			if (owner != null)
				result = base.ShowDialog(owner);
			else
				result = base.ShowDialog();

			//set result properties
			if (result == DialogResult.OK)
			{
				_registrationEmail = registrationEmailAddressTextBox.Text;
				_activationKey = JoinActivationKey();
			}
			else
			{
				_registrationEmail = null;
				_activationKey = null;
			}

			return result;
		}


		/// <summary>
		/// Gets the last successfully submitted registration email.  Until the users clicks 'OK' 
		/// and submits a valid email & key code, this property will return null.
		/// </summary>
		public string RegistrationEmail
		{
			get
			{
				return _registrationEmail;
			}
		}


		/// <summary>
		/// Gets the last successfully submitted activation key.  Until the users clicks 'OK' 
		/// and submits a valid email & key code, this property will return null.
		/// </summary>
		public string ActivationKey
		{
			get
			{
				return _activationKey;
			}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LicenseForm));
			this.activationKeyPart1TextBox = new System.Windows.Forms.TextBox();
			this.activationKeyPart2TextBox = new System.Windows.Forms.TextBox();
			this.activationKeyPart3TextBox = new System.Windows.Forms.TextBox();
			this.activationKeyPart4TextBox = new System.Windows.Forms.TextBox();
			this.registrationEmailAddressTextBox = new System.Windows.Forms.TextBox();
			this.registrationEmailAddressLabel = new System.Windows.Forms.Label();
			this.activationKeyLabel = new System.Windows.Forms.Label();
			this.headerLabel = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.footerLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// activationKeyPart1TextBox
			// 
			this.activationKeyPart1TextBox.Location = new System.Drawing.Point(108, 76);
			this.activationKeyPart1TextBox.MaxLength = 5;
			this.activationKeyPart1TextBox.Name = "activationKeyPart1TextBox";
			this.activationKeyPart1TextBox.Size = new System.Drawing.Size(48, 21);
			this.activationKeyPart1TextBox.TabIndex = 4;
			this.activationKeyPart1TextBox.Text = "";
			this.activationKeyPart1TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.activationKeyTextBox_KeyDown);
			this.activationKeyPart1TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.activationKeyTextBox_KeyPress);
			this.activationKeyPart1TextBox.TextChanged += new System.EventHandler(this.activationKeyTextBox_TextChanged);
			// 
			// activationKeyPart2TextBox
			// 
			this.activationKeyPart2TextBox.Location = new System.Drawing.Point(160, 76);
			this.activationKeyPart2TextBox.MaxLength = 5;
			this.activationKeyPart2TextBox.Name = "activationKeyPart2TextBox";
			this.activationKeyPart2TextBox.Size = new System.Drawing.Size(48, 21);
			this.activationKeyPart2TextBox.TabIndex = 5;
			this.activationKeyPart2TextBox.Text = "";
			this.activationKeyPart2TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.activationKeyTextBox_KeyDown);
			this.activationKeyPart2TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.activationKeyTextBox_KeyPress);
			this.activationKeyPart2TextBox.TextChanged += new System.EventHandler(this.activationKeyTextBox_TextChanged);
			// 
			// activationKeyPart3TextBox
			// 
			this.activationKeyPart3TextBox.Location = new System.Drawing.Point(212, 76);
			this.activationKeyPart3TextBox.MaxLength = 5;
			this.activationKeyPart3TextBox.Name = "activationKeyPart3TextBox";
			this.activationKeyPart3TextBox.Size = new System.Drawing.Size(48, 21);
			this.activationKeyPart3TextBox.TabIndex = 6;
			this.activationKeyPart3TextBox.Text = "";
			this.activationKeyPart3TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.activationKeyTextBox_KeyDown);
			this.activationKeyPart3TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.activationKeyTextBox_KeyPress);
			this.activationKeyPart3TextBox.TextChanged += new System.EventHandler(this.activationKeyTextBox_TextChanged);
			// 
			// activationKeyPart4TextBox
			// 
			this.activationKeyPart4TextBox.Location = new System.Drawing.Point(264, 76);
			this.activationKeyPart4TextBox.MaxLength = 5;
			this.activationKeyPart4TextBox.Name = "activationKeyPart4TextBox";
			this.activationKeyPart4TextBox.Size = new System.Drawing.Size(48, 21);
			this.activationKeyPart4TextBox.TabIndex = 7;
			this.activationKeyPart4TextBox.Text = "";
			this.activationKeyPart4TextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.activationKeyTextBox_KeyDown);
			this.activationKeyPart4TextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.activationKeyTextBox_KeyPress);
			this.activationKeyPart4TextBox.TextChanged += new System.EventHandler(this.activationKeyTextBox_TextChanged);
			// 
			// registrationEmailAddressTextBox
			// 
			this.registrationEmailAddressTextBox.Location = new System.Drawing.Point(108, 48);
			this.registrationEmailAddressTextBox.Name = "registrationEmailAddressTextBox";
			this.registrationEmailAddressTextBox.Size = new System.Drawing.Size(204, 21);
			this.registrationEmailAddressTextBox.TabIndex = 2;
			this.registrationEmailAddressTextBox.Text = "";
			this.registrationEmailAddressTextBox.TextChanged += new System.EventHandler(this.registrationEmailAddressTextBox_TextChanged);
			// 
			// registrationEmailAddressLabel
			// 
			this.registrationEmailAddressLabel.Location = new System.Drawing.Point(12, 52);
			this.registrationEmailAddressLabel.Name = "registrationEmailAddressLabel";
			this.registrationEmailAddressLabel.Size = new System.Drawing.Size(100, 16);
			this.registrationEmailAddressLabel.TabIndex = 1;
			this.registrationEmailAddressLabel.Text = "&Registration Name";
			this.registrationEmailAddressLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// activationKeyLabel
			// 
			this.activationKeyLabel.Location = new System.Drawing.Point(12, 80);
			this.activationKeyLabel.Name = "activationKeyLabel";
			this.activationKeyLabel.Size = new System.Drawing.Size(84, 16);
			this.activationKeyLabel.TabIndex = 3;
			this.activationKeyLabel.Text = "&Activation Key";
			// 
			// headerLabel
			// 
			this.headerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.headerLabel.Location = new System.Drawing.Point(12, 8);
			this.headerLabel.Name = "headerLabel";
			this.headerLabel.Size = new System.Drawing.Size(304, 32);
			this.headerLabel.TabIndex = 0;
			this.headerLabel.Text = "If you have a Q Set license please enter your activation details now:";
			// 
			// okButton
			// 
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(156, 156);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 13;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(236, 156);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 14;
			this.cancelButton.Text = "&Continue";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// footerLabel
			// 
			this.footerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.footerLabel.Location = new System.Drawing.Point(12, 108);
			this.footerLabel.Name = "footerLabel";
			this.footerLabel.Size = new System.Drawing.Size(304, 44);
			this.footerLabel.TabIndex = 12;
			this.footerLabel.Text = "You are entitled to evaluate Q Set with some limited features for 14 days without" +
				" an activation key.  To do this press Continue.";
			// 
			// LicenseForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(322, 188);
			this.Controls.Add(this.footerLabel);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.headerLabel);
			this.Controls.Add(this.registrationEmailAddressTextBox);
			this.Controls.Add(this.activationKeyPart4TextBox);
			this.Controls.Add(this.activationKeyPart3TextBox);
			this.Controls.Add(this.activationKeyPart2TextBox);
			this.Controls.Add(this.activationKeyPart1TextBox);
			this.Controls.Add(this.activationKeyLabel);
			this.Controls.Add(this.registrationEmailAddressLabel);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LicenseForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Q Set Activation";
			this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Handles the key press event of an activation key part text box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void activationKeyTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			int ascii = (int)e.KeyChar;
			if (ascii != 8 && ascii != 3 && ascii != 22 && ascii != 24 &&
				!(ascii >= 97 && ascii <= 122) &&
				!(ascii >= 65 && ascii <= 90) &&
				!(ascii >= 48 && ascii <= 57))
			{
				e.Handled = true;
			}
			else if (ascii != 8)
			{
				TextBox activationKeyTextBox = sender as TextBox;
				if (activationKeyTextBox != null)
				{
					if (activationKeyTextBox.Text.Length == activationKeyTextBox.MaxLength - 1 && 
						activationKeyTextBox.SelectionStart == activationKeyTextBox.MaxLength - 1 &&
						IsOKToSelectNextActivationKeyControl(true, activationKeyTextBox))
					{
						SelectNextControl(activationKeyTextBox, true, true, true, true);
					}
				}
			}
		}


		/// <summary>
		/// Handles the key down of an activation part text box.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void activationKeyTextBox_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			TextBox activationKeyTextBox = sender as TextBox;
			if (activationKeyTextBox != null)
			{
				if (((activationKeyTextBox.Text.Length == 0 && e.KeyCode == Keys.Back) ||
					 (activationKeyTextBox.SelectionStart == 0 && activationKeyTextBox.SelectionLength == 0 && e.KeyCode == Keys.Left))
					&& IsOKToSelectNextActivationKeyControl(false, activationKeyTextBox))
				{
					SelectNextControl(activationKeyTextBox, false, true, true, true);
				}
				else if (activationKeyTextBox.Text.Length == activationKeyTextBox.MaxLength && 
						 activationKeyTextBox.SelectionStart == activationKeyTextBox.MaxLength && 
						 e.KeyCode == Keys.Right &&
						 IsOKToSelectNextActivationKeyControl(true, activationKeyTextBox))
				{
					SelectNextControl(activationKeyTextBox, true, true, true, true);
				}
			}
		}


		/// <summary>
		/// Checks whether it is OK to select the next activation key text box.  We won't auto 
		/// navigate past the first or last text box so this function will validate for this.
		/// </summary>
		/// <param name="forward">Indicates whether we want to navigate forwards or backwards.</param>
		/// <param name="currentActivationKeyTextBox">The text box which currently has focus.</param>
		/// <returns>True if it is OK to perform the requested navigation, else false.</returns>
		private bool IsOKToSelectNextActivationKeyControl(bool forward, TextBox currentActivationKeyTextBox)
		{
			bool result = true;

			if (forward && currentActivationKeyTextBox == _activationKeyTextBoxes[_activationKeyTextBoxes.Length - 1])
				result = false;
			else if (!forward && currentActivationKeyTextBox == _activationKeyTextBoxes[0])
				result = false;

			return result;
		}


		/// <summary>
		/// Configures the states of all form controls, according to what is currently entered.
		/// </summary>
		private void ConfigureControlsStates()
		{
			bool isOkEnabled = true;
			
			if (registrationEmailAddressTextBox.Text.Length == 0)
				isOkEnabled = false;

			if (isOkEnabled)
				foreach (TextBox activationKeyTextBox in _activationKeyTextBoxes)
					if (activationKeyTextBox.Text.Length != activationKeyTextBox.MaxLength)
					{
						isOkEnabled = false;
						break;
					}

			okButton.Enabled = isOkEnabled;
		}


		/// <summary>
		/// Joins the segments of the activation key into a single key.
		/// </summary>
		/// <returns></returns>
		private string JoinActivationKey()	
		{
			//we wont use a foreach in case for some reason we cannot 
			//rely on it to bring back the controls in the correct order

			System.Text.StringBuilder sb = new System.Text.StringBuilder(_LICENSE_LENGTH);
			for (int i = 0; i < _LICENSE_LENGTH / _LICENSE_PART_LENGTH; i ++)
				sb.Append(_activationKeyTextBoxes[i].Text);

			return sb.ToString();
		}


		/// <summary>
		/// Handles the OK button click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, System.EventArgs e)
		{
			string activationKey = JoinActivationKey();
			if (_isActivationKeyValid(registrationEmailAddressTextBox.Text, activationKey))
			{
				DialogResult = DialogResult.OK;
				Hide();
			}
			else
			{
				MessageBox.Show(this, "Please enter a valid email address and activation key.", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				registrationEmailAddressTextBox.Focus();
			}
		}


		/// <summary>
		/// Handles the Cancel button click.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Hide();
		}


		/// <summary>
		/// Handles the text change of any of the activation key text boxes..
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void activationKeyTextBox_TextChanged(object sender, System.EventArgs e)
		{
			TextBox activationKeyTextBox = sender as TextBox;
			if (activationKeyTextBox != null)
			{
				int selStart = activationKeyTextBox.SelectionStart;
				activationKeyTextBox.Text = activationKeyTextBox.Text.ToUpper();
				if (selStart != -1)
					activationKeyTextBox.SelectionStart = selStart;

				ConfigureControlsStates();
			}
		}


		/// <summary>
		/// Handles email address text change event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void registrationEmailAddressTextBox_TextChanged(object sender, System.EventArgs e)
		{
			ConfigureControlsStates();
		}

	}
}
