using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using System;

namespace BlockyBlock.Events
{
    public class GameEvents
    {
        public static Action<LevelID> LOAD_LEVEL;
        public static Action<LevelData> SETUP_LEVEL;
        public static Action<ControlButton, bool> ON_CONTROL_BUTTON_TOGGLE;
        public static Action ON_CLEAR_IDE;
    }
    public class UnitEvents
    {
        public static Action ON_MOVE_FORWARD;
        public static Action ON_TURN_LEFT;
        public static Action ON_TURN_RIGHT;
        public static Action ON_PICK_UP;
        public static Action ON_PUT_DOWN;
    }
    public class GameConstants
    {
        public static readonly string UIBLOCK_OUTSIDE_CONTAINER_TAG = "Block Container";
        public static readonly string IDE_CONTENT_TAG = "IDE Content";
        public static readonly string UI_BLOCK_TAG = "UI Block";
        public static readonly string LEVEL_MANAGER_TAG = "Level Manager";
        public static readonly string SCENE_MANAGER_TAG = "Scene Manager";
    }
}
