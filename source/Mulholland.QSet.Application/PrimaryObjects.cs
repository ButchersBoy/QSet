using System;
using Mulholland.QSet.Application.Licensing;
using Mulholland.WinForms;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Groups together the main, persistant objects of the environment.
	/// </summary>
	internal class PrimaryObjects
	{ 
		private ProcessVisualizer _processVisualizer;
		private UserSettings _userSettings;
		private License _license;

		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="processVisualizer">Environment's process visualizer.</param>
		/// <param name="userSettings">User settings.</param>
		/// <param name="license">The application license.</param>
		public PrimaryObjects(
			ProcessVisualizer processVisualizer,
			UserSettings userSettings,
			License license)
		{
			if (processVisualizer == null) throw new ArgumentNullException("processVisualizer");
			else if (userSettings == null) throw new ArgumentNullException("userSettings");
			else if (license == null) throw new ArgumentNullException("license");

			_processVisualizer = processVisualizer;
			_userSettings = userSettings;
			_license = license;
		}


		/// <summary>
		/// Gets the environment's process visualizer.
		/// </summary>
		public ProcessVisualizer ProcessVisualizer
		{
			get
			{
				return _processVisualizer;
			}
		}


		/// <summary>
		/// Gets the object containing the users settings.
		/// </summary>
		public UserSettings UserSettings
		{
			get
			{
				return _userSettings;
			}
		}


		/// <summary>
		/// Gets the application <see cref="License"/> object.
		/// </summary>
		public License License
		{
			get
			{
				return _license;
			}
		}
	}
}
