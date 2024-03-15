using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SwordAttackController : MonoBehaviour
{
    [SerializeField] private Transform _swordPivot = null;
    [SerializeField] private DamageInstigator _damageInstigator = null;
    [SerializeField] private float _attackTime = 0.8f;
    private float _attackTimer;
    private bool _canAttack;

    private void Start()
    {
        _damageInstigator.Init(1);
    }

    public void Toggle(bool toggle) 
    { 
        _canAttack = toggle;
        _damageInstigator.ToggleInstigator(false);
    }

    private void Update()
    {
        if(!_canAttack) return;

        _damageInstigator.ToggleInstigator(_attackTimer > 0f);

        if(_attackTimer > 0f)
        {
            _attackTimer -= Time.deltaTime;
            return;
        }

        if(Input.GetKeyDown(KeyCode.Z))
        {
            _attackTimer = _attackTime;
            _swordPivot.DOLocalRotate(new Vector3(0, 0, -360), _attackTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(Ease.Linear);
        }
    }
}
