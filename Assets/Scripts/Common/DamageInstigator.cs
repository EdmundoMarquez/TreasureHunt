using UnityEngine;

public class DamageInstigator : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private InstigatorTags _instigatorTag;
    private DataProperty[] _damageProperties;

    public void Init(DataProperty[] damageProperties)
    {
        _damageProperties = damageProperties;
    }

    public void ToggleInstigator(bool toggle)
    {
        _collider.enabled = toggle;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == _instigatorTag.ToString()) return;

        if(col.TryGetComponent<IDamageable>(out var damageable)) { OnDamage(damageable); }
    }

    private void OnDamage(IDamageable damageable)
    {
        foreach (var damage in _damageProperties)
        {
            damageable.Damage(damage.amount);
        }
    }
}

public enum InstigatorTags
{
    Player,
    Enemy
}