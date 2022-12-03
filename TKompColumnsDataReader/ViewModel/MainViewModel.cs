using System.Security;
using System.Windows.Input;
using TKompColumnsDataReader.Model;
using TKompColumnsDataReader.Utility;
using TKompColumnsDataReader.ViewModel.Base;
using TKompColumnsDataReader.Interface;
using System.Collections.ObjectModel;

namespace TKompColumnsDataReader.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		#region properties

		private string _login;
		public string Login
		{
			get { return _login; }
			set
			{
				this._login = value;
				DisableLoadDataBtn();
				OnPropertyChanged();
			}
		}

		private SecureString _password;
		public SecureString Password
		{
			get { return _password; }
			set
			{
				DisableLoadDataBtn();
				_password = value;
			}
		}

		private ObservableCollection<ColumnData> _columns;
		public ObservableCollection<ColumnData> Columns
		{
			get { return this._columns; }
			set
			{
				_columns = value;
				OnPropertyChanged();
			}
		}

		private bool _loadDataBtnIsEnabled;
		public bool LoadDataBtnIsEnabled
		{
			get { return this._loadDataBtnIsEnabled; }
			set
			{
				_loadDataBtnIsEnabled = value;
				OnPropertyChanged();
			}
		}

		private IDbService _dbService;
		#endregion

		#region init
		public MainViewModel(IDbService dbProvider)
		{
			_dbService = dbProvider;

			InitializeFields();
		}

		private void InitializeFields()
		{
			Columns = new ObservableCollection<ColumnData>();
			LoadDataBtnIsEnabled = false;
			Login = string.Empty;
			Password = new SecureString();
		}

		#endregion

		#region commands

		private ICommand _commandCheckConnection;
		public ICommand CommandCheckConnection
		{
			get
			{
				return _commandCheckConnection ?? (_commandCheckConnection = new CommandHandler(() => CheckConnection(), true));
			}
		}
		private void CheckConnection()
		{
			ClearColumns();

			if (!ValidFields())
			{
				MessageBoxes.ShowErrorMessage("Please enter Login and Password");
				return;
			}

			_dbService.SetUpCredentials(Login, Password);

			if (_dbService.CheckConnection(out var message))
			{
				EnableLoadDataBtn();
				MessageBoxes.ShowMessage(message);
			}
			else
			{
				DisableLoadDataBtn();
				MessageBoxes.ShowErrorMessage(message);
			}
		}

		
		private ICommand _commandLoadData;
		public ICommand CommandLoadData
		{
			get
			{
				return _commandLoadData ?? (_commandLoadData = new CommandHandler(() => LoadData(), true));
			}
		}
		private void LoadData()
		{
			ClearColumns();

			var result = _dbService.GetDbIntColumns(out var errorMessage);
			Columns = result;

			if (!string.IsNullOrWhiteSpace(errorMessage))
			{
				MessageBoxes.ShowErrorMessage(errorMessage);
			}
		}

		#endregion

		#region methods
		private void EnableLoadDataBtn() => LoadDataBtnIsEnabled = true;

		private void DisableLoadDataBtn() => LoadDataBtnIsEnabled = false;

		private void ClearColumns() => Columns.Clear();

		private bool ValidFields() => !string.IsNullOrWhiteSpace(Login) && Password.Length != 0;

		public void SetPassword(SecureString password) => Password = password;

		#endregion
	}
}
