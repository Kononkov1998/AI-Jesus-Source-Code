using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Upgrades;
using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Upgrades/UpgradesData")]
    public class UpgradesData : ScriptableObject
    {
        [SerializeField] private FloatUpgrade[] _speedUpgrades;
        [SerializeField] private BaseUpgrade[] _hireUpgrades;
        [SerializeField] private BaseUpgrade[] _miracleUpgrades;

        private void OnValidate()
        {
            ValidateSpeed();
            ValidateHires();
            ValidateMiracles();
        }

        public FloatUpgrade GetSpeedUpgradeByLevel(int level)
        {
            return GetUpgradeByLevel(_speedUpgrades, level);
        }

        public BaseUpgrade GetHireUpgradeByLevel(int level)
        {
            return GetUpgradeByLevel(_hireUpgrades, level);
        }

        public BaseUpgrade GetMiracleUpgradeByLevel(int level)
        {
            return GetUpgradeByLevel(_miracleUpgrades, level);
        }

        public bool SpeedHaveMaxLevel(int level)
        {
            return IsMaxLevel(_speedUpgrades, level);
        }

        public bool HireHaveMaxLevel(int level)
        {
            return IsMaxLevel(_hireUpgrades, level);
        }

        public bool MiracleHaveMaxLevel(int level)
        {
            return IsMaxLevel(_miracleUpgrades, level);
        }

        public int GetSpeedCost(int level)
        {
            var upgrade = GetUpgradeByLevel(_speedUpgrades, level);
            return upgrade?.Cost ?? -1;
        }

        public int GetHireCost(int level)
        {
            var upgrade = GetUpgradeByLevel(_hireUpgrades, level);
            return upgrade?.Cost ?? -1;
        }

        public int GetMiracleCost(int level)
        {
            var upgrade = GetUpgradeByLevel(_miracleUpgrades, level);
            return upgrade?.Cost ?? -1;
        }

        private static T GetUpgradeByLevel<T>(IEnumerable<T> upgrades, int level) where T : BaseUpgrade
        {
            return upgrades.FirstOrDefault(x => x.Level == level);
        }

        private static bool IsMaxLevel<T>(IEnumerable<T> upgrades, int level) where T : BaseUpgrade
        {
            return upgrades.Last().Level == level;
        }

        private void ValidateSpeed()
        {
            /*for (var i = 1; i < _speedUpgrades.Length; i++)
        {
            _speedUpgrades[i].Value = _speedUpgrades[i - 1].Value + _speedValueAddition;
            _speedUpgrades[i].Cost = _speedUpgrades[i - 1].Cost * _speedCostModifier;
        }*/

            ValidateUpgradesLevels(_speedUpgrades);
        }

        private void ValidateHires()
        {
            /*for (var i = 1; i < _hireUpgrades.Length; i++)
        {
            _hireUpgrades[i].Cost = _hireUpgrades[i - 1].Cost * _hireCostModifier;
        }*/

            ValidateUpgradesLevels(_hireUpgrades);
        }

        private void ValidateMiracles()
        {
            /*for (var i = 1; i < _miracleUpgrades.Length; i++)
        {
            _miracleUpgrades[i].Cost = _miracleUpgrades[i - 1].Cost * _miracleCostModifier;
        }*/

            ValidateUpgradesLevels(_miracleUpgrades);
        }

        private static void ValidateUpgradesLevels(IReadOnlyList<BaseUpgrade> upgrades)
        {
            if (upgrades.Count == 0) return;

            for (var i = 1; i < upgrades.Count; i++) upgrades[i].Level = upgrades[i - 1].Level + 1;
        }
    }
}