namespace Mulholland.QSet.Application.DockForms
{
    partial class WebServiceClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebServiceClientForm));
            this.webServiceClientControl = new Mulholland.QSet.Application.Controls.WebServiceClientControl();
            this.SuspendLayout();
            // 
            // webServiceClientControl
            // 
            this.webServiceClientControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webServiceClientControl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.webServiceClientControl.Location = new System.Drawing.Point(0, 0);
            this.webServiceClientControl.Name = "webServiceClientControl";
            this.webServiceClientControl.QSetWebServiceItem = null;
            this.webServiceClientControl.Size = new System.Drawing.Size(482, 334);
            this.webServiceClientControl.TabIndex = 0;
            // 
            // WebServiceClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 334);
            this.Controls.Add(this.webServiceClientControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebServiceClientForm";
            this.Text = "Web Service Client";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.WebServiceClientControl webServiceClientControl;
    }
}