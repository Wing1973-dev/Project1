using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using static IVMElectro.Services.ServiceIO;

namespace IVMElectro
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        public App() {
            Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += AppDomainUnhandledException;
        }

        void AppDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) {
            if (e.Exception.GetType() == typeof(FileNotFoundException)) {
                if (!CheckLibrary())
                    Current.Shutdown();
            }
            else
                ErrorReport(e.Exception.Message);
        }
        void AppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e) {
            if (e.ExceptionObject.GetType() == typeof(FileNotFoundException)) {
                if (!CheckLibrary())
                    Current.Shutdown(); 
            }
            else
                ErrorReport(e.ExceptionObject.ToString());
        }
        static bool CheckLibrary() {
            if (!IsResourceExist("LibraryAlgorithms.dll")) return false;
            return true;
        }
        static bool IsResourceExist(string fileName) {
            var process = Process.GetCurrentProcess();
            var path = process.MainModule.FileName.Replace("\\" + process.ProcessName + ".exe", "");
            try {
                if(!File.Exists(Path.Combine(path, fileName))) {
                    ErrorReport("Unable to load " + fileName + " library");
                    return false;
                }
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
