namespace Treasure.Player
{
    using UnityEngine;
    using DG.Tweening;
    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Inventory;
    using Treasure.Swords;

    public class SwordAttackController : MonoBehaviour, IEventReceiver<AddSwordItem>
    {
        [SerializeField] private Transform _swordPivot = null;
        [SerializeField] private SpriteRenderer _swordSprite = null;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private InventoryController _inventoryController = null;
        [SerializeField] private float _attackTime = 0.8f;
        private float _attackTimer;
        private bool _canAttack;

        private void Start()
        {
            UpdateSword(_inventoryController.EquippedSword);
        }

        private void UpdateSword(string swordId)
        {
            SwordData equippedSword = SwordFactory.Instance.GetSwordById(swordId);
            _swordSprite.sprite = equippedSword.SwordImage;
            _damageInstigator.Init(equippedSword.Damage);
        }

        public void OnEvent(AddSwordItem e)
        {
            UpdateSword(e.newItemId);
        }

        private void OnEnable()
        {
            EventBus<AddSwordItem>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<AddSwordItem>.UnRegister(this);
        }

        public void Toggle(bool toggle)
        {
            _canAttack = toggle;
            _damageInstigator.ToggleInstigator(false);
        }

        public void Attack()
        {
            if (!_canAttack || _attackTimer > 0f) return;
            _attackTimer = _attackTime;
            _swordPivot.DOLocalRotate(new Vector3(0, 0, -360), _attackTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
        }

        private void Update()
        {
            if (!_canAttack) return;

            _damageInstigator.ToggleInstigator(_attackTimer > 0f);

            if (_attackTimer > 0f)
            {
                _attackTimer -= Time.deltaTime;
                return;
            }
        }
    }

}