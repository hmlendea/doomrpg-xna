namespace DoomRPG.Models
{
    public sealed class MoveResult
    {
        public string PickedUpObjectName { get; set; }

        public int HealAmountReceived { get; set; }

        public string PickedUpWeaponId { get; set; }
    }
}
