namespace Interaction
{
    public class InteractablePill : InteractableBase
    {
        public PlayerInfo _info;

        public override void Interact()
        {
            _info.AddPill();
            Destroy(gameObject);
        }
    }
}
