using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DiabloDiscordBot {
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application {
		void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
			// Process unhandled exception
			ILogger.Service.ErrorLine(e.Exception.Message + "\n" + e.Exception.StackTrace);

			// Prevent default unhandled exception processing
			e.Handled = true;
		}
	}
}
