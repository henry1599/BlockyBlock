namespace BlockyBlock.Enums
{
    public enum WinType {
        COLLECT_ALL_STUFF = 1000,
        COLLECT_THE_CHEST = 1001,
        REACH_TO_POSITION = 1002
    }
    public enum ErrorType {
        INVALID_MOVE,
        INVALID_PUT_DOWN_PLACE,
        INVALID_PUT_DOWN_NOTHING,
        INVALID_PICK_UP,
        INVALID_PUSH
    }
    public enum TurnDirection {
        LEFT = 0,
        RIGHT = 1
    }
    public enum CursorType {
        SELECTION = 0,
        MOVE = 1,
        ROTATE = 2,
        DRAGGING = 3
    }
    public enum ZoomType {
        ZOOM_IN = 0,
        ZOOM_OUT = 1
    }
    public enum IDERunState {
        MANNUAL,
        DEBUGGING,
        STOP
    }
    public enum ScrollIDEState {
        SCROLL_UP,
        SCROLL_DOWN,
        STOP_SCROLLING
    }
    public enum GroundType {
        GROUND =      0b000001, // * 0
        WATER =       0b000010, // * 1
        TREE =        0b010000, // * 2
        BOX =         0b001000,
        SPACE =       0b000011, // * 4
        TRAP =        0b011000, // * 5
        COLLECTIBLE = 0b100000, // * 6
        BOX_ON_GROUND = BOX | GROUND, // * 3
        BOX_IN_WATER = BOX | WATER // * 7
    }
    public enum ControlButton {
        STOP = 0,
        PLAY = 1,
        DEBUG = 2
    }
    public enum UnitDirection {
        UP = 0,
        DOWN = 1,
        LEFT = 2,
        RIGHT = 3
    }
    public enum BlockMode {
        IDE, 
        PREVIEW,
        BLOCK_ON_BLOCK,
        DUMMY_BLOCK
    }
    public enum BlockType {
        MOVE_FORWARD = 0,
        PICK_UP = 1,
        PUT_DOWN = 2,
        DO_UNTIL = 3,
        TURN = 4,
        JUMP = 5,
        SKIP = 6,
        PUSH = 7,
        JUMP_GRAB_STH = 8,
        SKIP_GRAB_STH = 9,
        JUMP_GRAB_NTH = 10,
        SKIP_GRAB_NTH = 11,
        JUMP_IF_STH_FRONT = 12,
        SKIP_IF_STH_FRONT = 13
    }
    public enum ChapterID
    {
        CHAPTER_01 = 0,
        CHAPTER_02 = 1,
        CHAPTER_03 = 2
    }
    public enum LevelStatus
    {
        UNLOCK = 0,
        LOCK = 1,
        WIN = 2
    }
    public enum LevelID {
        HOME = 0,
        LEVEL_SELECTION = 10,
        LEVEL_MANNUAL_00 = 1000,
        LEVEL_MANNUAL_01 = 1001,
        LEVEL_MANNUAL_02 = 1002,
        LEVEL_MANNUAL_03 = 1003,
        LEVEL_MANNUAL_04 = 1004,
        LEVEL_MANNUAL_05 = 1005,
        LEVEL_MANNUAL_06 = 1006,
        LEVEL_MANNUAL_07 = 1007,
        LEVEL_MANNUAL_08 = 1008,
        LEVEL_MANNUAL_09 = 1009,
        LEVEL_MANNUAL_10 = 1010,
        LEVEL_MANNUAL_11 = 1011,
        LEVEL_MANNUAL_12 = 1012,
    }
    public enum LevelType {
        HOME = 0,
        MANUAL = 1,
        CUSTOM = 2,
        EVENT = 3
    }
    public enum ConditionDirection
    {
        LEFT = 0b0100,
        MID = 0b1000,
        RIGHT = 0b1100,
        TOP = 0b0001,
        CENTER = 0b0010,
        BOTTOM = 0b0011,
        TOP_LEFT = TOP | LEFT,
        TOP_MID = TOP | MID,
        TOP_RIGHT = TOP | RIGHT,
        CENTER_LEFT = CENTER | LEFT,
        CENTER_MID = CENTER | MID,
        CENTER_RIGHT = CENTER | RIGHT,
        BOTTOM_LEFT = BOTTOM | LEFT,
        BOTTOM_MID = BOTTOM | MID,
        BOTTOM_RIGHT = BOTTOM | RIGHT
    }
}
