namespace Interaction
{
    public class InteractableBattery : InteractableBase
    {
        public PlayerInfo _info;

        public override void Interact()
        {
            _info.AddBattery();
            Destroy(gameObject);
        }
    }
}
