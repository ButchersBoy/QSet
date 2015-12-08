using System;
using Mulholland.Core;

namespace Mulholland.QSet.Application
{
	/// <summary>
	/// Excpetion published when a queue cannot be read.
	/// </summary>
	internal class UnableToReadQueueException : MulhollandException 
	{

		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		public UnableToReadQueueException() 
			: base () 
		{}


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="message">Message exception.</param>
		public UnableToReadQueueException(string message) 
			: base(message) 
		{}


		/// <summary>
		/// Constructs the exception class.
		/// </summary>
		/// <param name="message">Exception message.</param>
		/// <param name="innerException">Inner exception.</param>
		public UnableToReadQueueException(string message, Exception innerException) 
			: base(message, innerException)
		{}		
	}
}
