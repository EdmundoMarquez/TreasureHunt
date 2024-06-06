namespace Treasure.Dungeon
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.EventBus;

    public class GameStatesController : MonoBehaviour, IEventReceiver<OnPlayerCharactersGenerated>, IEventReceiver<OnPlayerCharacterDefeated>, IEventReceiver<OnCompletedDungeon>
    {
        public static GameStatesController Instance;
        private GameStates _state;
        public GameStates State => _state;
        private bool _initiliazed = false;
        private IPlayableCharacter[] _playableCharacters;
        private string _instigatorId;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
        }

        private void CheckPlayerCharactersStatus()
        {
            if(!_initiliazed) return;

            int aliveCharacters = 0;
            foreach (var character in _playableCharacters)
                aliveCharacters += character.IsDead ? 0 : 1;
            
            if(aliveCharacters <= 0)
            {
                _initiliazed = false;
                EventBus<GameOverEvent>.Raise(new GameOverEvent
                {
                    instigatorId = _instigatorId
                });
            }
        }

        public void OnEvent(OnPlayerCharactersGenerated e)
        {
            _initiliazed = true;
            _playableCharacters = e.characters;
        }

        
        public void OnEvent(OnPlayerCharacterDefeated e)
        {
            _initiliazed = true;
            _instigatorId = e.damageInstigator;
            CheckPlayerCharactersStatus();
        }

        public void OnEvent(OnCompletedDungeon e)
        {
            _initiliazed = false;
        }

        public void ChangeState(int state)
        {
            if(!_initiliazed) return;
            _state = (GameStates)state;
        }

        private void OnEnable()
        {
            EventBus<OnPlayerCharactersGenerated>.Register(this);
            EventBus<OnPlayerCharacterDefeated>.Register(this);
            EventBus<OnCompletedDungeon>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnPlayerCharactersGenerated>.UnRegister(this);
            EventBus<OnPlayerCharacterDefeated>.UnRegister(this);
            EventBus<OnCompletedDungeon>.UnRegister(this);
        }
    }


    public enum GameStates
    {
        NAVIGATION,
        PAUSE,
        GAME_OVER
    }
}