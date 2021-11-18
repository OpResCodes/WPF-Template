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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatWpfControls
{
    /// <summary>
    /// Interaction logic for ColorVectorImage.xaml
    /// </summary>
    public partial class ColorVectorImage : UserControl
    {
        public ColorVectorImage()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        static ColorVectorImage()
        {
            var meta = new FrameworkPropertyMetadata();
            meta.AffectsArrange = true;
            meta.AffectsMeasure = true;
            meta.AffectsRender = true;
            meta.DefaultValue = null;
            meta.AffectsParentArrange = true;
            meta.AffectsParentMeasure = true;
            PathDataProperty = DependencyProperty.Register("PathData", typeof(Geometry), typeof(ColorVectorImage), meta);
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill",
                typeof(Brush),
                typeof(ColorVectorImage),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));


        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public static readonly DependencyProperty StrokeProperty =
            DependencyProperty.Register("Stroke",
                typeof(Brush),
                typeof(ColorVectorImage),
                new PropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Geometry PathData
        {
            get { return (Geometry)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public static readonly DependencyProperty PathDataProperty;
    }
}
