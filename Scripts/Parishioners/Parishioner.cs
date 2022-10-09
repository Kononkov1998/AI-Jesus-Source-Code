using System;
using _Project.Scripts.BibleSpot;
using _Project.Scripts.Queue;
using _Project.Scripts.WineSpot;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Parishioners
{
    public class Parishioner : MonoBehaviour
    {
        // todo: gets angry after some time and quit queue
        //[SerializeField] private float _timeToGetAngry;
        [SerializeField] private Transform _biblePoint;
        [SerializeField] private Transform _cupPoint;
        [SerializeField] private ParishionerMovement _movement;
        [SerializeField] private CharacterAnimator _animator;
        private int _completedInteractionsCount;
        private Transform _exit;
        private ActivitiesQueuesManager _queuesManager;

        public ParishionerMovement Movement => _movement;

        public event Action<Parishioner> InteractionEnded;

        public void Init(Transform exit, ActivitiesQueuesManager queuesManager)
        {
            _queuesManager = queuesManager;
            _exit = exit;
            AddToNextQueueOrGoAway();
        }

        public void TakeBible(Item item)
        {
            _animator.TakeInLeftHand();
            item.transform.SetParent(_biblePoint);
            DOTween.Sequence()
                .Append(item.transform.DOLocalMove(Vector3.zero, 1f))
                .Join(item.transform.DOLocalRotateQuaternion(Quaternion.identity, .5f).SetEase(Ease.InSine))
                .OnComplete(() =>
                {
                    InteractionEnded?.Invoke(this);
                    _completedInteractionsCount++;
                    AddToNextQueueOrGoAway();
                });
        }

        public void TakeCup(Cup cup)
        {
            _animator.TakeInRightHand();
            cup.transform.SetParent(_cupPoint);
            DOTween.Sequence()
                .Append(cup.transform.DOLocalMove(Vector3.zero, 1f))
                .Join(cup.transform.DOLocalRotateQuaternion(Quaternion.identity, 1f))
                .OnComplete(() =>
                {
                    InteractionEnded?.Invoke(this);
                    _completedInteractionsCount++;

                    AddToNextQueueOrGoAway();
                });
        }

        public void OnAscensionStarted()
        {
            _animator.Ascend();
        }

        public void OnAscensionCompleted()
        {
            InteractionEnded?.Invoke(this);
            Destroy(gameObject);
        }

        private void AddToNextQueueOrGoAway()
        {
            var nextQueue = _queuesManager.GetNextQueue(_completedInteractionsCount - 1);
            if (nextQueue == null)
            {
                _movement.MoveTo(_exit, false);
                _movement.ReachedPosition += () => Destroy(gameObject);
            }
            else
            {
                nextQueue.AddParishioner(this);
            }
        }
    }
}