using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Nimbuses;
using _Project.Scripts.Utilities;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerStack : MonoBehaviour
    {
        [SerializeField] private float _distanceBetweenItems;
        [SerializeField] private int _maxItemsCount;
        [SerializeField] private int _maxDistance;
        [SerializeField] private float _maxRotationAngle;
        [SerializeField] private AnimationCurve _stackRotationCurve;
        [SerializeField] private List<StackPivot> _pivots = new();
        private readonly List<Vector3> _positions = new();

        public int ItemsCount => _pivots.Count(p => p.HasItem);
        public StackPivot LastPivotWithItem => _pivots.Last(p => p.HasItem);
        private bool HasFreeSpace => ItemsCount < _maxItemsCount;

        private void Awake()
        {
            for (var i = 0; i < Mathf.Min(_maxItemsCount, Wallet.Wallet.Instance.NimbusCount); i++)
                AddItem(NimbusCreator.Instance.SpawnNimbus().transform, 0f);
            Wallet.Wallet.Instance.NimbusCountChanged += MapStackCountToWallet;
        }

        private void FixedUpdate()
        {
            _positions.Add(transform.position);
            if (_positions.Count > _maxDistance)
                _positions.RemoveRange(0, _positions.Count - _maxDistance);
        }

        public void AddItems(IEnumerable<Nimbus> items)
        {
            foreach (var item in items)
                AddItem(item.transform);
        }

        public Transform RemoveItem()
        {
            var pivot = LastPivotWithItem;
            var nimbus = pivot.Item;
            pivot.RemoveItem();
            nimbus.SetParent(NimbusCreator.Instance.transform);
            return nimbus;
        }

        private void AddItem(Transform item, float duration = 1f)
        {
            var hasFreeSpace = HasFreeSpace;
            var pivot = GetTargetPivot();

            if (hasFreeSpace)
                pivot.SetItem(item);
            else
                item.SetParent(pivot.transform);

            StartCoroutine(MoveToStack(item, pivot.transform, !hasFreeSpace, duration));
        }

        private static IEnumerator MoveToStack(Transform item, Transform target, bool destroy, float duration)
        {
            var t = 0f;
            var startPosition = item.position;
            var startRotation = item.rotation;

            while (t < duration)
            {
                t += Time.deltaTime;
                item.position = Vector3.Lerp(startPosition, target.position, t);
                item.rotation = Quaternion.Slerp(startRotation, target.rotation, t);
                if (t < duration)
                    yield return null;
            }

            if (destroy)
            {
                item.SetParent(NimbusCreator.Instance.transform);
                item.gameObject.SetActive(false);
            }
            else
            {
                item.SetParent(target);
                item.localPosition = Vector3.zero;
                item.localRotation = Quaternion.identity;
            }
        }

        public void UpdateStackPositionAndRotation(float movementMagnitude)
        {
            for (var i = 1; i < _pivots.Count; i++)
            {
                var pivotTransform = _pivots[i].transform;
                var newRotation = Vector3.zero;
                var heightMultiplier = i.Remap(0, _maxItemsCount, 0, 1);
                heightMultiplier = _stackRotationCurve.Evaluate(heightMultiplier);
                newRotation.x = -_maxRotationAngle * heightMultiplier * movementMagnitude;
                /*var lookPosition = transform.position - _items[i].transform.position;
                lookPosition.x = 0;
                lookPosition.y = 0;
                newRotation.z = -Quaternion.LookRotation(lookPosition).eulerAngles.z * 15f * heightMultiplier;*/
                var localRotation = Quaternion.Euler(newRotation);
                //_pivots[i].transform.localEulerAngles = newRotation;
                pivotTransform.localRotation = Quaternion.RotateTowards(pivotTransform.localRotation,
                    localRotation, Time.deltaTime * 360f);

                var newPosition = Vector3.Lerp(transform.position, _positions[0], heightMultiplier);
                pivotTransform.position = new Vector3(newPosition.x, pivotTransform.position.y, newPosition.z);

                /*if (movingDirectionMagnitude == 0f)
                {
                    if (_returningToDefault) continue;
                    _returningToDefault = true;
                    pivotTransform.DOLocalRotate(newRotation, .5f);
                }
                else
                {
                    _returningToDefault = false;
                    DOTween.Kill(pivotTransform);
                    pivotTransform.localEulerAngles = newRotation;
                }*/
            }
        }

        private void MapStackCountToWallet(int previousAmount, int currentAmount)
        {
            while (ItemsCount > currentAmount)
            {
                var nimbus = RemoveItem();
                nimbus.gameObject.SetActive(false);
            }
        }

        private StackPivot GetTargetPivot()
        {
            return HasFreeSpace ? _pivots.First(p => !p.HasItem) : _pivots[^1];
        }

        [Button]
        private void CreatePivots()
        {
            _pivots.Clear();

            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }

            for (var i = 0; i < _maxItemsCount; i++)
            {
                var pivot = new GameObject("Pivot").AddComponent<StackPivot>();
                var pivotTransform = pivot.transform;
                pivotTransform.SetParent(transform);
                pivotTransform.localPosition = Vector3.up * i * _distanceBetweenItems;
                _pivots.Add(pivot);
            }
        }
    }
}