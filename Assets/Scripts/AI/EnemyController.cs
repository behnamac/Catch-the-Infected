using System.Collections;
using Controllers;
using Dreamteck.Splines;
using Levels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AI
{
    [RequireComponent(typeof(SplineFollower))]
    public class EnemyController : MonoBehaviour
    {
        #region Serialize Fields

        [Header("Normal Settings")]
        [SerializeField] private float normalSpeed;

        [Header("Infected Settings")]
        [SerializeField] private float infectedSpeed;
        [SerializeField] private float possibilityOfBecomingInfected;
        [SerializeField] private Color infectedColor;
        [SerializeField] private Renderer[] meshRenderers;
        [SerializeField] private float infectedLine;

        [Header("Health Settings")]
        [SerializeField] private float maxHealth;
        [SerializeField] private GameObject capsule;

        #endregion

        #region Private Fields

        private SplineFollower _follower;
        private bool _check;
        private float _currentHealth;

        #endregion

        #region Public Fields

        public bool Infected { get; private set; }

        #endregion

        #region Unity Functions

        private void Awake()
        {
            _follower = GetComponent<SplineFollower>();
            _follower.followSpeed = 0;
            _currentHealth = maxHealth;

            LevelManager.onLevelStart += OnLevelStart;
            LevelManager.onLevelComplete += OnLevelComplete;
            LevelManager.onLevelFail += OnLevelFail;
        }

        private void OnDestroy()
        {
            LevelManager.onLevelStart -= OnLevelStart;
            LevelManager.onLevelComplete -= OnLevelComplete;
            LevelManager.onLevelFail -= OnLevelFail;
        }

        #endregion

        #region Public Functions

        public void CheckInfected()
        {
            if (_check || Infected) return;

            int randomCheck = Random.Range(0, 100);
            if (randomCheck <= possibilityOfBecomingInfected)
            {
                ActiveInfected();
            }

            _check = true;
        }

        public void ActiveInfected()
        {
            if (Infected) return;

            _follower.followSpeed = infectedSpeed;
            _follower.direction = Spline.Direction.Forward;

            foreach (Renderer renderer in meshRenderers)
            {
                renderer.material.color = infectedColor;
            }

            StartCoroutine(ChangeLine(infectedLine));

            Infected = true;
        }

        public void TakeDamage(float value)
        {
            _currentHealth -= value;
            if (_currentHealth <= 0)
            {
                Capsulitis();
            }
        }

        public void Capsulitis()
        {
            _follower.followSpeed = 0;
            capsule.SetActive(true);
            Destroy(this);
        }

        #endregion

        #region Private Functions

        private IEnumerator ChangeLine(float value)
        {
            float line = Random.Range(-value, value);
            while (!Mathf.Approximately(_follower.motion.offset.x, line))
            {
                _follower.motion.offset = Vector2.MoveTowards(_follower.motion.offset,
                    new Vector2(line, _follower.motion.offset.y), 5 * Time.deltaTime);
                yield return null;
            }
        }

        #endregion

        #region Events

        private void OnLevelStart(Level level)
        {
            _follower.followSpeed = normalSpeed;
        }

        private void OnLevelComplete(Level level)
        {
            _follower.followSpeed = 0;
        }

        private void OnLevelFail(Level level)
        {
            _follower.followSpeed = 0;
        }

        #endregion
    }
}
