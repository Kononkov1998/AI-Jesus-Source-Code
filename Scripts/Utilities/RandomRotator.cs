using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class RandomRotator : MonoBehaviour
    {
        [Button]
        private void RotateChildren()
        {
            foreach (var obj in GetComponentsInChildren<Transform>())
            {
                if (obj != transform)
                    obj.eulerAngles = new Vector3(0, Random.Range(0f, 360f), 0);
            }
        }
    }
}