using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Mulholland.QSet.Application.Controls;
using WeifenLuo.WinFormsUI.Docking;

namespace Mulholland.QSet.Application.DockForms
{
    public partial class QueueSetMonitorForm : DockContent
    {
        public QueueSetMonitorForm()
        {
            InitializeComponent();
        }

        internal QSetMonitor QSetMonitor { get { return this.qSetMonitor; } }
    }
}
