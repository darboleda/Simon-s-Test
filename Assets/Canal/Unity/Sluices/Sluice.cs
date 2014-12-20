using System;

namespace Canal.Unity.Sluices
{
    public abstract class Sluice
    {
        protected Sluice(float initialTimeScale, bool initialPaused)
        {
            timeScale = initialTimeScale;
            Paused = initialPaused;
        }

        protected static void SetPause(Sluice target, bool oldPause, bool newPause)
        {
            target.PauseChanged(oldPause, newPause);
        }

        private float timeScale;

        public bool Paused { get; private set; }
        public float TimeScale
        {
            get { return (Paused ? 0 : timeScale); }
            set { timeScale = value; }
        }

        public void Update(float deltaTime)
        {
            if (this.Paused) return;
            PerformUpdate(TimeScale * deltaTime);
        }

        public void FixedUpdate(float deltaTime)
        {
            if (this.Paused) return;
            if (TimeScale == 0) return;
            PerformFixedUpdate(TimeScale * deltaTime);
        }

        public void LateUpdate(float deltaTime)
        {
            if (this.Paused) return;
            PerformLateUpdate(TimeScale * deltaTime);
        }

        public void Pause()
        {
            bool oldPaused = Paused;
            Paused = true;
            PauseChanged(oldPaused, true);
        }

        public void Unpause()
        {
            bool oldPaused = Paused;
            Paused = false;
            PauseChanged(oldPaused, false);
        }

        protected abstract void PerformUpdate(float dt);
        protected abstract void PerformFixedUpdate(float dt);
        protected abstract void PerformLateUpdate(float dt);
        protected abstract void PauseChanged(bool oldPause, bool newPause);
    }
}
