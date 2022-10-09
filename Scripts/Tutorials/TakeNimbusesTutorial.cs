using _Project.Scripts.Nimbuses;
using UnityEngine;

namespace _Project.Scripts.Tutorials
{
    public class TakeNimbusesTutorial : BaseTutorial
    {
        [SerializeField] private NimbusBox _nimbusBox;

        protected override void StartTutorial()
        {
            _nimbusBox.NimbusesTaken += OnNimbusesTaken;
            base.StartTutorial();
        }

        protected override void CompleteTutorial()
        {
            _nimbusBox.NimbusesTaken -= OnNimbusesTaken;
            base.CompleteTutorial();
        }

        private void OnNimbusesTaken()
        {
            CompleteTutorial();
        }
    }
}