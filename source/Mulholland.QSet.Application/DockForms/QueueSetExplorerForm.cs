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
    public partial class QueueSetExplorerForm : DockContent
    {
        public QueueSetExplorerForm()
        {
            InitializeComponent();

            this.queueSetExplorer.ContextMenuStrip = MenuItemBag.QSetCtxMenu;
        }
        internal QSetExplorer QSetExplorer { get { return this.queueSetExplorer; } }
    }
}
