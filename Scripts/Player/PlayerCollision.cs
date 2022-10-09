using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerCollision : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;
        private IInteractable _interactable;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IInteractable interactable)) return;

            if (_interactable != null && _interactable != interactable && _interactable.IsInteracting())
                StopInteracting();

            _interactable = interactable;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_interactable == null) return;
            if (!other.TryGetComponent(out IInteractable interactable)) return;
            if (interactable != _interactable) return;

            if (_interactable.IsInteracting())
                StopInteracting();
            else
                _interactable = null;
        }

        private void OnTriggerStay(Collider other)
        {
            if (_interactable == null) return;

            if (!_interactable.IsInteracting() && !_movement.HaveInput && !_interactable.IsAutoWorking())
                StartInteracting(_movement.Animator);
            else if (_interactable.IsInteracting() && _movement.HaveInput)
                StopInteracting();
        }

        private void StartInteracting(CharacterAnimator character)
        {
            var interactablePoint = _interactable.GetInteractablePoint();
            _interactable.StartInteracting(character);
            if (interactablePoint != null)
                _movement.MoveToTarget(interactablePoint);
        }

        private void StopInteracting()
        {
            _interactable.StopInteracting();
            if (_interactable.GetInteractablePoint() != null)
                _movement.StopMovingToTarget();
            _interactable = null;
        }
    }
}