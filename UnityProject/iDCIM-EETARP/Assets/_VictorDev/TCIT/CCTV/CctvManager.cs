using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.RTSP;

public class CctvManager : MonoBehaviour
{
    public void OnCloseFullScreenPlayer()
    {
        _selectedRtspScreen.RemoveRenderingTarget(FullScreenPlayerRawImage);
    }

    private void OnClickInfoButton(Transform target)
    {
    }

    private void OnClickLocateButton(Transform target)
    {
    }

    private void OnClickScaleButton(RtspScreen rtspScreen)
    {
        cctvFullScreenPlayer.gameObject.SetActive(true);
        _selectedRtspScreen = rtspScreen;
        _selectedRtspScreen.AddRenderingTarget(FullScreenPlayerRawImage);
    }

    #region Initialized

    private void OnEnable()
    {
        LandmarkCctvs.ForEach(landmark =>
        {
            landmark.onClickInfoButton.AddListener(OnClickInfoButton);
            landmark.onClickLocateEvent.AddListener(OnClickLocateButton);
            landmark.onClickScaleEvent.AddListener(OnClickScaleButton);
        });
    }

    private void OnDisable()
    {
        LandmarkCctvs.ForEach(landmark =>
        {
            landmark.onClickInfoButton.RemoveListener(OnClickInfoButton);
            landmark.onClickLocateEvent.RemoveListener(OnClickLocateButton);
            landmark.onClickScaleEvent.RemoveListener(OnClickScaleButton);
        });
    }

    #endregion

    [SerializeField] private Transform landmarkContainer;
    [SerializeField] private GameObject cctvFullScreenPlayer;

    [NonSerialized] private RtspScreen _selectedRtspScreen;

    private GameObject FullScreenPlayerRawImage => _fullScreenPlayerRawImage ??=
        cctvFullScreenPlayer.GetComponentInChildren<RawImage>().gameObject;

    private GameObject _fullScreenPlayerRawImage;

    private List<LandmarkCctv> LandmarkCctvs =>
        _landmarkCctvs ??= landmarkContainer.GetComponentsInChildren<LandmarkCctv>().ToList();

    private List<LandmarkCctv> _landmarkCctvs;
}