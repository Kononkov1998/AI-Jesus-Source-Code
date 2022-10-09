using _Project.Scripts.Nimbuses;
using _Project.Scripts.Parishioners;
using UnityEngine;

namespace _Project.Scripts
{
    public abstract class BaseConstruction : MonoBehaviour
    {
        [SerializeField] protected Queue.Queue Queue;
        protected NimbusBox NimbusBox;
        protected int NimbusCountForParishioner;

        public abstract void Init(NimbusBox nimbusBox, int nimbusCountForParishioner);
        public abstract InteractiveSpot GetReplenishmentSpot();
        public abstract InteractiveSpot GetDistributionSpot();
        protected abstract void OnParishionerIsReady();

        protected void CreateNimbuses(Parishioner parishioner)
        {
            parishioner.InteractionEnded -= CreateNimbuses;
            for (var i = 0; i < NimbusCountForParishioner; i++)
            {
                var nimbus = NimbusCreator.Instance.SpawnNimbus(parishioner.transform.position);
                NimbusBox.AddItem(nimbus);
            }
        }
    }
}