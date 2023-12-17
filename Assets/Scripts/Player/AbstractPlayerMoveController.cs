using UnityEngine;

namespace Player
{
    public abstract class AbstractPlayerMoveController : MonoBehaviour
    {
        [Header("MOVE")]
        public float forwardSpeed;
        public float horizontalSpeed;
        public float maximumHorizontalPosition;

        public abstract void HorizontalMoveControl();
        public abstract void StartRun();
        public abstract void StopRun();
        public abstract void StopHorizontalControl(bool controlIsLock = true);

        protected Vector3 HorizontalPosition(Vector3 targetPosition, float swipeDelta)
        {
            var xDirection = Time.deltaTime * swipeDelta * horizontalSpeed;
            var xPos = Mathf.Clamp(
                targetPosition.x + xDirection,
                -maximumHorizontalPosition,
                maximumHorizontalPosition);

            return new Vector3(xPos, targetPosition.y, targetPosition.z);
        }

        protected virtual void OnComponentAwake() { }
        protected virtual void OnComponentStart() { }
        protected virtual void OnComponentUpdate() { }
        protected virtual void OnComponentDestroy() { }

        private void Awake() => OnComponentAwake();
        private void Start() => OnComponentStart();
        private void Update() => OnComponentUpdate();
        private void OnDestroy() => OnComponentDestroy();
    }
}
