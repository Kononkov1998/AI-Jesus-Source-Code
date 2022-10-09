using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Parishioners;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Queue
{
    public class Queue : MonoBehaviour
    {
        [SerializeField] private List<Transform> _places;
        [SerializeField] private float _distanceBetweenPlaces;
        private readonly List<Parishioner> _parishioners = new();

        public bool HasEmptyPlace => _parishioners.Count < _places.Count;
        public bool Empty => _parishioners.Count == 0;
        public Parishioner Parishioner => _parishioners[0];
        public event Action ParishionerIsReadyToBeServed;

        public void AddParishioner(Parishioner parishioner)
        {
            var parishionersCount = _parishioners.Count;
            if (parishionersCount == 0) parishioner.Movement.ReachedPosition += OnFirstParishionerPositionReached;

            _parishioners.Add(parishioner);
            parishioner.InteractionEnded += RemoveParishioner;
            parishioner.Movement.MoveTo(_places[parishionersCount]);
        }

        private void RemoveParishioner(Parishioner parishioner)
        {
            parishioner.InteractionEnded -= RemoveParishioner;
            var index = _parishioners.IndexOf(parishioner);
            _parishioners.RemoveAt(index);

            if (index == 0 && _parishioners.Count > 0)
                _parishioners[0].Movement.ReachedPosition += OnFirstParishionerPositionReached;

            UpdateParishionersDestinations();
        }

        private void UpdateParishionersDestinations()
        {
            for (var i = 0; i < _parishioners.Count; i++) _parishioners[i].Movement.MoveTo(_places[i]);
        }

        private void OnFirstParishionerPositionReached()
        {
            _parishioners[0].Movement.ReachedPosition -= OnFirstParishionerPositionReached;
            ParishionerIsReadyToBeServed?.Invoke();
        }

        [Button]
        private void GetPlacesFromChildren()
        {
            _places = GetComponentsInChildren<Transform>().Where(p => p != transform).ToList();
        }

        [Button]
        private void UpdatePlacesPositions()
        {
            for (var i = 1; i < _places.Count; i++)
            {
                var previousPlaceTransform = _places[i - 1].transform;
                _places[i].transform.rotation = previousPlaceTransform.rotation;
                _places[i].transform.position = previousPlaceTransform.position -
                                                previousPlaceTransform.forward * _distanceBetweenPlaces;
            }
        }
    }
}