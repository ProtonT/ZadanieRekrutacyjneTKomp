using System.Collections.ObjectModel;
using System.Security;
using TKompColumnsDataReader.Model;

namespace TKompColumnsDataReader.Interface
{
	public interface IDbService
	{
		void SetUpCredentials(string login, SecureString password);
		bool CheckConnection(out string message);
		ObservableCollection<ColumnData> GetDbIntColumns(out string errorMessage);
	}
}
