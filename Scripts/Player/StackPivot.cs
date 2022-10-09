using UnityEngine;

namespace _Project.Scripts.Player
{
    public class StackPivot : MonoBehaviour
    {
        public Transform Item { get; private set; }

        public bool HasItem => Item != null;

        public void SetItem(Transform item)
        {
            Item = item;
        }

        public void RemoveItem()
        {
            Item = null;
        }
    }
}