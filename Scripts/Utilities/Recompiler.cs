#if UNITY_EDITOR
using NaughtyAttributes;
using UnityEditor.Compilation;
using UnityEngine;

namespace _Project.Scripts.Utilities
{
    public class Recompiler : MonoBehaviour
    {
        [Button]
        private void Recompile()
        {
            CompilationPipeline.RequestScriptCompilation();
        }
    }
}
#endif