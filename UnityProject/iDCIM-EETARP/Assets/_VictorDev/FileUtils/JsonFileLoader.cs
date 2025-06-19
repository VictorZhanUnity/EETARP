using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using Debug = VictorDev.Common.Debug;

namespace VictorDev.FileUtils
{
    /// [Editor] - 讀取StreamingAssets資料夾裡的機櫃Json檔
    public class JsonFileLoader : MonoBehaviour
    {
        /// 建立StreamingAssets資料夾
        [Button]
        private void CreateStreamingAssetsFolder()
        {
            Debug.Log("CreateStreamingAssetsFolder...", this, EmojiEnum.Folder);
            StreamAssetsFileLoader.CheckStreamingAssetsFolder();
            Debug.Log("CreateStreamingAssetsFolder... OK!", this, EmojiEnum.Done);
        }

        /// 讀取Json檔案
        [Button]
        public void LoadJsonFile(string path = "")
        {
            _jsonData = string.Empty;
            if (string.IsNullOrEmpty(filePath)) filePath = path;
            filePath = filePath.Trim();
            Debug.Log("LoadJsonFile...", this, EmojiEnum.Download);
            StreamAssetsFileLoader.LoadJsonFile(filePath, OnSuccessHandler);
        }

        private void OnSuccessHandler(string data)
        {
            _jsonData = data;
            Debug.Log($"LoadJsonFile... OK!\n{data}", this, EmojiEnum.Done);
            invokeOnSuccess?.Invoke(data);
        }

        private void Start()
        {
            if (isActivatedInStart) LoadJsonFile();
        }

        #region Variables

        [Header("[Event] - 成功時Invoke")] public UnityEvent<string> invokeOnSuccess;

        [Foldout("[設定]")] [SerializeField] bool isActivatedInStart = true;
        [Foldout("[設定]")] [SerializeField] string filePath = "jsonfile";

        [NonSerialized] private string _jsonData;

        #endregion
    }
}