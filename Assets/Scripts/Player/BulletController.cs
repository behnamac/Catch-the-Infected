using AI;
using UnityEngine;

namespace Player
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float damage;

        private void Start()
        {
            Invoke(nameof(Deactivate), 3);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                enemy.TakeDamage(damage);
                Deactivate();
            }
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }
    }
}
