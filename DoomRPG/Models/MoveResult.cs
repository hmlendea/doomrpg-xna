namespace DoomRPG.Models
{
    public sealed class MoveResult
    {
        public string PickedUpObjectName { get; set; }

        public int HealAmountReceived { get; set; }

        public string PickedUpWeaponId { get; set; }

        public string PickedUpKeyId { get; set; }

        public string PickedUpAmmoId { get; set; }

        public int PickedUpAmmoAmount { get; set; }
    }
}
