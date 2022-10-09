using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Utilities.Coroutines
{
    public sealed class CoroutineObject : CoroutineObjectBase
    {
        public CoroutineObject(MonoBehaviour owner, Func<IEnumerator> routine)
        {
            Owner = owner;
            Routine = routine;
        }

        private Func<IEnumerator> Routine { get; }

        public override event Action Finished;

        private IEnumerator Process()
        {
            yield return Routine.Invoke();
            Coroutine = null;
            Finished?.Invoke();
        }

        public void Start()
        {
            Stop();
            Coroutine = Owner.StartCoroutine(Process());
        }

        public void Stop()
        {
            if (!IsProcessing) return;
            if (Owner != null)
                Owner.StopCoroutine(Coroutine);
            Coroutine = null;
        }
    }
}