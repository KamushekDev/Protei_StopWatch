using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Protei_StopWatch.Annotations;

namespace Protei_StopWatch {
    class MyStopwatchViewModel : INotifyPropertyChanged, INotifyCollectionChanged {

        private readonly Stopwatch _stopwatch;

        private CancellationTokenSource cancellationTokenSource;

        public ObservableCollection<Lap> Laps { get; private set; }

        public String EllapsedTime => _stopwatch.Elapsed.ToString(@"hh\:mm\:ss\:fff");

        public MyStopwatchViewModel() {
            _stopwatch = new Stopwatch();
            Laps = new ObservableCollection<Lap>();
            Laps.CollectionChanged+= OnCollectionChanged;
            cancellationTokenSource=new CancellationTokenSource();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
            OnPropertyChanged(nameof(Laps));
        }

        public void Start() {
            _stopwatch.Start();
            Task.Run(() => {
                while (true) {
                    OnPropertyChanged(nameof(EllapsedTime));
                    Thread.Sleep(13);
                }
            }, cancellationTokenSource.Token);

        }

        public void Stop() {
            _stopwatch.Stop();
            cancellationTokenSource.Cancel();
        }

        public void Lap() {
            Laps.Add(new Lap(_stopwatch.Elapsed, Laps.Count+1));
        }

        public void Reset() {
            _stopwatch.Reset();
            Laps.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
