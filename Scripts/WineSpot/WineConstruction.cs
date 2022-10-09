using System.Collections;
using _Project.Scripts.Nimbuses;
using UnityEngine;

namespace _Project.Scripts.WineSpot
{
    public class WineConstruction : BaseConstruction
    {
        [SerializeField] private WineReplenishmentSpot _wineReplenishmentSpot;
        [SerializeField] private WineDistributionSpot _wineDistributionSpot;
        private bool _barrelFilled;

        private void Awake()
        {
            _wineDistributionSpot.InteractionStarted += StartReplenishmentInteraction;
            _wineDistributionSpot.InteractionEnded += StopReplenishmentInteraction;
            _wineDistributionSpot.WineCanBeDistributed += DistributeItem;
            _wineDistributionSpot.CupStartedToFill += ReduceBarrelLiquid;
            Queue.ParishionerIsReadyToBeServed += OnParishionerIsReady;
        }

        private void OnDestroy()
        {
            _wineDistributionSpot.InteractionStarted += StartReplenishmentInteraction;
            _wineDistributionSpot.InteractionEnded += StopReplenishmentInteraction;
            _wineDistributionSpot.WineCanBeDistributed -= DistributeItem;
            _wineDistributionSpot.CupStartedToFill -= ReduceBarrelLiquid;
            Queue.ParishionerIsReadyToBeServed -= OnParishionerIsReady;
        }

        public override void Init(NimbusBox nimbusBox, int nimbusCountForParishioner)
        {
            NimbusBox = nimbusBox;
            NimbusCountForParishioner = nimbusCountForParishioner;
            _wineDistributionSpot.Init();
            _wineReplenishmentSpot.Init();
        }

        public override InteractiveSpot GetReplenishmentSpot()
        {
            return _wineReplenishmentSpot;
        }

        public override InteractiveSpot GetDistributionSpot()
        {
            return _wineDistributionSpot;
        }

        protected override void OnParishionerIsReady()
        {
            StartCoroutine(PrepareWineDistribution());
        }

        private IEnumerator PrepareWineDistribution()
        {
            while (_wineReplenishmentSpot.NeedFilling)
                yield return null;

            _wineDistributionSpot.OnDistributionPrepared();
        }

        private void ReduceBarrelLiquid(float duration)
        {
            _wineReplenishmentSpot.ReduceLiquid(duration);
        }

        private void DistributeItem(Cup cup)
        {
            if (!_wineReplenishmentSpot.NeedFilling && _wineDistributionSpot.Worker != null)
                _wineDistributionSpot.Worker.Put();
            Queue.Parishioner.TakeCup(cup);
            Queue.Parishioner.InteractionEnded += CreateNimbuses;
        }


        private void StartReplenishmentInteraction()
        {
            _wineReplenishmentSpot.StartInteracting(_wineDistributionSpot.Worker);
        }

        private void StopReplenishmentInteraction()
        {
            _wineReplenishmentSpot.StopInteracting();
        }
    }
}