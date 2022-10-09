using System.Linq;
using _Project.Scripts.Nimbuses;
using UnityEngine;

namespace _Project.Scripts.Church
{
    public class Church : MonoBehaviour
    {
        [SerializeField] private Bench[] _benches;
        [SerializeField] private NimbusBox _nimbusBox;
        [SerializeField] private float _nimbusesSpawnTime;

        public float NimbusesSpawnTime => _nimbusesSpawnTime;
        public NimbusBox NimbusBox => _nimbusBox;
        public bool HasEmptyPlace => _benches.Any(b => b.HasEmptyPlace && b.gameObject.activeInHierarchy);

        public Bench GetRandomNotFilledBench()
        {
            var notFilledBenches = _benches.Where(q => q.HasEmptyPlace && q.gameObject.activeInHierarchy).ToList();
            var randomIndex = Random.Range(0, notFilledBenches.Count);
            return notFilledBenches[randomIndex];
        }
    }
}