using Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionControl : MonoBehaviour
    {
        private PlayerAttackController _playerAttack;
        private void Awake()
        {
            _playerAttack = GetComponent<PlayerAttackController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ammu"))
            {
                _playerAttack.AddAmmu();
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("Coin"))
            {
                UiController.Instance.AddCoin(1);
                Destroy(other.gameObject);
            }
            else if (other.gameObject.CompareTag("FinishLine"))
            {
                LevelManager.instance.LevelComplete();
            }
        }
    }
}