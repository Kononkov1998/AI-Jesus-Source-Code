using System.Collections;
using _Project.Scripts.Nimbuses;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Ascension
{
    public class AscensionConstruction : BaseConstruction
    {
        [SerializeField] private AscensionSpot _ascensionSpot;
        [SerializeField] private ParticleSystem _ascensionEffect;
        [SerializeField] private Transform _ascensionStartPoint;
        [SerializeField] private Transform _ascensionEndPoint;
        [SerializeField] private float _ascensionDuration;
        private bool _barrelFilled;

        private void Awake()
        {
            //_wineDistributionSpot.WineCanBeDistributed += DistributeItem;
            Queue.ParishionerIsReadyToBeServed += OnParishionerIsReady;
        }

        private void OnDestroy()
        {
            //_wineDistributionSpot.WineCanBeDistributed -= DistributeItem;
            Queue.ParishionerIsReadyToBeServed -= OnParishionerIsReady;
        }

        public override void Init(NimbusBox nimbusBox, int nimbusCountForParishioner)
        {
            NimbusBox = nimbusBox;
            NimbusCountForParishioner = nimbusCountForParishioner;
            _ascensionSpot.Init();
        }

        public override InteractiveSpot GetReplenishmentSpot()
        {
            return null;
        }

        public override InteractiveSpot GetDistributionSpot()
        {
            return _ascensionSpot;
        }

        protected override void OnParishionerIsReady()
        {
            if (_ascensionSpot.Worker != null)
                _ascensionSpot.Worker.ToHeaven();
            StartCoroutine(MoveParishionerToAscensionPoint());
        }

        private IEnumerator MoveParishionerToAscensionPoint()
        {
            while (!_ascensionSpot.GateOpened)
                yield return null;
            yield return null;

            Queue.Parishioner.Movement.MoveTo(_ascensionStartPoint);
            Queue.Parishioner.Movement.ReachedPosition += StartAscension;
            Queue.Parishioner.InteractionEnded += CreateNimbuses;
        }

        private void StartAscension()
        {
            _ascensionEffect.Play();
            var parishioner = Queue.Parishioner;
            parishioner.Movement.ReachedPosition -= StartAscension;
            parishioner.OnAscensionStarted();
            DOTween.Sequence()
                .AppendInterval(0.5f)
                .Append(parishioner.transform.DOMove(_ascensionEndPoint.position, _ascensionDuration))
                .OnComplete(() =>
                {
                    _ascensionEffect.Stop();
                    parishioner.OnAscensionCompleted();
                });
        }
    }
}