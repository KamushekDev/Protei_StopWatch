using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protei_StopWatch {
    public struct Lap {

        public TimeSpan LapTime { get; }
        public int LapNumber { get; }

        public Lap(TimeSpan timeSpan, int lapNumber) {
            LapTime = timeSpan;
            LapNumber = lapNumber;
        }

        public override string ToString() {
            return $"Lap {LapNumber}: {LapTime:hh\\:mm\\:ss\\:fff}";
        }
    }
}
