namespace Mulholland.QSet.Application.DockForms
{
    partial class MessageBrowserForm
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
            Mulholland.QSet.Application.UserSettings userSettings1 = new Mulholland.QSet.Application.UserSettings();
            this.messageBrowser = new Mulholland.QSet.Application.Controls.MessageBrowser();
            this.SuspendLayout();
            // 
            // messageBrowser
            // 
            this.messageBrowser.AllowDrop = true;
            this.messageBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.messageBrowser.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.messageBrowser.ImageList = null;
            this.messageBrowser.Location = new System.Drawing.Point(0, 0);
            this.messageBrowser.Name = "messageBrowser";
            this.messageBrowser.QSetQueueItem = null;
            this.messageBrowser.Size = new System.Drawing.Size(284, 261);
            this.messageBrowser.TabIndex = 0;
            userSettings1.RecentFileListMaximumEntries = -1;
            userSettings1.ShowMessageViewerWindow = true;
            userSettings1.ShowPropertiesWindow = true;
            userSettings1.ShowQSetExplorerWindow = true;
            userSettings1.ShowQSetMonitorWindow = true;
            this.messageBrowser.UserSettings = userSettings1;
            // 
            // MessageBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.messageBrowser);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MessageBrowserForm";
            this.Text = "Message Browser";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.MessageBrowser messageBrowser;
    }
}