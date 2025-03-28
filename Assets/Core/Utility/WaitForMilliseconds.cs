using System.Diagnostics;
using UnityEngine;

namespace Core.Utility
{
    public class WaitForMilliseconds : CustomYieldInstruction
    {
        private readonly Stopwatch _stopwatch;
        private readonly long _ms;

        public override bool keepWaiting => _stopwatch.ElapsedMilliseconds < _ms;

        public WaitForMilliseconds(int ms)
        {
            _ms = ms;
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
    }
}