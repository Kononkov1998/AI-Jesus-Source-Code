using UnityEngine;

namespace _Project.Scripts.Wallet
{
    public static class WalletSaveLoadSystem
    {
        public static void Save(WalletData data)
        {
            var dataStr = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(nameof(WalletData), dataStr);
        }

        public static WalletData Load()
        {
            var dataStr = PlayerPrefs.GetString(nameof(WalletData));
            return string.IsNullOrWhiteSpace(dataStr) ? new WalletData() : JsonUtility.FromJson<WalletData>(dataStr);
        }
    }
}