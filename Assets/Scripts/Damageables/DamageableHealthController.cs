using System;
using UnityEngine;

public class DamageableHealthController : MonoBehaviour, IDamageable
{
    [SerializeField] private int _maxHealth = 10;
    private float _health;
    public delegate void OnDamageFeedback();
    public OnDamageFeedback onDamageFeedback;
    public delegate void OnDestroyDamageable();
    public OnDestroyDamageable onDestroyDamageable;

    private void Start()
    {
        _health = _maxHealth;
    }

    public void Damage(int amount)
    {
        _health -= amount;

        if(_health <= 0)
        {
            _health = 0;
            if(onDestroyDamageable != null) onDestroyDamageable();
            return;
        }

        if(onDamageFeedback != null) onDamageFeedback();
    }
}