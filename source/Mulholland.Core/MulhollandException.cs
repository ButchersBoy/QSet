using System;

namespace Mulholland.Core
{
	/// <summary>
	/// Mulholland software standard exception class.
	/// </summary>
	public class MulhollandException : ApplicationException
	{
		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		public MulhollandException() : base () {}


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="message">Message exception.</param>
		public MulhollandException(string message) : base(message) {}


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="message">Exception message.</param>
		/// <param name="innerException">Inner exception.</param>
		public MulhollandException(string message, Exception innerException) : base(message, innerException) {}		
	}
}
