using System;
using System.Windows.Forms;
using Mulholland.QSet.Resources;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Coordinates controls and user driven activites in the environment.
	/// </summary>
	internal class EnvironmentCoordinator
	{
		private TaskManager _taskManager;
		private PrimaryObjects _primaryObjects;
		private PrimaryForms _primaryForms;
		private PrimaryControls _primaryControls;
		private PrimaryMenuListener _primaryMenuListener;
		private PrimaryControlListener _primaryControlListener;		
		private PrimaryFormsListener _primaryFormListener;
		

		/// <summary>
		/// Constructs the coordinator.
		/// </summary>
		/// <param name="qSetEnvironmentForm">Environment form.</param>
		/// <param name="primaryControls">Primary controls.</param>
		/// <param name="primaryMenus">Primary menus.</param>
		/// <param name="primaryForms">Primary forms.</param>
		/// <param name="primaryObjects">Primary objects.</param>
		public EnvironmentCoordinator(
			QSetEnvironmentForm qSetEnvironmentForm, 
			PrimaryMenus primaryMenus, 
			PrimaryControls primaryControls,
			PrimaryForms primaryForms,
			PrimaryObjects primaryObjects)
		{						
			_primaryForms = primaryForms;
			_primaryObjects = primaryObjects;
			_primaryControls = primaryControls;

			_taskManager = new TaskManager(primaryMenus, primaryControls, primaryForms, primaryObjects);	
			_primaryMenuListener = new PrimaryMenuListener(_taskManager, _primaryObjects, _primaryForms, _primaryControls, primaryMenus);
			_primaryControlListener = new PrimaryControlListener(_taskManager, _primaryObjects, _primaryForms, _primaryControls, primaryMenus);
			_primaryFormListener = new PrimaryFormsListener(_taskManager, _primaryObjects, _primaryForms, _primaryControls, primaryMenus);
		}


		/// <summary>
		/// Performs initial set up required on application start.
		/// </summary>
		public void SetUp()
		{
			_primaryForms.QueueSearchForm.AllowMachineSelect = true;
			_primaryForms.QueueSearchForm.ImageList = _primaryControls.Images.Icon16ImageList;
			_primaryForms.QueueSearchForm.ComputerImageIndex = (int)Images.IconType.Server;
			_primaryForms.QueueSearchForm.QueueImageIndex = (int)Images.IconType.Queue;			

			_primaryObjects.ProcessVisualizer.AwaitingText = Locale.UserMessages.Ready;
			_primaryObjects.UserSettings.CheckFileAssociations();

			_taskManager.SetTitleBarText();
			_taskManager.MenuStateManger.SetAllMenusState();
			_taskManager.ConfigureEnvironmentAcccordingToUserSettings();						
		}


		/// <summary>
		/// Opens the requested Q Set.
		/// </summary>
		/// <param name="path"></param>
		public void OpenQSet(string path)
		{
			_taskManager.OpenQSet(path);
		}


		/// <summary>
		/// Shuts the environment down.
		/// </summary>
		public bool ShutDown()
		{
			bool result = true;

			if (!_taskManager.HasShutDown)
				result = _taskManager.ShutDown();

			return result;
		}


		/// <summary>
		/// Gets  whether the environment has completed all required tasks required before shut down.
		/// </summary>
		public bool HasShutDown
		{
			get
			{
				return _taskManager.HasShutDown;
			}
		}

	}

}
