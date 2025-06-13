using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.RTSP;

public class Demo_RTSP : MonoBehaviour
{
    public RtspScreenOLD rtspScreenOldPrefab;
    public Transform gridContainer;
    public List<Button> buttonList = new List<Button>();

    private string url = "rtsp://admin:sks12345@ibms.sks.com.tw:554/{0}/1";
    private void Start()
    {
        buttonList.ForEach(btn => btn.onClick.AddListener(() => OnClickBtnHandler(btn)));
    }

    private void OnClickBtnHandler(Button btn)
    {
        RtspScreenOLD player = Instantiate(rtspScreenOldPrefab, gridContainer);
        player.Play(string.Format(url, btn.name.Split("-")[1].Trim()));
    }
}
