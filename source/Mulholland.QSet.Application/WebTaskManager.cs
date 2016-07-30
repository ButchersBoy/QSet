using System;
using Mulholland.QSet.Model;
using Mulholland.QSet.Application.Controls;
using TD.SandDock;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Performs tasks associated with the web.
	/// </summary>
	internal class WebTaskManager
	{
		private TaskManager _taskManager;
		private PrimaryControls _primaryControls;
		private PrimaryObjects _primaryObjects;
		private PrimaryForms _primaryForms;

		/// <summary>
		/// Constructs the WebTaskManager.
		/// </summary>
		public WebTaskManager(
			TaskManager taskManager,
			PrimaryControls primaryControls,
			PrimaryObjects primaryObjects,
			PrimaryForms primaryForms)
		{
			_taskManager = taskManager;
			_primaryObjects = primaryObjects;
			_primaryControls = primaryControls;						
			_primaryForms = primaryForms;
		}


		/// <summary>
		/// Adds a new web service client to the Q Set.
		/// </summary>
		public void AddNewWebServiceClient()
		{
			//ensure the q set is selected at a valid position
			if (_primaryControls.QSetExplorer.QSet == null)
				_taskManager.CreateNewQSet();
			if (_primaryControls.QSetExplorer.ActiveItem == null || 
				!(_primaryControls.QSetExplorer.ActiveItem is QSetFolderItem) ||
				_primaryControls.QSetExplorer.ActiveItem is QSetMachineItem)
				_primaryControls.QSetExplorer.ActiveItem = _primaryControls.QSetExplorer.QSet;

			QSetFolderItem parentItem = _primaryControls.QSetExplorer.ActiveItem as QSetFolderItem;

			if (parentItem != null)
			{
				QSetWebServiceItem webServiceItem = new QSetWebServiceItem(_taskManager.GetNextAvailableNewItemName("New Web Service Client", parentItem.ChildItems));
				parentItem.ChildItems.Add(webServiceItem);
				LoadNewWebServiceClientControl(webServiceItem);
			}
		}


		/// <summary>
		/// Opens a <see cref="WebServiceClientControl"/> for a <see cref="QSetWebServiceItem"/> if a 
		/// control is not already open, otherwise the existing control is opened.
		/// </summary>
		/// <param name="webServiceItem">Web Service item to open</param>
		public void OpenWebServiceClient(QSetWebServiceItem webServiceItem)
		{
			if (_taskManager.IsItemOpen(webServiceItem))
				_taskManager.BringDocumentToFront(webServiceItem);
			else
				LoadNewWebServiceClientControl(webServiceItem);
		}


		/// <summary>
		/// Loads a new <see cref="WebServiceClientControl"/> and adds it into the document collection, and opens the document.
		/// </summary>
		/// <param name="webServiceItem"></param>
		private void LoadNewWebServiceClientControl(QSetWebServiceItem webServiceItem)
		{			
			//set up a web service control, and create a dock for it
			WebServiceClientControl webServiceClientControl = new WebServiceClientControl();			
			webServiceClientControl.QSetWebServiceItem = webServiceItem;
			_primaryControls.WebServiceClientControlCollection.Add(webServiceItem.ID.ToString(), webServiceClientControl);

   //         DockControl newDockControl = new TabbedDocument(_primaryControls.DocumentContainer.Manager, webServiceClientControl, webServiceItem.Name);
   //         newDockControl.Open();
			//newDockControl.Activate();
		}
	}
}
