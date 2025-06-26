using System;
using TMPro;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.TCIT
{
    public class CoBieListItem : MonoBehaviour
    {
        /// 欄位名稱
        public string ColumnName
        {
            set
            {
                value = value.Split("_")[1].Trim();
                TxtColumnName.SetText(value.ToCapitalizeFirstLetter());
            }
        }

        /// 值
        public string Value
        {
            set
            {
                value = value.Trim();
                TxtValue.SetText(string.IsNullOrEmpty(value) ? "" : value.ToCapitalizeFirstLetter());
                TxtValue.gameObject.SetActive(string.IsNullOrEmpty(value) == false);
                TxtEmpty.SetActive(string.IsNullOrEmpty(value));
            }
        }

        private TextMeshProUGUI TxtColumnName =>
            _txtColumnName ??= transform.Find("TxtColumnName").GetComponent<TextMeshProUGUI>();
        private TextMeshProUGUI TxtValue => _txtValue ??= transform.Find("TxtValue").GetComponent<TextMeshProUGUI>();
        [NonSerialized] private TextMeshProUGUI _txtColumnName, _txtValue;

        private GameObject TxtEmpty => _txtEmpty ??= transform.Find("TxtEmpty").gameObject;
        [NonSerialized] private GameObject _txtEmpty;
    }
}