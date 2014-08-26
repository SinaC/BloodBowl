using System.Collections.Generic;

namespace BloodBowlPOC.ViewModels
{
    public enum Modes
    {
        Pass,
        KickOff,
    }

    public class BounceOptionsViewModel : ViewModelBase
    {
        private int _selectedBounceCount;
        public int SelectedBounceCount
        {
            get { return _selectedBounceCount; }
            set { Set(() => SelectedBounceCount, ref _selectedBounceCount, value); }
        }

        private List<int> _bounceCountList;
        public List<int> BounceCountList
        {
            get
            {
                _bounceCountList = _bounceCountList ?? new List<int>
                    {
                        1,
                        2,
                        3,
                        4,
                        5
                    };
                return _bounceCountList;
            }
        }

        private Modes _selectedMode;
        public Modes SelectedMode
        {
            get { return _selectedMode; }
            set { Set(() => SelectedMode, ref _selectedMode, value); }
        }

        public BounceOptionsViewModel()
        {
            SelectedBounceCount = BounceCountList[2];
            SelectedMode = Modes.Pass;
        }
    }

    public class BounceOptionsViewModelDesignData : BounceOptionsViewModel
    {
        public BounceOptionsViewModelDesignData()
        {
            SelectedBounceCount = BounceCountList[3];
            SelectedMode = Modes.KickOff;
        }
    }
}
