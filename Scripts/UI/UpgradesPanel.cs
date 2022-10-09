using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    public class UpgradesPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _title;
        [SerializeField] private Image _background;
        [SerializeField] private Transform _upgradePanelsParent;
        [SerializeField] private UpgradePanel _hireUpgradePanel;
        [SerializeField] private UpgradePanel _speedUpgradePanel;
        [SerializeField] private UpgradePanel _miracleUpgradePanel;

        public UpgradePanel HireUpgradePanel => _hireUpgradePanel;

        public UpgradePanel SpeedUpgradePanel => _speedUpgradePanel;

        public UpgradePanel MiracleUpgradePanel => _miracleUpgradePanel;

        public void Show(string title)
        {
            _title.text = title;
            _background.enabled = true;
            _upgradePanelsParent.DOScale(Vector3.one, .5f);
        }

        public void Hide(float duration = .5f)
        {
            _background.enabled = false;
            _upgradePanelsParent.DOScale(Vector3.zero, duration);
        }
    }
}