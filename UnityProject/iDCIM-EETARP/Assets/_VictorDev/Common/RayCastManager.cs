using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace VictorDev.Common
{
    public class RayCastManager : MonoBehaviour
    {
        [Foldout("[Event] - Invoke RayCast點擊的物件")]
        public UnityEvent<GameObject> onRaycastHitObject = new();
        [Foldout("[Event] - Invoke RayCast射線經過的物件列表")]
        public UnityEvent<List<GameObject>> onRaycastHitObjects = new();
        
        /// 從畫面中心發射射線，取得命中物件
        public List<GameObject> GetRaycastHitObjectsFromCenter()
        {
            Ray ray = MainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // 中心射線
            return GetRaycastHitObjects(ray);
        }

        /// 從指定螢幕座標（如滑鼠位置）發射射線
        public List<GameObject> GetRaycastHitObjectsFromScreen(Vector2 screenPosition)
        {
            Ray ray = MainCamera.ScreenPointToRay(screenPosition);
            return GetRaycastHitObjects(ray);
        }

        /// 內部函式：執行實際 Raycast 並回傳排序後的物件列表
        private List<GameObject> GetRaycastHitObjects(Ray ray)
        {
            DebugDrawLineCheck(ray);
            
            RaycastHit[] hits = Physics.RaycastAll(ray, rayDistance, targetLayerMask);
            // ✔ 這裡其實就是依攝影機距離排序了（因為 ray.origin == camera.position）
            Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            return hits.ToList().Select(hit=>hit.collider.gameObject ).ToList();
        }

        private void Update()
        {
            if (isActivated && Input.GetMouseButtonDown(0) && EventHelper.IsPointerOverUI()==false)
            {
                List<GameObject> hitObjects = GetRaycastHitObjectsFromScreen(Input.mousePosition);
                if (hitObjects.Count > 0)
                {
                    if(isShowDebug) Debug.Log($"onRaycastHitResult: 共{hitObjects.Count}筆", this, EmojiEnum.Target);
                    onRaycastHitObject?.Invoke(hitObjects.First());
                    onRaycastHitObjects?.Invoke(hitObjects);
                }
            }
            DebugDrawLineCheck();
        }

        /// Debug畫線
        private void DebugDrawLineCheck(Ray? ray = null)
        {
            if (isDebugDrawLine)
            {
                if (ray == null) Debug.DrawRay(MainCamera.ScreenPointToRay(Input.mousePosition), rayDistance);
                else Debug.DrawRay(ray.Value, rayDistance, Color.red, 3f);
            }
        }
        
        #region Variables
        [Foldout("[設定]")] [SerializeField] private float rayDistance = 100f;
        [Foldout("[設定]")] [SerializeField] private LayerMask targetLayerMask = ~0;
        [Foldout("[設定]")] [SerializeField] private bool isActivated = true;
        [Foldout("[設定]")] [SerializeField] private bool isDebugDrawLine = true;
        [Foldout("[設定]")] [SerializeField] private bool isShowDebug = false;
        private Camera MainCamera => _mainCamera ??= Camera.main;
        [NonSerialized] private Camera _mainCamera;
        
        
        #endregion
    }
}