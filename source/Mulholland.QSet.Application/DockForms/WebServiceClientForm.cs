using System;
using Mulholland.QSet.Application.Controls;
using WeifenLuo.WinFormsUI.Docking;

namespace Mulholland.QSet.Application.DockForms
{
    public partial class WebServiceClientForm : DockContent
    {
        public WebServiceClientForm()
        {
            InitializeComponent();
        }

        public WebServiceClientControl WebServiceClientControl
        {
            get
            {
                return webServiceClientControl;
            }
        }
    }
}
