using System;
using System.Collections;
using System.Linq;
using NaughtyAttributes;
using UMP;
using UnityEngine;

namespace VictorDev.RTSP
{
    /// [RTSP] 單純播放畫面
    [RequireComponent(typeof(UniversalMediaPlayer))]
    public class RtspScreen : MonoBehaviour
    {
        [TextArea(0, 3)] [SerializeField] private string rtspUrl;

        //   [Foldout("[設定] - Error時每隔N秒再次連線播放")] [SerializeField] private float intervalSec = 3f;

        private UniversalMediaPlayer Ump => _ump ??= GetComponent<UniversalMediaPlayer>();
        [NonSerialized] private UniversalMediaPlayer _ump;

        private Coroutine _coroutine;

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
            //while (Ump.RenderingObjects != null) yield return null;
        }

        [Button]
        public void Stop() => Ump.Stop();

        /// 新增渲染對像到RTSP渲染器內, 只適用RawImage與MeshRenderer
        /// <para>+ isReset：是否先清空RTSP渲染器對像清單</para>
        public void AddRenderingTarget(GameObject target, bool isReset = false)
        {
            if (isReset) Array.Clear(Ump.RenderingObjects, 0, Ump.RenderingObjects.Length);

            Ump.RenderingObjects = Ump.RenderingObjects
                .Concat(new[] { target }) // 合併新的 GameObject
                .Distinct() // 去重，避免重複項目
                .ToArray();
        }

        /// 從RTSP渲染器內移除渲染對像
        public void RemoveRenderingTarget(GameObject target)
            => Ump.RenderingObjects = Ump.RenderingObjects.Where(obj => obj != target).ToArray();

        #region Initialized

        private void OnEnable() => Ump.AddEncounteredErrorEvent(OnEncounteredErrorEventHandler);
        private void OnEncounteredErrorEventHandler() => Play();

        private void OnDisable()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            Ump.RemoveEncounteredErrorEvent(OnEncounteredErrorEventHandler);
            Stop();
        }

        #endregion
    }
}