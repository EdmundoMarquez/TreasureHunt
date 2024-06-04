namespace Treasure.Player
{
    using Treasure.Common;
    using Treasure.Inventory;
    using Treasure.EventBus;
    using DG.Tweening;

    using UnityEngine;
    using System.Collections;

    public class CharacterPotionController : MonoBehaviour, IEventReceiver<OnPotionSelectCharacter> 
    {
        [SerializeField] private ItemDataConfiguration _itemDataConfiguration = null;
        [SerializeField] private SpriteRenderer _characterSprite = null;
        [Header("Configurations")]
        [SerializeField] private GameObject _healingFxPrefab = null;
        [SerializeField] private GameObject _speedFxPrefab = null;
        [SerializeField] private GameObject _invisibilityFxPrefab = null;
        [SerializeField] private GameObject _reviveFxPrefab = null;
        [SerializeField] private Transform _fxParent = null;
        private CharacterHealthController _healthController = null;
        private MovementController _movementController = null;
        private CompanionFollowController _followController = null;
        private string _characterId;

        public void Init(string characterId, MovementController movementController, CompanionFollowController followController,
            CharacterHealthController healthController)
        {
            _characterId = characterId;
            _movementController = movementController;
            _followController = followController;
            _healthController = healthController;
        }

        public void OnEvent(OnPotionSelectCharacter e)
        {
            if(e.selectedCharacterId != _characterId) return;
            ApplyPotion(e.potionId);
        }

        private void ApplyPotion(string potionId)
        {
            foreach(var potion in _itemDataConfiguration.HealingPotions)
            {
                if(potion.Properties.propertyId.Value == potionId)
                {
                    _healthController.Heal(potion.Properties.amount);
                    _characterSprite.DOColor(Color.green, 0.5f).SetLoops(2, LoopType.Yoyo).SetUpdate(true);
                    InstantiateEffect(_healingFxPrefab, 3f, true);
                    return;
                }
            }

            foreach(var potion in _itemDataConfiguration.SpeedPotions)
            {
                if(potion.Properties.propertyId.Value == potionId)
                {
                    _movementController.ChangeSpeed(potion.Properties.amount);
                    _characterSprite.DOColor(Color.blue, 0.5f).SetLoops(2, LoopType.Yoyo).SetUpdate(true);
                    InstantiateEffect(_speedFxPrefab, potion.Properties.amount);
                    return;
                }
            }

            foreach(var potion in _itemDataConfiguration.InvisibilityPotions)
            {
                if(potion.Properties.propertyId.Value == potionId)
                {
                    StartCoroutine(ApplyInvisibility_Timer(potion.Properties.amount));
                    return;
                }
            }

            if(_itemDataConfiguration.RevivePotion.Properties.propertyId.Value == potionId)
            {
                _healthController.Heal(_itemDataConfiguration.RevivePotion.Properties.amount);
                _characterSprite.DOColor(Color.green, 0.5f).SetLoops(2, LoopType.Yoyo).SetUpdate(true);
                InstantiateEffect(_reviveFxPrefab, 3f, true);
            }
        }
        
        private IEnumerator ApplyInvisibility_Timer(float duration)
        {
            InstantiateEffect(_invisibilityFxPrefab, 2f, true);
            yield return new WaitForSeconds(2f);
            _characterSprite.DOFade(0.5f, 0.5f);
            yield return new WaitForSeconds(duration);
            _characterSprite.DOFade(1f, 0.5f);
        }

        private void InstantiateEffect(GameObject effectPrefab, float lifeTime, bool unscaled = false)
        {
            StartCoroutine(InstantiateEffect_Timer(effectPrefab, lifeTime, unscaled));
        }


        private IEnumerator InstantiateEffect_Timer(GameObject effectPrefab, float lifeTime, bool unscaled = false)
        {
            GameObject fx = GameObject.Instantiate(effectPrefab, _fxParent);
            fx.transform.localPosition = Vector3.zero;

            if(unscaled) 
                yield return new WaitForSecondsRealtime(lifeTime);
            else 
                yield return new WaitForSeconds(lifeTime);
            
            Destroy(fx);
        }

        private void OnEnable() 
        {
            EventBus<OnPotionSelectCharacter>.Register(this);
        }

        private void OnDisable() 
        {
            EventBus<OnPotionSelectCharacter>.UnRegister(this);
        }
    }
}