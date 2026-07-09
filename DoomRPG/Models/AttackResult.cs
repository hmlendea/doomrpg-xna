namespace DoomRPG.Models
{
    public enum AttackOutcome
    {
        NoAmmo,
        Missed,
        Hit,
        Critical,
        Kill,
        CriticalKill,
        NoTarget
    }

    public sealed class AttackResult
    {
        public AttackOutcome Outcome { get; init; }

        public int Damage { get; init; }

        public string MobName { get; init; }

        public int RemainingAmmunition { get; init; }
    }
}
