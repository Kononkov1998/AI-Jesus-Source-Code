using System.Collections;
using _Project.Scripts.Queue;
using UnityEngine;

namespace _Project.Scripts.Parishioners
{
    public class ParishionerSpawner : MonoBehaviour
    {
        [SerializeField] private Parishioner[] _parishionerPrefab;
        [SerializeField] private float _timeToSpawn;
        [SerializeField] private ActivitiesQueuesManager _queuesManager;
        [SerializeField] private Transform _exit;

        private void Awake()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(_timeToSpawn);

                var firstQueue = _queuesManager.GetNextQueue(-1);
                if (firstQueue == null)
                    continue;

                var thisTransform = transform;
                var parishioner = Instantiate(_parishionerPrefab[Random.Range(0, _parishionerPrefab.Length)],
                    thisTransform.position,
                    thisTransform.rotation, thisTransform);
                parishioner.Init(_exit, _queuesManager);
            }
        }
    }
}