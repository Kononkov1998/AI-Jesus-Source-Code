using _Project.Scripts.BibleSpot;
using _Project.Scripts.Camera;
using _Project.Scripts.UI;
using UnityEngine;

namespace _Project.Scripts.Tutorials
{
    public class FinalEarnTutorial : BaseTutorial
    {
        [SerializeField] private ItemDistributionSpot _spot;
        [SerializeField] private UpgradesPanel _panel;
        [SerializeField] private GameObject _objectToShow;
        [SerializeField] private CameraPoint _cameraPoint;

        private void Update()
        {
            if (!Enabled) return;

            if (_spot.IsInteracting() || _spot.IsAutoWorking())
                CompleteTutorial();
        }

        protected override void CompleteTutorial()
        {
            _panel.Hide();
            _objectToShow.SetActive(true);
            _cameraPoint.Show();
            base.CompleteTutorial();
        }
    }
}