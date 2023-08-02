namespace TheRig.Other
{
    using UnityEngine;
    using TheRig.Player;
    using TheRig.GameEvents;

    public class EnemyXpDrop : MonoBehaviour
    {
        [SerializeField] DummyEntity _enemy;

        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerEntity _player))
            {
                GetComponent<Collider>().enabled = false;
                _player.GetXp(_enemy.XpToDrop);
            }
        }
    }
}
