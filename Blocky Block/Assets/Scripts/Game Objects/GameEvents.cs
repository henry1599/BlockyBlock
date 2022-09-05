using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using BlockyBlock.Core;
using BlockyBlock.UI;
using System;

namespace BlockyBlock.Events
{
    public class GameEvents
    {
        public static Action<LevelID> LOAD_LEVEL;
        public static Action<LevelData> SETUP_LEVEL;
        public static Action<ControlButton, bool> ON_CONTROL_BUTTON_TOGGLE;
        public static Action<bool> ON_CONTROL_BUTTON_TOGGLE_ALL;
        public static Action ON_CLEAR_IDE;
    }
    public class UnitEvents
    {
        public static Action<BlockFunctionMoveForward> ON_MOVE_FORWARD;
        public static Action<BlockFunctionTurnLeft> ON_TURN_LEFT;
        public static Action<BlockFunctionTurnRight> ON_TURN_RIGHT;
        public static Action<BlockFunctionJump> ON_JUMP;
        public static Action ON_PICK_UP;
        public static Action ON_PUT_DOWN;
        public static Action ON_STOP;
        public static Action ON_RESET;
    }
    public class EditorEvents
    {
        public static Action ON_PLAY;
        public static Action ON_PREVIEW_STATUS_TOGGLE;
        public static Action<bool> ON_FORCE_PREVIEW_STATUS_TOGGLE;
        public static Action ON_UPDATE_LINE_NUMBER;
        public static Action<ScrollIDEState> ON_IDE_SCROLL;
        public static Action<bool> ON_BLOCK_EDITOR;
    }
    public class BlockEvents
    {
        public static Action<UIBlock, IDERunState> ON_HIGHLIGHT;
        public static Action ON_UPDATE_VERTICAL_LAYOUT;
    }
    public class GameConstants
    {
        public static readonly string UIBLOCK_OUTSIDE_CONTAINER_TAG = "Block Container";
        public static readonly string IDE_CONTENT_TAG = "IDE Content";
        public static readonly string UI_BLOCK_TAG = "UI Block";
        public static readonly string UI_DUMMY_BLOCK_TAG = "UI Dummy Block";
        public static readonly string LEVEL_MANAGER_TAG = "Level Manager";
        public static readonly string SCENE_MANAGER_TAG = "Scene Manager";
        public static readonly string GROUND_TAG = "Ground";
        public static readonly string WATER_TAG = "Water";
        public static readonly string TOP_IDE_TAG = "Top IDE";
        public static readonly string BELOW_IDE_TAG = "Below IDE";
    }
}
