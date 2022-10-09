using UnityEngine;

namespace _Project.Scripts.Upgrades
{
    public static class UpgradesSaveLoadSystem
    {
        public static void Save(string key, UpgradesLevels upgradesLevels)
        {
            var dataStr = JsonUtility.ToJson(upgradesLevels);
            PlayerPrefs.SetString(key, dataStr);
        }

        public static UpgradesLevels Load(string key)
        {
            var upgradeLevels = PlayerPrefs.GetString(key);
            return string.IsNullOrWhiteSpace(upgradeLevels)
                ? new UpgradesLevels()
                : JsonUtility.FromJson<UpgradesLevels>(upgradeLevels);
        }
    }
}