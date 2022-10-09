using DG.Tweening;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.UI
{
    public class NimbusPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nimbusCount;
        [SerializeField] private float _visualUpdateDuration;

        private void Awake()
        {
            Wallet.Wallet.Instance.NimbusCountChanged += OnNimbusCountChanged;
        }

        private void OnDestroy()
        {
            Wallet.Wallet.Instance.NimbusCountChanged -= OnNimbusCountChanged;
        }

        private void OnNimbusCountChanged(int previousNimbusCount, int newNimbusCount)
        {
            DOTween.To(() => previousNimbusCount, value => _nimbusCount.text = value.ToString(),
                newNimbusCount, _visualUpdateDuration);
        }
    }
}