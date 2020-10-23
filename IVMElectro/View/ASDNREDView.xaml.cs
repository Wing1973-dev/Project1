using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IVMElectro.View {
    /// <summary>
    /// Interaction logic for ASDNREDView.xaml
    /// </summary>
    public partial class ASDNREDView : Window {
        public ASDNREDView() {
            InitializeComponent();
            string temp;
        }
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
            //BindingOperations.GetBindingExpression(lb_bПНBounds, ContentProperty).UpdateTarget();
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
            BindingOperations.GetBindingExpression(cbx_p, System.Windows.Controls.Primitives.Selector.SelectedValueProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(cbxPAS, System.Windows.Controls.Primitives.Selector.SelectedValueProperty).UpdateTarget();
            BindingOperations.GetBindingExpression(cbx_bСК, System.Windows.Controls.Primitives.Selector.SelectedValueProperty).UpdateTarget();

            BindingOperations.GetBindingExpression(tblDiagnostic, TextBlock.TextProperty).UpdateTarget();
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e) { 
        
        }

        private void OpenFile(object sender, ExecutedRoutedEventArgs e) { 
        
        }

        private void SaveFile(object sender, ExecutedRoutedEventArgs e) { 
        
        }
    }
}
