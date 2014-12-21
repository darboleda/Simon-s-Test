using System.Collections.Generic;

using UnityEngine;

namespace Canal.Unity.Modes
{
    public class GameModeManager : Behavior
    {
        public ModeConfiguration StartingConfiguration;

        private Dictionary<ModeConfiguration, GameMode> loadedModes = new Dictionary<ModeConfiguration, GameMode>();
        private GameMode currentGameMode;

        public void Awake()
        {
            if (StartingConfiguration != null)
            {
                SetGameMode(StartingConfiguration);
            }
        }

        public void SetGameMode(ModeConfiguration configuration)
        {
            GameMode previousGameMode = currentGameMode;
            currentGameMode = LoadGameMode(configuration);
            currentGameMode.LoadRequiredFeatures();

            // TODO unload previous game mode
        }

        public T GetFeature<T>(string id)
        {
            return currentGameMode.GetFeature<T>(id);
        }

        private GameMode LoadGameMode(ModeConfiguration configuration)
        {
            GameMode gameMode;
            if (loadedModes.TryGetValue(configuration, out gameMode)) return gameMode;

            gameMode = new GameObject().AddComponent<GameMode>();
            loadedModes[configuration] = gameMode;

            ModeConfiguration parent = Resources.Load(configuration.ParentAssetPath) as ModeConfiguration;
            GameMode parentGameMode = null;
            if (parent != null)
            {
                parentGameMode = LoadGameMode(parent);
            }

            gameMode.transform.parent = (parentGameMode == null ? transform : parentGameMode.transform);
            gameMode.Initialize(configuration, parentGameMode);
            Debug.Log("Initialized " + configuration.name);
            return gameMode;
        }
    }
}

