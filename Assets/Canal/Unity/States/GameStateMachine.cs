using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Canal.Unity.Modes;

namespace Canal.Unity.States
{
    public class GameStateMachine : Behavior
    {
        [System.Serializable]
        public struct GameStateMap
        {
            public string Id;
            public string AssetPath;
        }

        public delegate void StateChangedHandler(GameState oldState, GameState newState);
        public event StateChangedHandler OnStateChanged;

        private Dictionary<string, GameState> loadedStates = new Dictionary<string, GameState>();

        [SerializeField]
        private GameStateMap[] gameStates;

        [SerializeField]
        private GameModeManager modeManager;

        private GameState currentState;

        public void Awake()
        {
            RequestState(gameStates[0]);
        }

        public void RequestState(string stateId)
        {
            GameStateMap map = gameStates.FirstOrDefault(x => x.Id == stateId);
            if (map.Id == stateId)
            {
                RequestState(map);
            }
        }

        public T RequestFeature<T>(string id)
        {
            return modeManager.GetFeature<T>(id);
        }

        private void RequestState(GameStateMap map)
        {
            GameState oldState = currentState;
            GameState newState;
            if (loadedStates.TryGetValue(map.Id, out newState))
            {
                newState = GameObject.Instantiate(newState) as GameState;
            }
            else
            {
                newState = Resources.Load<GameObject>(map.AssetPath).GetComponent<GameState>();
                loadedStates[map.Id] = newState;
                newState = GameObject.Instantiate(newState) as GameState;
            }

            newState.name = string.Format("Current State: {0}", map.Id);
            newState.transform.parent = transform;
            RequestState(oldState, newState);
            if (oldState != null)
            {
                GameObject.Destroy(oldState.gameObject);
            }
        }

        private void RequestState(GameState oldState, GameState newState)
        {
            if (oldState != null)
            {
                oldState.Exit(this, newState);
            }
            currentState = newState;
            newState.Enter(this, oldState);
            modeManager.SetGameMode(newState.Configuration);
            if (OnStateChanged != null) OnStateChanged(oldState, newState);
        }
    }
}
