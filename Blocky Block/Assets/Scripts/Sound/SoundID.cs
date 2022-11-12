using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AudioPlayer
{
    [Serializable]
    public enum SoundID
    {
        NONE,
        HOME_BG_MUSIC,
        BUTTON_CLICK,
        BUTTON_HOVER,
        LEVEL_SELECTION_BG_MUSIC,
        TRANSITION_IN,
        TRANSITION_OUT,
        CHARACTER_MOVE,
        BOX_TO_WATER,
        BOX_TO_GROUND,
        PUSH,
        CONFETTI,
        BOX_PICK_UP,
        LEVEL_THEME
    }
}