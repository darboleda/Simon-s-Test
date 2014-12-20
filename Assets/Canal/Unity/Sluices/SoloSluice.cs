using System;

namespace Canal.Unity.Sluices
{
    public class SoloSluice : Sluice
    {
        public event SluiceManager.UpdateHandler OnUpdate;
        public event SluiceManager.UpdateHandler OnFixedUpdate;
        public event SluiceManager.UpdateHandler OnLateUpdate;
        public event SluiceManager.PausedHandler OnPaused;
        public event SluiceManager.UnpausedHandler OnUnpaused;

        public SoloSluice(float initialTimeScale, bool initialPause) : base(initialTimeScale, initialPause) { }

        protected override void PerformUpdate(float dt)
        {
            if (OnUpdate != null) OnUpdate(dt);
        }

        protected override void PerformFixedUpdate(float dt)
        {
            if (OnFixedUpdate != null) OnFixedUpdate(dt);
        }

        protected override void PerformLateUpdate(float dt)
        {
            if (OnLateUpdate != null) OnLateUpdate(dt);
        }

        protected override void PauseChanged(bool oldPause, bool newPause)
        {
            if (oldPause && !newPause)
            {
                if (OnUnpaused != null) OnUnpaused();
            }
            else if (!oldPause && newPause)
            {
                if (OnPaused != null) OnPaused();
            }
        }
    }
}
