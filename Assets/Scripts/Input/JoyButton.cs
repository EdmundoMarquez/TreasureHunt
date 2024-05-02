using UnityEngine;
using UnityEngine.EventSystems;

namespace Treasure.PlayerInput
{
    public class JoyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public bool IsPressed { get; private set; }
        public bool IsPressedUp { get; private set; }
        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            IsPressedUp = false;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            IsPressedUp = true;
        }

    }

}
