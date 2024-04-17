namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;

    public class CharacterHealthController : MonoBehaviour, IDamageable
    {
        [SerializeField] private int _maxHealth = 100;
        private float _health;
        public delegate void OnDamageFeedback();
        public OnDamageFeedback onDamageFeedback;
        public delegate void OnDead();
        public OnDead onDead;
        public float Health => _health;
        public float MaxHealth => _maxHealth;

        public void Init()
        {
            _health = _maxHealth;
        }

        public void Damage(int amount)
        {
            _health -= amount;

            if (_health <= 0)
            {
                _health = 0;
                if (onDead != null) onDead();
                return;
            }

            if (onDamageFeedback != null) onDamageFeedback();
        }
    }
}