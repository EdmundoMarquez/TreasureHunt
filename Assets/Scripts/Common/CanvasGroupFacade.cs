namespace Treasure.Common
{
    using UnityEngine;

    public class CanvasGroupFacade : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup[] _canvasGroup;

        public void ToggleVisibility(bool toggle)
        {
            foreach (var canvas in _canvasGroup)
            {
                canvas.interactable = toggle;
                canvas.blocksRaycasts = toggle;
                canvas.alpha = toggle ? 1f : 0f;
            }
        }
    }

}