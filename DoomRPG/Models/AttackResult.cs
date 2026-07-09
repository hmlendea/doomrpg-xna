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
        NoTarget,
        WorldObjectHit,
        WorldObjectDestroyed
    }

    public sealed class AttackResult
    {
        public AttackOutcome Outcome { get; init; }

        public int Damage { get; init; }

        public string MobName { get; init; }

        public int RemainingAmmunition { get; init; }

        public string WorldObjectName { get; init; }

        public int ExplosionDamageDealtToPlayer { get; init; }
    }
}
