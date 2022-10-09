using System.Collections;
using _Project.Scripts.Nimbuses;
using _Project.Scripts.Parishioners;
using UnityEngine;

namespace _Project.Scripts.Church
{
    public class ChurchParishioner : MonoBehaviour
    {
        [SerializeField] private ParishionerMovement _movement;
        [SerializeField] private CharacterAnimator _animator;
        private Church _church;
        private BenchPlace _targetPlace;

        public void Init(Church church)
        {
            _church = church;
        }

        public void TakePlace(BenchPlace place)
        {
            _targetPlace = place;
            _movement.MoveTo(_targetPlace.WalkPlace);
            _movement.ReachedPosition += SitOnPlace;
            _targetPlace.OnPlaceTaken();
        }

        private void SitOnPlace()
        {
            _movement.ReachedPosition -= SitOnPlace;
            _movement.enabled = false;
            transform.SetPositionAndRotation(_targetPlace.SeatPlace.position, _targetPlace.SeatPlace.rotation);
            _animator.Sit();
            StartCoroutine(GenerateNimbuses());
        }

        private IEnumerator GenerateNimbuses()
        {
            var delay = new WaitForSeconds(_church.NimbusesSpawnTime);
            while (enabled)
            {
                yield return delay;
                _church.NimbusBox.AddItem(NimbusCreator.Instance.SpawnNimbus(transform.position));
            }
        }
    }
}