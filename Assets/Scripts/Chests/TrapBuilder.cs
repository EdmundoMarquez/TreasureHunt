namespace Treasure.Chests
{
    using UnityEngine;
    using Treasure.Common;
    using System.Collections.Generic;

    public class TrapBuilder : MonoBehaviour
    {
        [SerializeField] private ContactFilter2D _contactFilter;
        [SerializeField] private DamageInstigator _spikeTrap = null;
        [SerializeField] private DataProperty[] _spikeTrapProperties;
        [SerializeField] private ArrowTrap _arrowTrap = null;
        [SerializeField] private FireTrap _fireTrap = null;
        private List<DamageInstigator> _spikeTraps = new List<DamageInstigator>();
        private List<ArrowTrap> _arrowTraps = new List<ArrowTrap>();
        private List<FireTrap> _fireTraps = new List<FireTrap>();
        private Vector2 _projectileMoveDirection;
        private TrapType _trapType;
        public void Init(int trapType)
        {
            _trapType = (TrapType)trapType;

            switch (_trapType)
            {
                case TrapType.Spikes:
                    BuildSpikesFromPrefab();
                    break;
                case TrapType.Arrow: 
                    BuildArrowFromPrefab();
                    break;
                case TrapType.Fire: 
                    BuildFireFromPrefab();
                    break;
                default: break;
            }
        }

        private void BuildFireFromPrefab()
        {
            _fireTraps = new List<FireTrap>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Vector2 squarePosition = new Vector2(x, y);
                    RaycastHit2D[] results = new RaycastHit2D[8];
                    int hits = Physics2D.Raycast((Vector2)transform.position + squarePosition, Vector2.zero, _contactFilter, results);

                    if (hits < 1) //if there is a tile
                    {
                        FireTrap trap = Instantiate(_fireTrap);
                        trap.transform.position = (Vector2)transform.position + squarePosition;
                        trap.Init();
                        _fireTraps.Add(trap);
                    }
                    else
                    {
                        Debug.DrawRay((Vector2)transform.position + squarePosition, Vector2.down, Color.blue, 1000);
                        // Debug.Log(results[0].transform.name + " : " + squarePosition.ToString());
                    }
                }
            }
        }

        private void BuildArrowFromPrefab()
        {
            int randomArrowCount = Random.Range(1, 5);

            for (int i = 0; i < randomArrowCount; i++)
            {
                ArrowTrap trap = Instantiate(_arrowTrap);
                trap.Init();
                _arrowTraps.Add(trap);
            }
        }

        private void BuildSpikesFromPrefab()
        {
            _spikeTraps = new List<DamageInstigator>();

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    Vector2 squarePosition = new Vector2(x, y);
                    RaycastHit2D[] results = new RaycastHit2D[8];
                    int hits = Physics2D.Raycast((Vector2)transform.position + squarePosition, Vector2.zero, _contactFilter, results);

                    if (hits < 1) //if there is a tile
                    {
                        DamageInstigator spike = Object.Instantiate(_spikeTrap, transform);
                        spike.transform.position = (Vector2)transform.position + squarePosition;
                        spike.Init(_spikeTrapProperties);
                        _spikeTraps.Add(spike);
                    }
                    else
                    {
                        Debug.DrawRay((Vector2)transform.position + squarePosition, Vector2.down, Color.blue, 1000);
                        // Debug.Log(results[0].transform.name + " : " + squarePosition.ToString());
                    }
                }
            }

            foreach (var trap in _spikeTraps)
                trap.ToggleInstigator(false);
        }

        public void ActivateGeneratedTraps()
        {
            foreach (var trap in _spikeTraps)
            {
                trap.ToggleInstigator(true);
                trap.transform.GetChild(0).gameObject.SetActive(false);
                trap.transform.GetChild(1).gameObject.SetActive(true);
            }

            foreach (var trap in _arrowTraps)
                trap.Activate();

            foreach (var trap in _fireTraps)
                trap.Activate();
        }
    }

    public enum TrapType
    {
        None,
        Fire,
        Arrow,
        Spikes,
    }
}

