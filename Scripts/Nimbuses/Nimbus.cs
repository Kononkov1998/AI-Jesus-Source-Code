using System;
using UnityEngine;

namespace _Project.Scripts.Nimbuses
{
    public class Nimbus : MonoBehaviour
    {
        private void OnDisable()
        {
            Disabled?.Invoke(this);
        }

        public event Action<Nimbus> Disabled;
    }
}