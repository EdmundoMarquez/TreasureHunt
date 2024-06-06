namespace Treasure.Dungeon
{
    using UnityEngine;
    using Treasure.Common;
    using DG.Tweening;
    using System.Collections;
    using TMPro;

    public class PauseView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup = null;
        [SerializeField] private CanvasGroupFacade _canvasFacade = null;
        

        public void ToggleVisibility(bool toggle)
        {
            Time.timeScale = toggle ? 0f : 1f;
            _canvasGroup.DOFade(toggle ? 1f: 0f, 0.3f).SetUpdate(true);
            _canvasGroup.interactable = toggle;
            _canvasGroup.blocksRaycasts = toggle;

            _canvasFacade.ToggleVisibility(!toggle);
        }
    }
}