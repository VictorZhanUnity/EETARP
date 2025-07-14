using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VictorDev.Common;
using VictorDev.Revit;
using VictorDev.TCIT.StorageAssetUtils;
using Debug = VictorDev.Common.Debug;

public class UploadDeviceHandler : MonoBehaviour, IRevitModelDataExtended
{
    [Foldout("[Event] - 點擊機櫃U層時放置設備")]
    public UnityEvent<GameObject> onSetDeviceAsset = new();
    
    [Foldout("[Event] - 點擊機櫃U層時進行上架Event {庫存設備，目標機櫃，佔用U層數}")]
    public UnityEvent<DeviceModelDataExtended, RackModelDataExtended, List<int>, Transform> onUploadDevice = new();

    /// 接收目前所選擇的機櫃/設備 
    public void ReceiveData(RevitModelDataExtended revitModelDataExtended)
    {
        ReviteModelData = revitModelDataExtended;
        if (ReviteModelData is DeviceModelDataExtended deviceModelDataExtended)
        {
            CancelUploadDevice();
            DeviceModelDataInfo = deviceModelDataExtended;
            _selectedStockDevice = DeviceModelDataInfo;
            InitialDragDevice();
        }
    }

    /// 建立欲上架的設備模型
    private void InitialDragDevice()
    {
        _dragUploadDevice = Instantiate(_selectedStockDevice.Model, transform);
        _dragUploadDevice.gameObject.SetActive(false);

        //_selectedStockDevice.Model = _dragUploadDevice;
        
        LayerMaskHelper.SetGameObjectLayerToLayerMask(_dragUploadDevice.gameObject, uploadDeviceLayerMask);

        //依Mesh大小建立外框        
        GameObject bound = ObjectHelper.CreateBoundingCube(_dragUploadDevice.gameObject, matUploadDevice);
        GameObject border = ObjectHelper.CreateBoundingCube(_dragUploadDevice.gameObject, matUploadDeviceBorder);
        Destroy(bound.GetComponent<Collider>());
        Destroy(border.GetComponent<Collider>());
    }
    
    /// 取消選擇上架設備
    public void CancelUploadDevice(bool isUploadSuccess = false)
    {
        CurrentOccupyRackSpaceDisplayers?.ForEach(rackDisplay => rackDisplay.IsPinULocationVisible = false);

        _isPutInRackSpacer = false;
        _selectedStockDevice = null;
        _currentMouseOverRackSpacer = null;

        if (_dragUploadDevice != null)
        {
            if(isUploadSuccess == false) Destroy(_dragUploadDevice.gameObject);
            _dragUploadDevice = null;
        }
    }

    private void Update()
    {
        if (_isPutInRackSpacer == false) DragUploadDevice();

        ClickToUploadDevice();
    }

    /// 點擊在RackSpacer上時放置設備於RackSpacer內
    private void ClickToUploadDevice()
    {
        if (_currentMouseOverRackSpacer != null && Mouse.current.leftButton.isPressed)
        {
            _isPutInRackSpacer = true; //取消上架模型跟隨鼠標
           
            //常駐顯示RackSpacer的U層數
            CurrentOccupyRackSpaceDisplayers.ForEach(rackDisplay => rackDisplay.IsPinULocationVisible = true);

            //進行動畫
            _dragUploadDevice.GetChild(0).GetComponent<MeshRenderer>().material
                .DOColor(new Color(0, 1, 1, 0 / 255f), 0.3f).SetEase(Ease.OutQuad);
            _dragUploadDevice.DOLocalMoveZ(_dragUploadDevice.transform.localPosition.z, 0.3f).SetEase(Ease.OutQuad);

            //點擊時上架設備Invoke事件
            onUploadDevice?.Invoke(_selectedStockDevice, _currentMouseOverRackSpacer.RackData
                , CurrentOccupyRackSpaceDisplayers.Select(displayer => displayer.ULevel).ToList(), _dragUploadDevice);
            
            onSetDeviceAsset?.Invoke(_dragUploadDevice.gameObject);
        }
    }

    /// 移動鼠標時設定上架庫存設備的位置
    private void DragUploadDevice()
    {
        if (_selectedStockDevice == null || _dragUploadDevice == null) return;

        Ray ray = MainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            //一般情形下
            Transform parentTransform = transform;
            Color matColor = new Color(1, 0, 0, 100 / 255f);
            Vector3 size = _dragUploadDevice.GetComponent<MeshFilter>().mesh.bounds.size;
            size.y = 0;
            Vector3 localPosition = hit.point - size * 0.5f;

            // 有MouseOver在RackSpacer上
            if (hit.transform.TryGetComponent(out RackUnitDisplay targetRackSpacer)
                && EventSystem.current.IsPointerOverGameObject() == false)
            {
                //若RackeDisplayer與之前的不一樣時
                if (_currentMouseOverRackSpacer != null && _currentMouseOverRackSpacer != targetRackSpacer)
                {
                    _currentMouseOverRackSpacer.RackData.HideRackSpaceDisplayerULevel();
                }

                _currentMouseOverRackSpacer = targetRackSpacer;

                //找出合適上架庫存設備高度U的ULevel
                int suitableStartULevelULevel = targetRackSpacer.RackData
                    .GetSuitableStartULevel(_selectedStockDevice, targetRackSpacer.ULevel);
                
                targetRackSpacer = targetRackSpacer.RackData.AvailableUDisplayer
                    .FirstOrDefault(displayer => displayer.ULevel.Equals(suitableStartULevelULevel));
                if (targetRackSpacer != null)
                {
                    _currentMouseOverRackSpacer.RackData
                        .ShowRackSpaceDisplayerULevel(suitableStartULevelULevel,
                            _selectedStockDevice.information.heightU);

                    parentTransform = targetRackSpacer.transform;
                    matColor = new Color(0, 1, 0, 10 / 255f);
                    Vector3 rackSpacerSize = targetRackSpacer.GetComponent<MeshFilter>().mesh.bounds.size;
                    
                    rackSpacerSize.y = 0;
                    localPosition = Vector3.zero - rackSpacerSize * 0.5f + uploadDevicePosOffset;
                    
                    //當設備Pivot在中心點
                    /*Vector3 diffSize = size - rackSpacerSize;
                    diffSize.y = 0;
                    localPosition = Vector3.zero + diffSize + uploadDevicePosOffset;*/
                    
                     _dragUploadDevice.gameObject.SetActive(true);
                }
            }
            else
            {
                _currentMouseOverRackSpacer?.RackData.AvailableUDisplayer.ForEach(
                    displayer => displayer.HideULevel());
                _currentMouseOverRackSpacer = null;
                _dragUploadDevice.gameObject.SetActive(false);
            }

            _dragUploadDevice.localRotation = Quaternion.Euler(Vector3.zero);
            _dragUploadDevice.SetParent(parentTransform, false);
            _dragUploadDevice.GetChild(0).GetComponent<MeshRenderer>().material.DOColor(matColor, 0.3f)
                .SetEase(Ease.OutQuad);
            _dragUploadDevice.localPosition = localPosition;
        }
    }

    public void OnUploadDeviceSuccess()
    {
        foreach (Transform child in _dragUploadDevice.transform)
        {
            Destroy(child.gameObject);
        }
        _dragUploadDevice = null;
        CancelUploadDevice(true);
    }

    #region Variables
    
    public RevitModelDataExtended ReviteModelData { get; private set; }
    public RackModelDataExtended RackModelDataInfo { get;  private set;}
    public DeviceModelDataExtended DeviceModelDataInfo { get;  private set;}
    
    /// 目前庫存設備所佔用的RackSpacerDisplayer
    private List<RackUnitDisplay> CurrentOccupyRackSpaceDisplayers
    {
        get
        {
            if (_currentMouseOverRackSpacer == null
                || _selectedStockDevice == null) return null;
            List<int> occupyULevel = Enumerable.Range(_currentMouseOverRackSpacer.ULevel,
                _selectedStockDevice.information.heightU).ToList();
            return _currentMouseOverRackSpacer.RackData.AvailableUDisplayer
                .Where(rackDisplayer => occupyULevel.Contains(rackDisplayer.ULevel)).ToList();
        }
    }

    private bool _isPutInRackSpacer = false;
    [NonSerialized]
    private RackUnitDisplay _currentMouseOverRackSpacer;

    /*[Header("[設定] - 機櫃空間的LayerMask")] [SerializeField]
    private LayerMask placeToRackSpacerLayerMask;*/

    [Header("[設定] - 上架設備的LayerMask")] [SerializeField]
    private LayerMask uploadDeviceLayerMask;

    [Header("[設定] - 上架設備的外框材質")] [SerializeField]
    private Material matUploadDevice;

    [SerializeField] private Material matUploadDeviceBorder;

    [Header("[設定] - 上架設備的位移")] [SerializeField]
    private Vector3 uploadDevicePosOffset;

    /// 目前所選擇的上架設備
    private DeviceModelDataExtended _selectedStockDevice;
    [NonSerialized]
    /// 新建的上架設備模型
    private Transform _dragUploadDevice;

    private Camera MainCamera => _mainCamera ??= Camera.main;
    [NonSerialized]
    private Camera _mainCamera;

    #endregion

   
   
}