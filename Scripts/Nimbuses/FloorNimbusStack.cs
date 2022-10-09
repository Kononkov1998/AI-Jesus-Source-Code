using _Project.Scripts.Camera;
using _Project.Scripts.Player;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Scripts.Nimbuses
{
    public class FloorNimbusStack : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _effect;
        [SerializeField] private float _distanceBetweenNimbuses;
        [SerializeField] private CameraPoint _cameraPoint;
        private Nimbus[] _nimbuses;

        private void Awake()
        {
            if (PlayerPrefs.GetInt("StartNimbusesAlreadyCollected") == 1)
                Destroy(gameObject);
            else
                _nimbuses = GetComponentsInChildren<Nimbus>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out PlayerMovement player)) return;

            PlayerPrefs.SetInt("StartNimbusesAlreadyCollected", 1);
            _effect.Stop();
            Wallet.Wallet.Instance.AddNimbusCount(_nimbuses.Length);
            player.Stack.AddItems(_nimbuses);
            _cameraPoint.Show();
            Destroy(gameObject, 2f);
        }

        [Button]
        private void UpdateNimbusesPlaces()
        {
            var nimbuses = GetComponentsInChildren<Nimbus>();
            for (var i = 1; i < nimbuses.Length; i++)
            {
                nimbuses[i].transform.position =
                    nimbuses[i - 1].transform.position + Vector3.up * _distanceBetweenNimbuses;
            }
        }
    }
}