using System.Collections.Generic;
using IVMElectro.Services.Directories.WireDirectory;

namespace IVMElectro.Services.Directories {
    public class ContentStatorImageControl {
        public ContentStatorImageControl(List<ContentLabelImage> contentLabels) {
            foreach (ContentLabelImage item in contentLabels) {
                switch (item.NameLabel) {
                    case "h1": {
                            h1Content = item.Content; h1Left = item.Left; h1Top = item.Top;
                        } break;
                    case "h2": {
                            h2Content = item.Content; h2Left = item.Left; h2Top = item.Top;
                        }
                        break;
                    case "h3": {
                            h3Content = item.Content; h3Left = item.Left; h3Top = item.Top;
                        }
                        break;
                    case "h4": {
                            h4Content = item.Content; h4Left = item.Left; h4Top = item.Top;
                        }
                        break;
                    case "h5": {
                            h5Content = item.Content; h5Left = item.Left; h5Top = item.Top;
                        }
                        break;
                    case "h6": {
                            h6Content = item.Content; h6Left = item.Left; h6Top = item.Top;
                        }
                        break;
                    case "h7": {
                            h7Content = item.Content; h7Left = item.Left; h7Top = item.Top;
                        }
                        break;
                    case "h8": {
                            h8Content = item.Content; h8Left = item.Left; h8Top = item.Top;
                        }
                        break;
                    case "ΔГ1": {
                            ΔГ1Content = item.Content; ΔГ1Left = item.Left; ΔГ1Top = item.Top;
                        }
                        break;
                    case "ac": {
                            acContent = item.Content; acLeft = item.Left; acTop = item.Top;
                        }
                        break;
                    case "bПН": {
                            bПНContent = item.Content; bПНLeft = item.Left; bПНTop = item.Top;
                        }
                        break;
                    case "bП": {
                            bПContent = item.Content; bПLeft = item.Left; bПTop = item.Top;
                        }
                        break;
                    case "bП1": {
                            bП1Content = item.Content; bП1Left = item.Left; bП1Top = item.Top;
                        }
                        break;
                    case "d1": {
                            d1Content = item.Content; d1Left = item.Left; d1Top = item.Top;
                        }
                        break;
                }
            }
        }
        public string ImageSource { get; set; }
        public string h1Content { get; private set; }
        public string h1Left { get; private set; }
        public string h1Top { get; private set; }
        public string h2Content { get; private set; }
        public string h2Left { get; private set; }
        public string h2Top { get; private set; }
        public string h3Content { get; private set; }
        public string h3Left { get; private set; }
        public string h3Top { get; private set; }
        public string h4Content { get; private set; }
        public string h4Left { get; private set; }
        public string h4Top { get; private set; }
        public string h5Content { get; private set; }
        public string h5Left { get; private set; }
        public string h5Top { get; private set; }
        public string h6Content { get; private set; }
        public string h6Left { get; private set; }
        public string h6Top { get; private set; }
        public string h7Content { get; private set; }
        public string h7Left { get; private set; }
        public string h7Top { get; private set; }
        public string h8Content { get; private set; }
        public string h8Left { get; private set; }
        public string h8Top { get; private set; }
        public string ΔГ1Content { get; private set; }
        public string ΔГ1Left { get; private set; }
        public string ΔГ1Top { get; private set; }
        public string acContent { get; private set; }
        public string acLeft { get; private set; }
        public string acTop { get; private set; }
        public string bПНContent { get; private set; }
        public string bПНLeft { get; private set; }
        public string bПНTop { get; private set; }
        public string bПContent { get; private set; }
        public string bПLeft { get; private set; }
        public string bПTop { get; private set; }
        public string bП1Content { get; private set; }
        public string bП1Left { get; private set; }
        public string bП1Top { get; private set; }
        public string d1Content { get; private set; }
        public string d1Left { get; private set; }
        public string d1Top { get; private set; }
    }
}
