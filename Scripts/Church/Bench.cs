using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Church
{
    public class Bench : MonoBehaviour
    {
        [SerializeField] private List<BenchPlace> _places;

        public bool HasEmptyPlace => _places.Any(p => p.IsFree);

        public BenchPlace GetRandomFreePlace()
        {
            var freePlaces = _places.Where(p => p.IsFree).ToArray();
            var randomIndex = Random.Range(0, freePlaces.Length);
            return freePlaces[randomIndex];
        }
    }
}