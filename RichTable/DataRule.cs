using NStandard;
using System;

namespace Richx
{
    public class DataRule<T> where T : class
    {
        public delegate void SetDelegate(DataTracker<T> selector);
        public int LeftOffset { get; }
        public int RightOffset { get; }
        public SetDelegate Set { get; }

        public DataRule(int leftOffset, int rightOffset, SetDelegate set)
        {
            if (leftOffset < 0) throw new ArgumentException($"The {nameof(leftOffset)} must be non-negative.", nameof(leftOffset));
            if (rightOffset < 0) throw new ArgumentException($"The {nameof(rightOffset)} must be non-negative.", nameof(rightOffset));

            LeftOffset = leftOffset;
            RightOffset = rightOffset;
            Set = set;
        }

        public void Apply(T[] models)
        {
            foreach (var window in SlidingWindow.Slide(models, LeftOffset + 1 + RightOffset))
            {
                Set(new DataTracker<T>(window, window[LeftOffset], LeftOffset));
            }
        }
    }
}
