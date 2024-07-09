namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.EventBus;
    using System.Collections;

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
        private bool _isActive;
        private bool _onDamageCooldown;
        private Coroutine DamageCooldownCoroutine;

        public void Init(int health, int maxHealth)
        {
            _health = health;
            _maxHealth = maxHealth;
        }

        public void Toggle(bool toggle)
        {
            _isActive = toggle;
        }

        public void Heal(int amount)
        {
            _health += amount;
            _health = Mathf.Clamp(_health, 0, MaxHealth);
            
            if (onHealFeedback != null) onHealFeedback();
        }

        public void Damage(int amount, string instigatorId = "")
        {
            if(!_isActive || _onDamageCooldown) return;

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

            if(DamageCooldownCoroutine != null)
            {
                StopCoroutine(DamageCooldownCoroutine);
            }
            DamageCooldownCoroutine = StartCoroutine(DamageCooldown_Timer());
        }

        private IEnumerator DamageCooldown_Timer()
        {
            _onDamageCooldown = true;
            yield return new WaitForSeconds(2.5f);
            _onDamageCooldown = false;
        }
    }
}