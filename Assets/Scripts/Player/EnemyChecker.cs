using AI;
using UnityEngine;

namespace Player
{
    public class EnemyChecker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyController>())
            {
                var enemy = other.GetComponent<EnemyController>();
                enemy.CheckInfected();
            }
        }
    }
}
