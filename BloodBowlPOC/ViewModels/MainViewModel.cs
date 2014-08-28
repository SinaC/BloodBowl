using System;
using BloodBowlPOC.Boards;
using BloodBowlPOC.MVVM;
using BloodBowlPOC.Messages;
using BloodBowlPOC.Utils;

namespace BloodBowlPOC.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public static readonly double CellWidth = 32;
        public static readonly double CellHeight = 24;
        public const int SizeX = 15;
        public const int SizeY = 24;

        public ProbabilitiesViewModel ProbabilitiesViewModel { get; set; }
        public BounceOptionsViewModel BounceOptionsViewModel { get; set; }

        public Board Board = new Board(SizeX, SizeY);

        private string _status;
        public string Status
        {
            get { return _status; }
            set { Set(() => Status, ref _status, value); }
        }
        
        public MainViewModel()
        {
            ProbabilitiesViewModel = new ProbabilitiesViewModel();
            BounceOptionsViewModel = new BounceOptionsViewModel();

            Mediator.Register<CellSelectedMessage>(this, OnCellSelected);

            //Board b1 = new Board(6, 6);
            //FieldCoordinate result = b1.GetLastInboundOnPath_CohenSutherland(new FieldCoordinate(0, 0), new FieldCoordinate(10, -1));
        }

        public void Initialize()
        {
            ProbabilitiesViewModel.Initialize(SizeX, SizeY, CellWidth, CellHeight);
        }


        private void OnCellSelected(CellSelectedMessage cellSelectedMessage)
        {
            int maxBounces = BounceOptionsViewModel.SelectedBounceCount;
            var selectedMode = BounceOptionsViewModel.SelectedMode;

            Board.Reset();
            Board.ComputeBounceProbabilities(new FieldCoordinate(cellSelectedMessage.X, cellSelectedMessage.Y), maxBounces, selectedMode);

            DisplayProbabilities(Board);
        }

        private void DisplayProbabilities(Board board)
        {
            ProbabilitiesViewModel.Display(board);
            Status = String.Format("Sum:{0:0.####} Min:{1:0.####} Max:{2:0.####}", ProbabilitiesViewModel.Sum * 100, ProbabilitiesViewModel.Min * 100, ProbabilitiesViewModel.Max * 100);
        }
    }

    public class MainViewModelDesignData : MainViewModel
    {
        public MainViewModelDesignData()
        {
            ProbabilitiesViewModel = new ProbabilitiesViewModelDesignData();
            BounceOptionsViewModel = new BounceOptionsViewModelDesignData();
        }
    }
}
