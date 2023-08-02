namespace TheRig.Gun
{
    using UnityEngine;
    using System.Collections;
    using TheRig.Ammo;
    using TheRig.Other;

    public class GlockEntity : Gun
    {
        Camera _playerCam;

        [SerializeField] int _damage = 10;
        [SerializeField] float _coolDownTime = .2f;
        [SerializeField][Min(0.1f)] float _bulletSpeed = 1;
        [SerializeField] Ammo _ammoPrefab;
        [Space(20)]
        [SerializeField] LayerMask _layers;
        [SerializeField] Transform _bulletSpawnPointRight;
        [SerializeField] Transform _bulletSpawnPointLeft;

        bool _isRight = true;
        bool _inCoolDown = false;

        protected override void Start()
        {
            base.Start();
            _ammo = MaxAmmo;
        }

        IEnumerator CoolDown()
        {
            _inCoolDown = true;
            yield return new WaitForSecondsRealtime(_coolDownTime);
            _inCoolDown = false;
        }

        public override void Shoot(Vector3 vel)
        {
            if(_ammo <= 0) return;

            if (_inCoolDown) return;
            StartCoroutine(CoolDown());

            _ammo -= 1;
            _ammo = Mathf.Clamp(_ammo, 0, MaxAmmo);

            GunHasFired(_ammo);

            if (_ammo == 0)
            {
                Reload();
            }

            Transform spawnPoint = null;
            if (_isRight)
            {
                spawnPoint = _bulletSpawnPointRight;
                _isRight = false;
            }
            else
            {
                spawnPoint = _bulletSpawnPointLeft;
                _isRight = true;
            }

            Ammo bullet = Instantiate(_ammoPrefab,
            spawnPoint.position,
            Quaternion.identity) as Ammo;

            bullet.transform.position += vel;

            bullet.FireUp(transform.forward, _bulletSpeed, _damage);
        }
    }
}
