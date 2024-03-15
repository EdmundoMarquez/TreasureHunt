using UnityEngine;

public class DamageInstigator : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    private int _damageAmount;

    public void Init(int damageAmount)
    {
        _damageAmount = damageAmount;
    }

    public void ToggleInstigator(bool toggle)
    {
        _collider.enabled = toggle;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(_damageAmount);
        }
    }
}