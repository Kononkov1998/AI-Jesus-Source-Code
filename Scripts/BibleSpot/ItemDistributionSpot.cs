using System;
using System.Collections;
using _Project.Scripts.Utilities.Coroutines;
using UnityEngine;

namespace _Project.Scripts.BibleSpot
{
    public class ItemDistributionSpot : InteractiveSpot
    {
        [SerializeField] private float _biblesDistributeRate;
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

        public event Action BibleCanBeDistributed;

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

                yield return new WaitForSeconds(1f / _biblesDistributeRate / SpeedMultiplier);
                _distributionPrepared = false;
                BibleCanBeDistributed?.Invoke();
            }
        }
    }
}