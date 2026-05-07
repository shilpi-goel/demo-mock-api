using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DfE.NCS.Course.Mock.Function.Utilities
{
    internal static class DeterministicGuidProvider
    {
        private static readonly object _lock = new();
        private static readonly Dictionary<string, long> _counters = new();

        public static void Reset(string stream = "default")
        {
            lock (_lock)
            {
                _counters[stream] = 0;
            }
        }

        public static void ResetAll()
        {
            lock (_lock)
            {
                _counters.Clear();
            }
        }

        public static Guid GetNext(string stream = "default")
        {
            lock (_lock)
            {
                if (!_counters.ContainsKey(stream))
                    _counters[stream] = 0;

                var counter = _counters[stream]++;
                var input = $"{stream}:{counter}";

                using var md5 = MD5.Create(); // 16 bytes -> Guid
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return new Guid(hash);
            }
        }
    }
}
