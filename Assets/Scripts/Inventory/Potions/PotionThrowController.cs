namespace Treasure.Inventory.Potions
{
    using UnityEngine;
    using Treasure.EventBus;
    using DG.Tweening;
    using System.Collections;
    using Treasure.Common;

    public class PotionThrowController : MonoBehaviour, IEventReceiver<ThrowPotionItem>
    {
        [SerializeField] private Camera _mainCamera = null;
        [SerializeField] private CanvasGroup _inventoryCanvas = null;
        [SerializeField] private PotionFactory _potionFactory = null;
        [SerializeField] private SpriteRenderer _potionCrosshair = null;
        [SerializeField] private ContactFilter2D _contactFilter;
        private bool _isThrowingPotion;
        private string _throwPotionId;
        private Coroutine ThrowPotionCoroutine;

        public void OnEvent(ThrowPotionItem e)
        {
            ToggleCrosshair(true);

            PotionData potionData = _potionFactory.GetPotionById(e.potionId);
            _potionCrosshair.sprite = potionData.PotionImage;
            _throwPotionId = potionData.Properties.propertyId.Value;
        }

        private void Update()
        {
            if (!_isThrowingPotion) return;

            Vector2 mousePosition = Input.mousePosition;
            Vector3 targetPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
            targetPosition.z = 10f;

            _potionCrosshair.transform.position = targetPosition;

            if (!Input.GetMouseButtonDown(0)) return;
            RaycastHit2D[] results = new RaycastHit2D[8];
            int hits = Physics2D.Raycast(targetPosition, Vector2.down, _contactFilter, results);
            if (hits > 0)
            {
                IPlayableCharacter character = results[0].transform.GetComponent<IPlayableCharacter>();
                
                if(ThrowPotionCoroutine != null)
                {
                    StopCoroutine(ThrowPotionCoroutine);
                }
                ThrowPotionCoroutine = StartCoroutine(ThrowPotionToCharacter(character.CharacterId.Value));
            }
        }

        private IEnumerator ThrowPotionToCharacter(string characterId)
        {
            Debug.Log("Throw potion at " + characterId);
            EventBus<RemovePotionItem>.Raise(new RemovePotionItem
            {
                potionId = _throwPotionId
            });
            EventBus<OnPotionSelectCharacter>.Raise(new OnPotionSelectCharacter
            {
                potionId = _throwPotionId,
                selectedCharacterId = characterId
            });
            
            yield return new WaitForSecondsRealtime(1.5f);
            ToggleCrosshair(false);
        }

        private void ToggleCrosshair(bool toggle)
        {
            _inventoryCanvas.alpha = toggle ? 0f : 1f;
            _inventoryCanvas.interactable = !toggle;
            _inventoryCanvas.blocksRaycasts = !toggle;
            _potionCrosshair.gameObject.SetActive(toggle);
            _isThrowingPotion = toggle;
        }

        private void OnEnable()
        {
            EventBus<ThrowPotionItem>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<ThrowPotionItem>.UnRegister(this);
        }
    }
}