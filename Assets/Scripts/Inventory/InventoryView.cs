using UnityEngine;
using DG.Tweening;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup = null;
    [SerializeField] private SwordInfoBox _currentSwordInfoBox = null;
    [SerializeField] private InventoryController _inventoryController = null;

    public void ToggleVisibility(bool toggle)
    {
        Time.timeScale = toggle ? 0f : 1f;
        _canvasGroup.DOFade(toggle ? 1:0, 0.3f).SetUpdate(true);
        _canvasGroup.interactable = toggle;
        _canvasGroup.blocksRaycasts = toggle;

        if(toggle) UpdateInventory();
    }

    private void UpdateInventory()
    {
        SwordData equippedSword = SwordFactory.Instance.GetSwordById(_inventoryController.EquippedSword);
        _currentSwordInfoBox.Fill(equippedSword.SwordId.Value, equippedSword.SwordImage, equippedSword.Damage);
    }
}
