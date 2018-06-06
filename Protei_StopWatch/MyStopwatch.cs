using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Protei_StopWatch.Annotations;

namespace Protei_StopWatch {
    class MyStopwatch : INotifyPropertyChanged, INotifyCollectionChanged {

        private readonly Stopwatch _stopwatch;

        private CancellationTokenSource _cancellationTokenSource;

        public ObservableCollection<Lap> Laps { get; }

        public String EllapsedTime => _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff");

        public MyStopwatch() {
            _stopwatch = new Stopwatch();
            Laps = new ObservableCollection<Lap>();
            Laps.CollectionChanged+= OnCollectionChanged;
            _cancellationTokenSource=new CancellationTokenSource();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged(nameof(Laps));
        }

        public void Start() {
            _stopwatch.Start();
            _cancellationTokenSource=new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            Task.Run(() => {
                while (true) {
                    OnPropertyChanged(nameof(EllapsedTime));
                    Thread.Sleep(13);
                    if (token.IsCancellationRequested)
                        break;
                }
            }, token);

        }

        public void Stop() {
            _stopwatch.Stop();
            _cancellationTokenSource.Cancel();
        }

        public void Lap() {
            Laps.Add(new Lap(_stopwatch.Elapsed, Laps.Count+1));
        }

        public void Reset() {
            _stopwatch.Reset();
            Laps.Clear();
            OnPropertyChanged(nameof(EllapsedTime));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
