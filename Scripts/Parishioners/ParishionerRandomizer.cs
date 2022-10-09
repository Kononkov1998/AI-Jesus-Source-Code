using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Parishioners
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class ParishionerRandomizer : MonoBehaviour
    {
        [SerializeField] private int[] _hairIndexes;
        [SerializeField] private int[] _clothIndexes;
        [SerializeField] private int[] _skinIndexes;

        [SerializeField] private Gradient _hairColors;
        [SerializeField] private Gradient _clothColors;
        [SerializeField] private Gradient _skinColors;

        [SerializeField] private List<Material> _hairMaterials;
        [SerializeField] private List<Material> _clothMaterials;
        [SerializeField] private List<Material> _skinMaterials;

        [SerializeField] private bool _isChangeCloth;
        private SkinnedMeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<SkinnedMeshRenderer>();
            Randomize();
        }

        private void Randomize()
        {
            foreach (var hairIndex in _hairIndexes)
            {
                var _random = Random.Range(0, _hairMaterials.Count);
                var _tempMats = _meshRenderer.sharedMaterials;
                _tempMats[hairIndex] = _hairMaterials[_random];
                _meshRenderer.materials = _tempMats;
            }

            if (_isChangeCloth)
                foreach (var clothIndex in _clothIndexes)
                    _meshRenderer.materials[clothIndex].color = _clothColors.Evaluate(Random.value);

            foreach (var skinIndex in _skinIndexes)
            {
                var _random = Random.Range(0, _skinMaterials.Count);
                var _tempMats = _meshRenderer.sharedMaterials;
                _tempMats[skinIndex] = _skinMaterials[_random];
                _meshRenderer.materials = _tempMats;
            }
        }
    }
}