using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts
{
    public class Construction : MonoBehaviour, IConsumer
    {
        [SerializeField] private GameObject[] _objectsToShow;
        [SerializeField] private GameObject[] _objectsToDestroy;

        public void PerformOnFilled()
        {
            foreach (var objectToShow in _objectsToShow)
                objectToShow.SetActive(true);

            foreach (var objectToDestroy in _objectsToDestroy)
                objectToDestroy.transform.parent = transform.parent;

            Destroy(gameObject);
        }

        public void PerformOnReceiveResource()
        {
        }

        [Button]
        private void HideObjectsToOpen()
        {
            foreach (var objectToShow in _objectsToShow)
                objectToShow.SetActive(false);
        }
    }
}