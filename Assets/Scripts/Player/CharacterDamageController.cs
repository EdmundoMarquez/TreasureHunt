namespace Treasure.Player
{
    using UnityEngine;
    using Treasure.Common;
    using Treasure.EventBus;
    using System.Collections;

    public class CharacterDamageController : MonoBehaviour
    {
        [SerializeField] private float _damageThrust = 3f;
        private Rigidbody2D _rigidbody;
        private CharacterHealthController _healthController;
        private Coroutine ApplyFrictionCoroutine;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Init(CharacterHealthController healthController)
        {
            _healthController = healthController;
            _healthController.onDamageFeedback += DamageFeedback;
        }

        private void DamageFeedback()
        {
            
            _rigidbody.AddForce(GetThrustDirection() * _damageThrust, ForceMode2D.Impulse);

            if(ApplyFrictionCoroutine != null)
            {
                StopCoroutine(ApplyFrictionCoroutine);
            }
            ApplyFrictionCoroutine = StartCoroutine(ApplyFriction_Timer());
            
        }

        private IEnumerator ApplyFriction_Timer()
        {
            yield return new WaitForSeconds(0.5f);
            _rigidbody.velocity = Vector2.zero;
        }

        public Vector2 GetThrustDirection()
        {
            Vector2 direction = Vector2.zero;

            //Get direction out of walls players can get cornered
            if(!Physics2D.Raycast(transform.position, Vector2.up, _damageThrust))
                direction.y -= 1;
            if(!Physics2D.Raycast(transform.position, Vector2.down, _damageThrust))
                direction.y += 1;
            if(!Physics2D.Raycast(transform.position, Vector2.left, _damageThrust))
                direction.x += 1;
            if(!Physics2D.Raycast(transform.position, Vector2.right, _damageThrust))
                direction.y -= 1;
            
            if(direction == Vector2.zero)
                direction = RandomizeDirection();

            Debug.Log(direction);

            return direction;
        }

        public Vector2 RandomizeDirection()
        {
            Vector2 randomizedDirection = Vector2.zero;

            float randomValue = Random.value;
            if(randomValue < 0.8f) randomizedDirection = Vector2.left;
            else if(randomValue < 0.6f) randomizedDirection = Vector2.right;
            else if(randomValue < 0.4f) randomizedDirection = Vector2.up;
            else randomizedDirection = Vector2.down;

            return randomizedDirection;
        }
    }
}