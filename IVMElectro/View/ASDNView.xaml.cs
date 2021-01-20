using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using static IVMElectro.Services.ServiceIO;
using IVMElectro.ViewModel;
using IVMElectro.Services.Directories;

namespace IVMElectro.View {
    /// <summary>
    /// Interaction logic for ASDNView.xaml
    /// </summary>
    public partial class ASDNView : Window {
        public ASDNView() => InitializeComponent();

        private void TxtBx_GotFocus(object sender, RoutedEventArgs e) => UpdateBinding();

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => UpdateBinding();
        void UpdateBinding() {
            BindingOperations.GetBindingExpression(lbPмехBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbxPmex, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lbDaBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbxDa, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lboxZ1, ItemsControl.ItemsSourceProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lbox_a1, ItemsControl.ItemsSourceProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbl_bП1, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbl_bПН, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_acBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_ac, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbl_h1, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_liBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbl_bП, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_y1Bounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbl_β, TextBlock.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_d1Bounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_ρ1ГBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lbDpстBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_hpBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_hp, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_dвBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_dв, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_bкBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_bк, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_aкBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_aк, TextBox.TextProperty).UpdateTarget();
            //BindingOperations.GetBindingExpression(cbx_p, System.Windows.Controls.Primitives.Selector.SelectedValueProperty).UpdateTarget();
            //BindingOperations.GetBindingExpression(cbxPЗ, System.Windows.Controls.Primitives.Selector.SelectedValueProperty).UpdateTarget();
            //BindingOperations.GetBindingExpression(cbx_bСК, System.Windows.Controls.Primitives.Selector.SelectedValueProperty).UpdateTarget();

            BindingOperations.GetBindingExpression(tblDiagnostic, TextBlock.TextProperty).UpdateTarget();
        }
        private void btnMenu_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "btnWires": {
                        WiresDirectory wires = new WiresDirectory(((AsdnSingleViewModel)DataContext).WireDirectory) {
                            Owner = this
                        };
                        wires.Show();
                    } break;
                case "btnK2": {
                        K2Directory k2 = new K2Directory() {
                            DataContext = DataContext,
                            Owner = this 
                        };
                        k2.Show();
                    } break;
                case "btn_ρРУБ": {
                        SteelPropertiesPartition propertiesPartition = new SteelPropertiesPartition() {
                            DataContext = DataContext,
                            Owner = this
                        };
                        propertiesPartition.Show();
                    } break;
                case "btn_p1050": {
                        SteelPropertiesStator propertiesStator = new SteelPropertiesStator() {
                            DataContext = DataContext,
                            Owner = this
                        };
                        propertiesStator.Show();
                    } break;
                case "btn_γ": {
                        SteelPropertiesRotor propertiesRotor = new SteelPropertiesRotor() {
                            DataContext = DataContext,
                            Owner = this
                        };
                        propertiesRotor.Show();
                    } break;
                case "btnZ2": {
                        Z2View z2View = new Z2View() {
                            DataContext = ((AsdnSingleViewModel)DataContext).Get_collectionZ2,
                            Owner = this
                        };
                        z2View.Show();
                    } break;
                case "btn_ImageStator": {
                        string h2 = Math.Round(((AsdnSingleViewModel)DataContext).Model.Common.h2, 2).ToString();
                        #region create windows
                        #region first window
                        List<ContentLabelImage> contentLabels = new List<ContentLabelImage> {
                            new ContentLabelImage("h1", ((AsdnSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "320", "200"),
                            new ContentLabelImage("h3", ((AsdnSingleViewModel)DataContext).h3, "335", "240"),
                            new ContentLabelImage("h4", ((AsdnSingleViewModel)DataContext).h4, "320", "350"),
                            new ContentLabelImage("h5", ((AsdnSingleViewModel)DataContext).h5, "335", "405"),
                            new ContentLabelImage("h6", ((AsdnSingleViewModel)DataContext).h6, "320", "442"),
                            new ContentLabelImage("h7", ((AsdnSingleViewModel)DataContext).h7, "335", "462"),
                            new ContentLabelImage("h8", ((AsdnSingleViewModel)DataContext).h8, "320", "505"),
                            new ContentLabelImage("ΔГ1", ((AsdnSingleViewModel)DataContext).ΔГ1, "17", "530"),
                            new ContentLabelImage("ac", ((AsdnSingleViewModel)DataContext).ac, "145", "507"),
                            new ContentLabelImage("bПН", ((AsdnSingleViewModel)DataContext).bПН, "145", "543"),
                            new ContentLabelImage("bП", ((AsdnSingleViewModel)DataContext).bП, "140", "8"),
                            new ContentLabelImage("bП1", ((AsdnSingleViewModel)DataContext).bП1, "145", "581")
                        };
                        ContentStatorImageControl contentStator = new ContentStatorImageControl(contentLabels) {
                            ImageSource = "pack://application:,,,/Resource/Stator1.JPG"
                        };
                        UserControlStatorImage statorImage1 = new UserControlStatorImage {
                            DataContext = contentStator
                        };
                        #endregion
                        #region second window
                        contentLabels = new List<ContentLabelImage> {
                            new ContentLabelImage("h1", ((AsdnSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "320", "200"),
                            new ContentLabelImage("h3", ((AsdnSingleViewModel)DataContext).h3, "335", "240"),
                            new ContentLabelImage("h4", ((AsdnSingleViewModel)DataContext).h4, "320", "350"),
                            new ContentLabelImage("h5", ((AsdnSingleViewModel)DataContext).h5, "335", "405"),
                            new ContentLabelImage("h6", ((AsdnSingleViewModel)DataContext).h6, "320", "445"),
                            new ContentLabelImage("h7", ((AsdnSingleViewModel)DataContext).h7, "335", "462"),
                            new ContentLabelImage("h8", ((AsdnSingleViewModel)DataContext).h8, "320", "510"),
                            new ContentLabelImage("ΔГ1", ((AsdnSingleViewModel)DataContext).ΔГ1, "17", "530"),
                            new ContentLabelImage("ac", ((AsdnSingleViewModel)DataContext).ac, "145", "510"),
                            new ContentLabelImage("bПН", ((AsdnSingleViewModel)DataContext).bПН, "145", "552"),
                            new ContentLabelImage("bП", ((AsdnSingleViewModel)DataContext).bП, "140", "5"),
                            new ContentLabelImage("d1", ((AsdnSingleViewModel)DataContext).d1, "145", "587")
                        };
                        contentStator = new ContentStatorImageControl(contentLabels) {
                            ImageSource = "pack://application:,,,/Resource/Stator2.jpg"
                        };
                        UserControlStatorImage statorImage2 = new UserControlStatorImage {
                            DataContext = contentStator
                        };
                        #endregion
                        #region third window
                        contentLabels = new List<ContentLabelImage> {
                            new ContentLabelImage("h1", ((AsdnSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "330", "210"),
                            new ContentLabelImage("h3", ((AsdnSingleViewModel)DataContext).h3, "315", "270"),
                            new ContentLabelImage("h4", ((AsdnSingleViewModel)DataContext).h4, "330", "380"),
                            new ContentLabelImage("h5", ((AsdnSingleViewModel)DataContext).h5, "315", "453"),
                            new ContentLabelImage("h6", ((AsdnSingleViewModel)DataContext).h6, "330", "497"),
                            new ContentLabelImage("h7", ((AsdnSingleViewModel)DataContext).h7, "315", "507"),
                            new ContentLabelImage("ac", ((AsdnSingleViewModel)DataContext).ac, "165", "516"),
                            new ContentLabelImage("bП", ((AsdnSingleViewModel)DataContext).bП, "140", "5"),
                            new ContentLabelImage("bП1", ((AsdnSingleViewModel)DataContext).bП1, "145", "558")
                        };
                        contentStator = new ContentStatorImageControl(contentLabels) {
                            ImageSource = "pack://application:,,,/Resource/Stator3.jpg"
                        };
                        UserControlStatorImage statorImage3 = new UserControlStatorImage {
                            DataContext = contentStator
                        };
                        #endregion
                        #region fourth window
                        contentLabels = new List<ContentLabelImage> {
                            new ContentLabelImage("h1", ((AsdnSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "330", "215"),
                            new ContentLabelImage("h3", ((AsdnSingleViewModel)DataContext).h3, "315", "270"),
                            new ContentLabelImage("h4", ((AsdnSingleViewModel)DataContext).h4, "330", "380"),
                            new ContentLabelImage("h5", ((AsdnSingleViewModel)DataContext).h5, "315", "455"),
                            new ContentLabelImage("h6", ((AsdnSingleViewModel)DataContext).h6, "330", "497"),
                            new ContentLabelImage("h7", ((AsdnSingleViewModel)DataContext).h7, "315", "507"),
                            new ContentLabelImage("ac", ((AsdnSingleViewModel)DataContext).ac, "170", "513"),
                            new ContentLabelImage("bП", ((AsdnSingleViewModel)DataContext).bП, "140", "10"),
                            new ContentLabelImage("d1",((AsdnSingleViewModel)DataContext).d1, "155", "555")
                        };
                        contentStator = new ContentStatorImageControl(contentLabels) {
                            ImageSource = "pack://application:,,,/Resource/Stator4.jpg"
                        };
                        UserControlStatorImage statorImage4 = new UserControlStatorImage {
                            DataContext = contentStator
                        };
                        #endregion
                        #endregion
                        StatorsDirectory stators = new StatorsDirectory(statorImage1, statorImage2, statorImage3, statorImage4) {
                            Owner = this
                        };
                        stators.Show();
                    } break;
            }
        }
        private void OpenFile(object sender, ExecutedRoutedEventArgs e) {
            bool isFormat = true;
            string namefile = "";
            XElement inputData = LoadFromFile(ref namefile);
            if(inputData != null) {
                if (inputData.Element("tbxP12") != null) ((AsdnSingleViewModel)DataContext).P12 = inputData.Element("tbxP12").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbxU") != null) ((AsdnSingleViewModel)DataContext).U1 = inputData.Element("tbxU").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbx_f1") != null) ((AsdnSingleViewModel)DataContext).f1 = inputData.Element("tbx_f1").Value.Trim();
                else isFormat = false;
                if (inputData.Element("cbx_p") != null) ((AsdnSingleViewModel)DataContext).p = inputData.Element("cbx_p").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxPmex") != null) ((AsdnSingleViewModel)DataContext).Pмех = inputData.Element("tbxPmex").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxDi") != null) ((AsdnSingleViewModel)DataContext).Di = inputData.Element("tbxDi").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxΔГ1") != null) ((AsdnSingleViewModel)DataContext).ΔГ1 = inputData.Element("tbxΔГ1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxZ1") != null) ((AsdnSingleViewModel)DataContext).Z1 = inputData.Element("tbxZ1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxDa") != null) ((AsdnSingleViewModel)DataContext).Da = inputData.Element("tbxDa").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_a1") != null) ((AsdnSingleViewModel)DataContext).a1 = inputData.Element("tbx_a1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_a2") != null) ((AsdnSingleViewModel)DataContext).a2 = inputData.Element("tbx_a2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxΔкр") != null) ((AsdnSingleViewModel)DataContext).Δкр = inputData.Element("tbxΔкр").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dиз") != null) ((AsdnSingleViewModel)DataContext).dиз = inputData.Element("tbx_dиз").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_qГ") != null) ((AsdnSingleViewModel)DataContext).qГ = inputData.Element("tbx_qГ").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bz1") != null) ((AsdnSingleViewModel)DataContext).bz1 = inputData.Element("tbx_bz1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h8") != null) ((AsdnSingleViewModel)DataContext).h8 = inputData.Element("tbx_h8").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h7") != null) ((AsdnSingleViewModel)DataContext).h7 = inputData.Element("tbx_h7").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h6") != null) ((AsdnSingleViewModel)DataContext).h6 = inputData.Element("tbx_h6").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h5") != null) ((AsdnSingleViewModel)DataContext).h5 = inputData.Element("tbx_h5").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h3") != null) ((AsdnSingleViewModel)DataContext).h3 = inputData.Element("tbx_h3").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h4") != null) ((AsdnSingleViewModel)DataContext).h4 = inputData.Element("tbx_h4").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ac") != null) ((AsdnSingleViewModel)DataContext).ac = inputData.Element("tbx_ac").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_li") != null) ((AsdnSingleViewModel)DataContext).li = inputData.Element("tbx_li").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_cз") != null) ((AsdnSingleViewModel)DataContext).cз = inputData.Element("tbx_cз").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxKзап") != null) ((AsdnSingleViewModel)DataContext).Kзап = inputData.Element("tbxKзап").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_y1") != null) ((AsdnSingleViewModel)DataContext).y1 = inputData.Element("tbx_y1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_K2") != null) ((AsdnSingleViewModel)DataContext).K2 = inputData.Element("tbx_K2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_d1") != null) ((AsdnSingleViewModel)DataContext).d1 = inputData.Element("tbx_d1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_Kfe1") != null) ((AsdnSingleViewModel)DataContext).Kfe1 = inputData.Element("tbx_Kfe1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρ1x") != null) ((AsdnSingleViewModel)DataContext).ρ1x = inputData.Element("tbx_ρ1x").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρРУБ") != null) ((AsdnSingleViewModel)DataContext).ρРУБ = inputData.Element("tbx_ρРУБ").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρ1Г") != null) ((AsdnSingleViewModel)DataContext).ρ1Г = inputData.Element("tbx_ρ1Г").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxB") != null) ((AsdnSingleViewModel)DataContext).B = inputData.Element("tbxB").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("cbxPЗ") != null) ((AsdnSingleViewModel)DataContext).PЗ = inputData.Element("cbxPЗ").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_p10_50") != null) ((AsdnSingleViewModel)DataContext).p10_50 = inputData.Element("tbx_p10_50").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxΔГ2") != null) ((AsdnSingleViewModel)DataContext).ΔГ2 = inputData.Element("tbxΔГ2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxDpст") != null) ((AsdnSingleViewModel)DataContext).Dpст = inputData.Element("tbxDpст").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("cbx_bСК") != null) ((AsdnSingleViewModel)DataContext).bСК = inputData.Element("cbx_bСК").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bП2") != null) ((AsdnSingleViewModel)DataContext).bП2 = inputData.Element("tbx_bП2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxZ2") != null) ((AsdnSingleViewModel)DataContext).Z2 = inputData.Element("tbxZ2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_hp") != null) ((AsdnSingleViewModel)DataContext).hp = inputData.Element("tbx_hp").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dв") != null) ((AsdnSingleViewModel)DataContext).dв = inputData.Element("tbx_dв").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bк") != null) ((AsdnSingleViewModel)DataContext).bк = inputData.Element("tbx_bк").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_aк") != null) ((AsdnSingleViewModel)DataContext).aк = inputData.Element("tbx_aк").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_γ") != null) ((AsdnSingleViewModel)DataContext).γ = inputData.Element("tbx_γ").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρ2Г") != null) ((AsdnSingleViewModel)DataContext).ρ2Г = inputData.Element("tbx_ρ2Г").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_Kfe2") != null) ((AsdnSingleViewModel)DataContext).Kfe2 = inputData.Element("tbx_Kfe2").Value.Trim();
                else
                    isFormat = false;
                if (!isFormat) ErrorReport("Некорректный или неполный файл исходных данных.");
            }

            ((AsdnSingleViewModel)DataContext).Diagnostic = $"Открыт файл {namefile}";

            UpdateBinding();
        }
        //save only user input
        private void SaveFile(object sender, ExecutedRoutedEventArgs e) {
            XElement inputData = new XElement("inputData",
                    new XElement("tbxP12", ((AsdnSingleViewModel)DataContext).P12),
                    new XElement("tbxU", ((AsdnSingleViewModel)DataContext).U1),
                    new XElement("tbx_f1", ((AsdnSingleViewModel)DataContext).f1),
                    new XElement("cbx_p", ((AsdnSingleViewModel)DataContext).p),
                    new XElement("tbxPmex", ((AsdnSingleViewModel)DataContext).Pмех),
                    new XElement("tbxDi", ((AsdnSingleViewModel)DataContext).Di),
                    new XElement("tbxΔГ1", ((AsdnSingleViewModel)DataContext).ΔГ1),
                    new XElement("tbxZ1", ((AsdnSingleViewModel)DataContext).Z1),
                    new XElement("tbxDa", ((AsdnSingleViewModel)DataContext).Da),
                    new XElement("tbx_a1", ((AsdnSingleViewModel)DataContext).a1),
                    new XElement("tbx_a2", ((AsdnSingleViewModel)DataContext).a2),
                    new XElement("tbxΔкр", ((AsdnSingleViewModel)DataContext).Δкр),
                    new XElement("tbx_dиз", ((AsdnSingleViewModel)DataContext).dиз),
                    new XElement("tbx_qГ", ((AsdnSingleViewModel)DataContext).qГ),
                    new XElement("tbx_bz1", ((AsdnSingleViewModel)DataContext).bz1),
                    new XElement("tbx_h8", ((AsdnSingleViewModel)DataContext).h8),
                    new XElement("tbx_h7", ((AsdnSingleViewModel)DataContext).h7),
                    new XElement("tbx_h6", ((AsdnSingleViewModel)DataContext).h6),
                    new XElement("tbx_h5", ((AsdnSingleViewModel)DataContext).h5),
                    new XElement("tbx_h3", ((AsdnSingleViewModel)DataContext).h3),
                    new XElement("tbx_h4", ((AsdnSingleViewModel)DataContext).h4),
                    new XElement("tbx_ac", ((AsdnSingleViewModel)DataContext).ac),
                    //new XElement("tbx_bПН", ((AsdnSingleViewModel)DataContext).bПН),
                    new XElement("tbx_li", ((AsdnSingleViewModel)DataContext).li),
                    new XElement("tbx_cз", ((AsdnSingleViewModel)DataContext).cз),
                    new XElement("tbxKзап", ((AsdnSingleViewModel)DataContext).Kзап),
                    new XElement("tbx_y1", ((AsdnSingleViewModel)DataContext).y1),
                    new XElement("tbx_K2", ((AsdnSingleViewModel)DataContext).K2),
                    new XElement("tbx_d1", ((AsdnSingleViewModel)DataContext).d1),
                    new XElement("tbx_Kfe1", ((AsdnSingleViewModel)DataContext).Kfe1),
                    new XElement("tbx_ρ1x", ((AsdnSingleViewModel)DataContext).ρ1x),
                    new XElement("tbx_ρРУБ", ((AsdnSingleViewModel)DataContext).ρРУБ),
                    new XElement("tbx_ρ1Г", ((AsdnSingleViewModel)DataContext).ρ1Г),
                    new XElement("tbxB", ((AsdnSingleViewModel)DataContext).B),
                    new XElement("cbxPЗ", ((AsdnSingleViewModel)DataContext).PЗ),
                    new XElement("tbx_p10_50", ((AsdnSingleViewModel)DataContext).p10_50),
                    new XElement("tbxΔГ2", ((AsdnSingleViewModel)DataContext).ΔГ2),
                    new XElement("tbxDpст", ((AsdnSingleViewModel)DataContext).Dpст),
                    new XElement("cbx_bСК", ((AsdnSingleViewModel)DataContext).bСК),
                    new XElement("tbx_bП2", ((AsdnSingleViewModel)DataContext).bП2),
                    new XElement("tbxZ2", ((AsdnSingleViewModel)DataContext).Z2),
                    new XElement("tbx_hp", ((AsdnSingleViewModel)DataContext).hp),
                    new XElement("tbx_dв", ((AsdnSingleViewModel)DataContext).dв),
                    new XElement("tbx_bк", ((AsdnSingleViewModel)DataContext).bк),
                    new XElement("tbx_aк", ((AsdnSingleViewModel)DataContext).aк),
                    new XElement("tbx_γ", ((AsdnSingleViewModel)DataContext).γ),
                    new XElement("tbx_ρ2Г", ((AsdnSingleViewModel)DataContext).ρ2Г),
                    new XElement("tbx_Kfe2", ((AsdnSingleViewModel)DataContext).Kfe2));
            string namefile = SaveObjectToXMLFile(inputData);
            ((AsdnSingleViewModel)DataContext).Diagnostic = $"Сохранен файл {namefile}";
            UpdateBinding();
        }
    }
}
