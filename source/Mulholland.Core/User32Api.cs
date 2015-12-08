using System;
using System.Runtime.InteropServices;

namespace Mulholland.Core
{
	/// <summary>
	/// Provides access to Windows API calls containined in User32.dll.
	/// </summary>
	public class User32Api
	{
		/// <summary/>
		public static readonly int WM_SETREDRAW      = 0x000B;
		/// <summary/>
		public static readonly int WM_USER           = 0x400;
		/// <summary/>
		public static readonly int EM_GETEVENTMASK   = (WM_USER + 59);
		/// <summary/>
		public static readonly int EM_SETEVENTMASK   = (WM_USER + 69);

		/// <summary>
		/// Default constructor.
		/// </summary>
		static User32Api() {}

		/// <summary/>
		[DllImport("user32", CharSet = CharSet.Auto)]
		public extern static IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

		/// <summary/>
		[DllImport("user32.dll")]
		public extern static bool LockWindowUpdate(IntPtr hWndLock);

	}
}
