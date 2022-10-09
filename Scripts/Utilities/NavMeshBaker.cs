#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class NavMeshBaker : MonoBehaviour
    {
        [SerializeField] private bool _useDisabledGameObjects;
        private IEnumerable<GameObject> _disabledObjects;

        [Button]
        private void Bake()
        {
            if (_useDisabledGameObjects)
            {
                _disabledObjects = GetAllObjectsOnlyInScene().Where(go => !go.activeSelf).ToArray();
                foreach (var obj in _disabledObjects)
                    obj.gameObject.SetActive(true);
            }

            NavMeshBuilder.ClearAllNavMeshes();
            NavMeshBuilder.BuildNavMesh();

            if (_useDisabledGameObjects)
                foreach (var obj in _disabledObjects)
                    obj.gameObject.SetActive(false);
        }

        private List<GameObject> GetAllObjectsOnlyInScene()
        {
            return ((GameObject[]) Resources.FindObjectsOfTypeAll(typeof(GameObject))).Where(go =>
                !EditorUtility.IsPersistent(go.transform.root.gameObject) &&
                go.hideFlags is not (HideFlags.NotEditable or HideFlags.HideAndDontSave)).ToList();
        }
    }
}
#endif