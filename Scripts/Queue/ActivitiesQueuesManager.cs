using UnityEngine;

namespace _Project.Scripts.Queue
{
    public class ActivitiesQueuesManager : MonoBehaviour
    {
        [SerializeField] private ActivityQueues[] _activityQueues;

        public Queue GetNextQueue(int previousQueueIndex)
        {
            var newIndex = previousQueueIndex + 1;
            if (newIndex == _activityQueues.Length)
                return null;

            return _activityQueues[newIndex].HasEmptyPlace ? _activityQueues[newIndex].GetRandomNotFilledQueue() : null;
        }
    }
}