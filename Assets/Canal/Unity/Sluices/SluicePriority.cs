using System;

namespace Canal.Unity.Sluices
{
    public struct SluicePriority<TPriority, TSluice>
        where TPriority : System.IComparable
        where TSluice : Sluice
    {
        public TPriority Priority;
        public TSluice Sluice;
    }
}
