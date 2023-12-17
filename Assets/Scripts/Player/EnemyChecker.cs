using AI;
using UnityEngine;

namespace Player
{
    public class EnemyChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyController enemy))
            {
                enemy.CheckInfected();
            }
        }
    }
}
