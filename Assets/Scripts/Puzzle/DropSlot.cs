namespace Treasure.Puzzle
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;
    using DG.Tweening;
    using TMPro;

    public class DropSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField] private Image _slotImage = null;
        [SerializeField] private LetterSlot _letterSlot = null;
        private RectTransform rectTransform;
        private string keyId;
        public delegate void OnUnlocked();
        public OnUnlocked onUnlocked;
        private bool canDropKey = true;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void Init(string letter)
        {
            _letterSlot.Init(letter);
            keyId = letter;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(!canDropKey) return;
            if (eventData.pointerDrag != null)
            {
                DragItem item = eventData.pointerDrag.GetComponent<DragItem>();
                if (item.Id.Value == keyId)
                {
                    _slotImage.DOFade(0f, 0.5f).SetUpdate(true);
                    _slotImage.rectTransform.DOAnchorPosY(-45f, 0.8f).SetUpdate(true);
                    canDropKey = false;
                    if (onUnlocked != null) onUnlocked();
                }
                else
                {
                    _slotImage.rectTransform.DOShakeAnchorPos(0.8f, new Vector3(2, 0, 0)).SetUpdate(true);
                }
            }
        }
    }
}