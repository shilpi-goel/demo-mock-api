using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;

namespace DfE.NCS.Course.Mock.Function.DataGenerators;

using System;
using System.Security.Cryptography;
using System.Threading;

public static class DeterministicGuidProvider
{
    private const int GuidCount = 500;
    private static readonly Guid[] _guids;
    private static int _currentIndex = -1;

    private static readonly byte[] _seed = new byte[]
    {
        10, 20, 30, 40, 50, 60, 70, 80,
        90, 100, 110, 120, 130, 140, 150, 160
    };

    static DeterministicGuidProvider()
    {
        _guids = GenerateGuids();
    }

    /// <summary>
    /// Returns one unique deterministic GUID per call.
    /// </summary>
    public static Guid GetNext()
    {
        int index = Interlocked.Increment(ref _currentIndex);

        if (index >= GuidCount)
        {
            throw new InvalidOperationException("All deterministic GUIDs have been used.");
        }

        return _guids[index];
    }

    /// <summary>
    /// Resets the sequence so the same GUIDs are returned again from the start.
    /// </summary>
    public static void Reset()
    {
        Interlocked.Exchange(ref _currentIndex, -1);
    }

    private static Guid[] GenerateGuids()
    {
        var result = new Guid[GuidCount];
        using var sha256 = SHA256.Create();

        byte[] current = _seed;

        for (int i = 0; i < GuidCount; i++)
        {
            current = sha256.ComputeHash(current);
            byte[] guidBytes = new byte[16];
            Array.Copy(current, guidBytes, 16);
            result[i] = new Guid(guidBytes);
        }

        return result;
    }
}
