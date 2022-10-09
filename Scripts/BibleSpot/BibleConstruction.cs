using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Nimbuses;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.BibleSpot
{
    public class BibleConstruction : BaseConstruction
    {
        [SerializeField] private Item itemPrefab;
        [SerializeField] private Transform _itemSpawnPoint;
        [SerializeField] private Transform _itemBottomPoint;
        [SerializeField] private float _distanceBetweenItems;
        [SerializeField] private int _maxItems;
        [SerializeField] private ItemReplenishmentSpot _itemReplenishmentSpot;
        [SerializeField] private ItemDistributionSpot _itemDistributionSpot;
        [SerializeField] private ParticleSystem _spawnEffect;
        [SerializeField] private ParticleSystem _landEffect;
        private readonly List<Item> _bibles = new();

        private void Awake()
        {
            _itemDistributionSpot.InteractionStarted += StartReplenishmentInteraction;
            _itemDistributionSpot.InteractionEnded += StopReplenishmentInteraction;
            _itemDistributionSpot.BibleCanBeDistributed += TryDistributeItem;
            Queue.ParishionerIsReadyToBeServed += OnParishionerIsReady;
        }

        private void Update()
        {
            if (_bibles.Count != 0) return;
            if (!_itemReplenishmentSpot.IsInteracting() && !_itemReplenishmentSpot.IsAutoWorking()) return;
            SpawnItems();
        }

        private void OnDestroy()
        {
            _itemDistributionSpot.InteractionStarted -= StartReplenishmentInteraction;
            _itemDistributionSpot.InteractionEnded -= StopReplenishmentInteraction;
            _itemDistributionSpot.BibleCanBeDistributed -= TryDistributeItem;
            Queue.ParishionerIsReadyToBeServed -= OnParishionerIsReady;
        }

        public override void Init(NimbusBox nimbusBox, int nimbusCountForParishioner)
        {
            NimbusBox = nimbusBox;
            NimbusCountForParishioner = nimbusCountForParishioner;
            _itemDistributionSpot.Init();
            _itemReplenishmentSpot.Init();
        }

        public override InteractiveSpot GetReplenishmentSpot()
        {
            return _itemReplenishmentSpot;
        }

        public override InteractiveSpot GetDistributionSpot()
        {
            return _itemDistributionSpot;
        }

        protected override void OnParishionerIsReady()
        {
            StartCoroutine(PrepareBibleDistribution());
        }

        private IEnumerator PrepareBibleDistribution()
        {
            while (_bibles.Count == 0)
                yield return null;

            _itemDistributionSpot.OnDistributionPrepared();
        }

        private void SpawnItems()
        {
            for (var i = 0; i < _maxItems; i++)
            {
                var bible = Instantiate(itemPrefab, _itemSpawnPoint.position + _distanceBetweenItems * i * Vector3.up,
                    _itemSpawnPoint.rotation,
                    _itemDistributionSpot.transform);
                _bibles.Add(bible);
                bible.transform.localScale = Vector3.zero;
            }

            _spawnEffect.Play();
            _itemDistributionSpot.Worker.DoRandomMagick();

            DOTween.Sequence().AppendInterval(2f).OnComplete(() =>
            {
                _spawnEffect.Stop();
                foreach (var bible in _bibles)
                    bible.transform.DOScale(Vector3.one, .25f);

                UpdateBiblesPositions();
            });
        }

        private void UpdateBiblesPositions()
        {
            DOTween.Sequence().AppendInterval(.5f).OnComplete(() => _landEffect.Play());
            for (var i = 0; i < _bibles.Count; i++)
                _bibles[i].transform.DOMove(_itemBottomPoint.position + _distanceBetweenItems * i * Vector3.up, .5f);
        }

        private void TryDistributeItem()
        {
            if (_bibles.Count == 0)
            {
                Debug.LogWarning("Nothing to distribute");
                return;
            }

            _itemDistributionSpot.Worker.Put();

            var index = _bibles.Count - 1;
            Queue.Parishioner.TakeBible(_bibles[index]);
            Queue.Parishioner.InteractionEnded += CreateNimbuses;
            _bibles.RemoveAt(index);
        }

        private void StartReplenishmentInteraction()
        {
            _itemReplenishmentSpot.StartInteracting(null);
        }

        private void StopReplenishmentInteraction()
        {
            _itemReplenishmentSpot.StopInteracting();
        }
    }
}