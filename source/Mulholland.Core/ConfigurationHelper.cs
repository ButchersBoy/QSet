using System;
using System.Xml;
using System.Configuration;

namespace Mulholland.Core
{
	/// <summary>
	/// Provides setting contained within the Mulholland Software section node and in the config file.
	/// </summary>
	public class ConfigurationHelper : IConfigurationSectionHandler
	{
		const string _ROOT = "mulhollandSoftware";


		/// <summary>
		/// Default constructor.
		/// </summary>
		public ConfigurationHelper() {}


		/// <summary>
		/// Gets the root node for all configuraiotn settings.
		/// </summary>
		/// <returns></returns>
		protected XmlNode GetRoot()
		{			
			return (XmlNode)ConfigurationSettings.GetConfig(_ROOT);
		}

		
		/// <summary>
		/// Retrieves a string value from the application's config file.
		/// </summary>
		/// <param name="node">X-Path to setting node relative to Interactive Items root configuration node.</param>
		/// <returns>Setting value.</returns>
		/// <exception cref="ConfigurationException">Thrown if setting is not found.</exception>
		/// <overloaded/>
		public string GetValue(string node)
		{
			string result = null;

			try
			{
				result = ((XmlNode)ConfigurationSettings.GetConfig(_ROOT)).SelectSingleNode(node).InnerText;
			}
			catch
			{				
				throw new ConfigurationException(string.Format("Unable to retrieve setting: {0}.", node));
			}

			return result;
		}

		

		/// <summary>
		/// Retrieves a string value from the application's config file.
		/// </summary>
		/// <param name="node">X-Path to node relative to Interactive Items root configuration node.</param>
		/// <param name="attribute">Attribute of node which contains setting.</param>
		/// <exception cref="ConfigurationException">Thrown if setting is not found and no default is supplied.</exception>
		/// <overloaded/>
		public string GetValue(string node, string attribute)
		{
			string result = null;

			try
			{				
				XmlAttributeCollection attributes = ((XmlNode)ConfigurationSettings.GetConfig(_ROOT)).SelectSingleNode(node).Attributes;
				result = attributes.GetNamedItem(attribute).Value;
			}
			catch
			{				
				throw new ConfigurationException(string.Format("Unable to retrieve setting: {0}, {1}.", node, attribute));
			}

			return result;
		}

		#region IConfigurationSectionHandler Members
		
		/// <summary>
		/// IConfigurationSectionHandler implementation.
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public object Create(object parent, object configContext, System.Xml.XmlNode section)
		{
			return section;
		}

		#endregion
	}
}
