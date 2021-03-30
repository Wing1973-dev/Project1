using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using IVMElectro.Models.Premag;
using IVMElectro.ViewModel.Premag;
using static IVMElectro.Services.ServiceIO;
using static LibraryAlgorithms.Services.ServiceDT;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for PlungerView.xaml
    /// </summary>
    public partial class PlungerView : Window {
        public PlungerView() => InitializeComponent();
        private void OpenFile(object sender, ExecutedRoutedEventArgs e) {
            bool isFormat = true;
            string namefile = string.Empty;
            XElement inputData = LoadFromFile(ref namefile);
            if (inputData != null) {
                if (inputData.Element("tbxBδ") != null) ((PremagPlungerVM)DataContext).Bδ = inputData.Element("tbxBδ").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbx_ρx") != null) ((PremagPlungerVM)DataContext).ρx = inputData.Element("tbx_ρx").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbx_ρГ") != null) ((PremagPlungerVM)DataContext).ρГ = inputData.Element("tbx_ρГ").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbx_Δk1") != null) ((PremagPlungerVM)DataContext).Δk1 = inputData.Element("tbx_Δk1").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbxR0") != null) ((PremagPlungerVM)DataContext).R0 = inputData.Element("tbxR0").Value.Trim();
                else isFormat = false;
                if (inputData.Element("tbxR10") != null) ((PremagPlungerVM)DataContext).R10 = inputData.Element("tbxR10").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxR110") != null) ((PremagPlungerVM)DataContext).R110 = inputData.Element("tbxR110").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbxR1110") != null) ((PremagPlungerVM)DataContext).R1110 = inputData.Element("tbxR1110").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dпз1") != null) ((PremagPlungerVM)DataContext).dпз1 = inputData.Element("tbx_dпз1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dпз2") != null) ((PremagPlungerVM)DataContext).dпз2 = inputData.Element("tbx_dпз2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_hфл") != null) ((PremagPlungerVM)DataContext).hфл = inputData.Element("tbx_hфл").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_dвст") != null) ((PremagPlungerVM)DataContext).dвст = inputData.Element("tbx_dвст").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_l1") != null) ((PremagPlungerVM)DataContext).l1 = inputData.Element("tbx_l1").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("tbx_l2") != null) ((PremagPlungerVM)DataContext).l2 = inputData.Element("tbx_l2").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("cbx_MarkSteel") != null) ((PremagPlungerVM)DataContext).MarkSteel = inputData.Element("cbx_MarkSteel").Value.Trim();
                else
                    isFormat = false;
                if (inputData.Element("VarParameters") != null && inputData.Element("VarParameters").Elements().Count() != 0) {
                    ((PremagPlungerVM)DataContext).VariationData.Clear();
                    foreach (XElement item in inputData.Element("VarParameters").Elements())
                        ((PremagPlungerVM)DataContext).VariationData.Add(new StringOfVarParametersPlunger(item));
                }
                else
                    isFormat = false;
                if (!isFormat) ErrorReport("Некорректный или неполный файл исходных данных.");
            }
            
            ((PremagPlungerVM)DataContext).Diagnostic = string.IsNullOrEmpty(namefile) ? string.Empty : $"Открыт файл {namefile}";
        }
        private void SaveFile(object sender, ExecutedRoutedEventArgs e) {
            XElement inputData = new XElement("inputData",
                new XElement("tbxBδ", ((PremagPlungerVM)DataContext).Bδ),
                new XElement("tbx_ρx", ((PremagPlungerVM)DataContext).ρx),
                new XElement("tbx_ρГ", ((PremagPlungerVM)DataContext).ρГ),
                new XElement("tbx_Δk1", ((PremagPlungerVM)DataContext).Δk1),
                new XElement("tbxR0", ((PremagPlungerVM)DataContext).R0),
                new XElement("tbxR10", ((PremagPlungerVM)DataContext).R10),
                new XElement("tbxR110", ((PremagPlungerVM)DataContext).R110),
                new XElement("tbxR1110", ((PremagPlungerVM)DataContext).R1110),
                new XElement("tbx_dпз1", ((PremagPlungerVM)DataContext).dпз1),
                new XElement("tbx_dпз2", ((PremagPlungerVM)DataContext).dпз2),
                new XElement("tbx_hфл", ((PremagPlungerVM)DataContext).hфл),
                new XElement("tbx_dвст", ((PremagPlungerVM)DataContext).dвст),
                new XElement("tbx_l1", ((PremagPlungerVM)DataContext).l1),
                new XElement("tbx_l2", ((PremagPlungerVM)DataContext).l2),
                new XElement("cbx_MarkSteel", ((PremagPlungerVM)DataContext).MarkSteel));
            XElement elementVP = new XElement("VarParameters");
            if (((PremagPlungerVM)DataContext).VariationData.Count != 0)
                foreach (StringOfVarParametersPlunger item in ((PremagPlungerVM)DataContext).VariationData)
                    elementVP.Add(item.Serialise());
            inputData.Add(elementVP);
            string namefile = SaveObjectToXMLFile(inputData);
            
            ((PremagPlungerVM)DataContext).Diagnostic = string.IsNullOrEmpty(namefile) ? string.Empty : $"Сохранен файл {namefile}";
            //BindingOperations.GetBindingExpression(tblDiagnostic, TextBlock.TextProperty).UpdateTarget();
        }

        private void btnTable_Click(object sender, RoutedEventArgs e) {
            StringOfVarParametersPlunger stringOfVar = null;
            StringOfVarParamsPlngrVM varParamsVM = null;
            StringOfVarParamsPlngrView view = null;
            switch (((Button)sender).Name) {
                case "btnAdd":
                    int maxid = 0;
                    if (((PremagPlungerVM)DataContext).VariationData.Count != 0) {
                        maxid = ((PremagPlungerVM)DataContext).VariationData.Select(i => i.ID_culc).Max();
                        StringOfVarParametersPlunger varParametersMaxid = ((PremagPlungerVM)DataContext).VariationData.FirstOrDefault(i => i.ID_culc == maxid);
                        stringOfVar = (StringOfVarParametersPlunger)varParametersMaxid.Clone();
                        stringOfVar.ID_culc = ++maxid;
                    }
                    else
                        stringOfVar = new StringOfVarParametersPlunger { ID_culc = ++maxid }; //new item
                    //for validation
                    stringOfVar.SetParametersForModelValidation(StringToDouble(((PremagPlungerVM)DataContext).R0), StringToDouble(((PremagPlungerVM)DataContext).R10),
                        StringToDouble(((PremagPlungerVM)DataContext).R110), StringToDouble(((PremagPlungerVM)DataContext).R1110), 
                        StringToDouble(((PremagPlungerVM)DataContext).hфл), StringToDouble(((PremagPlungerVM)DataContext).l1), 
                        StringToDouble(((PremagPlungerVM)DataContext).l2));
                    varParamsVM = new StringOfVarParamsPlngrVM(stringOfVar);
                    view = new StringOfVarParamsPlngrView {
                        DataContext = varParamsVM,
                        Owner = this
                    };
                    view.ShowDialog();
                    if (!varParamsVM.IsOK) return;
                    ((PremagPlungerVM)DataContext).VariationData.Add(varParamsVM.Model); //add to db
                    break;
                case "btnEdit":
                    if (dtgrdVarParams.SelectedItem != null) {
                        StringOfVarParametersPlunger selectedStringOfVar = ((PremagPlungerVM)DataContext).VariationData.Where(
                            i => i.ID_culc == ((StringOfVarParametersPlunger)dtgrdVarParams.SelectedItem).ID_culc).FirstOrDefault();
                        stringOfVar = (StringOfVarParametersPlunger)selectedStringOfVar.Clone();
                        //for validation
                        stringOfVar.SetParametersForModelValidation(StringToDouble(((PremagPlungerVM)DataContext).R0), StringToDouble(((PremagPlungerVM)DataContext).R10),
                        StringToDouble(((PremagPlungerVM)DataContext).R110), StringToDouble(((PremagPlungerVM)DataContext).R1110),
                        StringToDouble(((PremagPlungerVM)DataContext).hфл), StringToDouble(((PremagPlungerVM)DataContext).l1),
                        StringToDouble(((PremagPlungerVM)DataContext).l2));
                        varParamsVM = new StringOfVarParamsPlngrVM(stringOfVar);
                        view = new StringOfVarParamsPlngrView {
                            DataContext = varParamsVM,
                            Owner = this
                        };
                        view.ShowDialog();
                        if (!varParamsVM.IsOK) return;
                        //modify the db
                        StringOfVarParametersPlunger removeItem = ((PremagPlungerVM)DataContext).VariationData.Where(i => i.ID_culc == varParamsVM.Model.ID_culc).FirstOrDefault();
                        ((PremagPlungerVM)DataContext).VariationData.Remove(removeItem);
                        ((PremagPlungerVM)DataContext).VariationData.Add(varParamsVM.Model);

                        #region sorting
                        var sortshot = ((PremagPlungerVM)DataContext).VariationData.OrderBy(i => i.ID_culc);
                        ObservableCollection<StringOfVarParametersPlunger> snapshot = new ObservableCollection<StringOfVarParametersPlunger>();
                        foreach (StringOfVarParametersPlunger item in sortshot)
                            snapshot.Add(item);
                        ((PremagPlungerVM)DataContext).VariationData.Clear();
                        foreach (StringOfVarParametersPlunger item in snapshot)
                            ((PremagPlungerVM)DataContext).VariationData.Add(item);
                        #endregion
                    }
                    break;
                case "btnDel":
                    if (dtgrdVarParams.SelectedItem != null)
                        ((PremagPlungerVM)DataContext).VariationData.Remove((StringOfVarParametersPlunger)dtgrdVarParams.SelectedItem);
                    break;
            }
        }
    }
}
