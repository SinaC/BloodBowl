using System;
using System.Windows;
using System.Windows.Controls;
using BloodBowlPOC.Boards;
using BloodBowlPOC.Utils;
using BloodBowlPOC.ViewModels;

namespace BloodBowlPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static readonly double CellWidth = 32;
        public static readonly double CellHeight = 24;
        public const int SizeX = 15;
        public const int SizeY = 24;
        public TextBlock[,] Texts;
        public Border[,] Cells;

        public Board Board = new Board(SizeX, SizeY);
        public ProbabilitiesViewModel ProbabilitiesViewModel { get; set; }

        public MainWindow()
        {
            // TODO: add MainViewModel including OptionsViewModel and ProbabilitiesViewModel
            InitializeComponent();

            ProbabilitiesViewModel = new ProbabilitiesViewModel();
            ProbabilitiesViewModel.Initialize(SizeX, SizeY, CellWidth, CellHeight);

            ProbabilitiesView.DataContext = ProbabilitiesViewModel;
            ProbabilitiesView.ProbabilityCellClicked += OnProbabilityCellClicked;

            MaxBouncesComboBox.SelectedIndex = 2; // TODO: move this to an new ViewModel/Views something like Options

            //Test(Board, SizeX/2, SizeY/2);
        }
        
        private void OnProbabilityCellClicked(int x, int y) // TODO: use command instead of event
        {
            int maxBounces = int.Parse(MaxBouncesComboBox.SelectionBoxItem.ToString());
            var radioChecked = (RadioKick.IsChecked.HasValue && RadioKick.IsChecked.Value) ? RadioKick.Content.ToString() : RadioPass.Content.ToString();
            
            Board.Reset();
            Board.ComputeBounceProbabilities(new FieldCoordinate(x, y), maxBounces, radioChecked);

            DisplayProbabilities(Board);
        }

        private void DisplayProbabilities(Board board)
        {
            ProbabilitiesViewModel.Display(board);
            Status.Text = String.Format("Sum:{0:0.####} Min:{1:0.####} Max:{2:0.####}", ProbabilitiesViewModel.Sum * 100, ProbabilitiesViewModel.Min * 100, ProbabilitiesViewModel.Max * 100);
        }
    }
}