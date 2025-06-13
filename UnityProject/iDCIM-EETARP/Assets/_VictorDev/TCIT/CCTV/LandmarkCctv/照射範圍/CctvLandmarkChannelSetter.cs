using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using VictorDev.RTSP;

/// [Editor] - 設置CCTV Channel資料給各個Landmark
public class CctvLandmarkChannelSetter : MonoBehaviour
{
    public List<SoData_RTSP_Channel> channels = new();
    [SerializeField] private Transform landmarkContainer;

    [Button]
    private void SetRtspChannel()
    {
        List<LandmarkCctv> landmark = landmarkContainer.GetComponentsInChildren<LandmarkCctv>().ToList();

        var matchQuery = from a in landmark
            join b in channels on a.Label equals b.name
            select new { a, b };

        foreach (var match in matchQuery)
        {
            match.a.SetRtspChannel(match.b);
        }
    }

    [Button]
    private void RemoveRtspChannel()
    {
        List<LandmarkCctv> landmark = landmarkContainer.GetComponentsInChildren<LandmarkCctv>().ToList();
        landmark.ForEach(m => m.SetRtspChannel(null));
    }
}