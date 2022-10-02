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
    public enum ConditionDirection
    {
        //*        |  LEFT   |  MID   |  RIGHT
        //*====================================
        //* TOP    |  0101   |  1001  |  1101
        //* CENTER |  0110   |  1010  |  1110
        //* BOTTOM |  0111   |  1011  |  1111
        LEFT = 0b0100, //* => 01 00
        MID = 0b1000, //* => 10 00
        RIGHT = 0b1100, //* => 11 00
        TOP = 0b0001, //* => 00 01
        CENTER = 0b0010, //* => 00 10
        BOTTOM = 0b0011, //* => 00 11
    }
    public enum Operator
    {
        EQUAL = 0b0,
        NOT_EQUAL = 0b1
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
        IF_ELSE,
        END_IF
    }
    public enum LevelID {
        HOME,
        LEVEL_01,
        LEVEL_02,
        LEVEL_03,
        LEVEL_04,
        LEVEL_05,
        LEVEL_06,
        LEVEL_07,
        LEVEL_08,
        LEVEL_09,
        LEVEL_10,
        LEVEL_TEST_3D
    }
    public enum LevelType {
        HOME,
        MANUAL,
        CUSTOM,
        EVENT
    }
}
