namespace TheRig.Gun
{
    using UnityEngine;
    using System.Collections;
    using TheRig.Handler;
    using TheRig.GameEvents;

    public abstract class Gun : MonoBehaviour
    {
        // protected GunHandler _gunHandler;
        // CursorHandler _cursorHandler;

        [field: SerializeField] public Texture2D Crosshair { get; private set; }
        [field: SerializeField] public int MaxAmmo { get; private set; } = 30;
        [SerializeField][Min(0)] float _reloadTime = 10;

        protected int _ammo;

        protected virtual void Start()
        {
            // _cursorHandler.ChangeCursor(Crosshair);
            // _gunHandler.GunChanged(this);
        }

        public virtual void Shoot(Vector3 vel)
        {

        }

        public virtual void ForceReload()
        {
            if (_ammo == 0 || _ammo == MaxAmmo) return;
            _ammo = 0;
            Reload();
        }

        protected void Reload()
        {

            StartCoroutine(CoGunAutOfAmmo()); 

            IEnumerator CoGunAutOfAmmo()
            {

                yield return new WaitForSecondsRealtime(_reloadTime);
                
                ReloadComplete();
            }
        }

        public virtual void ReloadComplete()
        {
            _ammo = MaxAmmo;
        }

        protected void GunHasFired(int remainingAmmo)
        {
        }
    }
}
