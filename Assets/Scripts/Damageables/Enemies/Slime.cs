namespace Treasure.Damageables
{

    using UnityEngine;
    using DG.Tweening;
    using Treasure.Common;
    using Pathfinding;
    using UnityEngine.Playables;

    public class Slime : MonoBehaviour
    {
        [SerializeField] private DamageableHealthController _healthController = null;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        [SerializeField] private Collider2D _collider = null;
        [SerializeField] private SpriteRenderer _slimeSprite = null;
        [SerializeField] private float _detectionRadius;
        [SerializeField] private ContactFilter2D _contactFilter;
        private IAstarAI ai;
        private Transform player;
        private bool _canTick = true;

        private void Awake()
        {
            ai = GetComponent<IAstarAI>();
            _damageInstigator.Init(_damageProperties);
        }

        private void OnEnable()
        {
            _healthController.onDestroyDamageable += OnDead;
            _healthController.onDamageFeedback += OnDamage;
        }

        private void OnDisable()
        {
            _healthController.onDestroyDamageable -= OnDead;
            _healthController.onDamageFeedback -= OnDamage;
        }

        private void OnDamage()
        {
            // _batSprite.transform.DOShakePosition(0.4f, 0.5f);
            _slimeSprite.DOColor(Color.red, 0.3f).OnComplete(()=>
            { _slimeSprite.DOColor(Color.white, 0.3f); });
        }

        private void OnDead()
        {
            ai.destination = transform.position;
            _slimeSprite.transform.DOShakePosition(0.4f, 0.5f);
            _slimeSprite.DOFade(0f, 0.4f);
            _collider.enabled = false;
            _canTick = false;
        }

        private void Update()
        {
            if(!_canTick) return;
            Collider2D[] results = new Collider2D[8];
            int hits = Physics2D.OverlapCircle(transform.position, _detectionRadius, _contactFilter, results);

            if(hits > 0)
            {
                IPlayableCharacter character = results[0].GetComponent<IPlayableCharacter>();
                if(!character.IsActive) return;
                player = results[0].transform;
            }
            
            if(player == null) return;
            if(Vector3.Distance(transform.position, player.position) > _detectionRadius * 1.5f) return;
            ai.destination = player.position;
        }
    }
}
