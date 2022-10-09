using System;
using _Project.Scripts.Analytics;
using _Project.Scripts.Player;
using _Project.Scripts.ScriptableObjects;
using _Project.Scripts.UI;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Upgrades
{
    public class Upgrades : MonoBehaviour
    {
        [SerializeField] private string _title;
        [SerializeField] private string _hireTitle;
        [SerializeField] private string _speedTitle;
        [SerializeField] private string _miracleTitle;
        [SerializeField] private UpgradesData _upgradesData;
        [SerializeField] private string _saveKey;
        [SerializeField] private UpgradesPanel _upgradesPanel;
        [SerializeField] private bool _ignoreMiracleUpgrade;
        private UpgradesLevels _data;

        public FloatUpgrade SpeedData { get; private set; }
        public BaseUpgrade HireData { get; private set; }
        public BaseUpgrade MiracleData { get; private set; }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerCollision _)) return;
            _upgradesPanel.HireUpgradePanel.Clicked += IncreaseHireLevel;
            _upgradesPanel.SpeedUpgradePanel.Clicked += IncreaseSpeedLevel;
            _upgradesPanel.MiracleUpgradePanel.Clicked += IncreaseMiracleLevel;
            UpdatePanels();
            _upgradesPanel.Show(_title);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out PlayerCollision _)) return;
            _upgradesPanel.HireUpgradePanel.Clicked -= IncreaseHireLevel;
            _upgradesPanel.SpeedUpgradePanel.Clicked -= IncreaseSpeedLevel;
            _upgradesPanel.MiracleUpgradePanel.Clicked -= IncreaseMiracleLevel;
            _upgradesPanel.Hide();
        }

        public void Init()
        {
            _data = UpgradesSaveLoadSystem.Load(_saveKey);

            UpdateSpeed(_data.SpeedLevel);
            UpdateHire(_data.HireLevel);
            UpdateMiracle(_data.MiracleLevel);
        }

        public event Action SpeedUpgraded;
        public event Action HireUpgraded;
        public event Action MiracleUpgraded;

        private void IncreaseSpeedLevel()
        {
            IncreaseUpgradeLevel(ref _data.SpeedLevel, UpdateSpeed, GetNextSpeedCost());
            SpeedUpgraded?.Invoke();
            MyFacebook.LogAppEvent($"{_saveKey}_Speed_{_data.SpeedLevel}");
            MyGameAnalytics.LogProgressionEvent($"{_saveKey}_Speed", _data.SpeedLevel);
        }

        private void IncreaseHireLevel()
        {
            IncreaseUpgradeLevel(ref _data.HireLevel, UpdateHire, GetNextHireCost());
            HireUpgraded?.Invoke();
            MyFacebook.LogAppEvent($"{_saveKey}_Hire_{_data.HireLevel}");
            MyGameAnalytics.LogProgressionEvent($"{_saveKey}_Hire", _data.HireLevel);
        }

        private void IncreaseMiracleLevel()
        {
            IncreaseUpgradeLevel(ref _data.MiracleLevel, UpdateMiracle, GetNextMiracleCost());
            MiracleUpgraded?.Invoke();
            //MyFacebook.LogAppEvent($"Miracle_{_data.MiracleLevel}");
            //MyGameAnalytics.LogProgressionEvent("Miracle", _data.MiracleLevel);
            Debug.Log($"Miracle increased to {_data.MiracleLevel}");
        }

        private bool SpeedHasMaxLevel()
        {
            return _upgradesData.SpeedHaveMaxLevel(_data.SpeedLevel);
        }

        private bool HireHasMaxLevel()
        {
            return _upgradesData.HireHaveMaxLevel(_data.HireLevel);
        }

        private bool MiracleHasMaxLevel()
        {
            return _upgradesData.MiracleHaveMaxLevel(_data.MiracleLevel);
        }

        private int GetNextSpeedCost()
        {
            return _upgradesData.GetSpeedCost(_data.SpeedLevel + 1);
        }

        private int GetNextHireCost()
        {
            return _upgradesData.GetHireCost(_data.HireLevel + 1);
        }

        private int GetNextMiracleCost()
        {
            return _upgradesData.GetMiracleCost(_data.MiracleLevel + 1);
        }

        private void IncreaseUpgradeLevel(ref int level, Action<int> updateAction, int cost)
        {
            level++;
            updateAction.Invoke(level);
            Save();
            if (cost > 0)
                Wallet.Wallet.Instance.ReduceNimbusCount(cost);
            UpdatePanels();
        }

        private void UpdateSpeed(int level)
        {
            SpeedData = _upgradesData.GetSpeedUpgradeByLevel(level);
        }

        private void UpdateHire(int level)
        {
            HireData = _upgradesData.GetHireUpgradeByLevel(level);
        }

        private void UpdateMiracle(int level)
        {
            MiracleData = _upgradesData.GetMiracleUpgradeByLevel(level);
        }

        private void UpdatePanels()
        {
            UpdateSpeedPanel();
            UpdateHirePanel();
            UpdateMiraclePanel();
        }

        private void UpdateSpeedPanel()
        {
            UpdatePanel(_upgradesPanel.SpeedUpgradePanel, SpeedHasMaxLevel(),
                _data.SpeedLevel + 1, GetNextSpeedCost(), _speedTitle);
        }

        private void UpdateHirePanel()
        {
            UpdatePanel(_upgradesPanel.HireUpgradePanel, HireHasMaxLevel(),
                _data.HireLevel + 1, GetNextHireCost(), _hireTitle);
        }

        private void UpdateMiraclePanel()
        {
            if (_ignoreMiracleUpgrade)
            {
                _upgradesPanel.MiracleUpgradePanel.gameObject.SetActive(false);
            }
            else
            {
                _upgradesPanel.MiracleUpgradePanel.gameObject.SetActive(true);
                UpdatePanel(_upgradesPanel.MiracleUpgradePanel, MiracleHasMaxLevel(),
                    _data.MiracleLevel + 1, GetNextMiracleCost(), _miracleTitle);
            }
        }

        private static void UpdatePanel(UpgradePanel panel, bool hasMaxLevel, int nextLevel, int nextCost, string title)
        {
            panel.UpdateVisuals(title, nextLevel, nextCost);

            if (!hasMaxLevel)
                panel.UpdateInteractivity(nextCost <= Wallet.Wallet.Instance.NimbusCount);
            else
                panel.UpdateInteractivity(false);
        }

        private void Save()
        {
            UpgradesSaveLoadSystem.Save(_saveKey, _data);
        }

        [Button]
        private void GenerateSaveKey()
        {
            _saveKey = Guid.NewGuid().ToString();
        }
    }
}