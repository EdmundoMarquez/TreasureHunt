namespace Treasure.Chests
{
    using UnityEngine;
    using Treasure.Common;
    using System;
    using System.Collections;
    using DG.Tweening;
    using Random = UnityEngine.Random;

    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private DataProperty[] _damageProperties;
        [SerializeField] private DamageInstigator _damageInstigator = null;
        private SpriteRenderer _arrowSprite;
        private Vector2 _moveDirection;
        private bool canTick;
        
        public void Init()
        {
            _arrowSprite = GetComponent<SpriteRenderer>();
            _arrowSprite.enabled = false;
            _damageInstigator.Init(_damageProperties);
            _damageInstigator.onHit += OnHit;
        }
        public void Activate()
        {
            _damageInstigator.ToggleInstigator(true);
            transform.position = GetStartPoint();
            canTick = true;
        }

        private void Update()
        {
            if(!canTick) return;
            transform.position += (Vector3)_moveDirection * _speed * Time.deltaTime;

            if(_arrowSprite.enabled) return;
            Collider2D[] results = new Collider2D[2];
            int hits = Physics2D.OverlapCircle(transform.position, 1f, _contactFilter, results);

            if(hits > 0)
                _arrowSprite.enabled = true;
        }

        private Vector2 GetStartPoint()
        {
            var spriteSize = GetComponent<SpriteRenderer>().bounds.size.x * .5f; // Working with a simple box here, adapt to you necessity

            var cam = Camera.main;// Camera component to get their size, if this change in runtime make sure to update values
            var camHeight = cam.orthographicSize;
            var camWidth = cam.orthographicSize * cam.aspect;

            float yMin = -camHeight + spriteSize; // lower bound
            float yMax = camHeight - spriteSize; // upper bound
        
            float xMin = -camWidth + spriteSize; // left bound
            float xMax = camWidth - spriteSize; // right bound 

            float randValue = Random.value;
            Vector2 spawnPoint = Vector2.zero;
            Vector2 offset = (Vector2)(Camera.main.transform.position - transform.position);

            if(randValue > 0.75f) //upwards
            {
                transform.rotation = Quaternion.Euler(0,0,180);
                spawnPoint = new Vector2(0 , yMax);
                _moveDirection = Vector2.down;
            }
            else if (randValue > 0.5f) //downwards
            {
                spawnPoint = new Vector2(0 , yMin);
                _moveDirection = Vector2.up;
            }
            else if(randValue > 0.25f) //left
            {
                transform.rotation = Quaternion.Euler(0,0,-90);
                spawnPoint = new Vector2(xMin , 0);
                _moveDirection = Vector2.right;
            }
            else //right
            {
                transform.rotation = Quaternion.Euler(0,0,90);
                spawnPoint = new Vector2(xMax, 0);
                _moveDirection = Vector2.left;
            }
            return spawnPoint + offset;
        }

        private void OnHit()
        {
            gameObject.SetActive(false);
        }


        private IEnumerator AutoHide()
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }
    }
}

