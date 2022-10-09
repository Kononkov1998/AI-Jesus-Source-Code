using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace _Project.Scripts
{
    public class RectTransformAnchorsButtonEditor
    {
        public static List<Transform> m_transformList;
        public static List<Vector3> m_correspondingPosList;
        public static List<Quaternion> m_correspondingRotList;

        [MenuItem("Tools/Make it easy %&a")] // Ctrl + alt + a
        private static void NewMenuOption()
        {
            foreach (var s in Selection.objects)
            {
                var selected = s;
                var rt = ((GameObject) selected).GetComponent<RectTransform>();
                Debug.Log(selected);
                //rt.anchoredPosition = Vector2.zero;
                var parentRect = ((GameObject) selected).GetComponent<Transform>().parent.GetComponent<RectTransform>();
                //rt.anchorMin = new Vector2((parentRect.rect.height - rt.rect.height) / parentRect.rect.height, (parentRect.rect.height - rt.rect.height) / parentRect.rect.height);
                var w = rt.rect.width;
                var h = rt.rect.height;
                var x = rt.rect.x;
                var y = rt.rect.y;

                var offsetForMinAnchorByX = (rt.localPosition.x + parentRect.rect.width / 2 - rt.rect.width / 2) /
                                            parentRect.rect.width;
                var offsetForMaxAnchorByX = offsetForMinAnchorByX + rt.rect.width / parentRect.rect.width;

                var offsetForMaxAnchorByY = (rt.localPosition.y + parentRect.rect.height / 2 - rt.rect.height / 2) /
                                            parentRect.rect.height;
                var offsetForMinAnchorByY = offsetForMaxAnchorByY + rt.rect.height / parentRect.rect.height;

                rt.anchorMax = new Vector2(offsetForMaxAnchorByX, offsetForMinAnchorByY);
                rt.anchorMin = new Vector2(offsetForMinAnchorByX, offsetForMaxAnchorByY);

                rt.anchoredPosition = new Vector2(0f, 0f);
                rt.sizeDelta = Vector2.zero;
            }
        }

        [MenuItem("Tools/Make it center %&w")] // Ctrl + alt + w
        private static void SuperMenuOption()
        {
            foreach (var s in Selection.objects)
            {
                var selected = s;
                var rt = ((GameObject) selected).GetComponent<RectTransform>();
                Debug.Log(selected);
                //rt.anchoredPosition = Vector2.zero;
                var parentRect = ((GameObject) selected).GetComponent<Transform>().parent.GetComponent<RectTransform>();
                //rt.anchorMin = new Vector2((parentRect.rect.height - rt.rect.height) / parentRect.rect.height, (parentRect.rect.height - rt.rect.height) / parentRect.rect.height);
                var w = rt.rect.width;
                var h = rt.rect.height;
                var x = rt.rect.x;
                var y = rt.rect.y;

                var offsetForMinAnchorByX = (rt.localPosition.x + parentRect.rect.width / 2 - rt.rect.width / 2) /
                                            parentRect.rect.width;
                var offsetForMaxAnchorByX = offsetForMinAnchorByX + rt.rect.width / parentRect.rect.width;

                var offsetForMaxAnchorByY = (rt.localPosition.y + parentRect.rect.height / 2 - rt.rect.height / 2) /
                                            parentRect.rect.height;
                var offsetForMinAnchorByY = offsetForMaxAnchorByY + rt.rect.height / parentRect.rect.height;

                rt.anchorMax = new Vector2((offsetForMaxAnchorByX + offsetForMinAnchorByX) / 2f,
                    (offsetForMinAnchorByY + offsetForMaxAnchorByY) / 2f);
                rt.anchorMin = new Vector2((offsetForMaxAnchorByX + offsetForMinAnchorByX) / 2f,
                    (offsetForMinAnchorByY + offsetForMaxAnchorByY) / 2f);

                rt.anchoredPosition = new Vector2(0f, 0f);
                rt.sizeDelta = new Vector2(w, h);
            }
        }

        [MenuItem("Tools/Say what is it %&q")] // Ctrl + alt + q
        private static void AddMenuOption()
        {
            var selected = Selection.activeObject;
            foreach (var s in Selection.objects) Debug.Log(s);
        }

        [MenuItem("Tools/Parent Transform/Remember child positions %q")]
        private static void PrepareToMoveParent()
        {
            if (m_transformList != null)
                m_transformList.Clear();
            else
                m_transformList = new List<Transform>();

            if (m_correspondingPosList != null)
                m_correspondingPosList.Clear();
            else
                m_correspondingPosList = new List<Vector3>();

            if (m_correspondingRotList != null)
                m_correspondingRotList.Clear();
            else
                m_correspondingRotList = new List<Quaternion>();

            var selected = Selection.activeGameObject;

            for (var i = 0; i < selected.transform.childCount; i++)
            {
                var child = selected.transform.GetChild(i);
                m_transformList.Add(child);
                m_correspondingPosList.Add(child.transform.position);
                m_correspondingRotList.Add(child.transform.rotation);
            }

            Debug.Log($"Remembered {m_transformList.Count} transforms");
        }

        [MenuItem("Tools/Parent Transform/Switch child positions %w")]
        private static void EndToMoveParent()
        {
            var index = 0;

            foreach (var item in m_transformList)
            {
                var oldPos = item.position;
                var oldRot = item.rotation;
                item.position = m_correspondingPosList[index];
                item.rotation = m_correspondingRotList[index];
                m_correspondingPosList[index] = oldPos;
                m_correspondingRotList[index] = oldRot;
                index++;
            }

            if (m_transformList == null)
                Debug.Log("Switched 0 transforms");
            else
                Debug.Log($"Switched {m_transformList.Count} transforms");
        }
    }
}
#endif