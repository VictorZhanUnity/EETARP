using UnityEngine;
using VictorDev.Common;

public class LandmarkCctv : MonoBehaviour
{
    #region Initialized
    private void OnEnable() => LandmarkInstance.onToggleValueChanged.AddListener(OnValueChangedHandler);
    private void OnDisable() => LandmarkInstance.onToggleValueChanged.RemoveListener(OnValueChangedHandler);
    private void OnValueChangedHandler(bool isOn)
    {
        Pt2DPoint.Target3DObject.GetChild(0)?.gameObject.SetActive(isOn);
    }
    #endregion

    #region Variables
    private Landmark LandmarkInstance => _landmark ??= GetComponent<Landmark>();
    private Landmark _landmark;

    private PositionTo2DPoint Pt2DPoint => _positionTo2DPoint ??= GetComponent<PositionTo2DPoint>();
    private PositionTo2DPoint _positionTo2DPoint;
    #endregion
}