using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance { get; private set; }


        #region SERIALIZE FIELDS

        [SerializeField] private Transform target;
        [SerializeField] private float followSpeed = 0.1f;
        
        #endregion

        #region PRIVATE METHODS

        private void Initialize()
        {
            
        }
        
        private void SmoothFollow()
        {
            var transform1 = transform;
            Transform thisTransform = transform1;
            thisTransform.position =
                Vector3.Lerp(transform1.position, target.position, followSpeed * Time.deltaTime);

            var eulerAngles = thisTransform.eulerAngles;
            eulerAngles.y = target.eulerAngles.y;
            thisTransform.eulerAngles = eulerAngles;
        }

        #endregion

        #region UNITY EVENT METHODS

        private void Awake()
        {
            if (instance == null) instance = this;
        }

        private void Start() => Initialize();

        private void LateUpdate() => SmoothFollow();

        #endregion
    }
}