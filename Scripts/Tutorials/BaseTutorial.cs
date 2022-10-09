using UnityEngine;

namespace _Project.Scripts.Tutorials
{
    [RequireComponent(typeof(Arrow))]
    public abstract class BaseTutorial : MonoBehaviour
    {
        [SerializeField] private string _saveKey;
        [SerializeField] private BaseTutorial _nextTutorial;
        [SerializeField] protected bool Enabled;
        protected Arrow Arrow;


        private void Awake()
        {
            Arrow = GetComponent<Arrow>();
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt(_saveKey) != 1) return;
            
            if (_nextTutorial)
                _nextTutorial.StartTutorial();
            Destroy(gameObject);
        }

        protected virtual void StartTutorial()
        {
            Arrow.Show();
            Enabled = true;
        }

        protected virtual void CompleteTutorial()
        {
            Arrow.Hide();
            if (_nextTutorial)
                _nextTutorial.StartTutorial();
            Enabled = false;
            Save();
            Destroy(gameObject);
        }

        private void Save()
        {
            PlayerPrefs.SetInt(_saveKey, 1);
        }
    }
}