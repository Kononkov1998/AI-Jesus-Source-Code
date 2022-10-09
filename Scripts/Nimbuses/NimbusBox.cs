using System;
using System.Collections.Generic;
using _Project.Scripts.Player;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Nimbuses
{
    public class NimbusBox : MonoBehaviour
    {
        [SerializeField] private string _saveKey;
        [SerializeField] private int _rows;
        [SerializeField] private int _columns;
        [SerializeField] private float _xDistanceBetweenItems;
        [SerializeField] private float _yDistanceBetweenItems;
        [SerializeField] private float _zDistanceBetweenItems;
        [SerializeField] private Transform _startPoint;
        private readonly List<Nimbus> _nimbuses = new();

        public int NimbusesCount => _nimbuses.Count;

        private void Awake()
        {
            Load();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_nimbuses.Count < 1) return;
            if (!other.TryGetComponent(out PlayerMovement player)) return;

            Wallet.Wallet.Instance.AddNimbusCount(_nimbuses.Count);
            _nimbuses.ForEach(n => DOTween.Kill(n.transform));
            player.Stack.AddItems(_nimbuses);
            _nimbuses.Clear();
            Save();
            NimbusesTaken?.Invoke();
        }

        public event Action NimbusesTaken;

        public void AddItem(Nimbus item, float duration = 1f)
        {
            var thisTransform = transform;
            var itemTransform = item.transform;
            var targetPosition = _startPoint.position
                                 + thisTransform.right *
                                 (_nimbuses.Count / _columns % _columns * _xDistanceBetweenItems)
                                 + thisTransform.up * (_nimbuses.Count / (_rows * _columns) * _yDistanceBetweenItems)
                                 + thisTransform.forward * (_nimbuses.Count % _rows * _zDistanceBetweenItems);

            _nimbuses.Add(item);
            itemTransform.SetParent(thisTransform);
            var ease = itemTransform.position.y < targetPosition.y ? Ease.OutBack : Ease.InBack;
            DOTween.Sequence()
                .Join(itemTransform.DOMoveX(targetPosition.x, duration))
                .Join(itemTransform.DOMoveY(targetPosition.y, duration).SetEase(ease))
                .Join(itemTransform.DOMoveZ(targetPosition.z, duration));
            Save();
        }

        private void Save()
        {
            PlayerPrefs.SetInt(_saveKey, _nimbuses.Count);
        }

        private void Load()
        {
            var nimbusesCount = PlayerPrefs.GetInt(_saveKey);
            for (var i = 0; i < nimbusesCount; i++)
            {
                var nimbus = NimbusCreator.Instance.SpawnNimbus();
                AddItem(nimbus, 0f);
            }
        }

        [Button]
        private void GenerateSaveKey()
        {
            _saveKey = Guid.NewGuid().ToString();
        }
    }
}