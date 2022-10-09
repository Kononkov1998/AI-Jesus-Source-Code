using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Parishioners
{
    public class ParishionerMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private CharacterAnimator _animator;
        private bool _moving;
        private bool _rotateAtTheEnd;
        private Transform _target;
        private Vector3 _velocity;

        private void Awake()
        {
            _agent.updatePosition = false;
        }

        private void FixedUpdate()
        {
            transform.position = _agent.nextPosition;

            if (!_moving) return;
            if (_agent.pathPending) return;
            if (_agent.remainingDistance > _agent.stoppingDistance) return;
            if (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f)
                OnPathComplete();
        }

        public event Action ReachedPosition;

        public void MoveTo(Transform target, bool rotateAtTheEnd = true)
        {
            _animator.Walk();
            _target = target;
            _rotateAtTheEnd = rotateAtTheEnd;
            _agent.SetDestination(_target.position);
            _moving = true;
        }

        private void OnPathComplete()
        {
            _animator.StopWalking();
            _moving = false;
            ReachedPosition?.Invoke();
            if (_rotateAtTheEnd)
                RotateTo(_target.eulerAngles);
        }

        private void RotateTo(Vector3 eulerRotation, float duration = 0.5f)
        {
            transform.DORotate(eulerRotation, duration);
        }
    }
}