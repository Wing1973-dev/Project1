using System;
using System.Collections.Generic;
using System.Text;

namespace IVMElectro.Services.Directories.WireDirectory {
    public struct ContentLabelImage {
        public string NameLabel;
        public string Left;
        public string Top;
        public string Content;
        public ContentLabelImage(string name, string content, string left, string top) {
            NameLabel = name; Content = content; Left = left; Top = top;
        }
    }
}
