
namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.PlayerInput;

    public class CharacterInteractionController : MonoBehaviour
    {
        [SerializeField]
        private float _detectionRadius;
        [SerializeField]
        private ContactFilter2D _contactFilter;
        private IInteractable _currentInteractable;
        private IPlayerInput _inputAdapter;
        private bool _canTick = false;

        public void Init(IPlayerInput inputAdapter)
        {
            _inputAdapter = inputAdapter;
        }

        public void Toggle(bool toggle)
        {
            _canTick = toggle;
        }

        public void Tick()
        {
            if(!_canTick) return;

            _currentInteractable = FindInteractableInRange();
            
            if(_inputAdapter.InteractButtonPressed())
            {
                TryInteract();
            }
        }

        private IInteractable FindInteractableInRange()
        {
            IInteractable interactable = null;

            Collider2D[] results = new Collider2D[8];
            int hits = Physics2D.OverlapCircle(transform.position, _detectionRadius, _contactFilter, results);

            if(hits > 0)
            {
                interactable = results[0].transform.GetComponent<IInteractable>();
            }
            return interactable;
        }   

        private void TryInteract()
        {
            if(_currentInteractable == null) return;
            if(!_currentInteractable.CanInteract) return;
            _currentInteractable.Interact();
        }
    }
}