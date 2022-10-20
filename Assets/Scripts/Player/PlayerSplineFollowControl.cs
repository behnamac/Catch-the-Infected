using Controllers;
using Dreamteck.Splines;
using Levels;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(SplineFollower))]
    public class PlayerSplineFollowControl : AbstractPlayerMoveController
    {
        #region SERIALIZE FIELDS


        #endregion

        #region PRIVATE FIELDS

        private SplineFollower _follower;
        private bool _isMove;
        private bool _isHorizontalMoveLock;
        private float _mouseXStartPosition;
        private float _swipeDelta;

        #endregion

        #region PRIVATE METHODS

        private void SetSpline()
        {
            _follower.spline = FindObjectOfType<SplineComputer>();
            _follower.followSpeed = forwardSpeed;
            _follower.follow = false;
        }

        #endregion

        #region PUBLIC METHODS

        public override void HorizontalMoveControl()
        {
            if (_isHorizontalMoveLock) return;

            // MOUSE DOWN
            if (Input.GetMouseButtonDown(0)) _mouseXStartPosition = Input.mousePosition.x;

            // MOUSE ON PRESS
            if (Input.GetMouseButton(0))
            {
                _swipeDelta = Input.mousePosition.x - _mouseXStartPosition;
                _mouseXStartPosition = Input.mousePosition.x;
            }

            // MOUSE UP
            if (Input.GetMouseButtonUp(0)) _swipeDelta = 0;

            _follower.motion.offset = HorizontalPosition(_follower.motion.offset, _swipeDelta);
        }

        public override void StartRun()
        {
            _isMove = true;
            _follower.follow = true;
        }

        public override void StopRun()
        {
            _isMove = false;
            _follower.follow = false;
        }

        public override void StopHorizontalControl(bool controlIsLock = true) => _isHorizontalMoveLock = controlIsLock;

        #endregion

        #region CUSTOM EVENT METHODS

        private void OnLevelLoad(Level levelData) => SetSpline();

        private void OnLevelStart(Level levelData) => StartRun();

        private void OnLevelFail(Level levelData)
        {
            StopRun();
            StopHorizontalControl();
        }

        private void OnLevelStageComplete(Level levelData, int stageIndex)
        {
            StopHorizontalControl();
        }

        private void OnLevelComplete(Level levelData)
        {
            StopRun();
            StopHorizontalControl();
        }

        #endregion

        #region UNITY EVENT METHODS

        protected override void OnComponentAwake()
        {
            base.OnComponentAwake();
            _follower = GetComponent<SplineFollower>();
            LevelManager.onLevelLoad += OnLevelLoad;
        }

        protected override void OnComponentStart()
        {
            base.OnComponentAwake();
            LevelManager.onLevelStart += OnLevelStart;
            LevelManager.onLevelStageComplete += OnLevelStageComplete;
            LevelManager.onLevelFail += OnLevelFail;
            LevelManager.onLevelComplete += OnLevelComplete;
        }

        protected override void OnComponentUpdate()
        {
            if (!_isMove) return;
            HorizontalMoveControl();
        }

        protected override void OnComponentDestroy()
        {
            LevelManager.onLevelLoad -= OnLevelLoad;
            LevelManager.onLevelStart -= OnLevelStart;
            LevelManager.onLevelStageComplete -= OnLevelStageComplete;
            LevelManager.onLevelFail -= OnLevelFail;
            LevelManager.onLevelComplete -= OnLevelComplete;
        }

        #endregion
    }
}