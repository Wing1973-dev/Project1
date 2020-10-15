using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace LibraryAlgorithm.Services {
    /// <summary>
    /// Функции преобразования данных
    /// </summary>
    public class ServiceDT {
        /// <summary>
        /// Символ разделителя слов в строке
        /// </summary>
        public static char[] Get_blankDelim { get; } = new char[] { ' ', '\t' };
        /// <summary>
        /// Символ разделителя целой и дробной части для вещественного числа
        /// </summary>
        public static string Get_decimalSeparator { get; } = NumberFormatInfo.CurrentInfo.NumberDecimalSeparator;
        /// <summary>
        /// Разделитель в файле пользовательского ввода ( ( , ) )
        /// </summary>
        public static char[] Get_bracketDelim { get; } = new char[] { '(', ')' };
        public static char[] Get_specificDelim { get; } = new char[] { '+' };
        /// <summary>
        /// Проверяет повторяемость элементов в массиве
        /// </summary>
        /// <param name="line">массив для проверки</param>
        /// <returns>true - нет повторяющихся элементов, false - есть повторяющиеся элементы</returns>
        public static bool CheckingHit<T>( T[] line ) {
            for ( int i = 0; i < line.Length; i++ )
                if ( Array.IndexOf(line, line[i]) != Array.LastIndexOf(line, line[i]) )
                    return false;
            return true;
        }
        /// <summary>
        /// Преобразование строки в массив строк
        /// </summary>
        /// <param name="in_date">исходная строка</param>
        /// <param name="separator">разделитель элементов в строке</param>
        /// <returns>массив строк</returns>
        public static string[] StringToStringArray( string in_date, char[] separator ) {
            string[] words;
            in_date.Trim();
            words = in_date.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            for ( int i = 0; i < words.Length; i++ )
                words[i] = words[i].Trim();
            return words;
        }
        public static string[] StringToStringArray( string in_date, char separator ) {
            in_date.Trim();
            string[] words = in_date.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            for ( int i = 0; i < words.Length; i++ )
                words[i] = words[i].Trim();
            return words;
        }
        /// <summary>
        /// Перевод содержимого строки в double[]
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>массив значений</returns>
        public static double[] StringToArrayDouble( string in_date ) {
            in_date.Trim();
            string[] words = in_date.Split(Get_blankDelim, StringSplitOptions.RemoveEmptyEntries);
            double[] out_date = new double[words.Length];
            for ( int i = 0; i < words.Length; i++ ) {
                words[i].Trim(); //удаление символа табуляции
                //замена разделителей дробной части
                words[i] = words[i].Replace(",", Get_decimalSeparator); words[i] = words[i].Replace(".", Get_decimalSeparator);
                if ( double.TryParse(words[i], out double outv) ) out_date[i] = outv;
            }
            return out_date;
        }
        /// <summary>
        /// Перевод содержимого строки в List (double)
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>List double</returns>
        public static List<double> StringToListDouble( string in_date ) {
            in_date.Trim();
            string[] words = in_date.Split(Get_blankDelim, StringSplitOptions.RemoveEmptyEntries);
            List<double> out_date = new List<double>(words.Length);
            for ( int i = 0; i < words.Length; i++ ) {
                words[i].Trim(); //удаление символа табуляции
                //замена разделителей дробной части
                words[i] = words[i].Replace(",", Get_decimalSeparator); words[i] = words[i].Replace(".", Get_decimalSeparator);
                if ( double.TryParse(words[i], out double outv) ) out_date.Add(outv);
                else { out_date = null; break; }
            }
            return out_date;
        }
        /// <summary>
        /// Перевод содержимого строки в List (int)
        /// </summary>
        /// <param name="in_date"></param>
        /// <returns>List int</returns>
        public static List<int> StringToListInt( string in_date ) {
            in_date.Trim();
            string[] words = in_date.Split(Get_blankDelim, StringSplitOptions.RemoveEmptyEntries);
            List<int> out_date = new List<int>(words.Length);
            for ( int i = 0; i < words.Length; i++ ) {
                words[i].Trim(); //удаление символа табуляции
                if ( int.TryParse(words[i], out int outv) ) out_date.Add(outv);
                else { out_date = null; break; }
            }
            return out_date;
        }
        /// <summary>
        /// Перевод содержимого строки в List (string)
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>List string</returns>
        public static List<string> StringToListString( string in_date, char separator ) {
            in_date.Trim();
            string[] words = in_date.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            List<string> out_date = new List<string>(words.Length);
            for ( int i = 0; i < words.Length; i++ ) 
                out_date.Add(words[i].Trim());
            return out_date;
        }
        public static List<string> StringToListString( string in_date, char[] separator ) {
            in_date.Trim();
            string[] words = in_date.Split( separator, StringSplitOptions.RemoveEmptyEntries);
            List<string> out_date = new List<string>(words.Length);
            for ( int i = 0; i < words.Length; i++ ) 
                out_date.Add(words[i].Trim());
            return out_date;
        }
        /// <summary>
        /// Перевод содержимого строки в double
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>значение</returns>
        public static double StringToDouble( string in_date ) {
            in_date = in_date.Replace(",", Get_decimalSeparator); in_date = in_date.Replace(".", Get_decimalSeparator);
            in_date.Trim();
            if ( double.TryParse(in_date, out double outv) ) return outv;
            return double.NaN;
        }
        /// <summary>
        /// Перевод содержимого строки в int[]
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>массив значений</returns>
        public static int[] StringToArrayInt( ref string in_date ) {
            in_date.Trim();
            string[] words = in_date.Split(Get_blankDelim, StringSplitOptions.RemoveEmptyEntries);
            int[] out_date = new int[words.Length];
            for ( int i = 0; i < out_date.Length; i++ ) {
                words[i].Trim();
                if ( int.TryParse(words[i], out int outv) ) out_date[i] = outv;
                else { out_date = null; return out_date; }
            }
            if ( out_date.Length == 0 ) out_date = null;
            return out_date;
        }
        /// <summary>
        /// Перевод содержимого строки в int
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>значение</returns>
        public static int StringToInt( string in_date ) {
            in_date.Trim();
            if ( int.TryParse(in_date, out int outv) ) return outv;
            return -1;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в double и проверка значения на >= 0
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToDoubleGEZ( string in_date ) {
            in_date = in_date.Replace(",", Get_decimalSeparator); in_date = in_date.Replace(".", Get_decimalSeparator);
            in_date.Trim();
            if ( !double.TryParse(in_date, out double r) || double.Parse(in_date) < 0 ) return false;
            return true;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в double и проверка значения на меньше 0
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToDoubleLZ( string in_date ) {
            in_date = in_date.Replace(",", Get_decimalSeparator); in_date = in_date.Replace(".", Get_decimalSeparator);
            in_date.Trim();
            if ( !double.TryParse(in_date, out double r) || double.Parse(in_date) >= 0 )
                return false;
            return true;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в double и проверка значения на > 0
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToDoubleGZ( string in_date ) {
            in_date = in_date.Replace(",", Get_decimalSeparator); in_date = in_date.Replace(".", Get_decimalSeparator);
            in_date.Trim();
            if ( !double.TryParse(in_date, out double r) || double.Parse(in_date) <= 0 )
                return false;
            return true;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в double и проверка значения на != 0
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToDoubleNZ( string in_date ) {
            in_date = in_date.Replace(",", Get_decimalSeparator); in_date = in_date.Replace(".", Get_decimalSeparator);
            in_date.Trim();
            if ( !double.TryParse(in_date, out double r) || double.Parse(in_date) == 0 ) return false;
            return true;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в double 
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToDouble_feasible( string in_date ) {
            in_date = in_date.Replace(",", Get_decimalSeparator); in_date = in_date.Replace(".", Get_decimalSeparator);
            in_date.Trim();
            if ( !double.TryParse(in_date, out double r) ) return false;
            return true;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в int и проверка значения на > 0
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToIntGZ( string in_date ) {
            in_date.Trim();
            if ( !int.TryParse(in_date, out int r) || int.Parse(in_date) <= 0 )
                return false;
            return true;
        }
        /// <summary>
        /// Проверка возможности перевода содержимого строки в int и проверка значения на >= 0
        /// </summary>
        /// <param name="in_date">строка</param>
        /// <returns>true - успешно, false - не успешно</returns>
        public static bool StringToIntGEZ( string in_date ) {
            in_date.Trim();
            if ( !int.TryParse(in_date, out int r) || int.Parse(in_date) < 0 )
                return false;
            return true;
        }
        /// <summary>
        /// Преобразование array в string
        /// </summary>
        /// <param name="array">array представление</param>
        /// <returns>string представление</returns>
        public static string ArrayToString<T>( T[] array ) {
            StringBuilder outString = new StringBuilder(100);
            for ( int i = 0; i < array.Length; i++ )
                outString.AppendFormat("{0} ", array[i]);
            return typeof(T) == typeof(string) ? outString.ToString().TrimEnd(Get_blankDelim[0]) : outString.ToString();
        }
        /// <summary>
        /// Преобразование List в string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ListToString<T>( List<T> list ) {
            if ( list == null ) return "";
            StringBuilder outString = new StringBuilder(100);
            for ( int i = 0; i < list.Count; i++ )
                outString.AppendFormat("{0} ", list[i]);
            return typeof(T) == typeof(string) ? outString.ToString().TrimEnd(Get_blankDelim[0]) : outString.ToString();
        }
        public static string ListToString<T>( List<T> list, char separator ) {
            StringBuilder outString = new StringBuilder(100);
            for ( int i = 0; i < list.Count; i++ ) {
                outString.AppendFormat("{0}", list[i]); outString.Append(separator.ToString());
            }
            return outString.ToString().TrimEnd(new char[] { separator });
        }
    }
}
