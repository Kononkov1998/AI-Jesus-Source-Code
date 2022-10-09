using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Joystick _joystick;
        [SerializeField] private PlayerStack _stack;
        [SerializeField] private CharacterAnimator _animator;
        [SerializeField] private ParticleSystem _movingEffect;
        private bool _moving;

        public CharacterAnimator Animator => _animator;
        public PlayerStack Stack => _stack;
        public bool HaveInput => _joystick.Direction != Vector2.zero;

        private void Update()
        {
            _stack.UpdateStackPositionAndRotation(_joystick.Direction.magnitude);
            UpdateRunningAnimation();
            UpdateMovingEffect();

            if (!HaveInput && _rigidbody.velocity != Vector3.zero)
                _rigidbody.velocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            if (!_joystick.Active || !HaveInput)
                return;

            var direction = Vector3.forward * _joystick.Vertical + Vector3.right * _joystick.Horizontal;
            _rigidbody.MovePosition(_rigidbody.position + _maxSpeed * Time.deltaTime * direction);
            _rigidbody.rotation = Quaternion.LookRotation(direction);
        }

        public void MoveToTarget(Transform target)
        {
            _rigidbody.DOMove(target.position, 1f);
            _rigidbody.DORotate(target.eulerAngles, 1f);
        }

        public void StopMovingToTarget()
        {
            DOTween.Kill(_rigidbody);
        }

        private void UpdateRunningAnimation()
        {
            switch (HaveInput)
            {
                case true when !_moving:
                    _animator.Run();
                    _moving = true;
                    break;
                case false when _moving:
                    _animator.StopRunning();
                    _moving = false;
                    break;
            }
        }

        private void UpdateMovingEffect()
        {
            switch (HaveInput)
            {
                case false when _movingEffect.isEmitting:
                    _movingEffect.Stop();
                    break;
                case true when !_movingEffect.isEmitting:
                    _movingEffect.Play();
                    break;
            }
        }
    }
}