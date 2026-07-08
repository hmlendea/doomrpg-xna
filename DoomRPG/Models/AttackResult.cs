namespace DoomRPG.Models
{
    public enum AttackOutcome
    {
        NoAmmo,
        Missed,
        Hit,
        Crit,
        Kill,
        CritKill,
        NoTarget
    }

    public sealed class AttackResult
    {
        public AttackOutcome Outcome { get; init; }

        public int Damage { get; init; }

        public string MobName { get; init; }

        public int AmmoRemaining { get; init; }
    }
}
