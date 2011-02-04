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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WickedNite.Flux;
using ImageView.PropertyBags;

namespace ImageView.Views
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl, IView<TestViewModel>
    {
        public TestViewModel ViewModel { get; set; }

        public TestView()
        {
            InitializeComponent();

            Loaded += (s, e) => DataContext = ViewModel;
        }
    }
}
