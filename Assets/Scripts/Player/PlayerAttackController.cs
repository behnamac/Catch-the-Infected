using System.Collections.Generic;
using AI;
using Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerAttackController : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private int maxBullet;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private Rigidbody bullet;
        [SerializeField] private float attackRadius;
        [SerializeField] private float delayAttack;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private LayerMask targetLayer;
        
        [Header("UI Settings")]
        [SerializeField] private GameObject bulletImage;
        [SerializeField] private Transform bulletImageHolder;

        private List<GameObject> _bulletImages;
        private List<Rigidbody> _bulletPool;
        private Transform _bulletPoolParent;
        private EnemyController _enemy;
        private float _currentDelayAttack;

        public int currentBullet { get; set; }

        private void Awake()
        {
            _bulletImages = new List<GameObject>();
            _bulletPool = new List<Rigidbody>();

            _bulletPoolParent = new GameObject("BulletPool").transform;
            currentBullet = maxBullet;
            
            GenerateBulletImage();
        }

        private void Update()
        {
            if (_enemy)
            {
                Attack();
            }
            else
            {
                CheckInfected();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }
        private void CheckInfected()
        {
            Collider[] enemys = new Collider[50];
            var findEnemy = Physics.OverlapSphereNonAlloc(transform.position, attackRadius, enemys, targetLayer);

            for (int i = 0; i < findEnemy; i++)
            {
                if (enemys[i].GetComponent<EnemyController>() && enemys[i].GetComponent<EnemyController>().infected)
                {
                    _enemy = enemys[i].GetComponent<EnemyController>();
                    return;
                }
            }

            _enemy = null;
        }

        private void Attack()
        {
            if (currentBullet <= 0) return;
            
            _currentDelayAttack -= Time.deltaTime;
            if (_currentDelayAttack > 0) return;

            if (currentBullet - 1 > 0)
                _bulletImages[currentBullet - 1].SetActive(false);
            else
                _bulletImages[0].SetActive(false);
            
            currentBullet--;
            currentBullet = Mathf.Clamp(currentBullet, 0, maxBullet);

            if (currentBullet <= 0)
            {
                LevelManager.instance.LevelFail();
            }

            _currentDelayAttack = delayAttack;
            shootPoint.LookAt(_enemy.transform.position);
            for (int i = 0; i < _bulletPool.Count; i++)
            {
                if (!_bulletPool[i].gameObject.activeInHierarchy)
                {
                    _bulletPool[i].transform.position = shootPoint.position;
                    _bulletPool[i].transform.rotation = shootPoint.rotation;
                    _bulletPool[i].gameObject.SetActive(true);
                    _bulletPool[i].velocity = shootPoint.forward * bulletSpeed;

                    return;
                }
            }

            var bull = Instantiate(bullet, shootPoint.position, shootPoint.rotation, _bulletPoolParent);
            bull.velocity = shootPoint.forward * bulletSpeed;
            _bulletPool.Add(bull);
        }

        private void GenerateBulletImage()
        {
            for (int i = 0; i < maxBullet; i++)
            {
                var image = Instantiate(bulletImage, bulletImageHolder);
                _bulletImages.Add(image.transform.GetChild(0).gameObject);
            }
        }

        public void AddAmmu()
        {
            currentBullet++;
            currentBullet = Mathf.Clamp(currentBullet, 0, maxBullet);
            _bulletImages[currentBullet - 1].SetActive(true);
        }
    }
}
