namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.EventBus;

    public class CharacterHealthController : MonoBehaviour, IDamageable
    {
        private int _maxHealth;
        private int _health;
        public delegate void OnDamageFeedback();
        public OnDamageFeedback onDamageFeedback;
        public delegate void OnDead();
        public OnDead onDead;
        public delegate void OnHealFeedback();
        public OnHealFeedback onHealFeedback;
        public int Health => _health;
        public int MaxHealth => _maxHealth;
        private bool _canTakeDamage;

        public void Init(int health, int maxHealth)
        {
            _health = health;
            _maxHealth = maxHealth;
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

        public void Damage(int amount, string instigatorId = "")
        {
            if(!_canTakeDamage) return;

            _health -= amount;

            if (_health <= 0)
            {
                _health = 0;

                EventBus<OnPlayerCharacterDefeated>.Raise(new OnPlayerCharacterDefeated
                {
                    damageInstigator = instigatorId
                });
                
                if (onDead != null) onDead();
                return;
            }

            if (onDamageFeedback != null) onDamageFeedback();
        }
    }
}