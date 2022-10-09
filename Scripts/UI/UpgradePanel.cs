using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class UpgradePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private TextMeshProUGUI _level;
        [SerializeField] private TextMeshProUGUI _price;
        [SerializeField] private Button _buyButton;
        private Shadow _buttonShadow;

        private void Awake()
        {
            _buyButton.onClick.AddListener(() => Clicked?.Invoke());
            _buttonShadow = _buyButton.GetComponent<Shadow>();
        }

        private void OnDestroy()
        {
            _buyButton.onClick.RemoveAllListeners();
        }

        public event Action Clicked;

        public void UpdateVisuals(string title, int nextLevel, int nextPrice)
        {
            _title.text = title;
            var price = nextPrice == -1 ? "MAX" : nextPrice.ToString();
            _level.text = $"Level {nextLevel}";
            _price.text = $"{price}";
        }

        public void UpdateInteractivity(bool interactable)
        {
            _buyButton.interactable = interactable;
            _buttonShadow.enabled = interactable;
        }
    }
}