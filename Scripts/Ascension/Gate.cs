using System.Collections;
using _Project.Scripts.Utilities.Coroutines;
using UnityEngine;

namespace _Project.Scripts.Ascension
{
    public class Gate : MonoBehaviour
    {
        [SerializeField] private Transform _firstGatePart;
        [SerializeField] private Transform _secondGatePart;
        [SerializeField] private Vector3 _firstGatePartOpenEulerRotation;
        [SerializeField] private Vector3 _secondGatePartOpenEulerRotation;
        [SerializeField] private float _openingSpeed;
        private readonly Quaternion _identityRotation = Quaternion.Euler(new Vector3(0, -90, 0));
        private CoroutineObject _closeRoutine;
        private Quaternion _firstGatePartOpenRotation;
        private CoroutineObject _openRoutine;
        private Quaternion _secondGatePartOpenRotation;

        public bool Opened => Quaternion.Angle(_firstGatePart.localRotation, _firstGatePartOpenRotation) < 0.01f;
        private bool Closed => Quaternion.Angle(_firstGatePart.localRotation, _identityRotation) < 0.01f;

        private void Awake()
        {
            _openRoutine = new CoroutineObject(this, OpenRoutine);
            _closeRoutine = new CoroutineObject(this, CloseRoutine);
            _firstGatePartOpenRotation = Quaternion.Euler(_firstGatePartOpenEulerRotation);
            _secondGatePartOpenRotation = Quaternion.Euler(_secondGatePartOpenEulerRotation);
        }

        public void Open()
        {
            _closeRoutine.Stop();
            _openRoutine.Start();
        }

        public void Close()
        {
            _openRoutine.Stop();
            _closeRoutine.Start();
        }

        private IEnumerator OpenRoutine()
        {
            while (!Opened)
            {
                MoveGateTowards(_firstGatePart, _firstGatePartOpenRotation);
                MoveGateTowards(_secondGatePart, _secondGatePartOpenRotation);
                yield return null;
            }
        }

        private IEnumerator CloseRoutine()
        {
            while (!Closed)
            {
                MoveGateTowards(_firstGatePart, _identityRotation);
                MoveGateTowards(_secondGatePart, _identityRotation);
                yield return null;
            }
        }

        private void MoveGateTowards(Transform target, Quaternion to)
        {
            target.localRotation = Quaternion.RotateTowards(target.localRotation, to, _openingSpeed * Time.deltaTime);
        }
    }
}