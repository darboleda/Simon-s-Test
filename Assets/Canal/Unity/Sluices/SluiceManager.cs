using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Canal.Unity.Sluices
{
    public class SluiceManager : Behavior
    {
        [System.Serializable]
        public struct SluiceInitializer
        {
            public string Id;
            public string Parent;
            public int Priority;
            public float TimeScale;
            public bool Paused;
            public bool IsGroup;
        }

        public delegate void UpdateHandler(float deltaTime);
        public delegate void PausedHandler();
        public delegate void UnpausedHandler();

        [SerializeField]
        private SluiceInitializer[] sluices;

        private Dictionary<string, Sluice> loadedSluices = new Dictionary<string, Sluice>();
        private SluiceGroup<string, int, Sluice> root = new SluiceGroup<string, int, Sluice>(1, false);

        public void Update()
        {
            root.Update(Time.deltaTime);
        }

        public void FixedUpdate()
        {
            root.FixedUpdate(Time.deltaTime);
        }

        public void LateUpdate()
        {
            root.LateUpdate(Time.deltaTime);
        }

        public void Subscribe(string sluiceId,
            UpdateHandler update = null,
            UpdateHandler fixedUpdate = null,
            UpdateHandler lateUpdate = null,
            PausedHandler onPause = null,
            UnpausedHandler onUnpause = null)
        {
            PerformAction<SoloSluice>(sluiceId, sluice =>
                {
                    if (update != null) sluice.OnUpdate += update;
                    if (fixedUpdate != null) sluice.OnFixedUpdate += fixedUpdate;
                    if (lateUpdate != null) sluice.OnLateUpdate += lateUpdate;
                    if (onPause != null) sluice.OnPaused += onPause;
                    if (onUnpause != null) sluice.OnUnpaused += onUnpause;
                });
        }

        public void Unsubscribe(string sluiceId,
            UpdateHandler update = null,
            UpdateHandler fixedUpdate = null,
            UpdateHandler lateUpdate = null,
            PausedHandler onPause = null,
            UnpausedHandler onUnpause = null)
        {
            PerformAction<SoloSluice>(sluiceId, sluice =>
                {
                    if (update != null) sluice.OnUpdate -= update;
                    if (fixedUpdate != null) sluice.OnFixedUpdate -= fixedUpdate;
                    if (lateUpdate != null) sluice.OnLateUpdate -= lateUpdate;
                    if (onPause != null) sluice.OnPaused -= onPause;
                    if (onUnpause != null) sluice.OnUnpaused -= onUnpause;
                });
        }

        public void Pause(string sluiceId)
        {
            PerformAction<Sluice>(sluiceId, sluice => { sluice.Pause(); });
        }

        public void Unpause(string sluiceId)
        {
            PerformAction<Sluice>(sluiceId, sluice => { sluice.Unpause(); });
        }

        public void SetTimeScale(string sluiceId, float timeScale)
        {
            PerformAction<Sluice>(sluiceId, sluice => { sluice.TimeScale = timeScale; });
        }

        private bool PerformAction<T>(string sluiceId, System.Action<T> action) where T : Sluice
        {
            Sluice sluice;
            if (TryGetSluice(sluiceId, out sluice) && sluice is T)
            {
                action((T)sluice);
                return true;
            }
            return false;
        }

        private bool TryGetSluice(string sluiceId, out Sluice sluice)
        {
            sluice = null;
            if (loadedSluices.TryGetValue(sluiceId, out sluice))
            {
                return true;
            }

            SluiceInitializer initializer = sluices.FirstOrDefault(x => x.Id == sluiceId);
            if (initializer.Id != sluiceId)
            {
                return false;
            }

            sluice = (initializer.IsGroup ? (Sluice)(new SluiceGroup<string, int, Sluice>(initializer.TimeScale, initializer.Paused))
                        : (Sluice)(new SoloSluice(initializer.TimeScale, initializer.Paused)));
            loadedSluices[initializer.Id] = sluice;

            if (string.IsNullOrEmpty(initializer.Parent))
            {
                root.AddSluice(initializer.Id, initializer.Priority, sluice);
                return true;
            }
            else
            {
                Sluice parent;
                if (TryGetSluice(initializer.Parent, out parent) && parent is SluiceGroup<string, int, Sluice>)
                {
                    ((SluiceGroup<string, int, Sluice>)parent).AddSluice(initializer.Id, initializer.Priority, sluice);
                    return true;
                }
            }
            return false;
        }
    }
}
