using System;
using UnityEngine;

namespace _Project.Scripts.Church
{
    [Serializable]
    public class BenchPlace
    {
        [SerializeField] private Transform _walkPlace;
        [SerializeField] private Transform _seatPlace;

        public bool IsFree { get; private set; } = true;
        public Transform WalkPlace => _walkPlace;
        public Transform SeatPlace => _seatPlace;

        public void OnPlaceTaken()
        {
            IsFree = false;
        }
    }
}