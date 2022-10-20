using System;
using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace Player
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float damage;
        private void Start()
        {
            Invoke(nameof(Diactive), 3);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<EnemyController>())
            {
                var enemy = other.GetComponent<EnemyController>();
                enemy.TakeDamage(damage);
                Diactive();
            }
        }

        private void Diactive()
        {
            gameObject.SetActive(false);
        }
    }
}
