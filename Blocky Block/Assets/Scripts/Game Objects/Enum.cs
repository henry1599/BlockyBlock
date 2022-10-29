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
        GROUND = 0b000001, // * 0
        WATER = 0b000010, // * 1
        TREE = 0b010000, // * 2
        BOX = 0b001000,
        SPACE = 0b000011, // * 4
        TRAP = 0b011000, // * 5
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
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    public enum BlockMode {
        IDE, 
        PREVIEW,
        BLOCK_ON_BLOCK,
        DUMMY_BLOCK
    }
    public enum BlockType {
        MOVE_FORWARD,
        PICK_UP,
        PUT_DOWN,
        DO_UNTIL,
        TURN,
        JUMP,
        SKIP,
        PUSH,
        JUMP_GRAB_STH,
        SKIP_GRAB_STH,
        JUMP_GRAB_NTH,
        SKIP_GRAB_NTH,
        JUMP_IF_STH_FRONT,
        SKIP_IF_STH_FRONT
    }
    public enum LevelID {
        HOME,
        LEVEL
    }
    public enum LevelType {
        HOME,
        MANUAL,
        CUSTOM,
        EVENT
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
