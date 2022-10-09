using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts
{
    [Serializable]
    public class ConsumerView
    {
        [SerializeField] private TextMeshProUGUI _neededAmountText;
        [SerializeField] private Image _fill;
        private int _startNeededAmount;

        public void Init(int startNeededAmount)
        {
            _startNeededAmount = startNeededAmount;
            _neededAmountText.text = startNeededAmount.ToString();
        }

        public void UpdateVisual(int neededAmount)
        {
            _neededAmountText.text = neededAmount.ToString();
            _fill.fillAmount = 1f * (_startNeededAmount - neededAmount) / _startNeededAmount;
        }
    }
}