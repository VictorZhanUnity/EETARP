using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using VictorDev.Common;
using VictorDev.RTSP;

public class LandmarkCctv : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Transform> onClickInfoButton = new();
    [HideInInspector] public UnityEvent<RtspScreen> onClickScaleEvent = new();
    [HideInInspector] public UnityEvent<Transform> onClickLocateEvent = new();

    [SerializeField] private SoData_RTSP_Channel rtspChannel;
    public string Label => LandmarkInstance.Label;

    public void SetRtspChannel(SoData_RTSP_Channel channel)
    {
        rtspChannel = channel;
        #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        #endif
    }

    public void OnClickInfoButton() => onClickInfoButton?.Invoke(Pt2DPoint.Target3DObject);
    public void OnClickScaleButton(RtspScreen rtspScreen) => onClickScaleEvent?.Invoke(rtspScreen);
    public void OnClickLocateButton() => onClickLocateEvent?.Invoke(Pt2DPoint.Target3DObject);

    private void OnValueChangedHandler(bool isOn)
    {
        // 開啟/關閉 CCTV照射範圍
        Pt2DPoint.Target3DObject.GetChild(0)?.gameObject.SetActive(isOn);
        // 是否進行播放
        if (isOn) RtspScreenInstance.Play(rtspChannel.RTSP_URL);
        else RtspScreenInstance.Stop();
    }
    
    #region Initialized
    private void OnEnable() => LandmarkInstance.onToggleValueChanged.AddListener(OnValueChangedHandler);
    private void OnDisable() => LandmarkInstance.onToggleValueChanged.RemoveListener(OnValueChangedHandler);

    #endregion

    #region Variables

    private Landmark LandmarkInstance => _landmark ??= GetComponent<Landmark>();
    [NonSerialized] private Landmark _landmark;

    private PositionTo2DPoint Pt2DPoint => _positionTo2DPoint ??= GetComponent<PositionTo2DPoint>();
    [NonSerialized] private PositionTo2DPoint _positionTo2DPoint;

    private RtspScreen RtspScreenInstance =>
        _rtspScreen ??= transform.Find("CCTVDisplayer/RtspScreen").GetComponent<RtspScreen>();

    [NonSerialized] private RtspScreen _rtspScreen;

    #endregion
}