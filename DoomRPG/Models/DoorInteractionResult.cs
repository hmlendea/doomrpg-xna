namespace DoomRPG.Models
{
    public sealed class DoorInteractionResult
    {
        public static DoorInteractionResult NoDoor { get; } = new() { WasDoorFound = false };

        public bool WasDoorFound { get; init; } = true;

        public string ErrorMessage { get; init; }

        public string DestinationLevelId { get; init; }
    }
}
