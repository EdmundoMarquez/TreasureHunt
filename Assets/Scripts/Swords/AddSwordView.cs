using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Treasure.EventBus;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class AddSwordView : MonoBehaviour, IEventReceiver<ConfirmAddSwordItem>
{
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private SwordInfoBox _currentSwordInfoBox = null;
    [SerializeField] private SwordInfoBox _newSwordInfoBox = null;
    [SerializeField] private InventoryController _inventoryController = null;
    [SerializeField] private Button _yesButton = null;
    [SerializeField] private Button _noButton = null;
    private string currentSwordId;
    private string newSwordId;
    private GameObject swordObject;

    private void Start()
    {
        _yesButton.onClick.AddListener(SendAddSwordEvent);
        _noButton.onClick.AddListener(()=> ToggleVisibility(false));
    }

    public void OnEvent(ConfirmAddSwordItem e)
    {
        ToggleVisibility(true);

        currentSwordId = _inventoryController.EquippedSword;
        newSwordId = e.itemId;
        swordObject = e.swordObject;

        SwordData currentSword = SwordFactory.Instance.GetSwordById(currentSwordId);
        SwordData newSword = SwordFactory.Instance.GetSwordById(newSwordId);

        _currentSwordInfoBox.Fill(currentSword.SwordId.Value, currentSword.SwordImage, currentSword.Damage);
        _newSwordInfoBox.Fill(newSword.SwordId.Value, newSword.SwordImage, newSword.Damage);
    }

    private void SendAddSwordEvent()
    {
        swordObject.SetActive(false);
        ToggleVisibility(false);

        EventBus<AddSwordItem>.Raise(new AddSwordItem
        {
            previousItemId = currentSwordId,
            newItemId = newSwordId
        });
    }

    private void ToggleVisibility(bool toggle)
    {
        Time.timeScale = toggle ? 0f : 1f;
        _canvasGroup.DOFade(toggle ? 1:0, 0.3f).SetUpdate(true);
        _canvasGroup.interactable = toggle;
        _canvasGroup.blocksRaycasts = toggle;
    }

    private void OnEnable()
    {
        EventBus<ConfirmAddSwordItem>.Register(this);
    }

    private void OnDisable()
    {
        EventBus<ConfirmAddSwordItem>.Register(this);
    }
    
}
