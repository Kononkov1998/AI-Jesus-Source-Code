using GameAnalyticsSDK;
using UnityEngine;

namespace _Project.Scripts.Analytics
{
    public class MyGameAnalytics : MonoBehaviour
    {
        private static int _currentLevel;
        private static bool _initialized;

        private void Awake()
        {
            GameAnalytics.Initialize();
        }

        public static void OnLevelStarted(int level = 1)
        {
            _currentLevel = level;
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "level_event", $"level_{level}");
        }

        public static void OnLevelCompleted()
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "level_event", $"level_{_currentLevel}");
        }

        public static void OnLevelFailed()
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "level_event", $"level_{_currentLevel}");
        }

        public static void LogProgressionEvent(string eventName, float eventValue)
        {
            GameAnalytics.NewDesignEvent(eventName, eventValue);
        }

        public static void LogProgressionEvent(string eventName)
        {
            GameAnalytics.NewDesignEvent(eventName);
        }
    }
}