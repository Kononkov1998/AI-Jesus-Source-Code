using UnityEngine;

namespace _Project.Scripts.Camera
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform _targetToFollow;
        private bool _following = true;
        private Vector3 _offset;

        public Vector3 TargetPosition => _targetToFollow.position + _offset;

        private void Awake()
        {
            _offset = transform.position - _targetToFollow.position;
        }

        private void FixedUpdate()
        {
            if (_following)
                transform.position = TargetPosition;
        }

        public void StartFollow()
        {
            _following = true;
        }

        public void StopFollow()
        {
            _following = false;
        }
    }
}