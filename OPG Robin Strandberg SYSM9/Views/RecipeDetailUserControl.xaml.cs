using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OPG_Robin_Strandberg_SYSM9.Views
{
    /// <summary>
    /// Interaction logic for RecipeDetailWindow.xaml
    /// </summary>
    public partial class RecipeDetailUserControl : UserControl
    {
        public RecipeDetailUserControl()
        {
            InitializeComponent();
            DataContext = new RecipeDetailViewModel();
        }
    }
}
