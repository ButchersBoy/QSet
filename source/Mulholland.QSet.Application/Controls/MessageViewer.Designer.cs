namespace Mulholland.QSet.Application.Controls
{
    partial class MessageViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageViewer));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.messageRichTextBox = new System.Windows.Forms.RichTextBox();
            this.binaryViewer = new Mulholland.WinForms.Controls.BinaryViewer();
            this.messageXmlViewer = new Mulholland.WinForms.Controls.XmlViewer();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.dataViewButtonItem = new System.Windows.Forms.ToolStripButton();
            this.xsltViewButtonItem = new System.Windows.Forms.ToolStripButton();
            this.binaryViewButtonItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveButtonItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.webBrowser1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.messageRichTextBox);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.binaryViewer);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.messageXmlViewer);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(435, 335);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(435, 360);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(34, 148);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(83, 82);
            this.webBrowser1.TabIndex = 3;
            // 
            // messageRichTextBox
            // 
            this.messageRichTextBox.Location = new System.Drawing.Point(251, 28);
            this.messageRichTextBox.Name = "messageRichTextBox";
            this.messageRichTextBox.Size = new System.Drawing.Size(100, 96);
            this.messageRichTextBox.TabIndex = 2;
            this.messageRichTextBox.Text = "";
            // 
            // binaryViewer
            // 
            this.binaryViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.binaryViewer.BorderStyle = Mulholland.WinForms.Controls.SimpleBorderStyle.None;
            this.binaryViewer.Location = new System.Drawing.Point(34, 28);
            this.binaryViewer.Name = "binaryViewer";
            this.binaryViewer.Size = new System.Drawing.Size(123, 114);
            this.binaryViewer.TabIndex = 1;
            // 
            // messageXmlViewer
            // 
            this.messageXmlViewer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.messageXmlViewer.BorderStyle = Mulholland.WinForms.Controls.SimpleBorderStyle.None;
            this.messageXmlViewer.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageXmlViewer.Location = new System.Drawing.Point(163, 28);
            this.messageXmlViewer.Name = "messageXmlViewer";
            this.messageXmlViewer.Size = new System.Drawing.Size(82, 82);
            this.messageXmlViewer.TabIndex = 0;
            this.messageXmlViewer.TabWidth = 6;
            this.messageXmlViewer.Xml = null;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataViewButtonItem,
            this.xsltViewButtonItem,
            this.binaryViewButtonItem,
            this.toolStripSeparator1,
            this.saveButtonItem});
            this.toolStrip.Location = new System.Drawing.Point(3, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(141, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // dataViewButtonItem
            // 
            this.dataViewButtonItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dataViewButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("dataViewButtonItem.Image")));
            this.dataViewButtonItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dataViewButtonItem.Name = "dataViewButtonItem";
            this.dataViewButtonItem.Size = new System.Drawing.Size(23, 22);
            this.dataViewButtonItem.Text = "Data View";
            this.dataViewButtonItem.ToolTipText = "Data View";
            // 
            // xsltViewButtonItem
            // 
            this.xsltViewButtonItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.xsltViewButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("xsltViewButtonItem.Image")));
            this.xsltViewButtonItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.xsltViewButtonItem.Name = "xsltViewButtonItem";
            this.xsltViewButtonItem.Size = new System.Drawing.Size(23, 22);
            this.xsltViewButtonItem.Text = "Transform View";
            this.xsltViewButtonItem.ToolTipText = "Transform View";
            // 
            // binaryViewButtonItem
            // 
            this.binaryViewButtonItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.binaryViewButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("binaryViewButtonItem.Image")));
            this.binaryViewButtonItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.binaryViewButtonItem.Name = "binaryViewButtonItem";
            this.binaryViewButtonItem.Size = new System.Drawing.Size(23, 22);
            this.binaryViewButtonItem.Text = "Binary View";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // saveButtonItem
            // 
            this.saveButtonItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveButtonItem.Image = ((System.Drawing.Image)(resources.GetObject("saveButtonItem.Image")));
            this.saveButtonItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButtonItem.Name = "saveButtonItem";
            this.saveButtonItem.Size = new System.Drawing.Size(23, 22);
            this.saveButtonItem.Text = "Save";
            // 
            // MessageViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 360);
            this.Controls.Add(this.toolStripContainer1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MessageViewer";
            this.Text = "Message Viewer";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.RichTextBox messageRichTextBox;
        private WinForms.Controls.BinaryViewer binaryViewer;
        private WinForms.Controls.XmlViewer messageXmlViewer;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton dataViewButtonItem;
        private System.Windows.Forms.ToolStripButton xsltViewButtonItem;
        private System.Windows.Forms.ToolStripButton binaryViewButtonItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton saveButtonItem;
    }
}