using System;
using System.Diagnostics;
using System.IO;

namespace IVMElectroLauncher {
    class Program {
        static void Main(string[] args) {
            Process processIVME = null;
            if (CheckLibrary() && CheckJson()) {
                try {
                    processIVME = new Process();
                    processIVME.StartInfo.FileName =
                        Process.GetCurrentProcess().MainModule.FileName.Replace($"{Process.GetCurrentProcess().ProcessName}.exe",
                        "IVMElectro.exe");
                    processIVME.Start();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message + $": {processIVME.StartInfo.FileName}");
                    Console.ReadLine();
                }
            }
        }
        static bool CheckLibrary() {
            if (!IsResourceExist("LibraryAlgorithms.dll")) return false;
            if (!IsResourceExist("MaterialDesignColors.dll")) return false;
            if (!IsResourceExist("MaterialDesignThemes.Wpf.dll")) return false;
            if (!IsResourceExist("NLog.dll")) return false;
            if (!IsResourceExist("IVMElectro.dll")) return false;
            return true;
        }
        static bool CheckJson() {
            if (!IsResourceExist("IVMElectro.deps.json")) return false;
            if (!IsResourceExist("IVMElectro.runtimeconfig.json")) return false;
            if (!IsResourceExist("IVMElectroLauncher.deps.json")) return false;
            if (!IsResourceExist("IVMElectroLauncher.runtimeconfig.json")) return false;
            if (!IsResourceExist("NLog.config")) return false;
            return true;
        }
        static bool IsResourceExist(string fileName) {
            var process = Process.GetCurrentProcess();
            var path = process.MainModule.FileName.Replace($"\\{process.ProcessName}.exe", "");
            try {
                if (!File.Exists(Path.Combine(path, fileName))) {
                    Console.WriteLine($"Unable to load {fileName} library");
                    Console.ReadLine();
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
