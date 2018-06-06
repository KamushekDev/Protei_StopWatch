using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Protei_StopWatch.Annotations;

namespace Protei_StopWatch {

    [Flags]
    enum TimerState : byte {
        Idle,
        Working,
        WaitingForContinuing
    }

    class MainViewModel : INotifyPropertyChanged {

        private MyStopwatch _stopwatchViewModel;

        private TimerState _timerState;

        private string _startStopBtnContent;
        private string _lapResetBtnContent;

        private RelayCommand _startStopCommand;
        private RelayCommand _lapResetCommand;

        public MyStopwatch StopwatchViewModel {
            get => _stopwatchViewModel;
            private set {
                _stopwatchViewModel = value;
                OnPropertyChanged(nameof(StopwatchViewModel));
            }
        }

        public RelayCommand StartStopCommand =>
            _startStopCommand ??
            (_startStopCommand = new RelayCommand(StartStopButton));

        public RelayCommand LapResetCommand =>
            _lapResetCommand ??
            (_lapResetCommand = new RelayCommand(LapResetButton, (obj) => _timerState != TimerState.Idle));


        public string StartStopBtnContent {
            get => _startStopBtnContent;
            set {
                _startStopBtnContent = value;
                OnPropertyChanged(nameof(StartStopBtnContent));
            }
        }

        public string LapResetBtnContent {
            get => _lapResetBtnContent;
            set {
                _lapResetBtnContent = value;
                OnPropertyChanged(nameof(LapResetBtnContent));
            }
        }

        public MainViewModel() {
            _stopwatchViewModel = new MyStopwatch();
            _startStopBtnContent = "Start";
            _lapResetBtnContent = "Lap";
            _timerState = TimerState.Idle;
            
        }

        private void StartStopButton(object obj) {
            switch (_timerState) {
                case TimerState.Idle:
                    ChangeState(TimerState.Working, "Stop");
                    _stopwatchViewModel.Start();
                    break;
                case TimerState.Working:
                    ChangeState(TimerState.WaitingForContinuing, "Continue", "Reset");
                    _stopwatchViewModel.Stop();
                    break;
                case TimerState.WaitingForContinuing:
                    ChangeState(TimerState.Working, "Stop", "Lap");
                    _stopwatchViewModel.Start();
                    break;
            }
        }

        private void LapResetButton(object obj) {
            switch (_timerState) {
                case TimerState.Working:
                    ChangeState(_timerState);
                    _stopwatchViewModel.Lap();
                    break;
                case TimerState.WaitingForContinuing:
                    ChangeState(TimerState.Idle, "Start", "Lap");
                    _stopwatchViewModel.Reset();
                    break;
            }
        }

        private void ChangeState(TimerState state, string startStopBtnContent = null, string lapResetBtnContent = null) {
            if (startStopBtnContent != null)
                StartStopBtnContent = startStopBtnContent;
            if (lapResetBtnContent != null)
                LapResetBtnContent = lapResetBtnContent;
            _timerState = state;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
