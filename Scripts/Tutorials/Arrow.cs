using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Tutorials
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] private bool _hideFromStart = true;
        private Vector3 _startPosition;

        public bool Showed { get; private set; } = true;

        private void Awake()
        {
            _startPosition = transform.position;
            if (_hideFromStart)
                Hide(0f);
        }

        private void Start()
        {
            transform.DOMoveY(_startPosition.y + .5f, 1f).SetLoops(-1, LoopType.Yoyo);
        }

        [Button]
        public void Show()
        {
            Showed = true;
            transform.DOScale(Vector3.one, .5f);
        }

        [Button]
        public void Hide(float duration = .5f)
        {
            Showed = false;
            transform.DOScale(Vector3.zero, duration);
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }
    }
}