// Guids.cs
// MUST match guids.h
using System;

namespace BorislavIvanov.ClearTrackedChanges
{
    static class GuidList
    {
        public const string guidClearTrackedChangesPkgString = "07c4bfc1-60dd-4aea-be7f-5d4be5c664e3";
        public const string guidClearTrackedChangesCmdSetString = "87ccc87c-cd75-4581-b844-f54ce6377a5b";

        public static readonly Guid guidClearTrackedChangesCmdSet = new Guid(guidClearTrackedChangesCmdSetString);
    };
}