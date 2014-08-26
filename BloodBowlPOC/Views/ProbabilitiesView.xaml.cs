using System.Windows.Controls;
using System.Windows.Input;
using BloodBowlPOC.ViewModels;

namespace BloodBowlPOC.Views
{
    /// <summary>
    /// Interaction logic for ProbabilitiesView.xaml
    /// </summary>
    public partial class ProbabilitiesView : UserControl
    {
        public ProbabilitiesView()
        {
            InitializeComponent();
        }

        public delegate void ProbabilityCellClickedEventHandler(int x, int y);
        public event ProbabilityCellClickedEventHandler ProbabilityCellClicked;

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Border border = (sender as Border);
            if (border != null)
            {
                // Get cell
                int x, y;
                ProbabilityItem item = border.DataContext as ProbabilityItem;
                if (item != null)
                {
                    // Raise event
                    if (ProbabilityCellClicked != null)
                        ProbabilityCellClicked(item.X, item.Y);
                }
            }
        }
    }
}
