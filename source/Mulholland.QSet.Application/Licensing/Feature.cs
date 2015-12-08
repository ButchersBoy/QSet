using System;

namespace Mulholland.QSet.Application.Licensing
{
	/// <summary>
	/// Features that can be enabled or disabled by the license.
	/// </summary>
	public enum Feature
	{
		/// <summary>
		/// Initial application access.
		/// </summary>
		StartUp,
		/// <summary>
		/// Send a new message.
		/// </summary>
		NewMessage,
		/// <summary>
		/// Forward existing messages.
		/// </summary>
		FowardMessage,
		/// <summary>
		/// Save a message.
		/// </summary>
		SaveMessage,
		/// <summary>
		/// Drag and drop messages.
		/// </summary>
		DragAndDropMessage,
		/// <summary>
		/// Delete message.
		/// </summary>
		DeleteMessage,
		/// <summary>
		/// Save the current Q Set.
		/// </summary>
		SaveQSet
	}
}
