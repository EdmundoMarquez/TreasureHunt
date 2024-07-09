
namespace Treasure.Damageables
{
    using UnityEngine;
    using Treasure.Common;
    using DG.Tweening;
    public class DeadState : IState
    {
        private SpriteRenderer[] _sprites;
        private Collider2D[] _colliders;
        private bool _shakeSprite;
        public DeadState(SpriteRenderer[] sprites, Collider2D[] colliders, bool shakeSprite)
        {
            _sprites = sprites;
            _colliders = colliders;
            _shakeSprite = shakeSprite;
        }

        public void Awake(){}

        public void Init()
        {
            foreach (var sprite in _sprites)
            {
                if(_shakeSprite) sprite.transform.DOShakePosition(0.4f, 0.5f);
                sprite.DOFade(0f, 0.4f);
            }

            foreach (var collider in _colliders)
            {
                collider.enabled = false;
            }
        }

        public void Tick()
        {
        }

        public void FixedTick(){}

        public void Stop()
        {
            
        }
    }
}