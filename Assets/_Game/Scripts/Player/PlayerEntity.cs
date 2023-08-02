using System;
using System.Collections;

namespace TheRig.Player
{
    using UnityEngine;
    using UnityEngine.InputSystem;
    using TheRig.Gun;
    using TheRig.GameEvents;

    [RequireComponent(typeof(CharacterController))]
    public class PlayerEntity : MonoBehaviour
    {
        public int CurrentHealth { get; private set; }

        [SerializeField] int _maxHealth = 100;
        [SerializeField] Gun _gun;
        [SerializeField] float _speed = .1f;
        [SerializeField] LayerMask _ground;

        CharacterController _controller;
        bool _isDead;
        int _currentXp;

        void Awake()
        {
            CurrentHealth = _maxHealth;
            _controller = GetComponent<CharacterController>();
        }

        void Start()
        {
            StartCoroutine(Shoot());
        }

        void Update()
        {
            if (_isDead) return;

            Reload();

            Rotation();

            Movement();
        }

        void Reload()
        {
            // if (_reload.action.triggered)
            // {
            //     _gun.ForceReload();
            // }
        }

        void Rotation()
        {
            Vector3 screenPos = ServiceLocator.Get<InputManager>().GetPointerPos();
            Ray ray = ServiceLocator.Get<Camera>(SerLocID.mainCam).ScreenPointToRay(screenPos);

            RaycastHit hit;
            if (Physics.Raycast(ray.origin,
                            ray.direction,
                            out hit, Mathf.Infinity, _ground))
            {
                Vector3 direction = (hit.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            }

        }

        private void Movement()
        {
            Vector2 movementInput = ServiceLocator.Get<InputManager>().GetMovementVectorNormalized();
            Vector3 moveDir = new Vector3(movementInput.x, 0, movementInput.y) * _speed;
            _controller.Move(moveDir * Time.deltaTime);
        }

        IEnumerator Shoot()
        {
            while (_isDead == false)
            {
                _gun.GetComponent<Gun>().Shoot(_controller.velocity * Time.deltaTime);
                yield return new WaitForSecondsRealtime(.2f);
            }
        }

        public void GetDamage(int damage)
        {
            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _maxHealth);

            if (CurrentHealth <= 0)
            {
                HandleDeath();
            }
        }

        void HandleDeath()
        {
            _isDead = true;
        }

        public void GetXp(int xp)
        {
            _currentXp += xp;
        }
    }
}