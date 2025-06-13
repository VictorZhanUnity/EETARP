using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VictorDev.Common;

public class Landmark : MonoBehaviour
{
    [SerializeField] private string label = "冷通道A-前";
    public string Label => label;
    [Foldout("[Event] - Toggle點選時Invoke")]
    public UnityEvent<bool> onToggleValueChanged = new();
    
    public void SetLabel(string value)
    {
        label = value;
        OnValidate();
    }

    public void SetTarget(Transform target) => PosTo2DPoint.SetTargetObject(target);
    /// 設定可視距離
    public void SetVisibleRange(float range) => PosTo2DPoint.SetVisibleRange(range);

    public ToggleGroup toggleGroup
    {
        set => ToggleInstance.group = value;
    }
    
    #region Initialized

    private void OnEnable() => ToggleInstance.onValueChanged.AddListener(OnToggleValueChangedHandler);
    private void OnDisable()
    {
        ToggleInstance.isOn = false;
        ToggleInstance.onValueChanged.RemoveListener(OnToggleValueChangedHandler);
    }

    private void OnToggleValueChangedHandler(bool isOn) => onToggleValueChanged?.Invoke(isOn);

    private void OnValidate()
    {
        label = label.Trim();
        
        TxtLabel.SetText(label);
        TxtLabelSelected.SetText(label);
    }
    #endregion
    
    
    private Toggle ToggleInstance => _toggle ??= transform.Find("Container").GetComponent<Toggle>();
    [NonSerialized] private Toggle _toggle;
    
    private TextMeshProUGUI TxtLabel => _txtLabel ??= transform.Find("Container/TxtLabel").GetComponent<TextMeshProUGUI>();
    private TextMeshProUGUI TxtLabelSelected => _txtLabelSelected ??= transform.Find("Container/Selected/TxtLabel").GetComponent<TextMeshProUGUI>();
    [NonSerialized] private TextMeshProUGUI _txtLabel, _txtLabelSelected;
    
    private PositionTo2DPoint PosTo2DPoint => _positionTo2DPoint ??= GetComponent<PositionTo2DPoint>();
    [NonSerialized] private PositionTo2DPoint _positionTo2DPoint;
}
