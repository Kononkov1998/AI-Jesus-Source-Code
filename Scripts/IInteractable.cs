using UnityEngine;

namespace _Project.Scripts
{
    public interface IInteractable
    {
        public bool IsInteracting();
        public void StartInteracting(CharacterAnimator character);
        public void StopInteracting();
        public Transform GetInteractablePoint();
        public bool IsAutoWorking();
    }
}