using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.DemoUtils
{
    public class ValueAutoJumperManager : MonoBehaviour
    {
        [Foldout("底下所有的ValueAutoJumper")]
        [SerializeField] private List<MonoBehaviour> valueAutoJumpers;

        [Button]
        private void FindJumperInChildrent()
        {
            CheckParent();
            valueAutoJumpers = ObjectHelper.FindChildrenByClass<IAutoJumper>(parentTransform);
        }

        private void CheckParent()
        {
            if(parentTransform == null) parentTransform = transform;
        }
        private void Start() => CheckParent();

        /// 手動更新所有的AutoJumper
        [Button]
        public void UpdateJumpersValue()
        {
            valueAutoJumpers.ForEach(target =>
            {
                if (target.gameObject.activeInHierarchy)
                {
                    if (target is IAutoJumper autoJumper)
                    {
                        autoJumper.ValueUpdateByManual();
                    }
                }
            });
        }
        
        [Foldout("[設定] - 尋找Jumper的父物件對像")]
        [SerializeField] private Transform parentTransform;
    }
    
    public interface IAutoJumper
    {
        /// 手動設置Value
        [Button]
        void ValueUpdateByManual();
    }
}