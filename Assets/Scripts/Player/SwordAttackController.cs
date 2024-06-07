namespace Treasure.Player
{
    using UnityEngine;
    using DG.Tweening;
    using Treasure.EventBus;
    using Treasure.Common;
    using Treasure.Inventory;

    public class SwordAttackController : MonoBehaviour, IEventReceiver<EquipSwordItem>
    {
        [SerializeField] private Transform _swordPivot = null;
        [SerializeField] private SpriteRenderer _swordSprite = null;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private float _attackTime = 0.8f;
        private float _attackTimer;
        private bool _canAttack;

        public void Init(string swordId)
        {
            UpdateSword(swordId);
        }

        private void UpdateSword(string swordId)
        {
            SwordData equippedSword = SwordFactory.Instance.GetSwordById(swordId);
            _swordSprite.sprite = equippedSword.SwordImage;
            _damageInstigator.Init(equippedSword.Damage);
        }

        public void OnEvent(EquipSwordItem e)
        {
            UpdateSword(e.swordId);
        }

        private void OnEnable()
        {
            EventBus<EquipSwordItem>.Register(this);
        }

        private void OnDisable()
        {
            EventBus<EquipSwordItem>.UnRegister(this);
        }

        public void Toggle(bool toggle)
        {
            _canAttack = toggle;
            _damageInstigator.ToggleInstigator(false);

            //Set character outline
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

            _swordSprite.GetPropertyBlock(propertyBlock);

            propertyBlock.SetInt("_Intensity", toggle ? 1 : 0);
            _swordSprite.SetPropertyBlock(propertyBlock);
            _swordSprite.sortingOrder = toggle ? 2 : 1;
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