namespace Treasure.InfiniteMode
{
    using UnityEngine;
    using UnityEngine.UI;
    using DG.Tweening;
    using TMPro;
    using UnityEngine.Events;
    using Treasure.EventBus;

    public class DungeonLevelCounter : MonoBehaviour, IEventReceiver<OnCompletedDungeon>
    {   
        [SerializeField] private DungeonLevelData _levelData = null;
        [SerializeField] private TMP_Text _counterLevelText = null;
        [SerializeField] private TMP_Text _transitionLevelText = null;
        [SerializeField] private Image _backgroundDimmer = null;
        public UnityEvent DungeonComplete;
        public Sequence TransitionSequence;
        private void Start()
        {
            _counterLevelText.SetText($"Piso {_levelData.currentLevel}");
        }

        public void OnEvent(OnCompletedDungeon e)
        {
            _backgroundDimmer.raycastTarget = true;
            _transitionLevelText.SetText($"Piso {++_levelData.currentLevel}");

            TransitionSequence = DOTween.Sequence();
            TransitionSequence.Append(_backgroundDimmer.DOFade(1f, 0.5f))
            .Append(_transitionLevelText.DOFade(1f, 0.3f))
            .AppendInterval(0.5f)
            .Append(_transitionLevelText.DOFade(0f, 0.5f).OnComplete(()=>
            {
                DungeonComplete?.Invoke();
                _counterLevelText.SetText($"Piso {_levelData.currentLevel}");
            }))
            .Append(_backgroundDimmer.DOFade(0f, 0.5f).OnComplete(() => 
            { 
                _backgroundDimmer.raycastTarget = false; 
            }));
        }

        private void OnEnable()
        {
            EventBus<OnCompletedDungeon>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<OnCompletedDungeon>.UnRegister(this);
        }
    }
}

