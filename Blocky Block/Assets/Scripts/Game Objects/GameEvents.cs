using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using BlockyBlock.Configurations;
using BlockyBlock.Core;
using BlockyBlock.UI;
using BlockyBlock.Managers;
using System;

namespace BlockyBlock.Events
{
    public class BEFormEvents
    {
        public static Action<FormType, Action> ON_ENABLED;
        public static Action ON_OPEN_VERIFICATION_FORM;
    }
    public class LevelSelectionEvents
    {
        public static Action<LevelItem> ON_HIGHLIGHT_ITEM;
        public static Action<bool> ON_ITEM_HOVER;
    }
    public class HomeEvents
    {
        public static Action<HomeState> ON_STAGE_CHANGED;
    }
    public class GameEvents
    {
        public static Action<bool> ON_LOADING;
        public static Action<LevelID> LOAD_LEVEL;
        public static Action<LevelData> SETUP_LEVEL;
        public static Action SETUP_GROUND;
        public static Action<int, Vector2> SETUP_CAMERA;
        public static Action<ControlButton, bool> ON_CONTROL_BUTTON_TOGGLE;
        public static Action<bool> ON_CONTROL_BUTTON_TOGGLE_ALL;
        public static Action ON_CLEAR_IDE;
        public static Action<bool> ON_TOGGLE_CONTROLLER_PANEL;
        public static Action<bool> ON_EXECUTING_BLOCK;
        public static Action ON_LEVEL_CHECK;
        public static Action ON_WIN;
        public static Action ON_LOSE;
        public static Action ON_SHAKE_CAMERA;
    }
    public class UnitEvents
    {
        public static Action<BlockFunctionMoveForward> ON_MOVE_FORWARD;
        public static Action<BlockFunctionTurnLeft> ON_TURN_LEFT;
        public static Action<BlockFunctionTurnRight> ON_TURN_RIGHT;
        public static Action<BlockFunctionJump> ON_JUMP;
        public static Action<BlockFunctionPickup> ON_PICK_UP;
        public static Action<BlockFunctionPutdown> ON_PUT_DOWN;
        public static Action<BlockFunctionPush> ON_PUSH;
        public static Action<BlockFunctionJumpIfGrabSth> ON_JUMP_IF_GRAB_STH;
        public static Action<BlockFunctionJumpIfGrabSth, bool> ON_JUMP_IF_GRAB_STH_VALIDATE;
        public static Action<BlockFunctionJumpIfSthFront> ON_JUMP_IF_STH_FRONT; 
        public static Action<BlockFunctionJumpIfSthFront, bool> ON_JUMP_IF_STH_FRONT_VALIDATE; 
        public static Action ON_COLLECT_STUFF;
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
        public static Action<int> ON_IDE_CONTENT_CHANGED;
        public static Action<float> ON_IDE_SCROLL_SNAP;
    }
    public class BlockEvents
    {
        public static Action<UIBlock, IDERunState> ON_HIGHLIGHT;
        public static Action ON_UPDATE_VERTICAL_LAYOUT;
        public static Action<bool> ON_UI_BLOCK_DRAG;
        public static Action<UIBlockOption, bool> ON_UI_BLOCK_OPTION_SELECTED;
        public static Action<UIBlock> BLOCK_IDE_UI;
        public static Action<UIBlock> UNBLOCK_IDE_UI;
        public static Action ON_DISABLE_UI_FUNCTION;
        public static Action ON_UNHOVER_ALL_GRID_ITEM;
    }
    public class ToolEvents
    {
        public static Action<CursorType> ON_CURSOR_CHANGED;
        public static Action<ZoomType> ON_ZOOM_BUTTON_CLICKED;
        public static Action ON_RESET_BUTTON_CLICKED;
    }
    public class ErrorEvents
    {
        public static Action<ErrorType> ON_ERROR;
        public static Action ON_ERROR_HANDLING;
    }
    public class GameConstants
    {
        public static readonly float GROUND_HEIGHT_LEVEL = 5;
        public static readonly string UIBLOCK_OUTSIDE_CONTAINER_TAG = "Block Container";
        public static readonly string IDE_CONTENT_TAG = "IDE Content";
        public static readonly string IDE_SCROLL_RECT_TAG = "IDE Scroll Rect";
        public static readonly string UI_BLOCK_TAG = "UI Block";
        public static readonly string UI_BLOCK_OPTION_TAG = "UI Block Option";
        public static readonly string UI_BLOCK_OPTION_TURN_TAG = "UI Block Option Turn";
        public static readonly string UI_BLOCK_OPTION_STH_FRONT_TAG = "UI Block Option Sth Front";
        public static readonly string UI_DUMMY_BLOCK_TAG = "UI Dummy Block";
        public static readonly string LEVEL_MANAGER_TAG = "Level Manager";
        public static readonly string SCENE_MANAGER_TAG = "Scene Manager";
        public static readonly string RESOURCE_LOADER_TAG = "Resource Loader";
        public static readonly string CONFIG_MANAGER_TAG = "Config Manager";
        public static readonly string CONNECTION_MANAGER_TAG = "Connection Manager";
        public static readonly string IDE_MAIN_FIELD_TAG = "IDE Main Field";
        public static readonly string GROUND_TAG = "Ground";
        public static readonly string WATER_TAG = "Water";
        public static readonly string TOP_IDE_TAG = "Top IDE";
        public static readonly string BELOW_IDE_TAG = "Below IDE";
        public static readonly string NOT_ANY_BLOCK_TAG = "Not Any Block";
        public static readonly string WALKABLE_TAG = "Walkable";
        public static readonly string UNWALKABLE_TAG = "Unwalkable";
        public static readonly string UIERROR_TAG = "UIError";
        public static readonly string LEVELS_RAW_DATA_PATH = $"Data/Levels/";
        public static readonly float TRANSITION_IN_DURATION = 1.5f;
        public static readonly float TRANSITION_OUT_DURATION = 1;
        public static readonly string CHAPTER_CHOSEN_KEY = "Ch4pt3R_Ch053n";
        public static readonly string PROFILE_KEY = "Pr0F1l3_K3y";
        public static readonly string LEVEL_TO_BACK_KEY = "L3v3L_T0_B4ck";
    }
}

namespace BlockyBlock.BackEnd
{
    public class BEConstants
    {
        public static readonly string DEFAULT_VALUE = "d3F4aU1";
        public static readonly string CONTENT_VALUE = "application/json";
        public static readonly string CONTENT_TYPE = "Content-Type";
    }
}
