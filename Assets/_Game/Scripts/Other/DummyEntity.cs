namespace TheRig.Other
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.AI;
    using TheRig.CommonInterfaces;
    using TheRig.Player;
    using TheRig.GameEvents;

    public class DummyEntity : MonoBehaviour, IDamageable
    {
        [field: SerializeField] public int XpToDrop
        {
            get;
            private set;
        } = 5;

        [SerializeField][Min(0)] float _maxHealth = 100;
        [SerializeField] float _attackRange = 5;
        [SerializeField] float _attackInterval = 1;
        [SerializeField] int _damage = 4;
        float _health;
        PlayerEntity _player;
        NavMeshAgent _agent;

        void Start()
        {
            _player = ServiceLocator.Get<PlayerEntity>();
            _agent = GetComponent<NavMeshAgent>();
            if(_agent.stoppingDistance > _attackRange)
            {
                Debug.LogError($"attack range is too small for enemy");
            }
            _health = _maxHealth;
            StartCoroutine(Follow());
            StartCoroutine(CheckForPlayerToAttack());

        }

        IEnumerator Follow()
        {
            while (_health > 0)
            {
                _agent.SetDestination(_player.transform.position);
                yield return new WaitForSecondsRealtime(1);
            }
        }

        IEnumerator CheckForPlayerToAttack()
        {
            while (_health > 0 && _player.CurrentHealth > 0)
            {
                if (Vector3.Distance(transform.position, _player.transform.position) < _attackRange)
                {
                    _player.GetDamage(_damage);
                }

                yield return new WaitForSecondsRealtime(_attackInterval);
            }

        }

        public void GetDamage(int damage)
        {
            if (_health <= 0) return;
            _health -= damage;
            _health = Mathf.Clamp(_health, 0, _maxHealth);
            if (_health <= 0)
            {
                HandleDeath();
            }
        }

        void HandleDeath()
        {
            _agent.enabled = false;
            GetComponent<Collider>().enabled = false;
        }
    }

}