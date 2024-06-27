namespace Treasure.Player
{
    using UnityEngine;
    using UnityEngine.UI;
    using Treasure.Common;
    using Treasure.EventBus;
    using DG.Tweening;

    public class CharacterMessageBox : MonoBehaviour, IEventReceiver<InventoryFullMessageEvent>, IEventReceiver<CharacterRequiredMessageEvent>,
    IEventReceiver<ChestTriesDepletedMessageEvent>
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private Image _messageIcon = null;
        [SerializeField] private GameObject _crossIcon = null;
        [Header("Configurations")]
        [SerializeField] private Sprite _inventoryIcon = null;
        [SerializeField] private Sprite _lockIcon = null;
        [SerializeField] private Sprite _warriorIcon = null;
        [SerializeField] private Sprite _sageIcon = null;
        [SerializeField] private ObjectId _warriorId;
        private IPlayableCharacter _character;
        private Sequence ShowMessageSequence;
        public void Init(IPlayableCharacter character)
        {
            _character = character;
        }

        public void OnEvent(InventoryFullMessageEvent e)
        {
            if(!_character.IsActive) return;

            if(ShowMessageSequence != null)
                ShowMessageSequence.Complete();

            ShowMessageSequence = DOTween.Sequence();

            _messageIcon.sprite = _inventoryIcon;
            _crossIcon.SetActive(true);

            ShowMessageSequence.Append(_canvasGroup.DOFade(1f, 1f))
            .AppendInterval(3f)
            .Append(_canvasGroup.DOFade(0f, 1f))
            .OnComplete(() => { _crossIcon.SetActive(false); });
        }

        public void OnEvent(CharacterRequiredMessageEvent e)
        {
            if(!_character.IsActive) return;

            if(ShowMessageSequence != null)
                ShowMessageSequence.Complete();

            ShowMessageSequence = DOTween.Sequence();

            if(e.characterId == _warriorId.Value)
                _messageIcon.sprite = _warriorIcon;
            else
                _messageIcon.sprite = _sageIcon;

            ShowMessageSequence.Append(_canvasGroup.DOFade(1f, 1f))
            .AppendInterval(3f)
            .Append(_canvasGroup.DOFade(0f, 1f));
        }

        public void OnEvent(ChestTriesDepletedMessageEvent e)
        {
            if(!_character.IsActive) return;

            if(ShowMessageSequence != null)
                ShowMessageSequence.Complete();

            ShowMessageSequence = DOTween.Sequence();

            _messageIcon.sprite = _lockIcon;
            _crossIcon.SetActive(true);

            ShowMessageSequence.Append(_canvasGroup.DOFade(1f, 1f))
            .AppendInterval(3f)
            .Append(_canvasGroup.DOFade(0f, 1f))
            .OnComplete(() => { _crossIcon.SetActive(false); });
        }

        private void OnEnable()
        {
            EventBus<InventoryFullMessageEvent>.Register(this);
            EventBus<ChestTriesDepletedMessageEvent>.Register(this);
            EventBus<CharacterRequiredMessageEvent>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<InventoryFullMessageEvent>.UnRegister(this);
            EventBus<CharacterRequiredMessageEvent>.UnRegister(this);
            EventBus<ChestTriesDepletedMessageEvent>.UnRegister(this);
        }

    }
}