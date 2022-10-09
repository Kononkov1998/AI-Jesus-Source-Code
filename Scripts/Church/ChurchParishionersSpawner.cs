using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Church
{
    public class ChurchParishionersSpawner : MonoBehaviour
    {
        [SerializeField] private ChurchParishioner[] _parishionerPrefab;
        [SerializeField] private float _timeToSpawn;
        [SerializeField] private Church[] _churches;

        private void Awake()
        {
            StartCoroutine(SpawnRoutine());
        }

        private IEnumerator SpawnRoutine()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(_timeToSpawn);

                foreach (var church in _churches)
                {
                    if (!church.HasEmptyPlace)
                        continue;

                    var thisTransform = transform;
                    var randomPrefab = _parishionerPrefab[Random.Range(0, _parishionerPrefab.Length)];
                    var parishioner = Instantiate(randomPrefab, thisTransform.position,
                        thisTransform.rotation, thisTransform);
                    var place = church.GetRandomNotFilledBench().GetRandomFreePlace();

                    parishioner.Init(church);
                    parishioner.TakePlace(place);
                }
            }
        }
    }
}