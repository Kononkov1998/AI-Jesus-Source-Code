using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int RunningHash = Animator.StringToHash("IsRunning");
        private static readonly int AscendHash = Animator.StringToHash("Ascend");
        private static readonly int SittingHash = Animator.StringToHash("IsSitting");
        private static readonly int TakeInLeftHandHash = Animator.StringToHash("TakeInLeftHand");
        private static readonly int TakeInRightHandHash = Animator.StringToHash("TakeInRightHand");
        private static readonly int PutHash = Animator.StringToHash("Put");
        private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
        private static readonly int ToHeavenHash = Animator.StringToHash("ToHeaven");
        private static readonly int MagickIndexHash = Animator.StringToHash("MagickIndex");
        private static readonly int MagickHash = Animator.StringToHash("Magick");
        private static readonly int IsPrayingHash = Animator.StringToHash("IsPraying");
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Run()
        {
            _animator.SetBool(RunningHash, true);
        }

        public void StopRunning()
        {
            _animator.SetBool(RunningHash, false);
        }

        public void Walk()
        {
            _animator.SetBool(IsWalkingHash, true);
        }

        public void StopWalking()
        {
            _animator.SetBool(IsWalkingHash, false);
        }

        public void Ascend()
        {
            _animator.SetTrigger(AscendHash);
        }

        public void Sit()
        {
            _animator.SetBool(SittingHash, true);
        }

        public void TakeInLeftHand()
        {
            _animator.SetTrigger(TakeInLeftHandHash);
        }

        public void TakeInRightHand()
        {
            _animator.SetTrigger(TakeInRightHandHash);
        }

        public void Put()
        {
            _animator.ResetTrigger(PutHash);
            _animator.SetTrigger(PutHash);
            DOTween.Sequence().AppendInterval(0.1f).OnComplete(() => _animator.ResetTrigger(PutHash));
        }

        public void ToHeaven()
        {
            _animator.SetTrigger(ToHeavenHash);
        }

        public void DoRandomMagick()
        {
            _animator.ResetTrigger(PutHash);
            var randomIndex = Random.Range(0, 7);
            _animator.SetInteger(MagickIndexHash, randomIndex);
            _animator.SetTrigger(MagickHash);
        }

        public void StartPray()
        {
            _animator.SetBool(IsPrayingHash, true);
        }

        public void StopPray()
        {
            _animator.SetBool(IsPrayingHash, false);
        }
    }
}