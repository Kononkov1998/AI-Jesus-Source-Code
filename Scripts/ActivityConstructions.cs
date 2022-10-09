using _Project.Scripts.Nimbuses;
using UnityEngine;

namespace _Project.Scripts
{
    public class ActivityConstructions : MonoBehaviour
    {
        [SerializeField] private BaseConstruction[] _constructions;
        [SerializeField] private int _nimbusCountForParishioner;
        [SerializeField] private NimbusBox _nimbusBox;
        [SerializeField] private Upgrades.Upgrades _upgrades;

        private void Awake()
        {
            _upgrades.Init();
            foreach (var construction in _constructions)
                construction.Init(_nimbusBox, _nimbusCountForParishioner);

            _upgrades.HireUpgraded += OnHireUpgraded;
            _upgrades.SpeedUpgraded += UpdateConstructionsSpeed;
        }

        private void Start()
        {
            for (var i = 0; i < _constructions.Length; i++)
            {
                if (_upgrades.HireData.Level <= i) break;
                _constructions[i].GetDistributionSpot()?.EnableAutoWorking();
                _constructions[i].GetReplenishmentSpot()?.EnableAutoWorking();
            }

            UpdateConstructionsSpeed();
        }

        private void OnHireUpgraded()
        {
            var hireLevel = _upgrades.HireData.Level;
            _constructions[hireLevel - 1].GetDistributionSpot()?.EnableAutoWorking();
            _constructions[hireLevel - 1].GetReplenishmentSpot()?.EnableAutoWorking();
        }

        private void UpdateConstructionsSpeed()
        {
            foreach (var construction in _constructions)
            {
                var speedValue = _upgrades.SpeedData.Value;
                construction.GetDistributionSpot()?.SetSpeedMultiplier(speedValue);
                construction.GetReplenishmentSpot()?.SetSpeedMultiplier(speedValue);
            }
        }
    }
}