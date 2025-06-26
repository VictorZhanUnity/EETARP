using System;
using System.Linq;
using TMPro;
using UnityEngine;
using VictorDev.DoTweenUtils;
using VictorDev.Revit;
using XCharts.Runtime;

namespace VictorDev.TCIT
{
    /// 顯示機櫃內設備類型數量
    public class RackDevicesDisplayer : MonoBehaviour, RackRevitInfoPage.IRackModelDataExtended
    {
        public void ReceiveRackModelData(RackModelDataExtended rackModelData)
        {
            _rackModelData = rackModelData;
            UpdateUI();
            UpdateChart();
        }

        private void UpdateUI()
        {
            int totalDevices = _rackModelData.Containers.Count;
            _numOfServer = _rackModelData.Containers.Count(device => device.DeviceKind == EnumReviteModelKind.Server);
            _numOfRouter = _rackModelData.Containers.Count(device => device.DeviceKind == EnumReviteModelKind.Router);
            _numOfSwitch = _rackModelData.Containers.Count(device => device.DeviceKind == EnumReviteModelKind.Switch);

            void ToBlink(TextMeshProUGUI target, string text) => DotweenHelper.ToBlink(target, text, 0.1f, 0.3f, true);
            DotweenHelper.ToBlink(TxtNumberOfDevices, totalDevices.ToString());
            ToBlink(TxtNumberOfServer, _numOfServer.ToString());
            ToBlink(TxtNumberOfRouter, _numOfRouter.ToString());
            ToBlink(TxtNumberOfSwitch, _numOfSwitch.ToString());
        }

        private void UpdateChart()
        {
            PieChartInstance.ClearData();
            PieChartInstance.AddData(0, _numOfServer, "Server");
            PieChartInstance.AddData(0, _numOfRouter, "Router");
            PieChartInstance.AddData(0, _numOfSwitch, "Switch");
        }


        #region Variables

        private RackModelDataExtended _rackModelData;
        private int _numOfServer, _numOfRouter, _numOfSwitch;
        private PieChart PieChartInstance =>
            _pieChartInstance ??= transform.Find("Panel/Container/PieChart").GetComponent<PieChart>();

        [NonSerialized] private PieChart _pieChartInstance;

        private TextMeshProUGUI TxtNumberOfDevices => _txtNumberOfDevices ??=
            transform.Find("Panel/Container/TxtNumberOfDevices").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtNumberOfServer => _txtNumberOfServer ??=
            transform.Find("Panel/Container/VLayout/TxtNumberOfServer").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtNumberOfRouter => _txtNumberOfRouter ??=
            transform.Find("Panel/Container/VLayout/TxtNumberOfRouter").GetComponent<TextMeshProUGUI>();

        private TextMeshProUGUI TxtNumberOfSwitch => _txtNumberOfSwitch ??=
            transform.Find("Panel/Container/VLayout/TxtNumberOfSwitch").GetComponent<TextMeshProUGUI>();

        [NonSerialized]
        private TextMeshProUGUI _txtNumberOfDevices, _txtNumberOfServer, _txtNumberOfRouter, _txtNumberOfSwitch;

        #endregion
    }
}