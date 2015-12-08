using System;
using Mulholland.QSet.Model;

namespace Mulholland.QSet.Application.Controls
{
	/// <summary>
	/// Controls which implement this interface have the ability to work with a single Q Set item at a time.
	/// </summary>
	internal interface IQSetItemControl
	{
		QSetItemBase QSetItem
		{
			get;
		}
	}
}
