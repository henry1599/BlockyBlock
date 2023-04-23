using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockyBlock.Tracking
{
    public enum LevelEntry
    {
        Level_selection = 0,
        Prev_level,
        This_level,
        None
    }
    public enum EndCause
    {
        Back_button = 0,
        Pause_game,
        Replay_button,
        Next_button,
        Level_selection_button,
        Quit_button,
    }
    public static class TrackingEnum
    {
        public static string LevelEntryToString(this LevelEntry levelEntry)
        {
            switch (levelEntry)
            {
                case LevelEntry.Level_selection:
                default:
                    return "level selection";
                case LevelEntry.Prev_level:
                    return "previous level";
                case LevelEntry.This_level:
                    return "this level";
                case LevelEntry.None:
                    return "Unknown";
            }
        }
        public static string EndCauseToString(this EndCause endCause)
        {
            switch (endCause)
            {
                case EndCause.Back_button:
                    return "back button";
                case EndCause.Pause_game:
                    return "pause game";
                case EndCause.Replay_button:
                    return "replay button";
                case EndCause.Next_button:
                    return "next button";
                case EndCause.Level_selection_button:
                    return "level selection button";
                case EndCause.Quit_button:
                    return "quit button";
                default:
                    return "none";
            }
        }
    }
}
