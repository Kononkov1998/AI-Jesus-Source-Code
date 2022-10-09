using Facebook.Unity;
using UnityEngine;

namespace _Project.Scripts.Analytics
{
    public class MyFacebook : MonoBehaviour
    {
        private static int _currentLevel;

        // Awake function from Unity's MonoBehavior
        private void Awake()
        {
            if (!FB.IsInitialized)
                // Initialize the Facebook SDK
                FB.Init(InitCallback, OnHideUnity);
            else
                // Already initialized, signal an app activation App Event
                FB.ActivateApp();
        }

        private static void InitCallback()
        {
            if (FB.IsInitialized)
                // Signal an app activation App Event
                FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
            else
                Debug.Log("Failed to Initialize the Facebook SDK");
        }

        private static void OnHideUnity(bool isGameShown)
        {
            Time.timeScale = isGameShown ? 1 : 0;
        }

        public static void OnLevelStarted(int level = 1)
        {
            _currentLevel = level;
            FB.LogAppEvent($"Started level {level}");
        }

        public static void OnLevelCompleted()
        {
            FB.LogAppEvent($"Completed level {_currentLevel}");
        }

        public static void OnLevelFailed()
        {
            FB.LogAppEvent($"Failed level {_currentLevel}");
        }

        public static void OnRewarded(string rewardName)
        {
            FB.LogAppEvent($"Rewarded {rewardName}");
        }

        public static void OnInterstitial()
        {
            FB.LogAppEvent("Interstitial watched");
        }

        public static void LogAppEvent(string logEvent)
        {
            FB.LogAppEvent(logEvent);
        }
    }
}