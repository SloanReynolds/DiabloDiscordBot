using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiabloDiscordBot {
	/// <summary>
	/// Interaction logic for ConsoleOutput.xaml
	/// </summary>
	public partial class ConsoleOutput : UserControl, ILogger {
		public ConsoleOutput() {
			InitializeComponent();
		}

		public void WriteLine(string text) {
			WriteLine(text, Colors.White);
		}

		public void WriteLine(string text, Color color) {
			Debug.WriteLine(text);
			Application.Current.Dispatcher.Invoke(() => _WriteLine(text, color));
		}

		public void ErrorLine(string text) {
			WriteLine(text, Colors.Red);
		}





		private void _WriteLine(string text, Color color) {
			TextBox newText = new();
			newText.TextWrapping = TextWrapping.Wrap;
			newText.Foreground = new SolidColorBrush(color);
			newText.IsReadOnly = true;
			newText.Background = new SolidColorBrush(Colors.Transparent);
			newText.BorderThickness = new Thickness(0);
			newText.Text = text;

			stackPanel.Children.Add(newText);

			while (stackPanel.Children.Count >= 201) {
				stackPanel.Children.RemoveAt(0);
			}

			scrollViewer.ScrollToBottom();
		}
	}
}
