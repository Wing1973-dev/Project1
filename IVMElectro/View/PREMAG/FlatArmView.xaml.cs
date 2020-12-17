using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using IVMElectro.Models.Premag;
using IVMElectro.ViewModel.Premag;
using static IVMElectro.Services.ServiceIO;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for FlatArmView.xaml
    /// </summary>
    public partial class FlatArmView : Window {
        public FlatArmView() => InitializeComponent();

        private void OpenFile(object sender, ExecutedRoutedEventArgs e) {
            string namefile = string.Empty;
            XElement inputData = LoadFromFile(ref namefile);
            if (inputData != null) {
                if (inputData.Element("tbxBδ") != null) ((PremagFlatArmVM)DataContext).Bδ = inputData.Element("tbxBδ").Value.Trim();
                if (inputData.Element("tbx_ρx") != null) ((PremagFlatArmVM)DataContext).ρx = inputData.Element("tbx_ρx").Value.Trim();
                if (inputData.Element("tbx_ρГ") != null) ((PremagFlatArmVM)DataContext).ρГ = inputData.Element("tbx_ρГ").Value.Trim();
                if (inputData.Element("tbx_Δk1") != null) ((PremagFlatArmVM)DataContext).Δk1 = inputData.Element("tbx_Δk1").Value.Trim();
                if (inputData.Element("tbxR0") != null) ((PremagFlatArmVM)DataContext).R0 = inputData.Element("tbxR0").Value.Trim();
                if (inputData.Element("tbxR10") != null) ((PremagFlatArmVM)DataContext).R10 = inputData.Element("tbxR10").Value.Trim();
                if (inputData.Element("tbx_dпз1") != null) ((PremagFlatArmVM)DataContext).dпз1 = inputData.Element("tbx_dпз1").Value.Trim();
                if (inputData.Element("tbx_dвст") != null) ((PremagFlatArmVM)DataContext).dвст = inputData.Element("tbx_dвст").Value.Trim();
                if (inputData.Element("tbx_hяр") != null) ((PremagFlatArmVM)DataContext).hяр = inputData.Element("tbx_hяр").Value.Trim();
                if (inputData.Element("tbx_hяк") != null) ((PremagFlatArmVM)DataContext).hяк = inputData.Element("tbx_hяк").Value.Trim();
                if (inputData.Element("cbx_MarkSteel") != null) ((PremagFlatArmVM)DataContext).MarkSteel = inputData.Element("cbx_MarkSteel").Value.Trim();
                if (inputData.Element("VarParameters") != null) {
                    if (inputData.Element("VarParameters").Elements().Count() != 0) {
                        ((PremagFlatArmVM)DataContext).VariationData.Clear();
                        foreach (XElement item in inputData.Element("VarParameters").Elements()) 
                            ((PremagFlatArmVM)DataContext).VariationData.Add(new StringOfVarParameters(item));
                    }
                }
            }
            ((PremagFlatArmVM)DataContext).Diagnostic = $"Открыт файл {namefile}";
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs e) {
            XElement inputData = new XElement("inputData",
                new XElement("tbxBδ", ((PremagFlatArmVM)DataContext).Bδ),
                new XElement("tbx_ρx", ((PremagFlatArmVM)DataContext).ρx),
                new XElement("tbx_ρГ", ((PremagFlatArmVM)DataContext).ρГ),
                new XElement("tbx_Δk1", ((PremagFlatArmVM)DataContext).Δk1),
                new XElement("tbxR0", ((PremagFlatArmVM)DataContext).R0),
                new XElement("tbxR10", ((PremagFlatArmVM)DataContext).R10),
                new XElement("tbx_dпз1", ((PremagFlatArmVM)DataContext).dпз1),
                new XElement("tbx_dвст", ((PremagFlatArmVM)DataContext).dвст),
                new XElement("tbx_hяр", ((PremagFlatArmVM)DataContext).hяр),
                new XElement("tbx_hяк", ((PremagFlatArmVM)DataContext).hяк),
                new XElement("cbx_MarkSteel", ((PremagFlatArmVM)DataContext).MarkSteel));
            XElement elementVP = new XElement("VarParameters");
            if(((PremagFlatArmVM)DataContext).VariationData.Count!=0)
                foreach (StringOfVarParameters item in ((PremagFlatArmVM)DataContext).VariationData)
                    elementVP.Add(item.Serialise());
            inputData.Add(elementVP);
            string namefile = SaveObjectToXMLFile(inputData);
            ((PremagFlatArmVM)DataContext).Diagnostic = $"Сохранен файл {namefile}";
        }

        private void btnTable_Click(object sender, RoutedEventArgs e) {
            StringOfVarParameters stringOfVar = null;
            StringOfVarParamsVM varParamsVM = null;
            StringOfVarParametersView view = null;
            switch (((Button)sender).Name) {
                case "btnAdd":
                    int maxid = ((PremagFlatArmVM)DataContext).VariationData.Count != 0 ? ((PremagFlatArmVM)DataContext).VariationData.Select(i => i.ID_culc).Max() :
                        0;
                    stringOfVar = new StringOfVarParameters { ID_culc = ++maxid }; //new item
                    //for validation
                    stringOfVar.SetParametersForModelValidation(Convert.ToDouble(((PremagFlatArmVM)DataContext).R0), Convert.ToDouble(((PremagFlatArmVM)DataContext).R10),
                        Convert.ToDouble(((PremagFlatArmVM)DataContext).dпз1), Convert.ToDouble(((PremagFlatArmVM)DataContext).dвст));
                    varParamsVM = new StringOfVarParamsVM(stringOfVar);
                    view = new StringOfVarParametersView {
                        DataContext = varParamsVM,
                        Owner = this
                    };
                    view.ShowDialog();
                    if (!varParamsVM.IsOK) return;
                    ((PremagFlatArmVM)DataContext).VariationData.Add(varParamsVM.Model); //add to db
                    break;
                case "btnEdit":
                    if (dtgrdVarParams.SelectedItem != null) {
                        StringOfVarParameters selectedStringOfVar = ((PremagFlatArmVM)DataContext).VariationData.Where(
                            i => i.ID_culc == ((StringOfVarParameters)dtgrdVarParams.SelectedItem).ID_culc).FirstOrDefault();
                        stringOfVar = (StringOfVarParameters)selectedStringOfVar.Clone();
                        //for validation
                        stringOfVar.SetParametersForModelValidation(Convert.ToDouble(((PremagFlatArmVM)DataContext).R0), Convert.ToDouble(((PremagFlatArmVM)DataContext).R10),
                        Convert.ToDouble(((PremagFlatArmVM)DataContext).dпз1), Convert.ToDouble(((PremagFlatArmVM)DataContext).dвст));
                        varParamsVM = new StringOfVarParamsVM(stringOfVar);
                        view = new StringOfVarParametersView {
                            DataContext = varParamsVM,
                            Owner = this
                        };
                        view.ShowDialog();
                        if (!varParamsVM.IsOK) return;
                        //modify the db
                        StringOfVarParameters removeItem = ((PremagFlatArmVM)DataContext).VariationData.Where(i => i.ID_culc == varParamsVM.Model.ID_culc).FirstOrDefault();
                        ((PremagFlatArmVM)DataContext).VariationData.Remove(removeItem);
                        ((PremagFlatArmVM)DataContext).VariationData.Add(varParamsVM.Model);
                    }
                    break;
                case "btnDel":
                    if (dtgrdVarParams.SelectedItem != null)
                        ((PremagFlatArmVM)DataContext).VariationData.Remove((StringOfVarParameters)dtgrdVarParams.SelectedItem);
                    break;
            }
        }
    }
}
