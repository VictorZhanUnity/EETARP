using System;
using NaughtyAttributes;
using UnityEngine;

namespace VictorDev.Common
{
    /// 依指定3D物件之座標，轉成螢幕2D座標進行定位
    /// + 因為跟攝影機視角有關，所以不能直接加offset偏移值於座標上
    public class PositionTo2DPoint : MonoBehaviour
    {
        void Update()
        {
            Vector3 targetPos = target3DObject.position;

            // 是否在可視範圍內
            if (isVisibleInRange)
            {
                Container.SetActive(Vector3.Distance(targetPos, MainCamera.transform.position) <= visibleRange);
            }
            if (Container.activeSelf == false) return;
            
            if (isCenterPivot && target3DObject.TryGetComponent(out MeshRenderer meshRenderer))
            {
                Bounds bounds = meshRenderer.bounds;
                targetPos = bounds.center;
            }
            targetPos += offsetPos;

            // 1. 轉換 3D 世界座標到螢幕座標
            Vector3 screenPos = MainCamera.WorldToScreenPoint(targetPos);

            if (screenPos.z < 0) return;
            
            // 2. 取得 Canvas 尺寸
            float scaleX = CanvasRect.sizeDelta.x / Screen.width;
            float scaleY = CanvasRect.sizeDelta.y / Screen.height;

            // 3. 計算相對於 Canvas 的座標
            Vector2 localPos = new Vector2(
                (screenPos.x - (Screen.width * 0.5f)) * scaleX,
                (screenPos.y - (Screen.height * 0.5f)) * scaleY
            );

            // 4. 設定 UI 位置
            RectTrans.anchoredPosition = localPos ;
        }
        
        /// 設定目標物件
        public void SetTargetObject(Transform target) => target3DObject = target;
        
        /// 設定可視距離
        public void SetVisibleRange(float range) => visibleRange = range;

        #region Components
        [Header(">>> 目標3D物件")] [SerializeField] Transform target3DObject;
        
        [Foldout("位置設定")]
        [Header(">>> 位置偏移設定")] 
        [SerializeField] Vector3 offsetPos = Vector3.up * 0.1f;
        [Foldout("位置設定")]
        [Header(">>> 是否於目標模型置中(目標物件需有MeshRenderer)")] 
        [SerializeField] bool isCenterPivot = true;
        
        [Foldout("顯示設定")]
        [SerializeField] float visibleRange = 20f;
        [Foldout("顯示設定")]
        [SerializeField] bool  isVisibleInRange = true;
        
        // 指定主要攝影機
        private Camera MainCamera =>_mainCamera ??= Camera.main;
        [NonSerialized]
        private Camera _mainCamera; // 指定主要攝影機 
        /// 與Main攝影機之距離
        public float DistanceFromCamera => Vector3.Distance(MainCamera.transform.position, target3DObject.position);
        private RectTransform RectTrans => _rectTrans ??= GetComponent<RectTransform>();
        [NonSerialized]
        private RectTransform _rectTrans; // UI 按鈕 (或任何 UI 元件)
        private RectTransform CanvasRect =>_canvasRect ??= GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        [NonSerialized]
        private RectTransform _canvasRect; // UI 按鈕 (或任何 UI 元件)

        private GameObject Container => _container ??= transform.Find("Container").gameObject;
        [NonSerialized] private GameObject _container;
        #endregion
    }
}