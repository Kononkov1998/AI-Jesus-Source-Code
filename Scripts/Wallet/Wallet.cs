using System;
using _Project.Scripts.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Wallet
{
    public class Wallet : Singleton<Wallet>
    {
        [SerializeField] private int _debugCountToAdd;
        private WalletData _data;

        public int NimbusCount => _data.NimbusCount;

        private void Start()
        {
            NimbusCountChanged?.Invoke(_data.NimbusCount, _data.NimbusCount);
        }

        public event Action<int, int> NimbusCountChanged;

        protected override void Initialize()
        {
            _data = WalletSaveLoadSystem.Load();
        }

        public void AddNimbusCount(int nimbusCount)
        {
            if (nimbusCount <= 0)
                Debug.LogError("You need to add value greater than zero");

            ChangeNimbusCount(nimbusCount);
        }

        public void ReduceNimbusCount(int nimbusCount)
        {
            if (nimbusCount <= 0)
                Debug.LogError("You need to withdraw value greater than zero");
            else if (nimbusCount > _data.NimbusCount)
                Debug.LogError("You cannot withdraw more than you have");

            ChangeNimbusCount(-nimbusCount);
        }

        private void ChangeNimbusCount(int nimbusCountToAdd)
        {
            var previousMoney = _data.NimbusCount;
            _data.NimbusCount += nimbusCountToAdd;
            NimbusCountChanged?.Invoke(previousMoney, _data.NimbusCount);
            Save();
        }

        private void Save()
        {
            WalletSaveLoadSystem.Save(_data);
        }

#if UNITY_EDITOR
        [Button]
        private void AddNimbusCount()
        {
            ChangeNimbusCount(_debugCountToAdd);
        }
#endif
    }
}