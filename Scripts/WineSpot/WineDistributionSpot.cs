using System;
using System.Collections;
using _Project.Scripts.Utilities.Coroutines;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.WineSpot
{
    public class WineDistributionSpot : InteractiveSpot
    {
        [SerializeField] private Cup _cupPrefab;
        [SerializeField] private float _distributeRate;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _fillPoint;
        private CoroutineObject _distributeRoutine;
        private bool _distributionPrepared;

        private void OnEnable()
        {
            if (IsAutoWorking())
                _distributeRoutine.Start();
        }

        public override void Init()
        {
            _distributeRoutine = new CoroutineObject(this, DistributeRoutine);
            base.Init();
        }

        public event Action<float> CupStartedToFill;
        public event Action<Cup> WineCanBeDistributed;

        public override void StartInteracting(CharacterAnimator character)
        {
            if (!gameObject.activeInHierarchy) return;
            _distributeRoutine.Start();
            if (!IsAutoWorking())
                base.StartInteracting(character);
        }

        public override void StopInteracting()
        {
            _distributeRoutine.Stop();
            base.StopInteracting();
        }

        public void OnDistributionPrepared()
        {
            _distributionPrepared = true;
        }

        private IEnumerator DistributeRoutine()
        {
            while (enabled)
            {
                while (!_distributionPrepared)
                    yield return null;

                var distributingDuration = 1f / _distributeRate / SpeedMultiplier;
                var cup = Instantiate(_cupPrefab, _spawnPoint.position, _spawnPoint.rotation, transform);
                var cupTransform = cup.transform;
                var targetScale = cupTransform.localScale;

                cupTransform.localScale = Vector3.zero;
                _distributionPrepared = false;
                DOTween.Sequence()
                    .Append(cupTransform.DOScale(targetScale, .5f))
                    .Append(cupTransform.DOMove(_fillPoint.position, .5f))
                    .AppendCallback(() => CupStartedToFill?.Invoke(distributingDuration))
                    .Join(cup.Fill(distributingDuration))
                    .Append(cupTransform.DOMove(_spawnPoint.position, .5f))
                    .OnComplete(() => WineCanBeDistributed?.Invoke(cup));
            }
        }
    }
}