using NStandard;

namespace Richx
{
    public class DataTracker<T> where T : class
    {
        public SlidingWindow<T> Window { get; }
        public T Current { get; }
        public int Index { get; }
        public T this[int offset] => Window[Index + offset];

        public DataTracker(SlidingWindow<T> window, T current, int index)
        {
            Window = window;
            Current = current;
            Index = index;
        }
    }

}
