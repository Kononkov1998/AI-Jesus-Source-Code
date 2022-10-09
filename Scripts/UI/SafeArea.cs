using UnityEngine;

namespace _Project.Scripts.UI
{
    /// <summary>
    ///     Safe area implementation for notched mobile devices. Usage:
    ///     (1) Add this component to the top level of any GUI panel.
    ///     (2) If the panel uses a full screen background image, then create an immediate child and put the component on that
    ///     instead, with all other elements childed below it.
    ///     This will allow the background image to stretch to the full extents of the screen behind the notch, which looks
    ///     nicer.
    ///     (3) For other cases that use a mixture of full horizontal and vertical background stripes, use the Conform X & Y
    ///     controls on separate elements as needed.
    /// </summary>
    public class SafeArea : MonoBehaviour
    {
        [SerializeField]
        private bool ConformX = true; // Conform to screen safe area on X-axis (default true, disable to ignore)

        [SerializeField]
        private bool ConformY = true; // Conform to screen safe area on Y-axis (default true, disable to ignore)

        private Rect LastSafeArea = new(0, 0, 0, 0);

        private RectTransform Panel;

        private void Awake()
        {
            Panel = GetComponent<RectTransform>();

            if (Panel == null)
            {
                Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(gameObject);
            }

            Refresh();
        }

        private void Refresh()
        {
            var safeArea = GetSafeArea();

            if (safeArea != LastSafeArea)
                ApplySafeArea(safeArea);
        }

        private static Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        private void ApplySafeArea(Rect r)
        {
            LastSafeArea = r;

            // Ignore x-axis?
            if (!ConformX)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            // Ignore y-axis?
            if (!ConformY)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            var anchorMin = r.position;
            var anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = anchorMax;

            //Debug.LogFormat ("New safe area applied to {0}: x={1}, y={2}, w={3}, h={4} on full extents w={5}, h={6}",
            //    name, r.x, r.y, r.width, r.height, Screen.width, Screen.height); dev
        }
    }
}