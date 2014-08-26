using System.Windows.Controls;
using System.Windows.Input;
using BloodBowlPOC.MVVM;
using BloodBowlPOC.Messages;
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

        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Border border = (sender as Border);
            if (border != null)
            {
                // Get cell
                ProbabilityItem item = border.DataContext as ProbabilityItem;
                if (item != null)
                {
                    Mediator.Send(new CellSelectedMessage
                        {
                            X = item.X,
                            Y = item.Y
                        });
                }
            }
        }
    }
}
