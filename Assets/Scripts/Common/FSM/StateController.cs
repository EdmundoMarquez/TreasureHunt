using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Treasure.Common
{
    public class StateController : MonoBehaviour, IStateController
    {
        private Dictionary<int, IState> _idToState;
        private IState _currentState;
        public int CurrentStateIndex {get; private set;}
        public void Init(Dictionary<int, IState> idToState)
        {
            _idToState = idToState;
        }
        public void StartFromState(int state)
        {
            _currentState = GetState(state);
            _currentState.Awake();
            _currentState.Init();
            CurrentStateIndex = state;
        }

        public void ChangeToNextState(int nextState)
        {
            _currentState?.Stop();
            _currentState = GetState(nextState);
            _currentState.Init();
            CurrentStateIndex = nextState;
        }

        public void StopStates()
        {
            _currentState?.Stop();
        }

        private IState GetState(int playerState)
        {
            if(!_idToState.TryGetValue(playerState, out IState state))  
                return null;
            return state;
        }
        public void DelayChangeToNextState(int nextState, int delayTime)
        {
            StartCoroutine(DelayOnChangeState(nextState, delayTime));
        }
        private IEnumerator DelayOnChangeState(int nextState, int delayTime)
        {
            yield return delayTime;
            ChangeToNextState(nextState);
        }
        private void FixedUpdate()
        {
            _currentState?.FixedTick();
        }
        private void Update()
        {
            _currentState?.Tick();
        }

        private void OnDisable() {
            _currentState?.Stop();
        }
        
    }
}