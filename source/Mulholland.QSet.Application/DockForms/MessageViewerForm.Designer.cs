namespace Mulholland.QSet.Application.DockForms
{
    partial class MessageViewerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageViewerForm));
            this.messageViewer = new Mulholland.QSet.Application.Controls.MessageViewer();
            this.SuspendLayout();
            // 
            // messageViewer
            // 
            this.messageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageViewer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageViewer.License = null;
            this.messageViewer.Location = new System.Drawing.Point(0, 0);
            this.messageViewer.Name = "messageViewer";
            this.messageViewer.Size = new System.Drawing.Size(284, 261);
            this.messageViewer.TabIndex = 0;
            // 
            // MessageViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.messageViewer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageViewerForm";
            this.Text = "Message Viewer";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MessageViewer messageViewer;
    }
}