
using System;
using Unity.VisualScripting;
using UnityEngine;
using VictorDev.MaterialUtils;
using VictorDev.ObjectUtils;

public class CctvManager : MonoBehaviour
{
    private ModelFinder ModelFinderInstance => _modelFinder ??= GetComponent<ModelFinder>();
    [NonSerialized] private ModelFinder _modelFinder;
}