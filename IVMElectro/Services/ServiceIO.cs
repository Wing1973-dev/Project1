using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Diagnostics;

namespace IVMElectro.Services {
    /// <summary>
    /// Осуществляет вывод на экран/файл
    /// </summary>
    sealed class ServiceIO {
        static string pathFolder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        /// <summary>
        /// Имя открываемого файла
        /// </summary>
        private static string _file_name;

        /// <summary>
        /// Поток для записи лога
        /// </summary>
        private static StreamWriter log_sw;
        
        public static void WriteToLog(string s)
        {
            if (log_sw == null)
            {
                log_sw = new StreamWriter("IVMElectro.log");
                log_sw.Close();
            }
            
            log_sw = new StreamWriter("IVMElectro.log", true);
            log_sw.WriteLine(s);
            log_sw.Close();
        }

        public static string FileName
        {
            get => _file_name;
            set
            {
                
                 _file_name = value;
                
            }
        }

        public static void LaunchBrowser(string url)
        {
            string browserName = @"C:\Program Files\Internet Explorer\iexplore.exe";

            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice"))
            {
                if (userChoiceKey != null)
                {
                    object progIdValue = userChoiceKey.GetValue("Progid");
                    if (progIdValue != null)
                    {
                        if (progIdValue.ToString().ToLower().Contains("chrome"))
                        {
                            using (RegistryKey ChromeKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\chrome.exe"))
                            {
                                if (ChromeKey != null)
                                {
                                    object ChromeKeyPath = ChromeKey.GetValue("");
                                    if (ChromeKeyPath != null)
                                    {
                                        browserName = ChromeKeyPath.ToString();
                                    }
                                }
                            }
                        }
                        else if (progIdValue.ToString().ToLower().Contains("IE"))
                        {
                            using (RegistryKey IEKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\IEXPLORE.EXE"))
                            {
                                if (IEKey != null)
                                {
                                    object IEKeyPath = IEKey.GetValue("");
                                    if (IEKeyPath != null)
                                    {
                                        browserName = IEKeyPath.ToString();
                                    }
                                }
                            }
                        }
                        else if (progIdValue.ToString().ToLower().Contains("firefox"))
                        {
                            using (RegistryKey FireFoxKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\firefox.exe"))
                            {
                                if (FireFoxKey != null)
                                {
                                    object FireFoxPath = FireFoxKey.GetValue("");
                                    if (FireFoxPath != null)
                                    {
                                        browserName = FireFoxPath.ToString();
                                    }
                                }
                            }
                        }
                        else if (progIdValue.ToString().ToLower().Contains("opera"))
                        {
                            using (RegistryKey OperaKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\opera.exe"))
                            {
                                if (OperaKey != null)
                                {
                                    object OperaPath = OperaKey.GetValue("");
                                    if (OperaPath != null)
                                    {
                                        browserName = OperaPath.ToString();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Process.Start(new ProcessStartInfo(browserName, url));
        }

        /// <summary>
        /// Сохранение в файл по умолчанию
        /// </summary>
        /// <param name="saveData">построковое представление файла</param>
        public static bool SaveDataToFile_fixName(List<string> saveData) {
            string nameFile = Get_DateTimeNow() + ".txt";
            if (saveData.Count == 0) {
                ErrorReport("Создание файла " + nameFile + " невозможно.\r\nНедостаточно данных.");
                return false;
            }
            StreamWriter sr;
            try {
                sr = new StreamWriter(nameFile, false, Encoding.GetEncoding(1200));
            }
            catch (IOException e) {
                ErrorReport(e.Message);
                return false;
            }
            foreach (string item in saveData) sr.WriteLine(item);
            sr.Close(); return true;
        }
        /// <summary>
        /// Сохранение в файл с возможностью выбора папки пользователем
        /// </summary>
        /// <param name="saveData"></param>
        /// <param name="nameFile"></param>
        /// <returns></returns>
        public static bool SaveDataToFile(List<string> saveData) {
            string nameFile = Get_DateTimeNow() + ".txt";
            if (saveData.Count == 0) {
                ErrorReport("Создание файла " + nameFile + " невозможно.\r\nНедостаточно данных.");
                return false;
            }
            SaveFileDialog dlg = new SaveFileDialog() {
                Title = "Сохранение данных",
                InitialDirectory = pathFolder,
                Filter = "Текстовые файлы (*.txt)|*.txt",
                FileName = nameFile
            };
            if ( dlg.ShowDialog() == true)
                nameFile = dlg.FileName.Trim();
            if (nameFile != "") {
                pathFolder = nameFile.Substring(0, nameFile.LastIndexOf("\\")); //запоминает место сохранения
                StreamWriter sr;
                try {
                    sr = new StreamWriter(nameFile, false, Encoding.GetEncoding(1200));
                }
                catch (IOException e) {
                    ErrorReport(e.Message);
                    return false;
                }
                foreach (string item in saveData) sr.WriteLine(item);
                sr.Close();
                return true;
            }
            return false;
        }
        /// <summary>
        /// Сохранение XML в файл
        /// </summary>
        /// <param name="data"></param>
        /// <param name="nameFile">учитывается выбор пользователя</param>
        public static void SaveToFile(XElement data, string nameFile ) {
            string fileName = string.Empty;
            SaveFileDialog dlg = new SaveFileDialog() {
                Title = "Сохранение данных",
                InitialDirectory = pathFolder,
                Filter = "Файлы разметки (*.xml)|*.xml",
                FileName = nameFile
            };
            if ( dlg.ShowDialog() == true )
                fileName = dlg.FileName.Trim();
            if ( fileName != "" ) {
                pathFolder = fileName.Substring(0, fileName.LastIndexOf("\\"));
                using ( FileStream stream = new FileStream(fileName, FileMode.Create) ) {
                    data.Save(stream);
                }
            }
        }
        /// <summary>
        /// Сохранение XML в файл 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="nameFile">имя файла</param>
        public static void SaveToXMLFile_fixName( XElement data, string nameFile ) {
            string fileName = string.Empty;
            SaveFileDialog dlg = new SaveFileDialog() {
                Title = "Сохранение данных",
                InitialDirectory = pathFolder,
                Filter = "Файлы разметки (*.xml)|*.xml",
                FileName = nameFile
            };
            if ( dlg.ShowDialog() == true )
                fileName = dlg.FileName.Trim();
            if ( fileName != "" ) {
                pathFolder = fileName.Substring(0, fileName.LastIndexOf("\\"));
                string fix_name = pathFolder + "\\" + nameFile;
                using ( FileStream stream = new FileStream(fix_name, FileMode.Create) ) {
                    data.Save(stream);
                }
            }
        }
        public static string Get_OpenFileName() {
            string fileName = "";
            OpenFileDialog dlg = new OpenFileDialog() {
                Title = "Загрузка параметров расчета",
                InitialDirectory = pathFolder,
                Filter = "Файлы разметки (*.xml)|*.xml"
            };
            if ( dlg.ShowDialog() == true ) fileName = dlg.FileName;
            return fileName;
        }
        public static XElement LoadFromFile( string nameFile ) {
            XElement outData;
            using ( FileStream fs = new FileStream(nameFile, FileMode.Open) ) {
                outData = XElement.Load(fs);
            }
            return outData;
        }
        public static XElement LoadFromFile(ref string fileName) {
            fileName = Get_OpenFileName();
            _file_name = fileName;
            return !string.IsNullOrEmpty(fileName) ? LoadFromFile(fileName) : null;
        }
        /// <summary>
        /// Сохранить объект класса в XML-файл по умолчанию
        /// </summary>
        /// <param name="data">Объект класса для сериализации</param>
        public static void SaveObjectToXMLFile_fixName(object data) {
            XmlSerializer s = new XmlSerializer(data.GetType());
            StreamWriter sw = new StreamWriter(Get_DateTimeNow() + ".xml", false, Encoding.Unicode);
            s.Serialize(sw, data);
            sw.Close();
        }
        /// <summary>
        /// Сохранить объект класса в XML-файл с выбором имени файла
        /// </summary>
        /// <param name="data">Объект класса для сериализации</param>
        public static string SaveObjectToXMLFile(object data) {
            string nameFile = Get_DateTimeNow() + ".xml";
            SaveFileDialog dlg = new SaveFileDialog() {
                Title = "Сохранение данных",
                InitialDirectory = pathFolder,
                Filter = "Файлы разметки (*.xml)|*.xml",
                FileName = nameFile
            };
            if (dlg.ShowDialog() == true)
                nameFile = dlg.FileName.Trim();
            if (nameFile != "") {
                pathFolder = nameFile.LastIndexOf("\\") != -1 ? nameFile.Substring(0, nameFile.LastIndexOf("\\")) : pathFolder; //запоминает место сохранения
                XmlSerializer s = new XmlSerializer(data.GetType());
                StreamWriter sw;
                try {
                    sw = new StreamWriter(nameFile, false, Encoding.Unicode);
                }
                catch (IOException e) {
                    ErrorReport(e.Message);
                    return string.Empty;
                }
                s.Serialize(sw, data);
                sw.Close();
            }
            return nameFile;
        }
        /// <summary>
        /// Загрузить (десериализовать) объект класса из XML-файла
        /// </summary>
        /// <param name="sFilename">Имя XML-файла</param>
        /// <param name="encoding">Кодировка файла</param>
        /// <param name="tDataType">Тип класса</param>
        /// <returns>Объект десериализированного класса</returns>
        public static object LoadXMLFile(string sFilename, Encoding encoding, Type tDataType) {
            XmlSerializer s = new XmlSerializer(tDataType);
            StreamReader sr = new StreamReader(sFilename, encoding);
            object o = s.Deserialize(sr);
            sr.Close();
            return o;
        }
        /// <summary>
        /// Вывод сообщения об ошибке
        /// </summary>
        /// <param name="message">сообщения об ошибке</param>
        public static void ErrorReport(string message) => MessageBox.Show(message, "Ошибка данных.", MessageBoxButton.OK, MessageBoxImage.Error);
        /// <summary>
        /// Вывод сервисного сообщения
        /// </summary>
        /// <param name="message">сервисное сообщение</param>
        public static void ServiceReport( string message ) => MessageBox.Show(message, "Сервисное сообщение.", MessageBoxButton.OK, MessageBoxImage.Information);
        /// <summary>
        /// Сервисный диалог
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <returns>выбор пользователя</returns>
        public static MessageBoxResult ServiceDialog( string message ) => MessageBox.Show(message, "Сервисное сообщение.", MessageBoxButton.YesNo, MessageBoxImage.Warning);
        public static string Get_pathFolder {
            get {
                if ( !pathFolder.EndsWith("\\") ) {
                    pathFolder += "\\";
                }
                return pathFolder;
            }
        }
        public static DirectoryInfo Get_DirectoryInfo => new DirectoryInfo(Environment.CurrentDirectory);
        public static string Get_DateTimeNow() => DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss",
                    new System.Globalization.CultureInfo(System.Globalization.CultureInfo.InstalledUICulture.Name));
        public static StreamReader GetReader( string nameFile ) {
            FileStream rFile = null;
            StreamReader sr = null;
            try {
                rFile = new FileStream(nameFile, FileMode.Open, FileAccess.Read);
                sr = new StreamReader(rFile, Encoding.GetEncoding(1251));
            }
            catch ( IOException e ) {
                rFile.Dispose();
                Console.WriteLine(e.ToString());
            }
            return sr;
        }
        public static StreamWriter GetWriter( string nameFile ) {
            StreamWriter sw = null;
            try {
                sw = new StreamWriter(nameFile, false, Encoding.GetEncoding(1251));
            }
            catch ( IOException e ) {
                sw.Dispose();
                Console.WriteLine(e.Message);
            }
            return sw;
        }
    }
}
