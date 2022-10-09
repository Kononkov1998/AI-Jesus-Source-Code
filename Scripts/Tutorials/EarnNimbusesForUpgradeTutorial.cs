using _Project.Scripts.BibleSpot;
using _Project.Scripts.Nimbuses;
using UnityEngine;

namespace _Project.Scripts.Tutorials
{
    public class EarnNimbusesForUpgradeTutorial : BaseTutorial
    {
        [SerializeField] private ItemDistributionSpot _spot;
        [SerializeField] private NimbusBox _nimbusBox;

        private void Update()
        {
            if (!Enabled) return;

            if (_spot.IsInteracting() && Arrow.Showed)
                Arrow.Hide();
            else if (!_spot.IsInteracting() && !Arrow.Showed)
                Arrow.Show();

            if (Wallet.Wallet.Instance.NimbusCount + _nimbusBox.NimbusesCount >= 10)
            {
                CompleteTutorial();
            }
        }
    }
}