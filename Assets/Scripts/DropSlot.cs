using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class DropSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private ObjectId _objectId = null;
    [SerializeField] private Image _slotImage = null;
    private RectTransform rectTransform;

    private void Awake() 
    {
        rectTransform = GetComponent<RectTransform>();    
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            DragItem item = eventData.pointerDrag.GetComponent<DragItem>();
            if(item.Id.Value == _objectId.Value)
            {
                _slotImage.DOFade(0f, 0.5f);
                _slotImage.rectTransform.DOAnchorPosY(-45f, 0.8f);
                Debug.Log("Lock Removed");
            }
            else
            {
                _slotImage.rectTransform.DOShakeAnchorPos(0.8f, new Vector3(2,0,0));
            }
        }
    }
}
