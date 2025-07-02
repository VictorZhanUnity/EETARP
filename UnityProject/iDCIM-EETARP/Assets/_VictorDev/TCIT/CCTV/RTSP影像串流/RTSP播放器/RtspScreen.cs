using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
#if !UNITY_WEBGL
using UMP;
#endif

namespace VictorDev.RTSP
{
    /// [RTSP] 單純播放畫面
    public class RtspScreen : MonoBehaviour
    {
        [TextArea(0, 3)] [SerializeField] private string rtspUrl;

        private UmpMediator Ump;

        private Coroutine _coroutine;

        private void Awake() => Ump = new UmpMediator(transform);


        [Button]
        private void Play() => Play(rtspUrl.Trim());

        public void Play(string url)
        {
            rtspUrl = url;

            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(CheckUmpPluginInit());
        }

        IEnumerator CheckUmpPluginInit()
        {
            Ump.Path = rtspUrl;
            Ump.Play();
            yield return null;
        }

        [Button]
        public void Stop() => Ump.Stop();

        /// 新增渲染對像到RTSP渲染器內, 只適用RawImage與MeshRenderer
        /// <para>+ isReset：是否先清空RTSP渲染器對像清單</para>
        public void AddRenderingTarget(GameObject target, bool isReset = false)
        {
            Ump.AddRenderingTarget(target, isReset);
        }

        /// 從RTSP渲染器內移除渲染對像
        public void RemoveRenderingTarget(GameObject target)
            => Ump.RemoveRenderingTarget(target);

        #region Initialized

        private void OnEnable() => Ump.OnEnable();

        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            Ump.OnDisable();
            Stop();
        }

        #endregion

        
        /// UMP仲介，以方便拔除UMP套件功能
        public class UmpMediator
        {
            private readonly Transform _parent;
            public UmpMediator(Transform parent) => _parent = parent;

            public string Path
            {
                set
                {
#if !UNITY_WEBGL
                    Ump.Path = value;
#endif
                }
            }

            public void Play()
            {
#if !UNITY_WEBGL
                Ump.Play();
#endif
            }

            public void Stop()
            {
#if !UNITY_WEBGL
                Ump.Stop();
#endif
            }

            /// 新增渲染對像到RTSP渲染器內, 只適用RawImage與MeshRenderer
            /// <para>+ isReset：是否先清空RTSP渲染器對像清單</para>
            public void AddRenderingTarget(GameObject target, bool isReset = false)
            {
#if !UNITY_WEBGL
                if (isReset) Array.Clear(Ump.RenderingObjects, 0, Ump.RenderingObjects.Length);

                Ump.RenderingObjects = Ump.RenderingObjects
                    .Concat(new[] { target }) // 合併新的 GameObject
                    .Distinct() // 去重，避免重複項目
                    .ToArray();
#endif
            }


            /// 從RTSP渲染器內移除渲染對像
            public void RemoveRenderingTarget(GameObject target)
            {
#if !UNITY_WEBGL
                Ump.RenderingObjects = Ump.RenderingObjects.Where(obj => obj != target).ToArray();
#endif
            }


            public void OnEnable()
            {
#if !UNITY_WEBGL
                Ump.AddEncounteredErrorEvent(OnEncounteredErrorEventHandler);
#endif
            }

            private void OnEncounteredErrorEventHandler() => Play();

            public void OnDisable()
            {
#if !UNITY_WEBGL
                Ump.RemoveEncounteredErrorEvent(OnEncounteredErrorEventHandler);
#endif
            }

#if !UNITY_WEBGL
            public UniversalMediaPlayer Ump => _ump ??= _parent.GetComponent<UniversalMediaPlayer>();
            private UniversalMediaPlayer _ump;
#endif
        }
    }
}