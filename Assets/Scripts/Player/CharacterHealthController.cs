namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;

    public class CharacterHealthController : MonoBehaviour, IDamageable
    {
        private int _maxHealth;
        private float _health;
        public delegate void OnDamageFeedback();
        public OnDamageFeedback onDamageFeedback;
        public delegate void OnDead();
        public OnDead onDead;
        public delegate void OnHealFeedback();
        public OnHealFeedback onHealFeedback;
        public float Health => _health;
        public float MaxHealth => _maxHealth;
        private bool _canTakeDamage;

        public void Init(int maxHealth)
        {
            _maxHealth = maxHealth;
            _health = _maxHealth;
        }

        public void Toggle(bool toggle)
        {
            _canTakeDamage = toggle;
        }

        public void Heal(int amount)
        {
            _health += amount;
            _health = Mathf.Clamp(_health, 0, MaxHealth);
            
            if (onHealFeedback != null) onHealFeedback();
        }

        public void Damage(int amount)
        {
            if(!_canTakeDamage) return;

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