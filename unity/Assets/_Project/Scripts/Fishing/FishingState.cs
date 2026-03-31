namespace Fisherman.Fishing
{
    public enum FishingState
    {
        Idle,           // Waiting at the fishing spot, ready to cast
        AimCast,        // Player is holding/charging cast power
        Casting,        // Rod animation playing, line flying out
        WaitingBite,    // Bobber in water, waiting for fish
        BiteWindow,     // Fish bit! Player must react in time
        Fighting,       // Tug of war with the fish
        CatchSuccess,   // Fish caught! Show result
        CatchFail       // Fish escaped (line break or unhook)
    }
}
