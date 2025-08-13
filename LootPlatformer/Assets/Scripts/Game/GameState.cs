using UnityEngine;

namespace CMIYC
{
    public enum GameState
    {
        Planning,   // pre-heist (optional)
        Heist,      // active run
        Escape,     // reaching exit after alarm
        CashOut,    // between runs / end
        Caught,     // lose state
        Paused
    }
}
