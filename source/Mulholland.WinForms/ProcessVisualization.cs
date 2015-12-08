using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Forms;
using Mulholland.Core;

namespace Mulholland.WinForms
{
	#region public class ProcessVisualizer

	/// <summary>
	/// Manages visual feedback given to the user to indicate when a control's processes are being completed.
	/// </summary>
	public class ProcessVisualizer
	{
		private Control _owner;
		private StatusBarPanel _statusBarPanel = null;
		private ListDictionary _processesDictionary;	
		private Cursor _seizedCursor = null;
		private string _awaitingText = string.Empty;

		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="owner">The owning control of the visualizer.</param>
		public ProcessVisualizer(Control owner) 
		{
			if (owner == null) throw new ArgumentNullException("owner");

			_owner = owner;
			_processesDictionary = new ListDictionary();			
		}


		/// <summary>
		/// Gets or sets the optional status bar panel associated with the object.
		/// </summary>
		public StatusBarPanel StatusBarPanel
		{
			get
			{
				return _statusBarPanel;				
			}
			set
			{
				_statusBarPanel = value;
				VisualizeProcesses();
			}
		}


		/// <summary>
		/// Starts the visualization of a process.
		/// </summary>
		/// <param name="process">Process to be visualized.</param>
		public void ProcessStarting(VisualizableProcess process)
		{			
			if (process == null) throw new ArgumentNullException("process");

			_processesDictionary.Add(process.Guid, process);						
			VisualizeProcesses();
		}


		/// <summary>
		/// Gets or sets the text displayed in the status bar when the are no processes currently being visualized.
		/// </summary>
		public string AwaitingText
		{
			get
			{
				return _awaitingText;
			}
			set
			{
				_awaitingText = value;
			}
		}


		/// <summary>
		/// Ends the visualization of a process.
		/// </summary>
		/// <param name="process">Process which has ended.</param>
		public void ProcessCompleted(VisualizableProcess process)
		{
			if (process == null) throw new ArgumentNullException("process");

			_processesDictionary.Remove(process.Guid);
			VisualizeProcesses();
		}


		/// <summary>
		/// Enforces the owner control to use a particular mouse pointer until <see cref="ReleaseCursor"/> is called.
		/// </summary>
		/// <param name="cursor">Cursor to use.</param>
		public void SeizeCursor(Cursor cursor)
		{
			_seizedCursor = cursor;
			VisualizeProcesses();
		}


		/// <summary>
		/// Releases a cursor enforced by <see cref="SeizeCursor"/>
		/// </summary>
		public void ReleaseCursor()
		{
			_seizedCursor = null;
			VisualizeProcesses();
		}

        private delegate void Action();

		/// <summary>
		/// Sets up the mouse cursor and status bar, according to what is current running.
		/// </summary>
		private void VisualizeProcesses()
		{
			//ascertain which cursor to display, an arrow, an arrow & hourglass, or hourglass, or an overridden (seized) cursor
			Cursor newCursor = Cursors.Arrow;
			if (_seizedCursor == null)
			{										
				if (_processesDictionary.Count > 0)
				{
					newCursor = Cursors.AppStarting;
					foreach (DictionaryEntry entry in _processesDictionary)
					{
						VisualizableProcess process = entry.Value as VisualizableProcess;
						if (process != null && process.AllowUserInteraction == false)
						{					
							newCursor = Cursors.WaitCursor;
						}			
					}
				}				
			}
			else
				//cursor has been overridden
				newCursor = _seizedCursor;

            Action hack = () =>
                {
                    //display the cursor 
                    _owner.Cursor = newCursor;

                    //set the staus bar
                    if (_statusBarPanel != null)
                    {
                        //ascertain the new text
                        string newText = _awaitingText == null ? "" : _awaitingText;
                        System.Collections.IDictionaryEnumerator de = _processesDictionary.GetEnumerator();
                        if (de.MoveNext())
                        {
                            VisualizableProcess oldestProcess = de.Value as VisualizableProcess;
                            if (oldestProcess != null)
                                newText = oldestProcess.Description;
                        }
                        _statusBarPanel.Text = newText;
                    }
                };
            if (_owner.Visible)
                _owner.Invoke(hack);
						
		}
	}

	#endregion

	#region public class VisualizableProcess

	/// <summary>
	/// Allows identification of a process which is to be visualised to the user.
	/// </summary>
	public class VisualizableProcess
	{		
		private bool _allowUserInteraction;
		private string _description;
		private Guid _guid;

		/// <summary>
		/// Constructs the object.  It is assumed that user 
		/// interaction is not allowed until the task is completed.
		/// </summary>		
		public VisualizableProcess() 
			: this(null, false)
		{}


		/// <summary>
		/// Constructs the object.  It is assumed that user 
		/// interaction is not allowed until the task is completed.
		/// </summary>		
		/// <param name="description">A user friendly description of the process.</param>
		public VisualizableProcess(string description) 
			: this(description, false)
		{}


		/// <summary>
		/// Constructs the object.  It is assumed that user 
		/// interaction is not allowed until the task is completed.
		/// </summary>				
		/// <param name="allowUserInteraction">Set to true if the arrow hourglass cursor is to be displayed,
		/// allowing the user to continue interacting during the duration of a process.</param>
		public VisualizableProcess(bool allowUserInteraction) 
			: this(null, allowUserInteraction)
		{}


		/// <summary>
		/// Constructs the object
		/// </summary>		
		/// <param name="description">A user friendly description of the process.</param>
		/// <param name="allowUserInteraction">Set to true if the arrow hourglass cursor is to be displayed,
		/// allowing the user to continue interacting during the duration of a process.</param>
		public VisualizableProcess(string description, bool allowUserInteraction) 			
		{
			_description = description;
			_allowUserInteraction = allowUserInteraction;
			_guid = Guid.NewGuid();
		}


		/// <summary>
		/// Gets or sets the description associated with the process.
		/// </summary>
		/// <remarks>This value may be null.</remarks>
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}


		/// <summary>
		/// Gets the GUID which is associated with the process. This value is automatically generated.
		/// </summary>
		public Guid Guid
		{
			get
			{
				return _guid;
			}
		}	


		/// <summary>
		/// Gets whether user interaction is allowed, determining whether 
		/// an arrow and hourglass is displayed instead of an hourglass.
		/// </summary>
		public bool AllowUserInteraction
		{
			get
			{
				return _allowUserInteraction;
			}
		}
	}

	#endregion

	/// <summary>
	/// Event signature for events which signify the start and end of a job for which the lifetime 
	/// of the job can be displayed to the user, using methods such as changing the mouse cursor.
	/// </summary>
	public delegate void VisualizableProcessEvent(object sender, VisualizableProcessEventArgs e);

	#region public class VisualizableProcessEventArgs : MulhollandException

	/// <summary>
	/// Event args for VisualizableProcessEvent event delegate.
	/// </summary>
	public class VisualizableProcessEventArgs : MulhollandException
	{
		private VisualizableProcess _process;

		/// <summary>
		/// Constructs the object.
		/// </summary>
		/// <param name="process">Process which can be visualized.</param>
		public VisualizableProcessEventArgs(VisualizableProcess process)
		{
			_process = process;
		}


		/// <summary>
		/// Gets the process associated with the event arguments
		/// </summary>
		public VisualizableProcess Process
		{
			get
			{
				return _process;
			}
		}
	}

	#endregion

}
