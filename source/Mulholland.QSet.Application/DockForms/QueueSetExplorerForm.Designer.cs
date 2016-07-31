namespace Mulholland.QSet.Application.DockForms
{
    partial class QueueSetExplorerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueSetExplorerForm));
            this.queueSetExplorer = new Mulholland.QSet.Application.Controls.QSetExplorer();
            this.SuspendLayout();
            // 
            // queueSetExplorer
            // 
            this.queueSetExplorer.ActiveItem = null;
            this.queueSetExplorer.BackColor = System.Drawing.SystemColors.ControlDark;
            this.queueSetExplorer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueSetExplorer.ImageList = null;
            this.queueSetExplorer.Location = new System.Drawing.Point(0, 0);
            this.queueSetExplorer.Name = "queueSetExplorer";
            this.queueSetExplorer.QSet = null;
            this.queueSetExplorer.Size = new System.Drawing.Size(284, 261);
            this.queueSetExplorer.TabIndex = 0;
            // 
            // QueueSetExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.queueSetExplorer);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "QueueSetExplorerForm";
            this.Text = "Queue Set Explorer";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.QSetExplorer queueSetExplorer;
    }
}