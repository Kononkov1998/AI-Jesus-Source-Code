namespace _Project.Scripts.BibleSpot
{
    public class ItemReplenishmentSpot : InteractiveSpot
    {
        public override void StartInteracting(CharacterAnimator character)
        {
            if (!gameObject.activeInHierarchy) return;
            if (!IsAutoWorking())
                base.StartInteracting(character);
        }
    }
}