using System;
using _Project.Scripts.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    public abstract class InteractiveSpot : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpotsColorData _colorData;
        [SerializeField] private Image _indicator;
        [SerializeField] private Transform _interactablePoint;
        [SerializeField] private CharacterAnimator _worker;
        private bool _autoWorking;
        private bool _isInteracting;
        private Vector3 _startIndicatorScale;
        protected float SpeedMultiplier = 1f;

        public CharacterAnimator Worker { get; private set; }

        public bool IsAutoWorking()
        {
            return _autoWorking;
        }

        public bool IsInteracting()
        {
            return _isInteracting;
        }

        public virtual void StartInteracting(CharacterAnimator character)
        {
            Worker = character;
            _isInteracting = true;
            _indicator.color = _colorData.ActiveSpotColor;
            InteractionStarted?.Invoke();

            DOTween.Sequence(_indicator.transform)
                .Append(_indicator.transform.DOScale(_startIndicatorScale * 1.15f, 1f))
                .Append(_indicator.transform.DOScale(_startIndicatorScale, 1f))
                .SetLoops(-1);
        }

        public virtual void StopInteracting()
        {
            Worker = null;
            _isInteracting = false;
            _indicator.color = _colorData.InactiveSpotColor;
            DOTween.Kill(_indicator.transform);
            _indicator.transform.localScale = _startIndicatorScale;
            InteractionEnded?.Invoke();
        }

        public Transform GetInteractablePoint()
        {
            return _interactablePoint;
        }

        public event Action InteractionStarted;
        public event Action InteractionEnded;

        public void SetSpeedMultiplier(float value)
        {
            SpeedMultiplier = value;
        }

        public virtual void Init()
        {
            _startIndicatorScale = _indicator.transform.localScale;
        }

        public void EnableAutoWorking()
        {
            Worker = _worker;
            _autoWorking = true;
            if (_worker != null)
                _worker.gameObject.SetActive(true);
            StartInteracting(_worker);
        }
    }
}