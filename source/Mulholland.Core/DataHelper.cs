using System;
using System.Data;
using System.Data.SqlClient;

namespace Mulholland.Core
{
	/// <summary>
	/// Provides data operations against a standard Mulholland data source.
	/// </summary>
	public class DataHelper
	{
		private const string _DATA_SOURCES_NODE = "dataSources";
		private const string _DATA_SOURCE_NAME_ATTRIBUTE = "name";
		private const string _DATA_SOURCE_CONNECTION_ATTRIBUTE = "cn";

		private string _connectionString;

		/// <summary>
		/// Constructs the data provider with the logical database name.
		/// </summary>
		/// <param name="dataSourceName">The logical name of the database.</param>
		public DataHelper(string dataSourceName) 
		{
			ConfigurationHelper config = new ConfigurationHelper();
			_connectionString = config.GetValue(string.Format("dataSources/dataSource[@name='{0}']", dataSourceName), "cn"); 	
		}


		/// <summary>
		/// Executes astored procedure opening up an IDataReader to process the result.
		/// </summary>
		/// <param name="storedProcedure">Name of stored procedure to execute.</param>
		/// <param name="parameters">Parameters to pass to stored procedure.</param>
		/// <returns>IDataReader</returns>
		/// <exception cref="MulhollandException">Wraps any data exception which may occur.</exception>
		public IDataReader ExecuteReader(string storedProcedure, IDbDataParameter[] parameters)
		{
			IDbCommand cmd = null;
			IDataReader reader = null;

			try
			{				
				cmd = new SqlCommand(storedProcedure, new SqlConnection(_connectionString));
				cmd.CommandType = CommandType.StoredProcedure;
 
				AttachParametersToCommand(cmd, parameters);

				cmd.Connection.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (Exception exc)
			{
				throw new MulhollandException("Encoutered an error performing data operation.", exc);
			}
			finally
			{
				if (cmd != null)
				{
					cmd.Dispose();
				}
			}

			return reader;
		}


		/// <summary>
		/// Executes astored procedure opening up an IDataReader to process the result.
		/// </summary>
		/// <param name="storedProcedure">Name of stored procedure to execute.</param>
		/// <returns>IDataReader</returns>
		/// <exception cref="MulhollandException">Wraps any data exception which may occur.</exception>
		public IDataReader ExecuteReader(string storedProcedure)
		{
			return ExecuteReader(storedProcedure, null);
		}


		/// <summary>
		/// Executes a stored procedure.
		/// </summary>
		/// <param name="storedProcedure">Name of stored procedure to execute.</param>
		/// <param name="parameters">Parameters to pass to stored procedure.</param>
		/// <exception cref="MulhollandException">Wraps any data exception which may occur.</exception>
		public void ExecuteNonQuery(string storedProcedure, IDbDataParameter[] parameters)
		{			
			IDbCommand cmd = null;

			try
			{				
				cmd = new SqlCommand(storedProcedure, new SqlConnection(_connectionString));
				cmd.CommandType = CommandType.StoredProcedure;

				AttachParametersToCommand(cmd, parameters);

				cmd.Connection.Open();
				cmd.ExecuteNonQuery();
			}
			catch (Exception exc)
			{
				throw new MulhollandException("Encoutered an error performing data operation.", exc);
			}
			finally
			{
				if (cmd != null)
				{
					CloseConnection(cmd);
					cmd.Dispose();
				}
			}
		}


		/// <summary>
		/// Executes a stored procedure.
		/// </summary>
		/// <param name="storedProcedure">Name of stored procedure to execute.</param>
		/// <exception cref="MulhollandException">Wraps any data exception which may occur.</exception>
		public void ExecuteNonQuery(string storedProcedure)
		{
			ExecuteNonQuery(storedProcedure, null);
		}


		/// <summary>
		/// Creates an array of parameters of the type specific to the classes data source.
		/// </summary>
		/// <param name="elements">The number of elements to create in the array.</param>
		/// <returns>An empty array of parameters.</returns>
		public IDbDataParameter[] CreateParameterArray(int elements)
		{
			return new SqlParameter[elements];
		}


		/// <summary>
		/// Creates an input parameter of the type specific to the classes data source.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="type">Type.</param>
		/// <returns>Instantiated IDbDataParameter.</returns>
		public IDbDataParameter CreateInputParameter(string name, DbType type)
		{
			SqlParameter param = new SqlParameter();			

			((IDbDataParameter)param).ParameterName = name;
			((IDbDataParameter)param).DbType = type;
			((IDbDataParameter)param).Direction = ParameterDirection.Input;

			return param;
		}


		/// <summary>
		/// Creates an input parameter of the type specific to the classes data source.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="type">Type.</param>
		/// <param name="value">The inital value of the parameter</param>
		/// <returns>Instantiated IDbDataParameter.</returns>
		public IDbDataParameter CreateInputParameter(string name, DbType type, object value)
		{
			IDbDataParameter param = CreateInputParameter(name, type);
			
			param.Value = value;

			return param;
		}


		/// <summary>
		/// Creates an input/ouput parameter of the type specific to the classes data source.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="type">Type.</param>
		/// <param name="size">The size of the parameter.</param>		
		/// <returns>Instantiated IDbDataParameter.</returns>
		public IDbDataParameter CreateInputOutputParameter(string name, DbType type, int size)		
		{
			SqlParameter param = new SqlParameter();			

			((IDbDataParameter)param).ParameterName = name;
			((IDbDataParameter)param).DbType = type;
			((IDbDataParameter)param).Size = size;
			((IDbDataParameter)param).Direction = ParameterDirection.InputOutput;

			return param;
		}

				
		/// <summary>
		/// Creates an input/ouput parameter of the type specific to the classes data source.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="type">Type.</param>
		/// <param name="size">The size of the parameter.</param>		
		/// <param name="initialValue">The inital value of the parameter</param>		
		/// <returns>Instantiated IDbDataParameter.</returns>
		public IDbDataParameter CreateInputOutputParameter(string name, DbType type, int size, object initialValue)		
		{
			IDbDataParameter param = CreateInputOutputParameter(name, type, size);
			
			param.Value = initialValue;

			return param;
		}


		/// <summary>
		/// Creates an input/ouput parameter of the type specific to the classes data source.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="type">Type.</param>
		/// <returns>Instantiated IDbDataParameter.</returns>
		public IDbDataParameter CreateInputOutputParameter(string name, DbType type)		
		{
			return CreateInputOutputParameter(name, type, 0);
		}

				
		/// <summary>
		/// Creates an input/ouput parameter of the type specific to the classes data source.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="type">Type.</param>
		/// <param name="initialValue">The inital value of the parameter</param>		
		/// <returns>Instantiated IDbDataParameter.</returns>
		public IDbDataParameter CreateInputOutputParameter(string name, DbType type, object initialValue)		
		{
			return CreateInputOutputParameter(name, type, 0, initialValue);
		}


		/// <summary>
		/// Attaches an array of parameters to a command.
		/// </summary>
		/// <param name="cmd">Command to attach to.</param>
		/// <param name="parameters">Parameters to attach.</param>
		private void AttachParametersToCommand(IDbCommand cmd, IDbDataParameter[] parameters)
		{
			if (parameters != null)
				foreach (IDbDataParameter parameter in parameters)
					cmd.Parameters.Add(parameter);
		}


		/// <summary>
		/// Closes a connection which is attached to a command object.
		/// </summary>
		/// <param name="cmd">Command on which a connection is open.</param>
		private void CloseConnection(IDbCommand cmd)
		{
			if (cmd != null && cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
				cmd.Connection.Close();
		}
	}
}
