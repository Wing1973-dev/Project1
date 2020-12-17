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

namespace IVMElectro.View.PREMAG {
    /// <summary>
    /// Interaction logic for AxisView.xaml
    /// </summary>
    public partial class AxisView : Window {
        public AxisView() => InitializeComponent();
        private void OpenFile(object sender, ExecutedRoutedEventArgs e) { }
        private void SaveFile(object sender, ExecutedRoutedEventArgs e) { }
        //TODO write here
        private void btnTableUp_Click(object sender, RoutedEventArgs e) {
            StringOfVarParametersAxis stringOfVar = null;
            StringOfVarParamsAxisVM varParamsVM = null;
            StringOfVarParamsAxisView view = null;
            switch (((Button)sender).Name) {
                case "btnAddUp":
                    if (((PremagAxisVM)DataContext).VariationDataMainData.Count == 0) return;

                    break;
                case "btnEditUp":
                    break;
                case "btnDelUp":
                    break;
            }
        }
        private void btnTableDwn_Click(object sender, RoutedEventArgs e) { }
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
                        }
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
                            if (((PremagAxisVM)DataContext).VariationDataUpMagnets[i].ID_slot == idSlot)
                                ((PremagAxisVM)DataContext).VariationDataUpMagnets.Remove(((PremagAxisVM)DataContext).VariationDataUpMagnets[i]);
                        }
                        
                    }
                    break;
            }
        }

    }
}
