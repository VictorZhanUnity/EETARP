using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace VictorDev.Common
{
    /// 跟隨鼠標移動(2D)
    public class FollowMouse2D : MonoBehaviour
    {
        void Update()
        {
            // 取得滑鼠在世界座標的位置（假設攝影機是正交2D或主攝影機在2D模式）
            Vector3 targetPos = Input.mousePosition;
            targetPos.z = transform.position.z;
            targetPos += offsetPos;

            // 終止先前Tween
            if (_moveTween != null && _moveTween.IsActive())
            {
                _moveTween.Kill();
            }

            // 新增Tween平滑移動到目標位置（Ease可調）
            _moveTween = transform.DOMove(targetPos, duration).SetEase(Ease.OutQuad);
        }

        [Foldout("[位置設定]")] [SerializeField] private Vector3 offsetPos = Vector3.zero;

        [Foldout("[跟隨Ease設定]")] [SerializeField]
        private float duration = 0f;

        [Foldout("[跟隨Ease設定]")] [SerializeField]
        private Ease ease = Ease.OutQuad;

        private Tween _moveTween;
    }
}