using UnityEngine;

namespace _Project.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Colors/SpotsColorsData")]
    public class SpotsColorData : ScriptableObject
    {
        public Color ActiveSpotColor;
        public Color InactiveSpotColor;
    }
}