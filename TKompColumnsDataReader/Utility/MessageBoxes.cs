using System.Windows;

namespace TKompColumnsDataReader.Utility
{
	public static class MessageBoxes
    {
        public static void ShowErrorMessage(string message) => 
			Show("Error", message, MessageBoxImage.Error);

		public static void ShowMessage(string message) => 
			Show("Success", message, MessageBoxImage.Information);

		private static void Show(string caption, string message, MessageBoxImage icon) => 
			MessageBox.Show(message, caption, MessageBoxButton.OK, icon, MessageBoxResult.Yes);
	}
}
