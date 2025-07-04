using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Advanced
{
    public class DistanceFromMainCamera : MonoBehaviour
    {
        private void Update() => CheckDistance();
        private void OnEnable() => CheckDistance(true);

        private void CheckDistance(bool isForceInvoke = true)
        {
            float distance = Vector3.Distance(MainCamera.transform.position, transform.position);

            bool isOverThreshold = distance >= distanceThreshold;

            // 與之前不同時才Invoke事件
            if (_currentIsOverThreshold != isOverThreshold || isForceInvoke)
            {
                _currentIsOverThreshold = isOverThreshold;

                if (_currentIsOverThreshold) invokeOnOverThreshold.Invoke();
                else invokeOnLessThreshold.Invoke();

                invokeOnThreshold?.Invoke(_currentIsOverThreshold);
            }
        }

        #region Variables

        [Header("與Camera之間的距離Threshold")] [SerializeField]
        private float distanceThreshold = 1f;

        [Foldout("[Event] - 距離是否大於Threshold")] public UnityEvent<bool> invokeOnThreshold;

        [Foldout("[Event] - 距離大於Threshold時Invoke")]
        public UnityEvent invokeOnOverThreshold;

        [Foldout("[Event] - 距離小於Threshold時Invoke")]
        public UnityEvent invokeOnLessThreshold;

        /// 目前是否超過Threshold
        private bool _currentIsOverThreshold = false;

        private Camera MainCamera => _mainCamera ??= Camera.main;
        [NonSerialized] private Camera _mainCamera;

        #endregion
    }
}