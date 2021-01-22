using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Linq;
using IVMElectro.Services.Directories;
using IVMElectro.ViewModel;
using static IVMElectro.Services.ServiceIO;

namespace IVMElectro.View {
    /// <summary>
    /// Interaction logic for ASDNREDView.xaml
    /// </summary>
    public partial class ASDNREDView : Window {
        public ASDNREDView() => InitializeComponent();
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
            BindingOperations.GetBindingExpression(tbx_dкп, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_dкпBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_bZH, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_bZHBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_aкн, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_aкнBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_aк, TextBox.TextProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(lb_aкBounds, ContentProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(tbx_hр2, TextBox.TextProperty).UpdateTarget();

            BindingOperations.GetBindingExpression(tblDiagnostic, TextBlock.TextProperty).UpdateTarget();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e) {
            switch (((Button)sender).Name) {
                case "btnWires": {
                        WiresDirectory wires = new WiresDirectory(((AsdnRedSingleViewModel)DataContext).WireDirectory) {
                            Owner = this
                        };
                        wires.Show();
                    }
                    break;
                case "btnK2": {
                        K2Directory k2 = new K2Directory() {
                            DataContext = DataContext,
                            Owner = this
                        };
                        k2.Show();
                    }
                    break;
                case "btn_ρРУБ": {
                        SteelPropertiesPartition propertiesPartition = new SteelPropertiesPartition() {
                            DataContext = DataContext,
                            Owner = this
                        };
                        propertiesPartition.Show();
                    }
                    break;
                case "btn_p1050": {
                        SteelPropertiesStator propertiesStator = new SteelPropertiesStator() {
                            DataContext = DataContext,
                            Owner = this
                        };
                        propertiesStator.Show();
                    }
                    break;
                case "btnZ2": {
                        Z2View z2View = new Z2View() {
                            DataContext = ((AsdnRedSingleViewModel)DataContext).Get_collectionZ2,
                            Owner = this
                        };
                        z2View.CollectionIsZero.Content = ((AsdnSingleViewModel)DataContext).Get_collectionZ2.Z2.Count == 0 ? "нет данных" : string.Empty;
                        z2View.Show();
                    }
                    break;
                case "btn_ImageStator": {
                        string h2 = Math.Round(((AsdnRedSingleViewModel)DataContext).Model.Common.h2, 2).ToString();
                        #region create windows
                        #region first window
                        List<ContentLabelImage> contentLabels = new List<ContentLabelImage> {
                            new ContentLabelImage("h1", ((AsdnRedSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "320", "200"),
                            new ContentLabelImage("h3", ((AsdnRedSingleViewModel)DataContext).h3, "335", "240"),
                            new ContentLabelImage("h4", ((AsdnRedSingleViewModel)DataContext).h4, "320", "350"),
                            new ContentLabelImage("h5", ((AsdnRedSingleViewModel)DataContext).h5, "335", "405"),
                            new ContentLabelImage("h6", ((AsdnRedSingleViewModel)DataContext).h6, "320", "442"),
                            new ContentLabelImage("h7", ((AsdnRedSingleViewModel)DataContext).h7, "335", "462"),
                            new ContentLabelImage("h8", ((AsdnRedSingleViewModel)DataContext).h8, "320", "505"),
                            new ContentLabelImage("ΔГ1", ((AsdnRedSingleViewModel)DataContext).ΔГ1, "17", "530"),
                            new ContentLabelImage("ac", ((AsdnRedSingleViewModel)DataContext).ac, "145", "507"),
                            new ContentLabelImage("bПН", ((AsdnRedSingleViewModel)DataContext).bПН, "145", "543"),
                            new ContentLabelImage("bП", ((AsdnRedSingleViewModel)DataContext).bП, "140", "8"),
                            new ContentLabelImage("bП1", ((AsdnRedSingleViewModel)DataContext).bП1, "145", "581")
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
                            new ContentLabelImage("h1", ((AsdnRedSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "320", "200"),
                            new ContentLabelImage("h3", ((AsdnRedSingleViewModel)DataContext).h3, "335", "240"),
                            new ContentLabelImage("h4", ((AsdnRedSingleViewModel)DataContext).h4, "320", "350"),
                            new ContentLabelImage("h5", ((AsdnRedSingleViewModel)DataContext).h5, "335", "405"),
                            new ContentLabelImage("h6", ((AsdnRedSingleViewModel)DataContext).h6, "320", "445"),
                            new ContentLabelImage("h7", ((AsdnRedSingleViewModel)DataContext).h7, "335", "462"),
                            new ContentLabelImage("h8", ((AsdnRedSingleViewModel)DataContext).h8, "320", "510"),
                            new ContentLabelImage("ΔГ1", ((AsdnRedSingleViewModel)DataContext).ΔГ1, "17", "530"),
                            new ContentLabelImage("ac", ((AsdnRedSingleViewModel)DataContext).ac, "145", "510"),
                            new ContentLabelImage("bПН", ((AsdnRedSingleViewModel)DataContext).bПН, "145", "552"),
                            new ContentLabelImage("bП", ((AsdnRedSingleViewModel)DataContext).bП, "140", "5"),
                            new ContentLabelImage("d1", ((AsdnRedSingleViewModel)DataContext).d1, "145", "587")
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
                            new ContentLabelImage("h1", ((AsdnRedSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "330", "210"),
                            new ContentLabelImage("h3", ((AsdnRedSingleViewModel)DataContext).h3, "315", "270"),
                            new ContentLabelImage("h4", ((AsdnRedSingleViewModel)DataContext).h4, "330", "380"),
                            new ContentLabelImage("h5", ((AsdnRedSingleViewModel)DataContext).h5, "315", "453"),
                            new ContentLabelImage("h6", ((AsdnRedSingleViewModel)DataContext).h6, "330", "497"),
                            new ContentLabelImage("h7", ((AsdnRedSingleViewModel)DataContext).h7, "315", "507"),
                            new ContentLabelImage("ac", ((AsdnRedSingleViewModel)DataContext).ac, "165", "516"),
                            new ContentLabelImage("bП", ((AsdnRedSingleViewModel)DataContext).bП, "140", "5"),
                            new ContentLabelImage("bП1", ((AsdnRedSingleViewModel)DataContext).bП1, "145", "558")
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
                            new ContentLabelImage("h1", ((AsdnRedSingleViewModel)DataContext).h1, "7", "350"),
                            new ContentLabelImage("h2", h2, "330", "215"),
                            new ContentLabelImage("h3", ((AsdnRedSingleViewModel)DataContext).h3, "315", "270"),
                            new ContentLabelImage("h4", ((AsdnRedSingleViewModel)DataContext).h4, "330", "380"),
                            new ContentLabelImage("h5", ((AsdnRedSingleViewModel)DataContext).h5, "315", "455"),
                            new ContentLabelImage("h6", ((AsdnRedSingleViewModel)DataContext).h6, "330", "497"),
                            new ContentLabelImage("h7", ((AsdnRedSingleViewModel)DataContext).h7, "315", "507"),
                            new ContentLabelImage("ac", ((AsdnRedSingleViewModel)DataContext).ac, "170", "513"),
                            new ContentLabelImage("bП", ((AsdnRedSingleViewModel)DataContext).bП, "140", "10"),
                            new ContentLabelImage("d1",((AsdnRedSingleViewModel)DataContext).d1, "155", "555")
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
                    }
                    break;
            }
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e) {
            bool isFormat = true;
            string namefile = string.Empty;
            XElement inputData = LoadFromFile(ref namefile);
            if (inputData != null) {
                if (inputData.Element("tbxP12") != null) ((AsdnRedSingleViewModel)DataContext).P12 = inputData.Element("tbxP12").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbxU") != null) ((AsdnRedSingleViewModel)DataContext).U1 = inputData.Element("tbxU").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbx_f1") != null) ((AsdnRedSingleViewModel)DataContext).f1 = inputData.Element("tbx_f1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("cbx_p") != null) ((AsdnRedSingleViewModel)DataContext).p = inputData.Element("cbx_p").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxPmex") != null) ((AsdnRedSingleViewModel)DataContext).Pмех = inputData.Element("tbxPmex").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxDi") != null) ((AsdnRedSingleViewModel)DataContext).Di = inputData.Element("tbxDi").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxΔГ1") != null) ((AsdnRedSingleViewModel)DataContext).ΔГ1 = inputData.Element("tbxΔГ1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxZ1") != null) ((AsdnRedSingleViewModel)DataContext).Z1 = inputData.Element("tbxZ1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxDa") != null) ((AsdnRedSingleViewModel)DataContext).Da = inputData.Element("tbxDa").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_a1") != null) ((AsdnRedSingleViewModel)DataContext).a1 = inputData.Element("tbx_a1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_a2") != null) ((AsdnRedSingleViewModel)DataContext).a2 = inputData.Element("tbx_a2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxΔкр") != null) ((AsdnRedSingleViewModel)DataContext).Δкр = inputData.Element("tbxΔкр").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dиз") != null) ((AsdnRedSingleViewModel)DataContext).dиз = inputData.Element("tbx_dиз").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_qГ") != null) ((AsdnRedSingleViewModel)DataContext).qГ = inputData.Element("tbx_qГ").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bz1") != null) ((AsdnRedSingleViewModel)DataContext).bz1 = inputData.Element("tbx_bz1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h8") != null) ((AsdnRedSingleViewModel)DataContext).h8 = inputData.Element("tbx_h8").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h7") != null) ((AsdnRedSingleViewModel)DataContext).h7 = inputData.Element("tbx_h7").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h6") != null) ((AsdnRedSingleViewModel)DataContext).h6 = inputData.Element("tbx_h6").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h5") != null) ((AsdnRedSingleViewModel)DataContext).h5 = inputData.Element("tbx_h5").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h3") != null) ((AsdnRedSingleViewModel)DataContext).h3 = inputData.Element("tbx_h3").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_h4") != null) ((AsdnRedSingleViewModel)DataContext).h4 = inputData.Element("tbx_h4").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ac") != null) ((AsdnRedSingleViewModel)DataContext).ac = inputData.Element("tbx_ac").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_li") != null) ((AsdnRedSingleViewModel)DataContext).li = inputData.Element("tbx_li").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_cз") != null) ((AsdnRedSingleViewModel)DataContext).cз = inputData.Element("tbx_cз").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxKзап") != null) ((AsdnRedSingleViewModel)DataContext).Kзап = inputData.Element("tbxKзап").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_y1") != null) ((AsdnRedSingleViewModel)DataContext).y1 = inputData.Element("tbx_y1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_K2") != null) ((AsdnRedSingleViewModel)DataContext).K2 = inputData.Element("tbx_K2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_d1") != null) ((AsdnRedSingleViewModel)DataContext).d1 = inputData.Element("tbx_d1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_Kfe1") != null) ((AsdnRedSingleViewModel)DataContext).Kfe1 = inputData.Element("tbx_Kfe1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρ1x") != null) ((AsdnRedSingleViewModel)DataContext).ρ1x = inputData.Element("tbx_ρ1x").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρРУБ") != null) ((AsdnRedSingleViewModel)DataContext).ρРУБ = inputData.Element("tbx_ρРУБ").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρ1Г") != null) ((AsdnRedSingleViewModel)DataContext).ρ1Г = inputData.Element("tbx_ρ1Г").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxB") != null) ((AsdnRedSingleViewModel)DataContext).B = inputData.Element("tbxB").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_p10_50") != null) ((AsdnRedSingleViewModel)DataContext).p10_50 = inputData.Element("tbx_p10_50").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxΔГ2") != null) ((AsdnRedSingleViewModel)DataContext).ΔГ2 = inputData.Element("tbxΔГ2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxDpст") != null) ((AsdnRedSingleViewModel)DataContext).Dpст = inputData.Element("tbxDpст").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("cbx_bСК") != null) ((AsdnRedSingleViewModel)DataContext).bСК = inputData.Element("cbx_bСК").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bП2") != null) ((AsdnRedSingleViewModel)DataContext).bП2 = inputData.Element("tbx_bП2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxZ2") != null) ((AsdnRedSingleViewModel)DataContext).Z2 = inputData.Element("tbxZ2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_hp") != null) ((AsdnRedSingleViewModel)DataContext).hp = inputData.Element("tbx_hp").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dв") != null) ((AsdnRedSingleViewModel)DataContext).dв = inputData.Element("tbx_dв").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bк") != null) ((AsdnRedSingleViewModel)DataContext).bк = inputData.Element("tbx_bк").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_ρ2Г") != null) ((AsdnRedSingleViewModel)DataContext).ρ2Г = inputData.Element("tbx_ρ2Г").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_Kfe2") != null) ((AsdnRedSingleViewModel)DataContext).Kfe2 = inputData.Element("tbx_Kfe2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("cbxPAS") != null) ((AsdnRedSingleViewModel)DataContext).PAS = inputData.Element("cbxPAS").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_hш") != null) ((AsdnRedSingleViewModel)DataContext).hш = inputData.Element("tbx_hш").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bш") != null) ((AsdnRedSingleViewModel)DataContext).bш = inputData.Element("tbx_bш").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dкп") != null) ((AsdnRedSingleViewModel)DataContext).dкп = inputData.Element("tbx_dкп").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bZH") != null) ((AsdnRedSingleViewModel)DataContext).bZH = inputData.Element("tbx_bZH").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_hр2") != null) ((AsdnRedSingleViewModel)DataContext).hр2 = inputData.Element("tbx_hр2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_bкн") != null) ((AsdnRedSingleViewModel)DataContext).bкн = inputData.Element("tbx_bкн").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_aкн") != null) ((AsdnRedSingleViewModel)DataContext).aкн = inputData.Element("tbx_aкн").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_aк") != null) ((AsdnRedSingleViewModel)DataContext).aк = inputData.Element("tbx_aк").Value.Trim();
                else
                    isFormat = false;
                if (!isFormat) ErrorReport("Некорректный или неполный файл исходных данных.");
            }

            ((AsdnRedSingleViewModel)DataContext).Diagnostic = $"Открыт файл {namefile}";
            UpdateBinding();
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs e) {
            XElement inputData = new XElement("inputData",
                        new XElement("tbxP12", ((AsdnRedSingleViewModel)DataContext).P12),
                        new XElement("tbxU", ((AsdnRedSingleViewModel)DataContext).U1),
                        new XElement("tbx_f1", ((AsdnRedSingleViewModel)DataContext).f1),
                        new XElement("cbx_p", ((AsdnRedSingleViewModel)DataContext).p),
                        new XElement("tbxPmex", ((AsdnRedSingleViewModel)DataContext).Pмех),
                        new XElement("tbxDi", ((AsdnRedSingleViewModel)DataContext).Di),
                        new XElement("tbxΔГ1", ((AsdnRedSingleViewModel)DataContext).ΔГ1),
                        new XElement("tbxZ1", ((AsdnRedSingleViewModel)DataContext).Z1),
                        new XElement("tbxDa", ((AsdnRedSingleViewModel)DataContext).Da),
                        new XElement("tbx_a1", ((AsdnRedSingleViewModel)DataContext).a1),
                        new XElement("tbx_a2", ((AsdnRedSingleViewModel)DataContext).a2),
                        new XElement("tbxΔкр", ((AsdnRedSingleViewModel)DataContext).Δкр),
                        new XElement("tbx_dиз", ((AsdnRedSingleViewModel)DataContext).dиз),
                        new XElement("tbx_qГ", ((AsdnRedSingleViewModel)DataContext).qГ),
                        new XElement("tbx_bz1", ((AsdnRedSingleViewModel)DataContext).bz1),
                        new XElement("tbx_h8", ((AsdnRedSingleViewModel)DataContext).h8),
                        new XElement("tbx_h7", ((AsdnRedSingleViewModel)DataContext).h7),
                        new XElement("tbx_h6", ((AsdnRedSingleViewModel)DataContext).h6),
                        new XElement("tbx_h5", ((AsdnRedSingleViewModel)DataContext).h5),
                        new XElement("tbx_h3", ((AsdnRedSingleViewModel)DataContext).h3),
                        new XElement("tbx_h4", ((AsdnRedSingleViewModel)DataContext).h4),
                        new XElement("tbx_ac", ((AsdnRedSingleViewModel)DataContext).ac),
                        new XElement("tbx_li", ((AsdnRedSingleViewModel)DataContext).li),
                        new XElement("tbx_cз", ((AsdnRedSingleViewModel)DataContext).cз),
                        new XElement("tbxKзап", ((AsdnRedSingleViewModel)DataContext).Kзап),
                        new XElement("tbx_y1", ((AsdnRedSingleViewModel)DataContext).y1),
                        new XElement("tbx_K2", ((AsdnRedSingleViewModel)DataContext).K2),
                        new XElement("tbx_d1", ((AsdnRedSingleViewModel)DataContext).d1),
                        new XElement("tbx_Kfe1", ((AsdnRedSingleViewModel)DataContext).Kfe1),
                        new XElement("tbx_ρ1x", ((AsdnRedSingleViewModel)DataContext).ρ1x),
                        new XElement("tbx_ρРУБ", ((AsdnRedSingleViewModel)DataContext).ρРУБ),
                        new XElement("tbx_ρ1Г", ((AsdnRedSingleViewModel)DataContext).ρ1Г),
                        new XElement("tbxB", ((AsdnRedSingleViewModel)DataContext).B),
                        new XElement("tbx_p10_50", ((AsdnRedSingleViewModel)DataContext).p10_50),
                        new XElement("tbxΔГ2", ((AsdnRedSingleViewModel)DataContext).ΔГ2),
                        new XElement("tbxDpст", ((AsdnRedSingleViewModel)DataContext).Dpст),
                        new XElement("cbx_bСК", ((AsdnRedSingleViewModel)DataContext).bСК),
                        new XElement("tbx_bП2", ((AsdnRedSingleViewModel)DataContext).bП2),
                        new XElement("tbxZ2", ((AsdnRedSingleViewModel)DataContext).Z2),
                        new XElement("tbx_hp", ((AsdnRedSingleViewModel)DataContext).hp),
                        new XElement("tbx_dв", ((AsdnRedSingleViewModel)DataContext).dв),
                        new XElement("tbx_bк", ((AsdnRedSingleViewModel)DataContext).bк),
                        new XElement("tbx_aк", ((AsdnRedSingleViewModel)DataContext).aк),
                        new XElement("tbx_ρ2Г", ((AsdnRedSingleViewModel)DataContext).ρ2Г),
                        new XElement("tbx_Kfe2", ((AsdnRedSingleViewModel)DataContext).Kfe2),
                        new XElement("cbxPAS", ((AsdnRedSingleViewModel)DataContext).PAS),
                        new XElement("tbx_hш", ((AsdnRedSingleViewModel)DataContext).hш),
                        new XElement("tbx_bш", ((AsdnRedSingleViewModel)DataContext).bш),
                        new XElement("tbx_dкп", ((AsdnRedSingleViewModel)DataContext).dкп),
                        new XElement("tbx_bZH", ((AsdnRedSingleViewModel)DataContext).bZH),
                        new XElement("tbx_hр2", ((AsdnRedSingleViewModel)DataContext).hр2),
                        new XElement("tbx_bкн", ((AsdnRedSingleViewModel)DataContext).bкн),
                        new XElement("tbx_aкн", ((AsdnRedSingleViewModel)DataContext).aкн)
                        );
            string namefile = SaveObjectToXMLFile(inputData);
            
            ((AsdnRedSingleViewModel)DataContext).Diagnostic = $"Сохранен файл {namefile}";
            UpdateBinding();
        }

        private void cbxPAS_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EnableStateRotorElements();
            switch (cbxPAS.SelectedItem.ToString()) {
                case "круглый":
                    RoundStateRotorElements();
                    break;
                case "прямоугольный":
                    RectangularStateRotorElements();
                    break;
                case "грушевидный":
                    PearFormStateRotorElements();
                    break;
                case "двойная клетка":
                    EnableStateRotorElements();
                    break;
            }
            UpdateBinding();
        }
        private void EnableStateRotorElements() {
            tbx_dкп.IsEnabled = true; tbx_bП2.IsEnabled = true; tbx_hр2.IsEnabled = true; tbx_bкн.IsEnabled = true; tbx_aкн.IsEnabled = true;
            lb_dкпBounds.Visibility = Visibility.Visible; lb_hр2.Visibility = Visibility.Visible; 
            lb_bкн.Visibility = Visibility.Visible; lb_bкBounds.Visibility = Visibility.Visible;
            lb_aкнBounds.Visibility = Visibility.Visible; lb_aкBounds.Visibility = Visibility.Visible;
        }
        private void RoundStateRotorElements() {
            tbx_dкп.IsEnabled = true; 
            tbx_bП2.IsEnabled = false; tbx_hр2.IsEnabled = false; tbx_bкн.IsEnabled = false; tbx_aкн.IsEnabled = false;
            lb_dкпBounds.Visibility = Visibility.Visible; lb_hр2.Visibility = Visibility.Hidden;
            lb_bкн.Visibility = Visibility.Hidden; lb_bкBounds.Visibility = Visibility.Hidden;
            lb_aкнBounds.Visibility = Visibility.Hidden; lb_aкBounds.Visibility = Visibility.Hidden;

            ((AsdnRedSingleViewModel)DataContext).bП2 = "0"; ((AsdnRedSingleViewModel)DataContext).hр2 = "0";
            ((AsdnRedSingleViewModel)DataContext).bкн = "0"; ((AsdnRedSingleViewModel)DataContext).aкн = "0";
        }
        private void RectangularStateRotorElements() {
            tbx_bП2.IsEnabled = true;
            tbx_dкп.IsEnabled = false;  tbx_hр2.IsEnabled = false; tbx_bкн.IsEnabled = false; tbx_aкн.IsEnabled = false;
            lb_dкпBounds.Visibility = Visibility.Hidden; lb_hр2.Visibility = Visibility.Hidden;
            lb_bкн.Visibility = Visibility.Hidden; lb_bкBounds.Visibility = Visibility.Visible;
            lb_aкнBounds.Visibility = Visibility.Hidden; lb_aкBounds.Visibility = Visibility.Hidden;

            ((AsdnRedSingleViewModel)DataContext).dкп = "0"; ((AsdnRedSingleViewModel)DataContext).hр2 = "0";
            ((AsdnRedSingleViewModel)DataContext).bкн = "0"; ((AsdnRedSingleViewModel)DataContext).aкн = "0";
        }
        private void PearFormStateRotorElements() {
            tbx_dкп.IsEnabled = false; tbx_bП2.IsEnabled = false; tbx_hр2.IsEnabled = false; tbx_bкн.IsEnabled = false; tbx_aкн.IsEnabled = false;
            lb_dкпBounds.Visibility = Visibility.Hidden; lb_hр2.Visibility = Visibility.Hidden;
            lb_bкн.Visibility = Visibility.Hidden; lb_bкBounds.Visibility = Visibility.Hidden;
            lb_aкнBounds.Visibility = Visibility.Hidden; lb_aкBounds.Visibility = Visibility.Hidden;

            ((AsdnRedSingleViewModel)DataContext).dкп = "0"; ((AsdnRedSingleViewModel)DataContext).bП2 = "0";
            ((AsdnRedSingleViewModel)DataContext).hр2 = "0"; ((AsdnRedSingleViewModel)DataContext).bкн = "0"; 
            ((AsdnRedSingleViewModel)DataContext).aкн = "0";
        }
    }
}
