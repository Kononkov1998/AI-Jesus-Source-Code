using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Queue
{
    public class ActivityQueues : MonoBehaviour
    {
        [SerializeField] private Queue[] _queues;

        public bool HasEmptyPlace => _queues.Any(q => q.HasEmptyPlace && q.gameObject.activeInHierarchy);

        public Queue GetRandomNotFilledQueue()
        {
            var notFilledQueues = _queues.Where(q => q.HasEmptyPlace && q.gameObject.activeInHierarchy).ToList();
            var randomIndex = Random.Range(0, notFilledQueues.Count);
            return notFilledQueues[randomIndex];
        }
    }
}