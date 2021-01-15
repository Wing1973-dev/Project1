using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IVMElectro.Models.Premag;
using IVMElectro.ViewModel.Premag;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using static IVMElectro.Services.ServiceIO;

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for AxisView.xaml
    /// </summary>
    public partial class AxisView : Window {
        public AxisView() => InitializeComponent();
        private void OpenFile(object sender, ExecutedRoutedEventArgs e) {
            string namefile = string.Empty;
            XElement inputData = LoadFromFile(ref namefile);
            if (inputData != null) {
                if (inputData.Element("cbx_MarkSteel") != null) ((PremagAxisVM)DataContext).MarkSteel = inputData.Element("cbx_MarkSteel").Value.Trim();
                //load main data
                if (inputData.Element("AxisMainData") != null) {
                    if (inputData.Element("AxisMainData").Elements().Count() != 0) {
                        ((PremagAxisVM)DataContext).VariationDataMainData.Clear();
                        foreach (XElement item in inputData.Element("AxisMainData").Elements())
                            ((PremagAxisVM)DataContext).VariationDataMainData.Add(new PremagAxisMainDataModel(item));
                    }
                }
                //load UpMagnets data
                if (inputData.Element("DataUp") != null) {
                    if (inputData.Element("DataUp").Elements().Count() != 0) {
                        ((PremagAxisVM)DataContext).VariationDataUpMagnets.Clear();
                        foreach (XElement item in inputData.Element("DataUp").Elements())
                            ((PremagAxisVM)DataContext).VariationDataUpMagnets.Add(new StringOfVarParametersAxis(item));
                    }
                }
                //save DownMagnets data
                if (inputData.Element("DataDown") != null) {
                    if (inputData.Element("DataDown").Elements().Count() != 0) {
                        ((PremagAxisVM)DataContext).VariationDataDownMagnets.Clear();
                        foreach (XElement item in inputData.Element("DataDown").Elements())
                            ((PremagAxisVM)DataContext).VariationDataDownMagnets.Add(new StringOfVarParametersAxis(item));
                    }
                }
            }
        }
        private void SaveFile(object sender, ExecutedRoutedEventArgs e) {
            XElement inputData = new XElement("inputData",
                    new XElement("cbx_MarkSteel", ((PremagAxisVM)DataContext).MarkSteel));
            //serialise main data
            XElement elementMD = new XElement("AxisMainData");
            if (((PremagAxisVM)DataContext).VariationDataMainData.Count != 0)
                foreach (PremagAxisMainDataModel item in ((PremagAxisVM)DataContext).VariationDataMainData)
                    elementMD.Add(item.Serialise());
            inputData.Add(elementMD);
            //serialise UpMagnets data
            XElement elementUpD = new XElement("DataUp");
            if (((PremagAxisVM)DataContext).VariationDataUpMagnets.Count != 0)
                foreach (StringOfVarParametersAxis item in ((PremagAxisVM)DataContext).VariationDataUpMagnets)
                    elementUpD.Add(item.Serialise());
            inputData.Add(elementUpD);
            //serialise DownMagnets data
            XElement elementDwnD = new XElement("DataDown");
            if (((PremagAxisVM)DataContext).VariationDataDownMagnets.Count != 0)
                foreach (StringOfVarParametersAxis item in ((PremagAxisVM)DataContext).VariationDataDownMagnets)
                    elementDwnD.Add(item.Serialise());
            inputData.Add(elementDwnD);

            string namefile = SaveObjectToXMLFile(inputData);
            ((PremagAxisVM)DataContext).Diagnostic = $"Сохранен файл {namefile}";
        }
        private void btnTableVarParams_Click(object sender, RoutedEventArgs e) {
            StringOfVarParametersAxis stringOfVar = null;
            StringOfVarParamsAxisVM varParamsVM = null;
            StringOfVarParamsAxisView view = null;
            switch (((Button)sender).Name) {
                case "btnAddUp":
                    if (((PremagAxisVM)DataContext).VariationDataMainData.Count == 0) return;

                    int max_idCulc = ((PremagAxisVM)DataContext).VariationDataUpMagnets.Count != 0 ?
                        ((PremagAxisVM)DataContext).VariationDataUpMagnets.Select(i => i.ID_culc).Max() :
                        0;
                    int max_idSlot = ((PremagAxisVM)DataContext).VariationDataUpMagnets.Count != 0 ?
                        ((PremagAxisVM)DataContext).VariationDataUpMagnets.Select(i => i.ID_slot).Max() :
                        ((PremagAxisVM)DataContext).VariationDataMainData.Select(i => i.ID_slot).Max();
                    //for all slots
                    for (int i = 0; i < max_idSlot; i++) {
                        stringOfVar = new StringOfVarParametersAxis { ID_culc = max_idCulc + 1, ID_slot = i + 1 };
                        varParamsVM = new StringOfVarParamsAxisVM(stringOfVar);
                        view = new StringOfVarParamsAxisView { DataContext = varParamsVM, Owner = this };
                        view.ShowDialog();
                        if (!varParamsVM.IsOK) return;
                        ((PremagAxisVM)DataContext).VariationDataUpMagnets.Add(varParamsVM.Model); //add to db
                        ((PremagAxisVM)DataContext).VariationDataDownMagnets.Add(varParamsVM.Model);
                    }
                    break;
                case "btnEditUp":
                    if (dtgrdVarParamsUp.SelectedItem != null) {
                        StringOfVarParametersAxis selectedStringOfVar = ((PremagAxisVM)DataContext).VariationDataUpMagnets.Where(
                            i => i.ID_culc == ((StringOfVarParametersAxis)dtgrdVarParamsUp.SelectedItem).ID_culc &&
                            i.ID_slot == ((StringOfVarParametersAxis)dtgrdVarParamsUp.SelectedItem).ID_slot).FirstOrDefault();
                        stringOfVar = (StringOfVarParametersAxis)selectedStringOfVar.Clone();
                        varParamsVM = new StringOfVarParamsAxisVM(stringOfVar);
                        view = new StringOfVarParamsAxisView {
                            DataContext = varParamsVM,
                            Owner = this
                        };
                        view.ShowDialog();
                        if (!varParamsVM.IsOK) return;
                        //modify the db
                        StringOfVarParametersAxis removeItem =
                            ((PremagAxisVM)DataContext).VariationDataUpMagnets.Where(
                                i => i.ID_culc == varParamsVM.Model.ID_culc && i.ID_slot == varParamsVM.Model.ID_slot).FirstOrDefault();
                        ((PremagAxisVM)DataContext).VariationDataUpMagnets.Remove(removeItem);
                        ((PremagAxisVM)DataContext).VariationDataUpMagnets.Add(varParamsVM.Model);
                        SortOfVariationDataUpMagnets();
                    }
                    break;
                case "btnEditDwn":
                    if (dtgrdVarParamsDwn.SelectedItem != null) {
                        StringOfVarParametersAxis selectedStringOfVar = ((PremagAxisVM)DataContext).VariationDataDownMagnets.Where(
                            i => i.ID_culc == ((StringOfVarParametersAxis)dtgrdVarParamsDwn.SelectedItem).ID_culc &&
                            i.ID_slot == ((StringOfVarParametersAxis)dtgrdVarParamsDwn.SelectedItem).ID_slot).FirstOrDefault();
                        stringOfVar = (StringOfVarParametersAxis)selectedStringOfVar.Clone();
                        varParamsVM = new StringOfVarParamsAxisVM(stringOfVar);
                        view = new StringOfVarParamsAxisView {
                            DataContext = varParamsVM,
                            Owner = this
                        };
                        view.ShowDialog();
                        if (!varParamsVM.IsOK) return;
                        //modify the db
                        StringOfVarParametersAxis removeItem =
                            ((PremagAxisVM)DataContext).VariationDataDownMagnets.Where(
                                i => i.ID_culc == varParamsVM.Model.ID_culc && i.ID_slot == varParamsVM.Model.ID_slot).FirstOrDefault();
                        ((PremagAxisVM)DataContext).VariationDataDownMagnets.Remove(removeItem);
                        ((PremagAxisVM)DataContext).VariationDataDownMagnets.Add(varParamsVM.Model);
                        #region sorting
                        SortOfVariationDataDownMagnets();
                        #endregion
                    }

                    break;
                case "btnDelUp":
                    if (dtgrdVarParamsUp.SelectedItem != null) {
                        int idCulc = ((StringOfVarParametersAxis)dtgrdVarParamsUp.SelectedItem).ID_culc;
                        for (int i = 0; i < ((PremagAxisVM)DataContext).VariationDataUpMagnets.Count; i++) {
                            if (((PremagAxisVM)DataContext).VariationDataUpMagnets[i].ID_culc == idCulc) {
                                ((PremagAxisVM)DataContext).VariationDataUpMagnets.Remove(((PremagAxisVM)DataContext).VariationDataUpMagnets[i]);
                                ((PremagAxisVM)DataContext).VariationDataDownMagnets.Remove(((PremagAxisVM)DataContext).VariationDataDownMagnets[i]);
                                i--;
                            }
                        }
                    }
                    break;
            }
        }
        private void btnTableMD_Click(object sender, RoutedEventArgs e) {
            PremagAxisMainDataModel stringOfVar = null;
            PremagAxisMDVM varParamsVM = null;
            AxisMDView view = null;
            switch (((Button)sender).Name) {
                case "btnAddMD":
                    int max_idSlot = ((PremagAxisVM)DataContext).VariationDataMainData.Count != 0 ? 
                        ((PremagAxisVM)DataContext).VariationDataMainData.Select(i => i.ID_slot).Max() :
                        0;
                    stringOfVar = new PremagAxisMainDataModel { ID_slot = ++max_idSlot }; //new item
                    varParamsVM = new PremagAxisMDVM(stringOfVar);
                    view = new AxisMDView {
                        DataContext = varParamsVM,
                        Owner = this
                    };
                    view.ShowDialog();
                    if (!varParamsVM.IsOK) return;
                    ((PremagAxisVM)DataContext).VariationDataMainData.Add(varParamsVM.Model); //add to db
                    #region modify of variation data
                    if (((PremagAxisVM)DataContext).VariationDataUpMagnets.Count != 0) {
                        List<StringOfVarParametersAxis> appendVarParams = new List<StringOfVarParametersAxis>();
                        foreach (StringOfVarParametersAxis vpUp in ((PremagAxisVM)DataContext).VariationDataUpMagnets) {
                            StringOfVarParametersAxis item = new StringOfVarParametersAxis { ID_culc = vpUp.ID_culc, ID_slot = varParamsVM.Model.ID_slot };
                            if(!appendVarParams.Contains(item)) appendVarParams.Add(item);
                        }
                        foreach (StringOfVarParametersAxis vp in appendVarParams) {
                            ((PremagAxisVM)DataContext).VariationDataUpMagnets.Add(vp);
                            ((PremagAxisVM)DataContext).VariationDataDownMagnets.Add(vp);
                        }
                        SortOfVariationDataUpMagnets(); SortOfVariationDataDownMagnets();
                    }
                    #endregion
                    break;
                case "btnEditMD":
                    if (dtgrdMainParams.SelectedItem != null) {
                        PremagAxisMainDataModel selectedStringOfVar = ((PremagAxisVM)DataContext).VariationDataMainData.Where(
                            i => i.ID_slot == ((PremagAxisMainDataModel)dtgrdMainParams.SelectedItem).ID_slot).FirstOrDefault();
                        stringOfVar = (PremagAxisMainDataModel)selectedStringOfVar.Clone();
                        varParamsVM = new PremagAxisMDVM(stringOfVar);
                        view = new AxisMDView {
                            DataContext = varParamsVM,
                            Owner = this
                        };
                        view.ShowDialog();
                        if (!varParamsVM.IsOK) return;
                        //modify the db
                        PremagAxisMainDataModel removeItem = 
                            ((PremagAxisVM)DataContext).VariationDataMainData.Where(i => i.ID_slot == varParamsVM.Model.ID_slot).FirstOrDefault();
                        ((PremagAxisVM)DataContext).VariationDataMainData.Remove(removeItem);
                        ((PremagAxisVM)DataContext).VariationDataMainData.Add(varParamsVM.Model);
                        #region sorting
                        var sortshot = ((PremagAxisVM)DataContext).VariationDataMainData.OrderBy(i => i.ID_slot);
                        ObservableCollection<PremagAxisMainDataModel> snapshot = new ObservableCollection<PremagAxisMainDataModel>();
                        foreach (PremagAxisMainDataModel item in sortshot)
                            snapshot.Add(item);
                        ((PremagAxisVM)DataContext).VariationDataMainData.Clear();
                        foreach (PremagAxisMainDataModel item in snapshot)
                            ((PremagAxisVM)DataContext).VariationDataMainData.Add(item); 
                        #endregion
                    }
                    break;
                case "btnDelMD":
                    if (dtgrdMainParams.SelectedItem != null) {
                        int idSlot = ((PremagAxisMainDataModel)dtgrdMainParams.SelectedItem).ID_slot;
                        ((PremagAxisVM)DataContext).VariationDataMainData.Remove((PremagAxisMainDataModel)dtgrdMainParams.SelectedItem);
                        for (int i = 0; i < ((PremagAxisVM)DataContext).VariationDataUpMagnets.Count; i++) {
                            if (((PremagAxisVM)DataContext).VariationDataUpMagnets[i].ID_slot == idSlot) {
                                ((PremagAxisVM)DataContext).VariationDataUpMagnets.Remove(((PremagAxisVM)DataContext).VariationDataUpMagnets[i]);
                                ((PremagAxisVM)DataContext).VariationDataDownMagnets.Remove(((PremagAxisVM)DataContext).VariationDataDownMagnets[i]);
                            }
                        }
                    }
                    break;
            }
        }
        void SortOfVariationDataUpMagnets() {
            var sortshot = ((PremagAxisVM)DataContext).VariationDataUpMagnets.OrderBy(i => i.ID_culc).ThenBy(i => i.ID_slot);
            ObservableCollection<StringOfVarParametersAxis> snapshot = new ObservableCollection<StringOfVarParametersAxis>();
            foreach (StringOfVarParametersAxis item in sortshot)
                snapshot.Add(item);
            ((PremagAxisVM)DataContext).VariationDataUpMagnets.Clear();
            foreach (StringOfVarParametersAxis item in snapshot)
                ((PremagAxisVM)DataContext).VariationDataUpMagnets.Add(item);
        }
        void SortOfVariationDataDownMagnets() {
            var sortshot = ((PremagAxisVM)DataContext).VariationDataDownMagnets.OrderBy(i => i.ID_culc).ThenBy(i => i.ID_slot);
            ObservableCollection<StringOfVarParametersAxis> snapshot = new ObservableCollection<StringOfVarParametersAxis>();
            foreach (StringOfVarParametersAxis item in sortshot)
                snapshot.Add(item);
            ((PremagAxisVM)DataContext).VariationDataDownMagnets.Clear();
            foreach (StringOfVarParametersAxis item in snapshot)
                ((PremagAxisVM)DataContext).VariationDataDownMagnets.Add(item);
        }
    }
}
