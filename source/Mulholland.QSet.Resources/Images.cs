using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;

namespace Mulholland.QSet.Resources
{
	/// <summary>
	/// Summary description for Components.
	/// </summary>
	public class Images : System.ComponentModel.Component
	{
		/// <summary>
		/// Q Set icons, sized 16 x 16.
		/// </summary>
		public System.Windows.Forms.ImageList Icon16ImageList;
		
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="container"></param>
		public Images(System.ComponentModel.IContainer container)
		{
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			container.Add(this);
			InitializeComponent();

		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public Images()
		{
			//
			// Required for Windows.Forms Class Composition Designer support
			//
			InitializeComponent();
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


		/// <summary>
		/// Enumerates the icons found in the icon image lists.
		/// </summary>
		public enum IconType
		{
			/// <summary>
			/// QSet icon.
			/// </summary>
			QSet = 0,
			/// <summary>
			/// Queue icon.
			/// </summary>
			Queue = 1,
			/// <summary>
			/// Folder icon.
			/// </summary>
			Folder = 2,
			/// <summary>
			/// Open folder icon.
			/// </summary>
			FolderOpen = 3,
			/// <summary>
			/// Message icon.
			/// </summary>
			Message = 4,
			/// <summary>
			/// Message open icon.
			/// </summary>
			MessageOpen = 5,
			/// <summary>
			/// Server icon.
			/// </summary>
			Server = 6,
			/// <summary>
			/// Options tab.
			/// </summary>
			OptionsTab = 7,
			/// <summary>
			/// Web Service.
			/// </summary>
			WebService = 8
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Images));
			this.Icon16ImageList = new System.Windows.Forms.ImageList(this.components);
			// 
			// Icon16ImageList
			// 
			this.Icon16ImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.Icon16ImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Icon16ImageList.ImageStream")));
			this.Icon16ImageList.TransparentColor = System.Drawing.Color.Transparent;

		}
		#endregion
	}
}
