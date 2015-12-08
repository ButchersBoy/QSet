using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Mulholland.QSet.Model;
using Mulholland.QSet.Application.WebServices;

namespace Mulholland.QSet.Application.Controls
{
	/// <summary>
	/// Summary description for WebServiceClientControl.
	/// </summary>
	public class WebServiceClientControl : System.Windows.Forms.UserControl, IQSetItemControl
	{
		private System.Windows.Forms.TextBox webServiceUrlTextBox;
		private System.Windows.Forms.Label webServiceUrlLabel;
		private System.Windows.Forms.Button generateProxyButton;
		private System.Windows.Forms.GroupBox testRunGroupBox;
		private System.Windows.Forms.ComboBox methodComboBox;
		private System.Windows.Forms.Label methodLabel;
		private System.Windows.Forms.Panel resultsPanel;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.Splitter resultsSplitter;
		private System.Windows.Forms.Panel inputsAndOutputsPanel;
		private System.Windows.Forms.Splitter inputsAndResultsSplitter;
		private System.Windows.Forms.Label inputParametersLabel;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Label inputParameterValueLabel;
		private System.Windows.Forms.TextBox textBox1;

		private QSetWebServiceItem _qSetWebServiceItem = null;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public WebServiceClientControl()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}


		/// <summary>
		/// Gets or sets the current <see cref="QSetWebServiceItem"/> which is being displayed by the control.
		/// </summary>
		public QSetWebServiceItem QSetWebServiceItem
		{
			get
			{
				return _qSetWebServiceItem;
			}
			set
			{
				_qSetWebServiceItem = value;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.webServiceUrlTextBox = new System.Windows.Forms.TextBox();
			this.webServiceUrlLabel = new System.Windows.Forms.Label();
			this.generateProxyButton = new System.Windows.Forms.Button();
			this.testRunGroupBox = new System.Windows.Forms.GroupBox();
			this.inputsAndOutputsPanel = new System.Windows.Forms.Panel();
			this.methodComboBox = new System.Windows.Forms.ComboBox();
			this.methodLabel = new System.Windows.Forms.Label();
			this.resultsPanel = new System.Windows.Forms.Panel();
			this.inputsAndResultsSplitter = new System.Windows.Forms.Splitter();
			this.listView2 = new System.Windows.Forms.ListView();
			this.resultsSplitter = new System.Windows.Forms.Splitter();
			this.inputParametersLabel = new System.Windows.Forms.Label();
			this.listView1 = new System.Windows.Forms.ListView();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.inputParameterValueLabel = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.testRunGroupBox.SuspendLayout();
			this.inputsAndOutputsPanel.SuspendLayout();
			this.resultsPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// webServiceUrlTextBox
			// 
			this.webServiceUrlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.webServiceUrlTextBox.Location = new System.Drawing.Point(12, 48);
			this.webServiceUrlTextBox.Name = "webServiceUrlTextBox";
			this.webServiceUrlTextBox.Size = new System.Drawing.Size(736, 21);
			this.webServiceUrlTextBox.TabIndex = 0;
			this.webServiceUrlTextBox.Text = "http://localhost/Mulholland.Web.SampleServices/Sample1.asmx";
			// 
			// webServiceUrlLabel
			// 
			this.webServiceUrlLabel.Location = new System.Drawing.Point(12, 32);
			this.webServiceUrlLabel.Name = "webServiceUrlLabel";
			this.webServiceUrlLabel.Size = new System.Drawing.Size(100, 16);
			this.webServiceUrlLabel.TabIndex = 1;
			this.webServiceUrlLabel.Text = "Web Service URL:";
			// 
			// generateProxyButton
			// 
			this.generateProxyButton.Location = new System.Drawing.Point(12, 76);
			this.generateProxyButton.Name = "generateProxyButton";
			this.generateProxyButton.Size = new System.Drawing.Size(92, 23);
			this.generateProxyButton.TabIndex = 2;
			this.generateProxyButton.Text = "Generate Proxy";
			this.generateProxyButton.Click += new System.EventHandler(this.generateProxyButton_Click);
			// 
			// testRunGroupBox
			// 
			this.testRunGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.testRunGroupBox.Controls.Add(this.methodLabel);
			this.testRunGroupBox.Controls.Add(this.methodComboBox);
			this.testRunGroupBox.Controls.Add(this.inputsAndOutputsPanel);
			this.testRunGroupBox.Location = new System.Drawing.Point(12, 108);
			this.testRunGroupBox.Name = "testRunGroupBox";
			this.testRunGroupBox.Size = new System.Drawing.Size(740, 484);
			this.testRunGroupBox.TabIndex = 3;
			this.testRunGroupBox.TabStop = false;
			this.testRunGroupBox.Text = "Test Run:";
			// 
			// inputsAndOutputsPanel
			// 
			this.inputsAndOutputsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.inputsAndOutputsPanel.Controls.Add(this.textBox1);
			this.inputsAndOutputsPanel.Controls.Add(this.inputParameterValueLabel);
			this.inputsAndOutputsPanel.Controls.Add(this.splitter1);
			this.inputsAndOutputsPanel.Controls.Add(this.listView1);
			this.inputsAndOutputsPanel.Controls.Add(this.inputParametersLabel);
			this.inputsAndOutputsPanel.Controls.Add(this.inputsAndResultsSplitter);
			this.inputsAndOutputsPanel.Controls.Add(this.resultsPanel);
			this.inputsAndOutputsPanel.Location = new System.Drawing.Point(8, 60);
			this.inputsAndOutputsPanel.Name = "inputsAndOutputsPanel";
			this.inputsAndOutputsPanel.Size = new System.Drawing.Size(724, 416);
			this.inputsAndOutputsPanel.TabIndex = 0;
			// 
			// methodComboBox
			// 
			this.methodComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.methodComboBox.Location = new System.Drawing.Point(64, 24);
			this.methodComboBox.Name = "methodComboBox";
			this.methodComboBox.Size = new System.Drawing.Size(668, 21);
			this.methodComboBox.TabIndex = 1;
			// 
			// methodLabel
			// 
			this.methodLabel.Location = new System.Drawing.Point(12, 28);
			this.methodLabel.Name = "methodLabel";
			this.methodLabel.Size = new System.Drawing.Size(52, 16);
			this.methodLabel.TabIndex = 2;
			this.methodLabel.Text = "Method";
			// 
			// resultsPanel
			// 
			this.resultsPanel.Controls.Add(this.resultsSplitter);
			this.resultsPanel.Controls.Add(this.listView2);
			this.resultsPanel.Dock = System.Windows.Forms.DockStyle.Right;
			this.resultsPanel.Location = new System.Drawing.Point(356, 0);
			this.resultsPanel.Name = "resultsPanel";
			this.resultsPanel.Size = new System.Drawing.Size(368, 416);
			this.resultsPanel.TabIndex = 0;
			// 
			// inputsAndResultsSplitter
			// 
			this.inputsAndResultsSplitter.Dock = System.Windows.Forms.DockStyle.Right;
			this.inputsAndResultsSplitter.Location = new System.Drawing.Point(354, 0);
			this.inputsAndResultsSplitter.Name = "inputsAndResultsSplitter";
			this.inputsAndResultsSplitter.Size = new System.Drawing.Size(2, 416);
			this.inputsAndResultsSplitter.TabIndex = 1;
			this.inputsAndResultsSplitter.TabStop = false;
			// 
			// listView2
			// 
			this.listView2.Dock = System.Windows.Forms.DockStyle.Top;
			this.listView2.Location = new System.Drawing.Point(0, 0);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(368, 97);
			this.listView2.TabIndex = 0;
			// 
			// resultsSplitter
			// 
			this.resultsSplitter.Dock = System.Windows.Forms.DockStyle.Top;
			this.resultsSplitter.Location = new System.Drawing.Point(0, 97);
			this.resultsSplitter.Name = "resultsSplitter";
			this.resultsSplitter.Size = new System.Drawing.Size(368, 3);
			this.resultsSplitter.TabIndex = 1;
			this.resultsSplitter.TabStop = false;
			// 
			// inputParametersLabel
			// 
			this.inputParametersLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.inputParametersLabel.Location = new System.Drawing.Point(0, 0);
			this.inputParametersLabel.Name = "inputParametersLabel";
			this.inputParametersLabel.Size = new System.Drawing.Size(354, 16);
			this.inputParametersLabel.TabIndex = 4;
			this.inputParametersLabel.Text = "Input Parameters";
			// 
			// listView1
			// 
			this.listView1.Dock = System.Windows.Forms.DockStyle.Top;
			this.listView1.Location = new System.Drawing.Point(0, 16);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(354, 88);
			this.listView1.TabIndex = 5;
			// 
			// splitter1
			// 
			this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter1.Location = new System.Drawing.Point(0, 104);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(354, 3);
			this.splitter1.TabIndex = 7;
			this.splitter1.TabStop = false;
			// 
			// inputParameterValueLabel
			// 
			this.inputParameterValueLabel.Dock = System.Windows.Forms.DockStyle.Top;
			this.inputParameterValueLabel.Location = new System.Drawing.Point(0, 107);
			this.inputParameterValueLabel.Name = "inputParameterValueLabel";
			this.inputParameterValueLabel.Size = new System.Drawing.Size(354, 16);
			this.inputParameterValueLabel.TabIndex = 8;
			this.inputParameterValueLabel.Text = "Parameter Value";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(140, 136);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(128, 21);
			this.textBox1.TabIndex = 9;
			this.textBox1.Text = "textBox1";
			// 
			// WebServiceClientControl
			// 
			this.Controls.Add(this.testRunGroupBox);
			this.Controls.Add(this.generateProxyButton);
			this.Controls.Add(this.webServiceUrlLabel);
			this.Controls.Add(this.webServiceUrlTextBox);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.Name = "WebServiceClientControl";
			this.Size = new System.Drawing.Size(764, 604);
			this.testRunGroupBox.ResumeLayout(false);
			this.inputsAndOutputsPanel.ResumeLayout(false);
			this.resultsPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void generateProxyButton_Click(object sender, System.EventArgs e)
		{
			WebServiceProxyGenerator proxyGenerator = new WebServiceProxyGenerator("C:\\", webServiceUrlTextBox.Text);
			proxyGenerator.GenerateProxy();

		}


		#region IQSetItemControl Members

		public QSetItemBase QSetItem
		{
			get
			{
				if (_qSetWebServiceItem != null)
					return (QSetItemBase)_qSetWebServiceItem;
				else
					return null;
			}
		}

		#endregion
	}
}
