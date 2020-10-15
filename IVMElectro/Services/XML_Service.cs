using System.Collections.Generic;
using System.Xml.Linq;

namespace IVMElectro.Services {
    /// <summary>
    /// Функции XML-преобразования данных
    /// </summary>
    public class XML_Service {
        /// <summary>
        /// Преобразует таблично-строковое представление данных в xml-представление
        /// </summary>
        /// <param name="input_List">таблично-строковое представление</param>
        /// <param name="out_xmlElement">xml-представление данных</param>
        public static void CollectionToXml(List<string> input_List, XElement out_xmlElement) {
            foreach (string stringContent in input_List)
                out_xmlElement.Add(new XElement("string", new XAttribute("value", stringContent)));
        }
        /// <summary>
        ///  Преобразует xml-представление представление данных в таблично-строковое
        /// </summary>
        /// <param name="in_xmlElement">xml-представление данных</param>
        /// <param name="out_List">таблично-строковое представление</param>
        public static void XMLToCollection(XElement in_xmlElement, ref List<string> out_List) {
            if (in_xmlElement.HasElements)
                foreach (XElement item in in_xmlElement.Elements())
                    out_List.Add(item.Attribute("value").Value);
        }
        /// <summary>
        /// Осуществляет преобразование xml данных в txt представление
        /// </summary>
        /// <param name="node">xml представленик данных</param>
        /// <returns>txt представление данных</returns>
        public static string XMLToTxt(XElement node) {
            if (node.HasAttributes)
                return node.Attribute("value").Value;
            return "";
        }
        public static string XMLToTxtStandard(XElement node) => node.Value;
        /// <summary>
        /// Осуществляет преобразование txt данных в xml представление с атрибутом
        /// </summary>
        /// <param name="nodeName">имя узла</param>
        /// <param name="value">атрибут value - данные для преобразования</param>
        /// <returns>XElement</returns>
        public static XElement TxtToXML(string nodeName, string value) => new XElement(nodeName, new XAttribute("value", value));
    }
}
