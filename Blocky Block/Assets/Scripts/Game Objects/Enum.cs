namespace BlockyBlock.Enums
{
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
        BLOCK_ON_BLOCK
    }
    public enum BlockType {
        MOVE_FORWARD,
        PICK_UP,
        PUT_DOWN,
        DO_UNTIL,
        TURN_LEFT,
        TURN_RIGHT
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
    }
    public enum LevelType {
        HOME,
        MANUAL,
        CUSTOM,
        EVENT
    }
}