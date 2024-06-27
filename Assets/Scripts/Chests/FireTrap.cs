namespace Treasure.Chests
{
    using UnityEngine;
    using Treasure.Common;
    using System;
    using System.Collections;
    using DG.Tweening;
    using Random = UnityEngine.Random;

    public class FireTrap : MonoBehaviour
    {
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private ParticleSystem[] _fireFx;
        public void Init()
        {
            _damageInstigator.Init(_damageProperties);
        }
        public void Activate()
        {
            _damageInstigator.ToggleInstigator(true, 3f);

            foreach (var fx in _fireFx)
                fx.Play();
        }
    }
}

