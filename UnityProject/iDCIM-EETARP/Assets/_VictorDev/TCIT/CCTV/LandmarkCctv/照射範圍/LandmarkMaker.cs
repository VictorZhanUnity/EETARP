using System.Linq;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using VictorDev.Common;
using VictorDev.ObjectUtils;

public class LandmarkMaker : MonoBehaviour
{
    [SerializeField] Landmark landmarkPrefab;
    [SerializeField] Transform container;
    [SerializeField] ModelFinder modelFinder;
    [Foldout("可選")] [SerializeField] ToggleGroup toggleGroup;

    [Foldout("設定")] [SerializeField] private string labelHeader = "CCTV";
    [Foldout("設定")] [SerializeField] private float visibleRange = 20f;
    private int _counter = 0;


    [Button]
    private void AddLandmarks()
    {
        RemoveLandmarks();
        _counter = 0;
        modelFinder.FindedModels.ToList().ForEach(target =>
        {
            Landmark landmark = PrefabUtility.InstantiatePrefab(landmarkPrefab, container).GetComponent<Landmark>();
            landmark.SetTarget(target);

            //設置Label
            string[] splitString = target.name.Split(",");
            string label = (splitString.Length == 1) ? $"{labelHeader}-{++_counter}" : splitString[0].Trim();
            landmark.SetLabel(label);
            landmark.name = label;
            landmark.SetVisibleRange(visibleRange);
            if(toggleGroup != null) landmark.toggleGroup = toggleGroup;
        });
    }

    [Button]
    private void RemoveLandmarks()
    {
        ObjectHelper.DestoryObjectsOfContainer(container);
    }

    private void Awake() => gameObject.SetActive(false);
}