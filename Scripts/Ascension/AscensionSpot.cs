using UnityEngine;

namespace _Project.Scripts.Ascension
{
    public class AscensionSpot : InteractiveSpot
    {
        [SerializeField] private Gate _gate;

        public bool GateOpened => _gate.Opened;

        public override void StartInteracting(CharacterAnimator character)
        {
            _gate.Open();
            if (!IsAutoWorking())
                base.StartInteracting(character);
            Worker.StartPray();
        }

        public override void StopInteracting()
        {
            _gate.Close();
            Worker.StopPray();
            base.StopInteracting();
        }
    }
}