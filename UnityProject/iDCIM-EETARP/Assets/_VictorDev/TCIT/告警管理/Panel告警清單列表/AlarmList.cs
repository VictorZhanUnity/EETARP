using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace VictorDev.TCIT.AlarmModule
{
    public class AlarmList : MonoBehaviour
    {
        public UnityEvent<bool> onItemValueChanged;

        private void Start()
        {
            UpdateUI();
        }


        private void UpdateUI()
        {
            List<AlarmListItem> itemList = GetComponentsInChildren<AlarmListItem>().ToList();
            itemList.ForEach(item =>
            {
                item.toggleGroup = ToggleGroupInstance;
                item.onValueChanged.AddListener(OnItemValueChangedHandler);
            });
        }

        private void OnItemValueChangedHandler(bool isOn)
        {
            onItemValueChanged?.Invoke(isOn);
        }

        [Foldout("[設定]")] [SerializeField] private AlarmListItem listItemPrefab;

        private ToggleGroup ToggleGroupInstance => _toggleGroupInstance ??= GetComponent<ToggleGroup>();
        [NonSerialized] private ToggleGroup _toggleGroupInstance;
    }
}