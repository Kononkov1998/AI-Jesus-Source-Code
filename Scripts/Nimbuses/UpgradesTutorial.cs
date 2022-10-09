using _Project.Scripts.Tutorials;
using UnityEngine;

namespace _Project.Scripts.Nimbuses
{
    public class UpgradesTutorial : BaseTutorial
    {
        [SerializeField] private Upgrades.Upgrades _upgrades;

        protected override void StartTutorial()
        {
            _upgrades.SpeedUpgraded += OnUpgradeBought;
            _upgrades.HireUpgraded += OnUpgradeBought;
            base.StartTutorial();
        }


        protected override void CompleteTutorial()
        {
            _upgrades.SpeedUpgraded -= OnUpgradeBought;
            _upgrades.HireUpgraded -= OnUpgradeBought;
            base.CompleteTutorial();
        }

        private void OnUpgradeBought()
        {
            CompleteTutorial();
        }
    }
}