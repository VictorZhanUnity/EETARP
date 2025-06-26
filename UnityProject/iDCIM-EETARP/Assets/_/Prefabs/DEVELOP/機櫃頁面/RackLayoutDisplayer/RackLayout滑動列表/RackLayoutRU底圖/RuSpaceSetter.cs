using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VictorDev.Common;

namespace VictorDev.TCIT
{
    public class RuSpaceSetter : MonoBehaviour
    {
        [SerializeField] private GameObject ruSpaceListItem;
        [Foldout("[設定]")] [SerializeField] private int numOfRuSpace = 42;

        [Button]
        private void BuildRuSpaceListItems()
        {
            ClearListItems();

            for (int i = 1; i <= numOfRuSpace; i++)
            {
                var item = PrefabUtility.InstantiatePrefab(ruSpaceListItem, transform).GameObject();
                item.transform.Find("TxtIndexFront").GetComponent<TextMeshProUGUI>().SetText(i.ToString());
                item.transform.Find("TxtIndexBack").GetComponent<TextMeshProUGUI>().SetText(i.ToString());
                item.name = $"RuSpaceListItem - {i}";
            }
        }

        [Button]
        private void ClearListItems() => ObjectHelper.DestoryObjectsOfContainer(transform);

        private void Start() => OnValidate();
        private void OnValidate() => enabled = false;
    }
}