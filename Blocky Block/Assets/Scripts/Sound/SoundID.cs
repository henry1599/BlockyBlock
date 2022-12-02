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
        LEVEL_THEME,
        WIN,
        DRAG_UI_BLOCK,
        DROP_UI_BLOCK,
        CLICK_UI_BLOCK,
        STAR_GAIN_VICTORY,
        STAR_COLLECTED,
        CHAR_FOOTSTEP_GRASS,
        CHAR_FOOTSTEP_BOX,
        ERROR_ALERT
    }
}