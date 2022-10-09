using _Project.Scripts.Utilities;
using UnityEngine;
using UnityEngine.Pool;

namespace _Project.Scripts.Nimbuses
{
    public class NimbusCreator : Singleton<NimbusCreator>
    {
        [SerializeField] private Nimbus _nimbusPrefab;
        private IObjectPool<Nimbus> _pool;

        protected override void Initialize()
        {
            _pool = new ObjectPool<Nimbus>(CreateNimbus);
        }

        public Nimbus SpawnNimbus(Transform point)
        {
            var nimbus = SpawnNimbus();
            nimbus.transform.SetPositionAndRotation(point.position, point.rotation);
            return nimbus;
        }

        public Nimbus SpawnNimbus(Vector3 position)
        {
            var nimbus = SpawnNimbus();
            nimbus.transform.SetPositionAndRotation(position, Quaternion.identity);
            return nimbus;
        }

        public Nimbus SpawnNimbus()
        {
            var nimbus = _pool.Get();
            nimbus.gameObject.SetActive(true);
            return nimbus;
        }

        private Nimbus CreateNimbus()
        {
            var nimbus = Instantiate(_nimbusPrefab);
            nimbus.gameObject.SetActive(false);
            nimbus.Disabled += Release;
            return nimbus;
        }

        private void Release(Nimbus nimbus)
        {
            _pool.Release(nimbus);
        }
    }
}