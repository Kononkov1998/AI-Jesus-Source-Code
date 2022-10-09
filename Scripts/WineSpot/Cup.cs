using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.WineSpot
{
    public class Cup : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _liquid;

        public Tween Fill(float duration)
        {
            return _liquid.transform.DOScale(Vector3.one, duration);
        }
    }
}