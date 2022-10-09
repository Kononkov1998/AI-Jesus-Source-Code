using System;
using System.Collections;
using _Project.Scripts.Analytics;
using _Project.Scripts.Camera;
using _Project.Scripts.Nimbuses;
using _Project.Scripts.Player;
using _Project.Scripts.Utilities.Coroutines;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Construction))]
    public class NimbusConsumer : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _saveKey;
        [SerializeField] [Range(0f, 5f)] private float _consumePerSecondPercent = 0.25f;
        [SerializeField] private ConsumePercentSource _consumePercentSource;
        [SerializeField] private int _startNeededNimbusAmount;
        [SerializeField] private ConsumerView _consumerView;
        [SerializeField] private CameraPoint _cameraPoint;
        private IConsumer _consumer;
        private CoroutineObject _consumingRoutine;
        private int _neededNimbusAmount;
        private PlayerStack _stack;
        private Wallet.Wallet _wallet;

        private bool IsFinished => _neededNimbusAmount == 0;
        private bool CanConsume => _neededNimbusAmount > 0 && _wallet.NimbusCount > 0;

        private void Awake()
        {
            _wallet = Wallet.Wallet.Instance;
            _consumer = GetComponent<IConsumer>();
            _consumingRoutine = new CoroutineObject(this, ConsumingRoutine);
            _neededNimbusAmount = PlayerPrefs.GetInt(_saveKey, _startNeededNimbusAmount);
            _consumerView.Init(_startNeededNimbusAmount);
            _consumerView.UpdateVisual(_neededNimbusAmount);
            if (IsFinished)
                Finish(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerMovement player))
                _stack = player.Stack;
        }

        public bool IsInteracting()
        {
            return _consumingRoutine.IsProcessing;
        }

        public void StartInteracting(CharacterAnimator _)
        {
            _consumingRoutine.Start();
        }

        public void StopInteracting()
        {
            _consumingRoutine.Stop();
        }

        public Transform GetInteractablePoint()
        {
            return null;
        }

        public bool IsAutoWorking()
        {
            return false;
        }

        private IEnumerator ConsumingRoutine()
        {
            var t = 0f;
            var consumePerSecond = CalculateConsumePerSecond(_neededNimbusAmount);
            var timeForOneConsume = 1f / consumePerSecond;

            while (CanConsume)
            {
                t += Time.deltaTime;
                if (t > timeForOneConsume)
                {
                    var extraConsumesForFrame = (t - timeForOneConsume) / timeForOneConsume + 1;
                    var iterations = (int) Mathf.Min(_wallet.NimbusCount, _neededNimbusAmount, extraConsumesForFrame);
                    var spawnedFakeOnThisFrame = false;
                    for (var i = 0; i < iterations; i++)
                    {
                        if (_stack.ItemsCount > _wallet.NimbusCount - 1)
                        {
                            RemoveItemFromStack();
                        }
                        else if (!spawnedFakeOnThisFrame)
                        {
                            RemoveFakeItem();
                            spawnedFakeOnThisFrame = true;
                        }

                        ConsumeResource();
                    }

                    t = 0f;
                }

                yield return null;
            }
        }

        private float CalculateConsumePerSecond(int neededAmount)
        {
            return _consumePercentSource switch
            {
                ConsumePercentSource.Wallet => _wallet.NimbusCount * _consumePerSecondPercent,
                ConsumePercentSource.NeededAmount => neededAmount * _consumePerSecondPercent,
                _ => 0f
            };
        }

        private void ConsumeResource()
        {
            _wallet.ReduceNimbusCount(1);
            _neededNimbusAmount--;

            if (_neededNimbusAmount < 0)
                Debug.LogError("Excess consumption");

            Save();
            InvokeOnConsumedEvents();

            if (IsFinished)
                Finish(false);
        }

        private void RemoveFakeItem()
        {
            var item = NimbusCreator.Instance.SpawnNimbus(_stack.LastPivotWithItem.transform);
            MoveItemToConsumer(item.transform);
        }

        private void RemoveItemFromStack()
        {
            var item = _stack.RemoveItem();
            MoveItemToConsumer(item);
        }

        private void MoveItemToConsumer(Transform item)
        {
            item.DOMove(transform.position, .5f).OnComplete(() => item.gameObject.SetActive(false));
        }

        private void InvokeOnConsumedEvents()
        {
            _consumer.PerformOnReceiveResource();
            _consumerView.UpdateVisual(_neededNimbusAmount);
        }

        private void Finish(bool finishedFromLoad)
        {
            _consumer.PerformOnFilled();
            if (finishedFromLoad) return;

            MyFacebook.LogAppEvent($"Built_{_saveKey}");
            MyGameAnalytics.LogProgressionEvent($"Built_{_saveKey}");
            if (_cameraPoint != null)
                _cameraPoint.Show();
        }

        private void Save()
        {
            PlayerPrefs.SetInt(_saveKey, _neededNimbusAmount);
        }

        [Button]
        private void GenerateSaveKey()
        {
            _saveKey = Guid.NewGuid().ToString();
        }

        private enum ConsumePercentSource
        {
            NeededAmount = 0,
            Wallet = 1
        }
    }
}