using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using TKompColumnsDataReader.Utility;
using TKompColumnsDataReader.ViewModel;

namespace TKompColumnsDataReader.View
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();

			var config = new ConfigurationBuilder().AddUserSecrets(Assembly.GetExecutingAssembly(), true).Build();

            var connectionString = config.GetConnectionString("DevDataCon");

            DataContext = new MainViewModel(new DbService(connectionString));
        }

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
            var password = ((PasswordBox)sender).SecurePassword;

			((MainViewModel)DataContext).SetPassword(password); 
		}
	}
}
