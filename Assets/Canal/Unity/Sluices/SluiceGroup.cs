using System.Collections.Generic;

namespace Canal.Unity.Sluices
{
    public class SluiceGroup<TKey, TPriority, TSluice> : Sluice
        where TPriority : System.IComparable
        where TSluice : Sluice
    {
        private class SluiceComparer : IComparer<TPriority>
        {
            public int Compare(TPriority x, TPriority y)
            {
                return ((x.CompareTo(y) < 0) ? -1 : 1);
            }
        }

        private Dictionary<TKey, TSluice> map = new Dictionary<TKey, TSluice>();
        private SortedList<TPriority, TSluice> children;

        public SluiceGroup(float initialTimeScale, bool initialPause) : base(initialTimeScale, initialPause)
        {
            children = new SortedList<TPriority, TSluice>(new SluiceComparer());
        }

        protected override void PerformUpdate(float deltaTime)
        {
            foreach (KeyValuePair<TPriority, TSluice> pair in children) pair.Value.Update(deltaTime);
        }

        protected override void PerformFixedUpdate(float deltaTime)
        {
            foreach (KeyValuePair<TPriority, TSluice> pair in children) pair.Value.FixedUpdate(deltaTime);
        }

        protected override void PerformLateUpdate(float deltaTime)
        {
            foreach (KeyValuePair<TPriority, TSluice> pair in children) pair.Value.LateUpdate(deltaTime);
        }

        protected override void PauseChanged(bool wasPaused, bool nowPaused)
        {
            foreach (KeyValuePair<TPriority, TSluice> pair in children)
            {
                Sluice.SetPause(pair.Value, wasPaused && pair.Value.Paused, nowPaused && pair.Value.Paused);
            }
        }

        public TSluice this[TKey key] { get { return map[key]; } }

        public bool ContainsKey(TKey key)
        {
            return map.ContainsKey(key);
        }

        public void AddSluice(TKey key, TPriority priority, TSluice sluice)
        {
            if (map.ContainsKey(key))
            {
                throw new System.InvalidOperationException(string.Format("Attempted to add sluice with key \"{0}\", but key already exists.", key));
            }
            map[key] = sluice;
            children.Add(priority, sluice);
        }
    }
}
