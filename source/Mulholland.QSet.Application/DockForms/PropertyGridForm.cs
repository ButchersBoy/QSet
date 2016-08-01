using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace Mulholland.QSet.Application.DockForms
{
    public partial class PropertyGridForm : DockContent
    {
        public PropertyGridForm()
        {
            InitializeComponent();
        }

        internal PropertyGrid PropertyGrid { get { return this.propertyGrid; } }
    }
}
