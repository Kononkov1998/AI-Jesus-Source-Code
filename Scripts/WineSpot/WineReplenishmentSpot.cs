using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.WineSpot
{
    public class WineReplenishmentSpot : InteractiveSpot
    {
        [SerializeField] private float _fillSpeed;
        [SerializeField] private MeshRenderer _liquid;
        [SerializeField] private float _liquidForDrink;
        [SerializeField] private Color _waterColor;
        [SerializeField] private Color _wineColor;
        [SerializeField] private ParticleSystem _fillEffect;
        private readonly Vector3 _zeroLiquidSize = new(0.75418f, 1, 0);

        public bool NeedFilling { get; private set; } = true;

        private void Start()
        {
            if (IsAutoWorking())
                Worker.DoRandomMagick();
        }

        private void Update()
        {
            if (NeedFilling && (IsInteracting() || IsAutoWorking()))
            {
                if (!_liquid.enabled)
                {
                    _fillEffect.Play();
                    _liquid.enabled = true;
                }

                _liquid.transform.localScale = Vector3.MoveTowards(_liquid.transform.localScale, Vector3.one,
                    _fillSpeed * SpeedMultiplier * Time.deltaTime);

                if (_liquid.transform.localScale.x < 1f) return;
                NeedFilling = false;
                _fillEffect.Stop();
                _liquid.material.DOColor(_wineColor, 0.5f);
            }
        }

        public override void StartInteracting(CharacterAnimator character)
        {
            if (NeedFilling && gameObject.activeInHierarchy)
                character.DoRandomMagick();
            if (!IsAutoWorking())
                base.StartInteracting(character);
        }

        public void ReduceLiquid(float duration)
        {
            var newScale = Vector3.MoveTowards(_liquid.transform.localScale, _zeroLiquidSize, _liquidForDrink);
            _liquid.transform.DOScale(newScale, duration).OnComplete(() =>
            {
                if (newScale != _zeroLiquidSize) return;
                NeedFilling = true;
                _liquid.material.color = _waterColor;
                _liquid.enabled = false;
                Worker.DoRandomMagick();
            });
        }
    }
}