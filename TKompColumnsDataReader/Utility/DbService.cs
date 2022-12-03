using System;
using System.Data.SqlClient;
using System.Data;
using System.Security;
using TKompColumnsDataReader.Interface;
using TKompColumnsDataReader.Model;
using System.Collections.ObjectModel;

namespace TKompColumnsDataReader.Utility
{
	public class DbService : IDbService
	{
		private string _connectionString;

		private string _login;
		private SecureString _password;

		public DbService(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				MessageBoxes.ShowErrorMessage("Connection string is empty");
			}
			
			_connectionString = connectionString;
		}

		public void SetUpCredentials(string login, SecureString password)
		{
			_login = login;
			_password = password;
		}

		public bool CheckConnection(out string message)
		{
			message = string.Empty;
			var result = false;

			try
			{
				var password = _password;
				password?.MakeReadOnly();

				var sqlCredential = new SqlCredential(_login, password);

				using (var connection = new SqlConnection(_connectionString, sqlCredential))
				{
					connection.Open();

					if (connection.State == ConnectionState.Open)
					{
						result = true;
						message = "Succesfully connected to database";
					}

					connection.Close();
				}
			}
			catch (Exception exception) 
			{
				result = false;
				message = exception.Message;
			}

			return result;
		}

		public ObservableCollection<ColumnData> GetDbIntColumns(out string errorMessage)
		{
			errorMessage = string.Empty;
			var result = new ObservableCollection<ColumnData>();
			var query = @"SELECT tab.name tab_name, col.name col_name
			                FROM 
			                     sys.columns col
			               INNER JOIN
			                     sys.tables tab ON col.object_id = tab.object_id
			               INNER JOIN
			                     sys.types typ ON col.user_type_id = typ.user_type_id
			               WHERE typ.name = 'int'";

			try
			{
				var password = _password;
				password?.MakeReadOnly();

				var sqlCredential = new SqlCredential(_login, _password);

				using (var connection = new SqlConnection(_connectionString, sqlCredential))
				{
					connection.Open();

					using (var command = new SqlCommand(query, connection))
					{
						var reader = command.ExecuteReader();

						var table = new DataTable();
						table.Load(reader);

						foreach (DataRow dataRow in table.Rows)
						{
							var colName = dataRow["col_name"]?.ToString();
							var tabName = dataRow["tab_name"]?.ToString();

							result.Add(new ColumnData(colName, tabName));
						}
					}

					connection.Close();

					return result;
				}
			}
			catch (Exception exception)
			{
				errorMessage = exception.Message;
				return result;
			}
		}
	}
}
